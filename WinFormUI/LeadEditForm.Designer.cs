namespace WinFormUI
{
    partial class LeadEditForm
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
            txtFullName = new TextBox();
            lblCompanyName = new Label();
            txtCompanyName = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblSource = new Label();
            cmbSource = new ComboBox();
            lblInterestArea = new Label();
            txtInterestArea = new TextBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            chkFollowUp = new CheckBox();
            dtpFollowUp = new DateTimePicker();
            panelButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblFullName
            //
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(12, 15);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(90, 20);
            lblFullName.TabIndex = 0;
            lblFullName.Text = "Ad Soyad";
            //
            // txtFullName
            //
            txtFullName.Location = new Point(140, 12);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(220, 27);
            txtFullName.TabIndex = 1;
            //
            // lblCompanyName
            //
            lblCompanyName.AutoSize = true;
            lblCompanyName.Location = new Point(12, 48);
            lblCompanyName.Name = "lblCompanyName";
            lblCompanyName.Size = new Size(52, 20);
            lblCompanyName.TabIndex = 2;
            lblCompanyName.Text = "Firma";
            //
            // txtCompanyName
            //
            txtCompanyName.Location = new Point(140, 45);
            txtCompanyName.Name = "txtCompanyName";
            txtCompanyName.Size = new Size(220, 27);
            txtCompanyName.TabIndex = 3;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 81);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(63, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "E-posta";
            //
            // txtEmail
            //
            txtEmail.Location = new Point(140, 78);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(220, 27);
            txtEmail.TabIndex = 5;
            //
            // lblPhone
            //
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(12, 114);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(61, 20);
            lblPhone.TabIndex = 6;
            lblPhone.Text = "Telefon";
            //
            // txtPhone
            //
            txtPhone.Location = new Point(140, 111);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(220, 27);
            txtPhone.TabIndex = 7;
            //
            // lblSource
            //
            lblSource.AutoSize = true;
            lblSource.Location = new Point(12, 147);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(60, 20);
            lblSource.TabIndex = 8;
            lblSource.Text = "Kaynak";
            //
            // cmbSource
            //
            cmbSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSource.Location = new Point(140, 144);
            cmbSource.Name = "cmbSource";
            cmbSource.Size = new Size(220, 28);
            cmbSource.TabIndex = 9;
            //
            // lblInterestArea
            //
            lblInterestArea.AutoSize = true;
            lblInterestArea.Location = new Point(12, 180);
            lblInterestArea.Name = "lblInterestArea";
            lblInterestArea.Size = new Size(107, 20);
            lblInterestArea.TabIndex = 10;
            lblInterestArea.Text = "İlgi Alanı";
            //
            // txtInterestArea
            //
            txtInterestArea.Location = new Point(140, 177);
            txtInterestArea.Name = "txtInterestArea";
            txtInterestArea.Size = new Size(220, 27);
            txtInterestArea.TabIndex = 11;
            //
            // lblNotes
            //
            lblNotes.AutoSize = true;
            lblNotes.Location = new Point(12, 213);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(41, 20);
            lblNotes.TabIndex = 12;
            lblNotes.Text = "Not";
            //
            // txtNotes
            //
            txtNotes.Location = new Point(140, 210);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.Size = new Size(220, 60);
            txtNotes.TabIndex = 13;
            //
            // chkFollowUp
            //
            chkFollowUp.AutoSize = true;
            chkFollowUp.Location = new Point(140, 278);
            chkFollowUp.Name = "chkFollowUp";
            chkFollowUp.Size = new Size(130, 24);
            chkFollowUp.TabIndex = 14;
            chkFollowUp.Text = "Takip Tarihi Var";
            chkFollowUp.UseVisualStyleBackColor = true;
            chkFollowUp.CheckedChanged += chkFollowUp_CheckedChanged;
            //
            // dtpFollowUp
            //
            dtpFollowUp.Enabled = false;
            dtpFollowUp.Location = new Point(140, 308);
            dtpFollowUp.Name = "dtpFollowUp";
            dtpFollowUp.Size = new Size(220, 27);
            dtpFollowUp.TabIndex = 15;
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 350);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(380, 45);
            panelButtons.TabIndex = 16;
            //
            // btnSave
            //
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(197, 12);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 29);
            btnSave.TabIndex = 0;
            btnSave.Text = "Kaydet";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(287, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // LeadEditForm
            //
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(380, 395);
            Controls.Add(lblFullName);
            Controls.Add(txtFullName);
            Controls.Add(lblCompanyName);
            Controls.Add(txtCompanyName);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            Controls.Add(lblSource);
            Controls.Add(cmbSource);
            Controls.Add(lblInterestArea);
            Controls.Add(txtInterestArea);
            Controls.Add(lblNotes);
            Controls.Add(txtNotes);
            Controls.Add(chkFollowUp);
            Controls.Add(dtpFollowUp);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LeadEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Yeni Lead";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFullName;
        private TextBox txtFullName;
        private Label lblCompanyName;
        private TextBox txtCompanyName;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblSource;
        private ComboBox cmbSource;
        private Label lblInterestArea;
        private TextBox txtInterestArea;
        private Label lblNotes;
        private TextBox txtNotes;
        private CheckBox chkFollowUp;
        private DateTimePicker dtpFollowUp;
        private FlowLayoutPanel panelButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}
