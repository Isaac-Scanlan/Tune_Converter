namespace TuneConverterAppInterface
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            convert = new System.Windows.Forms.Button();
            assembledTuneDisplay = new System.Windows.Forms.PictureBox();
            tuneName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            tuneBox = new System.Windows.Forms.TextBox();
            previousImage = new System.Windows.Forms.Button();
            nextImage = new System.Windows.Forms.Button();
            pageNumberLabel = new System.Windows.Forms.Label();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            panel1 = new System.Windows.Forms.Panel();
            typeComboBox = new System.Windows.Forms.ComboBox();
            typeLabel = new System.Windows.Forms.Label();
            keyLabel = new System.Windows.Forms.Label();
            keyComboBox = new System.Windows.Forms.ComboBox();
            browse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)assembledTuneDisplay).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // convert
            // 
            convert.Location = new System.Drawing.Point(21, 645);
            convert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            convert.Name = "convert";
            convert.Size = new System.Drawing.Size(83, 42);
            convert.TabIndex = 0;
            convert.Text = "Convert";
            convert.UseVisualStyleBackColor = true;
            convert.Click += button1_Click;
            // 
            // assembledTuneDisplay
            // 
            assembledTuneDisplay.Image = (System.Drawing.Image)resources.GetObject("assembledTuneDisplay.Image");
            assembledTuneDisplay.Location = new System.Drawing.Point(3, 4);
            assembledTuneDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            assembledTuneDisplay.Name = "assembledTuneDisplay";
            assembledTuneDisplay.Size = new System.Drawing.Size(436, 617);
            assembledTuneDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            assembledTuneDisplay.TabIndex = 1;
            assembledTuneDisplay.TabStop = false;
            assembledTuneDisplay.Click += pictureBox1_Click;
            // 
            // tuneName
            // 
            tuneName.Location = new System.Drawing.Point(21, 68);
            tuneName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tuneName.Name = "tuneName";
            tuneName.Size = new System.Drawing.Size(228, 27);
            tuneName.TabIndex = 2;
            tuneName.Text = "The Squirrell's Nest";
            tuneName.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 44);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(76, 20);
            label1.TabIndex = 3;
            label1.Text = "File Name";
            label1.Click += label1_Click;
            // 
            // tuneBox
            // 
            tuneBox.AcceptsTab = true;
            tuneBox.Location = new System.Drawing.Point(21, 260);
            tuneBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tuneBox.Multiline = true;
            tuneBox.Name = "tuneBox";
            tuneBox.Size = new System.Drawing.Size(379, 377);
            tuneBox.TabIndex = 4;
            tuneBox.Text = "Brl D'BA BD'D' D'BA\r\nBE'E' E'F'G' F'E'D' E'_F'\r\nG'rl E'D'B BAB G*ABC*\r\nBAB GED EGG G_A\r\n__\r\nBAB GED EGG G_A\r\nBAB GED EAA AGA\r\nBAB GED EGG GBC\r\nD'BE' D'BA BGG G_A\r\n\t|G*ABC*";
            tuneBox.TextChanged += textBox2_TextChanged;
            // 
            // previousImage
            // 
            previousImage.Location = new System.Drawing.Point(112, 11);
            previousImage.Name = "previousImage";
            previousImage.Size = new System.Drawing.Size(35, 31);
            previousImage.TabIndex = 5;
            previousImage.Text = "<";
            previousImage.UseVisualStyleBackColor = true;
            previousImage.Click += previousImage_Click;
            // 
            // nextImage
            // 
            nextImage.Location = new System.Drawing.Point(232, 11);
            nextImage.Name = "nextImage";
            nextImage.Size = new System.Drawing.Size(35, 31);
            nextImage.TabIndex = 6;
            nextImage.Text = ">";
            nextImage.UseVisualStyleBackColor = true;
            nextImage.Click += nextImage_Click;
            // 
            // pageNumberLabel
            // 
            pageNumberLabel.AutoSize = true;
            pageNumberLabel.Location = new System.Drawing.Point(153, 16);
            pageNumberLabel.Name = "pageNumberLabel";
            pageNumberLabel.Size = new System.Drawing.Size(73, 20);
            pageNumberLabel.TabIndex = 7;
            pageNumberLabel.Text = "0 out of 0";
            pageNumberLabel.Click += label2_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(assembledTuneDisplay, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(579, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.19011F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.809892F));
            tableLayoutPanel1.Size = new System.Drawing.Size(442, 693);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            panel1.Controls.Add(pageNumberLabel);
            panel1.Controls.Add(previousImage);
            panel1.Controls.Add(nextImage);
            panel1.Location = new System.Drawing.Point(3, 637);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(311, 50);
            panel1.TabIndex = 9;
            // 
            // typeComboBox
            // 
            typeComboBox.FormattingEnabled = true;
            typeComboBox.Location = new System.Drawing.Point(21, 134);
            typeComboBox.Name = "typeComboBox";
            typeComboBox.Size = new System.Drawing.Size(151, 28);
            typeComboBox.TabIndex = 9;
            typeComboBox.SelectedIndexChanged += typeComboBox_SelectedIndexChanged;
            // 
            // typeLabel
            // 
            typeLabel.AutoSize = true;
            typeLabel.Location = new System.Drawing.Point(21, 111);
            typeLabel.Name = "typeLabel";
            typeLabel.Size = new System.Drawing.Size(40, 20);
            typeLabel.TabIndex = 10;
            typeLabel.Text = "Type";
            typeLabel.Click += label2_Click_1;
            // 
            // keyLabel
            // 
            keyLabel.AutoSize = true;
            keyLabel.Location = new System.Drawing.Point(21, 181);
            keyLabel.Name = "keyLabel";
            keyLabel.Size = new System.Drawing.Size(33, 20);
            keyLabel.TabIndex = 12;
            keyLabel.Text = "Key";
            // 
            // keyComboBox
            // 
            keyComboBox.FormattingEnabled = true;
            keyComboBox.Location = new System.Drawing.Point(21, 204);
            keyComboBox.Name = "keyComboBox";
            keyComboBox.Size = new System.Drawing.Size(151, 28);
            keyComboBox.TabIndex = 11;
            // 
            // browse
            // 
            browse.Location = new System.Drawing.Point(317, 649);
            browse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            browse.Name = "browse";
            browse.Size = new System.Drawing.Size(83, 42);
            browse.TabIndex = 13;
            browse.Text = "Browse";
            browse.UseVisualStyleBackColor = true;
            browse.Click += browse_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1031, 703);
            Controls.Add(browse);
            Controls.Add(keyLabel);
            Controls.Add(keyComboBox);
            Controls.Add(typeLabel);
            Controls.Add(typeComboBox);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(tuneBox);
            Controls.Add(label1);
            Controls.Add(tuneName);
            Controls.Add(convert);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)assembledTuneDisplay).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button convert;
        private System.Windows.Forms.PictureBox assembledTuneDisplay;
        private System.Windows.Forms.TextBox tuneName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tuneBox;
        private System.Windows.Forms.Button previousImage;
        private System.Windows.Forms.Button nextImage;
        private System.Windows.Forms.Label pageNumberLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.ComboBox keyComboBox;
        private System.Windows.Forms.Button browse;
    }
}

