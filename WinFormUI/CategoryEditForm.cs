using System.Windows.Forms;

namespace WinFormUI
{
    public partial class CategoryEditForm : Form
    {
        private readonly bool _isEditMode;

        public string CategoryName => txtName.Text.Trim();
        public string? Description => string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();
        public bool IsActive => chkActive.Checked;


        public CategoryEditForm(CategoryDto? existingCategory = null)
        {
            InitializeComponent();

            _isEditMode = existingCategory is not null;

            Text = _isEditMode ? "Kategori Düzenle" : "Kategori Ekle";

            if (!_isEditMode)
            {
                chkActive.Visible = false;
                ClientSize = new Size(ClientSize.Width, 190);
            }

            if (existingCategory is not null)
            {
                txtName.Text = existingCategory.Name;
                txtDescription.Text = existingCategory.Description;
                chkActive.Checked = existingCategory.IsActive;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Kategori adı boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
