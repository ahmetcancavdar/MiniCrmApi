using System.Windows.Forms;

namespace WinFormUI
{
    public partial class AddressForm : Form
    {
        public string RecipientName => txtRecipientName.Text.Trim();
        public string? Phone => string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
        public string AddressLine => txtAddressLine.Text.Trim();
        public string City => txtCity.Text.Trim();
        public string District => txtDistrict.Text.Trim();
        public string? PostalCode => string.IsNullOrWhiteSpace(txtPostalCode.Text) ? null : txtPostalCode.Text.Trim();
        public string Country => txtCountry.Text.Trim();

        public AddressForm()
        {
            InitializeComponent();

            txtCountry.Text = "Türkiye";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecipientName.Text) ||
                string.IsNullOrWhiteSpace(txtAddressLine.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtDistrict.Text) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show(
                    "Alıcı Adı, Adres, Şehir, İlçe ve Ülke alanları zorunludur.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
