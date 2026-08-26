using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class LeadDetailForm : Form
    {
        private static readonly ComboOption<int>[] StatusOptions =
        {
            new("İletişime Geçildi", 2),
            new("Potansiyel Olarak Değerlendirildi", 3),
            new("Teklif Gönderildi", 4),
            new("Kaybedildi", 6)
        };

        private readonly HttpClient _httpClient;
        private readonly int _leadId;

        public bool ChangesMade { get; private set; }

        public LeadDetailForm(HttpClient httpClient, int leadId)
        {
            InitializeComponent();

            _httpClient = httpClient;
            _leadId = leadId;

            cmbNewStatus.DataSource = StatusOptions;
            cmbNewStatus.DisplayMember = "Label";
            cmbNewStatus.ValueMember = "Value";

            Load += LeadDetailForm_Load;
        }

        private async void LeadDetailForm_Load(object? sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Leads/{_leadId}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Lead Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var lead = await response.Content.ReadFromJsonAsync<LeadDetailDto>();

                if (lead is null)
                {
                    return;
                }

                lblFullNameValue.Text = lead.FullName;
                lblCompanyNameValue.Text = string.IsNullOrWhiteSpace(lead.CompanyName) ? "-" : lead.CompanyName;
                lblEmailValue.Text = lead.Email;
                lblPhoneValue.Text = string.IsNullOrWhiteSpace(lead.Phone) ? "-" : lead.Phone;
                lblStatusValue.Text = lead.Status;
                lblSourceValue.Text = lead.Source;
                lblConvertedValue.Text = lead.ConvertedCustomerId is { } customerId ? $"#{customerId}" : "-";

                var isConverted = lead.Status == "Converted";

                cmbNewStatus.Enabled = !isConverted;
                btnUpdateStatus.Enabled = !isConverted;
                btnConvert.Enabled = !isConverted;
                txtLostReason.Visible = false;

                dgvNotes.DataSource = lead.LeadNotes;

                if (dgvNotes.Columns["AdminUserId"] is { } adminColumn)
                {
                    adminColumn.HeaderText = "Admin";
                }

                if (dgvNotes.Columns["Note"] is { } noteColumn)
                {
                    noteColumn.HeaderText = "Not";
                    noteColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvNotes.Columns["CreatedAtUtc"] is { } dateColumn)
                {
                    dateColumn.HeaderText = "Tarih";
                }

                if (dgvNotes.Columns["Id"] is { } idColumn)
                {
                    idColumn.Visible = false;
                }
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

        private void cmbNewStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLostReason.Visible = cmbNewStatus.SelectedValue is int value && value == 6;
        }

        private async void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            try
            {
                var request = new UpdateLeadStatusRequestDto
                {
                    Status = cmbNewStatus.SelectedValue is int value ? value : 2,
                    Reason = txtLostReason.Visible && !string.IsNullOrWhiteSpace(txtLostReason.Text)
                        ? txtLostReason.Text.Trim()
                        : null
                };

                var response = await _httpClient.PostAsJsonAsync($"api/Leads/{_leadId}/status", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Durum Güncellenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                ChangesMade = true;
                txtLostReason.Clear();
                await ReloadAsync();
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

        private async void btnAddNote_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewNote.Text))
            {
                MessageBox.Show("Not boş olamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new AddLeadNoteRequestDto
                {
                    Note = txtNewNote.Text.Trim()
                };

                var response = await _httpClient.PostAsJsonAsync($"api/Leads/{_leadId}/notes", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Not Eklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                ChangesMade = true;
                txtNewNote.Clear();
                await ReloadAsync();
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

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Bu lead'i müşteriye dönüştürmek istediğinize emin misiniz?",
                "Müşteriye Dönüştür",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"api/Leads/{_leadId}/convert",
                    new ConvertLeadRequestDto());

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorAsync(response),
                        "Dönüştürülemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                ChangesMade = true;

                MessageBox.Show(
                    "Lead başarıyla müşteriye dönüştürüldü.",
                    "Başarılı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await ReloadAsync();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = ChangesMade ? DialogResult.OK : DialogResult.Cancel;
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
