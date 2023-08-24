package com.palitri.openiot.constructor.arrayadapters;

import android.content.Context;
import android.view.View;
import android.widget.RadioButton;
import android.widget.Switch;
import android.widget.TextView;

import com.palitri.openiot.construction.framework.board.api.OpenIotBoard;
import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.board.models.BoardPropertyType;
import com.palitri.openiot.construction.framework.tools.utils.StringUtils;
import com.palitri.openiot.construction.framework.composite.CompositeProperty;
import com.palitri.openiot.constructor.R;
import com.palitri.openiot.constructor.activities.ActivityBase;
import com.palitri.openiot.constructor.dialogs.PropertyNumericValueDialog;

import java.util.ArrayList;

public class PropertiesArrayAdapter extends ArrayAdapterBase<PropertiesArrayAdapter.ViewContainer, CompositeProperty> {

    static class ViewContainer
    {
        TextView tvName;
        Switch sBool;
        RadioButton sBoolReadOnly;
        TextView tvValueEditable;
        TextView tvValueReadOnly;
        TextView tvValueResettable;
        TextView tvValueDefault;
        TextView tvValueWarning;

        TextView tvValue;
    }

    public PropertiesArrayAdapter(Context context, ArrayList<CompositeProperty> items) {
        super(context, R.layout.array_adapter_property, items);
    }

    @Override
    public ViewContainer GetView(View convertView, CompositeProperty compositeProperty) {
        ViewContainer view = new ViewContainer();
        view.tvName = (TextView) convertView.findViewById(R.id.tvName);
        view.sBool = (Switch) convertView.findViewById(R.id.sBool);
        view.sBoolReadOnly = (RadioButton) convertView.findViewById(R.id.sBoolReadOnly);
        view.tvValueEditable = (TextView) convertView.findViewById(R.id.tvValueEditable);
        view.tvValueReadOnly = (TextView) convertView.findViewById(R.id.tvValueReadOnly);
        view.tvValueResettable = (TextView) convertView.findViewById(R.id.tvValueResettable);
        view.tvValueDefault  = (TextView) convertView.findViewById(R.id.tvValueDefault);
        view.tvValueWarning  = (TextView) convertView.findViewById(R.id.tvValueWarning);

        view.sBool.setVisibility(View.GONE);
        view.sBoolReadOnly.setVisibility(View.GONE);
        view.tvValueEditable.setVisibility(View.GONE);
        view.tvValueReadOnly.setVisibility(View.GONE);
        view.tvValueResettable.setVisibility(View.GONE);
        view.tvValueDefault.setVisibility(View.GONE);
        view.tvValueWarning.setVisibility(View.GONE);

        switch (compositeProperty.boardProperty.type)
        {
            case Bool: {
                if (compositeProperty.boardProperty.isWriteable())
                    view.sBool.setVisibility(View.VISIBLE);
                else
                    view.sBoolReadOnly.setVisibility(View.VISIBLE);

                break;
            }

            case Integer:
            case Float:
            case Data:
            {
                view.tvValue = getValueTextView(compositeProperty.boardProperty, view);
                view.tvValue.setVisibility(View.VISIBLE);

                break;
            }
        }

        return view;
    }

    @Override
    public void SetView(ViewContainer view, CompositeProperty compositeProperty) {
        view.tvName.setText(compositeProperty.peripheralProperty.name);

        switch (compositeProperty.boardProperty.type)
        {
            case Bool: {
                if (compositeProperty.boardProperty.isWriteable())
                    view.sBool.setChecked((boolean) compositeProperty.boardProperty.value);
                else
                    view.sBoolReadOnly.setChecked((boolean) compositeProperty.boardProperty.value);

                break;
            }

            case Integer:
            case Float:
            case Data:
            {
                view.tvValue.setText(getItemValueString(compositeProperty));

                break;
            }
        }
    }

