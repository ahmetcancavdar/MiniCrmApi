using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class OrderVerificationForm : Form
    {
        private readonly int _orderId;
        private readonly HttpClient _httpClient;

        public bool IsConfirmed { get; private set; }

        public OrderVerificationForm(int orderId, string orderNumber, string token)
        {
            InitializeComponent();

            _orderId = orderId;

            _httpClient = new HttpClient { BaseAddress = new Uri(ApiConfig.BaseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            lblInfo.Text = $"Siparişiniz oluşturuldu (Sipariş No: {orderNumber}). " +
                           "E-posta adresinize gönderilen 6 haneli onay kodunu girin.";
        }

        private async void btnVerify_Click(object sender, EventArgs e)
        {
            var code = txtCode.Text.Trim();

            if (code.Length != 6 || !code.All(char.IsDigit))
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Lütfen 6 haneli onay kodunu girin.";
                return;
            }

            btnVerify.Enabled = false;
            btnVerify.Text = "Onaylanıyor...";

            try
            {
                var request = new VerifyOrderRequestDto { Code = code };

                var response = await _httpClient.PostAsJsonAsync($"api/Orders/{_orderId}/verify", request);

                if (!response.IsSuccessStatusCode)
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = await ReadErrorMessageAsync(response);
                    return;
                }

                IsConfirmed = true;
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Siparişiniz onaylandı!";
                txtCode.Enabled = false;
                btnVerify.Visible = false;
                btnClose.Text = "Kapat";
            }
            catch (Exception ex)
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}";
            }
            finally
            {
                if (!IsConfirmed)
                {
                    btnVerify.Enabled = true;
                    btnVerify.Text = "Onayla";
                }
            }
        }

        private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
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

            return $"Doğrulama başarısız oldu. (HTTP {(int)response.StatusCode})";
        }
    }


    // ============================================================
    // API MODELİ
    // ============================================================

    public class VerifyOrderRequestDto
    {
        public string Code { get; set; } = string.Empty;
    }
}
