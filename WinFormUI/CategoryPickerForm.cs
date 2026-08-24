using System.Windows.Forms;

namespace WinFormUI
{
    public partial class CategoryPickerForm : Form
    {
        public CategoryDto? SelectedCategory => cmbCategory.SelectedItem as CategoryDto;


        public CategoryPickerForm(List<CategoryDto> categories, string promptText)
        {
            InitializeComponent();

            lblPrompt.Text = promptText;

            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = nameof(CategoryDto.Name);
            cmbCategory.ValueMember = nameof(CategoryDto.Id);
        }
    }
}
