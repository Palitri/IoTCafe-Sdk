using OpenIoT.App.Dialogs;
using OpenIoT.App.Dialogs;
using OpenIoT.Lib.Board.Api;
using OpenIoT.Lib.Board.Models;
using OpenIoT.Lib.Board.Protocol;
using OpenIoT.Lib.Board.Protocol.Events;
using OpenIoT.Lib.Board.Scanner;
using OpenIoT.Lib.Board.Transmission;
using OpenIoT.Lib.Board.Transmission.Com;
using OpenIoT.Lib.Composite;
using OpenIoT.Lib.SoftwarePeripherals;
using OpenIoT.Lib.SoftwarePeripherals.SoftwareControls;
using OpenIoT.Lib.Web.Api;
using OpenIoT.Lib.Web.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenIoT.App.Forms
{
    public partial class MainForm : Form
    {
        private bool allowClose;
        private bool isRequestingBoardName;
        private bool isRequestingBoardInfo;

        private BoardScanner scanner;

        public MainForm()
        {
            InitializeComponent();

            this.allowClose = true;
            this.isRequestingBoardName = false;
            this.isRequestingBoardInfo = false;

            AppBase.Instance.Board.AddEventHandler(new MainFormOpenIoTEventHandler(this));
            AppBase.Instance.Board.OnConnected += this.OnBoardConnected;
            AppBase.Instance.Board.OnDisconnected += this.OnBoardDisconnected;


            // TODO: Rework all this handle and software dispather stuff
            AppBase.Instance.Board.Handle = this.Handle;


            this.scanner = new BoardScanner();
            this.scanner.OnPortAvailable += this.OnPortAvailable;
            this.scanner.OnPortUnavailable += this.OnPortUnavailable;
            this.scanner.OnBoardAvailable += this.OnBoardAvailable;
            this.scanner.OnBoardUnavailable += this.OnBoardUnavailable;
            this.scanner.ScanOnce();
        }

        private void OnBoardConnected(CompositeBoard sender, OpenIoTBoard board)
        {
            foreach (ToolStripMenuItem item in this.comToolStripMenuItem.DropDownItems)
                item.Checked = item.Name == board.transmissionChannel.Name;
        }

        private void OnBoardDisconnected(CompositeBoard sender)
        {
        }

        private ToolStripMenuItem GetPortMenuItem(string port)
        {
            return this.comToolStripMenuItem.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault(i => i.Name == port);
        }

        private void SetPortMenuItem(string port, BoardScanInfo info = null, bool allowCreate = true)
        {
            this.BeginInvoke(delegate
            {
                ToolStripMenuItem menuItem = this.GetPortMenuItem(port);
                if (menuItem == null)
                {
                    if (!allowCreate)
                        return;

                    menuItem = new ToolStripMenuItem();
                    menuItem.Click += new EventHandler(this.ComPortMenuItemClicked);
                    this.comToolStripMenuItem.DropDownItems.Add(menuItem);
                }

                menuItem.Name = port;
                menuItem.Text = info == null ? port : info.Port + " " + info.ToString();
                menuItem.Checked = AppBase.Instance.Board.boardDevice.transmissionChannel != null && AppBase.Instance.Board.boardDevice.transmissionChannel.Name == port;
            });
        }

        private void RemovePortMenuItem(string port)
        {
            this.BeginInvoke(delegate
            {
                ToolStripMenuItem menuItem = this.GetPortMenuItem(port);
                this.comToolStripMenuItem.DropDownItems.Remove(menuItem);
            });
        }

        private void OnPortAvailable(object sender, BoardPortEventArgs args)
        {
            this.SetPortMenuItem(args.Port);
        }

        private void OnPortUnavailable(object sender, BoardPortEventArgs args)
        {
            this.RemovePortMenuItem(args.Port);
        }

        private void OnBoardAvailable(object sender, BordInfoEventArgs args)
        {
            this.Invoke(delegate
            {
                if (!AppBase.Instance.Board.IsConnected)
                    AppBase.Instance.Board.ConnectToPort(args.Info.Port);
            });

            this.SetPortMenuItem(args.Info.Port, args.Info);
        }

        private void OnBoardUnavailable(object sender, BordInfoEventArgs args)
        {
            this.SetPortMenuItem(args.Info.Port, null, false);
        }

        private void LoadPresetsMenu()
        {
            this.presetToolStripMenuItem.DropDownItems.Clear();
            this.presetToolStripMenuItem.DropDownItems.AddRange(AppBase.Instance.Board.persistence.GetPresets().Select(p => new ToolStripMenuItem(p.Name, null, this.presetToolStripMenuItem_Click, p.ProjectPresetId)).ToArray());

            this.contextMenuTray.Items.Clear();
            this.contextMenuTray.Items.AddRange(AppBase.Instance.Board.persistence.GetPresets().Select(p => new ToolStripMenuItem(p.Name, null, this.presetToolStripMenuItem_Click, p.ProjectPresetId) { Image = Resources.icon_items }).ToArray());
            this.contextMenuTray.Items.Add(new ToolStripSeparator());
            this.contextMenuTray.Items.Add(new ToolStripMenuItem("Open", null, this.openToolStripMenuItem_Click));
            this.contextMenuTray.Items.Add(new ToolStripMenuItem("Close", null, this.exitToolStripMenuItem_Click));
        }

        private void ComPortMenuItemClicked(object? sender, EventArgs args)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;

            if (!menuItem.Checked)
            {
                AppBase.Instance.Board.DisconnectFromBoard();
                AppBase.Instance.Board.ConnectToPort(menuItem.Name);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.scanner.Dispose();

            AppBase.Instance.Board.DisconnectFromBoard();
            AppBase.Instance.Board.Dispose();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LoginForm().ShowDialog();
        }

        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AppBase.Instance.Board.persistence.IsUserLogged)
                new LoginForm().ShowDialog();

            if (!AppBase.Instance.Board.persistence.IsUserLogged)
                return;

            ProjectSelectForm projectSelectForm = new ProjectSelectForm();
            if (projectSelectForm.ShowDialog() == DialogResult.OK)
            {
                AppBase.Instance.Board.requestProjectUploadSequence();
            }

        }

        private void presetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AppBase.Instance.Board.IsConnected)
            {
                MessageBox.Show(Resources.DevicePleaseConnect);
                return;
            }

            string presetId = ((ToolStripMenuItem)sender).Name;

            AppBase.Instance.Board.ApplyPreset(AppBase.Instance.Board.persistence.GetPresets().First(p => p.ProjectPresetId == presetId));
        }

        private void presetsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!AppBase.Instance.Board.persistence.IsProjectSelected)
            {
                MessageBox.Show(Resources.PleaseSelectProject);
                return;
            }


            PresetsManagementForm presetsManagementForm = new PresetsManagementForm();
            presetsManagementForm.ShowDialog();
            this.LoadPresetsMenu();
        }

        private void boardInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AppBase.Instance.Board.IsConnected)
            {
                MessageBox.Show(Resources.DevicePleaseConnect);
                return;
            }
            
            this.isRequestingBoardInfo = true;
