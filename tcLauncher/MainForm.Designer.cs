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
            this.btnLocaleFiles = new System.Windows.Forms.Button();
            this.cbVersions = new System.Windows.Forms.ComboBox();
            this.lblNickname = new System.Windows.Forms.Label();
            this.txbNicknameEnter = new System.Windows.Forms.TextBox();
            this.Lv_Status = new System.Windows.Forms.Label();
            this.Pb_File = new System.Windows.Forms.ProgressBar();
            this.Pb_Progress = new System.Windows.Forms.ProgressBar();
            this.groupSettings = new System.Windows.Forms.Panel();
            this.chbAutoUpdate = new System.Windows.Forms.CheckBox();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.btnUpdatePack = new System.Windows.Forms.Button();
            this.txbJavaPath = new System.Windows.Forms.TextBox();
            this.lblRam = new System.Windows.Forms.Label();
            this.txbRam = new System.Windows.Forms.TextBox();
            this.btnJavaChange = new System.Windows.Forms.Button();
            this.lblJavaText = new System.Windows.Forms.Label();
            this.lblJavaArg = new System.Windows.Forms.Label();
            this.txbJavaArg = new System.Windows.Forms.TextBox();
            this.btnInstallVanilla = new System.Windows.Forms.Button();
            this.btnInstallFabric = new System.Windows.Forms.Button();
            this.groupMain.SuspendLayout();
            this.groupSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(9, 152);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(137, 23);
            this.btnLaunch.TabIndex = 0;
            this.btnLaunch.Text = "Start!";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // groupMain
            // 
            this.groupMain.Controls.Add(this.btnLocaleFiles);
            this.groupMain.Controls.Add(this.cbVersions);
            this.groupMain.Controls.Add(this.lblNickname);
            this.groupMain.Controls.Add(this.txbNicknameEnter);
            this.groupMain.Controls.Add(this.Lv_Status);
            this.groupMain.Controls.Add(this.Pb_File);
            this.groupMain.Controls.Add(this.Pb_Progress);
            this.groupMain.Controls.Add(this.btnLaunch);
            this.groupMain.Location = new System.Drawing.Point(118, 86);
            this.groupMain.Name = "groupMain";
            this.groupMain.Size = new System.Drawing.Size(196, 219);
            this.groupMain.TabIndex = 1;
            // 
            // btnLocaleFiles
            // 
            this.btnLocaleFiles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLocaleFiles.BackgroundImage")));
            this.btnLocaleFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLocaleFiles.Location = new System.Drawing.Point(152, 152);
            this.btnLocaleFiles.Name = "btnLocaleFiles";
            this.btnLocaleFiles.Size = new System.Drawing.Size(38, 23);
            this.btnLocaleFiles.TabIndex = 6;
            this.btnLocaleFiles.UseVisualStyleBackColor = true;
            this.btnLocaleFiles.Click += new System.EventHandler(this.btnLocaleFiles_Click);
            // 
            // cbVersions
            // 
            this.cbVersions.FormattingEnabled = true;
            this.cbVersions.Location = new System.Drawing.Point(9, 80);
            this.cbVersions.Name = "cbVersions";
            this.cbVersions.Size = new System.Drawing.Size(181, 23);
            this.cbVersions.TabIndex = 5;
            // 
            // lblNickname
            // 
            this.lblNickname.AutoSize = true;
            this.lblNickname.Location = new System.Drawing.Point(9, 46);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(34, 15);
            this.lblNickname.TabIndex = 4;
            this.lblNickname.Text = "Nick:";
            // 
            // txbNicknameEnter
            // 
            this.txbNicknameEnter.Location = new System.Drawing.Point(49, 43);
            this.txbNicknameEnter.Name = "txbNicknameEnter";
            this.txbNicknameEnter.Size = new System.Drawing.Size(141, 23);
            this.txbNicknameEnter.TabIndex = 3;
            // 
            // Lv_Status
            // 
            this.Lv_Status.AutoSize = true;
            this.Lv_Status.Location = new System.Drawing.Point(3, 178);
            this.Lv_Status.Name = "Lv_Status";
            this.Lv_Status.Size = new System.Drawing.Size(39, 15);
            this.Lv_Status.TabIndex = 2;
            this.Lv_Status.Text = "Ready";
            // 
            // Pb_File
            // 
            this.Pb_File.Location = new System.Drawing.Point(0, 196);
            this.Pb_File.Name = "Pb_File";
            this.Pb_File.Size = new System.Drawing.Size(196, 10);
            this.Pb_File.TabIndex = 2;
            // 
            // Pb_Progress
            // 
            this.Pb_Progress.Location = new System.Drawing.Point(0, 196);
            this.Pb_Progress.Name = "Pb_Progress";
            this.Pb_Progress.Size = new System.Drawing.Size(196, 23);
            this.Pb_Progress.TabIndex = 1;
            // 
            // groupSettings
            // 
            this.groupSettings.Controls.Add(this.chbAutoUpdate);
            this.groupSettings.Controls.Add(this.lblUpdate);
            this.groupSettings.Controls.Add(this.btnUpdatePack);
            this.groupSettings.Controls.Add(this.txbJavaPath);
            this.groupSettings.Controls.Add(this.lblRam);
            this.groupSettings.Controls.Add(this.txbRam);
            this.groupSettings.Controls.Add(this.btnJavaChange);
            this.groupSettings.Controls.Add(this.lblJavaText);
            this.groupSettings.Controls.Add(this.lblJavaArg);
            this.groupSettings.Controls.Add(this.txbJavaArg);
            this.groupSettings.Controls.Add(this.btnInstallVanilla);
            this.groupSettings.Controls.Add(this.btnInstallFabric);
            this.groupSettings.Location = new System.Drawing.Point(423, 33);
            this.groupSettings.Name = "groupSettings";
            this.groupSettings.Size = new System.Drawing.Size(341, 394);
            this.groupSettings.TabIndex = 2;
            // 
            // chbAutoUpdate
            // 
            this.chbAutoUpdate.AutoSize = true;
            this.chbAutoUpdate.Enabled = false;
            this.chbAutoUpdate.Location = new System.Drawing.Point(21, 372);
            this.chbAutoUpdate.Name = "chbAutoUpdate";
            this.chbAutoUpdate.Size = new System.Drawing.Size(90, 19);
            this.chbAutoUpdate.TabIndex = 12;
            this.chbAutoUpdate.Text = "AutoUpdate";
            this.chbAutoUpdate.UseVisualStyleBackColor = true;
            this.chbAutoUpdate.Visible = false;
            // 
            // lblUpdate
            // 
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.Location = new System.Drawing.Point(20, 316);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(140, 15);
            this.lblUpdate.TabIndex = 11;
            this.lblUpdate.Text = "У вас последняя версия!";
            // 
            // btnUpdatePack
            // 
            this.btnUpdatePack.Location = new System.Drawing.Point(20, 345);
            this.btnUpdatePack.Name = "btnUpdatePack";
            this.btnUpdatePack.Size = new System.Drawing.Size(109, 23);
            this.btnUpdatePack.TabIndex = 10;
            this.btnUpdatePack.Text = "Update modpack";
            this.btnUpdatePack.UseVisualStyleBackColor = true;
            this.btnUpdatePack.Click += new System.EventHandler(this.btnUpdatePack_Click);
            // 
            // txbJavaPath
            // 
            this.txbJavaPath.Location = new System.Drawing.Point(91, 25);
            this.txbJavaPath.Name = "txbJavaPath";
            this.txbJavaPath.Size = new System.Drawing.Size(236, 23);
            this.txbJavaPath.TabIndex = 9;
            this.txbJavaPath.Text = "Use default";
            // 
            // lblRam
            // 
            this.lblRam.AutoSize = true;
            this.lblRam.Location = new System.Drawing.Point(20, 141);
            this.lblRam.Name = "lblRam";
            this.lblRam.Size = new System.Drawing.Size(65, 15);
            this.lblRam.TabIndex = 8;
            this.lblRam.Text = "RAM (mb):";
            // 
            // txbRam
            // 
            this.txbRam.Location = new System.Drawing.Point(91, 138);
            this.txbRam.Name = "txbRam";
            this.txbRam.Size = new System.Drawing.Size(236, 23);
            this.txbRam.TabIndex = 7;
            this.txbRam.Text = "2048";
            // 
            // btnJavaChange
            // 
            this.btnJavaChange.Location = new System.Drawing.Point(252, 52);
            this.btnJavaChange.Name = "btnJavaChange";
            this.btnJavaChange.Size = new System.Drawing.Size(75, 23);
            this.btnJavaChange.TabIndex = 6;
            this.btnJavaChange.Text = "Change";
            this.btnJavaChange.UseVisualStyleBackColor = true;
            this.btnJavaChange.Click += new System.EventHandler(this.btnJavaChange_Click);
            // 
            // lblJavaText
            // 
            this.lblJavaText.AutoSize = true;
            this.lblJavaText.Location = new System.Drawing.Point(20, 32);
            this.lblJavaText.Name = "lblJavaText";
            this.lblJavaText.Size = new System.Drawing.Size(59, 15);
            this.lblJavaText.TabIndex = 4;
            this.lblJavaText.Text = "Java Path:";
            // 
            // lblJavaArg
            // 
            this.lblJavaArg.AutoSize = true;
            this.lblJavaArg.Location = new System.Drawing.Point(20, 99);
            this.lblJavaArg.Name = "lblJavaArg";
            this.lblJavaArg.Size = new System.Drawing.Size(59, 15);
            this.lblJavaArg.TabIndex = 3;
            this.lblJavaArg.Text = "Java Args:";
            // 
            // txbJavaArg
            // 
            this.txbJavaArg.Location = new System.Drawing.Point(91, 96);
            this.txbJavaArg.Name = "txbJavaArg";
            this.txbJavaArg.Size = new System.Drawing.Size(235, 23);
            this.txbJavaArg.TabIndex = 2;
            // 
            // btnInstallVanilla
            // 
            this.btnInstallVanilla.Location = new System.Drawing.Point(217, 345);
            this.btnInstallVanilla.Name = "btnInstallVanilla";
            this.btnInstallVanilla.Size = new System.Drawing.Size(109, 23);
            this.btnInstallVanilla.TabIndex = 1;
            this.btnInstallVanilla.Text = "Install Vanilla";
            this.btnInstallVanilla.UseVisualStyleBackColor = true;
            this.btnInstallVanilla.Click += new System.EventHandler(this.btnInstallVanilla_Click);
            // 
            // btnInstallFabric
            // 
            this.btnInstallFabric.Location = new System.Drawing.Point(217, 316);
            this.btnInstallFabric.Name = "btnInstallFabric";
            this.btnInstallFabric.Size = new System.Drawing.Size(109, 23);
            this.btnInstallFabric.TabIndex = 0;
            this.btnInstallFabric.Text = "Install Fabric";
            this.btnInstallFabric.UseVisualStyleBackColor = true;
            this.btnInstallFabric.Click += new System.EventHandler(this.btnInstallFabric_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupSettings);
            this.Controls.Add(this.groupMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "tcLauncher";
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
    }
}