    @Override
    public void OnItemClick(final CompositeProperty propertiesArrayAdapterItem) {
        switch (propertiesArrayAdapterItem.boardProperty.type)
        {
            case Bool: {
                if (propertiesArrayAdapterItem.boardProperty.isWriteable()) {
                    boolean newValue = !((boolean)propertiesArrayAdapterItem.boardProperty.value);
                    propertiesArrayAdapterItem.boardProperty.value = newValue;
                    ((ActivityBase)getContext()).board.boardDevice.RequestPropertyUpdate(propertiesArrayAdapterItem.boardProperty);
                }

                break;
            }

            case Integer:
            case Float:
            {
                if (propertiesArrayAdapterItem.boardProperty.isWriteable()) {
                    final OpenIotBoard board = ((ActivityBase)getContext()).board.boardDevice;
                    new PropertyNumericValueDialog(getContext(), propertiesArrayAdapterItem.peripheralProperty.name, propertiesArrayAdapterItem.boardProperty, propertiesArrayAdapterItem.peripheralProperty.min, propertiesArrayAdapterItem.peripheralProperty.max, propertiesArrayAdapterItem.peripheralProperty.step, propertiesArrayAdapterItem.peripheralProperty.instantUpdate) {
                        @Override
                        public String getValueText(float value) {
                            if (propertiesArrayAdapterItem.boardProperty.type == BoardPropertyType.Integer)
                                return getItemValueString(propertiesArrayAdapterItem, (int)value); // because float Object can't be cast directly to int, but has to be cast like (int)(float)the_float_object
                            else
                                return getItemValueString(propertiesArrayAdapterItem, value);
                        }

                        @Override
                        public void setValue(float value) {
                            if (propertiesArrayAdapterItem.boardProperty.type == BoardPropertyType.Integer)
                                board.RequestPropertyUpdate(new BoardProperty(propertiesArrayAdapterItem.boardProperty, (int) value));
                            else
                                board.RequestPropertyUpdate(new BoardProperty(propertiesArrayAdapterItem.boardProperty, value));
                        }
                    };
                }

                break;
            }
        }
    }

    private TextView getValueTextView(BoardProperty p, ViewContainer vHolder)
    {
        return p.isWriteable() ? vHolder.tvValueEditable : vHolder.tvValueReadOnly;
    }

    private String getItemValueString(CompositeProperty item, Object value)
    {
        if (!StringUtils.IsNullOrEmpty(item.peripheralProperty.displayFormat)) {

            switch (item.peripheralProperty.type) {
                case Integer:
                    String format = item.peripheralProperty.displayFormat.replace("%dt", timeToString((int)value));
                    return String.format(format, (int)value);
                case Float:
                    return String.format(item.peripheralProperty.displayFormat, (float)value);
                case Bool:
                    return String.format(item.peripheralProperty.displayFormat, (float)value != 0.0f);
                case Data:
                    return String.format(item.peripheralProperty.displayFormat, item.boardProperty.getString());
            }
        }

        switch (item.boardProperty.type) {
            case Integer:
                return String.valueOf(item.boardProperty.getInt());
            case Float:
                return String.valueOf(item.boardProperty.getFloat());
            case Bool:
                return String.valueOf(item.boardProperty.getBool() ? R.string.unit_boolean_true : R.string.unit_boolean_false);
            case Data:
                return item.boardProperty.getString();
        }

        return "--";
    }

    private String getItemValueString(CompositeProperty item)
    {
        return this.getItemValueString(item, item.boardProperty.value);
    }


    private String timeToString(int time)
    {
        String text = "";

        int value = time % 60;
        time /= 60;
        boolean nonZero = false;;
        if (value != 0) {
            text = String.valueOf(value) + getContext().getResources().getString(R.string.unit_seconds_short);
            nonZero = true;
        }

        value = time % 60;
        time /= 60;
        if (value != 0) {
            if (nonZero)
                text = " " + text;
            text = String.valueOf(value) + getContext().getResources().getString(R.string.unit_minutes_short) + text;
            nonZero = true;
        }

        value = time % 24;
        time /= 24;
        if (value != 0) {
            if (nonZero)
                text = " " + text;
            text = String.valueOf(value) + getContext().getResources().getString(R.string.unit_hours_short) + text;
            nonZero = true;
        }

        value = time;
        if (value != 0) {
            if (nonZero)
                text = " " + text;
            text = String.valueOf(value) + getContext().getResources().getString(R.string.unit_days_short) + text;
        }

        if (!nonZero)
            text = "0" + getContext().getResources().getString(R.string.unit_seconds_short);

        return text;
    }
}
