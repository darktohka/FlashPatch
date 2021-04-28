using System.Drawing;

namespace FlashPatch {
    partial class PatchForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatchForm));
            this.patchButton = new System.Windows.Forms.Button();
            this.restoreButton = new System.Windows.Forms.Button();
            this.logoLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.githubLabel = new System.Windows.Forms.Label();
            this.patchFileLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // patchButton
            // 
            this.patchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.patchButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.patchButton.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patchButton.ForeColor = System.Drawing.Color.White;
            this.patchButton.Location = new System.Drawing.Point(54, 134);
            this.patchButton.Name = "patchButton";
            this.patchButton.Size = new System.Drawing.Size(95, 45);
            this.patchButton.TabIndex = 0;
            this.patchButton.Text = "Patch";
            this.patchButton.UseVisualStyleBackColor = true;
            this.patchButton.Click += new System.EventHandler(this.patchButton_Click);
            // 
            // restoreButton
            // 
            this.restoreButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.restoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.restoreButton.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.restoreButton.ForeColor = System.Drawing.Color.White;
            this.restoreButton.Location = new System.Drawing.Point(159, 134);
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Size = new System.Drawing.Size(95, 45);
            this.restoreButton.TabIndex = 1;
            this.restoreButton.Text = "Restore";
            this.restoreButton.UseVisualStyleBackColor = true;
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // logoLabel
            // 
            this.logoLabel.AutoSize = true;
            this.logoLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoLabel.ForeColor = System.Drawing.Color.White;
            this.logoLabel.Location = new System.Drawing.Point(35, 9);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(197, 47);
            this.logoLabel.TabIndex = 2;
            this.logoLabel.Text = "FlashPatch!";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.Location = new System.Drawing.Point(220, 27);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(56, 25);
            this.versionLabel.TabIndex = 3;
            this.versionLabel.Text = "vDev";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionLabel.ForeColor = System.Drawing.Color.White;
            this.descriptionLabel.Location = new System.Drawing.Point(28, 62);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(250, 60);
            this.descriptionLabel.TabIndex = 4;
            this.descriptionLabel.Text = "Play Adobe Flash Player games in\r\nthe browser after January 12th, 2021.\r\nNow supp" +
    "orts Chinese Flash!";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // githubLabel
            // 
            this.githubLabel.AutoSize = true;
            this.githubLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.githubLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.githubLabel.ForeColor = System.Drawing.Color.White;
            this.githubLabel.Location = new System.Drawing.Point(12, 189);
            this.githubLabel.Name = "githubLabel";
            this.githubLabel.Size = new System.Drawing.Size(122, 13);
            this.githubLabel.TabIndex = 5;
            this.githubLabel.Text = "by darktohka - GitHub";
            this.githubLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.githubLabel.Click += new System.EventHandler(this.githubLabel_Click);
            this.githubLabel.MouseEnter += new System.EventHandler(this.githubLabel_MouseEnter);
            this.githubLabel.MouseLeave += new System.EventHandler(this.githubLabel_MouseLeave);
            // 
            // patchFileLabel
            // 
            this.patchFileLabel.AutoSize = true;
            this.patchFileLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.patchFileLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patchFileLabel.ForeColor = System.Drawing.Color.White;
            this.patchFileLabel.Location = new System.Drawing.Point(239, 189);
            this.patchFileLabel.Name = "patchFileLabel";
            this.patchFileLabel.Size = new System.Drawing.Size(65, 13);
            this.patchFileLabel.TabIndex = 6;
            this.patchFileLabel.Text = "Patch File...";
            this.patchFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.patchFileLabel.Click += new System.EventHandler(this.patchFileLabel_Click);
            // 
            // PatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(316, 213);
            this.Controls.Add(this.patchFileLabel);
            this.Controls.Add(this.githubLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.logoLabel);
            this.Controls.Add(this.restoreButton);
            this.Controls.Add(this.patchButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PatchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FlashPatch!";
            this.Load += new System.EventHandler(this.PatchForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button patchButton;
        private System.Windows.Forms.Button restoreButton;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label githubLabel;
        private System.Windows.Forms.Label patchFileLabel;
    }
}
