using System.Windows.Forms;

namespace WinFormUI
{
    public partial class LeadEditForm : Form
    {
        private static readonly ComboOption<int>[] SourceOptions =
        {
            new("Web Sitesi", 1),
            new("Telefon", 2),
            new("E-posta", 3),
            new("Sosyal Medya", 4),
            new("Fuar", 5),
            new("Referans", 6),
            new("Diğer", 99)
        };

        private readonly bool _isEditMode;

        public string FullNameValue => txtFullName.Text.Trim();
        public string? CompanyNameValue => string.IsNullOrWhiteSpace(txtCompanyName.Text) ? null : txtCompanyName.Text.Trim();
        public string EmailValue => txtEmail.Text.Trim();
        public string? PhoneValue => string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
        public int SourceValue => cmbSource.SelectedValue is int value ? value : 1;
        public string? InterestAreaValue => string.IsNullOrWhiteSpace(txtInterestArea.Text) ? null : txtInterestArea.Text.Trim();
        public string? NotesValue => string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();
        public DateTime? NextFollowUpDateValue => chkFollowUp.Checked ? dtpFollowUp.Value.Date : null;

        public LeadEditForm(LeadDto? existingLead = null)
        {
            InitializeComponent();

            cmbSource.DataSource = SourceOptions;
            cmbSource.DisplayMember = "Label";
            cmbSource.ValueMember = "Value";

            _isEditMode = existingLead is not null;

            Text = _isEditMode ? "Lead Düzenle" : "Yeni Lead";

            if (existingLead is not null)
            {
                txtFullName.Text = existingLead.FullName;
                txtCompanyName.Text = existingLead.CompanyName;
                txtEmail.Text = existingLead.Email;
                txtPhone.Text = existingLead.Phone;
                txtInterestArea.Text = existingLead.InterestArea;
                txtNotes.Text = existingLead.Notes;

                cmbSource.SelectedValue = MapSourceNameToValue(existingLead.Source);

                if (existingLead.NextFollowUpDate is { } date)
                {
                    chkFollowUp.Checked = true;
                    dtpFollowUp.Value = date;
                }
                else
                {
                    chkFollowUp.Checked = false;
                }
            }
        }

        private static int MapSourceNameToValue(string sourceName)
        {
            return sourceName switch
            {
                "Website" => 1,
                "PhoneCall" => 2,
                "Email" => 3,
                "SocialMedia" => 4,
                "Fair" => 5,
                "Reference" => 6,
                _ => 99
            };
        }

        private void chkFollowUp_CheckedChanged(object sender, EventArgs e)
        {
            dtpFollowUp.Enabled = chkFollowUp.Checked;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Ad soyad boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("E-posta boş bırakılamaz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }
    }
}
