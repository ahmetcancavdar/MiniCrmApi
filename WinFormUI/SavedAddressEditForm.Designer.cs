namespace WinFormUI
{
    partial class SavedAddressEditForm
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
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblAddressLine = new Label();
            txtAddressLine = new TextBox();
            lblCity = new Label();
            txtCity = new TextBox();
            lblDistrict = new Label();
            txtDistrict = new TextBox();
            lblPostalCode = new Label();
            txtPostalCode = new TextBox();
            lblCountry = new Label();
            txtCountry = new TextBox();
            chkIsDefault = new CheckBox();
            panelButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnSave = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(12, 15);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(58, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Başlık";
            //
            // txtTitle
            //
            txtTitle.Location = new Point(120, 12);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "Ev, İş, ...";
            txtTitle.Size = new Size(220, 27);
            txtTitle.TabIndex = 1;
            //
            // lblAddressLine
            //
            lblAddressLine.AutoSize = true;
            lblAddressLine.Location = new Point(12, 45);
            lblAddressLine.Name = "lblAddressLine";
            lblAddressLine.Size = new Size(45, 20);
            lblAddressLine.TabIndex = 2;
            lblAddressLine.Text = "Adres";
            //
            // txtAddressLine
            //
            txtAddressLine.Location = new Point(120, 42);
            txtAddressLine.Multiline = true;
            txtAddressLine.Name = "txtAddressLine";
            txtAddressLine.Size = new Size(220, 60);
            txtAddressLine.TabIndex = 3;
            //
            // lblCity
            //
            lblCity.AutoSize = true;
            lblCity.Location = new Point(12, 115);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(48, 20);
            lblCity.TabIndex = 4;
            lblCity.Text = "Şehir";
            //
            // txtCity
            //
            txtCity.Location = new Point(120, 112);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(220, 27);
            txtCity.TabIndex = 5;
            //
            // lblDistrict
            //
            lblDistrict.AutoSize = true;
            lblDistrict.Location = new Point(12, 145);
            lblDistrict.Name = "lblDistrict";
            lblDistrict.Size = new Size(37, 20);
            lblDistrict.TabIndex = 6;
            lblDistrict.Text = "İlçe";
            //
            // txtDistrict
            //
            txtDistrict.Location = new Point(120, 142);
            txtDistrict.Name = "txtDistrict";
            txtDistrict.Size = new Size(220, 27);
            txtDistrict.TabIndex = 7;
            //
            // lblPostalCode
            //
            lblPostalCode.AutoSize = true;
            lblPostalCode.Location = new Point(12, 175);
            lblPostalCode.Name = "lblPostalCode";
            lblPostalCode.Size = new Size(83, 20);
            lblPostalCode.TabIndex = 8;
            lblPostalCode.Text = "Posta Kodu";
            //
            // txtPostalCode
            //
            txtPostalCode.Location = new Point(120, 172);
            txtPostalCode.Name = "txtPostalCode";
            txtPostalCode.Size = new Size(220, 27);
            txtPostalCode.TabIndex = 9;
            //
            // lblCountry
            //
            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 205);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(42, 20);
            lblCountry.TabIndex = 10;
            lblCountry.Text = "Ülke";
            //
            // txtCountry
            //
            txtCountry.Location = new Point(120, 202);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(220, 27);
            txtCountry.TabIndex = 11;
            //
            // chkIsDefault
            //
            chkIsDefault.AutoSize = true;
            chkIsDefault.Location = new Point(120, 235);
            chkIsDefault.Name = "chkIsDefault";
            chkIsDefault.Size = new Size(150, 24);
            chkIsDefault.TabIndex = 12;
            chkIsDefault.Text = "Varsayılan Adres";
            chkIsDefault.UseVisualStyleBackColor = true;
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 275);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(360, 45);
            panelButtons.TabIndex = 13;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(243, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // btnSave
            //
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(147, 15);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 29);
            btnSave.TabIndex = 0;
            btnSave.Text = "Kaydet";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // SavedAddressEditForm
            //
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(360, 320);
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblAddressLine);
            Controls.Add(txtAddressLine);
            Controls.Add(lblCity);
            Controls.Add(txtCity);
            Controls.Add(lblDistrict);
            Controls.Add(txtDistrict);
            Controls.Add(lblPostalCode);
            Controls.Add(txtPostalCode);
            Controls.Add(lblCountry);
            Controls.Add(txtCountry);
            Controls.Add(chkIsDefault);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SavedAddressEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Yeni Adres";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblAddressLine;
        private TextBox txtAddressLine;
        private Label lblCity;
        private TextBox txtCity;
        private Label lblDistrict;
        private TextBox txtDistrict;
        private Label lblPostalCode;
        private TextBox txtPostalCode;
        private Label lblCountry;
        private TextBox txtCountry;
        private CheckBox chkIsDefault;
        private FlowLayoutPanel panelButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}
