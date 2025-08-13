namespace Palitri.OpenIoT.App.Forms
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            notifyIcon = new NotifyIcon(components);
            contextMenuTray = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            propertiesListControl = new Palitri.OpenIoT.App.Controls.PropertiesListControl();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            loginToolStripMenuItem = new ToolStripMenuItem();
            projectsToolStripMenuItem = new ToolStripMenuItem();
            presetsToolStripMenuItem1 = new ToolStripMenuItem();
            boardNameToolStripMenuItem = new ToolStripMenuItem();
            boardInfoToolStripMenuItem = new ToolStripMenuItem();
            presetToolStripMenuItem = new ToolStripMenuItem();
            comToolStripMenuItem = new ToolStripMenuItem();
            menuMain = new MenuStrip();
            contextMenuTray.SuspendLayout();
            menuMain.SuspendLayout();
            SuspendLayout();
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuTray;
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "OpenIoT";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            notifyIcon.MouseDoubleClick += notifyIcon_MouseDoubleClick;
            // 
            // contextMenuTray
            // 
            contextMenuTray.ImageScalingSize = new Size(32, 32);
            contextMenuTray.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, exitToolStripMenuItem });
            contextMenuTray.Name = "contextMenuTray";
            contextMenuTray.Size = new Size(148, 80);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(147, 38);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(147, 38);
            exitToolStripMenuItem.Text = "Close";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // propertiesListControl
            // 
            propertiesListControl.Dock = DockStyle.Fill;
            propertiesListControl.Location = new Point(0, 40);
            propertiesListControl.Name = "propertiesListControl";
            propertiesListControl.Size = new Size(800, 410);
            propertiesListControl.TabIndex = 1;
            propertiesListControl.PropertyClicked += propertiesListControl_PropertyClicked;
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem, projectsToolStripMenuItem, presetsToolStripMenuItem1, boardNameToolStripMenuItem, boardInfoToolStripMenuItem });
            optionsToolStripMenuItem.Image = (Image)resources.GetObject("optionsToolStripMenuItem.Image");
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(52, 36);
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Image = (Image)resources.GetObject("loginToolStripMenuItem.Image");
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(359, 46);
            loginToolStripMenuItem.Text = "Login";
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // projectsToolStripMenuItem
            // 
            projectsToolStripMenuItem.Image = (Image)resources.GetObject("projectsToolStripMenuItem.Image");
            projectsToolStripMenuItem.Name = "projectsToolStripMenuItem";
            projectsToolStripMenuItem.Size = new Size(359, 46);
            projectsToolStripMenuItem.Text = "Projects";
            projectsToolStripMenuItem.Click += projectsToolStripMenuItem_Click;
            // 
            // presetsToolStripMenuItem1
            // 
            presetsToolStripMenuItem1.Image = (Image)resources.GetObject("presetsToolStripMenuItem1.Image");
            presetsToolStripMenuItem1.Name = "presetsToolStripMenuItem1";
            presetsToolStripMenuItem1.Size = new Size(359, 46);
            presetsToolStripMenuItem1.Text = "Presets";
            presetsToolStripMenuItem1.Click += presetsToolStripMenuItem1_Click;
            // 
            // boardNameToolStripMenuItem
            // 
            boardNameToolStripMenuItem.Image = (Image)resources.GetObject("boardNameToolStripMenuItem.Image");
            boardNameToolStripMenuItem.Name = "boardNameToolStripMenuItem";
            boardNameToolStripMenuItem.Size = new Size(359, 46);
            boardNameToolStripMenuItem.Text = "Board name";
            boardNameToolStripMenuItem.Click += boardNameToolStripMenuItem_Click;
            // 
            // boardInfoToolStripMenuItem
            // 
            boardInfoToolStripMenuItem.Image = (Image)resources.GetObject("boardInfoToolStripMenuItem.Image");
            boardInfoToolStripMenuItem.Name = "boardInfoToolStripMenuItem";
            boardInfoToolStripMenuItem.Size = new Size(359, 46);
            boardInfoToolStripMenuItem.Text = "Board info";
            boardInfoToolStripMenuItem.Click += boardInfoToolStripMenuItem_Click;
            // 
            // presetToolStripMenuItem
            // 
            presetToolStripMenuItem.Image = (Image)resources.GetObject("presetToolStripMenuItem.Image");
            presetToolStripMenuItem.Name = "presetToolStripMenuItem";
            presetToolStripMenuItem.Size = new Size(52, 36);
            // 
            // comToolStripMenuItem
            // 
            comToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            comToolStripMenuItem.Image = (Image)resources.GetObject("comToolStripMenuItem.Image");
            comToolStripMenuItem.Name = "comToolStripMenuItem";
            comToolStripMenuItem.Size = new Size(52, 36);
            // 
            // menuMain
            // 
            menuMain.Font = new Font("Segoe UI", 10F);
            menuMain.ImageScalingSize = new Size(32, 32);
            menuMain.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, presetToolStripMenuItem, comToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(800, 40);
            menuMain.TabIndex = 0;
            menuMain.Text = "menuMain";
            // 
            // MainForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(propertiesListControl);
            Controls.Add(menuMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuMain;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OpenIoT";
            FormClosing += MainForm_FormClosing;
            FormClosed += Form1_FormClosed;
            SizeChanged += MainForm_SizeChanged;
            contextMenuTray.ResumeLayout(false);
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuTray;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private Controls.PropertiesListControl propertiesListControl;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem projectsToolStripMenuItem;
        private ToolStripMenuItem presetsToolStripMenuItem1;
        private ToolStripMenuItem boardNameToolStripMenuItem;
        private ToolStripMenuItem boardInfoToolStripMenuItem;
        private ToolStripMenuItem presetToolStripMenuItem;
        private ToolStripMenuItem comToolStripMenuItem;
        private MenuStrip menuMain;
    }
}