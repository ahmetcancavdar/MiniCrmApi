namespace WinFormUI
{
    partial class KayıtOlmaForm
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            textBox1 = new TextBox();
            label7 = new Label();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(350, 19);
            label1.Name = "label1";
            label1.Size = new Size(80, 25);
            label1.TabIndex = 0;
            label1.Text = "Kayıt Ol";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(255, 112);
            label2.Name = "label2";
            label2.Size = new Size(76, 20);
            label2.TabIndex = 1;
            label2.Text = "Ad Soyad:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(255, 143);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 2;
            label3.Text = "Email:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(255, 178);
            label4.Name = "label4";
            label4.Size = new Size(42, 20);
            label4.TabIndex = 3;
            label4.Text = "Şifre:";
            label4.Click += label4_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(255, 213);
            label5.Name = "label5";
            label5.Size = new Size(85, 20);
            label5.TabIndex = 4;
            label5.Text = "Telefon No:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(255, 250);
            label6.Name = "label6";
            label6.Size = new Size(49, 20);
            label6.TabIndex = 5;
            label6.Text = "Şirket:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(349, 109);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(162, 27);
            textBox1.TabIndex = 6;
            textBox1.Text = "Ad Soyad giriniz...";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(364, 153);
            label7.Name = "label7";
            label7.Size = new Size(0, 20);
            label7.TabIndex = 7;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(349, 140);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(162, 27);
            textBox2.TabIndex = 8;
            textBox2.Text = "Email giriniz...";
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(349, 171);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(162, 27);
            textBox3.TabIndex = 9;
            textBox3.Text = "Şifre giriniz...";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(349, 210);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(162, 27);
            textBox4.TabIndex = 10;
            textBox4.Text = "Telefon No giriniz...";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(349, 246);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(162, 27);
            textBox5.TabIndex = 11;
            textBox5.Text = "Şirket adı giriniz...";
            // 
            // button1
            // 
            button1.Location = new Point(347, 311);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 12;
            button1.Text = "Kayıt Ol";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            //
            // KayıtOlmaForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(textBox5);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(label7);
            Controls.Add(textBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "KayıtOlmaForm";
            Text = "KayıtOlmaForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox textBox1;
        private Label label7;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Button button1;
    }
}