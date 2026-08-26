using System.Windows.Forms;

namespace WinFormUI
{
    public partial class SavedAddressEditForm : Form
    {
        private readonly bool _isEditMode;

        public string TitleValue => txtTitle.Text.Trim();
        public string AddressLineValue => txtAddressLine.Text.Trim();
        public string CityValue => txtCity.Text.Trim();
        public string DistrictValue => txtDistrict.Text.Trim();
        public string? PostalCodeValue => string.IsNullOrWhiteSpace(txtPostalCode.Text) ? null : txtPostalCode.Text.Trim();
        public string CountryValue => txtCountry.Text.Trim();
        public bool IsDefaultValue => chkIsDefault.Checked;

        public SavedAddressEditForm(AddressDto? existingAddress = null)
        {
            InitializeComponent();

            txtCountry.Text = "Türkiye";

            _isEditMode = existingAddress is not null;

            Text = _isEditMode ? "Adresi Düzenle" : "Yeni Adres";

            if (existingAddress is not null)
            {
                txtTitle.Text = existingAddress.Title;
                txtAddressLine.Text = existingAddress.AddressLine;
                txtCity.Text = existingAddress.City;
                txtDistrict.Text = existingAddress.District;
                txtPostalCode.Text = existingAddress.PostalCode;
                txtCountry.Text = existingAddress.Country;
                chkIsDefault.Checked = existingAddress.IsDefault;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAddressLine.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtDistrict.Text) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show(
                    "Başlık, Adres, Şehir, İlçe ve Ülke alanları zorunludur.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
