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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.samplesPerPixLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.separator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.startPositionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.customWaveViewer1 = new CustomComponents.CustomWaveViewer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bn_Unhide = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.samplesPerPixLabel,
            this.separator1,
            this.startPositionLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 414);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(724, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // samplesPerPixLabel
            // 
            this.samplesPerPixLabel.Name = "samplesPerPixLabel";
            this.samplesPerPixLabel.Size = new System.Drawing.Size(110, 17);
            this.samplesPerPixLabel.Text = "samplesPerPixLabel";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(13, 17);
            this.separator1.Text = "||";
            // 
            // startPositionLabel
            // 
            this.startPositionLabel.Name = "startPositionLabel";
            this.startPositionLabel.Size = new System.Drawing.Size(101, 17);
            this.startPositionLabel.Text = "startPositionLabel";
            // 
            // customWaveViewer1
            // 
            this.customWaveViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customWaveViewer1.AxisPenColor = System.Drawing.Color.Red;
            this.customWaveViewer1.AxisPenWidth = 1F;
            this.customWaveViewer1.BackColor = System.Drawing.Color.Gainsboro;
            this.customWaveViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customWaveViewer1.Location = new System.Drawing.Point(12, 200);
            this.customWaveViewer1.Name = "customWaveViewer1";
            this.customWaveViewer1.SamplesPerPixel = -1;
            this.customWaveViewer1.Size = new System.Drawing.Size(700, 211);
            this.customWaveViewer1.StartPosition = ((long)(-1));
            this.customWaveViewer1.TabIndex = 3;
            this.customWaveViewer1.VerticalLinesCount = 10;
            this.customWaveViewer1.WavePenColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.customWaveViewer1.WavePenWidth = 1F;
            this.customWaveViewer1.StartPositionChanged += new CustomComponents.CustomWaveViewer.CollectionChangeEventHandler(this.customWaveViewer1_StartPositionChanged);
            this.customWaveViewer1.SamplesPerPixelChanged += new CustomComponents.CustomWaveViewer.CollectionChangeEventHandler(this.customWaveViewer1_SamplesPerPixelChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(300, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(412, 143);
            this.textBox1.TabIndex = 5;
            // 
            // bn_Unhide
            // 
            this.bn_Unhide.Location = new System.Drawing.Point(78, 147);
            this.bn_Unhide.Name = "bn_Unhide";
            this.bn_Unhide.Size = new System.Drawing.Size(75, 23);
            this.bn_Unhide.TabIndex = 6;
            this.bn_Unhide.Text = "unhide";
            this.bn_Unhide.UseVisualStyleBackColor = true;
            this.bn_Unhide.Click += new System.EventHandler(this.bn_Unhide_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 436);
            this.Controls.Add(this.bn_Unhide);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.customWaveViewer1);
            this.Controls.Add(this.btn_testWaveViewer);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "NAudioWinFormTest";
            this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_testWaveViewer;
        private CustomComponents.CustomWaveViewer customWaveViewer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel samplesPerPixLabel;
        private System.Windows.Forms.ToolStripStatusLabel startPositionLabel;
        private System.Windows.Forms.ToolStripStatusLabel separator1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button bn_Unhide;
    }
}

