namespace WinFormUI
{
    partial class OrderVerificationForm
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
            if (disposing)
            {
                components?.Dispose();
                _httpClient?.Dispose();
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
            lblInfo = new Label();
            lblCode = new Label();
            txtCode = new TextBox();
            lblStatus = new Label();
            panelButtons = new FlowLayoutPanel();
            btnClose = new Button();
            btnVerify = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            //
            // lblInfo
            //
            lblInfo.Location = new Point(12, 15);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(320, 60);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Siparişiniz oluşturuldu. E-posta adresinize gönderilen 6 haneli onay kodunu girin.";
            //
            // lblCode
            //
            lblCode.AutoSize = true;
            lblCode.Location = new Point(12, 85);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(87, 20);
            lblCode.TabIndex = 1;
            lblCode.Text = "Onay Kodu";
            //
            // txtCode
            //
            txtCode.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtCode.Location = new Point(12, 108);
            txtCode.MaxLength = 6;
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(150, 37);
            txtCode.TabIndex = 2;
            txtCode.TextAlign = HorizontalAlignment.Center;
            //
            // lblStatus
            //
            lblStatus.Location = new Point(12, 155);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(320, 45);
            lblStatus.TabIndex = 3;
            //
            // panelButtons
            //
            panelButtons.Controls.Add(btnClose);
            panelButtons.Controls.Add(btnVerify);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.FlowDirection = FlowDirection.RightToLeft;
            panelButtons.Location = new Point(0, 210);
            panelButtons.Name = "panelButtons";
            panelButtons.Padding = new Padding(12);
            panelButtons.Size = new Size(344, 45);
            panelButtons.TabIndex = 4;
            //
            // btnClose
            //
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(227, 15);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 29);
            btnClose.TabIndex = 1;
            btnClose.Text = "Kapat";
            btnClose.UseVisualStyleBackColor = true;
            //
            // btnVerify
            //
            btnVerify.Location = new Point(131, 15);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(90, 29);
            btnVerify.TabIndex = 0;
            btnVerify.Text = "Onayla";
            btnVerify.UseVisualStyleBackColor = true;
            btnVerify.Click += btnVerify_Click;
            //
            // OrderVerificationForm
            //
            AcceptButton = btnVerify;
            CancelButton = btnClose;
            ClientSize = new Size(344, 255);
            Controls.Add(lblInfo);
            Controls.Add(lblCode);
            Controls.Add(txtCode);
            Controls.Add(lblStatus);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "OrderVerificationForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sipariş Onayı";
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblInfo;
        private Label lblCode;
        private TextBox txtCode;
        private Label lblStatus;
        private FlowLayoutPanel panelButtons;
        private Button btnClose;
        private Button btnVerify;
    }
}
