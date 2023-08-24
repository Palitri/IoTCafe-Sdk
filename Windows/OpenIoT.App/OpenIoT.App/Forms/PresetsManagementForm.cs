using OpenIoT.App.Dialogs;
using OpenIoT.Lib.Web.Api;
using OpenIoT.Lib.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenIoT.App.Forms.ProjectSelectForm;

namespace OpenIoT.App.Forms
{
    public partial class PresetsManagementForm : Form
    {
        private PresetsCollection presets;

        public PresetsManagementForm()
        {
            InitializeComponent();
        }

        private async void PresetsManagementForm_Load(object sender, EventArgs e)
        {
            this.presets = await new OpenIoTService()
            {
                Token = AppBase.Instance.Board.persistence.getToken()
            }.RequestProjectPresets(AppBase.Instance.Board.persistence.GetProject().ProjectId);

            AppBase.Instance.Board.persistence.SetPresets(this.presets);

            this.presetsList.Presets = this.presets;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdatePresets()
        {
            AppBase.Instance.Board.persistence.SetPresets(this.presets);
            this.presetsList.Presets = this.presets;
        }

        private async void btnNewPreset_Click(object sender, EventArgs e)
        {
            InputDialog presetNameDialog = new InputDialog() { Input = AppBase.Instance.Board.GenerateUniquePresetName(Resources.PresetNewDefaultName) };
            if (presetNameDialog.ShowDialog() == DialogResult.OK)
            {
                Preset preset = AppBase.Instance.Board.SnapshotProjectPreset(presetNameDialog.Input);

                Preset savedPreset = await new OpenIoTService()
                {
                    Token = AppBase.Instance.Board.persistence.getToken()
                }.RequestSaveProjectPreset(AppBase.Instance.Board.persistence.GetProject().ProjectId, preset);

                this.presets.Add(savedPreset);
                this.UpdatePresets();
            }
        }

        private async void presetsList_PresetDelete(object sender, Controls.EventHandlers.PresetEventArgs args)
        {
            if (await new OpenIoTService()
            {
                Token = AppBase.Instance.Board.persistence.getToken()
            }.RequestDeleteProjectPreset(args.Preset))
            {
                this.presets.Remove(args.Preset);
                this.UpdatePresets();
            }
            else
                MessageBox.Show(Resources.PresetErrorDeleting, Resources.Preset, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void presetsList_PresetRename(object sender, Controls.EventHandlers.PresetEventArgs args)
        {
            InputDialog presetNameDialog = new InputDialog() { Input = args.Preset.Name };
            if (presetNameDialog.ShowDialog() == DialogResult.OK)
            {
                args.Preset.Name = presetNameDialog.Input;
                Preset preset = await new OpenIoTService()
                {
                    Token = AppBase.Instance.Board.persistence.getToken()
                }.RequestUpdateProjectPreset(args.Preset);

                this.UpdatePresets();
            }
        }

        private async void presetsList_PresetUpdate(object sender, Controls.EventHandlers.PresetEventArgs args)
        {
            Preset updatedPresetData = AppBase.Instance.Board.SnapshotProjectPreset(args.Preset.Name);
            args.Preset.Config = updatedPresetData.Config;

            Preset updatedPreset = await new OpenIoTService()
            {
                Token = AppBase.Instance.Board.persistence.getToken()
            }.RequestUpdateProjectPreset(args.Preset);

            this.UpdatePresets();
        }

        private void presetsList_PresetClicked(object sender, Controls.EventHandlers.PresetEventArgs args)
        {
            AppBase.Instance.Board.ApplyPreset(args.Preset);
        }
    }
}
