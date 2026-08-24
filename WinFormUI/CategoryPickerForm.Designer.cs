namespace WinFormUI
{
    partial class CategoryPickerForm
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
            lblPrompt = new Label();
            cmbCategory = new ComboBox();
            panelButtons = new FlowLayoutPanel();
            btnOk = new Button();
            btnCancel = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblPrompt
            //
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(12, 15);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(150, 20);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Kategori seçin";
            //
            // cmbCategory
            //
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Location = new Point(12, 40);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(300, 28);
            cmbCategory.TabIndex = 1;
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnOk);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 85);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(340, 45);
            panelButtons.TabIndex = 2;
            //
            // btnOk
            //
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(157, 12);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(90, 29);
            btnOk.TabIndex = 0;
            btnOk.Text = "Tamam";
            btnOk.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(247, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // CategoryPickerForm
            //
            AcceptButton = btnOk;
            CancelButton = btnCancel;
            ClientSize = new Size(340, 130);
            Controls.Add(lblPrompt);
            Controls.Add(cmbCategory);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CategoryPickerForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kategori Seç";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblPrompt;
        private ComboBox cmbCategory;
        private FlowLayoutPanel panelButtons;
        private Button btnOk;
        private Button btnCancel;
    }
}