//            AppBase.Instance.Board.boardDevice.requestBoardInfo();
            AppBase.Instance.Board.boardDevice.requestAllDeviceProperties();
        }

        private void propertiesListControl_PropertyClicked(object sender, Controls.EventHandlers.PropertyEventArgs args)
        {
            if ((args.Property.BoardProperty.type == BoardPropertyType.Float) || (args.Property.BoardProperty.type == BoardPropertyType.Integer))
            {
                new PropertyNumericValueDialog(args.Property).ShowDialog();
            }
            else if (args.Property.BoardProperty.type == BoardPropertyType.Bool)
            {
                args.Property.BoardProperty.SetValue(!args.Property.BoardProperty.GetBool());
                args.Property.Board.boardDevice.RequestPropertyUpdate(args.Property.BoardProperty);
            }
        }

        #region IOpenCNCProtocolEvents

        public void onAllPropertiesInfoReceived(object sender)
        {
            this.Invoke((MethodInvoker) delegate 
            {
                this.LoadPresetsMenu();

                this.propertiesListControl.Properties = AppBase.Instance.Board.visibleProperties;
            });
        }

        public void onSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue)
        {
            this.propertiesListControl.Invoke((MethodInvoker)delegate
            {
                this.propertiesListControl.UpdateProperty(AppBase.Instance.Board.properties.FirstOrDefault(bp => bp.BoardProperty == p));
            });
        }

        public void onInfoReceived(object sender, string info)
        {
            MessageBox.Show(info, Resources.BoardInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void onDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
            if (!this.isRequestingBoardInfo)
                return;

            this.isRequestingBoardInfo = false;

            MessageBox.Show(String.Join(Environment.NewLine, properties.Select(p => 
            {
                string name;
                object value;
                OpenIoTProtocol.GetDevicePropertyFriendly(p.Key, p.Value, out name, out value);
                return String.Format("{0}: {1}", name, value);
            })), Resources.BoardInfo, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        public void onDeviceNameReceived(object sender, string name)
        {
            if (!this.isRequestingBoardName)
                return;

            this.isRequestingBoardName = false;

            InputDialog dialog = new InputDialog();
            dialog.Input = name;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                AppBase.Instance.Board.boardDevice.requestSetDeviceName(dialog.Input);
            }
        }

        #endregion

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
            e.Cancel = !this.allowClose;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.allowClose = true;
            this.Close();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void boardNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!AppBase.Instance.Board.IsConnected)
            {
                MessageBox.Show(Resources.DevicePleaseConnect);
                return;
            }
            
            this.isRequestingBoardName = true;
            AppBase.Instance.Board.boardDevice.requestDeviceName();
        }
    }

    internal class MainFormOpenIoTEventHandler : OpenIoTProtocolEventsHandler
    {
        private MainForm mainForm;

        private IntPtr handle;

        public MainFormOpenIoTEventHandler(MainForm mainForm)
        {
            this.mainForm = mainForm;
            this.handle = mainForm.Handle;
        }

        public override void onAllPropertiesInfoReceived(object sender)
        {
            this.mainForm.onAllPropertiesInfoReceived(sender);
        }

        public override void onSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue)
        {
            this.mainForm.onSubscribedPropertyValueChanged(sender, p, oldValue);
        }

        public override void onInfoReceived(object sender, string info)
        {
            this.mainForm.onInfoReceived(sender, info);
        }

        public override void onDeviceNameReceived(object sender, string name)
        {
            this.mainForm.onDeviceNameReceived(sender, name);
        }

        public override void onDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
            this.mainForm.onDevicePropertiesReceived(sender, properties);
        }
    }
}