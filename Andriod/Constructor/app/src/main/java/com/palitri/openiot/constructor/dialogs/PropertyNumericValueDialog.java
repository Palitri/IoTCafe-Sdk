package com.palitri.openiot.constructor.dialogs;

import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.Button;
import android.widget.SeekBar;
import android.widget.TextView;

import com.palitri.openiot.construction.framework.board.models.BoardProperty;
import com.palitri.openiot.construction.framework.board.models.BoardPropertyType;
import com.palitri.openiot.constructor.R;

public abstract class PropertyNumericValueDialog extends Dialog {

    private float min, max, step;
    private boolean instantUpdate;

    private float originalValue;
    private boolean changed;

    private SeekBar sbValue;

    public PropertyNumericValueDialog(Context context, String name, BoardProperty p, float min, float max, float step, boolean instantUpdate) {
        super(context);

        this.Init(context, null, name, p.type == BoardPropertyType.Float ? (float)p.value : (int)p.value, min, max, step, instantUpdate);
    }

    public PropertyNumericValueDialog(Context context, String name, float value, float min, float max, float step, boolean instantUpdate) {
        super(context);

        this.Init(context, null, name, value, min, max, step, instantUpdate);
    }

    public PropertyNumericValueDialog(Context context, String title, String name, float value, float min, float max, float step, boolean instantUpdate) {
        super(context);

        this.Init(context, title, name, value, min, max, step, instantUpdate);
    }

    private void Init(Context context, String title, String name, float value, float min, float max, float step, boolean instantUpdate)
    {
        this.min = min;
        this.max = max;
        this.step = step;
        this.instantUpdate = instantUpdate;

        this.changed = false;
        this.originalValue = value;

        if (title != null) {
            //this.setTitle(title);
            this.requestWindowFeature(Window.FEATURE_NO_TITLE);
            this.getWindow().setBackgroundDrawable(new ColorDrawable(context.getResources().getColor(R.color.design_default_color_background)));
        }
        else {
            this.requestWindowFeature(Window.FEATURE_NO_TITLE);
            this.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        }

        this.setContentView(R.layout.dialog_analog_input);
        this.getWindow().setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        this.show();

        final PropertyNumericValueDialog self = this;

        final TextView tvName = this.findViewById(R.id.tvName);
        tvName.setText(name);

        final TextView tvValue = this.findViewById(R.id.tvValue);
        tvValue.setText(getValueText(value));

        this.sbValue = this.findViewById(R.id.sbValue);
        final SeekBar sbValue = this.sbValue;
        sbValue.setMax((int)((this.max - this.min) / this.step));
        sbValue.setProgress((int) ((-this.min + value) / this.step));
        sbValue.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int i, boolean b) {
                tvValue.setText(getValueText(self.getValue()));

                self.changed = true;

                if (self.instantUpdate)
                    self.setValue(self.getValue());
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {

            }

            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {

            }
        });

        final Button bDec = this.findViewById(R.id.bDec);
        bDec.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                sbValue.setProgress((sbValue.getProgress() - 1));
            }
        });

        final Button bInc = this.findViewById(R.id.bInc);
        bInc.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                sbValue.setProgress((sbValue.getProgress() + 1));
            }
        });

        final Button bOk = this.findViewById(R.id.bOK);
        bOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (self.changed && !self.instantUpdate)
                    self.setValue(self.getValue());

                self.dismiss();
            }
        });

        final Button bCancel = this.findViewById(R.id.bCancel);
        bCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (self.changed && self.instantUpdate)
                    self.setValue(self.originalValue);

                self.dismiss();
            }
        });
    }

    public float getValue()
    {
        return this.min + ((float)this.sbValue.getProgress() * this.step);
    }

    public abstract String getValueText(float value);

    public abstract void setValue(float value);
}
