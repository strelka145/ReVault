namespace UploadToNAS
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            original_data_path = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            label2 = new Label();
            save_path = new TextBox();
            button3 = new Button();
            label3 = new Label();
            lastrunTXT_path = new TextBox();
            button4 = new Button();
            SuspendLayout();
            // 
            // original_data_path
            // 
            original_data_path.Location = new Point(131, 30);
            original_data_path.Name = "original_data_path";
            original_data_path.Size = new Size(243, 23);
            original_data_path.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(50, 33);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 1;
            label1.Text = "元データ";
            // 
            // button1
            // 
            button1.Location = new Point(380, 33);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "選択";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(380, 90);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 5;
            button2.Text = "選択";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(50, 90);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 4;
            label2.Text = "保存先";
            label2.Click += label2_Click;
            // 
            // save_path
            // 
            save_path.Location = new Point(131, 87);
            save_path.Name = "save_path";
            save_path.Size = new Size(243, 23);
            save_path.TabIndex = 3;
            // 
            // button3
            // 
            button3.Location = new Point(380, 144);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 8;
            button3.Text = "選択";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 148);
            label3.Name = "label3";
            label3.Size = new Size(108, 15);
            label3.TabIndex = 7;
            label3.Text = "lastrun.txt保存場所";
            // 
            // lastrunTXT_path
            // 
            lastrunTXT_path.Location = new Point(131, 141);
            lastrunTXT_path.Name = "lastrunTXT_path";
            lastrunTXT_path.Size = new Size(243, 23);
            lastrunTXT_path.TabIndex = 6;
            // 
            // button4
            // 
            button4.Location = new Point(160, 209);
            button4.Name = "button4";
            button4.Size = new Size(156, 54);
            button4.TabIndex = 9;
            button4.Text = "ショートカット作成";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(485, 293);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label3);
            Controls.Add(lastrunTXT_path);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(save_path);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(original_data_path);
            Name = "Form1";
            Text = "初期設定";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox original_data_path;
        private Label label1;
        private Button button1;
        private Button button2;
        private Label label2;
        private TextBox save_path;
        private Button button3;
        private Label label3;
        private TextBox lastrunTXT_path;
        private Button button4;
    }
}
