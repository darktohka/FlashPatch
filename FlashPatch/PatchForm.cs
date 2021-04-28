using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashPatch {
    public partial class PatchForm : Form {
        public PatchForm() {
            InitializeComponent();
        }

        private void githubLabel_MouseEnter(object sender, EventArgs e) {
            githubLabel.Font = new Font(githubLabel.Font.Name, githubLabel.Font.SizeInPoints, FontStyle.Underline);
        }

        private void githubLabel_MouseLeave(object sender, EventArgs e) {
            githubLabel.Font = new Font(githubLabel.Font.Name, githubLabel.Font.SizeInPoints, FontStyle.Regular);
        }

        private void patchFileLabel_MouseEnter(object sender, EventArgs e) {
            patchFileLabel.Font = new Font(patchFileLabel.Font.Name, patchFileLabel.Font.SizeInPoints, FontStyle.Underline);
        }

        private void patchFileLabel_MouseLeave(object sender, EventArgs e) {
            patchFileLabel.Font = new Font(patchFileLabel.Font.Name, patchFileLabel.Font.SizeInPoints, FontStyle.Regular);
        }

        private void patchButton_Click(object sender, EventArgs e) {
            Patcher.PatchAll();
        }

        private void restoreButton_Click(object sender, EventArgs e) {
            Patcher.RestoreAll();
        }

        private void githubLabel_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/darktohka/FlashPatch");
        }

        private void patchFileLabel_Click(object sender, EventArgs e) {
            IntPtr redirection = Patcher.DisableRedirection();

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select Flash Player binaries to patch!";
            dialog.Multiselect = true;
            dialog.ShowDialog();

            string[] paths = dialog.FileNames;

            if (paths.Length != 0) {
                Patcher.PatchFiles(paths);
            }

            Patcher.EnableRedirection(redirection);
        }

        private async void PatchForm_Load(object sender, EventArgs e) {
            versionLabel.Text = UpdateChecker.GetCurrentVersion();
            StartUpdateChecker();
        }

        private async void StartUpdateChecker() {
#if !DEBUG
            await Task.Run(UpdateChecker.GetLatestVersion).ContinueWith(result => {
                Version version = result.Result;

                if (version == null) {
                    // We couldn't find the latest version.
                    return;
                }

                if (version.GetVersion().Equals(UpdateChecker.GetCurrentVersion())) {
                    // We're running the latest version!
                    return;
                }

                string caption = "A new update is available for FlashPatch!\n\n" + version.GetName() + "\n\nWould you like to update now?";

                if (MessageBox.Show(caption, "FlashPatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    Process.Start(version.GetUrl());
                }
            });
#endif
        }
    }
}
