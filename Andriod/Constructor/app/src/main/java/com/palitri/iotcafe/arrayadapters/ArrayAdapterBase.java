package com.palitri.iotcafe.arrayadapters;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import java.util.ArrayList;

public abstract class ArrayAdapterBase<TViewContainer, TArrayItem> extends ArrayAdapter {

    static class ViewHolder
    {
        Object view;
        Object item;
    }

    private Context mContext;
    private int mResource;

    public abstract TViewContainer GetView(View convertView, TArrayItem item);
    public abstract void SetView(TViewContainer view, TArrayItem item);
    public abstract void OnItemClick(TArrayItem item);

    public ArrayAdapterBase(Context context, int resource, ArrayList<TArrayItem> objects) {
        super(context, resource, objects);

        this.mContext = context;
        this.mResource = resource;
    }


    @NonNull
    @Override
    public View getView(int position, @Nullable View convertView, @NonNull ViewGroup parent) {
        try {
            final ArrayAdapterBase.ViewHolder viewHolder;

            final Object item = getItem(position);

            if (convertView == null) {
                LayoutInflater layoutInflater = LayoutInflater.from(mContext);
                convertView = layoutInflater.inflate(mResource, parent, false);

                viewHolder = new ArrayAdapterBase.ViewHolder();
                viewHolder.view = this.GetView(convertView, (TArrayItem)item);

                convertView.setTag(viewHolder);
            }
            else
            {
                viewHolder = (ArrayAdapterBase.ViewHolder)convertView.getTag();
            }

            this.SetView((TViewContainer)viewHolder.view, (TArrayItem)item);

            if (viewHolder.item == item)
                return convertView;

            viewHolder.item = item;

            final Activity activity = (Activity)this.mContext;
            final ArrayAdapterBase self = this;
            convertView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View view) {
                    self.OnItemClick(item);
                }
            });

            return convertView;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
