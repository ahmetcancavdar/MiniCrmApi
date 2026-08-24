namespace WinFormUI
{
    partial class NewSupportRequestForm
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
            lblMessage = new Label();
            txtMessage = new TextBox();
            lblOrderNumber = new Label();
            txtOrderNumber = new TextBox();
            lblOrderNumberHint = new Label();
            panelButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnCreate = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblMessage
            //
            lblMessage.AutoSize = true;
            lblMessage.Location = new Point(12, 15);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(63, 20);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Mesajınız";
            //
            // txtMessage
            //
            txtMessage.Location = new Point(12, 38);
            txtMessage.Multiline = true;
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(360, 100);
            txtMessage.TabIndex = 1;
            //
            // lblOrderNumber
            //
            lblOrderNumber.AutoSize = true;
            lblOrderNumber.Location = new Point(12, 150);
            lblOrderNumber.Name = "lblOrderNumber";
            lblOrderNumber.Size = new Size(160, 20);
            lblOrderNumber.TabIndex = 2;
            lblOrderNumber.Text = "Sipariş No (isteğe bağlı)";
            //
            // txtOrderNumber
            //
            txtOrderNumber.Location = new Point(12, 173);
            txtOrderNumber.Name = "txtOrderNumber";
            txtOrderNumber.Size = new Size(360, 27);
            txtOrderNumber.TabIndex = 3;
            //
            // lblOrderNumberHint
            //
            lblOrderNumberHint.AutoSize = true;
            lblOrderNumberHint.ForeColor = SystemColors.GrayText;
            lblOrderNumberHint.Location = new Point(12, 203);
            lblOrderNumberHint.Name = "lblOrderNumberHint";
            lblOrderNumberHint.Size = new Size(300, 20);
            lblOrderNumberHint.TabIndex = 4;
            lblOrderNumberHint.Text = "Belirli bir siparişle ilgiliyse sipariş numarasını girin.";
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnCreate);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 235);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(384, 45);
            panelButtons.TabIndex = 5;
            //
            // btnCancel
            //
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(267, 15);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 29);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "İptal";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // btnCreate
            //
            btnCreate.DialogResult = DialogResult.OK;
            btnCreate.Location = new Point(171, 15);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(90, 29);
            btnCreate.TabIndex = 0;
            btnCreate.Text = "Oluştur";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            //
            // NewSupportRequestForm
            //
            AcceptButton = btnCreate;
            CancelButton = btnCancel;
            ClientSize = new Size(384, 280);
            Controls.Add(lblMessage);
            Controls.Add(txtMessage);
            Controls.Add(lblOrderNumber);
            Controls.Add(txtOrderNumber);
            Controls.Add(lblOrderNumberHint);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NewSupportRequestForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Yeni Destek Talebi";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMessage;
        private TextBox txtMessage;
        private Label lblOrderNumber;
        private TextBox txtOrderNumber;
        private Label lblOrderNumberHint;
        private FlowLayoutPanel panelButtons;
        private Button btnCreate;
        private Button btnCancel;
    }
}
