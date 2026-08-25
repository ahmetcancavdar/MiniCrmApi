namespace WinFormUI
{
    partial class CustomerPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            button2 = new Button();
            CustomerTabControl = new TabControl();
            UrunlerTabPage = new TabPage();
            dataGridView1 = new DataGridView();
            SepetimTabPage = new TabPage();
            lblSepetBos = new Label();
            panelSepetDetay = new Panel();
            ToplamLabel = new Label();
            SiparisVerButton = new Button();
            SepetTemizleButton = new Button();
            dataGridView2 = new DataGridView();
            SiparislerimTabPage = new TabPage();
            lblSiparisYok = new Label();
            panel1 = new Panel();
            SiparisOnaylaButton = new Button();
            SiparisIptalButton = new Button();
            UrunlerLıstBox = new ListBox();
            DurumLabel = new Label();
            SıparısNoLabel = new Label();
            dataGridView3 = new DataGridView();
            DestekTabPage = new TabPage();
            DestekTalebiButton = new Button();
            DestekSplitContainer = new SplitContainer();
            CustomerDestekDataGridView = new DataGridView();
            DestekDetayTableLayoutPanel = new TableLayoutPanel();
            DestekDurumu = new Label();
            MesajlarAraCızgıLabel = new Label();
            listBox1 = new TextBox();
            YanıtGondermeTextBox = new TextBox();
            YanıtGondermeButton = new Button();
            CustomerTabControl.SuspendLayout();
            UrunlerTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SepetimTabPage.SuspendLayout();
            panelSepetDetay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SiparislerimTabPage.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            DestekTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DestekSplitContainer).BeginInit();
            DestekSplitContainer.Panel1.SuspendLayout();
            DestekSplitContainer.Panel2.SuspendLayout();
            DestekSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CustomerDestekDataGridView).BeginInit();
            DestekDetayTableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(330, 9);
            label1.Name = "label1";
            label1.Size = new Size(134, 25);
            label1.TabIndex = 0;
            label1.Text = "Müşteri Paneli";
            label1.Click += label1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(767, 9);
            button2.Name = "button2";
            button2.Size = new Size(30, 29);
            button2.TabIndex = 2;
            button2.Text = "⚙";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // CustomerTabControl
            // 
            CustomerTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomerTabControl.Controls.Add(UrunlerTabPage);
            CustomerTabControl.Controls.Add(SepetimTabPage);
            CustomerTabControl.Controls.Add(SiparislerimTabPage);
            CustomerTabControl.Controls.Add(DestekTabPage);
            CustomerTabControl.Location = new Point(-1, 44);
            CustomerTabControl.Name = "CustomerTabControl";
            CustomerTabControl.SelectedIndex = 0;
            CustomerTabControl.Size = new Size(809, 411);
            CustomerTabControl.TabIndex = 3;
            // 
            // UrunlerTabPage
            // 
            UrunlerTabPage.Controls.Add(dataGridView1);
            UrunlerTabPage.Location = new Point(4, 29);
            UrunlerTabPage.Name = "UrunlerTabPage";
            UrunlerTabPage.Padding = new Padding(3);
            UrunlerTabPage.Size = new Size(791, 378);
            UrunlerTabPage.TabIndex = 0;
            UrunlerTabPage.Text = "Ürünler";
            UrunlerTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(785, 372);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // SepetimTabPage
            // 
            SepetimTabPage.Controls.Add(lblSepetBos);
            SepetimTabPage.Controls.Add(panelSepetDetay);
            SepetimTabPage.Controls.Add(dataGridView2);
            SepetimTabPage.Location = new Point(4, 29);
            SepetimTabPage.Name = "SepetimTabPage";
            SepetimTabPage.Padding = new Padding(3);
            SepetimTabPage.Size = new Size(791, 378);
            SepetimTabPage.TabIndex = 1;
            SepetimTabPage.Text = "Sepetim";
            SepetimTabPage.UseVisualStyleBackColor = true;
            // 
            // lblSepetBos
            // 
            lblSepetBos.AutoSize = true;
            lblSepetBos.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblSepetBos.ForeColor = SystemColors.GrayText;
            lblSepetBos.Location = new Point(250, 175);
            lblSepetBos.Name = "lblSepetBos";
            lblSepetBos.Size = new Size(129, 28);
            lblSepetBos.TabIndex = 1;
            lblSepetBos.Text = "Sepetiniz boş";
            lblSepetBos.Visible = false;
            // 
            // panelSepetDetay
            // 
            panelSepetDetay.Controls.Add(ToplamLabel);
            panelSepetDetay.Controls.Add(SiparisVerButton);
            panelSepetDetay.Controls.Add(SepetTemizleButton);
            panelSepetDetay.Dock = DockStyle.Right;
            panelSepetDetay.Location = new Point(599, 3);
            panelSepetDetay.Name = "panelSepetDetay";
            panelSepetDetay.Size = new Size(189, 372);
            panelSepetDetay.TabIndex = 2;
            // 
            // ToplamLabel
            // 
            ToplamLabel.AutoSize = true;
            ToplamLabel.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 162);
            ToplamLabel.ForeColor = Color.DarkOrange;
            ToplamLabel.Location = new Point(30, 48);
            ToplamLabel.Name = "ToplamLabel";
            ToplamLabel.Size = new Size(82, 25);
            ToplamLabel.TabIndex = 1;
            ToplamLabel.Text = "Toplam:";
            ToplamLabel.Click += label2_Click;
            // 
            // SiparisVerButton
            // 
            SiparisVerButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            SiparisVerButton.Location = new Point(30, 106);
            SiparisVerButton.Name = "SiparisVerButton";
            SiparisVerButton.Size = new Size(122, 29);
            SiparisVerButton.TabIndex = 2;
            SiparisVerButton.Text = "Sipariş Ver";
            SiparisVerButton.UseVisualStyleBackColor = true;
            SiparisVerButton.Click += SiparisVerButton_Click;
            // 
            // SepetTemizleButton
            // 
            SepetTemizleButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            SepetTemizleButton.Location = new Point(30, 161);
            SepetTemizleButton.Name = "SepetTemizleButton";
            SepetTemizleButton.Size = new Size(122, 29);
            SepetTemizleButton.TabIndex = 3;
            SepetTemizleButton.Text = "Sepeti Temizle";
            SepetTemizleButton.UseVisualStyleBackColor = true;
            SepetTemizleButton.Click += SepetTemizleButton_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 3);
            dataGridView2.MultiSelect = false;
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Size = new Size(785, 372);
            dataGridView2.TabIndex = 0;
            // 
            // SiparislerimTabPage
            // 
            SiparislerimTabPage.Controls.Add(lblSiparisYok);
            SiparislerimTabPage.Controls.Add(panel1);
            SiparislerimTabPage.Controls.Add(dataGridView3);
            SiparislerimTabPage.Location = new Point(4, 29);
            SiparislerimTabPage.Name = "SiparislerimTabPage";
            SiparislerimTabPage.Padding = new Padding(3);
            SiparislerimTabPage.Size = new Size(791, 378);
            SiparislerimTabPage.TabIndex = 2;
            SiparislerimTabPage.Text = "Siparişlerim";
            SiparislerimTabPage.UseVisualStyleBackColor = true;
            // 
            // lblSiparisYok
            // 
            lblSiparisYok.AutoSize = true;
            lblSiparisYok.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            lblSiparisYok.ForeColor = SystemColors.GrayText;
            lblSiparisYok.Location = new Point(280, 85);
            lblSiparisYok.Name = "lblSiparisYok";
            lblSiparisYok.Size = new Size(188, 28);
            lblSiparisYok.TabIndex = 2;
            lblSiparisYok.Text = "Sipariş verilmemiştir";
            lblSiparisYok.Visible = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(SiparisOnaylaButton);
            panel1.Controls.Add(SiparisIptalButton);
            panel1.Controls.Add(UrunlerLıstBox);
            panel1.Controls.Add(DurumLabel);
            panel1.Controls.Add(SıparısNoLabel);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 178);
            panel1.Name = "panel1";
            panel1.Size = new Size(785, 197);
            panel1.TabIndex = 1;
            // 
            // SiparisOnaylaButton
            // 
            SiparisOnaylaButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            SiparisOnaylaButton.Location = new Point(9, 121);
            SiparisOnaylaButton.Name = "SiparisOnaylaButton";
            SiparisOnaylaButton.Size = new Size(137, 29);
            SiparisOnaylaButton.TabIndex = 4;
            SiparisOnaylaButton.Text = "Siparişi Onayla";
            SiparisOnaylaButton.UseVisualStyleBackColor = true;
            SiparisOnaylaButton.Click += SiparisOnaylaButton_Click;
            // 
            // SiparisIptalButton
            // 
            SiparisIptalButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            SiparisIptalButton.Location = new Point(9, 86);
            SiparisIptalButton.Name = "SiparisIptalButton";
            SiparisIptalButton.Size = new Size(137, 29);
            SiparisIptalButton.TabIndex = 3;
            SiparisIptalButton.Text = "Siparişi İptal Et";
            SiparisIptalButton.UseVisualStyleBackColor = true;
            SiparisIptalButton.Click += SiparisIptalButton_Click;
            // 
            // UrunlerLıstBox
            // 
            UrunlerLıstBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UrunlerLıstBox.Location = new Point(410, 3);
            UrunlerLıstBox.Name = "UrunlerLıstBox";
            UrunlerLıstBox.Size = new Size(372, 184);
            UrunlerLıstBox.TabIndex = 2;
            // 
            // DurumLabel
            // 
            DurumLabel.AutoSize = true;
            DurumLabel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            DurumLabel.Location = new Point(9, 47);
            DurumLabel.Name = "DurumLabel";
            DurumLabel.Size = new Size(71, 23);
            DurumLabel.TabIndex = 1;
            DurumLabel.Text = "Durum:";
            DurumLabel.Click += label2_Click_1;
            // 
            // SıparısNoLabel
            // 
            SıparısNoLabel.AutoSize = true;
            SıparısNoLabel.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            SıparısNoLabel.Location = new Point(9, 16);
            SıparısNoLabel.Name = "SıparısNoLabel";
            SıparısNoLabel.Size = new Size(97, 23);
            SıparısNoLabel.TabIndex = 0;
            SıparısNoLabel.Text = "Sipariş No:";
            // 
            // dataGridView3
            // 
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Dock = DockStyle.Fill;
            dataGridView3.Location = new Point(3, 3);
            dataGridView3.MultiSelect = false;
            dataGridView3.Name = "dataGridView3";
            dataGridView3.ReadOnly = true;
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.Size = new Size(785, 372);
            dataGridView3.TabIndex = 0;
            dataGridView3.SelectionChanged += dataGridView3_SelectionChanged;
            // 
            // DestekTabPage
            //
            DestekTabPage.Controls.Add(DestekSplitContainer);
            DestekTabPage.Controls.Add(DestekTalebiButton);
            DestekTabPage.Location = new Point(4, 29);
            DestekTabPage.Name = "DestekTabPage";
            DestekTabPage.Padding = new Padding(3);
            DestekTabPage.Size = new Size(801, 378);
            DestekTabPage.TabIndex = 3;
            DestekTabPage.Text = "Destek";
            DestekTabPage.UseVisualStyleBackColor = true;
            //
            // DestekTalebiButton
            //
            DestekTalebiButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            DestekTalebiButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            DestekTalebiButton.Location = new Point(9, 6);
            DestekTalebiButton.Name = "DestekTalebiButton";
            DestekTalebiButton.Size = new Size(158, 29);
            DestekTalebiButton.TabIndex = 0;
            DestekTalebiButton.Text = "Yeni Destek Oluştur";
            DestekTalebiButton.UseVisualStyleBackColor = true;
            DestekTalebiButton.Click += DestekTalebiButton_Click;
            //
            // DestekSplitContainer
            //
            // İki panelin (sol: talep listesi, sağ: konuşma detayı) HİÇBİR
            // koşulda birbirine girmemesi/üst üste binmemesi için elle Anchor
            // hesaplamak yerine SplitContainer kullanılıyor; bu kontrol iki
            // panelin sınırlarını yapısal olarak asla çakıştırmayacak şekilde
            // garanti eder. Sayfa tam ortadan (soldaki liste biraz daha dar
            // kalacak şekilde) bölünüyor; bu oran CustomerPage_Resize'da
            // pencere boyutuna göre yeniden hesaplanıyor (bkz. CustomerPage.cs).
            DestekSplitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DestekSplitContainer.FixedPanel = FixedPanel.None;
            DestekSplitContainer.Location = new Point(9, 41);
            DestekSplitContainer.Name = "DestekSplitContainer";
            DestekSplitContainer.Panel1.Controls.Add(CustomerDestekDataGridView);
            DestekSplitContainer.Panel1MinSize = 220;
            DestekSplitContainer.Panel2.Controls.Add(DestekDetayTableLayoutPanel);
            DestekSplitContainer.Panel2MinSize = 260;
            DestekSplitContainer.Size = new Size(783, 334);
            DestekSplitContainer.SplitterDistance = 360;
            DestekSplitContainer.SplitterWidth = 6;
            DestekSplitContainer.TabIndex = 1;
            //
            // CustomerDestekDataGridView
            //
            CustomerDestekDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            CustomerDestekDataGridView.Dock = DockStyle.Fill;
            CustomerDestekDataGridView.Location = new Point(0, 0);
            CustomerDestekDataGridView.Name = "CustomerDestekDataGridView";
            CustomerDestekDataGridView.RowHeadersWidth = 51;
            CustomerDestekDataGridView.Size = new Size(360, 334);
            CustomerDestekDataGridView.TabIndex = 1;
            CustomerDestekDataGridView.SelectionChanged += CustomerDestekDataGridView_SelectionChanged;
            //
            // DestekDetayTableLayoutPanel
            //
            // Mesaj kutusunun (listBox1) sağ paneli GÜVENİLİR şekilde
            // doldurması için Anchor yerine TableLayoutPanel kullanılıyor:
            // Durum/başlık/yanıt kutusu/gönder butonu sabit yükseklikte,
            // mesaj kutusunun satırı ise %100 (Percent) olarak ayarlı —
            // bu da kalan tüm dikey alanı listBox1'e matematiksel olarak
            // garanti eder.
            DestekDetayTableLayoutPanel.ColumnCount = 1;
            DestekDetayTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            DestekDetayTableLayoutPanel.Controls.Add(DestekDurumu, 0, 0);
            DestekDetayTableLayoutPanel.Controls.Add(MesajlarAraCızgıLabel, 0, 1);
            DestekDetayTableLayoutPanel.Controls.Add(listBox1, 0, 2);
            DestekDetayTableLayoutPanel.Controls.Add(YanıtGondermeTextBox, 0, 3);
            DestekDetayTableLayoutPanel.Controls.Add(YanıtGondermeButton, 0, 4);
            DestekDetayTableLayoutPanel.Dock = DockStyle.Fill;
            DestekDetayTableLayoutPanel.Location = new Point(0, 0);
            DestekDetayTableLayoutPanel.Name = "DestekDetayTableLayoutPanel";
            DestekDetayTableLayoutPanel.Padding = new Padding(7);
            DestekDetayTableLayoutPanel.RowCount = 5;
            DestekDetayTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            DestekDetayTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            DestekDetayTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            DestekDetayTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            DestekDetayTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            DestekDetayTableLayoutPanel.Size = new Size(417, 334);
            DestekDetayTableLayoutPanel.TabIndex = 2;
            //
            // DestekDurumu
            //
            DestekDurumu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            DestekDurumu.AutoSize = true;
            DestekDurumu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            DestekDurumu.Margin = new Padding(0, 0, 0, 4);
            DestekDurumu.Name = "DestekDurumu";
            DestekDurumu.Size = new Size(62, 20);
            DestekDurumu.TabIndex = 0;
            DestekDurumu.Text = "Durum:";
            //
            // MesajlarAraCızgıLabel
            //
            MesajlarAraCızgıLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            MesajlarAraCızgıLabel.AutoSize = true;
            MesajlarAraCızgıLabel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            MesajlarAraCızgıLabel.Margin = new Padding(0, 0, 0, 4);
            MesajlarAraCızgıLabel.Name = "MesajlarAraCızgıLabel";
            MesajlarAraCızgıLabel.Size = new Size(366, 20);
            MesajlarAraCızgıLabel.TabIndex = 1;
            MesajlarAraCızgıLabel.Text = "-------------------------Mesajlar-------------------------";
            //
            // listBox1
            //
            // Uzun mesajların kesilmeden alt satıra geçmesi (word-wrap) ve
            // metnin seçilip kopyalanabilmesi için ListBox yerine salt-okunur,
            // çok satırlı bir TextBox kullanılıyor; ListBox tek satır çizer ve
            // metin seçimini desteklemez.
            listBox1.BackColor = SystemColors.Window;
            listBox1.Dock = DockStyle.Fill;
            listBox1.Margin = new Padding(0, 0, 0, 6);
            listBox1.Multiline = true;
            listBox1.Name = "listBox1";
            listBox1.ReadOnly = true;
            listBox1.ScrollBars = ScrollBars.Vertical;
            listBox1.Size = new Size(403, 168);
            listBox1.TabIndex = 2;
            //
            // YanıtGondermeTextBox
            //
            YanıtGondermeTextBox.Dock = DockStyle.Fill;
            YanıtGondermeTextBox.Margin = new Padding(0, 0, 0, 4);
            YanıtGondermeTextBox.Name = "YanıtGondermeTextBox";
            YanıtGondermeTextBox.Size = new Size(403, 30);
            YanıtGondermeTextBox.TabIndex = 3;
            YanıtGondermeTextBox.Text = "Yanıt yazınız...";
            //
            // YanıtGondermeButton
            //
            YanıtGondermeButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            YanıtGondermeButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            YanıtGondermeButton.Name = "YanıtGondermeButton";
            YanıtGondermeButton.Size = new Size(115, 32);
            YanıtGondermeButton.TabIndex = 4;
            YanıtGondermeButton.Text = "Gönder";
            YanıtGondermeButton.UseVisualStyleBackColor = true;
            YanıtGondermeButton.Click += YanıtGondermeButton_Click;
            //
            // CustomerPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(809, 449);
            Controls.Add(CustomerTabControl);
            Controls.Add(button2);
            Controls.Add(label1);
            MinimumSize = new Size(650, 400);
            Name = "CustomerPage";
            Text = "CustomerPage";
            Load += CustomerPage_Load;
            Resize += CustomerPage_Resize;
            CustomerTabControl.ResumeLayout(false);
            UrunlerTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            SepetimTabPage.ResumeLayout(false);
            SepetimTabPage.PerformLayout();
            panelSepetDetay.ResumeLayout(false);
            panelSepetDetay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            SiparislerimTabPage.ResumeLayout(false);
            SiparislerimTabPage.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            DestekTabPage.ResumeLayout(false);
            DestekSplitContainer.Panel1.ResumeLayout(false);
            DestekSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DestekSplitContainer).EndInit();
            DestekSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CustomerDestekDataGridView).EndInit();
            DestekDetayTableLayoutPanel.ResumeLayout(false);
            DestekDetayTableLayoutPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button2;
        private TabControl CustomerTabControl;
        private TabPage UrunlerTabPage;
        private TabPage SepetimTabPage;
        private TabPage SiparislerimTabPage;
        private TabPage DestekTabPage;
        private DataGridView dataGridView1;
        private Label ToplamLabel;
        private DataGridView dataGridView2;
        private Label lblSepetBos;
        private Panel panelSepetDetay;
        private Button SepetTemizleButton;
        private Button SiparisVerButton;
        private Panel panel1;
        private Label DurumLabel;
        private Label SıparısNoLabel;
        private DataGridView dataGridView3;
        private ListBox UrunlerLıstBox;
        private Button SiparisIptalButton;
        private Button SiparisOnaylaButton;
        private Label lblSiparisYok;
        private Button DestekTalebiButton;
        private SplitContainer DestekSplitContainer;
        private TableLayoutPanel DestekDetayTableLayoutPanel;
        private DataGridView CustomerDestekDataGridView;
        private Label DestekDurumu;
        private Label MesajlarAraCızgıLabel;
        private TextBox YanıtGondermeTextBox;
        private TextBox listBox1;
        private Button YanıtGondermeButton;
    }
}