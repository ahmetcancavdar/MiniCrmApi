using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class Login : Form
    {
        private readonly Dictionary<Control, Rectangle> _originalBounds = new();
        private readonly Size _originalClientSize;

        public Login()
        {
            InitializeComponent();

            MinimumSize = new Size(500, 350);

            // Ekran boyutuna göre ölçeklendirme için tüm kontrollerin orijinal konum/boyutunu sakla.
            _originalClientSize = ClientSize;

            foreach (Control control in Controls)
            {
                _originalBounds[control] = control.Bounds;
            }

            Resize += Login_Resize;

            // Login her zaman navigasyon zincirinin en altındaki ekrandır: Admin/Customer/Kayıt
            // ekranlarına geçerken sadece Hide() edilir (Application.Run bu formu takip ettiği
            // için Close() edilirse uygulama tamamen kapanır). Kullanıcı, bu ekranı (orijinal ya
            // da başka bir ekrandan "Çıkış Yap"/X sonrası yeniden açılan bir kopyasını) pencere
            // X'ine basarak kapatırsa gidecek başka bir yer yoktur; uygulamanın görünürde
            // kapanmış ama arka planda hayalet process olarak çalışmaya devam etmesini önlemek
            // için burada açıkça çıkış yapılır.
            FormClosed += (_, _) => Application.Exit();
        }


        // ============================================================
        // ÖLÇEKLENDİRME
        // ============================================================

        private void Login_Resize(object? sender, EventArgs e)
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

        // ============================================================
        // GİRİŞ YAP BUTONU TIKLANDIĞINDA ÇALIŞACAK METOT
        // ============================================================
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
                
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Lütfen E-Posta ve Şifre alanlarını boş bırakmayın!", "Eksik Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Butonu geçici olarak pasif yapıyoruz (Çift tıklamayı önlemek için)
            btnLogin.Enabled = false;
            btnLogin.Text = "Giriş Yapılıyor...";

            try
            {
                // 3. Web API'ye göndereceğimiz DTO nesnesi
                var loginModel = new
                {
                    Email = email,
                    Password = password
                };

                // HTTP İsteği Atıyoruz
                using (var httpClient = new HttpClient { BaseAddress = new Uri(ApiConfig.BaseUrl) })
                {
                    var response = await httpClient.PostAsJsonAsync("api/Auth/login", loginModel);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                        if (result is null)
                        {
                            MessageBox.Show("Sunucudan beklenmeyen bir yanıt alındı.", "Giriş Başarısız",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (result.Roles.Contains("Admin"))
                        {
                            AdminPage adminPage = new AdminPage(result.Email, result.Token);
                            adminPage.Show();
                        }
                        else
                        {
                            CustomerPage customerPage = new CustomerPage(result.Email, result.Token);
                            customerPage.Show();
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("E-posta adresi veya şifre hatalı!", "Giriş Başarısız",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "Giriş Yap";
            }
        }

        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            var kayitForm = new KayıtOlmaForm();
            kayitForm.Show();
            this.Hide();
        }
    }

    // ============================================================
    // API'DEN DÖNEN YANIT MODELİ (DTO)
    // ============================================================
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = string.Empty;
    }
}