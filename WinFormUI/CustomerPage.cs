using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class CustomerPage : Form
    {
        private readonly string _customerEmail;
        private readonly string _token;
        private readonly HttpClient _httpClient;

        private int _currentCartItemCount;
        private int? _selectedOrderId;
        private string? _selectedOrderNumber;
        private int? _selectedSupportConversationId;

        public CustomerPage(string email, string token)
        {
            InitializeComponent();
            _customerEmail = email;
            _token = token;

            _httpClient = new HttpClient { BaseAddress = new Uri(ApiConfig.BaseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);

            FormClosed += CustomerPage_FormClosed;

            CenterTitleLabel();
        }

        private bool _navigatedAway;


        // ============================================================
        // KAPATMA (X) -> LOGIN'E DÖN
        // ============================================================

        private void CustomerPage_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (_navigatedAway)
            {
                return;
            }

            new Login().Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        // BAŞLIK ETİKETİNİ ORTALA / ORANTILI YENİDEN BOYUTLANDIRMA

        private void CustomerPage_Resize(object sender, EventArgs e)
        {
            CenterTitleLabel();

            if (SepetimTabPage.Width > 0)
            {
                panelSepetDetay.Width = Math.Max(190, (int)(SepetimTabPage.Width * 0.24));
            }

            if (SiparislerimTabPage.Height > 0)
            {
                panel1.Height = Math.Max(150, (int)(SiparislerimTabPage.Height * 0.52));
            }

            AdjustSupportSplitter();

            CenterEmptyCartLabel();
            CenterNoOrdersLabel();
        }

        // Destek sekmesini sayfanın tam ortasından, sol taraf (talep listesi)
        // biraz daha dar kalacak şekilde böler; pencere her büyüyüp
        // küçüldüğünde bu oran korunur (SplitContainer'ın SplitterDistance'ı
        // tek başına yüzdesel değil, piksel cinsinden sabit bir değerdir; bu
        // yüzden oranı korumak için burada yeniden hesaplanması gerekiyor).
        private void AdjustSupportSplitter()
        {
            if (DestekSplitContainer.Width <= 0)
            {
                return;
            }

            var desiredLeftWidth =
                (int)(DestekSplitContainer.Width * 0.46);

            var minLeft = DestekSplitContainer.Panel1MinSize;
            var maxLeft = DestekSplitContainer.Width - DestekSplitContainer.Panel2MinSize - DestekSplitContainer.SplitterWidth;

            if (maxLeft <= minLeft)
            {
                return;
            }

            DestekSplitContainer.SplitterDistance =
                Math.Max(minLeft, Math.Min(desiredLeftWidth, maxLeft));
        }

        private void CenterTitleLabel()
        {
            label1.Left = (ClientSize.Width - label1.Width) / 2;
        }

        // HESAP BİLGİLERİ / ÇIKIŞ YAP

        private void button2_Click(object sender, EventArgs e)
        {
            using var accountForm = new AccountForm(_httpClient, _customerEmail, "Müşteri", isCustomer: true);

            accountForm.ShowDialog(this);

            if (accountForm.LoggedOut)
            {
                _navigatedAway = true;

                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private async void CustomerPage_Load(object sender, EventArgs e)
        {
            AdjustSupportSplitter();

            await LoadProductsAsync();
            await LoadCartAsync();
            await LoadOrdersAsync();
            await LoadSupportConversationsAsync();
        }


        // ÜRÜNLERİ YÜKLE

        private async Task LoadProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Products");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Ürünler Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var products =
                    await response.Content.ReadFromJsonAsync<List<ProductDto>>()
                    ?? new List<ProductDto>();

                dataGridView1.DataSource = products;
                ConfigureProductGridColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureProductGridColumns()
        {
            HideColumn("Id");
            HideColumn("CategoryId");
            HideColumn("CategoryName");
            HideColumn("Sku");
            HideColumn("Description");
            HideColumn("IsActive");

            RenameColumn("ImageUrl", "Görsel");
            RenameColumn("Name", "Ürün Adı");
            RenameColumn("StockQuantity", "Stok");

            if (dataGridView1.Columns["Price"] is { } priceColumn)
            {
                priceColumn.HeaderText = "Fiyat";
                priceColumn.DefaultCellStyle.Format = "N2";
            }

            SetDisplayIndex("ImageUrl", 0);
            SetDisplayIndex("Name", 1);
            SetDisplayIndex("Price", 2);
            SetDisplayIndex("StockQuantity", 3);

            if (!dataGridView1.Columns.Contains("SepeteEkle"))
            {
                var sepeteEkleColumn = new DataGridViewButtonColumn
                {
                    Name = "SepeteEkle",
                    HeaderText = string.Empty,
                    UseColumnTextForButtonValue = false
                };

                dataGridView1.Columns.Add(sepeteEkleColumn);
            }

            SetDisplayIndex("SepeteEkle", 4);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.DataBoundItem is ProductDto product)
                {
                    row.Cells["SepeteEkle"].Value = product.StockQuantity > 0 ? "Sepete Ekle" : "─────";
                }
            }

            // AdminPage'deki tablolarla aynı yöntem: tüm sütunlar orantılı paylaşıyor
            // (Fill), MinimumWidth ile de pencere küçülse bile okunaksız hale gelmiyorlar.
            // MinimumWidth, AutoSizeColumnsMode=Fill AKTİF OLMADAN ÖNCE verilmeli;
            // aksi halde grid henüz görünür olmayan bir sekmedeyken (örn. uygulama
            // açılışında) .NET'in Fill genişlik hesaplaması NullReferenceException
            // fırlatabiliyor.
            SetColumnSizing("ImageUrl", minimumWidth: 50, fillWeight: 15);
            SetColumnSizing("Name", minimumWidth: 140, fillWeight: 40);
            SetColumnSizing("Price", minimumWidth: 80, fillWeight: 15);
            SetColumnSizing("StockQuantity", minimumWidth: 50, fillWeight: 15);
            SetColumnSizing("SepeteEkle", minimumWidth: 100, fillWeight: 15);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void SetColumnSizing(string columnName, int minimumWidth, int fillWeight)
        {
            if (dataGridView1.Columns[columnName] is { } column)
            {
                column.MinimumWidth = minimumWidth;
                column.FillWeight = fillWeight;
            }
        }

        private void HideColumn(string columnName)
        {
            if (dataGridView1.Columns[columnName] is { } column)
            {
                column.Visible = false;
            }
        }

        private void RenameColumn(string columnName, string headerText)
        {
            if (dataGridView1.Columns[columnName] is { } column)
            {
                column.HeaderText = headerText;
            }
        }

        private void SetDisplayIndex(string columnName, int displayIndex)
        {
            if (dataGridView1.Columns[columnName] is { } column)
            {
                column.DisplayIndex = displayIndex;
            }
        }


        // SEPETE EKLE

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (dataGridView1.Columns[e.ColumnIndex].Name != "SepeteEkle")
            {
                return;
            }

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem is not ProductDto product)
            {
                return;
            }

            if (product.StockQuantity <= 0)
            {
                MessageBox.Show(
                    "Bu ürün stokta yok.",
                    "Sepete Eklenemedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new AddCartItemRequestDto
                {
                    ProductId = product.Id,
                    Quantity = 1
                };

                var response = await _httpClient.PostAsJsonAsync("api/Cart/items", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sepete Eklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    $"\"{product.Name}\" sepete eklendi.",
                    "Sepete Eklendi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                await LoadCartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private static string FormatConnectionError(Exception ex)
        {
            var detail = ex.InnerException is null
                ? $"{ex.GetType().Name}: {ex.Message}"
                : $"{ex.GetType().Name}: {ex.Message}\n({ex.InnerException.GetType().Name}: {ex.InnerException.Message})";

            return $"Sunucuya bağlanırken bir hata oluştu:\n{detail}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!";
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

            return $"İşlem başarısız oldu. (HTTP {(int)response.StatusCode})";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        // ============================================================
        // SEPETİ YÜKLE
        // ============================================================

        private async Task LoadCartAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Cart");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sepet Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var cart =
                    await response.Content.ReadFromJsonAsync<CartResponseDto>()
                    ?? new CartResponseDto();

                _currentCartItemCount = cart.Items.Count;
                ToplamLabel.Text = $"Toplam: {cart.TotalAmount:N2} ₺";

                if (cart.Items.Count == 0)
                {
                    dataGridView2.DataSource = null;
                    dataGridView2.Visible = false;
                    lblSepetBos.Visible = true;
                    CenterEmptyCartLabel();
                    return;
                }

                lblSepetBos.Visible = false;
                dataGridView2.Visible = true;
                dataGridView2.DataSource = cart.Items;
                ConfigureCartGridColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureCartGridColumns()
        {
            HideCartColumn("ProductId");
            HideCartColumn("SKU");
            HideCartColumn("AvailableStock");
            HideCartColumn("ImageUrl");
            HideCartColumn("IsAvailable");

            RenameCartColumn("ProductName", "Ürün");
            RenameCartColumn("Quantity", "Adet");

            if (dataGridView2.Columns["UnitPrice"] is { } unitPriceColumn)
            {
                unitPriceColumn.HeaderText = "Fiyat";
                unitPriceColumn.DefaultCellStyle.Format = "N2";
            }

            if (dataGridView2.Columns["LineTotal"] is { } lineTotalColumn)
            {
                lineTotalColumn.HeaderText = "Toplam";
                lineTotalColumn.DefaultCellStyle.Format = "N2";
            }

            // MinimumWidth, AutoSizeColumnsMode=Fill AKTİF OLMADAN ÖNCE verilmeli;
            // aksi halde grid henüz görünür olmayan bir sekmedeyken (örn. uygulama
            // açılışında) .NET'in Fill genişlik hesaplaması NullReferenceException
            // fırlatabiliyor.
            if (dataGridView2.Columns["ProductName"] is { } nameColumn)
            {
                nameColumn.MinimumWidth = 160;
                nameColumn.FillWeight = 40;
            }

            if (dataGridView2.Columns["Quantity"] is { } quantityColumn)
            {
                quantityColumn.MinimumWidth = 60;
                quantityColumn.FillWeight = 20;
            }

            if (dataGridView2.Columns["UnitPrice"] is { } priceColumn)
            {
                priceColumn.MinimumWidth = 90;
                priceColumn.FillWeight = 20;
            }

            if (dataGridView2.Columns["LineTotal"] is { } totalColumn)
            {
                totalColumn.MinimumWidth = 90;
                totalColumn.FillWeight = 20;
            }

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideCartColumn(string columnName)
        {
            if (dataGridView2.Columns[columnName] is { } column)
            {
                column.Visible = false;
            }
        }

        private void RenameCartColumn(string columnName, string headerText)
        {
            if (dataGridView2.Columns[columnName] is { } column)
            {
                column.HeaderText = headerText;
            }
        }

        private void CenterEmptyCartLabel()
        {
            lblSepetBos.Left = dataGridView2.Left + (dataGridView2.Width - lblSepetBos.Width) / 2;
            lblSepetBos.Top = dataGridView2.Top + (dataGridView2.Height - lblSepetBos.Height) / 2;
        }


        // ============================================================
        // SEPETİ TEMİZLE
        // ============================================================

        private async void SepetTemizleButton_Click(object sender, EventArgs e)
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/Cart");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sepet Temizlenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadCartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // SİPARİŞ VER
        // ============================================================

        private async void SiparisVerButton_Click(object sender, EventArgs e)
        {
            if (_currentCartItemCount == 0)
            {
                MessageBox.Show(
                    "Sepetiniz boş, sipariş verilemez.",
                    "Sepet Boş",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using var addressPicker = new AddressPickerForm(_httpClient);

            if (addressPicker.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            SiparisVerButton.Enabled = false;
            SepetTemizleButton.Enabled = false;

            try
            {
                var request = new CheckoutOrderRequestDto
                {
                    RecipientName = addressPicker.RecipientName,
                    Phone = addressPicker.Phone,
                    AddressLine = addressPicker.AddressLine,
                    City = addressPicker.City,
                    District = addressPicker.District,
                    PostalCode = addressPicker.PostalCode,
                    Country = addressPicker.Country
                };

                var response = await _httpClient.PostAsJsonAsync("api/Orders/checkout", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sipariş Verilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var checkoutResult = await response.Content.ReadFromJsonAsync<CheckoutOrderResponseDto>();

                if (checkoutResult is not null)
                {
                    using var verificationForm = new OrderVerificationForm(
                        checkoutResult.Order.Id,
                        checkoutResult.Order.OrderNumber,
                        _token);

                    verificationForm.ShowDialog(this);
                }

                // Sipariş, doğrulansın ya da doğrulanmasın (PendingVerification
                // durumunda bile) hemen "Siparişlerim" listesinde görünmeli;
                // önceden sadece sepet yenileniyordu, siparişler ancak
                // hesaptan çıkış/giriş yapılınca güncelleniyordu. Doğrulama
                // başarılı olduysa stok da düşer; bu yüzden ürün listesi de
                // burada yenilenir, aksi halde stok değişimi ancak hesaptan
                // çıkış/giriş yapılınca görünüyordu.
                await LoadCartAsync();
                await LoadOrdersAsync(checkoutResult?.Order.Id);
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SiparisVerButton.Enabled = true;
                SepetTemizleButton.Enabled = true;
            }
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }


        // ============================================================
        // SİPARİŞLERİ YÜKLE
        // ============================================================

        private async Task LoadOrdersAsync(int? selectOrderId = null)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Orders");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Siparişler Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var orders =
                    await response.Content.ReadFromJsonAsync<List<OrderSummaryResponseDto>>()
                    ?? new List<OrderSummaryResponseDto>();

                if (orders.Count == 0)
                {
                    dataGridView3.DataSource = null;
                    dataGridView3.Visible = false;
                    lblSiparisYok.Visible = true;
                    CenterNoOrdersLabel();
                    ClearOrderDetailPanel();
                    return;
                }

                lblSiparisYok.Visible = false;
                dataGridView3.Visible = true;
                dataGridView3.DataSource = orders;
                ConfigureOrdersGridColumns();

                if (selectOrderId.HasValue)
                {
                    SelectOrderById(selectOrderId.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SelectOrderById(int orderId)
        {
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.DataBoundItem is OrderSummaryResponseDto order && order.Id == orderId)
                {
                    // "Id" kolonu ConfigureOrdersGridColumns() içinde gizleniyor;
                    // CurrentCell görünmez bir hücreye ayarlanamayacağı için
                    // burada ilk görünür hücre kullanılmalı, sabit index 0 değil.
                    var targetCell =
                        row.Cells
                            .Cast<DataGridViewCell>()
                            .FirstOrDefault(cell => cell.OwningColumn?.Visible == true);

                    if (targetCell is not null)
                    {
                        dataGridView3.CurrentCell = targetCell;
                    }

                    return;
                }
            }
        }

        private void ConfigureOrdersGridColumns()
        {
            HideOrderColumn("Id");
            HideOrderColumn("ConfirmedAtUtc");
            HideOrderColumn("ShippedAtUtc");
            HideOrderColumn("DeliveredAtUtc");
            HideOrderColumn("CancelledAtUtc");

            RenameOrderColumn("OrderNumber", "Sipariş No");
            RenameOrderColumn("CreatedAtUtc", "Tarih");
            RenameOrderColumn("Status", "Durum");
            RenameOrderColumn("TotalAmount", "Tutar");

            if (dataGridView3.Columns["CreatedAtUtc"] is { } dateColumn)
            {
                dateColumn.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }

            if (dataGridView3.Columns["TotalAmount"] is { } amountColumn)
            {
                amountColumn.DefaultCellStyle.Format = "N2";
            }

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.DataBoundItem is OrderSummaryResponseDto order)
                {
                    row.Cells["Status"].Value = TranslateOrderStatus(order.Status);
                }
            }

            // MinimumWidth, AutoSizeColumnsMode=Fill AKTİF OLMADAN ÖNCE verilmeli;
            // aksi halde grid henüz görünür olmayan bir sekmedeyken (örn. uygulama
            // açılışında) .NET'in Fill genişlik hesaplaması NullReferenceException
            // fırlatabiliyor.
            SetOrderColumnSizing("OrderNumber", minimumWidth: 160, fillWeight: 30);
            SetOrderColumnSizing("CreatedAtUtc", minimumWidth: 130, fillWeight: 25);
            SetOrderColumnSizing("Status", minimumWidth: 110, fillWeight: 25);
            SetOrderColumnSizing("TotalAmount", minimumWidth: 100, fillWeight: 20);
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void HideOrderColumn(string columnName)
        {
            if (dataGridView3.Columns[columnName] is { } column)
            {
                column.Visible = false;
            }
        }

        private void RenameOrderColumn(string columnName, string headerText)
        {
            if (dataGridView3.Columns[columnName] is { } column)
            {
                column.HeaderText = headerText;
            }
        }

        private void SetOrderColumnSizing(string columnName, int minimumWidth, int fillWeight)
        {
            if (dataGridView3.Columns[columnName] is { } column)
            {
                column.MinimumWidth = minimumWidth;
                column.FillWeight = fillWeight;
            }
        }

        private static string TranslateOrderStatus(string status)
        {
            return status switch
            {
                "PendingVerification" => "Onay Bekliyor",
                "Confirmed" => "Onaylandı",
                "Preparing" => "Hazırlanıyor",
                "Shipped" => "Kargoda",
                "Delivered" => "Teslim Edildi",
                "Cancelled" => "İptal Edildi",
                _ => status
            };
        }

        private void CenterNoOrdersLabel()
        {
            lblSiparisYok.Left = dataGridView3.Left + (dataGridView3.Width - lblSiparisYok.Width) / 2;
            lblSiparisYok.Top = dataGridView3.Top + (dataGridView3.Height - lblSiparisYok.Height) / 2;
        }


        // ============================================================
        // SİPARİŞ DETAYI
        // ============================================================

        private async void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow?.DataBoundItem is not OrderSummaryResponseDto selected)
            {
                ClearOrderDetailPanel();
                return;
            }

            await UpdateOrderDetailPanelAsync(selected.Id);
        }

        private async Task UpdateOrderDetailPanelAsync(int orderId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Orders/{orderId}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sipariş Detayı Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var order = await response.Content.ReadFromJsonAsync<OrderResponseDto>();

                if (order is null)
                {
                    return;
                }

                _selectedOrderId = order.Id;
                _selectedOrderNumber = order.OrderNumber;
                SıparısNoLabel.Text = $"Sipariş No: {order.OrderNumber}";
                DurumLabel.Text = $"Durum: {TranslateOrderStatus(order.Status)}";
                SiparisIptalButton.Enabled = order.Status is "PendingVerification" or "Confirmed" or "Preparing";
                SiparisOnaylaButton.Enabled = order.Status == "PendingVerification";

                UrunlerLıstBox.Items.Clear();
                UrunlerLıstBox.Items.Add("Ürünler:");

                foreach (var item in order.Items)
                {
                    UrunlerLıstBox.Items.Add($"  - {item.ProductName} x{item.Quantity} → {item.LineTotal:N2}₺");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearOrderDetailPanel()
        {
            _selectedOrderId = null;
            _selectedOrderNumber = null;
            SıparısNoLabel.Text = "Sipariş No:";
            DurumLabel.Text = "Durum:";
            SiparisIptalButton.Enabled = false;
            SiparisOnaylaButton.Enabled = false;
            UrunlerLıstBox.Items.Clear();
        }


        // ============================================================
        // SİPARİŞİ İPTAL ET
        // ============================================================

        private async void SiparisIptalButton_Click(object sender, EventArgs e)
        {
            if (_selectedOrderId is null)
            {
                return;
            }

            var confirm = MessageBox.Show(
                "Siparişi iptal etmek istediğinize emin misiniz?",
                "Siparişi İptal Et",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            var orderId = _selectedOrderId.Value;
            SiparisIptalButton.Enabled = false;
            SiparisOnaylaButton.Enabled = false;

            try
            {
                var request = new CancelOrderRequestDto
                {
                    Reason = "Müşteri tarafından iptal edildi."
                };

                var response = await _httpClient.PostAsJsonAsync($"api/Orders/{orderId}/cancel", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sipariş İptal Edilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(
                    "Siparişiniz iptal edildi.",
                    "Sipariş İptal Edildi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // İptal edilen sipariş Confirmed/Preparing durumundaysa
                // backend stoğu geri ekliyor; ürün listesi burada
                // yenilenmezse bu değişiklik ancak hesaptan çıkış/giriş
                // yapılınca görünüyordu.
                await LoadOrdersAsync(orderId);
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                await UpdateOrderDetailPanelAsync(orderId);
            }
        }


        // ============================================================
        // SİPARİŞİ ONAYLA
        // ============================================================

        private async void SiparisOnaylaButton_Click(object sender, EventArgs e)
        {
            if (_selectedOrderId is null || _selectedOrderNumber is null)
            {
                return;
            }

            try
            {
                using var verificationForm = new OrderVerificationForm(
                    _selectedOrderId.Value,
                    _selectedOrderNumber,
                    _token);

                verificationForm.ShowDialog(this);

                // Doğrulama başarılı olduysa stok düşer; ürün listesi de
                // hemen yenilenmeli, aksi halde stok değişimi ancak
                // hesaptan çıkış/giriş yapılınca görünüyordu.
                await LoadOrdersAsync(_selectedOrderId);
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // DESTEK SOHBETLERİNİ YÜKLE
        // ============================================================

        private async Task LoadSupportConversationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/SupportConversations");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Destek Sohbetleri Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var conversations =
                    await response.Content.ReadFromJsonAsync<List<SupportConversationSummaryDto>>()
                    ?? new List<SupportConversationSummaryDto>();

                CustomerDestekDataGridView.DataSource = conversations;
                ConfigureSupportGridColumns();

                if (_selectedSupportConversationId.HasValue)
                {
                    SelectSupportConversationById(_selectedSupportConversationId.Value);
                }

                await UpdateSupportDetailPanelAsync(GetSelectedSupportConversationId());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureSupportGridColumns()
        {
            // SupportConversationSummaryDto sınıfı AdminPage.cs ile paylaşılıyor
            // ve orada admin'e özel CustomerId/CustomerName/CustomerEmail
            // alanları da var; müşteri tarafındaki API yanıtında bu alanlar
            // gelmediği için (varsayılan/boş değerde kalıyorlar) DataGridView
            // yine de bunlar için sütun oluşturuyor. Müşteri için anlamsız
            // olduklarından burada gizleniyorlar.
            foreach (var columnName in new[] { "OrderId", "CustomerId", "CustomerName", "CustomerEmail", "UpdatedAtUtc" })
            {
                if (CustomerDestekDataGridView.Columns[columnName] is { } hiddenColumn)
                {
                    hiddenColumn.Visible = false;
                }
            }

            if (CustomerDestekDataGridView.Columns["Id"] is { } idColumn)
            {
                idColumn.HeaderText = "ID";
            }

            if (CustomerDestekDataGridView.Columns["Status"] is { } statusColumn)
            {
                statusColumn.HeaderText = "Durum";
            }

            if (CustomerDestekDataGridView.Columns["OrderNumber"] is { } orderNumberColumn)
            {
                orderNumberColumn.HeaderText = "Sipariş No";
            }

            if (CustomerDestekDataGridView.Columns["CreatedAtUtc"] is { } createdColumn)
            {
                createdColumn.HeaderText = "Tarih";
                createdColumn.DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            }

            foreach (DataGridViewRow row in CustomerDestekDataGridView.Rows)
            {
                if (row.DataBoundItem is SupportConversationSummaryDto conversation)
                {
                    row.Cells["Status"].Value = TranslateSupportStatus(conversation.Status);
                }
            }

            CustomerDestekDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private static string TranslateSupportStatus(string status)
        {
            return status switch
            {
                "Open" => "Açık",
                "Closed" => "Kapalı",
                _ => status
            };
        }

        private SupportConversationSummaryDto? GetSelectedSupportConversation()
        {
            return CustomerDestekDataGridView.CurrentRow?.DataBoundItem as SupportConversationSummaryDto;
        }

        private int? GetSelectedSupportConversationId()
        {
            return GetSelectedSupportConversation()?.Id;
        }

        private void SelectSupportConversationById(int conversationId)
        {
            foreach (DataGridViewRow row in CustomerDestekDataGridView.Rows)
            {
                if (row.DataBoundItem is SupportConversationSummaryDto conversation &&
                    conversation.Id == conversationId)
                {
                    var targetCell =
                        row.Cells
                            .Cast<DataGridViewCell>()
                            .FirstOrDefault(cell => cell.OwningColumn?.Visible == true);

                    if (targetCell is not null)
                    {
                        CustomerDestekDataGridView.CurrentCell = targetCell;
                    }

                    return;
                }
            }
        }

        private async void CustomerDestekDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            await UpdateSupportDetailPanelAsync(GetSelectedSupportConversationId());
        }


        // ============================================================
        // DESTEK SOHBETİ DETAYI
        // ============================================================

        private async Task UpdateSupportDetailPanelAsync(int? conversationId)
        {
            if (conversationId is null)
            {
                ClearSupportDetailPanel();
                return;
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/SupportConversations/{conversationId}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Destek Sohbeti Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var conversation = await response.Content.ReadFromJsonAsync<SupportConversationDetailDto>();

                if (conversation is null)
                {
                    return;
                }

                _selectedSupportConversationId = conversation.Id;

                var isOpen = conversation.Status == "Open";

                DestekDurumu.Text = $"Durum: {TranslateSupportStatus(conversation.Status)}";
                YanıtGondermeTextBox.Enabled = isOpen;
                YanıtGondermeButton.Enabled = isOpen;

                var mesajSatirlari = conversation.Messages.Select(message =>
                {
                    var gonderen = message.SenderType == "Customer" ? "Siz" : "Destek";
                    return $"[{message.CreatedAtUtc:dd.MM HH:mm}] {gonderen}: {message.Message}";
                });

                listBox1.Text = string.Join(Environment.NewLine + Environment.NewLine, mesajSatirlari);
                listBox1.SelectionStart = listBox1.Text.Length;
                listBox1.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearSupportDetailPanel()
        {
            _selectedSupportConversationId = null;
            DestekDurumu.Text = "Durum:";
            listBox1.Text = string.Empty;
            YanıtGondermeTextBox.Enabled = false;
            YanıtGondermeButton.Enabled = false;
        }


        // ============================================================
        // YENİ DESTEK TALEBİ OLUŞTUR
        // ============================================================

        private async void DestekTalebiButton_Click(object sender, EventArgs e)
        {
            using var requestForm = new NewSupportRequestForm();

            if (requestForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            DestekTalebiButton.Enabled = false;

            try
            {
                int? orderId = null;

                // Sipariş no isteğe bağlı; girildiyse müşterinin kendi
                // siparişleri arasından eşleşen ID'ye çevrilir. Eşleşme
                // bulunamazsa talep yine de sipariş bağlantısı olmadan
                // oluşturulur, kullanıcı bilgilendirilir.
                if (requestForm.OrderNumber is not null)
                {
                    orderId = await ResolveOrderIdByNumberAsync(requestForm.OrderNumber);

                    if (orderId is null)
                    {
                        MessageBox.Show(
                            $"\"{requestForm.OrderNumber}\" numaralı bir siparişiniz bulunamadı. Destek talebi sipariş bağlantısı olmadan oluşturulacak.",
                            "Sipariş Bulunamadı",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                var request = new CreateSupportConversationRequestDto
                {
                    Message = requestForm.Message,
                    OrderId = orderId
                };

                var response = await _httpClient.PostAsJsonAsync("api/SupportConversations", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Destek Talebi Oluşturulamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var created = await response.Content.ReadFromJsonAsync<SupportConversationDetailDto>();

                _selectedSupportConversationId = created?.Id;

                await LoadSupportConversationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                DestekTalebiButton.Enabled = true;
            }
        }

        private async Task<int?> ResolveOrderIdByNumberAsync(string orderNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Orders");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var orders =
                    await response.Content.ReadFromJsonAsync<List<OrderSummaryResponseDto>>()
                    ?? new List<OrderSummaryResponseDto>();

                var match = orders.FirstOrDefault(o =>
                    string.Equals(o.OrderNumber, orderNumber, StringComparison.OrdinalIgnoreCase));

                return match?.Id;
            }
            catch
            {
                return null;
            }
        }


        // ============================================================
        // YANIT GÖNDER
        // ============================================================

        private async void YanıtGondermeButton_Click(object sender, EventArgs e)
        {
            if (_selectedSupportConversationId is null)
            {
                return;
            }

            var messageText = YanıtGondermeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(messageText) ||
                messageText == "Yanıt yazınız...")
            {
                MessageBox.Show(
                    "Lütfen bir mesaj yazın.",
                    "Mesaj Boş",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var conversationId = _selectedSupportConversationId.Value;
            YanıtGondermeButton.Enabled = false;
            YanıtGondermeTextBox.Enabled = false;

            try
            {
                var request = new AddSupportMessageRequestDto
                {
                    Message = messageText
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"api/SupportConversations/{conversationId}/messages",
                    request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Mesaj Gönderilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                YanıtGondermeTextBox.Text = string.Empty;

                await LoadSupportConversationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    FormatConnectionError(ex),
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                await UpdateSupportDetailPanelAsync(_selectedSupportConversationId);
            }
        }
    }


    // API MODELLERİ

    public class AddCartItemRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    // SupportConversationSummaryDto, SupportMessageDto, SupportConversationDetailDto
    // ve AddSupportMessageRequestDto AdminPage.cs'de zaten tanımlı; aynı ad alanını
    // (WinFormUI) paylaştıkları ve alan yapıları bu sayfanın API yanıtlarıyla
    // (SupportConversationSummaryResponseDto vb.) birebir uyumlu olduğu için burada
    // tekrar tanımlanmıyor.

    public class CreateSupportConversationRequestDto
    {
        public string Message { get; set; } = string.Empty;
        public int? OrderId { get; set; }
    }

    public class OrderSummaryResponseDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ConfirmedAtUtc { get; set; }
        public DateTime? ShippedAtUtc { get; set; }
        public DateTime? DeliveredAtUtc { get; set; }
        public DateTime? CancelledAtUtc { get; set; }
    }

    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }


    public class CartResponseDto
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int TotalItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
    }

    public class CartItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
        public int AvailableStock { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CheckoutOrderRequestDto
    {
        public string RecipientName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = string.Empty;
    }

    public class CheckoutOrderResponseDto
    {
        public OrderResponseDto Order { get; set; } = new();
        public DateTime VerificationExpiresAtUtc { get; set; }
        public bool VerificationEmailSent { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class CreateAddressRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }

    public class UpdateAddressRequestDto
    {
        public string Title { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }

    public class ChangeEmailRequestDto
    {
        public string NewEmail { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }

    public class ProfileDto
    {
        public int CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class UpdateProfileRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
    }

    public class ChangePasswordRequestDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
