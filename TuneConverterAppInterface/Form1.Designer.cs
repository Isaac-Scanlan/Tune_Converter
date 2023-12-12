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
            ((System.ComponentModel.ISupportInitialize)assembledTuneDisplay).BeginInit();
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
            assembledTuneDisplay.Location = new System.Drawing.Point(629, 13);
            assembledTuneDisplay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            assembledTuneDisplay.Name = "assembledTuneDisplay";
            assembledTuneDisplay.Size = new System.Drawing.Size(485, 609);
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
            tuneBox.Location = new System.Drawing.Point(21, 129);
            tuneBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tuneBox.Multiline = true;
            tuneBox.Name = "tuneBox";
            tuneBox.Size = new System.Drawing.Size(379, 493);
            tuneBox.TabIndex = 4;
            tuneBox.Text = resources.GetString("tuneBox.Text");
            tuneBox.TextChanged += textBox2_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1144, 703);
            Controls.Add(tuneBox);
            Controls.Add(label1);
            Controls.Add(tuneName);
            Controls.Add(assembledTuneDisplay);
            Controls.Add(convert);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)assembledTuneDisplay).EndInit();
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
    }
}

