using Palitri.OpenIoT.App.Dialogs;
using Palitri.OpenIoT.App.Dialogs;
using Palitri.OpenIoT.Board.Api;
using Palitri.OpenIoT.Board.Models;
using Palitri.OpenIoT.Board.Protocol;
using Palitri.OpenIoT.Board.Protocol.Events;
using Palitri.OpenIoT.Board.Scanner;
using Palitri.OpenIoT.Board.Transmission;
using Palitri.OpenIoT.Board.Transmission.Com;
using Palitri.OpenIoT.Composite;
using Palitri.OpenIoT.SoftwarePeripherals;
using Palitri.OpenIoT.SoftwarePeripherals.SoftwareControls;
using Palitri.OpenIoT.Web.Api;
using Palitri.OpenIoT.Web.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Palitri.OpenIoT.App.Forms
{
    public partial class MainForm : Form
    {
        private bool allowClose;
        private bool isRequestingBoardName;
        private bool isRequestingBoardInfo;

        private OpenIoTBoardScanner scanner;

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


            this.scanner = new OpenIoTBoardScanner();
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
            AppBase.Instance.Board.boardDevice.RequestAllDeviceProperties();
        }

        private void propertiesListControl_PropertyClicked(object sender, Controls.EventHandlers.PropertyEventArgs args)
        {
            if ((args.Property.BoardProperty.type == BoardPropertyType.Float) || (args.Property.BoardProperty.type == BoardPropertyType.Integer))
            {
                new PropertyNumericValueDialog(args.Property).ShowDialog();
            }
            else if (args.Property.BoardProperty.type == BoardPropertyType.Data)
            {
                new PropertyDataValueDialog(args.Property).ShowDialog();
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
            this.Invoke((MethodInvoker)delegate
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
                AppBase.Instance.Board.boardDevice.RequestSetDeviceName(dialog.Input);
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
            AppBase.Instance.Board.boardDevice.RequestDeviceName();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            byte commandId = 72;

            byte[] fakeData = {
                2, // peripheralId - async
                1, // commandCode SetNumberOfChannels
                1, // commandSize
                2, // numberOfChannels

                2, // peripheralId - async
                2, // commandCode SetChannelDevice
                2, // commandSize
                0, // channelId
                0, // peripheralId - motor0

                2, // peripheralId - async
                2, // commandCode SetChannelDevice
                2, // commandSize
                1, // channelId
                6, // peripheralId - motor1				

                3, // peripheralId - async
                1, // commandCode CommandCode_SetAsyncDevice
                1, // commandSize
                2, // peripheralId
            };

            AppBase.Instance.Board.boardDevice.SendCommand(commandId, fakeData);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            byte commandId = 72;

            float fakeVector1 = 2 * 200.0f;
            byte[] fakeVector1Bytes = BitConverter.GetBytes(fakeVector1);
            float fakeVector2 = 3 * 200.0f;
            byte[] fakeVector2Bytes = BitConverter.GetBytes(fakeVector2);
            float fakeVector3 = -1 * 200.0f;
            byte[] fakeVector3Bytes = BitConverter.GetBytes(fakeVector3);
            float fakeVector4 = -2 * 200.0f;
            byte[] fakeVector4Bytes = BitConverter.GetBytes(fakeVector4);
            float fakeTime = 2.0f;
            byte[] fakeTimeBytes = BitConverter.GetBytes(fakeTime);

            byte[] fakeData = new byte[]
            {
                2, // peripheralId - async
                4, // commandCode SetVector
                5, // commandSize
                0, // channelId
                fakeVector1Bytes[0],
                fakeVector1Bytes[1],
                fakeVector1Bytes[2],
                fakeVector1Bytes[3],

                2, // peripheralId - async
                4, // commandCode SetVector
                5, // commandSize
                1, // channelId
                fakeVector2Bytes[0],
                fakeVector2Bytes[1],
                fakeVector2Bytes[2],
                fakeVector2Bytes[3],

                2, // peripheralId - async
                5, // commandCode Drive
                4, // commandSize
                fakeTimeBytes[0],
                fakeTimeBytes[1],
                fakeTimeBytes[2],
                fakeTimeBytes[3],


                2, // peripheralId - async
                4, // commandCode SetVector
                5, // commandSize
                0, // channelId
                fakeVector3Bytes[0],
                fakeVector3Bytes[1],
                fakeVector3Bytes[2],
                fakeVector3Bytes[3],

                2, // peripheralId - async
                4, // commandCode SetVector
                5, // commandSize
                1, // channelId
                fakeVector4Bytes[0],
                fakeVector4Bytes[1],
                fakeVector4Bytes[2],
                fakeVector4Bytes[3],

                2, // peripheralId - async
                5, // commandCode Drive
                4, // commandSize
                fakeTimeBytes[0],
                fakeTimeBytes[1],
                fakeTimeBytes[2],
                fakeTimeBytes[3],
            };

            AppBase.Instance.Board.boardDevice.SendCommand(commandId, fakeData);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            byte commandId = 72;

            float fakeSpeed = 200.0f;
            byte[] fakeSpeedBytes = BitConverter.GetBytes(fakeSpeed);
            float fakePos = 500.0f;
            byte[] fakePosBytes = BitConverter.GetBytes(fakePos);
            float fakeNegValue = -500.0f;
            byte[] fakeNegBytes = BitConverter.GetBytes(fakeNegValue);
            float fake0 = 0.0f;
            byte[] fake0Bytes = BitConverter.GetBytes(fake0);

            byte[] fakeData = new byte[]
            {
                3, // peripheralId - async
                12, // CommandCode_Bezier
                (1 + (3 * 2)) * 4, // commandSize
                fakeSpeedBytes[0],
                fakeSpeedBytes[1],
                fakeSpeedBytes[2],
                fakeSpeedBytes[3],

                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],
                fake0Bytes[0],
                fake0Bytes[1],
                fake0Bytes[2],
                fake0Bytes[3],

                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],
                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],

                fake0Bytes[0],
                fake0Bytes[1],
                fake0Bytes[2],
                fake0Bytes[3],
                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],
            };

            AppBase.Instance.Board.boardDevice.SendCommand(commandId, fakeData);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            byte commandId = 72;

            float fakeSpeed = 200.0f;
            byte[] fakeSpeedBytes = BitConverter.GetBytes(fakeSpeed);
            float fakeArcStart = 0.0f;
            byte[] fakeArcStartBytes = BitConverter.GetBytes(fakeArcStart);
            float fakeArcEnd = (float)Math.PI * 2;
            byte[] fakeArcEndBytes = BitConverter.GetBytes(fakeArcEnd);
            float fakeFloat0 = 0.0f;
            byte[] fakeFloat0Bytes = BitConverter.GetBytes(fakeFloat0);
            float fakeFloat1 = 3.0f;
            byte[] fakeFloat1Bytes = BitConverter.GetBytes(fakeFloat1);

            byte[] fakeData = new byte[]
            {
                3, // peripheralId - async
                13, // CommandCode_Arc
                (1 + 2 + 2 * 2) * 4, // commandSize
                fakeSpeedBytes[0],
                fakeSpeedBytes[1],
                fakeSpeedBytes[2],
                fakeSpeedBytes[3],
                fakeArcStartBytes[0],
                fakeArcStartBytes[1],
                fakeArcStartBytes[2],
                fakeArcStartBytes[3],
                fakeArcEndBytes[0],
                fakeArcEndBytes[1],
                fakeArcEndBytes[2],
                fakeArcEndBytes[3],
                fakeFloat1Bytes[0],
                fakeFloat1Bytes[1],
                fakeFloat1Bytes[2],
                fakeFloat1Bytes[3],
                fakeFloat0Bytes[0],
                fakeFloat0Bytes[1],
                fakeFloat0Bytes[2],
                fakeFloat0Bytes[3],
                fakeFloat0Bytes[0],
                fakeFloat0Bytes[1],
                fakeFloat0Bytes[2],
                fakeFloat0Bytes[3],
                fakeFloat1Bytes[0],
                fakeFloat1Bytes[1],
                fakeFloat1Bytes[2],
                fakeFloat1Bytes[3],
            };

            AppBase.Instance.Board.boardDevice.SendCommand(commandId, fakeData);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            byte commandId = 72;

            float fakeSpeed = 200.0f;
            byte[] fakeSpeedBytes = BitConverter.GetBytes(fakeSpeed);
            float fakePos = 500.0f;
            byte[] fakePosBytes = BitConverter.GetBytes(fakePos);
            float fakeNegValue = -500.0f;
            byte[] fakeNegBytes = BitConverter.GetBytes(fakeNegValue);
            float fake0 = 0.0f;
            byte[] fake0Bytes = BitConverter.GetBytes(fake0);

            byte[] fakeData = new byte[]
            {
                3, // peripheralId - async
                11, // CommandCode_Polyline
                (1 + (3 * 2)) * 4, // commandSize
                fakeSpeedBytes[0],
                fakeSpeedBytes[1],
                fakeSpeedBytes[2],
                fakeSpeedBytes[3],
                
                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],
                fake0Bytes[0],
                fake0Bytes[1],
                fake0Bytes[2],
                fake0Bytes[3],
                
                fake0Bytes[0],
                fake0Bytes[1],
                fake0Bytes[2],
                fake0Bytes[3],
                fakePosBytes[0],
                fakePosBytes[1],
                fakePosBytes[2],
                fakePosBytes[3],
                
                fakeNegBytes[0],
                fakeNegBytes[1],
                fakeNegBytes[2],
                fakeNegBytes[3],
                fakeNegBytes[0],
                fakeNegBytes[1],
                fakeNegBytes[2],
                fakeNegBytes[3],
            };

            AppBase.Instance.Board.boardDevice.SendCommand(commandId, fakeData);
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

        public override void OnAllPropertiesInfoReceived(object sender)
        {
            this.mainForm.onAllPropertiesInfoReceived(sender);
        }

        public override void OnSubscribedPropertyValueChanged(object sender, BoardProperty p, object oldValue)
        {
            this.mainForm.onSubscribedPropertyValueChanged(sender, p, oldValue);
        }

        public override void OnInfoReceived(object sender, string info)
        {
            this.mainForm.onInfoReceived(sender, info);
        }

        public override void OnDeviceNameReceived(object sender, string name)
        {
            this.mainForm.onDeviceNameReceived(sender, name);
        }

        public override void OnDevicePropertiesReceived(object sender, Dictionary<int, byte[]> properties)
        {
            this.mainForm.onDevicePropertiesReceived(sender, properties);
        }
    }
}