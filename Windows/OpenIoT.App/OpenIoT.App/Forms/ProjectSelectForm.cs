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

namespace OpenIoT.App.Forms
{
    public partial class ProjectSelectForm : Form
    {
        public class ProjectsListItem
        {
            public Project Project { get; set; }

            public override string ToString() { return this.Project != null ? this.Project.Name : String.Empty; }
        }

        public ProjectSelectForm()
        {
            InitializeComponent();
        }

        private async void ProjectSelectForm_Load(object sender, EventArgs e)
        {
            IEnumerable<Project> projects = await new OpenIoTService()
            {
                Token = AppBase.Instance.Board.persistence.getToken()
            }.RequestUserProjects();

            this.lbProjects.Items.Clear();
            this.lbProjects.Items.AddRange(projects.Select(p => new ProjectsListItem() { Project = p }).ToArray());
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            Project project = this.lbProjects.SelectedItem != null ? ((ProjectsListItem)this.lbProjects.SelectedItem).Project : null;

            if (project == null)
            {
                MessageBox.Show(Resources.PleaseSelectProject, Resources.Projects, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Tuple<Project, PresetsCollection> p = await new OpenIoTService()
            {
                Token = AppBase.Instance.Board.persistence.getToken()
            }.RequestUserProject(project.ProjectId);

            AppBase.Instance.Board.LoadProject(p.Item1, p.Item2);

            this.DialogResult = DialogResult.OK;
        }

        private void lbProjects_DoubleClick(object sender, EventArgs e)
        {
            this.btnOk.PerformClick();
        }
    }
}
