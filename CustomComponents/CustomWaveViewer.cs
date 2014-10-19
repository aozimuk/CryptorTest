using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace CustomComponents
{
    /// <summary>
    /// Доработанный контрол NAudio.Gui.WaveViewer
    /// <paramref name="http://www.youtube.com/watch?v=BP2MhB2KQe0"/>
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private WaveStream firstWaveStream;
        private WaveStream secondWaveStream;

        private HScrollBar hScrollBar1;

        private int samplesPerPixel;
        private long startPosition;
        private int bytesPerSample;
        private int samplesCount;

        #region Events

        public delegate void CollectionChangeEventHandler(object sender, ChangeEventArgs e);

        public event CollectionChangeEventHandler StartPositionChanged;
        public event CollectionChangeEventHandler SamplesPerPixelChanged;


        protected virtual void OnStartPositionChanged(ChangeEventArgs e)
        {
            if (StartPositionChanged != null)
                StartPositionChanged(this, e);
        }

        protected virtual void OnSamplesPerPixelChanged(ChangeEventArgs e)
        {
            if (SamplesPerPixelChanged != null)
                SamplesPerPixelChanged(this, e);
        }

        #endregion

        #region Properties

        public Color WavePenColor { get; set; }
        public float WavePenWidth { get; set; }

        public Color AxisPenColor { get; set; }
        public float AxisPenWidth { get; set; }
        
        public override System.Drawing.Font Font { get; set; }

        public int VerticalLinesCount { get; set; }

        private int DisplayHeight { get; set; }
        private int DisplayWidth { get; set; }

        //public WaveStream WaveStream
        //{
        //    get
        //    {
        //        return firstWaveStream;
        //    }
        //    set
        //    {
        //        SetWaveStreamAndDraw(value);
        //    }
        //}

        public int SamplesCount { get { return samplesCount; } }

        /// <summary>
        /// Уровень масштабирования, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                SetSamplesPerPixelAndDraw(value);
            }
        }

        /// <summary>
        /// Стартовая позиция (текущая в байтах)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                SetStartPositionAndDraw(value);
            }
        }

        #endregion


        public CustomWaveViewer()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.WavePenColor = Color.DodgerBlue;
            this.WavePenWidth = 1;

            this.AxisPenColor = Color.LimeGreen;
            this.AxisPenWidth = 1;

            this.Font = new System.Drawing.Font("Consolas", 8);

            samplesPerPixel = -1;
            startPosition = -1;
            bytesPerSample = -1;
            samplesCount = -1;

            hScrollBar1.Minimum = 0;
            hScrollBar1.Value = 0;
            hScrollBar1.Maximum = 1;
            hScrollBar1.Visible = false;

            UpdateDisplaySize();

            VerticalLinesCount = 10;
        }

        public void AddFisrtWave(WaveStream waveStream)
        {
            SetWaveStreamAndDraw(waveStream);
        }

        public void AddSecondWave(WaveStream waveStream)
        {
            if (firstWaveStream != null && waveStream != null)
            {
                int secondBytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                int secondSamplesCount = (int)(waveStream.Length / bytesPerSample);

                if (secondBytesPerSample == bytesPerSample && secondSamplesCount == samplesCount)
                {
                    secondWaveStream = waveStream;
                    this.Invalidate();
                }
            }            
        }


        #region Ovverride methods OnKeyDown, OnMouseWheel, OnPaint, OnResize, Dispose

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D0)
            {
                FitToScreen();
            }
            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                // Масштабирование
                SamplesPerPixel -= (e.Delta / 12) * 10 * bytesPerSample;
            }
            else
            {
                // Прокрутка (вправо/влево)
                StartPosition += (e.Delta / 12) * SamplesPerPixel * bytesPerSample;
            }
            hScrollBar1.Value = Math.Min(hScrollBar1.Maximum, (int)StartPosition / bytesPerSample);
            base.OnMouseWheel(e);
        }
             
        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {       
            if (firstWaveStream != null)
            {
                if (e.ClipRectangle.Left != 0) MessageBox.Show("как это возможно: " + e.ClipRectangle.Left);

                long leftSample = startPosition / bytesPerSample; //startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel); 
                
                DrawWave(e.Graphics, e.ClipRectangle.X, e.ClipRectangle.Right, firstWaveStream);

                if (secondWaveStream != null)
                {
                    DrawWave(e.Graphics, Color.FromArgb(50,Color.DodgerBlue), WavePenWidth, e.ClipRectangle.X, e.ClipRectangle.Right, secondWaveStream);
                }

                DrawAxisGrid(e.Graphics, VerticalLinesCount, leftSample.ToString(), (leftSample + DisplayWidth * samplesPerPixel).ToString());
            }
            
            base.OnPaint(e);
        }

        private void DrawWave(Graphics g, int displayLeft, int displayRight, WaveStream ws)
        {
            DrawWave(g, WavePenColor, WavePenWidth, displayLeft, displayRight, ws);
        }

        private void DrawWave(Graphics g, Color penColor, float penWidth, int displayLeft, int displayRight, WaveStream ws)
        {
            if (ws != null)
            {
                int bytesRead = 0;
                int bytesPerPixel = samplesPerPixel * bytesPerSample;
                byte[] readWaveData = new byte[bytesPerPixel];

                ws.Position = startPosition;

                using (Pen wavePen = new Pen(penColor, penWidth))
                {
                    for (int x = displayLeft; x < displayRight; x++)
                    {
                        short low = 0;
                        short high = 0;
                        bytesRead = ws.Read(readWaveData, 0, bytesPerPixel);

                        if (bytesRead > 0)
                        {
                            // выбираем самое маленькое и самое большое значение сэмплов среди считанных данных
                            for (int i = 0; i < bytesRead; i += 2)
                            {
                                short sample = BitConverter.ToInt16(readWaveData, i);
                                if (sample < low) low = sample;
                                if (sample > high) high = sample;
                            }

                            float lowPercent = ((((float)low) - short.MinValue) / ushort.MaxValue);
                            float highPercent = ((((float)high) - short.MinValue) / ushort.MaxValue);

                            g.DrawLine(wavePen, x, DisplayHeight * lowPercent, x, DisplayHeight * highPercent);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }       

        private void DrawAxisGrid(Graphics g, int verticalLinesCount, string startLabel, string endLabel)
        {
            using (Pen axisPen = new Pen(AxisPenColor, AxisPenWidth))
            {
                // ось
                g.DrawLine(axisPen, new Point(0, DisplayHeight / 2), new Point(DisplayWidth, DisplayHeight / 2));
                
                // cетка
                int step = DisplayWidth / (verticalLinesCount);
                for (int i = 1; i < verticalLinesCount; i++)
                {
                    g.DrawLine(axisPen, new Point(i * step, 0), new Point(i * step, DisplayHeight));
                }

                // начало координат
                g.DrawString(
                      startLabel
                    , Font
                    , axisPen.Brush
                    , new PointF(0, DisplayHeight / 2));

                // конец координат
                g.DrawString(
                    endLabel
                    , Font
                    , axisPen.Brush
                    , new PointF(DisplayWidth - g.MeasureString(endLabel.ToString(), Font).Width, DisplayHeight / 2));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateDisplaySize();
            FitToScreen();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            StartPosition = (long)e.NewValue * bytesPerSample;
        }

        #endregion


        #region Set And Rise Redraw Functions

        /// <summary>
        /// Установка нового потока волны, вычисление параметров волны, перересовка комопоненты
        /// </summary>
        private void SetWaveStreamAndDraw(WaveStream value)
        {
            firstWaveStream = value;
            if (firstWaveStream != null)
            {
                UpdateDisplaySize();

                bytesPerSample = (firstWaveStream.WaveFormat.BitsPerSample / 8) * firstWaveStream.WaveFormat.Channels;
                samplesCount = (int)(firstWaveStream.Length / bytesPerSample);
                //FitToScreen();
                FitToWaveLength();
                hScrollBar1.Visible = true;
            }            
            this.Invalidate();
        }

        /// <summary>
        /// Установка уровня масштабирования, перересовка компоненты
        /// </summary>
        private void SetSamplesPerPixelAndDraw(int value)
        {
            int maxSamplesPerPixel = SamplesCount / DisplayWidth;
            
            if (firstWaveStream != null)
            {
                int oldValue = samplesPerPixel;

                if (value < maxSamplesPerPixel)
                {
                    samplesPerPixel = Math.Max(1, value);
                    hScrollBar1.Maximum = (int)(SamplesCount - DisplayWidth * samplesPerPixel);
                }
                else if (value == maxSamplesPerPixel)
                {
                    samplesPerPixel = value;
                    hScrollBar1.Maximum = 1;
                }

                OnSamplesPerPixelChanged(new ChangeEventArgs(oldValue, samplesPerPixel));
                
                this.Invalidate();
            }
        }

        /// <summary>
        /// Установка стартовой позиции байт, перересовка компоненты
        /// </summary>
        private void SetStartPositionAndDraw(long value)
        {
            long bytesToEnd = (long)DisplayWidth * samplesPerPixel * bytesPerSample;
            if (firstWaveStream != null)
            {
                long oldValue = startPosition;

                if (bytesToEnd + value < firstWaveStream.Length)
                {
                    startPosition = Math.Max(0, value);
                }
                else if (bytesToEnd + value == firstWaveStream.Length)
                {
                    startPosition = value;
                }

                OnStartPositionChanged(new ChangeEventArgs(oldValue / bytesPerSample, startPosition / bytesPerSample));

                this.Invalidate();
                
            }
        }

        /// <summary>
        /// Растянуть волну по ширине компоненты
        /// </summary>
        public void FitToScreen()
        {
            StartPosition = 0;
            SetSamplesPerPixelAndDraw(SamplesCount / DisplayWidth);
        }

        /// <summary>
        /// Растянуть по длине волны
        /// </summary>
        public void FitToWaveLength()
        {
            StartPosition = 0;
            SetSamplesPerPixelAndDraw(1);
        }

        private void UpdateDisplaySize()
        {
            DisplayHeight = Height - hScrollBar1.Height - 4;
            DisplayWidth = ClientRectangle.Width;
        }

        #endregion


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.LargeChange = 2;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 133);
            this.hScrollBar1.Maximum = 1;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(150, 17);
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // CustomWaveViewer
            // 
            this.Controls.Add(this.hScrollBar1);
            this.Name = "CustomWaveViewer";
            this.ResumeLayout(false);

        }
        #endregion

        #region Zoom region commented
        /*
         
        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, DisplayHeight)), Color.Black);
        }
         
        public void Zoom(int leftSample, int rightSample)
        {
            startPosition = leftSample * bytesPerSample;
            SamplesPerPixel = (rightSample - leftSample) / this.Width;
        }

        private Point mousePos, startPos;
        private bool mouseDrag = false;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                startPos = e.Location;
                mousePos = new Point(-1, -1);
                mouseDrag = true;
                DrawVerticalLine(e.X);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDrag)
            {
                DrawVerticalLine(e.X);
                if (mousePos.X != -1) DrawVerticalLine(mousePos.X);
                mousePos = e.Location;
                DrawShadowedArea(startPos.X, e.X);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mouseDrag && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDrag = false;
                DrawVerticalLine(startPos.X);

                if (mousePos.X == -1) return;
                DrawVerticalLine(mousePos.X);

                int leftSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Min(startPos.X, mousePos.X));
                int rightSample = (int)(StartPosition / bytesPerSample + samplesPerPixel * Math.Max(startPos.X, mousePos.X));
                Zoom(leftSample, rightSample);
            }
            base.OnMouseUp(e);
        }
        */
        #endregion


    }
}
