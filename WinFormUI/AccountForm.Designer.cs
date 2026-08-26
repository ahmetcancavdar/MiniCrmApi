namespace WinFormUI
{
    partial class AccountForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelAccount = new GroupBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblEmailPassword = new Label();
            txtEmailPassword = new TextBox();
            btnUpdateEmail = new Button();
            lblRole = new Label();
            lblRoleValue = new Label();
            panelProfile = new GroupBox();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblCompanyName = new Label();
            txtCompanyName = new TextBox();
            btnSaveProfile = new Button();
            panelPassword = new GroupBox();
            lblCurrentPassword = new Label();
            txtCurrentPassword = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnChangePassword = new Button();
            panelAddresses = new GroupBox();
            dgvAddresses = new DataGridView();
            btnAddAddress = new Button();
            btnEditAddress = new Button();
            btnSetDefaultAddress = new Button();
            btnDeleteAddress = new Button();
            panelFooter = new Panel();
            btnLogout = new Button();
            btnClose = new Button();
            panelAccount.SuspendLayout();
            panelProfile.SuspendLayout();
            panelPassword.SuspendLayout();
            panelAddresses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).BeginInit();
            panelFooter.SuspendLayout();
            SuspendLayout();
            //
            // panelAccount
            //
            panelAccount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelAccount.Controls.Add(lblEmail);
            panelAccount.Controls.Add(txtEmail);
            panelAccount.Controls.Add(lblEmailPassword);
            panelAccount.Controls.Add(txtEmailPassword);
            panelAccount.Controls.Add(btnUpdateEmail);
            panelAccount.Controls.Add(lblRole);
            panelAccount.Controls.Add(lblRoleValue);
            panelAccount.Location = new Point(12, 12);
            panelAccount.Name = "panelAccount";
            panelAccount.Size = new Size(600, 130);
            panelAccount.TabIndex = 0;
            panelAccount.TabStop = false;
            panelAccount.Text = "Hesap Bilgileri";
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 30);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(63, 20);
            lblEmail.TabIndex = 0;
            lblEmail.Text = "E-posta";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(140, 27);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(300, 27);
            txtEmail.TabIndex = 1;
            //
            // lblEmailPassword
            //
            lblEmailPassword.AutoSize = true;
            lblEmailPassword.Location = new Point(12, 63);
            lblEmailPassword.Name = "lblEmailPassword";
            lblEmailPassword.Size = new Size(110, 20);
            lblEmailPassword.TabIndex = 2;
            lblEmailPassword.Text = "Mevcut Şifre";
            //
            // txtEmailPassword
            //
            txtEmailPassword.Location = new Point(140, 60);
            txtEmailPassword.Name = "txtEmailPassword";
            txtEmailPassword.PasswordChar = '●';
            txtEmailPassword.Size = new Size(300, 27);
            txtEmailPassword.TabIndex = 3;
            //
            // btnUpdateEmail
            //
            btnUpdateEmail.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdateEmail.Location = new Point(460, 60);
            btnUpdateEmail.Name = "btnUpdateEmail";
            btnUpdateEmail.Size = new Size(130, 29);
            btnUpdateEmail.TabIndex = 4;
            btnUpdateEmail.Text = "E-postayı Güncelle";
            btnUpdateEmail.UseVisualStyleBackColor = true;
            btnUpdateEmail.Click += btnUpdateEmail_Click;
            //
            // lblRole
            //
            lblRole.AutoSize = true;
            lblRole.Location = new Point(12, 96);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(38, 20);
            lblRole.TabIndex = 5;
            lblRole.Text = "Rol";
            //
            // lblRoleValue
            //
            lblRoleValue.AutoSize = true;
            lblRoleValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRoleValue.Location = new Point(140, 96);
            lblRoleValue.Name = "lblRoleValue";
            lblRoleValue.Size = new Size(20, 20);
            lblRoleValue.TabIndex = 6;
            lblRoleValue.Text = "-";
            //
            // panelProfile
            //
            panelProfile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelProfile.Controls.Add(lblFullName);
            panelProfile.Controls.Add(txtFullName);
            panelProfile.Controls.Add(lblPhone);
            panelProfile.Controls.Add(txtPhone);
            panelProfile.Controls.Add(lblCompanyName);
            panelProfile.Controls.Add(txtCompanyName);
            panelProfile.Controls.Add(btnSaveProfile);
            panelProfile.Location = new Point(12, 150);
            panelProfile.Name = "panelProfile";
            panelProfile.Size = new Size(600, 140);
            panelProfile.TabIndex = 1;
            panelProfile.TabStop = false;
            panelProfile.Text = "Profil Bilgileri";
            //
            // lblFullName
            //
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(12, 30);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(90, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Ad Soyad";
            //
            // txtFullName
            //
            txtFullName.Location = new Point(140, 27);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(300, 27);
            txtFullName.TabIndex = 1;
            //
            // lblPhone
            //
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(12, 63);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(61, 20);
            lblPhone.TabIndex = 2;
            lblPhone.Text = "Telefon";
            //
            // txtPhone
            //
            txtPhone.Location = new Point(140, 60);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(300, 27);
            txtPhone.TabIndex = 3;
            //
            // lblCompanyName
            //
            lblCompanyName.AutoSize = true;
            lblCompanyName.Location = new Point(12, 96);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new Size(52, 20);
            lblCompanyName.TabIndex = 4;
            lblCompanyName.Text = "Firma";
            //
            // txtCompanyName
            //
            txtCompanyName.Location = new Point(140, 93);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(300, 27);
            txtCompanyName.TabIndex = 5;
            //
            // btnSaveProfile
            //
            btnSaveProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveProfile.Location = new Point(460, 60);
            btnSaveProfile.Name = "btnSaveProfile";
            btnSaveProfile.Size = new Size(130, 29);
            btnSaveProfile.TabIndex = 6;
            btnSaveProfile.Text = "Bilgileri Kaydet";
            btnSaveProfile.UseVisualStyleBackColor = true;
            btnSaveProfile.Click += btnSaveProfile_Click;
            //
            // panelPassword
            //
            panelPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelPassword.Controls.Add(lblCurrentPassword);
            panelPassword.Controls.Add(txtCurrentPassword);
            panelPassword.Controls.Add(lblNewPassword);
            panelPassword.Controls.Add(txtNewPassword);
            panelPassword.Controls.Add(lblConfirmPassword);
            panelPassword.Controls.Add(txtConfirmPassword);
            panelPassword.Controls.Add(btnChangePassword);
            panelPassword.Location = new Point(12, 296);
            panelPassword.Name = "panelPassword";
            panelPassword.Size = new Size(600, 150);
            panelPassword.TabIndex = 2;
            panelPassword.TabStop = false;
            panelPassword.Text = "Şifre Değiştir";
            //
            // lblCurrentPassword
            //
            lblCurrentPassword.AutoSize = true;
            lblCurrentPassword.Location = new Point(12, 30);
            lblCurrentPassword.Name = "lblCurrentPassword";
            lblCurrentPassword.Size = new Size(110, 20);
            lblCurrentPassword.TabIndex = 0;
            lblCurrentPassword.Text = "Mevcut Şifre";
            //
            // txtCurrentPassword
            //
            txtCurrentPassword.Location = new Point(140, 27);
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.PasswordChar = '●';
            txtCurrentPassword.Size = new Size(300, 27);
            txtCurrentPassword.TabIndex = 1;
            //
            // lblNewPassword
            //
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(12, 63);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(96, 20);
            lblNewPassword.TabIndex = 2;
            lblNewPassword.Text = "Yeni Şifre";
            //
            // txtNewPassword
            //
            txtNewPassword.Location = new Point(140, 60);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PasswordChar = '●';
            txtNewPassword.Size = new Size(300, 27);
            txtNewPassword.TabIndex = 3;
            //
            // lblConfirmPassword
            //
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(12, 96);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(130, 20);
            lblConfirmPassword.TabIndex = 4;
            lblConfirmPassword.Text = "Yeni Şifre (Tekrar)";
            //
            // txtConfirmPassword
            //
            txtConfirmPassword.Location = new Point(140, 93);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PasswordChar = '●';
            txtConfirmPassword.Size = new Size(300, 27);
            txtConfirmPassword.TabIndex = 5;
            //
            // btnChangePassword
            //
            btnChangePassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChangePassword.Location = new Point(460, 93);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(130, 29);
            btnChangePassword.TabIndex = 6;
            btnChangePassword.Text = "Şifreyi Değiştir";
            btnChangePassword.UseVisualStyleBackColor = true;
            btnChangePassword.Click += btnChangePassword_Click;
            //
            // panelAddresses
            //
            panelAddresses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelAddresses.Controls.Add(dgvAddresses);
            panelAddresses.Controls.Add(btnAddAddress);
            panelAddresses.Controls.Add(btnEditAddress);
            panelAddresses.Controls.Add(btnSetDefaultAddress);
            panelAddresses.Controls.Add(btnDeleteAddress);
            panelAddresses.Location = new Point(12, 452);
            panelAddresses.Name = "panelAddresses";
            panelAddresses.Size = new Size(600, 235);
            panelAddresses.TabIndex = 3;
            panelAddresses.TabStop = false;
            panelAddresses.Text = "Adres Defteri";
            //
            // dgvAddresses
            //
            dgvAddresses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAddresses.AllowUserToAddRows = false;
            dgvAddresses.AutoGenerateColumns = true;
            dgvAddresses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAddresses.Location = new Point(12, 27);
            dgvAddresses.MultiSelect = false;
            dgvAddresses.Name = "dgvAddresses";
            dgvAddresses.ReadOnly = true;
            dgvAddresses.RowHeadersWidth = 51;
            dgvAddresses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAddresses.Size = new Size(576, 160);
            dgvAddresses.TabIndex = 0;
            //
            // btnAddAddress
            //
            btnAddAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddAddress.Location = new Point(12, 196);
            btnAddAddress.Name = "btnAddAddress";
            btnAddAddress.Size = new Size(130, 29);
            btnAddAddress.TabIndex = 1;
            btnAddAddress.Text = "Yeni Adres Ekle";
            btnAddAddress.UseVisualStyleBackColor = true;
            btnAddAddress.Click += btnAddAddress_Click;
            //
            // btnEditAddress
            //
            btnEditAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnEditAddress.Location = new Point(148, 196);
            btnEditAddress.Name = "btnEditAddress";
            btnEditAddress.Size = new Size(106, 29);
            btnEditAddress.TabIndex = 2;
            btnEditAddress.Text = "Düzenle";
            btnEditAddress.UseVisualStyleBackColor = true;
            btnEditAddress.Click += btnEditAddress_Click;
            //
            // btnSetDefaultAddress
            //
            btnSetDefaultAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSetDefaultAddress.Location = new Point(260, 196);
            btnSetDefaultAddress.Name = "btnSetDefaultAddress";
            btnSetDefaultAddress.Size = new Size(140, 29);
            btnSetDefaultAddress.TabIndex = 3;
            btnSetDefaultAddress.Text = "Varsayılan Yap";
            btnSetDefaultAddress.UseVisualStyleBackColor = true;
            btnSetDefaultAddress.Click += btnSetDefaultAddress_Click;
            //
            // btnDeleteAddress
            //
            btnDeleteAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteAddress.Location = new Point(406, 196);
            btnDeleteAddress.Name = "btnDeleteAddress";
            btnDeleteAddress.Size = new Size(94, 29);
            btnDeleteAddress.TabIndex = 4;
            btnDeleteAddress.Text = "Sil";
            btnDeleteAddress.UseVisualStyleBackColor = true;
            btnDeleteAddress.Click += btnDeleteAddress_Click;
            //
            // panelFooter
            //
            panelFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelFooter.Controls.Add(btnLogout);
            panelFooter.Controls.Add(btnClose);
            panelFooter.Dock = DockStyle.Bottom;
            panelFooter.Location = new Point(0, 730);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(624, 55);
            panelFooter.TabIndex = 4;
            //
            // btnLogout
            //
            btnLogout.Location = new Point(12, 13);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(110, 29);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "Çıkış Yap";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            //
            // btnClose
            //
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(522, 13);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 29);
            btnClose.TabIndex = 1;
            btnClose.Text = "Kapat";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            //
            // AccountForm
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(624, 785);
            Controls.Add(panelAccount);
            Controls.Add(panelProfile);
            Controls.Add(panelPassword);
            Controls.Add(panelAddresses);
            Controls.Add(panelFooter);
            MinimumSize = new Size(660, 500);
            Name = "AccountForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Hesabım";
            panelAccount.ResumeLayout(false);
            panelAccount.PerformLayout();
            panelProfile.ResumeLayout(false);
            panelProfile.PerformLayout();
            panelPassword.ResumeLayout(false);
            panelPassword.PerformLayout();
            panelAddresses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).EndInit();
            panelFooter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox panelAccount;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblEmailPassword;
        private TextBox txtEmailPassword;
        private Button btnUpdateEmail;
        private Label lblRole;
        private Label lblRoleValue;
        private GroupBox panelProfile;
        private Label lblFullName;
        private TextBox txtFullName;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblCompanyName;
        private TextBox txtCompanyName;
        private Button btnSaveProfile;
        private GroupBox panelPassword;
        private Label lblCurrentPassword;
        private TextBox txtCurrentPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword;
        private GroupBox panelAddresses;
        private DataGridView dgvAddresses;
        private Button btnAddAddress;
        private Button btnEditAddress;
        private Button btnSetDefaultAddress;
        private Button btnDeleteAddress;
        private Panel panelFooter;
        private Button btnLogout;
        private Button btnClose;
    }
}
