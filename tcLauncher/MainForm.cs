using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;

using DnKR.tcLauncher.tcUpdater;

using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Concurrent;


namespace DnKR.tcLauncher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            properties = new();
            new Task(async () => await properties.JsonRead()).Start();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            InitializeComponent();
        }

        MSession session;
        CMLauncher launcher;

        UserProperties properties;

        UpdaterConfig updaterConfig = config.GetUpdConfig();


        static ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();

        public static MinecraftPath gamePath = new MinecraftPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.tclaucner");


        private async void MainForm_Shown(object sender, EventArgs e)
        {

            if (properties.BkgPath != null && File.Exists(properties.BkgPath))
                this.BackgroundImage = Image.FromFile(properties.BkgPath);

            this.launcher = new CMLauncher(gamePath);
            launcher.FileChanged += Launcher_FileChanged;
            launcher.ProgressChanged += Launcher_ProgressChanged;
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            launcher.FileDownloader = new AsyncParallelDownloader();

            await UpdateModpack(true);
            //new Thread(() => updateThread(true)).Start();


            txbNicknameEnter.Text = properties.Nickname;
            txbJavaPath.Text = properties.JavaPath;
            txbJavaArg.Text = properties.JavaArgs;
            txbRam.Text = properties.Ram;

            await refreshVersions();

        }

        private async Task refreshVersions()
        {
            cbVersions.Items.Clear();

            var versions = Directory.EnumerateDirectories(gamePath.Versions);
            if (!versions.Any()) return;

            foreach (string ver in versions)
            {
                string[] v = ver.Split(Path.DirectorySeparatorChar);
                string verName = v[^1];

                cbVersions.Items.Add(verName);
            }
            if (string.IsNullOrWhiteSpace(properties.LatestVersion))
                cbVersions.Text = cbVersions.Items[^1].ToString();
            else
                cbVersions.Text = properties.LatestVersion;
        }

        private async void btnLaunch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txbNicknameEnter.Text))
            {
                MessageBox.Show("Enter the nickname first!");
                return;
            }
            this.session = MSession.GetOfflineSession(txbNicknameEnter.Text);

            if (cbVersions.Text == "")
            {
                MessageBox.Show("Select the version first!");
                return;
            }


            setUiEnabled(false);

            try
            {

                MLaunchOption launchOption = new MLaunchOption
                {
                    Session = this.session,
                    FullScreen = false
                };

                if (File.Exists(txbJavaPath.Text))
                    launchOption.JavaPath = txbJavaPath.Text;
                else if (!string.IsNullOrWhiteSpace(txbJavaPath.Text))
                {
                    MessageBox.Show("Incorrent location to java");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txbJavaArg.Text))
                {
                    launchOption.JVMArguments = txbJavaArg.Text.Split(' ');
                }

                if (!string.IsNullOrWhiteSpace(txbRam.Text))
                    launchOption.MaximumRamMb = int.Parse(txbRam.Text);

                var process = await launcher.CreateProcessAsync(cbVersions.Text, launchOption);

                UpdateProperties();

                Lv_Status.Text = "Playing...";
                btnLaunch.Enabled = false;

                File.Delete(Path.Combine(gamePath.ToString(), "\\logs.txt"));
                tmLog.Enabled = true;

                StartProcess(process);
            }

            catch (Win32Exception wex) // java exception
            {
                MessageBox.Show(wex + "\n\nIt seems your java setting has problem");
            }
            catch (Exception ex) // all exception
            {
                Lv_Status.Text = "Ready";
                btnLaunch.Enabled = true;
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
            groupMain.Enabled = value;
            groupSettings.Enabled = value;
        }

        private void StartProcess(Process process)
        {
            Output(process.StartInfo.Arguments);

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.Exited += Process_Exited;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private void Process_Exited(object? sender, EventArgs e)
        {
            tmLog.Enabled = false;

            Lv_Status.Invoke((MethodInvoker)delegate {
                Lv_Status.Text = "Ready";
            });

            btnLaunch.Invoke((MethodInvoker)delegate {
                btnLaunch.Enabled = true;
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

        private void txbRam_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnInstallVanilla_Click(object sender, EventArgs e)
        {
            setUiEnabled(false);
            Form form = new InstallVanillaForm(launcher);
            form.FormClosing += async delegate { setUiEnabled(true); await refreshVersions(); };
            form.Show();

        }

        private async void btnInstallFabric_Click(object sender, EventArgs e)
        {
            setUiEnabled(false);
            InstallFabricForm form = new(launcher);
            form.FormClosing += async delegate { setUiEnabled(true); await refreshVersions(); };
            form.Show();

        }

        private void btnInstallQuilt_Click(object sender, EventArgs e)
        {
            setUiEnabled(false);
            InstallQuiltForm form = new(launcher);
            form.FormClosing += async delegate { setUiEnabled(true); await refreshVersions(); };
            form.Show();
        }

        private void btnJavaChange_Click(object sender, EventArgs e)
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
            catch (ArgumentException)
            {

            }
            
        }

        private readonly int uiThreadId = Thread.CurrentThread.ManagedThreadId;
        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                Debug.WriteLine(e);
            }
            Pb_Progress.Maximum = 100;
            Pb_Progress.Value = e.ProgressPercentage;
        }

        private void Launcher_FileChanged(DownloadFileChangedEventArgs e)
        {
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                Debug.WriteLine(e);
            }
            Pb_File.Maximum = e.TotalFileCount;
            Pb_File.Value = e.ProgressedFileCount;
            Lv_Status.Text = $"{e.FileKind} : {e.FileName} ({e.ProgressedFileCount}/{e.TotalFileCount})";
        }

        private async void btnUpdatePack_Click(object? sender, EventArgs? e)
        {
            //new Thread(() => updateThread(false)).Start();
            await UpdateModpack(false);
        }

        private void btnLocaleFiles_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", gamePath.ToString());
        }

        private void btnBkg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE"); ;
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            dialog.FilterIndex = 0;
            dialog.Multiselect = false;

            dialog.ShowDialog();

            try
            {
                this.BackgroundImage = Image.FromFile(dialog.FileName);
                properties.BkgPath = dialog.FileName;
            }
            catch (ArgumentException)
            {
                
            }

        }

        private void btnBkgClear_Click(object sender, EventArgs e)
        {
            this.BackgroundImage = Properties.Resources.tclaucher_bg;
            properties.BkgPath = string.Empty;
        }

        private async Task UpdateModpack(bool IsChecking)
        {
            Thread.CurrentThread.IsBackground = true;

            UpdateStateHandler(lblUpdate.Text, false);

            try
            {
                ModpackUpdater.UpdateModpack(updaterConfig, (msg, uis) => UpdateStateHandler(msg, uis), IsChecking);
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

            UpdateStateHandler(lblUpdate.Text);

        }

        private void UpdateStateHandler(string message, bool uiState = true)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblUpdate.Text = message;
                setUiEnabled(uiState);
            });
        }
        public async void UpdateProperties()
        {
            properties.JavaPath = txbJavaPath.Text;
            properties.Ram = txbRam.Text;
            properties.LatestVersion = cbVersions.Text;
            properties.Nickname = txbNicknameEnter.Text;
            properties.JavaArgs = txbJavaArg.Text;

            await properties.JsonWrite();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateProperties();
        }
    }
}

