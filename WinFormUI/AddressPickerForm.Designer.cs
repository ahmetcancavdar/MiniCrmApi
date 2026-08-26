namespace WinFormUI
{
    partial class AddressPickerForm
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
            lblRecipientName = new Label();
            txtRecipientName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblAddresses = new Label();
            lblHint = new Label();
            dgvAddresses = new DataGridView();
            btnAddAddress = new Button();
            panelButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnUseAddress = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).BeginInit();
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
            txtRecipientName.Size = new Size(300, 27);
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
            txtPhone.Size = new Size(300, 27);
            txtPhone.TabIndex = 3;
            //
            // lblAddresses
            //
            lblAddresses.AutoSize = true;
            lblAddresses.Location = new Point(12, 80);
            lblAddresses.Name = "lblAddresses";
            lblAddresses.Size = new Size(120, 20);
            lblAddresses.TabIndex = 4;
            lblAddresses.Text = "Kayıtlı Adresler";
            //
            // lblHint
            //
            lblHint.AutoSize = true;
            lblHint.ForeColor = Color.DimGray;
            lblHint.Location = new Point(140, 80);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(280, 20);
            lblHint.TabIndex = 5;
            lblHint.Text = "Henüz kayıtlı adresiniz yok, önce bir tane ekleyin.";
            lblHint.Visible = false;
            //
            // dgvAddresses
            //
            dgvAddresses.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAddresses.AllowUserToAddRows = false;
            dgvAddresses.AutoGenerateColumns = true;
            dgvAddresses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAddresses.Location = new Point(12, 105);
            dgvAddresses.MultiSelect = false;
            dgvAddresses.Name = "dgvAddresses";
            dgvAddresses.ReadOnly = true;
            dgvAddresses.RowHeadersWidth = 51;
            dgvAddresses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAddresses.Size = new Size(560, 220);
            dgvAddresses.TabIndex = 6;
            //
            // btnAddAddress
            //
            btnAddAddress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAddAddress.Location = new Point(12, 335);
            btnAddAddress.Name = "btnAddAddress";
            btnAddAddress.Size = new Size(150, 29);
            btnAddAddress.TabIndex = 7;
            btnAddAddress.Text = "Yeni Adres Ekle";
            btnAddAddress.UseVisualStyleBackColor = true;
            btnAddAddress.Click += btnAddAddress_Click;
            //
            // panelButtons
            //
            panelButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnUseAddress);
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(340, 330);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(232, 40);
            panelButtons.TabIndex = 8;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(142, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // btnUseAddress
            //
            btnUseAddress.Enabled = false;
            btnUseAddress.Location = new Point(12, 3);
            btnUseAddress.Name = "btnUseAddress";
            btnUseAddress.Size = new Size(124, 29);
            btnUseAddress.TabIndex = 0;
            btnUseAddress.Text = "Bu Adresi Kullan";
            btnUseAddress.UseVisualStyleBackColor = true;
            btnUseAddress.Click += btnUseAddress_Click;
            //
            // AddressPickerForm
            //
            AcceptButton = btnUseAddress;
            CancelButton = btnCancel;
            ClientSize = new Size(584, 375);
            Controls.Add(lblRecipientName);
            Controls.Add(txtRecipientName);
            Controls.Add(lblPhone);
            Controls.Add(txtPhone);
            Controls.Add(lblAddresses);
            Controls.Add(lblHint);
            Controls.Add(dgvAddresses);
            Controls.Add(btnAddAddress);
            Controls.Add(panelButtons);
            MinimumSize = new Size(600, 420);
            Name = "AddressPickerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Teslimat Adresi Seç";
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblRecipientName;
        private TextBox txtRecipientName;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblAddresses;
        private Label lblHint;
        private DataGridView dgvAddresses;
        private Button btnAddAddress;
        private FlowLayoutPanel panelButtons;
        private Button btnCancel;
        private Button btnUseAddress;
    }
}
