namespace DnKR.tcLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnLaunch = new System.Windows.Forms.Button();
            this.groupMain = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.Pb_File = new System.Windows.Forms.ProgressBar();
            this.btnLocaleFiles = new System.Windows.Forms.Button();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.lblNickname = new System.Windows.Forms.Label();
            this.txbNicknameEnter = new System.Windows.Forms.TextBox();
            this.Lv_Status = new System.Windows.Forms.Label();
            this.Pb_Progress = new System.Windows.Forms.ProgressBar();
            this.groupSettings = new System.Windows.Forms.Panel();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.lblRam = new System.Windows.Forms.Label();
            this.btnJavaChange = new System.Windows.Forms.Button();
            this.lblJavaText = new System.Windows.Forms.Label();
            this.txbRam = new System.Windows.Forms.TextBox();
            this.lblJavaArg = new System.Windows.Forms.Label();
            this.btnUpdatePack = new System.Windows.Forms.Button();
            this.chbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.txbJavaArg = new System.Windows.Forms.TextBox();
            this.btnInstallVanilla = new System.Windows.Forms.Button();
            this.btnInstallFabric = new System.Windows.Forms.Button();
            this.txbJavaPath = new System.Windows.Forms.TextBox();
            this.btnBkg = new System.Windows.Forms.Button();
            this.btnBkgClear = new System.Windows.Forms.Button();
            this.btnInstallQuilt = new System.Windows.Forms.Button();
            this.groupMain.SuspendLayout();
            this.groupSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLaunch
            // 
            resources.ApplyResources(this.btnLaunch, "btnLaunch");
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // groupMain
            // 
            this.groupMain.BackColor = System.Drawing.Color.Transparent;
            this.groupMain.BackgroundImage = global::DnKR.tcLauncher.Properties.Resources.tcGroup_bg;
            resources.ApplyResources(this.groupMain, "groupMain");
            this.groupMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupMain.Controls.Add(this.label1);
            this.groupMain.Controls.Add(this.Pb_File);
            this.groupMain.Controls.Add(this.btnLocaleFiles);
            this.groupMain.Controls.Add(this.cbVersions);
            this.groupMain.Controls.Add(this.lblNickname);
            this.groupMain.Controls.Add(this.txbNicknameEnter);
            this.groupMain.Controls.Add(this.Lv_Status);
            this.groupMain.Controls.Add(this.Pb_Progress);
            this.groupMain.Controls.Add(this.btnLaunch);
            this.groupMain.Name = "groupMain";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Pb_File
            // 
            resources.ApplyResources(this.Pb_File, "Pb_File");
            this.Pb_File.Name = "Pb_File";
            // 
            // btnLocaleFiles
            // 
            this.btnLocaleFiles.BackgroundImage = global::DnKR.tcLauncher.Properties.Resources.dirIco;
            resources.ApplyResources(this.btnLocaleFiles, "btnLocaleFiles");
            this.btnLocaleFiles.Name = "btnLocaleFiles";
            this.btnLocaleFiles.UseVisualStyleBackColor = true;
            this.btnLocaleFiles.Click += new System.EventHandler(this.btnLocaleFiles_Click);
            // 
            // cbVersions
            // 
            this.cbVersions.FormattingEnabled = true;
            resources.ApplyResources(this.cbVersions, "cbVersions");
            this.cbVersions.Name = "cbVersions";
            // 
            // lblNickname
            // 
            resources.ApplyResources(this.lblNickname, "lblNickname");
            this.lblNickname.Name = "lblNickname";
            // 
            // txbNicknameEnter
            // 
            resources.ApplyResources(this.txbNicknameEnter, "txbNicknameEnter");
            this.txbNicknameEnter.Name = "txbNicknameEnter";
            // 
            // Lv_Status
            // 
            resources.ApplyResources(this.Lv_Status, "Lv_Status");
            this.Lv_Status.Name = "Lv_Status";
            // 
            // Pb_Progress
            // 
            resources.ApplyResources(this.Pb_Progress, "Pb_Progress");
            this.Pb_Progress.Name = "Pb_Progress";
            // 
            // groupSettings
            // 
            this.groupSettings.BackColor = System.Drawing.Color.Transparent;
            this.groupSettings.BackgroundImage = global::DnKR.tcLauncher.Properties.Resources.tcGroup_bg;
            this.groupSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupSettings.Controls.Add(this.btnInstallQuilt);
            this.groupSettings.Controls.Add(this.lblUpdate);
            this.groupSettings.Controls.Add(this.lblRam);
            this.groupSettings.Controls.Add(this.btnJavaChange);
            this.groupSettings.Controls.Add(this.lblJavaText);
            this.groupSettings.Controls.Add(this.txbRam);
            this.groupSettings.Controls.Add(this.lblJavaArg);
            this.groupSettings.Controls.Add(this.btnUpdatePack);
            this.groupSettings.Controls.Add(this.chbAutoUpdate);
            this.groupSettings.Controls.Add(this.txbJavaArg);
            this.groupSettings.Controls.Add(this.btnInstallVanilla);
            this.groupSettings.Controls.Add(this.btnInstallFabric);
            this.groupSettings.Controls.Add(this.txbJavaPath);
            resources.ApplyResources(this.groupSettings, "groupSettings");
            this.groupSettings.Name = "groupSettings";
            // 
            // lblUpdate
            // 
            resources.ApplyResources(this.lblUpdate, "lblUpdate");
            this.lblUpdate.Name = "lblUpdate";
            // 
            // lblRam
            // 
            resources.ApplyResources(this.lblRam, "lblRam");
            this.lblRam.Name = "lblRam";
            // 
            // btnJavaChange
            // 
            resources.ApplyResources(this.btnJavaChange, "btnJavaChange");
            this.btnJavaChange.Name = "btnJavaChange";
            this.btnJavaChange.UseVisualStyleBackColor = true;
            this.btnJavaChange.Click += new System.EventHandler(this.btnJavaChange_Click);
            // 
            // lblJavaText
            // 
            resources.ApplyResources(this.lblJavaText, "lblJavaText");
            this.lblJavaText.Name = "lblJavaText";
            // 
            // txbRam
            // 
            resources.ApplyResources(this.txbRam, "txbRam");
            this.txbRam.Name = "txbRam";
            this.txbRam.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txbRam_KeyPress);
            // 
            // lblJavaArg
            // 
            resources.ApplyResources(this.lblJavaArg, "lblJavaArg");
            this.lblJavaArg.Name = "lblJavaArg";
            // 
            // btnUpdatePack
            // 
            resources.ApplyResources(this.btnUpdatePack, "btnUpdatePack");
            this.btnUpdatePack.Name = "btnUpdatePack";
            this.btnUpdatePack.UseVisualStyleBackColor = true;
            this.btnUpdatePack.Click += new System.EventHandler(this.btnUpdatePack_Click);
            // 
            // chbAutoUpdate
            // 
            resources.ApplyResources(this.chbAutoUpdate, "chbAutoUpdate");
            this.chbAutoUpdate.Name = "chbAutoUpdate";
            this.chbAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // txbJavaArg
            // 
            resources.ApplyResources(this.txbJavaArg, "txbJavaArg");
            this.txbJavaArg.Name = "txbJavaArg";
            // 
            // btnInstallVanilla
            // 
            resources.ApplyResources(this.btnInstallVanilla, "btnInstallVanilla");
            this.btnInstallVanilla.Name = "btnInstallVanilla";
            this.btnInstallVanilla.UseVisualStyleBackColor = true;
            this.btnInstallVanilla.Click += new System.EventHandler(this.btnInstallVanilla_Click);
            // 
            // btnInstallFabric
            // 
            resources.ApplyResources(this.btnInstallFabric, "btnInstallFabric");
            this.btnInstallFabric.Name = "btnInstallFabric";
            this.btnInstallFabric.UseVisualStyleBackColor = true;
            this.btnInstallFabric.Click += new System.EventHandler(this.btnInstallFabric_Click);
            // 
            // txbJavaPath
            // 
            resources.ApplyResources(this.txbJavaPath, "txbJavaPath");
            this.txbJavaPath.Name = "txbJavaPath";
            // 
            // btnBkg
            // 
            this.btnBkg.BackgroundImage = global::DnKR.tcLauncher.Properties.Resources.bkgIco;
            resources.ApplyResources(this.btnBkg, "btnBkg");
            this.btnBkg.Name = "btnBkg";
            this.btnBkg.UseVisualStyleBackColor = true;
            this.btnBkg.Click += new System.EventHandler(this.btnBkg_Click);
            // 
            // btnBkgClear
            // 
            this.btnBkgClear.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnBkgClear, "btnBkgClear");
            this.btnBkgClear.FlatAppearance.BorderSize = 0;
            this.btnBkgClear.ForeColor = System.Drawing.Color.DarkRed;
            this.btnBkgClear.Name = "btnBkgClear";
            this.btnBkgClear.UseVisualStyleBackColor = false;
            this.btnBkgClear.Click += new System.EventHandler(this.btnBkgClear_Click);
            // 
            // btnInstallQuilt
            // 
            resources.ApplyResources(this.btnInstallQuilt, "btnInstallQuilt");
            this.btnInstallQuilt.Name = "btnInstallQuilt";
            this.btnInstallQuilt.UseVisualStyleBackColor = true;
            this.btnInstallQuilt.Click += new System.EventHandler(this.btnInstallQuilt_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::DnKR.tcLauncher.Properties.Resources.tclaucher_bg;
            this.Controls.Add(this.btnBkgClear);
            this.Controls.Add(this.btnBkg);
            this.Controls.Add(this.groupSettings);
            this.Controls.Add(this.groupMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupMain.ResumeLayout(false);
            this.groupMain.PerformLayout();
            this.groupSettings.ResumeLayout(false);
            this.groupSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Button btnLaunch;
        private Panel groupMain;
        private ProgressBar Pb_Progress;
        private ProgressBar Pb_File;
        private TextBox txbNicknameEnter;
        private Label Lv_Status;
        private Label lblNickname;
        private ComboBox cbVersions;
        private Panel groupSettings;
        private Button btnInstallFabric;
        private Button btnInstallVanilla;
        private TextBox txbJavaArg;
        private Label lblJavaArg;
        private Label lblRam;
        private TextBox txbRam;
        private Button btnJavaChange;
        private Label lblJavaText;
        private TextBox txbJavaPath;
        private Button btnUpdatePack;
        private Label lblUpdate;
        private CheckBox chbAutoUpdate;
        private Button btnLocaleFiles;
        private Button btnBkg;
        private Button btnBkgClear;
        private Label label1;
        private Button btnInstallQuilt;
    }
}