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
            menuMain = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            loginToolStripMenuItem = new ToolStripMenuItem();
            projectsToolStripMenuItem = new ToolStripMenuItem();
            presetsToolStripMenuItem1 = new ToolStripMenuItem();
            boardNameToolStripMenuItem = new ToolStripMenuItem();
            boardInfoToolStripMenuItem = new ToolStripMenuItem();
            presetToolStripMenuItem = new ToolStripMenuItem();
            comToolStripMenuItem = new ToolStripMenuItem();
            testToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            notifyIcon = new NotifyIcon(components);
            contextMenuTray = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            propertiesListControl = new OpenIoT.App.Controls.PropertiesListControl();
            toolStripMenuItem6 = new ToolStripMenuItem();
            menuMain.SuspendLayout();
            contextMenuTray.SuspendLayout();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            menuMain.ImageScalingSize = new Size(32, 32);
            menuMain.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, presetToolStripMenuItem, comToolStripMenuItem, testToolStripMenuItem });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(800, 45);
            menuMain.TabIndex = 0;
            menuMain.Text = "menuMain";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem, projectsToolStripMenuItem, presetsToolStripMenuItem1, boardNameToolStripMenuItem, boardInfoToolStripMenuItem });
            optionsToolStripMenuItem.Image = (Image)resources.GetObject("optionsToolStripMenuItem.Image");
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(52, 41);
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Image = (Image)resources.GetObject("loginToolStripMenuItem.Image");
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(296, 46);
            loginToolStripMenuItem.Text = Resources.Login;
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // projectsToolStripMenuItem
            // 
            projectsToolStripMenuItem.Image = (Image)resources.GetObject("projectsToolStripMenuItem.Image");
            projectsToolStripMenuItem.Name = "projectsToolStripMenuItem";
            projectsToolStripMenuItem.Size = new Size(296, 46);
            projectsToolStripMenuItem.Text = Resources.Projects;
            projectsToolStripMenuItem.Click += projectsToolStripMenuItem_Click;
            // 
            // presetsToolStripMenuItem1
            // 
            presetsToolStripMenuItem1.Image = (Image)resources.GetObject("presetsToolStripMenuItem1.Image");
            presetsToolStripMenuItem1.Name = "presetsToolStripMenuItem1";
            presetsToolStripMenuItem1.Size = new Size(296, 46);
            presetsToolStripMenuItem1.Text = Resources.Presets;
            presetsToolStripMenuItem1.Click += presetsToolStripMenuItem1_Click;
            // 
            // boardNameToolStripMenuItem
            // 
            boardNameToolStripMenuItem.Image = Resources.icon_edit;
            boardNameToolStripMenuItem.Name = "boardNameToolStripMenuItem";
            boardNameToolStripMenuItem.Size = new Size(296, 46);
            boardNameToolStripMenuItem.Text = "Board name";
            boardNameToolStripMenuItem.Click += boardNameToolStripMenuItem_Click;
            // 
            // boardInfoToolStripMenuItem
            // 
            boardInfoToolStripMenuItem.Image = (Image)resources.GetObject("boardInfoToolStripMenuItem.Image");
            boardInfoToolStripMenuItem.Name = "boardInfoToolStripMenuItem";
            boardInfoToolStripMenuItem.Size = new Size(296, 46);
            boardInfoToolStripMenuItem.Text = Resources.BoardInfo;
            boardInfoToolStripMenuItem.Click += boardInfoToolStripMenuItem_Click;
            // 
            // presetToolStripMenuItem
            // 
            presetToolStripMenuItem.Image = (Image)resources.GetObject("presetToolStripMenuItem.Image");
            presetToolStripMenuItem.Name = "presetToolStripMenuItem";
            presetToolStripMenuItem.Size = new Size(52, 41);
            // 
            // comToolStripMenuItem
            // 
            comToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            comToolStripMenuItem.Image = (Image)resources.GetObject("comToolStripMenuItem.Image");
            comToolStripMenuItem.Name = "comToolStripMenuItem";
            comToolStripMenuItem.Size = new Size(52, 41);
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6 });
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(80, 41);
            testToolStripMenuItem.Text = "test";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(359, 46);
            toolStripMenuItem2.Text = "1";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(359, 46);
            toolStripMenuItem3.Text = "2";
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(359, 46);
            toolStripMenuItem4.Text = "3";
            toolStripMenuItem4.Click += toolStripMenuItem4_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(359, 46);
            toolStripMenuItem5.Text = "4";
            toolStripMenuItem5.Click += toolStripMenuItem5_Click;
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
            propertiesListControl.Location = new Point(0, 45);
            propertiesListControl.Name = "propertiesListControl";
            propertiesListControl.Size = new Size(800, 405);
            propertiesListControl.TabIndex = 1;
            propertiesListControl.PropertyClicked += propertiesListControl_PropertyClicked;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(359, 46);
            toolStripMenuItem6.Text = "5";
            toolStripMenuItem6.Click += toolStripMenuItem6_Click;
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
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            contextMenuTray.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStripMenuItem testToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
    }
}