using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;

namespace NAudioWinFormTest
{
    /// <summary>
    /// Доработанный контрол NAudio.Gui.WaveViewer
    /// <paramref name="http://www.youtube.com/watch?v=BP2MhB2KQe0"/>
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        private System.Collections.Generic.List<IDisposable> objToDispose;
        private WaveStream waveStream;
        private HScrollBar hScrollBar1;

        private int samplesPerPixel;
        private long startPosition;
        private int bytesPerSample;
        private int samplesCount;

        private int DisplayHeight { get; set; }
        private int DisplayWidth { get; set; }

        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            objToDispose = new System.Collections.Generic.List<IDisposable>();
            objToDispose.Add(hScrollBar1);

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
            hScrollBar1.Maximum = 1;
            hScrollBar1.Visible = false;

            DisplayHeight = Height - hScrollBar1.Height - 4;
            DisplayWidth = ClientRectangle.Width;
        }


        #region Properties

        public Color WavePenColor { get; set; }
        public float WavePenWidth { get; set; }

        public Color AxisPenColor { get; set; }
        public float AxisPenWidth { get; set; }
        
        public override System.Drawing.Font Font { get; set; }

        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                SetWaveStreamAndDraw(value);
            }
        }

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
        
        
        
        #region Ovverride methods

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
                SamplesPerPixel -= e.Delta * 10;
            }
            else
            {
                // Прокрутка (вправо/влево)
                StartPosition += e.Delta * 10;
            }
            hScrollBar1.Value = Math.Min(hScrollBar1.Maximum,(int)StartPosition / bytesPerSample);
            base.OnMouseWheel(e);
        }
             
        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {       

            if (waveStream != null)
            {
                waveStream.Position = 0;
                int bytesRead;
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample]; // [bytes per pixel]

                //e.ClipRectangle.Left равен 0, зачем это слогаемое ваще нужно?? хз
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                long leftSample = waveStream.Position / bytesPerSample;
                
                // отрисовка волны
                using (Pen wavePen = new Pen(WavePenColor, WavePenWidth))
                {
                    for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                    {
                        short low = 0;
                        short high = 0;
                        bytesRead = waveStream.Read(waveData, 0, samplesPerPixel * bytesPerSample);
                        if (bytesRead == 0)
                            break;
                        for (int n = 0; n < bytesRead; n += 2)
                        {
                            short sample = BitConverter.ToInt16(waveData, n);
                            if (sample < low) low = sample;
                            if (sample > high) high = sample;
                        }
                        float lowPercent = ((((float)low) - short.MinValue) / ushort.MaxValue);
                        float highPercent = ((((float)high) - short.MinValue) / ushort.MaxValue);
                        e.Graphics.DrawLine(wavePen, x, DisplayHeight * lowPercent, x, DisplayHeight * highPercent);
                    }
                }

                // отрисовка осей координат и сетки
                using (Pen axisPen = new Pen(AxisPenColor, AxisPenWidth))
                {
                    e.Graphics.DrawLine(axisPen, new Point(0, DisplayHeight / 2), new Point(DisplayWidth, DisplayHeight / 2));
                    int vertLineCount = 11;
                    int step = DisplayWidth / (vertLineCount);
                    for (int i = 1; i < vertLineCount; i++)
                    {
                        e.Graphics.DrawLine(axisPen, new Point(i * step, 0), new Point(i * step, DisplayHeight));
                    }

                    
                    // начало координат
                    e.Graphics.DrawString(
                          leftSample.ToString()
                        , Font
                        , axisPen.Brush
                        , new PointF(0, DisplayHeight / 2));
                    // конец координат
                    e.Graphics.DrawString(
                        (leftSample + DisplayWidth * samplesPerPixel).ToString()
                        , Font
                        , axisPen.Brush
                        , new PointF(
                              DisplayWidth - e.Graphics.MeasureString((leftSample + DisplayWidth * samplesPerPixel).ToString()
                            , Font).Width
                            , DisplayHeight / 2));
                }
            }

            base.OnPaint(e);
        }

        private void DrawAxisGrid(Graphics g, int verticalLineCount, string startLabel, string endLabel)
        {
            // отрисовка осей координат и сетки
            using (Pen axisPen = new Pen(AxisPenColor, AxisPenWidth))
            {
                g.DrawLine(axisPen, new Point(0, DisplayHeight / 2), new Point(DisplayWidth, DisplayHeight / 2));
                int step = DisplayWidth / (verticalLineCount);
                for (int i = 1; i < verticalLineCount; i++)
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
                    , new PointF(
                          DisplayWidth - g.MeasureString(endLabel.ToString()
                        , Font).Width
                        , DisplayHeight / 2));
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GetDisplaySize();
            FitToScreen();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var obj in objToDispose)
                {
                    obj.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            StartPosition = (long)e.NewValue * bytesPerSample;
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



        #region Set And Rise Redraw Functions

        /// <summary>
        /// Установка нового потока волны, вычисление параметров волны, перересовка комопоненты
        /// </summary>
        private void SetWaveStreamAndDraw(WaveStream value)
        {
            if (waveStream != null) {
                objToDispose.Remove(waveStream);
                waveStream.Close(); // чет как-то не работает (
                waveStream = null;
            }
            waveStream = value;
            if (waveStream != null)
            {
                GetDisplaySize();

                bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                samplesCount = (int)(waveStream.Length / bytesPerSample);
                FitToScreen();
                hScrollBar1.Visible = true;
                objToDispose.Add(waveStream);
            }
            this.Invalidate();
        }

        /// <summary>
        /// Установка уровня масштабирования, перересовка компоненты
        /// </summary>
        private void SetSamplesPerPixelAndDraw(int value)
        {
            int maxSamplesPerPixel = SamplesCount / DisplayWidth;
            if (waveStream != null)
            {
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
                this.Invalidate();
            }
        }

        /// <summary>
        /// Установка стартовой позиции байт, перересовка компоненты
        /// </summary>
        private void SetStartPositionAndDraw(long value)
        {
            long bytesToEnd = (long)DisplayWidth * samplesPerPixel * bytesPerSample;
            if (waveStream != null)
            {
                if (bytesToEnd + value < waveStream.Length)
                {
                    startPosition = Math.Max(0, value);
                }
                else if (bytesToEnd + value == waveStream.Length)
                {
                    startPosition = value;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// Растянуть волну по ширине компоненты
        /// </summary>
        public void FitToScreen()
        {
            startPosition = 0;
            SetSamplesPerPixelAndDraw(SamplesCount / DisplayWidth);
        }

        private void GetDisplaySize()
        {
            DisplayHeight = Height - hScrollBar1.Height - 4;
            DisplayWidth = ClientRectangle.Width;
        }

        #endregion



        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, DisplayHeight)), Color.Black);
        }





        #region Zoom region commented
        /*
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
