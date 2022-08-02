using System;
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace FlashPatch {
    public partial class PatchWindow : Window {

        public PatchWindow() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            versionLabel.Content = UpdateChecker.GetCurrentVersion();
            StartUpdateChecker();
        }

        private void PatchButton_Click(object sender, RoutedEventArgs e) {
            Patcher.PatchAll();
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e) {
            Patcher.RestoreAll();
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e) {
            Process.Start("https://github.com/darktohka/FlashPatch");
        }

        private void PatchFileButton_Click(object sender, RoutedEventArgs e) {
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

        private void StartUpdateChecker() {
#if !DEBUG
            Task.Run(UpdateChecker.GetLatestVersion).ContinueWith(result => {
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

                if (MessageBox.Show(caption, "FlashPatch!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) {
                    Process.Start(version.GetUrl());
                }
            });
        }
#endif
    }
}
