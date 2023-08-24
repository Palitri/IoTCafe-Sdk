package com.palitri.openiot.constructor.dialogs;

import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import com.palitri.openiot.constructor.R;

public class StringInputDialog extends Dialog {

    public StringInputDialog(Context context, String value, String title)
    {
        super(context);

        if (title != null) {
            this.requestWindowFeature(Window.FEATURE_NO_TITLE);
            this.getWindow().setBackgroundDrawable(new ColorDrawable(context.getResources().getColor(R.color.design_default_color_background)));
        }
        else {
            this.requestWindowFeature(Window.FEATURE_NO_TITLE);
            this.getWindow().setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
        }

        this.setContentView(R.layout.dialog_string_input);
        this.getWindow().setLayout(ViewGroup.LayoutParams.MATCH_PARENT, ViewGroup.LayoutParams.WRAP_CONTENT);
        this.show();

        final TextView tvName = this.findViewById(R.id.tvName);
        tvName.setText(title);

        final EditText editText = this.findViewById(R.id.editText);
        editText.setText(value);
        editText.requestFocus();
        editText.selectAll();

        this.ShowKeyboard(editText);

        final Button bOk = this.findViewById(R.id.bOK);
        bOk.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                setValue(editText.getText().toString());

                HideKeyboard(editText);

                dismiss();
            }
        });

        final Button bCancel = this.findViewById(R.id.bCancel);
        bCancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                HideKeyboard(editText);

                dismiss();
            }
        });
    }

    public void setValue(String value) {
        }

    private void ShowKeyboard(View view)
    {
        InputMethodManager imm = (InputMethodManager)getContext().getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.toggleSoftInput(InputMethodManager.SHOW_FORCED, 0);
       //imm.toggleSoftInputFromWindow(getCurrentFocus().getWindowToken(), InputMethodManager.SHOW_FORCED, 0);
    }

    private void HideKeyboard(EditText editText)
    {
        InputMethodManager imm = (InputMethodManager)getContext().getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(getCurrentFocus().getWindowToken(), 0);
    }

}
