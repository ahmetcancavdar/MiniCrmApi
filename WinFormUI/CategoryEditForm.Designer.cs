namespace WinFormUI
{
    partial class CategoryEditForm
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
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            chkActive = new CheckBox();
            panelButtons = new FlowLayoutPanel();
            btnSave = new Button();
            btnCancel = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblName
            //
            lblName.AutoSize = true;
            lblName.Location = new Point(12, 15);
            lblName.Name = "lblName";
            lblName.Size = new Size(24, 20);
            lblName.TabIndex = 0;
            lblName.Text = "Ad";
            //
            // txtName
            //
            txtName.Location = new Point(110, 12);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 27);
            txtName.TabIndex = 1;
            //
            // lblDescription
            //
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(12, 45);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(76, 20);
            lblDescription.TabIndex = 2;
            lblDescription.Text = "Açıklama";
            //
            // txtDescription
            //
            txtDescription.Location = new Point(110, 42);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(200, 60);
            txtDescription.TabIndex = 3;
            //
            // chkActive
            //
            chkActive.AutoSize = true;
            chkActive.Checked = true;
            chkActive.CheckState = CheckState.Checked;
            chkActive.Location = new Point(110, 110);
            chkActive.Name = "chkActive";
            chkActive.Size = new Size(60, 24);
            chkActive.TabIndex = 4;
            chkActive.Text = "Aktif";
            chkActive.UseVisualStyleBackColor = true;
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnSave);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 175);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(340, 45);
            panelButtons.TabIndex = 5;
            //
            // btnSave
            //
            btnSave.DialogResult = DialogResult.OK;
            btnSave.Location = new Point(157, 12);
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
            btnCancel.Location = new Point(247, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // CategoryEditForm
            //
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            ClientSize = new Size(340, 220);
            Controls.Add(lblName);
            Controls.Add(txtName);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(chkActive);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CategoryEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Kategori Ekle";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Label lblDescription;
        private TextBox txtDescription;
        private CheckBox chkActive;
        private FlowLayoutPanel panelButtons;
        private Button btnSave;
        private Button btnCancel;
    }
}
