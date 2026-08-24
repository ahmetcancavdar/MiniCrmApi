namespace WinFormUI
{
    partial class ProductEditForm
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
            lblCategory = new Label();
            cmbCategory = new ComboBox();
            lblProductName = new Label();
            txtProductName = new TextBox();
            lblSku = new Label();
            txtSku = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblPrice = new Label();
            numPrice = new NumericUpDown();
            lblStock = new Label();
            numStock = new NumericUpDown();
            lblImageUrl = new Label();
            txtImageUrl = new TextBox();
            chkActive = new CheckBox();
            panelButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnSave = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numStock).BeginInit();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(12, 15);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(66, 20);
            lblCategory.TabIndex = 0;
            lblCategory.Text = "Kategori";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Location = new Point(110, 12);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(220, 28);
            cmbCategory.TabIndex = 1;
            //
            // lblProductName
            // 
            lblProductName.AutoSize = true;
            lblProductName.Location = new Point(12, 45);
            lblProductName.Name = "lblProductName";
            lblProductName.Size = new Size(67, 20);
            lblProductName.TabIndex = 2;
            lblProductName.Text = "Ürün Adı";
            // 
            // txtProductName
            // 
            txtProductName.Location = new Point(110, 42);
            txtProductName.Name = "txtProductName";
            txtProductName.Size = new Size(220, 27);
            txtProductName.TabIndex = 3;
            // 
            // lblSku
            // 
            lblSku.AutoSize = true;
            lblSku.Location = new Point(12, 75);
            lblSku.Name = "lblSku";
            lblSku.Size = new Size(36, 20);
            lblSku.TabIndex = 4;
            lblSku.Text = "SKU";
            // 
            // txtSku
            // 
            txtSku.Location = new Point(110, 72);
            txtSku.Name = "txtSku";
            txtSku.Size = new Size(220, 27);
            txtSku.TabIndex = 5;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(12, 105);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(70, 20);
            lblDescription.TabIndex = 6;
            lblDescription.Text = "Açıklama";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(110, 102);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(220, 60);
            txtDescription.TabIndex = 7;
            //
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(12, 175);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(40, 20);
            lblPrice.TabIndex = 8;
            lblPrice.Text = "Fiyat";
            // 
            // numPrice
            // 
            numPrice.DecimalPlaces = 2;
            numPrice.Location = new Point(110, 172);
            numPrice.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(220, 27);
            numPrice.TabIndex = 9;
            //
            // lblStock
            // 
            lblStock.AutoSize = true;
            lblStock.Location = new Point(12, 205);
            lblStock.Name = "lblStock";
            lblStock.Size = new Size(38, 20);
            lblStock.TabIndex = 10;
            lblStock.Text = "Stok";
            // 
            // numStock
            // 
            numStock.Location = new Point(110, 202);
            numStock.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numStock.Name = "numStock";
            numStock.Size = new Size(220, 27);
            numStock.TabIndex = 11;
            // 
            // lblImageUrl
            // 
            lblImageUrl.AutoSize = true;
            lblImageUrl.Location = new Point(12, 235);
            lblImageUrl.Name = "lblImageUrl";
            lblImageUrl.Size = new Size(81, 20);
            lblImageUrl.TabIndex = 12;
            lblImageUrl.Text = "Görsel URL";
            // 
            // txtImageUrl
            // 
            txtImageUrl.Location = new Point(110, 232);
            txtImageUrl.Name = "txtImageUrl";
            txtImageUrl.Size = new Size(220, 27);
            txtImageUrl.TabIndex = 13;
            //
            // chkActive
            // 
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Location = new Point(110, 265);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(62, 24);
            chkActive.TabIndex = 14;
            chkActive.Text = "Aktif";
            chkActive.UseVisualStyleBackColor = true;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 320);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(360, 45);
            panelButtons.TabIndex = 15;
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
            // ProductEditForm
            // 
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(360, 365);
            Controls.Add(lblCategory);
            Controls.Add(cmbCategory);
            Controls.Add(lblProductName);
            Controls.Add(txtProductName);
            Controls.Add(lblSku);
            Controls.Add(txtSku);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(lblPrice);
            Controls.Add(numPrice);
            Controls.Add(lblStock);
            Controls.Add(numStock);
            Controls.Add(lblImageUrl);
            Controls.Add(txtImageUrl);
            Controls.Add(chkActive);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ProductEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ürün Ekle";
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numStock).EndInit();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCategory;
        private ComboBox cmbCategory;
        private Label lblProductName;
        private TextBox txtProductName;
        private Label lblSku;
        private TextBox txtSku;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblPrice;
        private NumericUpDown numPrice;
        private Label lblStock;
        private NumericUpDown numStock;
        private Label lblImageUrl;
        private TextBox txtImageUrl;
        private CheckBox chkActive;
        private FlowLayoutPanel panelButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}
