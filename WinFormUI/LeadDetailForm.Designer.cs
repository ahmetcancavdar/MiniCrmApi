namespace WinFormUI
{
    partial class LeadDetailForm
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
            lblFullName = new Label();
            lblFullNameValue = new Label();
            lblCompanyName = new Label();
            lblCompanyNameValue = new Label();
            lblEmail = new Label();
            lblEmailValue = new Label();
            lblPhone = new Label();
            lblPhoneValue = new Label();
            lblSource = new Label();
            lblSourceValue = new Label();
            lblStatus = new Label();
            lblStatusValue = new Label();
            lblConverted = new Label();
            lblConvertedValue = new Label();
            panelStatus = new Panel();
            cmbNewStatus = new ComboBox();
            txtLostReason = new TextBox();
            btnUpdateStatus = new Button();
            btnConvert = new Button();
            lblNotesTitle = new Label();
            dgvNotes = new DataGridView();
            txtNewNote = new TextBox();
            btnAddNote = new Button();
            btnClose = new Button();
            panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNotes).BeginInit();
            SuspendLayout();
            //
            // lblFullName
            //
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(12, 12);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(90, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Ad Soyad:";
            //
            // lblFullNameValue
            //
            lblFullNameValue.AutoSize = true;
            lblFullNameValue.Location = new Point(140, 12);
            lblFullNameValue.Name = "lblFullNameValue";
            lblFullNameValue.Size = new Size(20, 20);
            lblFullNameValue.TabIndex = 1;
            lblFullNameValue.Text = "-";
            //
            // lblCompanyName
            //
            lblCompanyName.AutoSize = true;
            lblCompanyName.Location = new Point(12, 38);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new Size(52, 20);
            lblCompanyName.TabIndex = 2;
            lblCompanyName.Text = "Firma:";
            //
            // lblCompanyNameValue
            //
            lblCompanyNameValue.AutoSize = true;
            lblCompanyNameValue.Location = new Point(140, 38);
            lblCompanyNameValue.Name = "lblCompanyNameValue";
            lblCompanyNameValue.Size = new Size(20, 20);
            lblCompanyNameValue.TabIndex = 3;
            lblCompanyNameValue.Text = "-";
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 64);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(63, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "E-posta:";
            //
            // lblEmailValue
            //
            lblEmailValue.AutoSize = true;
            lblEmailValue.Location = new Point(140, 64);
            lblEmailValue.Name = "lblEmailValue";
            lblEmailValue.Size = new Size(20, 20);
            lblEmailValue.TabIndex = 5;
            lblEmailValue.Text = "-";
            //
            // lblPhone
            //
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(12, 90);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(61, 20);
            lblPhone.TabIndex = 6;
            lblPhone.Text = "Telefon:";
            //
            // lblPhoneValue
            //
            lblPhoneValue.AutoSize = true;
            lblPhoneValue.Location = new Point(140, 90);
            lblPhoneValue.Name = "lblPhoneValue";
            lblPhoneValue.Size = new Size(20, 20);
            lblPhoneValue.TabIndex = 7;
            lblPhoneValue.Text = "-";
            //
            // lblSource
            //
            lblSource.AutoSize = true;
            lblSource.Location = new Point(320, 12);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(60, 20);
            lblSource.TabIndex = 8;
            lblSource.Text = "Kaynak:";
            //
            // lblSourceValue
            //
            lblSourceValue.AutoSize = true;
            lblSourceValue.Location = new Point(420, 12);
            lblSourceValue.Name = "lblSourceValue";
            lblSourceValue.Size = new Size(20, 20);
            lblSourceValue.TabIndex = 9;
            lblSourceValue.Text = "-";
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(320, 38);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(55, 20);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Durum:";
            //
            // lblStatusValue
            //
            lblStatusValue.AutoSize = true;
            lblStatusValue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStatusValue.Location = new Point(420, 38);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new Size(20, 20);
            lblStatusValue.TabIndex = 11;
            lblStatusValue.Text = "-";
            //
            // lblConverted
            //
            lblConverted.AutoSize = true;
            lblConverted.Location = new Point(320, 64);
            lblConverted.Name = "lblConverted";
            lblConverted.Size = new Size(64, 20);
            lblConverted.TabIndex = 12;
            lblConverted.Text = "Müşteri:";
            //
            // lblConvertedValue
            //
            lblConvertedValue.AutoSize = true;
            lblConvertedValue.Location = new Point(420, 64);
            lblConvertedValue.Name = "lblConvertedValue";
            lblConvertedValue.Size = new Size(20, 20);
            lblConvertedValue.TabIndex = 13;
            lblConvertedValue.Text = "-";
            //
            // panelStatus
            //
            panelStatus.Controls.Add(cmbNewStatus);
            panelStatus.Controls.Add(txtLostReason);
            panelStatus.Controls.Add(btnUpdateStatus);
            panelStatus.Controls.Add(btnConvert);
            panelStatus.Location = new Point(12, 122);
            panelStatus.Name = "panelStatus";
            panelStatus.Size = new Size(560, 68);
            panelStatus.TabIndex = 14;
            //
            // cmbNewStatus
            //
            cmbNewStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNewStatus.Location = new Point(0, 0);
            cmbNewStatus.Name = "cmbNewStatus";
            cmbNewStatus.Size = new Size(220, 28);
            cmbNewStatus.TabIndex = 0;
            cmbNewStatus.SelectedIndexChanged += cmbNewStatus_SelectedIndexChanged;
            //
            // txtLostReason
            //
            txtLostReason.Location = new Point(0, 34);
            txtLostReason.Name = "txtLostReason";
            txtLostReason.PlaceholderText = "Kaybedilme nedeni (opsiyonel)";
            txtLostReason.Size = new Size(220, 27);
            txtLostReason.TabIndex = 1;
            txtLostReason.Visible = false;
            //
            // btnUpdateStatus
            //
            btnUpdateStatus.Location = new Point(226, 0);
            btnUpdateStatus.Name = "btnUpdateStatus";
            btnUpdateStatus.Size = new Size(140, 29);
            btnUpdateStatus.TabIndex = 2;
            btnUpdateStatus.Text = "Durumu Güncelle";
            btnUpdateStatus.UseVisualStyleBackColor = true;
            btnUpdateStatus.Click += btnUpdateStatus_Click;
            //
            // btnConvert
            //
            btnConvert.BackColor = Color.LightGreen;
            btnConvert.Location = new Point(226, 34);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(140, 29);
            btnConvert.TabIndex = 3;
            btnConvert.Text = "Müşteriye Dönüştür";
            btnConvert.UseVisualStyleBackColor = false;
            btnConvert.Click += btnConvert_Click;
            //
            // lblNotesTitle
            //
            lblNotesTitle.AutoSize = true;
            lblNotesTitle.Location = new Point(12, 200);
            lblNotesTitle.Name = "lblNotesTitle";
            lblNotesTitle.Size = new Size(45, 20);
            lblNotesTitle.TabIndex = 15;
            lblNotesTitle.Text = "Notlar";
            //
            // dgvNotes
            //
            dgvNotes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvNotes.AllowUserToAddRows = false;
            dgvNotes.AutoGenerateColumns = true;
            dgvNotes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNotes.Location = new Point(12, 224);
            dgvNotes.MultiSelect = false;
            dgvNotes.Name = "dgvNotes";
            dgvNotes.ReadOnly = true;
            dgvNotes.RowHeadersWidth = 51;
            dgvNotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNotes.Size = new Size(560, 180);
            dgvNotes.TabIndex = 16;
            //
            // txtNewNote
            //
            txtNewNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtNewNote.Location = new Point(12, 412);
            txtNewNote.Multiline = true;
            txtNewNote.Name = "txtNewNote";
            txtNewNote.PlaceholderText = "Yeni not...";
            txtNewNote.Size = new Size(440, 50);
            txtNewNote.TabIndex = 17;
            //
            // btnAddNote
            //
            btnAddNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAddNote.Location = new Point(458, 412);
            btnAddNote.Name = "btnAddNote";
            btnAddNote.Size = new Size(114, 50);
            btnAddNote.TabIndex = 18;
            btnAddNote.Text = "Not Ekle";
            btnAddNote.UseVisualStyleBackColor = true;
            btnAddNote.Click += btnAddNote_Click;
            //
            // btnClose
            //
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(497, 470);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 29);
            btnClose.TabIndex = 19;
            btnClose.Text = "Kapat";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            //
            // LeadDetailForm
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 511);
            Controls.Add(lblFullName);
            Controls.Add(lblFullNameValue);
            Controls.Add(lblCompanyName);
            Controls.Add(lblCompanyNameValue);
            Controls.Add(lblEmail);
            Controls.Add(lblEmailValue);
            Controls.Add(lblPhone);
            Controls.Add(lblPhoneValue);
            Controls.Add(lblSource);
            Controls.Add(lblSourceValue);
            Controls.Add(lblStatus);
            Controls.Add(lblStatusValue);
            Controls.Add(lblConverted);
            Controls.Add(lblConvertedValue);
            Controls.Add(panelStatus);
            Controls.Add(lblNotesTitle);
            Controls.Add(dgvNotes);
            Controls.Add(txtNewNote);
            Controls.Add(btnAddNote);
            Controls.Add(btnClose);
            MinimumSize = new Size(600, 550);
            Name = "LeadDetailForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Lead Detayı";
            panelStatus.ResumeLayout(false);
            panelStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNotes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFullName;
        private Label lblFullNameValue;
        private Label lblCompanyName;
        private Label lblCompanyNameValue;
        private Label lblEmail;
        private Label lblEmailValue;
        private Label lblPhone;
        private Label lblPhoneValue;
        private Label lblSource;
        private Label lblSourceValue;
        private Label lblStatus;
        private Label lblStatusValue;
        private Label lblConverted;
        private Label lblConvertedValue;
        private Panel panelStatus;
        private ComboBox cmbNewStatus;
        private TextBox txtLostReason;
        private Button btnUpdateStatus;
        private Button btnConvert;
        private Label lblNotesTitle;
        private DataGridView dgvNotes;
        private TextBox txtNewNote;
        private Button btnAddNote;
        private Button btnClose;
    }
}
