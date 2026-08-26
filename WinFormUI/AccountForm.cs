using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class AccountForm : Form
    {
        private readonly HttpClient _httpClient;
        private readonly bool _isCustomer;

        public bool LoggedOut { get; private set; }
        public string CurrentEmail { get; private set; }

        public AccountForm(HttpClient httpClient, string email, string roleLabel, bool isCustomer)
        {
            InitializeComponent();

            _httpClient = httpClient;
            _isCustomer = isCustomer;
            CurrentEmail = email;

            txtEmail.Text = email;
            lblRoleValue.Text = roleLabel;

            if (!isCustomer)
            {
                panelProfile.Visible = false;
                panelAddresses.Visible = false;

                panelPassword.Top = panelProfile.Top;

                ClientSize = new Size(ClientSize.Width, panelPassword.Bottom + 60);
            }

            Load += AccountForm_Load;
        }

        private async void AccountForm_Load(object? sender, EventArgs e)
        {
            if (!_isCustomer)
            {
                return;
            }

            await LoadProfileAsync();
            await LoadAddressesAsync();
        }


        // ============================================================
        // E-POSTA DEĞİŞTİR
        // ============================================================

        private async void btnUpdateEmail_Click(object sender, EventArgs e)
        {
            var newEmail = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(newEmail))
            {
                MessageBox.Show(
                    "E-posta boş bırakılamaz.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.Equals(newEmail, CurrentEmail, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Yeni e-posta, mevcut e-postayla aynı.",
                    "Değişiklik Yok",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmailPassword.Text))
            {
                MessageBox.Show(
                    "Onay için mevcut şifrenizi girin.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new ChangeEmailRequestDto
                {
                    NewEmail = newEmail,
                    CurrentPassword = txtEmailPassword.Text
                };

                var response = await _httpClient.PostAsJsonAsync("api/Auth/change-email", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "E-posta Güncellenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                CurrentEmail = newEmail;
                txtEmailPassword.Clear();

                MessageBox.Show(
                    "E-postanız güncellendi. Değişikliğin tam olarak yansıması için tekrar giriş yapmanız önerilir.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // PROFİL
        // ============================================================

        private async Task LoadProfileAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Profile");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Profil Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var profile = await response.Content.ReadFromJsonAsync<ProfileDto>();

                if (profile is null)
                {
                    return;
                }

                txtFullName.Text = profile.FullName;
                txtPhone.Text = profile.Phone;
                txtCompanyName.Text = profile.CompanyName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show(
                    "Ad soyad boş bırakılamaz.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new UpdateProfileRequestDto
                {
                    FullName = txtFullName.Text.Trim(),
                    Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    CompanyName = string.IsNullOrWhiteSpace(txtCompanyName.Text) ? null : txtCompanyName.Text.Trim()
                };

                var response = await _httpClient.PutAsJsonAsync("api/Profile", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Bilgiler Kaydedilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    "Bilgileriniz güncellendi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // ŞİFRE DEĞİŞTİR
        // ============================================================

        private async void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text) ||
                string.IsNullOrWhiteSpace(txtNewPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(
                    "Tüm şifre alanları doldurulmalıdır.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(
                    "Yeni şifre ve onayı birbiriyle eşleşmiyor.",
                    "Eşleşmiyor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new ChangePasswordRequestDto
                {
                    CurrentPassword = txtCurrentPassword.Text,
                    NewPassword = txtNewPassword.Text
                };

                var response = await _httpClient.PostAsJsonAsync("api/Auth/change-password", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Şifre Değiştirilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();

                MessageBox.Show(
                    "Şifreniz başarıyla değiştirildi.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // ADRES DEFTERİ
        // ============================================================

        private async Task LoadAddressesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Addresses");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Adresler Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var addresses =
                    await response.Content.ReadFromJsonAsync<List<AddressDto>>()
                    ?? new List<AddressDto>();

                dgvAddresses.DataSource = addresses;

                if (dgvAddresses.Columns["Id"] is { } idColumn)
                {
                    idColumn.Visible = false;
                }

                if (dgvAddresses.Columns["Title"] is { } titleColumn)
                {
                    titleColumn.HeaderText = "Başlık";
                }

                if (dgvAddresses.Columns["AddressLine"] is { } addressColumn)
                {
                    addressColumn.HeaderText = "Adres";
                }

                if (dgvAddresses.Columns["City"] is { } cityColumn)
                {
                    cityColumn.HeaderText = "Şehir";
                }

                if (dgvAddresses.Columns["District"] is { } districtColumn)
                {
                    districtColumn.HeaderText = "İlçe";
                }

                if (dgvAddresses.Columns["PostalCode"] is { } postalColumn)
                {
                    postalColumn.Visible = false;
                }

                if (dgvAddresses.Columns["Country"] is { } countryColumn)
                {
                    countryColumn.HeaderText = "Ülke";
                }

                if (dgvAddresses.Columns["IsDefault"] is { } defaultColumn)
                {
                    defaultColumn.HeaderText = "Varsayılan";
                }

                if (dgvAddresses.Columns["CreatedAtUtc"] is { } createdColumn)
                {
                    createdColumn.Visible = false;
                }

                dgvAddresses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private AddressDto? GetSelectedAddress()
        {
            return dgvAddresses.CurrentRow?.DataBoundItem as AddressDto;
        }

        private async void btnAddAddress_Click(object sender, EventArgs e)
        {
            using var form = new SavedAddressEditForm();

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var request = new CreateAddressRequestDto
            {
                Title = form.TitleValue,
                AddressLine = form.AddressLineValue,
                City = form.CityValue,
                District = form.DistrictValue,
                PostalCode = form.PostalCodeValue,
                Country = form.CountryValue,
                IsDefault = form.IsDefaultValue
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Addresses", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Adres Eklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadAddressesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnEditAddress_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedAddress();

            if (selected is null)
            {
                MessageBox.Show(
                    "Düzenlemek için bir adres seçin.",
                    "Adres Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var form = new SavedAddressEditForm(selected);

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var request = new UpdateAddressRequestDto
            {
                Title = form.TitleValue,
                AddressLine = form.AddressLineValue,
                City = form.CityValue,
                District = form.DistrictValue,
                PostalCode = form.PostalCodeValue,
                Country = form.CountryValue,
                IsDefault = form.IsDefaultValue
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Addresses/{selected.Id}", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Adres Güncellenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadAddressesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnSetDefaultAddress_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedAddress();

            if (selected is null)
            {
                MessageBox.Show(
                    "Varsayılan yapmak için bir adres seçin.",
                    "Adres Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var response = await _httpClient.PostAsync($"api/Addresses/{selected.Id}/default", null);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "İşlem Başarısız",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadAddressesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnDeleteAddress_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedAddress();

            if (selected is null)
            {
                MessageBox.Show(
                    "Silmek için bir adres seçin.",
                    "Adres Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"'{selected.Title}' adresi silinsin mi?",
                "Adres Sil",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"api/Addresses/{selected.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Adres Silinemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadAddressesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // ÇIKIŞ / KAPAT
        // ============================================================

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Sistemden çıkış yapmak istiyor musunuz?",
                "Çıkış Yap",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            LoggedOut = true;
            DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>();

                if (!string.IsNullOrWhiteSpace(problem?.Detail))
                {
                    return problem.Detail;
                }
            }
            catch
            {
                // Gövde ProblemDetails formatında değilse sessizce genel mesaja düşülür.
            }

            return $"İşlem başarısız oldu. (HTTP {(int)response.StatusCode})";
        }
    }
}
