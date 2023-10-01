package com.palitri.iotcafe.arrayadapters;

import android.content.Context;
import android.view.View;
import android.widget.TextView;

import com.palitri.iotcafe.R;

import java.util.ArrayList;

public abstract class BasicItemArrayAdapter<TArrayItem> extends ArrayAdapterBase<BasicItemArrayAdapter.ViewContainer, TArrayItem> {

    static class ViewContainer
    {
        TextView tvDeviceName;
    }

    public abstract void SetView(TextView view, TArrayItem item);

    public BasicItemArrayAdapter(Context context, ArrayList<TArrayItem> objects)
    {
        super(context, R.layout.array_adapter_basic_item, objects);
    }

    @Override
    public void SetView(ViewContainer view, TArrayItem item) {
        this.SetView(view.tvDeviceName, (TArrayItem)item);
    }

    @Override
    public ViewContainer GetView(View convertView, TArrayItem item) {
        ViewContainer viewContainer = new ViewContainer();

        viewContainer.tvDeviceName = convertView.findViewById(R.id.tvName);

        return viewContainer;
    }
}
