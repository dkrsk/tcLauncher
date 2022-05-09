using System;
using System.Windows.Media.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using Microsoft.Win32;

using CmlLib.Core;
using CmlLib.Core.Downloader;

using DnKR.tcLauncher.tcUpdater;
using DnKR.tcLauncher.MVersionInstaller;

using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace DnKR.tcLauncher.GUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MGame game;

        readonly UserProperties properties;

        readonly UpdaterConfig updaterConfig = config.GetUpdConfig();

        readonly static ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();

        readonly MinecraftPath gamePath;

        readonly DispatcherTimer tmLog;

        public MainWindow()
        {
            properties = new();
            new Task(async () => await properties.JsonRead()).Start();

            game = new();
            game.OutputRecieved += Process_OutputDataReceived;
            game.ProcessExited += Process_Exited;
            game.ProgressChanged += Launcher_ProgressChanged;
            game.FileChanged += Launcher_FileChanged;

            this.gamePath = game.GamePath;

            tmLog = new();
            tmLog.Tick += tmLog_Tick;
            tmLog.Interval = new TimeSpan(0,0,5);

            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            if (properties.BkgPath != null && File.Exists(properties.BkgPath))
            {
                this.Bkg.ImageSource = new BitmapImage(new Uri(properties.BkgPath));
            }

            await UpdateModpack(true).ConfigureAwait(false);

            txbNick.Text = properties.Nickname;
            txbJavaPath.Text = properties.JavaPath;
            txbJavaArgs.Text = properties.JavaArgs;
            txbRam.Text = properties.Ram;

            RefreshVersions();
        }

        private void RefreshVersions()
        {
            cbVersions.Items.Clear();

            var versions = Directory.EnumerateDirectories(gamePath.Versions);

            if (!versions.Any()) { return; };

            foreach (string ver in versions)
            {
                string[] v = ver.Split(Path.DirectorySeparatorChar);
                string verName = v[^1];

                cbVersions.Items.Add(verName);
            }
            if (string.IsNullOrWhiteSpace(properties.LatestVersion))
            {
                cbVersions.Text = cbVersions.Items[^1].ToString();
            }
            else
            {
                cbVersions.Text = properties.LatestVersion;
            }
        }

        private async void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            setUiEnabled(false);

            try
            {
                if (cbVersions.Text == "")
                {
                    MessageBox.Show("Select the version first!");
                    return;
                }
                else
                {
                    game.GameVersion = cbVersions.Text;
                }

                if (string.IsNullOrWhiteSpace(txbNick.Text))
                {
                    MessageBox.Show("Enter the nickname first!");
                    return;
                }
                else
                {
                    game.UserName = txbNick.Text;
                }

                if (File.Exists(txbJavaPath.Text))
                {
                    game.launchOption.JavaPath = txbJavaPath.Text;
                }
                else if (!string.IsNullOrWhiteSpace(txbJavaPath.Text))
                {
                    MessageBox.Show("Incorrent location to java");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txbJavaArgs.Text))
                {
                    game.launchOption.JVMArguments = txbJavaArgs.Text.Split(' ');
                }

                if (!string.IsNullOrWhiteSpace(txbRam.Text))
                {
                    game.launchOption.MaximumRamMb = int.Parse(txbRam.Text);
                }

                await UpdateProperties().ConfigureAwait(false);

                btnLaunch.IsEnabled = false;

                File.Delete(Path.Combine(gamePath.ToString(), "\\logs.txt"));
                tmLog.Start();


                await game.StartGame();

                Lv_Status.Content = "Playing...";
            }

            catch (Win32Exception wex) // java exception
            {
                MessageBox.Show(wex + "\n\nIt seems your java setting has problem");
            }
            catch (Exception ex) // all exception
            {
                Lv_Status.Content = "Ready";
                btnLaunch.IsEnabled = true;
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Pb_Progress.Value = 0;
                Pb_File.Value = 0;

                setUiEnabled(true);

            }
        }

        private void setUiEnabled(bool value)
        {
            groupMain.IsEnabled = value;
            groupSettings.IsEnabled = value;
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            tmLog.Stop();

            Dispatcher.Invoke(() =>
            {
                Lv_Status.Content = "Ready";
                btnLaunch.IsEnabled = true;
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Output(e.Data);
        }

        private void Output(string? msg)
        {
            logQueue.Enqueue(msg);
        }

        private void tmLog_Tick(object sender, EventArgs e)
        {
            string msg;
            while (logQueue.TryDequeue(out msg))
            {
                File.AppendAllText(gamePath.ToString() + "\\logs.txt", msg + '\n');
            }
        }
        private void txbRam_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = !char.IsDigit((char)e.Key) && !char.IsControl((char)e.Key);
        }

        private void btnInstallVanilla_Click(object sender, RoutedEventArgs e)
        {
            setUiEnabled(false);
            Window window = new InstallerWindow(new VanillaInstaller(game));
            window.Closing += delegate { setUiEnabled(true); RefreshVersions(); };
            window.Show();

        }

        private async void btnInstallFabric_Click(object sender, RoutedEventArgs e)
        {
            setUiEnabled(false);
            Window window = new InstallerWindow(new FabricInstaller(game));
            window.Closing += delegate { setUiEnabled(true); RefreshVersions(); };
            window.Show();
        }

        private void btnInstallQuilt_Click(object sender, RoutedEventArgs e)
        {
            setUiEnabled(false);
            Window window = new InstallerWindow(new QuiltInstaller(game));
            window.Closing += delegate { setUiEnabled(true); RefreshVersions(); };
            window.Show();
        }

        private void btnJavaChange_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\Program Files";
            dialog.Filter = "Exe Files |*.exe|All Files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.Multiselect = false;

            dialog.ShowDialog();

            try
            {
                txbJavaPath.Text = dialog.FileName;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                Pb_Progress.Value = 0;
                Lv_Status.Content = "Ready";
            }
            else
            {
                Pb_Progress.Maximum = 100;
                Pb_Progress.Value = e.ProgressPercentage;
            }
        }

        private void Launcher_FileChanged(DownloadFileChangedEventArgs e)
        {
            if (e.ProgressedFileCount == 100)
            {
                Pb_File.Value = 0;
                Lv_Status.Content = "Ready";
            }
            else
            {
                Pb_File.Maximum = e.TotalFileCount;
                Pb_File.Value = e.ProgressedFileCount;
                Lv_Status.Content = $"{e.FileKind} : {e.FileName} ({e.ProgressedFileCount}/{e.TotalFileCount})";
            }
        }

        private async Task btnUpdatePack_Click(object? sender, RoutedEventArgs? e)
        {
            await UpdateModpack(false);
        }

        private void btnLocaleFiles_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", gamePath.ToString());
        }

        private void btnBkg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE"); ;
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            dialog.FilterIndex = 0;
            dialog.Multiselect = false;

            dialog.ShowDialog();

            try
            {
                if (File.Exists(dialog.FileName))
                {
                    this.Bkg.ImageSource = new BitmapImage(new Uri(dialog.FileName));
                    properties.BkgPath = dialog.FileName;
                }
            }
            catch (ArgumentException)
            {

            }

        }

        private void btnBkgClear_Click(object sender, RoutedEventArgs e)
        {
            this.Bkg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/tclaucher-bg.png"));
            properties.BkgPath = string.Empty;
        }

        private async Task UpdateModpack(bool IsChecking)
        {
            Thread.CurrentThread.IsBackground = true;

            UpdateStateHandler(lblUpdate.Content.ToString(), false);

            try
            {
                await ModpackUpdater.UpdateModpack(updaterConfig, (msg, uis) => UpdateStateHandler(msg, uis), IsChecking);
            }
            catch (System.Net.WebException)
            {
                UpdateStateHandler("Failed to connect\nto the server");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                UpdateStateHandler("It seems something\nis broken...");
            }

            UpdateStateHandler(lblUpdate.Content.ToString());

        }

        private void UpdateStateHandler(string message, bool uiState)
        {
            lblUpdate.Content = message;
            setUiEnabled(uiState);
        }

        private void UpdateStateHandler(string message)
        {
            UpdateStateHandler(message, true);
        }

        public async Task UpdateProperties()
        {
            properties.JavaPath = txbJavaPath.Text;
            properties.Ram = txbRam.Text;
            properties.LatestVersion = cbVersions.Text;
            properties.Nickname = txbNick.Text;
            properties.JavaArgs = txbJavaArgs.Text;

            await properties.JsonWrite();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            UpdateProperties();
        }
    }
}
