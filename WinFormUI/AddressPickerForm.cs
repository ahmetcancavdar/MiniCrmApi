using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class AddressPickerForm : Form
    {
        private readonly HttpClient _httpClient;
        private List<AddressDto> _addresses = new();

        public string RecipientName => txtRecipientName.Text.Trim();
        public string? Phone => string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();

        public string AddressLine { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string District { get; private set; } = string.Empty;
        public string? PostalCode { get; private set; }
        public string Country { get; private set; } = string.Empty;

        public AddressPickerForm(HttpClient httpClient)
        {
            InitializeComponent();

            _httpClient = httpClient;

            Load += AddressPickerForm_Load;
            dgvAddresses.SelectionChanged += dgvAddresses_SelectionChanged;
        }

        private async void AddressPickerForm_Load(object? sender, EventArgs e)
        {
            try
            {
                var profileResponse = await _httpClient.GetAsync("api/Profile");

                if (profileResponse.IsSuccessStatusCode)
                {
                    var profile = await profileResponse.Content.ReadFromJsonAsync<ProfileDto>();

                    if (profile is not null)
                    {
                        txtRecipientName.Text = profile.FullName;
                        txtPhone.Text = profile.Phone;
                    }
                }
            }
            catch
            {
                // Profil ön doldurma opsiyoneldir, başarısız olsa da form kullanılabilir kalmalı.
            }

            await LoadAddressesAsync();
        }

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

                _addresses =
                    await response.Content.ReadFromJsonAsync<List<AddressDto>>()
                    ?? new List<AddressDto>();

                dgvAddresses.DataSource = _addresses;

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

                var defaultAddress = _addresses.FirstOrDefault(x => x.IsDefault) ?? _addresses.FirstOrDefault();

                if (defaultAddress is not null)
                {
                    foreach (DataGridViewRow row in dgvAddresses.Rows)
                    {
                        if (row.DataBoundItem is AddressDto address && address.Id == defaultAddress.Id)
                        {
                            row.Selected = true;
                            dgvAddresses.CurrentCell = row.Cells["Title"];
                            break;
                        }
                    }
                }

                lblHint.Visible = _addresses.Count == 0;
                btnUseAddress.Enabled = GetSelectedAddress() is not null;
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

        private void dgvAddresses_SelectionChanged(object? sender, EventArgs e)
        {
            btnUseAddress.Enabled = GetSelectedAddress() is not null;
        }

        private async void btnAddAddress_Click(object sender, EventArgs e)
        {
            using var form = new SavedAddressEditForm();

            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
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

                var created = await response.Content.ReadFromJsonAsync<AddressDto>();

                await LoadAddressesAsync();

                if (created is not null)
                {
                    foreach (DataGridViewRow row in dgvAddresses.Rows)
                    {
                        if (row.DataBoundItem is AddressDto address && address.Id == created.Id)
                        {
                            row.Selected = true;
                            dgvAddresses.CurrentCell = row.Cells["Title"];
                            break;
                        }
                    }
                }
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

        private void btnUseAddress_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedAddress();

            if (selected is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRecipientName.Text))
            {
                MessageBox.Show(
                    "Alıcı adı boş bırakılamaz.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            AddressLine = selected.AddressLine;
            City = selected.City;
            District = selected.District;
            PostalCode = selected.PostalCode;
            Country = selected.Country;

            DialogResult = DialogResult.OK;
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
