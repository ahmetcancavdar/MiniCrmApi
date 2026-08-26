using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class KayıtOlmaForm : Form
    {
        private const string AdSoyadPlaceholder = "Ad Soyad giriniz...";
        private const string EmailPlaceholder = "Email giriniz...";
        private const string SifrePlaceholder = "Şifre giriniz...";
        private const string TelefonPlaceholder = "Telefon No giriniz...";
        private const string SirketPlaceholder = "Şirket adı giriniz...";

        private readonly Dictionary<Control, Rectangle> _originalBounds = new();
        private readonly Size _originalClientSize;
        private bool _navigatedAway;

        public KayıtOlmaForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(500, 350);

            FormClosed += KayıtOlmaForm_FormClosed;

            AttachPlaceholder(textBox1, AdSoyadPlaceholder);
            AttachPlaceholder(textBox2, EmailPlaceholder);
            AttachPlaceholder(textBox4, TelefonPlaceholder);
            AttachPlaceholder(textBox5, SirketPlaceholder);
            AttachPasswordPlaceholder(textBox3, SifrePlaceholder);

            // Ekran boyutuna göre ölçeklendirme için tüm kontrollerin orijinal konum/boyutunu sakla.
            _originalClientSize = ClientSize;

            foreach (Control control in Controls)
            {
                _originalBounds[control] = control.Bounds;
            }

            Resize += KayıtOlmaForm_Resize;
        }


        // ============================================================
        // ÖLÇEKLENDİRME
        // ============================================================

        private void KayıtOlmaForm_Resize(object? sender, EventArgs e)
        {
            if (_originalClientSize.Width == 0 || _originalClientSize.Height == 0)
            {
                return;
            }

            var scaleX = (float)ClientSize.Width / _originalClientSize.Width;
            var scaleY = (float)ClientSize.Height / _originalClientSize.Height;

            foreach (var pair in _originalBounds)
            {
                var original = pair.Value;

                pair.Key.Bounds = new Rectangle(
                    (int)(original.X * scaleX),
                    (int)(original.Y * scaleY),
                    (int)(original.Width * scaleX),
                    (int)(original.Height * scaleY));
            }
        }


        // PLACEHOLDER YARDIMCILARI

        private static void AttachPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                }
            };
        }

        private static void AttachPasswordPlaceholder(TextBox textBox, string placeholder)
        {
            // Not: UseSystemPasswordChar çalışma zamanında değiştirilirse WinForms
            // kontrolün pencere tanıtıcısını (handle) yeniden oluşturur, bu da bazı
            // ortamlarda "Pencere işleyicisini oluşturmada hata" istisnasına yol açar.
            // PasswordChar aynı görsel sonucu handle yeniden oluşturmadan verir.
            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = string.Empty;
                }

                textBox.PasswordChar = '●';
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.PasswordChar = '\0';
                    textBox.Text = placeholder;
                }
            };
        }

        private static string GetValue(TextBox textBox, string placeholder)
        {
            return textBox.Text == placeholder ? string.Empty : textBox.Text.Trim();
        }


        // KAYIT OL

        private async void button1_Click(object sender, EventArgs e)
        {
            var fullName = GetValue(textBox1, AdSoyadPlaceholder);
            var email = GetValue(textBox2, EmailPlaceholder);
            var password = textBox3.Text == SifrePlaceholder ? string.Empty : textBox3.Text;
            var phone = GetValue(textBox4, TelefonPlaceholder);
            var companyName = GetValue(textBox5, SirketPlaceholder);

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Ad Soyad, Email ve Şifre alanları zorunludur.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show(
                    "Şifre en az 8 karakter olmalıdır.",
                    "Eksik Bilgi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            button1.Enabled = false;
            button1.Text = "Kayıt Olunuyor...";

            try
            {
                using var httpClient = new HttpClient { BaseAddress = new Uri(ApiConfig.BaseUrl) };

                var request = new
                {
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                    CompanyName = string.IsNullOrWhiteSpace(companyName) ? null : companyName
                };

                var response = await httpClient.PostAsJsonAsync("api/Auth/register", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Kayıt Başarısız",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    "Kayıt başarılı! Şimdi giriş yapabilirsiniz.",
                    "Kayıt Ol",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                _navigatedAway = true;

                var loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "Kayıt Ol";
            }
        }

        private static async Task<string> ReadErrorMessageAsync(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                if (json.TryGetProperty("message", out var messageProp))
                {
                    return messageProp.GetString() ?? "Kayıt işlemi başarısız oldu.";
                }

                if (json.TryGetProperty("detail", out var detailProp))
                {
                    return detailProp.GetString() ?? "Kayıt işlemi başarısız oldu.";
                }
            }
            catch
            {
                // Gövde beklenen formatta değilse sessizce genel mesaja düşülür.
            }

            return $"Kayıt işlemi başarısız oldu. (HTTP {(int)response.StatusCode})";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        // ============================================================
        // KAPATMA (X) -> LOGIN'E DÖN
        // ============================================================

        private void KayıtOlmaForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            // Kayıt başarılı olup zaten Login'e yönlendirdiysek (veya ileride
            // başka bir yere yönlendirilirse) tekrar bir Login penceresi
            // açmamak için bu bayrak kontrol edilir.
            if (_navigatedAway)
            {
                return;
            }

            new Login().Show();
        }
    }
}
