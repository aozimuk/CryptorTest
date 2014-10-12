namespace NAudioWinFormTest
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.btn_testWaveViewer = new System.Windows.Forms.Button();
            this.customWaveViewer1 = new NAudioWinFormTest.CustomWaveViewer();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(45, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_testWaveViewer
            // 
            this.btn_testWaveViewer.Location = new System.Drawing.Point(156, 85);
            this.btn_testWaveViewer.Name = "btn_testWaveViewer";
            this.btn_testWaveViewer.Size = new System.Drawing.Size(138, 23);
            this.btn_testWaveViewer.TabIndex = 2;
            this.btn_testWaveViewer.Text = "testWaveViewer";
            this.btn_testWaveViewer.UseVisualStyleBackColor = true;
            this.btn_testWaveViewer.Click += new System.EventHandler(this.btn_testWaveViewer_Click);
            // 
            // customWaveViewer1
            // 
            this.customWaveViewer1.Location = new System.Drawing.Point(12, 274);
            this.customWaveViewer1.Name = "customWaveViewer1";
            this.customWaveViewer1.SamplesPerPixel = 128;
            this.customWaveViewer1.Size = new System.Drawing.Size(689, 150);
            this.customWaveViewer1.StartPosition = ((long)(0));
            this.customWaveViewer1.TabIndex = 3;
            this.customWaveViewer1.WaveStream = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 436);
            this.Controls.Add(this.customWaveViewer1);
            this.Controls.Add(this.btn_testWaveViewer);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "NAudioWinFormTest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_testWaveViewer;
        private CustomWaveViewer customWaveViewer1;
    }
}

