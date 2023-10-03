package com.palitri.openiot.construction.framework.web.networking;

import android.os.AsyncTask;

import androidx.annotation.NonNull;

import com.palitri.openiot.construction.framework.tools.utils.StringUtils;

import java.io.BufferedOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.Scanner;

public class WebRequest extends AsyncTask<Object,Void,String> {

    private String requestUrl;
    private String bearerToken;
    private String requestBody;
    private String httpMethod;

    public WebRequest()
    {
        super();

        this.httpMethod = "GET";
    }

    private String sendRequest()
    {
        HttpURLConnection urlConnection = null;

        try
        {
            URL url = new URL(this.requestUrl);

            urlConnection = (HttpURLConnection)url.openConnection();
            urlConnection.setDoOutput(!StringUtils.IsNullOrEmpty(this.requestBody));
            urlConnection.setRequestMethod(this.httpMethod);
            urlConnection.setRequestProperty("Connection", "close");
            if (!StringUtils.IsNullOrEmpty(this.bearerToken))
                urlConnection.setRequestProperty("Authorization", " Bearer " + this.bearerToken);

            if (!StringUtils.IsNullOrEmpty(this.requestBody)) {
                urlConnection.setFixedLengthStreamingMode(this.requestBody.length());
                //urlConnection.setChunkedStreamingMode(0);

                BufferedOutputStream bodyStream = new BufferedOutputStream(urlConnection.getOutputStream());
                bodyStream.write(this.requestBody.getBytes(Charset.forName("UTF-8")));
                bodyStream.flush();
                bodyStream.close();

            }

            InputStream in = urlConnection.getInputStream();
            InputStreamReader reader = new InputStreamReader(in);

            Scanner s = new Scanner(reader).useDelimiter("\\A");
            String result = s.hasNext() ? s.next() : "";

            return result;
        }
        catch (Exception e)
        {
            return null;
        }
        finally
        {
            if (urlConnection != null)
                urlConnection.disconnect();
        }
    }

    @Override
    protected String doInBackground(@NonNull Object... params)
    {
        this.requestUrl = (String)params[0];

        String response = this.sendRequest();

        this.onResponse(response, params);

        return response;
    }

    public WebRequest setToken(String token)
    {
        this.bearerToken = token;
        return this;
    }

    public WebRequest setRequestBody(String body)
    {
        this.requestBody = body;
        return this;
    }

    public WebRequest setHttpMethod(String httpMethod)
    {
        this.httpMethod = httpMethod;
        return this;
    }

    public void send(Object... params)
    {
        this.execute(params);
    }

    public void onResponse(String response, Object... params){
    }

    //After download task
    @Override
    protected void onPostExecute(String result) {
        super.onPostExecute(result);
        try {
        }
        catch (Exception e) {
            e.printStackTrace();
        }

    }
}
