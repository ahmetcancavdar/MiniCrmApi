using System.Windows.Forms;

namespace WinFormUI
{
    public partial class NewSupportRequestForm : Form
    {
        public string Message => txtMessage.Text.Trim();
        public string? OrderNumber => string.IsNullOrWhiteSpace(txtOrderNumber.Text) ? null : txtOrderNumber.Text.Trim();

        public NewSupportRequestForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show(
                    "Lütfen bir mesaj yazın.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
