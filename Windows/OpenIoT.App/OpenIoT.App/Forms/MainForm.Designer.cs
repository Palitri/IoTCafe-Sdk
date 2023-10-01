namespace OpenIoT.App.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presetsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.boardNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boardInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.presetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesListControl = new OpenIoT.App.Controls.PropertiesListControl();
            this.menuMain.SuspendLayout();
            this.contextMenuTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.presetToolStripMenuItem,
            this.comToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(800, 40);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuMain";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.projectsToolStripMenuItem,
            this.presetsToolStripMenuItem1,
            this.boardNameToolStripMenuItem,
            this.boardInfoToolStripMenuItem});
            this.optionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("optionsToolStripMenuItem.Image")));
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(52, 36);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("loginToolStripMenuItem.Image")));
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(296, 46);
            this.loginToolStripMenuItem.Text = global::OpenIoT.App.Resources.Login;
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // projectsToolStripMenuItem
            // 
            this.projectsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("projectsToolStripMenuItem.Image")));
            this.projectsToolStripMenuItem.Name = "projectsToolStripMenuItem";
            this.projectsToolStripMenuItem.Size = new System.Drawing.Size(296, 46);
            this.projectsToolStripMenuItem.Text = global::OpenIoT.App.Resources.Projects;
            this.projectsToolStripMenuItem.Click += new System.EventHandler(this.projectsToolStripMenuItem_Click);
            // 
            // presetsToolStripMenuItem1
            // 
            this.presetsToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("presetsToolStripMenuItem1.Image")));
            this.presetsToolStripMenuItem1.Name = "presetsToolStripMenuItem1";
            this.presetsToolStripMenuItem1.Size = new System.Drawing.Size(296, 46);
            this.presetsToolStripMenuItem1.Text = global::OpenIoT.App.Resources.Presets;
            this.presetsToolStripMenuItem1.Click += new System.EventHandler(this.presetsToolStripMenuItem1_Click);
            // 
            // boardNameToolStripMenuItem
            // 
            this.boardNameToolStripMenuItem.Image = global::OpenIoT.App.Resources.icon_edit;
            this.boardNameToolStripMenuItem.Name = "boardNameToolStripMenuItem";
            this.boardNameToolStripMenuItem.Size = new System.Drawing.Size(296, 46);
            this.boardNameToolStripMenuItem.Text = "Board name";
            this.boardNameToolStripMenuItem.Click += new System.EventHandler(this.boardNameToolStripMenuItem_Click);
            // 
            // boardInfoToolStripMenuItem
            // 
            this.boardInfoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("boardInfoToolStripMenuItem.Image")));
            this.boardInfoToolStripMenuItem.Name = "boardInfoToolStripMenuItem";
            this.boardInfoToolStripMenuItem.Size = new System.Drawing.Size(296, 46);
            this.boardInfoToolStripMenuItem.Text = global::OpenIoT.App.Resources.BoardInfo;
            this.boardInfoToolStripMenuItem.Click += new System.EventHandler(this.boardInfoToolStripMenuItem_Click);
            // 
            // presetToolStripMenuItem
            // 
            this.presetToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("presetToolStripMenuItem.Image")));
            this.presetToolStripMenuItem.Name = "presetToolStripMenuItem";
            this.presetToolStripMenuItem.Size = new System.Drawing.Size(52, 36);
            // 
            // comToolStripMenuItem
            // 
            this.comToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.comToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("comToolStripMenuItem.Image")));
            this.comToolStripMenuItem.Name = "comToolStripMenuItem";
            this.comToolStripMenuItem.Size = new System.Drawing.Size(52, 36);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuTray;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "OpenIoT";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuTray
            // 
            this.contextMenuTray.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuTray.Name = "contextMenuTray";
            this.contextMenuTray.Size = new System.Drawing.Size(148, 80);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 38);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(147, 38);
            this.exitToolStripMenuItem.Text = "Close";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // propertiesListControl
            // 
            this.propertiesListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertiesListControl.Location = new System.Drawing.Point(0, 40);
            this.propertiesListControl.Name = "propertiesListControl";
            this.propertiesListControl.Size = new System.Drawing.Size(800, 410);
            this.propertiesListControl.TabIndex = 1;
            this.propertiesListControl.PropertyClicked += new OpenIoT.App.Controls.EventHandlers.PropertyEventHandler(this.propertiesListControl_PropertyClicked);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.propertiesListControl);
            this.Controls.Add(this.menuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenIoT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.contextMenuTray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuMain;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem boardInfoToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem projectsToolStripMenuItem;
        private ToolStripMenuItem presetsToolStripMenuItem1;
        private ToolStripMenuItem presetToolStripMenuItem;
        private ToolStripMenuItem comToolStripMenuItem;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuTray;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem boardNameToolStripMenuItem;   
        private Controls.PropertiesListControl propertiesListControl;
    }
}