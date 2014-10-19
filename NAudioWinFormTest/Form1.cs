using CustomComponents;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NAudioWinFormTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (System.IO.FileStream fs = new System.IO.FileStream(wavFileName, System.IO.FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];

                fs.Read(buffer, 0, buffer.Length);

                int AudioFormat = ((int)buffer[20 + 1] << 8) + (int)buffer[20];
                int NumChanels = ((int)buffer[22 + 1] << 8) + (int)buffer[22];
                int SampleRate = ((int)buffer[24+3] << 24) + ((int)buffer[24+2] << 16) + ((int)buffer[24+1] << 8) + (int)buffer[24];
                int ByteRate = ((int)buffer[28+3] << 24) + ((int)buffer[28+2] << 16) + ((int)buffer[28+1] << 8) + (int)buffer[28];
                int BlockAlign = ((int)buffer[32+1] << 8) + (int)buffer[32];
                int BitsPerSample = ((int)buffer[34+1] << 8) + (int)buffer[34];
                // 2 byte for extra param. why? nobody knows.
                int extraParamSize = 2;
                string Subchank2ID = Encoding.Default.GetString(buffer, 36 + extraParamSize, 4);
                int AudioDataSize = ((int)buffer[40 + 3 + extraParamSize] << 24) + ((int)buffer[40 + 2 + extraParamSize] << 16) + ((int)buffer[40 + 1 + extraParamSize] << 8) + (int)buffer[40 + extraParamSize];

                MessageBox.Show(string.Format(
                       "AudioFormat: {0}"+Environment.NewLine + 
                       "NumChanels: {1}"+Environment.NewLine + 
                       "SampleRate: {2}"+Environment.NewLine +
                       "ByteRate: {3}" + Environment.NewLine +
                       "BlockAlign: {4}" + Environment.NewLine +
                       "BitsPerSample: {5}" + Environment.NewLine +
                       "Subchank2ID: {6}" + Environment.NewLine +
                       "AudioDataSize: {7}" + Environment.NewLine,
                       AudioFormat, NumChanels, SampleRate, 
                       ByteRate, BlockAlign, BitsPerSample,
                       Subchank2ID, AudioDataSize
                        ));
            }

            using (WaveFileReader wavStream = new WaveFileReader(wavFileName))
            {
                byte[] buffer = new byte[wavStream.Length];

                wavStream.Read(buffer, 0, buffer.Length);

                MessageBox.Show(string.Format(
                       "AudioFormat: {0}" + Environment.NewLine +
                       "NumChanels: {1}" + Environment.NewLine +
                       "SampleRate: {2}" + Environment.NewLine +
                       "ByteRate: {3}" + Environment.NewLine +
                       "BlockAlign: {4}" + Environment.NewLine +
                       "BitsPerSample: {5}" + Environment.NewLine +
                       "Subchank2ID: {6}" + Environment.NewLine +
                       "AudioDataSize: {7}" + Environment.NewLine,
                       (int)wavStream.WaveFormat.Encoding, wavStream.WaveFormat.Channels, wavStream.WaveFormat.SampleRate,
                       wavStream.WaveFormat.AverageBytesPerSecond, wavStream.BlockAlign, wavStream.WaveFormat.BitsPerSample,
                       "unknw", wavStream.Length
                        ));
            }
        }

        string wavFileName = @"..\..\..\audio\[NFS Most Wanted].wav";
        string wavFileNameCopy = @"..\..\..\audio\[NFS Most Wanted] - copy.wav";

        string wavFileNameModified = @"..\..\..\audio\[NFS Most Wanted] - mod.wav";

        string message = "Yo ho ho!!!";
        private void btn_testWaveViewer_Click(object sender, EventArgs e)
        {
            Steganography.HideMessage(Encoding.Default.GetBytes(longstring), wavFileName, wavFileNameModified);
            

            customWaveViewer1.AddFisrtWave(new WaveFileReader(wavFileName));
            customWaveViewer1.AddSecondWave(new WaveFileReader(wavFileNameModified));
        }





        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            customWaveViewer1.Invalidate();
        }

        private void customWaveViewer1_SamplesPerPixelChanged(object sender, ChangeEventArgs e)
        {
            samplesPerPixLabel.Text = string.Format("SamplesPerPixel: {0}", e.NewValue);
        }

        private void customWaveViewer1_StartPositionChanged(object sender, ChangeEventArgs e)
        {
            startPositionLabel.Text = string.Format("StartPosition: {0}", e.NewValue);
        }



        string longstring = @"Программное средство должно иметь понятный и удобный графический интерфейс. В меню программы входит выбор режима стеганографии в аудиозапись mp3, прорисовка графика изменения структуры аудиофайла (осциллограмма) и mp3 плеер для возможности прослушать аудиофайл до и после внедрения.
Необходимо реализовать три режима внедрения данных в аудиофайлы: вставка в заголовки, вставка в тело данных, вставка в конец (чтобы антивирусные программы не блокировали содержимое).
Пользователь должен иметь возможность выбора mp3 файла со своего компьютера, также должен выбирать из файлов своего компьютера текстовый файл с сообщением, либо вводить его вручную.
Реализовать ограничение на расширение выбранных файлов.
Реализовать в первом режиме внедрения данных чтение информации из заголовка mp3 файлов, возможность изменения существующих данных в тегах, возможность просмотра информации путем вывода в отдельное окно программы.
Во втором режиме внедрения необходимо реализовать два способа записи информации в тело данных: в первом способе скрываемое сообщение шифруется, сжимается и внедряется в mp3 файл, второй способ реализует преобразование mp3 файла в формат wav, внедряет шифрованные данные и делает обратное преобразование в mp3.
Третий режим реализует открытие mp3 файла для правки и внедряет зашифрованные данные в конец файла.
Программное средство должно корректно выполнять функцию встраивания исходного сообщения в контейнер. В качестве контейнера разрешается использовать только аудио файлы в формате mp3. При некорректной эксплуатации должно быть показано сообщение об ошибке.
Шифрование алгоритмом DES или Blowfish или предложите свой.
Требования к надежности
Надежное функционирование программного средства должно быть обеспечено выполнением пользователем совокупности организационно-технических мероприятий, перечень которых приведен ниже:
";

        private void bn_Unhide_Click(object sender, EventArgs e)
        {
            string unhideString = Encoding.Default.GetString(Steganography.UnhideMessage(wavFileNameModified));

            textBox1.Lines = new string[] { unhideString };

        }
    }
}
