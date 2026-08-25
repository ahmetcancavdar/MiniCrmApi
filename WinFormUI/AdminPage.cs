using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WinFormUI
{
    public partial class AdminPage : Form
    {
        private const string ApiBaseUrl = "https://localhost:7048/";

        private readonly string _adminEmail;
        private readonly string _token;
        private readonly HttpClient _httpClient;

        private List<OrderDto> _allOrders = new();


        public AdminPage(string email, string token)
        {
            InitializeComponent();
            _adminEmail = email;
            _token = token;

            _httpClient = new HttpClient { BaseAddress = new Uri(ApiBaseUrl) };
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mesaj =
                $"Hesap Bilgileri\n\n" +
                $"E-Posta : {_adminEmail}\n" +
                $"Rol     : Admin\n\n" +
                $"Çıkış yapmak istiyor musunuz?";

            var cevap = MessageBox.Show(
                mesaj,
                "Hesap Bilgileri",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (cevap == DialogResult.Yes)
            {
                Login loginForm = new Login();
                loginForm.Show();
                this.Close();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private async void AdminPage_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AllowUserToAddRows = false;

            SıparısGrıdVıew.AutoGenerateColumns = true;
            SıparısGrıdVıew.AllowUserToAddRows = false;

            dataGridView2.AutoGenerateColumns = true;
            dataGridView2.AllowUserToAddRows = false;

            dataGridView3.AutoGenerateColumns = true;
            dataGridView3.AllowUserToAddRows = false;

            dataGridView4.AutoGenerateColumns = true;
            dataGridView4.AllowUserToAddRows = false;

            await LoadProductsAsync();
            await LoadOrdersAsync();
            await LoadCustomersAsync();
            await LoadSupportConversationsAsync();

            AdminPage_Resize(this, EventArgs.Empty);
        }


        // ============================================================
        // PENCERE BOYUTU DEĞİŞİNCE ORANLARI KORU
        // ============================================================

        private void AdminPage_Resize(object sender, EventArgs e)
        {
            if (Siparişler.Width > 0)
            {
                panel1.Width = Math.Max(180, (int)(Siparişler.Width * 0.28));
            }

            if (Müşteriler.Width > 0)
            {
                MusterıDetaylbl.Width = Math.Max(200, (int)(Müşteriler.Width * 0.30));
            }

            if (Destek.Width > 0)
            {
                panelDestekDetay.Width = Math.Max(300, (int)(Destek.Width * 0.50));
            }
        }


        // EKLE

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var categories = await LoadCategoriesAsync();

                if (categories.Count == 0)
                {
                    MessageBox.Show(
                        "Ürün ekleyebilmek için önce en az bir kategori oluşturulmalı.",
                        "Kategori Bulunamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using var form = new ProductEditForm(categories);

                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var request = new CreateProductRequestDto
                {
                    CategoryId = form.CategoryId,
                    Name = form.ProductNameValue,
                    Sku = form.Sku,
                    Description = form.Description,
                    Price = form.Price,
                    ImageUrl = form.ImageUrl
                };

                var response = await _httpClient.PostAsJsonAsync("api/Products", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Ürün Eklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                if (form.StockQuantity > 0)
                {
                    var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();

                    if (createdProduct is not null)
                    {
                        var stockResponse = await _httpClient.PostAsJsonAsync(
                            $"api/Products/{createdProduct.Id}/stock",
                            new AdjustStockRequestDto { QuantityChange = form.StockQuantity, Note = "Başlangıç stoğu" });

                        if (!stockResponse.IsSuccessStatusCode)
                        {
                            MessageBox.Show(
                                await ReadErrorMessageAsync(stockResponse),
                                "Stok Ayarlanamadı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }

                await LoadProductsAsync();
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


        // DÜZENLE

        private async void DuzenlemeButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = GetSelectedProduct();

                if (selected is null)
                {
                    MessageBox.Show(
                        "Lütfen düzenlemek için bir ürün seçin.",
                        "Ürün Seçilmedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var categories = await LoadCategoriesAsync();

                using var form = new ProductEditForm(categories, selected);

                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var request = new UpdateProductRequestDto
                {
                    CategoryId = form.CategoryId,
                    Name = form.ProductNameValue,
                    Sku = form.Sku,
                    Description = form.Description,
                    Price = form.Price,
                    ImageUrl = form.ImageUrl,
                    IsActive = form.IsActive
                };

                var response = await _httpClient.PutAsJsonAsync($"api/Products/{selected.Id}", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Ürün Güncellenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var stockDelta = form.StockQuantity - selected.StockQuantity;

                if (stockDelta != 0)
                {
                    var stockResponse = await _httpClient.PostAsJsonAsync(
                        $"api/Products/{selected.Id}/stock",
                        new AdjustStockRequestDto { QuantityChange = stockDelta, Note = "Manuel stok güncellemesi" });

                    if (!stockResponse.IsSuccessStatusCode)
                    {
                        MessageBox.Show(
                            await ReadErrorMessageAsync(stockResponse),
                            "Stok Güncellenemedi",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

                await LoadProductsAsync();
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


        // SİL

        private async void SilButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = GetSelectedProduct();

                if (selected is null)
                {
                    MessageBox.Show(
                        "Lütfen silmek için bir ürün seçin.",
                        "Ürün Seçilmedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var onay = MessageBox.Show(
                    $"\"{selected.Name}\" ürününü silmek istediğinize emin misiniz?",
                    "Ürünü Sil",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (onay != DialogResult.Yes)
                {
                    return;
                }

                var response = await _httpClient.DeleteAsync($"api/Products/{selected.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Ürün Silinemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadProductsAsync();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        // ÜRÜNLERİ YÜKLE


        private async Task LoadProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Products/admin");

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
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async Task<List<CategoryDto>> LoadCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/Categories/admin");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(
                    await ReadErrorMessageAsync(response),
                    "Kategoriler Yüklenemedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return new List<CategoryDto>();
            }

            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new List<CategoryDto>();
        }

        private void ConfigureProductGridColumns()
        {
            HideColumn("CategoryId");
            HideColumn("Description");
            HideColumn("ImageUrl");

            RenameColumn("Id", "ID");
            RenameColumn("CategoryName", "Kategori");
            RenameColumn("Name", "Ürün Adı");
            RenameColumn("Sku", "SKU");
            RenameColumn("StockQuantity", "Stok");
            RenameColumn("IsActive", "Aktif");

            if (dataGridView1.Columns["Price"] is { } priceColumn)
            {
                priceColumn.HeaderText = "Fiyat";
                priceColumn.DefaultCellStyle.Format = "N2";
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private ProductDto? GetSelectedProduct()
        {
            return dataGridView1.CurrentRow?.DataBoundItem as ProductDto;
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

        // ============================================================
        // KATEGORİ EKLE
        // ============================================================

        private async void KategoriAddButton_Click(object sender, EventArgs e)
        {
            try
            {
                using var form = new CategoryEditForm();

                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var request = new CreateCategoryRequestDto
                {
                    Name = form.CategoryName,
                    Description = form.Description
                };

                var response = await _httpClient.PostAsJsonAsync("api/Categories", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Kategori Eklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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


        // ============================================================
        // KATEGORİ DÜZENLE
        // ============================================================

        private async void KategoriDuzenle_Click(object sender, EventArgs e)
        {
            try
            {
                var categories = await LoadCategoriesAsync();

                if (categories.Count == 0)
                {
                    MessageBox.Show(
                        "Düzenlenecek kategori bulunamadı.",
                        "Kategori Bulunamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using var picker = new CategoryPickerForm(categories, "Düzenlenecek kategoriyi seçin");

                if (picker.ShowDialog(this) != DialogResult.OK || picker.SelectedCategory is null)
                {
                    return;
                }

                using var form = new CategoryEditForm(picker.SelectedCategory);

                if (form.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                var request = new UpdateCategoryRequestDto
                {
                    Name = form.CategoryName,
                    Description = form.Description,
                    IsActive = form.IsActive
                };

                var response = await _httpClient.PutAsJsonAsync($"api/Categories/{picker.SelectedCategory.Id}", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Kategori Güncellenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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


        // ============================================================
        // KATEGORİ SİL
        // ============================================================

        private async void kategoriSil_Click(object sender, EventArgs e)
        {
            try
            {
                var categories = await LoadCategoriesAsync();

                if (categories.Count == 0)
                {
                    MessageBox.Show(
                        "Silinecek kategori bulunamadı.",
                        "Kategori Bulunamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                using var picker = new CategoryPickerForm(categories, "Silinecek kategoriyi seçin");

                if (picker.ShowDialog(this) != DialogResult.OK || picker.SelectedCategory is null)
                {
                    return;
                }

                var onay = MessageBox.Show(
                    $"\"{picker.SelectedCategory.Name}\" kategorisini silmek istediğinize emin misiniz?",
                    "Kategoriyi Sil",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (onay != DialogResult.Yes)
                {
                    return;
                }

                var response = await _httpClient.DeleteAsync($"api/Categories/{picker.SelectedCategory.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Kategori Silinemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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

        // ============================================================
        // SİPARİŞLERİ YÜKLE
        // ============================================================

        private async Task LoadOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/AdminOrders");

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
                    await response.Content.ReadFromJsonAsync<List<OrderDto>>()
                    ?? new List<OrderDto>();

                _allOrders = orders;

                SıparısGrıdVıew.DataSource = orders;
                ConfigureOrderGridColumns();
                UpdateOrderDetailPanel(GetSelectedOrder());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureOrderGridColumns()
        {
            if (SıparısGrıdVıew.Columns["CustomerId"] is { } customerIdColumn)
            {
                customerIdColumn.Visible = false;
            }

            if (SıparısGrıdVıew.Columns["CustomerEmail"] is { } emailColumn)
            {
                emailColumn.Visible = false;
            }

            RenameOrderColumn("Id", "ID");
            RenameOrderColumn("OrderNumber", "Sipariş No");
            RenameOrderColumn("CustomerName", "Müşteri");
            RenameOrderColumn("Status", "Durum");
            RenameOrderColumn("CreatedAtUtc", "Oluşturulma Tarihi");

            if (SıparısGrıdVıew.Columns["TotalAmount"] is { } amountColumn)
            {
                amountColumn.HeaderText = "Tutar";
                amountColumn.DefaultCellStyle.Format = "N2";
            }

            foreach (var columnName in new[] { "ConfirmedAtUtc", "ShippedAtUtc", "DeliveredAtUtc", "CancelledAtUtc" })
            {
                if (SıparısGrıdVıew.Columns[columnName] is { } column)
                {
                    column.Visible = false;
                }
            }

            SıparısGrıdVıew.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void RenameOrderColumn(string columnName, string headerText)
        {
            if (SıparısGrıdVıew.Columns[columnName] is { } column)
            {
                column.HeaderText = headerText;
            }
        }

        private OrderDto? GetSelectedOrder()
        {
            return SıparısGrıdVıew.CurrentRow?.DataBoundItem as OrderDto;
        }

        private void SelectOrderById(int orderId)
        {
            foreach (DataGridViewRow row in SıparısGrıdVıew.Rows)
            {
                if (row.DataBoundItem is OrderDto order && order.Id == orderId)
                {
                    SıparısGrıdVıew.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private void SıparısGrıdVıew_SelectionChanged(object sender, EventArgs e)
        {
            UpdateOrderDetailPanel(GetSelectedOrder());
        }

        private void UpdateOrderDetailPanel(OrderDto? order)
        {
            lblOrderNo.Text = $"Sipariş No: {order?.OrderNumber ?? "-"}";
            lblOrderCustomer.Text = $"Müşteri: {order?.CustomerName ?? "-"}";
            lblOrderAmount.Text = $"Tutar: {(order is null ? "-" : order.TotalAmount.ToString("N2"))}";
            lblOrderStatus.Text = $"Durum: {order?.Status ?? "-"}";

            // Sadece geçerli durum geçişleri için butonlar aktif olsun;
            // aksi halde backend zaten DomainException ile reddediyordu.
            btnPrepare.Enabled = order?.Status == "Confirmed";
            btnShip.Enabled = order?.Status == "Preparing";
            btnDeliver.Enabled = order?.Status == "Shipped";
            btnCancelOrder.Enabled = order?.Status is "PendingVerification" or "Confirmed" or "Preparing";
        }


        // ============================================================
        // SİPARİŞ DURUMU DEĞİŞTİR
        // ============================================================

        private async void btnPrepare_Click(object sender, EventArgs e)
        {
            await ChangeOrderStatusAsync("prepare", "Sipariş Hazırlanıyor Yapılamadı");
        }

        private async void btnShip_Click(object sender, EventArgs e)
        {
            await ChangeOrderStatusAsync("ship", "Sipariş Kargoya Verilemedi");
        }

        private async void btnDeliver_Click(object sender, EventArgs e)
        {
            await ChangeOrderStatusAsync("deliver", "Sipariş Teslim Edildi Olarak İşaretlenemedi");
        }

        private async void btnCancelOrder_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedOrder();

            if (selected is null)
            {
                MessageBox.Show(
                    "Lütfen iptal etmek için bir sipariş seçin.",
                    "Sipariş Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var reason = Microsoft.VisualBasic.Interaction.InputBox(
                "İptal nedenini yazın:",
                "Siparişi İptal Et",
                string.Empty);

            if (string.IsNullOrWhiteSpace(reason))
            {
                return;
            }

            // İstek devam ederken buton çift tıklamayı önlemek için
            // pasifleştirilir; finally'de gerçek sipariş durumuna göre
            // yeniden hesaplanır (istek başarılı ya da başarısız olsun).
            SetOrderActionButtonsEnabled(false);

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"api/AdminOrders/{selected.Id}/cancel",
                    new CancelOrderRequestDto { Reason = reason });

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sipariş İptal Edilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // İptal edilen sipariş Confirmed/Preparing durumundaysa
                // backend stoğu geri ekliyor; ürün listesi burada
                // yenilenmezse admin panelinde eski stok görünmeye devam
                // ediyordu.
                await LoadOrdersAsync();
                await LoadProductsAsync();
                SelectOrderById(selected.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                UpdateOrderDetailPanel(GetSelectedOrder());
            }
        }

        private async Task ChangeOrderStatusAsync(string action, string errorTitle)
        {
            var selected = GetSelectedOrder();

            if (selected is null)
            {
                MessageBox.Show(
                    "Lütfen bir sipariş seçin.",
                    "Sipariş Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            SetOrderActionButtonsEnabled(false);

            try
            {
                var response = await _httpClient.PostAsync($"api/AdminOrders/{selected.Id}/{action}", null);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        errorTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadOrdersAsync();
                SelectOrderById(selected.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                UpdateOrderDetailPanel(GetSelectedOrder());
            }
        }

        private void SetOrderActionButtonsEnabled(bool enabled)
        {
            btnPrepare.Enabled = enabled;
            btnShip.Enabled = enabled;
            btnDeliver.Enabled = enabled;
            btnCancelOrder.Enabled = enabled;
        }

        private void MusteriLabel_Click(object sender, EventArgs e)
        {

        }

        private void MusterıDetaylbl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        // ============================================================
        // MÜŞTERİLERİ YÜKLE
        // ============================================================

        private async Task LoadCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/AdminCustomers");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Müşteriler Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var customers =
                    await response.Content.ReadFromJsonAsync<List<CustomerDto>>()
                    ?? new List<CustomerDto>();

                foreach (var customer in customers)
                {
                    customer.OrderCount = _allOrders.Count(x => x.CustomerId == customer.CustomerId);
                }

                dataGridView2.DataSource = customers;
                ConfigureCustomerGridColumns();
                UpdateCustomerDetailPanel(GetSelectedCustomer());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureCustomerGridColumns()
        {
            if (dataGridView2.Columns["UserId"] is { } userIdColumn)
            {
                userIdColumn.Visible = false;
            }

            if (dataGridView2.Columns["Phone"] is { } phoneColumn)
            {
                phoneColumn.Visible = false;
            }

            if (dataGridView2.Columns["CompanyName"] is { } companyColumn)
            {
                companyColumn.Visible = false;
            }

            if (dataGridView2.Columns["CreatedAtUtc"] is { } createdColumn)
            {
                createdColumn.Visible = false;
            }

            if (dataGridView2.Columns["CustomerId"] is { } idColumn)
            {
                idColumn.HeaderText = "ID";
            }

            if (dataGridView2.Columns["FullName"] is { } nameColumn)
            {
                nameColumn.HeaderText = "Ad Soyad";
            }

            if (dataGridView2.Columns["Email"] is { } emailColumn)
            {
                emailColumn.HeaderText = "E-posta";
            }

            if (dataGridView2.Columns["OrderCount"] is { } orderCountColumn)
            {
                orderCountColumn.HeaderText = "Sipariş";
            }

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private CustomerDto? GetSelectedCustomer()
        {
            return dataGridView2.CurrentRow?.DataBoundItem as CustomerDto;
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCustomerDetailPanel(GetSelectedCustomer());
        }

        private void UpdateCustomerDetailPanel(CustomerDto? customer)
        {
            Ad.Text = $"Ad: {customer?.FullName ?? "-"}";
            epostalabel.Text = $"E-posta: {customer?.Email ?? "-"}";
            telefonlabel.Text = $"Telefon: {customer?.Phone ?? "-"}";
            label3.Text = $"Firma: {customer?.CompanyName ?? "-"}";
            uyetarihilabel.Text = $"Üye tarihi: {(customer is null ? "-" : customer.CreatedAtUtc.ToString("dd.MM.yyyy"))}";

            var customerOrders =
                customer is null
                    ? new List<OrderDto>()
                    : _allOrders.Where(x => x.CustomerId == customer.CustomerId).ToList();

            dataGridView3.DataSource = customerOrders;
            ConfigureCustomerOrdersGridColumns();
        }

        private void ConfigureCustomerOrdersGridColumns()
        {
            foreach (var columnName in new[] { "Id", "CustomerId", "CustomerName", "CustomerEmail", "ConfirmedAtUtc", "ShippedAtUtc", "DeliveredAtUtc", "CancelledAtUtc" })
            {
                if (dataGridView3.Columns[columnName] is { } column)
                {
                    column.Visible = false;
                }
            }

            if (dataGridView3.Columns["OrderNumber"] is { } orderNumberColumn)
            {
                orderNumberColumn.HeaderText = "Sipariş No";
            }

            if (dataGridView3.Columns["Status"] is { } statusColumn)
            {
                statusColumn.HeaderText = "Durum";
            }

            if (dataGridView3.Columns["CreatedAtUtc"] is { } createdColumn)
            {
                createdColumn.HeaderText = "Tarih";
            }

            if (dataGridView3.Columns["TotalAmount"] is { } amountColumn)
            {
                amountColumn.HeaderText = "Tutar";
                amountColumn.DefaultCellStyle.Format = "N2";
            }

            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void TarihLbl_Click(object sender, EventArgs e)
        {

        }

        private void mesajTextBox_TextChanged(object sender, EventArgs e)
        {

        }


        // DESTEK SOHBETLERİNİ YÜKLE

        private async Task LoadSupportConversationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/AdminSupportConversations");

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

                dataGridView4.DataSource = conversations;
                ConfigureSupportGridColumns();
                await UpdateSupportDetailPanelAsync(GetSelectedConversation());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Sunucuya bağlanırken bir hata oluştu:\n{ex.Message}\n\nLütfen MiniCrm.Api projesinin çalıştığından emin olun!",
                    "Bağlantı Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigureSupportGridColumns()
        {
            foreach (var columnName in new[] { "CustomerId", "CustomerEmail", "OrderId", "UpdatedAtUtc" })
            {
                if (dataGridView4.Columns[columnName] is { } column)
                {
                    column.Visible = false;
                }
            }

            if (dataGridView4.Columns["Id"] is { } idColumn)
            {
                idColumn.HeaderText = "ID";
            }

            if (dataGridView4.Columns["CustomerName"] is { } nameColumn)
            {
                nameColumn.HeaderText = "Müşteri";
            }

            if (dataGridView4.Columns["Status"] is { } statusColumn)
            {
                statusColumn.HeaderText = "Durum";
            }

            if (dataGridView4.Columns["OrderNumber"] is { } orderNumberColumn)
            {
                orderNumberColumn.HeaderText = "Sipariş No";
            }

            if (dataGridView4.Columns["CreatedAtUtc"] is { } createdColumn)
            {
                createdColumn.HeaderText = "Tarih";
            }

            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private SupportConversationSummaryDto? GetSelectedConversation()
        {
            return dataGridView4.CurrentRow?.DataBoundItem as SupportConversationSummaryDto;
        }

        private void SelectConversationById(int conversationId)
        {
            foreach (DataGridViewRow row in dataGridView4.Rows)
            {
                if (row.DataBoundItem is SupportConversationSummaryDto conversation && conversation.Id == conversationId)
                {
                    dataGridView4.CurrentCell = row.Cells[0];
                    return;
                }
            }
        }

        private async void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            await UpdateSupportDetailPanelAsync(GetSelectedConversation());
        }

        private async Task UpdateSupportDetailPanelAsync(SupportConversationSummaryDto? conversation)
        {
            if (conversation is null)
            {
                musterıLbl.Text = "Müşteri: -";
                epostaLbl.Text = "E-posta: -";
                DurumLbl.Text = "Durum: -";
                TarihLbl.Text = "Tarih: -";
                siparisNolbl.Text = "Sipariş No: -";
                MesajlarTextBox.Text = string.Empty;
                mesajTextBox.Text = string.Empty;
                yanitbutton.Enabled = false;
                mesajTextBox.Enabled = false;
                konusmayıkapatButton.Enabled = false;
                return;
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/AdminSupportConversations/{conversation.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Sohbet Detayı Yüklenemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var detail =
                    await response.Content.ReadFromJsonAsync<AdminSupportConversationDetailDto>();

                if (detail is null)
                {
                    return;
                }

                musterıLbl.Text = $"Müşteri: {detail.CustomerName}";
                epostaLbl.Text = $"E-posta: {detail.CustomerEmail}";
                DurumLbl.Text = $"Durum: {detail.Conversation.Status}";
                TarihLbl.Text = $"Tarih: {detail.Conversation.CreatedAtUtc:dd.MM.yyyy HH:mm}";
                siparisNolbl.Text = $"Sipariş No: {detail.Conversation.OrderNumber ?? "-"}";

                var isClosed = detail.Conversation.Status == "Closed";
                yanitbutton.Enabled = !isClosed;
                mesajTextBox.Enabled = !isClosed;
                konusmayıkapatButton.Enabled = !isClosed;

                var mesajSatirlari = detail.Conversation.Messages.Select(message =>
                {
                    var gonderen = message.SenderType == "Customer" ? "Müşteri" : "Admin";
                    return $"[{message.CreatedAtUtc:dd.MM HH:mm}] {gonderen}: {message.Message}";
                });

                MesajlarTextBox.Text = string.Join(Environment.NewLine + Environment.NewLine, mesajSatirlari);
                MesajlarTextBox.SelectionStart = MesajlarTextBox.Text.Length;
                MesajlarTextBox.ScrollToCaret();

                mesajTextBox.Text = string.Empty;
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


        // yanit gonderme

        private async void yanitbutton_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedConversation();

            if (selected is null)
            {
                MessageBox.Show(
                    "Lütfen bir destek sohbeti seçin.",
                    "Sohbet Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(mesajTextBox.Text))
            {
                MessageBox.Show(
                    "Lütfen bir mesaj yazın.",
                    "Mesaj Boş",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            yanitbutton.Enabled = false;
            konusmayıkapatButton.Enabled = false;

            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                    $"api/AdminSupportConversations/{selected.Id}/messages",
                    new AddSupportMessageRequestDto { Message = mesajTextBox.Text.Trim() });

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Mesaj Gönderilemedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadSupportConversationsAsync();
                SelectConversationById(selected.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                await UpdateSupportDetailPanelAsync(GetSelectedConversation());
            }
        }


        
        // KONUŞMAYI KAPAT
        

        private async void konusmayıkapatButton_Click(object sender, EventArgs e)
        {
            var selected = GetSelectedConversation();

            if (selected is null)
            {
                MessageBox.Show(
                    "Lütfen kapatmak için bir destek sohbeti seçin.",
                    "Sohbet Seçilmedi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var onay = MessageBox.Show(
                $"\"{selected.CustomerName}\" ile olan konuşmayı kapatmak istediğinize emin misiniz?",
                "Konuşmayı Kapat",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (onay != DialogResult.Yes)
            {
                return;
            }

            yanitbutton.Enabled = false;
            konusmayıkapatButton.Enabled = false;

            try
            {
                var response = await _httpClient.PostAsync($"api/AdminSupportConversations/{selected.Id}/close", null);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        await ReadErrorMessageAsync(response),
                        "Konuşma Kapatılamadı",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                await LoadSupportConversationsAsync();
                SelectConversationById(selected.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Bir hata oluştu:\n{ex.Message}",
                    "Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                await UpdateSupportDetailPanelAsync(GetSelectedConversation());
            }
        }
    }


    // API MODELLERİ

    public class ProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateProductRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class UpdateProductRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class AdjustStockRequestDto
    {
        public int QuantityChange { get; set; }
        public string? Note { get; set; }
    }

    public class ProblemDetailsDto
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
    }

    public class CreateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class UpdateCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ConfirmedAtUtc { get; set; }
        public DateTime? ShippedAtUtc { get; set; }
        public DateTime? DeliveredAtUtc { get; set; }
        public DateTime? CancelledAtUtc { get; set; }
    }

    public class CancelOrderRequestDto
    {
        public string Reason { get; set; } = string.Empty;
    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int OrderCount { get; set; }
    }

    public class SupportConversationSummaryDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
    }

    public class SupportMessageDto
    {
        public int Id { get; set; }
        public Guid SenderUserId { get; set; }
        public string SenderType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }
    }

    public class SupportConversationDetailDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public List<SupportMessageDto> Messages { get; set; } = new();
    }

    public class AdminSupportConversationDetailDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public SupportConversationDetailDto Conversation { get; set; } = new();
    }

    public class AddSupportMessageRequestDto
    {
        public string Message { get; set; } = string.Empty;
    }
}
