using System.Windows.Forms;

namespace WinFormUI
{
    public partial class ProductEditForm : Form
    {
        private readonly bool _isEditMode;

        public int CategoryId => (int)(cmbCategory.SelectedValue ?? 0);
        public string ProductNameValue => txtProductName.Text.Trim();
        public string Sku => txtSku.Text.Trim();
        public string? Description => string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();
        public decimal Price => numPrice.Value;
        public int StockQuantity => (int)numStock.Value;
        public string? ImageUrl => string.IsNullOrWhiteSpace(txtImageUrl.Text) ? null : txtImageUrl.Text.Trim();
        public bool IsActive => chkActive.Checked;


        public ProductEditForm(List<CategoryDto> categories, ProductDto? existingProduct = null)
        {
            InitializeComponent();

            _isEditMode = existingProduct is not null;

            Text = _isEditMode ? "Ürün Düzenle" : "Ürün Ekle";
            lblStock.Text = _isEditMode ? "Stok" : "Başlangıç Stoğu";

            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = nameof(CategoryDto.Name);
            cmbCategory.ValueMember = nameof(CategoryDto.Id);

            if (!_isEditMode)
            {
                chkActive.Visible = false;
                ClientSize = new Size(ClientSize.Width, 335);
            }

            if (existingProduct is not null)
            {
                cmbCategory.SelectedValue = existingProduct.CategoryId;
                txtProductName.Text = existingProduct.Name;
                txtSku.Text = existingProduct.Sku;
                txtDescription.Text = existingProduct.Description;
                numPrice.Value = Math.Clamp(existingProduct.Price, numPrice.Minimum, numPrice.Maximum);
                numStock.Value = Math.Clamp(existingProduct.StockQuantity, (int)numStock.Minimum, (int)numStock.Maximum);
                txtImageUrl.Text = existingProduct.ImageUrl;
                chkActive.Checked = existingProduct.IsActive;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue is null)
            {
                MessageBox.Show("Lütfen bir kategori seçin.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtProductName.Text) || string.IsNullOrWhiteSpace(txtSku.Text))
            {
                MessageBox.Show("Ürün adı ve SKU boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }


    }
}
