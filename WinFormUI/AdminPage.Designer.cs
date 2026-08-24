namespace WinFormUI
{
    partial class AdminPage
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
            button1 = new Button();
            Müşteriler = new TabPage();
            MusterıDetaylbl = new Panel();
            MusterıSıparıslerılabel = new Label();
            uyetarihilabel = new Label();
            label3 = new Label();
            telefonlabel = new Label();
            epostalabel = new Label();
            Ad = new Label();
            label2 = new Label();
            dataGridView3 = new DataGridView();
            MusteriLabel = new Label();
            dataGridView2 = new DataGridView();
            Siparişler = new TabPage();
            panel1 = new Panel();
            lblOrderNo = new Label();
            lblOrderCustomer = new Label();
            lblOrderAmount = new Label();
            lblOrderStatus = new Label();
            btnPrepare = new Button();
            btnShip = new Button();
            btnDeliver = new Button();
            btnCancelOrder = new Button();
            SıparısLabel = new Label();
            SıparısGrıdVıew = new DataGridView();
            AdminPageControl = new TabControl();
            Ürünler = new TabPage();
            dataGridView1 = new DataGridView();
            panel2 = new Panel();
            KategoriDuzenle = new Button();
            kategoriSil = new Button();
            KategoriAddButton = new Button();
            SilButton = new Button();
            DuzenlemeButton = new Button();
            EklemeButton = new Button();
            Destek = new TabPage();
            panelDestekDetay = new Panel();
            Destekdetaylbl = new Label();
            musterıLbl = new Label();
            epostaLbl = new Label();
            DurumLbl = new Label();
            TarihLbl = new Label();
            siparisNolbl = new Label();
            MesajlarLbl = new Label();
            MesajlarListBox = new ListBox();
            mesajTextBox = new TextBox();
            yanitbutton = new Button();
            konusmayıkapatButton = new Button();
            TalepLbl = new Label();
            dataGridView4 = new DataGridView();
            Müşteriler.SuspendLayout();
            MusterıDetaylbl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            Siparişler.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)SıparısGrıdVıew).BeginInit();
            AdminPageControl.SuspendLayout();
            Ürünler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            Destek.SuspendLayout();
            panelDestekDetay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(334, 9);
            label1.Name = "label1";
            label1.Size = new Size(125, 25);
            label1.TabIndex = 0;
            label1.Text = "Admin Paneli";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(754, 9);
            button1.Name = "button1";
            button1.Size = new Size(30, 29);
            button1.TabIndex = 1;
            button1.Text = "⚙";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Müşteriler
            // 
            Müşteriler.Controls.Add(MusterıDetaylbl);
            Müşteriler.Controls.Add(MusteriLabel);
            Müşteriler.Controls.Add(dataGridView2);
            Müşteriler.Location = new Point(4, 29);
            Müşteriler.Name = "Müşteriler";
            Müşteriler.Padding = new Padding(3);
            Müşteriler.Size = new Size(785, 411);
            Müşteriler.TabIndex = 3;
            Müşteriler.Text = "Müşteriler";
            // 
            // MusterıDetaylbl
            // 
            MusterıDetaylbl.Controls.Add(MusterıSıparıslerılabel);
            MusterıDetaylbl.Controls.Add(uyetarihilabel);
            MusterıDetaylbl.Controls.Add(label3);
            MusterıDetaylbl.Controls.Add(telefonlabel);
            MusterıDetaylbl.Controls.Add(epostalabel);
            MusterıDetaylbl.Controls.Add(Ad);
            MusterıDetaylbl.Controls.Add(label2);
            MusterıDetaylbl.Controls.Add(dataGridView3);
            MusterıDetaylbl.Dock = DockStyle.Right;
            MusterıDetaylbl.Location = new Point(510, 3);
            MusterıDetaylbl.Name = "MusterıDetaylbl";
            MusterıDetaylbl.Size = new Size(272, 405);
            MusterıDetaylbl.TabIndex = 3;
            MusterıDetaylbl.Paint += MusterıDetaylbl_Paint;
            // 
            // MusterıSıparıslerılabel
            // 
            MusterıSıparıslerılabel.AutoSize = true;
            MusterıSıparıslerılabel.Location = new Point(0, 197);
            MusterıSıparıslerılabel.Name = "MusterıSıparıslerılabel";
            MusterıSıparıslerılabel.Size = new Size(77, 20);
            MusterıSıparıslerılabel.TabIndex = 7;
            MusterıSıparıslerılabel.Text = "Siparişleri:";
            // 
            // uyetarihilabel
            // 
            uyetarihilabel.AutoSize = true;
            uyetarihilabel.Location = new Point(6, 133);
            uyetarihilabel.Name = "uyetarihilabel";
            uyetarihilabel.Size = new Size(75, 20);
            uyetarihilabel.TabIndex = 6;
            uyetarihilabel.Text = "Üye tarihi:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 113);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 5;
            label3.Text = "Firma:";
            // 
            // telefonlabel
            // 
            telefonlabel.AutoSize = true;
            telefonlabel.Location = new Point(6, 93);
            telefonlabel.Name = "telefonlabel";
            telefonlabel.Size = new Size(61, 20);
            telefonlabel.TabIndex = 4;
            telefonlabel.Text = "Telefon:";
            // 
            // epostalabel
            // 
            epostalabel.AutoSize = true;
            epostalabel.Location = new Point(6, 73);
            epostalabel.Name = "epostalabel";
            epostalabel.Size = new Size(63, 20);
            epostalabel.TabIndex = 3;
            epostalabel.Text = "E-posta:";
            // 
            // Ad
            // 
            Ad.AutoSize = true;
            Ad.Location = new Point(6, 53);
            Ad.Name = "Ad";
            Ad.Size = new Size(31, 20);
            Ad.TabIndex = 2;
            Ad.Text = "Ad:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(70, 20);
            label2.Name = "label2";
            label2.Size = new Size(105, 20);
            label2.TabIndex = 1;
            label2.Text = "Müşteri Detayı";
            // 
            // dataGridView3
            // 
            dataGridView3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(0, 220);
            dataGridView3.MultiSelect = false;
            dataGridView3.Name = "dataGridView3";
            dataGridView3.ReadOnly = true;
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView3.Size = new Size(272, 188);
            dataGridView3.TabIndex = 0;
            dataGridView3.CellContentClick += dataGridView3_CellContentClick;
            // 
            // MusteriLabel
            // 
            MusteriLabel.AutoSize = true;
            MusteriLabel.Location = new Point(0, 1);
            MusteriLabel.Name = "MusteriLabel";
            MusteriLabel.Size = new Size(102, 20);
            MusteriLabel.TabIndex = 2;
            MusteriLabel.Text = "Müşteri Listesi";
            MusteriLabel.Click += MusteriLabel_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(0, 23);
            dataGridView2.MultiSelect = false;
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Size = new Size(511, 393);
            dataGridView2.TabIndex = 0;
            dataGridView2.SelectionChanged += dataGridView2_SelectionChanged;
            // 
            // Siparişler
            // 
            Siparişler.Controls.Add(panel1);
            Siparişler.Controls.Add(SıparısLabel);
            Siparişler.Controls.Add(SıparısGrıdVıew);
            Siparişler.Location = new Point(4, 29);
            Siparişler.Name = "Siparişler";
            Siparişler.Padding = new Padding(3);
            Siparişler.Size = new Size(785, 411);
            Siparişler.TabIndex = 2;
            Siparişler.Text = "Siparişler";
            Siparişler.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblOrderNo);
            panel1.Controls.Add(lblOrderCustomer);
            panel1.Controls.Add(lblOrderAmount);
            panel1.Controls.Add(lblOrderStatus);
            panel1.Controls.Add(btnPrepare);
            panel1.Controls.Add(btnShip);
            panel1.Controls.Add(btnDeliver);
            panel1.Controls.Add(btnCancelOrder);
            panel1.Dock = DockStyle.Right;
            panel1.Location = new Point(516, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(266, 405);
            panel1.TabIndex = 2;
            // 
            // lblOrderNo
            // 
            lblOrderNo.AutoSize = true;
            lblOrderNo.Location = new Point(15, 15);
            lblOrderNo.Name = "lblOrderNo";
            lblOrderNo.Size = new Size(90, 20);
            lblOrderNo.TabIndex = 0;
            lblOrderNo.Text = "Sipariş No: -";
            // 
            // lblOrderCustomer
            // 
            lblOrderCustomer.AutoSize = true;
            lblOrderCustomer.Location = new Point(15, 45);
            lblOrderCustomer.Name = "lblOrderCustomer";
            lblOrderCustomer.Size = new Size(71, 20);
            lblOrderCustomer.TabIndex = 1;
            lblOrderCustomer.Text = "Müşteri: -";
            // 
            // lblOrderAmount
            // 
            lblOrderAmount.AutoSize = true;
            lblOrderAmount.Location = new Point(15, 75);
            lblOrderAmount.Name = "lblOrderAmount";
            lblOrderAmount.Size = new Size(56, 20);
            lblOrderAmount.TabIndex = 2;
            lblOrderAmount.Text = "Tutar: -";
            // 
            // lblOrderStatus
            // 
            lblOrderStatus.AutoSize = true;
            lblOrderStatus.Location = new Point(15, 105);
            lblOrderStatus.Name = "lblOrderStatus";
            lblOrderStatus.Size = new Size(67, 20);
            lblOrderStatus.TabIndex = 3;
            lblOrderStatus.Text = "Durum: -";
            // 
            // btnPrepare
            // 
            btnPrepare.Location = new Point(15, 150);
            btnPrepare.Name = "btnPrepare";
            btnPrepare.Size = new Size(190, 30);
            btnPrepare.TabIndex = 4;
            btnPrepare.Text = "Hazırlanıyor";
            btnPrepare.UseVisualStyleBackColor = true;
            btnPrepare.Click += btnPrepare_Click;
            // 
            // btnShip
            // 
            btnShip.Location = new Point(15, 190);
            btnShip.Name = "btnShip";
            btnShip.Size = new Size(190, 30);
            btnShip.TabIndex = 5;
            btnShip.Text = "Kargoya Ver";
            btnShip.UseVisualStyleBackColor = true;
            btnShip.Click += btnShip_Click;
            // 
            // btnDeliver
            // 
            btnDeliver.Location = new Point(15, 230);
            btnDeliver.Name = "btnDeliver";
            btnDeliver.Size = new Size(190, 30);
            btnDeliver.TabIndex = 6;
            btnDeliver.Text = "Teslim Edildi";
            btnDeliver.UseVisualStyleBackColor = true;
            btnDeliver.Click += btnDeliver_Click;
            // 
            // btnCancelOrder
            // 
            btnCancelOrder.Location = new Point(15, 270);
            btnCancelOrder.Name = "btnCancelOrder";
            btnCancelOrder.Size = new Size(190, 30);
            btnCancelOrder.TabIndex = 7;
            btnCancelOrder.Text = "İptal Et";
            btnCancelOrder.UseVisualStyleBackColor = true;
            btnCancelOrder.Click += btnCancelOrder_Click;
            // 
            // SıparısLabel
            // 
            SıparısLabel.AutoSize = true;
            SıparısLabel.Location = new Point(6, 5);
            SıparısLabel.Name = "SıparısLabel";
            SıparısLabel.Size = new Size(97, 20);
            SıparısLabel.TabIndex = 1;
            SıparısLabel.Text = "Sipariş Listesi";
            // 
            // SıparısGrıdVıew
            // 
            SıparısGrıdVıew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SıparısGrıdVıew.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SıparısGrıdVıew.Location = new Point(6, 32);
            SıparısGrıdVıew.MultiSelect = false;
            SıparısGrıdVıew.Name = "SıparısGrıdVıew";
            SıparısGrıdVıew.ReadOnly = true;
            SıparısGrıdVıew.RowHeadersWidth = 51;
            SıparısGrıdVıew.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SıparısGrıdVıew.Size = new Size(510, 374);
            SıparısGrıdVıew.TabIndex = 0;
            SıparısGrıdVıew.SelectionChanged += SıparısGrıdVıew_SelectionChanged;
            // 
            // AdminPageControl
            // 
            AdminPageControl.AccessibleName = "";
            AdminPageControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AdminPageControl.Controls.Add(Ürünler);
            AdminPageControl.Controls.Add(Siparişler);
            AdminPageControl.Controls.Add(Müşteriler);
            AdminPageControl.Controls.Add(Destek);
            AdminPageControl.Location = new Point(2, 37);
            AdminPageControl.Name = "AdminPageControl";
            AdminPageControl.SelectedIndex = 0;
            AdminPageControl.Size = new Size(793, 444);
            AdminPageControl.TabIndex = 2;
            // 
            // Ürünler
            // 
            Ürünler.Controls.Add(dataGridView1);
            Ürünler.Controls.Add(panel2);
            Ürünler.Location = new Point(4, 29);
            Ürünler.Name = "Ürünler";
            Ürünler.Padding = new Padding(3);
            Ürünler.Size = new Size(785, 411);
            Ürünler.TabIndex = 1;
            Ürünler.Text = "Ürünler";
            Ürünler.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 49);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(744, 357);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(KategoriDuzenle);
            panel2.Controls.Add(kategoriSil);
            panel2.Controls.Add(KategoriAddButton);
            panel2.Controls.Add(SilButton);
            panel2.Controls.Add(DuzenlemeButton);
            panel2.Controls.Add(EklemeButton);
            panel2.Location = new Point(6, 6);
            panel2.Name = "panel2";
            panel2.Size = new Size(744, 37);
            panel2.TabIndex = 0;
            // 
            // KategoriDuzenle
            // 
            KategoriDuzenle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            KategoriDuzenle.Location = new Point(384, 5);
            KategoriDuzenle.Name = "KategoriDuzenle";
            KategoriDuzenle.Size = new Size(133, 29);
            KategoriDuzenle.TabIndex = 5;
            KategoriDuzenle.Text = "Kategori Düzenle";
            KategoriDuzenle.UseVisualStyleBackColor = true;
            KategoriDuzenle.Click += KategoriDuzenle_Click;
            // 
            // kategoriSil
            // 
            kategoriSil.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            kategoriSil.Location = new Point(523, 5);
            kategoriSil.Name = "kategoriSil";
            kategoriSil.Size = new Size(106, 29);
            kategoriSil.TabIndex = 4;
            kategoriSil.Text = "Kategori Sil";
            kategoriSil.UseVisualStyleBackColor = true;
            kategoriSil.Click += kategoriSil_Click;
            // 
            // KategoriAddButton
            // 
            KategoriAddButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            KategoriAddButton.Location = new Point(635, 5);
            KategoriAddButton.Name = "KategoriAddButton";
            KategoriAddButton.Size = new Size(106, 29);
            KategoriAddButton.TabIndex = 3;
            KategoriAddButton.Text = "Kategori Ekle";
            KategoriAddButton.UseVisualStyleBackColor = true;
            KategoriAddButton.Click += KategoriAddButton_Click;
            // 
            // SilButton
            // 
            SilButton.Location = new Point(215, 5);
            SilButton.Name = "SilButton";
            SilButton.Size = new Size(94, 29);
            SilButton.TabIndex = 2;
            SilButton.Text = "Ürün Sil";
            SilButton.UseVisualStyleBackColor = true;
            SilButton.Click += SilButton_Click;
            // 
            // DuzenlemeButton
            // 
            DuzenlemeButton.Location = new Point(103, 5);
            DuzenlemeButton.Name = "DuzenlemeButton";
            DuzenlemeButton.Size = new Size(106, 29);
            DuzenlemeButton.TabIndex = 1;
            DuzenlemeButton.Text = "Ürün Düzenle";
            DuzenlemeButton.UseVisualStyleBackColor = true;
            DuzenlemeButton.Click += DuzenlemeButton_Click;
            // 
            // EklemeButton
            // 
            EklemeButton.Location = new Point(3, 5);
            EklemeButton.Name = "EklemeButton";
            EklemeButton.Size = new Size(94, 29);
            EklemeButton.TabIndex = 0;
            EklemeButton.Text = " Ürün Ekle";
            EklemeButton.UseVisualStyleBackColor = true;
            EklemeButton.Click += button2_Click;
            // 
            // Destek
            // 
            Destek.Controls.Add(panelDestekDetay);
            Destek.Controls.Add(TalepLbl);
            Destek.Controls.Add(dataGridView4);
            Destek.Location = new Point(4, 29);
            Destek.Name = "Destek";
            Destek.Padding = new Padding(3);
            Destek.Size = new Size(785, 411);
            Destek.TabIndex = 4;
            Destek.Text = "Destek";
            Destek.UseVisualStyleBackColor = true;
            // 
            // panelDestekDetay
            // 
            panelDestekDetay.Controls.Add(Destekdetaylbl);
            panelDestekDetay.Controls.Add(musterıLbl);
            panelDestekDetay.Controls.Add(epostaLbl);
            panelDestekDetay.Controls.Add(DurumLbl);
            panelDestekDetay.Controls.Add(TarihLbl);
            panelDestekDetay.Controls.Add(siparisNolbl);
            panelDestekDetay.Controls.Add(MesajlarLbl);
            panelDestekDetay.Controls.Add(MesajlarListBox);
            panelDestekDetay.Controls.Add(mesajTextBox);
            panelDestekDetay.Controls.Add(yanitbutton);
            panelDestekDetay.Controls.Add(konusmayıkapatButton);
            panelDestekDetay.Dock = DockStyle.Right;
            panelDestekDetay.Location = new Point(429, 3);
            panelDestekDetay.Name = "panelDestekDetay";
            panelDestekDetay.Size = new Size(353, 405);
            panelDestekDetay.TabIndex = 3;
            // 
            // Destekdetaylbl
            // 
            Destekdetaylbl.AutoSize = true;
            Destekdetaylbl.Location = new Point(137, 1);
            Destekdetaylbl.Name = "Destekdetaylbl";
            Destekdetaylbl.Size = new Size(101, 20);
            Destekdetaylbl.TabIndex = 4;
            Destekdetaylbl.Text = "Destek Detayı";
            Destekdetaylbl.Click += label4_Click;
            // 
            // musterıLbl
            // 
            musterıLbl.AutoSize = true;
            musterıLbl.Location = new Point(6, 36);
            musterıLbl.Name = "musterıLbl";
            musterıLbl.Size = new Size(61, 20);
            musterıLbl.TabIndex = 5;
            musterıLbl.Text = "Müşteri:";
            // 
            // epostaLbl
            // 
            epostaLbl.AutoSize = true;
            epostaLbl.Location = new Point(6, 56);
            epostaLbl.Name = "epostaLbl";
            epostaLbl.Size = new Size(63, 20);
            epostaLbl.TabIndex = 6;
            epostaLbl.Text = "E-posta:";
            // 
            // DurumLbl
            // 
            DurumLbl.AutoSize = true;
            DurumLbl.Location = new Point(6, 76);
            DurumLbl.Name = "DurumLbl";
            DurumLbl.Size = new Size(57, 20);
            DurumLbl.TabIndex = 7;
            DurumLbl.Text = "Durum:";
            // 
            // TarihLbl
            // 
            TarihLbl.AutoSize = true;
            TarihLbl.Location = new Point(6, 96);
            TarihLbl.Name = "TarihLbl";
            TarihLbl.Size = new Size(43, 20);
            TarihLbl.TabIndex = 8;
            TarihLbl.Text = "Tarih:";
            TarihLbl.Click += TarihLbl_Click;
            // 
            // siparisNolbl
            // 
            siparisNolbl.AutoSize = true;
            siparisNolbl.Location = new Point(6, 116);
            siparisNolbl.Name = "siparisNolbl";
            siparisNolbl.Size = new Size(80, 20);
            siparisNolbl.TabIndex = 9;
            siparisNolbl.Text = "Sipariş No:";
            // 
            // MesajlarLbl
            // 
            MesajlarLbl.AutoSize = true;
            MesajlarLbl.Location = new Point(6, 144);
            MesajlarLbl.Name = "MesajlarLbl";
            MesajlarLbl.Size = new Size(373, 20);
            MesajlarLbl.TabIndex = 10;
            MesajlarLbl.Text = "------------------------- Mesajlar -------------------------";
            // 
            // MesajlarListBox
            // 
            MesajlarListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MesajlarListBox.BackColor = SystemColors.Window;
            MesajlarListBox.Location = new Point(6, 175);
            MesajlarListBox.Name = "MesajlarListBox";
            MesajlarListBox.Size = new Size(341, 124);
            MesajlarListBox.TabIndex = 11;
            // 
            // mesajTextBox
            // 
            mesajTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mesajTextBox.Location = new Point(6, 319);
            mesajTextBox.Name = "mesajTextBox";
            mesajTextBox.Size = new Size(341, 27);
            mesajTextBox.TabIndex = 12;
            mesajTextBox.TextChanged += mesajTextBox_TextChanged;
            // 
            // yanitbutton
            // 
            yanitbutton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            yanitbutton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            yanitbutton.Location = new Point(38, 361);
            yanitbutton.Name = "yanitbutton";
            yanitbutton.Size = new Size(113, 29);
            yanitbutton.TabIndex = 13;
            yanitbutton.Text = "Yanıt Gönder";
            yanitbutton.UseVisualStyleBackColor = true;
            yanitbutton.Click += yanitbutton_Click;
            // 
            // konusmayıkapatButton
            // 
            konusmayıkapatButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            konusmayıkapatButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            konusmayıkapatButton.Location = new Point(189, 361);
            konusmayıkapatButton.Name = "konusmayıkapatButton";
            konusmayıkapatButton.Size = new Size(142, 29);
            konusmayıkapatButton.TabIndex = 14;
            konusmayıkapatButton.Text = "Konuşmayı Kapat";
            konusmayıkapatButton.UseVisualStyleBackColor = true;
            konusmayıkapatButton.Click += konusmayıkapatButton_Click;
            // 
            // TalepLbl
            // 
            TalepLbl.AutoSize = true;
            TalepLbl.Location = new Point(141, 1);
            TalepLbl.Name = "TalepLbl";
            TalepLbl.Size = new Size(88, 20);
            TalepLbl.TabIndex = 2;
            TalepLbl.Text = "Talep Listesi";
            // 
            // dataGridView4
            // 
            dataGridView4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView4.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView4.Location = new Point(0, 23);
            dataGridView4.MultiSelect = false;
            dataGridView4.Name = "dataGridView4";
            dataGridView4.ReadOnly = true;
            dataGridView4.RowHeadersWidth = 51;
            dataGridView4.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView4.Size = new Size(429, 385);
            dataGridView4.TabIndex = 1;
            dataGridView4.SelectionChanged += dataGridView4_SelectionChanged;
            // 
            // AdminPage
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(796, 485);
            Controls.Add(AdminPageControl);
            Controls.Add(button1);
            Controls.Add(label1);
            MinimumSize = new Size(650, 400);
            Name = "AdminPage";
            Text = "AdminPage";
            Load += AdminPage_Load;
            Resize += AdminPage_Resize;
            Müşteriler.ResumeLayout(false);
            Müşteriler.PerformLayout();
            MusterıDetaylbl.ResumeLayout(false);
            MusterıDetaylbl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            Siparişler.ResumeLayout(false);
            Siparişler.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)SıparısGrıdVıew).EndInit();
            AdminPageControl.ResumeLayout(false);
            Ürünler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            Destek.ResumeLayout(false);
            Destek.PerformLayout();
            panelDestekDetay.ResumeLayout(false);
            panelDestekDetay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private TabPage Müşteriler;
        private TabPage Siparişler;
        private TabControl AdminPageControl;
        private TabPage Ürünler;
        private TabPage Destek;
        private Panel panel2;
        private Button EklemeButton;
        private DataGridView dataGridView1;
        private Button SilButton;
        private Button DuzenlemeButton;
        private Button KategoriAddButton;
        private Button KategoriDuzenle;
        private Button kategoriSil;
        private DataGridView SıparısGrıdVıew;
        private Label SıparısLabel;
        private Panel panel1;
        private Label lblOrderNo;
        private Label lblOrderCustomer;
        private Label lblOrderAmount;
        private Label lblOrderStatus;
        private Button btnPrepare;
        private Button btnShip;
        private Button btnDeliver;
        private Button btnCancelOrder;
        private Label MusteriLabel;
        private DataGridView dataGridView2;
        private Panel MusterıDetaylbl;
        private Label Ad;
        private Label label2;
        private DataGridView dataGridView3;
        private Label label3;
        private Label telefonlabel;
        private Label epostalabel;
        private Label uyetarihilabel;
        private Label MusterıSıparıslerılabel;
        private DataGridView dataGridView4;
        private Panel panelDestekDetay;
        private Label TalepLbl;
        private Label Destekdetaylbl;
        private Label musterıLbl;
        private Label siparisNolbl;
        private Label TarihLbl;
        private Label DurumLbl;
        private Label epostaLbl;
        private Label MesajlarLbl;
        private ListBox MesajlarListBox;
        private TextBox mesajTextBox;
        private Button yanitbutton;
        private Button konusmayıkapatButton;
    }
}