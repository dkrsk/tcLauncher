using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Downloader;
using CmlLib.Core.Version;

using DnKR.mineUpdater;

using System.ComponentModel;
using System.Diagnostics;


namespace DnKR.tcLauncher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        MSession session;
        CMLauncher launcher;
        
        bool isFabricInstalled = false;
        UserProperties? properties;

        UpdaterConfig updaterConfig = new(
            gamePath.ToString(),
            "ftp://ip",
            "dir",
            new System.Net.NetworkCredential("user", "pass")
        );

        GameLog logForm;
        bool devOps = false;

        public static MinecraftPath gamePath = new MinecraftPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.tclaucner");


        private async void MainForm_Shown(object sender, EventArgs e)
        {
            gamePath = new MinecraftPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/.tclaucner");

            new Thread(CheckUpdate).Start();

            properties = new();
            await properties.JsonRead();

            this.launcher = new CMLauncher(gamePath);
            launcher.FileChanged += Launcher_FileChanged;
            launcher.ProgressChanged += Launcher_ProgressChanged;
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;
            launcher.FileDownloader = new AsyncParallelDownloader();


            await refreshVersions();


            txbNicknameEnter.Text = properties.nickname;
            txbJavaPath.Text = properties.javaPath;
            txbJavaArg.Text = properties.javaArgs;
            txbRam.Text = properties.ram;
            devOps = properties.devOps;
            //chbAutoUpdate.Checked = properties.autoUpdate;
        }

        private async Task refreshVersions()
        {
            cbVersions.Items.Clear();

            var versions = Directory.EnumerateDirectories(gamePath.Versions);
            if (versions.Count() == 0) return;
            //var versions = await launcher.GetAllVersionsAsync();
            foreach (string ver in versions)
            {
                string[] v = ver.Split(Path.DirectorySeparatorChar);
                string verName = v[v.Length - 1];
                if (verName.Contains("fabric"))
                    isFabricInstalled = true;
                cbVersions.Items.Add(verName);
            }
            if (string.IsNullOrWhiteSpace(properties.latestVersion))
                cbVersions.Text = cbVersions.Items[cbVersions.Items.Count - 1].ToString();
            else
                cbVersions.Text = properties.latestVersion;

        }

        private async void btnLaunch_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txbNicknameEnter.Text))
            {
                MessageBox.Show("Сначала впишите ник!");
                return;
            }
            this.session = MSession.GetOfflineSession(txbNicknameEnter.Text);

            if (cbVersions.Text == "")
            {
                MessageBox.Show("Сначала выберете версию!");
                return;
            }


            setUiEnabled(false);

            try
            {
                MLaunchOption launchOption = new MLaunchOption
                {
                    MaximumRamMb = int.Parse(txbRam.Text),
                    Session = this.session,

                    FullScreen = false
                };

                if (File.Exists(txbJavaPath.Text))
                    launchOption.JavaPath = txbJavaPath.Text;
                else if (!string.IsNullOrWhiteSpace(txbJavaPath.Text) && txbJavaPath.Text != "Use default")
                {
                    MessageBox.Show("Неверное расположение java");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(txbJavaArg.Text))
                    launchOption.JVMArguments = txbJavaArg.Text.Split(' ');

                var process = await launcher.CreateProcessAsync(cbVersions.Text, launchOption);

                UpdateProperties();

                StartProcess(process);
            }
            catch (Win32Exception wex) // java exception
            {
                MessageBox.Show(wex + "\n\nIt seems your java setting has problem");
            }
            catch (Exception ex) // all exception
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (logForm != null)
                    logForm.Close();

                if (devOps)
                {
                    logForm = new GameLog();
                    
                    logForm.Show();
                }

                setUiEnabled(true);
                Lv_Status.Text = "Ready";
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

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
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
            File.AppendAllText(gamePath.ToString() + "\\logs.txt", msg+'\n');
            GameLog.AddLog(msg);
        }

        private void txbRam_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private async void btnInstallFabric_Click(object sender, EventArgs e)
        {
            try
            {
                MVersion ver = await FabricInstaller.installFabric(gamePath);

                await launcher.CheckAndDownloadAsync(ver);
                MessageBox.Show("Успех!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnJavaChange_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\Program Files";
            dialog.Filter = "Exe Files |*.exe|All Files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.Multiselect = false;

            dialog.ShowDialog();
            //javaPath = dialog.FileName;
            txbJavaPath.Text = dialog.FileName;
            
        }

        private readonly int uiThreadId = Thread.CurrentThread.ManagedThreadId;
        private void Launcher_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
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

        private void btnInstallVanilla_Click(object sender, EventArgs e)
        {
            setUiEnabled(false);
            Form form = new InstallVanillaForm(launcher);
            form.FormClosing += delegate { setUiEnabled(true); refreshVersions(); };
            form.Show();

        }

        private void btnUpdatePack_Click(object? sender, EventArgs? e)
        {
            //if (!isFabricInstalled)
            //{
            //    MessageBox.Show("Сначала установи фабрик!");
            //    return;
            //}

            new Thread(updateThread).Start();

        }

        private void updateThread()
        {
            lblUpdate.Invoke((MethodInvoker)delegate {

                lblUpdate.Text = "Обновляю...";
                setUiEnabled(false);
            });
            

            try
            {
                modpackUpdater updater = new(updaterConfig);

                string[] RemoteNames = updater.GetNames();
                string PackageName = RemoteNames[RemoteNames.Length - 1];

                Int16 RemoteVersion = Convert.ToInt16(PackageName.Substring(0, PackageName.Length - 4));

                Int16 CurrentVersion;

                try
                {
                    using (StreamReader fs = new StreamReader(gamePath.ToString() + "\\info"))
                        CurrentVersion = Convert.ToInt16(fs.ReadToEnd());
                }
                catch (FileNotFoundException)
                {
                    CurrentVersion = 0;
                }

                if (RemoteVersion > CurrentVersion)
                {
                    updater.DownloadFile(PackageName);

                    using (StreamWriter fw = new StreamWriter(gamePath.ToString() + "\\info", false))
                    {
                        fw.Write(RemoteVersion);
                    }

                    updater.ExtractFile(PackageName);

                    if (File.Exists(gamePath + "\\tmpUpdateLauncher"))
                    {
                        Process currentProc = Process.GetCurrentProcess();
                        Process.Start("tcUpdater.exe", $"{gamePath}\\tmpUpdateLauncher {currentProc.MainModule.FileName} {currentProc.Id}");
                    }

                    MessageBox.Show($"Успех! Обновлен до build{RemoteVersion}");
                }

                lblUpdate.Invoke((MethodInvoker)delegate {

                    lblUpdate.Text = "У вас последняя версия!";
                });

            }
            catch (System.Net.WebException)
            {
                lblUpdate.Invoke((MethodInvoker)delegate {

                    lblUpdate.Text = "Не удалось\nсоединиться с сервером";
                });
            }

            lblUpdate.Invoke((MethodInvoker)delegate {

                setUiEnabled(true);
            });
            
        }

        private void CheckUpdate()
        {
            lblUpdate.Invoke((MethodInvoker)delegate {

                lblUpdate.Text = "Обновляю...";

            });

            try
            {
                modpackUpdater updater = new(updaterConfig);

                string[] RemoteNames = updater.GetNames();
                string PackageName = RemoteNames[RemoteNames.Length - 1];

                Int16 RemoteVersion = Convert.ToInt16(PackageName.Substring(0, PackageName.Length - 4));

                Int16 CurrentVersion;

                try
                {
                    using (StreamReader fs = new StreamReader(gamePath.ToString() + "\\info"))
                        CurrentVersion = Convert.ToInt16(fs.ReadToEnd());
                }
                catch (FileNotFoundException)
                {
                    CurrentVersion = 0;
                }
                if (RemoteVersion > CurrentVersion)
                    lblUpdate.Invoke((MethodInvoker)delegate {

                        lblUpdate.Text = "Найдена новая версия!";
                    });
                else
                {
                    lblUpdate.Invoke((MethodInvoker)delegate {

                        lblUpdate.Text = "У вас последняя версия!";
                    });
                }

            }
            catch (System.Net.WebException)
            {
                lblUpdate.Invoke((MethodInvoker)delegate {

                    lblUpdate.Text = "Не удалось\nсоединиться с сервером";
                });
            }
        }


        public void UpdateProperties()
        {
            properties.javaPath = txbJavaPath.Text;
            properties.ram = txbRam.Text;
            properties.latestVersion = cbVersions.Text;
            properties.nickname = txbNicknameEnter.Text;
            properties.javaArgs = txbJavaArg.Text;
            properties.autoUpdate = chbAutoUpdate.Checked;
            properties.JsonWrite();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateProperties();
        }

        private void btnLocaleFiles_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe",gamePath.ToString());
        }
    }
}

