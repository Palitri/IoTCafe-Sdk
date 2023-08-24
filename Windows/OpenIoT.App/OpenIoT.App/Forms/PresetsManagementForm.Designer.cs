namespace OpenIoT.App.Forms
{
    partial class PresetsManagementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PresetsManagementForm));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnNewPreset = new System.Windows.Forms.Button();
            this.presetsList = new OpenIoT.App.Controls.PresetsListControl();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(412, 471);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 46);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = global::OpenIoT.App.Resources.Ok;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnNewPreset
            // 
            this.btnNewPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewPreset.Image = ((System.Drawing.Image)(resources.GetObject("btnNewPreset.Image")));
            this.btnNewPreset.Location = new System.Drawing.Point(516, 12);
            this.btnNewPreset.Name = "btnNewPreset";
            this.btnNewPreset.Size = new System.Drawing.Size(46, 46);
            this.btnNewPreset.TabIndex = 6;
            this.btnNewPreset.UseVisualStyleBackColor = true;
            this.btnNewPreset.Click += new System.EventHandler(this.btnNewPreset_Click);
            // 
            // presetsList
            // 
            this.presetsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.presetsList.Location = new System.Drawing.Point(12, 64);
            this.presetsList.Name = "presetsList";
            this.presetsList.Size = new System.Drawing.Size(550, 363);
            this.presetsList.TabIndex = 7;
            this.presetsList.PresetClicked += new OpenIoT.App.Controls.EventHandlers.PresetEventHandler(this.presetsList_PresetClicked);
            this.presetsList.PresetUpdate += new OpenIoT.App.Controls.EventHandlers.PresetEventHandler(this.presetsList_PresetUpdate);
            this.presetsList.PresetRename += new OpenIoT.App.Controls.EventHandlers.PresetEventHandler(this.presetsList_PresetRename);
            this.presetsList.PresetDelete += new OpenIoT.App.Controls.EventHandlers.PresetEventHandler(this.presetsList_PresetDelete);
            // 
            // PresetsManagementForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 529);
            this.Controls.Add(this.presetsList);
            this.Controls.Add(this.btnNewPreset);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PresetsManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Presets";
            this.Load += new System.EventHandler(this.PresetsManagementForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnOk;
        private Button btnNewPreset;
        private Controls.PresetsListControl presetsList;
    }
}