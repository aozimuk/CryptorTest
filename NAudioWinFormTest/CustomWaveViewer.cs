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
    /// Control for viewing waveforms
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        public Color WavePenColor { get; set; }
        public float WavePenWidth { get; set; }
        public Color AxisPenColor { get; set; }
        public float AxisPenWidth { get; set; }
        public override System.Drawing.Font Font { get; set; }
                        
        private System.ComponentModel.Container components = null;
        private WaveStream waveStream;
        private int samplesPerPixel;
        private long startPosition;
        private int bytesPerSample;
                       
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.DoubleBuffered = true;

            this.WavePenColor = Color.DodgerBlue;
            this.WavePenWidth = 1;

            this.AxisPenColor = Color.LimeGreen;
            this.AxisPenWidth = 1;

            this.Font = new System.Drawing.Font("Consolas", 8);
        }
        
        /// <summary>
        /// Растянуть волну по ширине компонент
        /// </summary>
        public void FitToScreen()
        {
            if (waveStream != null)
            {
                int samples = (int)(waveStream.Length / bytesPerSample);
                startPosition = 0;
                SamplesPerPixel = samples / this.Width;
            }
        }

        #region Zoom region
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                // Масштабирование
                SamplesPerPixel -= e.Delta * 10;
            }
            else
            {
                // Прокрутка (вперед/назад)
                StartPosition += e.Delta * 10;
            }
            base.OnMouseWheel(e);
        }
        

        private void DrawVerticalLine(int x)
        {
            ControlPaint.DrawReversibleLine(PointToScreen(new Point(x, 0)), PointToScreen(new Point(x, Height)),Color.Black);
        }

        public WaveStream WaveStream
        {
            get
            {
                return waveStream;
            }
            set
            {
                waveStream = value;
                if (waveStream != null)
                {
                    bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8) * waveStream.WaveFormat.Channels;
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in samples per pixel
        /// </summary>
        public int SamplesPerPixel
        {
            get
            {
                return samplesPerPixel;
            }
            set
            {
                samplesPerPixel = Math.Max(1,value);
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start position (currently in bytes)
        /// </summary>
        public long StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
             // нужна проверка чтоб не сдвигалось влево
                startPosition = Math.Max(0, value);
                this.Invalidate();
            }
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
                byte[] waveData = new byte[samplesPerPixel * bytesPerSample];
                waveStream.Position = startPosition + (e.ClipRectangle.Left * bytesPerSample * samplesPerPixel);

                long leftSample = waveStream.Position / bytesPerSample;

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
                        e.Graphics.DrawLine(wavePen, x, this.Height * lowPercent, x, this.Height * highPercent);
                    }
                }

                using (Pen axisPen = new Pen(AxisPenColor, AxisPenWidth))
                {
                    e.Graphics.DrawLine(axisPen, new Point(0, Height / 2), new Point(Width, Height / 2));
                    int vertLineCount = 10;
                    int step = Width / (vertLineCount);
                    for (int i = 1; i < vertLineCount; i++)
                    {
                        e.Graphics.DrawLine(axisPen, new Point(i * step, 0), new Point(i*step, Height));
                    }

                    

                    e.Graphics.DrawString(
                          leftSample.ToString()
                        , Font
                        , axisPen.Brush
                        , new PointF(0, Height / 2));
                    e.Graphics.DrawString(
                        (leftSample + Width * samplesPerPixel).ToString()
                        , Font
                        , axisPen.Brush
                        , new PointF(
                              Width - e.Graphics.MeasureString((leftSample + Width * samplesPerPixel).ToString()
                            , Font).Width
                            , Height / 2));
                }
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            FitToScreen();
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
}
