namespace WinFormUI
{
    partial class AddressForm
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
            lblRecipientName = new Label();
            txtRecipientName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
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
            panelButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnSave = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblRecipientName
            //
            lblRecipientName.AutoSize = true;
            lblRecipientName.Location = new Point(12, 15);
            lblRecipientName.Name = "lblRecipientName";
            lblRecipientName.Size = new Size(70, 20);
            lblRecipientName.TabIndex = 0;
            lblRecipientName.Text = "Alıcı Adı";
            //
            // txtRecipientName
            //
            txtRecipientName.Location = new Point(120, 12);
            txtRecipientName.Name = "txtRecipientName";
            txtRecipientName.Size = new Size(220, 27);
            txtRecipientName.TabIndex = 1;
            //
            // lblPhone
            //
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(12, 45);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(56, 20);
            lblPhone.TabIndex = 2;
            lblPhone.Text = "Telefon";
            //
            // txtPhone
            //
            txtPhone.Location = new Point(120, 42);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(220, 27);
            txtPhone.TabIndex = 3;
            //
            // lblAddressLine
            //
            lblAddressLine.AutoSize = true;
            lblAddressLine.Location = new Point(12, 75);
            lblAddressLine.Name = "lblAddressLine";
            lblAddressLine.Size = new Size(45, 20);
            lblAddressLine.TabIndex = 4;
            lblAddressLine.Text = "Adres";
            //
            // txtAddressLine
            //
            txtAddressLine.Location = new Point(120, 72);
            txtAddressLine.Multiline = true;
            txtAddressLine.Name = "txtAddressLine";
            txtAddressLine.Size = new Size(220, 60);
            txtAddressLine.TabIndex = 5;
            //
            // lblCity
            //
            lblCity.AutoSize = true;
            lblCity.Location = new Point(12, 145);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(48, 20);
            lblCity.TabIndex = 6;
            lblCity.Text = "Şehir";
            //
            // txtCity
            //
            txtCity.Location = new Point(120, 142);
            txtCity.Name = "txtCity";
            txtCity.Size = new Size(220, 27);
            txtCity.TabIndex = 7;
            //
            // lblDistrict
            //
            lblDistrict.AutoSize = true;
            lblDistrict.Location = new Point(12, 175);
            lblDistrict.Name = "lblDistrict";
            lblDistrict.Size = new Size(37, 20);
            lblDistrict.TabIndex = 8;
            lblDistrict.Text = "İlçe";
            //
            // txtDistrict
            //
            txtDistrict.Location = new Point(120, 172);
            txtDistrict.Name = "txtDistrict";
            txtDistrict.Size = new Size(220, 27);
            txtDistrict.TabIndex = 9;
            //
            // lblPostalCode
            //
            lblPostalCode.AutoSize = true;
            lblPostalCode.Location = new Point(12, 205);
            lblPostalCode.Name = "lblPostalCode";
            lblPostalCode.Size = new Size(83, 20);
            lblPostalCode.TabIndex = 10;
            lblPostalCode.Text = "Posta Kodu";
            //
            // txtPostalCode
            //
            txtPostalCode.Location = new Point(120, 202);
            txtPostalCode.Name = "txtPostalCode";
            txtPostalCode.Size = new Size(220, 27);
            txtPostalCode.TabIndex = 11;
            //
            // lblCountry
            //
            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(12, 235);
            lblCountry.Name = "lblCountry";
            lblCountry.Size = new Size(42, 20);
            lblCountry.TabIndex = 12;
            lblCountry.Text = "Ülke";
            //
            // txtCountry
            //
            txtCountry.Location = new Point(120, 232);
            txtCountry.Name = "txtCountry";
            txtCountry.Size = new Size(220, 27);
            txtCountry.TabIndex = 13;
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
            panelButtons.TabIndex = 14;
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
            btnSave.Text = "Devam Et";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            //
            // AddressForm
            //
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(360, 320);
            Controls.Add(lblRecipientName);
            Controls.Add(txtRecipientName);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
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
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddressForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Teslimat Adresi";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRecipientName;
        private TextBox txtRecipientName;
        private Label lblPhone;
        private TextBox txtPhone;
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
        private FlowLayoutPanel panelButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}
