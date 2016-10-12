// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
namespace MS_PCCRC_TestTool
{
    partial class ImportDataForm
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
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.filePathBrowseButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.hashAlgorithmComboBox = new System.Windows.Forms.ComboBox();
            this.HashAlgorithmLabel = new System.Windows.Forms.Label();
            this.transportLabel = new System.Windows.Forms.Label();
            this.transportComboBox = new System.Windows.Forms.ComboBox();
            this.branchCacheVersionLabel = new System.Windows.Forms.Label();
            this.branchCacheVersionComboBox = new System.Windows.Forms.ComboBox();
            this.serverSecretTextBox = new System.Windows.Forms.TextBox();
            this.operationModeLabel = new System.Windows.Forms.Label();
            this.operationModeComboBox = new System.Windows.Forms.ComboBox();
            this.serverSecretLabel = new System.Windows.Forms.Label();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.topSplitContainer = new System.Windows.Forms.SplitContainer();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.saveLogButton = new System.Windows.Forms.Button();
            this.domainNameLabel = new System.Windows.Forms.Label();
            this.domainNameTextBox = new System.Windows.Forms.TextBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.userPasswordLabel = new System.Windows.Forms.Label();
            this.userPasswordTextBox = new System.Windows.Forms.TextBox();
            this.smb2AuthenticationComboBox = new System.Windows.Forms.ComboBox();
            this.smb2AuthenticationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.topSplitContainer)).BeginInit();
            this.topSplitContainer.Panel1.SuspendLayout();
            this.topSplitContainer.Panel2.SuspendLayout();
            this.topSplitContainer.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Location = new System.Drawing.Point(75, 138);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(309, 20);
            this.filePathTextBox.TabIndex = 6;
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(21, 142);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(48, 13);
            this.filePathLabel.TabIndex = 6;
            this.filePathLabel.Text = "File Path";
            // 
            // filePathBrowseButton
            // 
            this.filePathBrowseButton.Location = new System.Drawing.Point(390, 136);
            this.filePathBrowseButton.Name = "filePathBrowseButton";
            this.filePathBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.filePathBrowseButton.TabIndex = 0;
            this.filePathBrowseButton.Text = "Browse...";
            this.filePathBrowseButton.UseVisualStyleBackColor = true;
            this.filePathBrowseButton.Click += new System.EventHandler(this.filePathBrowseButton_Click);
            // 
            // executeButton
            // 
            this.executeButton.Location = new System.Drawing.Point(510, 137);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(75, 23);
            this.executeButton.TabIndex = 4;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // hashAlgorithmComboBox
            // 
            this.hashAlgorithmComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hashAlgorithmComboBox.FormattingEnabled = true;
            this.hashAlgorithmComboBox.Location = new System.Drawing.Point(370, 61);
            this.hashAlgorithmComboBox.Name = "hashAlgorithmComboBox";
            this.hashAlgorithmComboBox.Size = new System.Drawing.Size(100, 21);
            this.hashAlgorithmComboBox.TabIndex = 2;
            // 
            // HashAlgorithmLabel
            // 
            this.HashAlgorithmLabel.AutoSize = true;
            this.HashAlgorithmLabel.Location = new System.Drawing.Point(286, 69);
            this.HashAlgorithmLabel.Name = "HashAlgorithmLabel";
            this.HashAlgorithmLabel.Size = new System.Drawing.Size(78, 13);
            this.HashAlgorithmLabel.TabIndex = 7;
            this.HashAlgorithmLabel.Text = "Hash Algorithm";
            // 
            // transportLabel
            // 
            this.transportLabel.AutoSize = true;
            this.transportLabel.Location = new System.Drawing.Point(23, 95);
            this.transportLabel.Name = "transportLabel";
            this.transportLabel.Size = new System.Drawing.Size(52, 13);
            this.transportLabel.TabIndex = 14;
            this.transportLabel.Text = "Transport";
            // 
            // transportComboBox
            // 
            this.transportComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transportComboBox.FormattingEnabled = true;
            this.transportComboBox.Location = new System.Drawing.Point(142, 95);
            this.transportComboBox.Name = "transportComboBox";
            this.transportComboBox.Size = new System.Drawing.Size(127, 21);
            this.transportComboBox.TabIndex = 15;
            this.transportComboBox.SelectedIndexChanged += new System.EventHandler(this.transportComboBox_SelectedIndexChanged);
            // 
            // branchCacheVersionLabel
            // 
            this.branchCacheVersionLabel.AutoSize = true;
            this.branchCacheVersionLabel.Location = new System.Drawing.Point(23, 40);
            this.branchCacheVersionLabel.Name = "branchCacheVersionLabel";
            this.branchCacheVersionLabel.Size = new System.Drawing.Size(113, 13);
            this.branchCacheVersionLabel.TabIndex = 16;
            this.branchCacheVersionLabel.Text = "Branch Cache Version";
            // 
            // branchCacheVersionComboBox
            // 
            this.branchCacheVersionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.branchCacheVersionComboBox.FormattingEnabled = true;
            this.branchCacheVersionComboBox.Location = new System.Drawing.Point(142, 37);
            this.branchCacheVersionComboBox.Name = "branchCacheVersionComboBox";
            this.branchCacheVersionComboBox.Size = new System.Drawing.Size(127, 21);
            this.branchCacheVersionComboBox.TabIndex = 17;
            this.branchCacheVersionComboBox.SelectedIndexChanged += new System.EventHandler(this.branchCacheVersionComboBox_SelectedIndexChanged);
            // 
            // serverSecretTextBox
            // 
            this.serverSecretTextBox.Location = new System.Drawing.Point(371, 34);
            this.serverSecretTextBox.Name = "serverSecretTextBox";
            this.serverSecretTextBox.Size = new System.Drawing.Size(100, 20);
            this.serverSecretTextBox.TabIndex = 20;
            this.serverSecretTextBox.Text = "server secret";
            // 
            // operationModeLabel
            // 
            this.operationModeLabel.AutoSize = true;
            this.operationModeLabel.Location = new System.Drawing.Point(25, 67);
            this.operationModeLabel.Name = "operationModeLabel";
            this.operationModeLabel.Size = new System.Drawing.Size(83, 13);
            this.operationModeLabel.TabIndex = 21;
            this.operationModeLabel.Text = "Operation Mode";
            // 
            // operationModeComboBox
            // 
            this.operationModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operationModeComboBox.FormattingEnabled = true;
            this.operationModeComboBox.Location = new System.Drawing.Point(142, 64);
            this.operationModeComboBox.Name = "operationModeComboBox";
            this.operationModeComboBox.Size = new System.Drawing.Size(127, 21);
            this.operationModeComboBox.TabIndex = 22;
            this.operationModeComboBox.SelectedIndexChanged += new System.EventHandler(this.operationModeComboBox_SelectedIndexChanged);
            // 
            // serverSecretLabel
            // 
            this.serverSecretLabel.AutoSize = true;
            this.serverSecretLabel.Location = new System.Drawing.Point(286, 37);
            this.serverSecretLabel.Name = "serverSecretLabel";
            this.serverSecretLabel.Size = new System.Drawing.Size(72, 13);
            this.serverSecretLabel.TabIndex = 23;
            this.serverSecretLabel.Text = "Server Secret";
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logRichTextBox.Location = new System.Drawing.Point(10, 0);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.ReadOnly = true;
            this.logRichTextBox.Size = new System.Drawing.Size(757, 190);
            this.logRichTextBox.TabIndex = 24;
            this.logRichTextBox.Text = "";
            // 
            // topSplitContainer
            // 
            this.topSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.topSplitContainer.IsSplitterFixed = true;
            this.topSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.topSplitContainer.Name = "topSplitContainer";
            this.topSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // topSplitContainer.Panel1
            // 
            this.topSplitContainer.Panel1.Controls.Add(this.settingsPanel);
            // 
            // topSplitContainer.Panel2
            // 
            this.topSplitContainer.Panel2.Controls.Add(this.logRichTextBox);
            this.topSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.topSplitContainer.Size = new System.Drawing.Size(777, 441);
            this.topSplitContainer.SplitterDistance = 237;
            this.topSplitContainer.TabIndex = 25;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.settingsPanel.Controls.Add(this.smb2AuthenticationComboBox);
            this.settingsPanel.Controls.Add(this.smb2AuthenticationLabel);
            this.settingsPanel.Controls.Add(this.userPasswordLabel);
            this.settingsPanel.Controls.Add(this.userPasswordTextBox);
            this.settingsPanel.Controls.Add(this.userNameLabel);
            this.settingsPanel.Controls.Add(this.userNameTextBox);
            this.settingsPanel.Controls.Add(this.domainNameLabel);
            this.settingsPanel.Controls.Add(this.domainNameTextBox);
            this.settingsPanel.Controls.Add(this.saveLogButton);
            this.settingsPanel.Controls.Add(this.branchCacheVersionComboBox);
            this.settingsPanel.Controls.Add(this.serverSecretLabel);
            this.settingsPanel.Controls.Add(this.filePathTextBox);
            this.settingsPanel.Controls.Add(this.operationModeComboBox);
            this.settingsPanel.Controls.Add(this.filePathLabel);
            this.settingsPanel.Controls.Add(this.operationModeLabel);
            this.settingsPanel.Controls.Add(this.filePathBrowseButton);
            this.settingsPanel.Controls.Add(this.serverSecretTextBox);
            this.settingsPanel.Controls.Add(this.executeButton);
            this.settingsPanel.Controls.Add(this.hashAlgorithmComboBox);
            this.settingsPanel.Controls.Add(this.branchCacheVersionLabel);
            this.settingsPanel.Controls.Add(this.HashAlgorithmLabel);
            this.settingsPanel.Controls.Add(this.transportComboBox);
            this.settingsPanel.Controls.Add(this.transportLabel);
            this.settingsPanel.Location = new System.Drawing.Point(27, 16);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(723, 207);
            this.settingsPanel.TabIndex = 0;
            // 
            // saveLogButton
            // 
            this.saveLogButton.Location = new System.Drawing.Point(591, 137);
            this.saveLogButton.Name = "saveLogButton";
            this.saveLogButton.Size = new System.Drawing.Size(75, 23);
            this.saveLogButton.TabIndex = 24;
            this.saveLogButton.Text = "Save Log";
            this.saveLogButton.UseVisualStyleBackColor = true;
            this.saveLogButton.Click += new System.EventHandler(this.saveLogButton_Click);
            // 
            // domainNameLabel
            // 
            this.domainNameLabel.AutoSize = true;
            this.domainNameLabel.Location = new System.Drawing.Point(491, 40);
            this.domainNameLabel.Name = "domainNameLabel";
            this.domainNameLabel.Size = new System.Drawing.Size(43, 13);
            this.domainNameLabel.TabIndex = 26;
            this.domainNameLabel.Text = "Domain";
            // 
            // domainNameTextBox
            // 
            this.domainNameTextBox.Location = new System.Drawing.Point(576, 37);
            this.domainNameTextBox.Name = "domainNameTextBox";
            this.domainNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.domainNameTextBox.TabIndex = 25;
            this.domainNameTextBox.Text = "contoso";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Location = new System.Drawing.Point(491, 74);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(60, 13);
            this.userNameLabel.TabIndex = 28;
            this.userNameLabel.Text = "User Name";
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(576, 71);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.userNameTextBox.TabIndex = 27;
            this.userNameTextBox.Text = "administrator";
            // 
            // userPasswordLabel
            // 
            this.userPasswordLabel.AutoSize = true;
            this.userPasswordLabel.Location = new System.Drawing.Point(491, 103);
            this.userPasswordLabel.Name = "userPasswordLabel";
            this.userPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.userPasswordLabel.TabIndex = 30;
            this.userPasswordLabel.Text = "Password";
            // 
            // userPasswordTextBox
            // 
            this.userPasswordTextBox.Location = new System.Drawing.Point(576, 100);
            this.userPasswordTextBox.Name = "userPasswordTextBox";
            this.userPasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.userPasswordTextBox.TabIndex = 29;
            this.userPasswordTextBox.Text = "Password01!";
            // 
            // smb2AuthenticationComboBox
            // 
            this.smb2AuthenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.smb2AuthenticationComboBox.FormattingEnabled = true;
            this.smb2AuthenticationComboBox.Location = new System.Drawing.Point(370, 95);
            this.smb2AuthenticationComboBox.Name = "smb2AuthenticationComboBox";
            this.smb2AuthenticationComboBox.Size = new System.Drawing.Size(100, 21);
            this.smb2AuthenticationComboBox.TabIndex = 31;
            // 
            // smb2AuthenticationLabel
            // 
            this.smb2AuthenticationLabel.AutoSize = true;
            this.smb2AuthenticationLabel.Location = new System.Drawing.Point(286, 103);
            this.smb2AuthenticationLabel.Name = "smb2AuthenticationLabel";
            this.smb2AuthenticationLabel.Size = new System.Drawing.Size(75, 13);
            this.smb2AuthenticationLabel.TabIndex = 32;
            this.smb2AuthenticationLabel.Text = "Authentication";
            // 
            // ImportDataForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(777, 441);
            this.Controls.Add(this.topSplitContainer);
            this.MinimumSize = new System.Drawing.Size(793, 480);
            this.Name = "ImportDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hash Generation & Verification Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ImportDataForm_Load);
            this.topSplitContainer.Panel1.ResumeLayout(false);
            this.topSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.topSplitContainer)).EndInit();
            this.topSplitContainer.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Button filePathBrowseButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.ComboBox hashAlgorithmComboBox;
        private System.Windows.Forms.Label HashAlgorithmLabel;
        private System.Windows.Forms.Label transportLabel;
        private System.Windows.Forms.ComboBox transportComboBox;
        private System.Windows.Forms.Label branchCacheVersionLabel;
        private System.Windows.Forms.ComboBox branchCacheVersionComboBox;
        private System.Windows.Forms.TextBox serverSecretTextBox;
        private System.Windows.Forms.Label operationModeLabel;
        private System.Windows.Forms.ComboBox operationModeComboBox;
        private System.Windows.Forms.Label serverSecretLabel;
        private System.Windows.Forms.RichTextBox logRichTextBox;
        private System.Windows.Forms.SplitContainer topSplitContainer;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Button saveLogButton;
        private System.Windows.Forms.ComboBox smb2AuthenticationComboBox;
        private System.Windows.Forms.Label smb2AuthenticationLabel;
        private System.Windows.Forms.Label userPasswordLabel;
        private System.Windows.Forms.TextBox userPasswordTextBox;
        private System.Windows.Forms.Label userNameLabel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label domainNameLabel;
        private System.Windows.Forms.TextBox domainNameTextBox;

    }
}
