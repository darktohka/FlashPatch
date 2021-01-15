using System;
using System.Diagnostics;
using System.Drawing;
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

        private void patchButton_Click(object sender, EventArgs e) {
            Patcher.PatchAll();
        }

        private void restoreButton_Click(object sender, EventArgs e) {
            Patcher.RestoreAll();
        }

        private void githubLabel_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/darktohka/FlashPatch");
        }
    }
}
