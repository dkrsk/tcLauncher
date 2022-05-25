using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using DnKR.tcLauncher.MVersionInstaller;

namespace DnKR.tcLauncher.GUI
{
    /// <summary>
    /// Логика взаимодействия для InstallerWindow.xaml
    /// </summary>
    public partial class InstallerWindow : Window
    {
        readonly IVersionInstaller versionInstaller;

        public InstallerWindow(IVersionInstaller versionInstaller)
        {
            this.versionInstaller = versionInstaller;

            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            this.Title = $"{versionInstaller.InstallerName} version installer";

            cbVersions.Items.Clear();

            var versions = await versionInstaller.GetVersionMetadatas();

            foreach (var item in versions)
            {
                cbVersions.Items.Add(item.Name);
            }
        }

        private async void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            btnInstall.IsEnabled = false;

            if (!string.IsNullOrWhiteSpace(cbVersions.Text))
            {
                await versionInstaller.InstallVersion(cbVersions.Text);

                MessageBox.Show("Success!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Select the version first!");
            }

            btnInstall.IsEnabled = true;
        }
    }
}
