using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MP3Sharp;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly API api = new API();

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {


            #region:Analyze MP3
            MP3Stream mp3 = new MP3Stream(fileName: "Music\\Sample.mp3");
            byte[] buffer = new byte[8];
            int bytesReturned = 1;
            int totalBytesRead = 0;
            ArrayList wav = new ArrayList();
            int Sum;

            #region:obsoleted full-sized extraction
            //byte[] wav = new byte[0];
            //byte[] wav_new;
            #endregion

            while (bytesReturned > 0)
            {
                Sum = 0x00;
                bytesReturned = mp3.Read(buffer, 0, buffer.Length);
                totalBytesRead += bytesReturned;
                foreach(byte b in buffer)
                {
                    Sum += (int)b;
                }
                wav.Add(Sum / buffer.Length);

                #region:obsoleted full-sized extraction
                //wav_new = new byte[wav.Length + buffer.Length];
                //wav.CopyTo(wav_new, 0);
                //buffer.CopyTo(wav_new, wav.Length);
                //wav = wav_new;
                #endregion
            }
            mp3.Close();
            int a = 2000;
            chart1.Series[0].Points.DataBindY(wav.GetRange(600, a));
            chart1.Series[0].ChartType = SeriesChartType.Spline;
            double c = (double)a / wav.Count;
            textBox1.Text = c.ToString();

            #endregion
        }

        private async void Button2_Click(object sender, EventArgs e)
        {
            #region:Get MP3
            string url = textBox1.Text;
            if (url.Contains("song?id=") | url.Contains("song/"))
            {
                Regex re = new Regex(@"(song\?id=)\d+", RegexOptions.Compiled);
                string id = re.Match(url).ToString();
                if (id != "")
                {
                    id = id.Replace("song?id=", "");
                }
                else
                {
                    re = new Regex(@"(song/)\d+", RegexOptions.Compiled);
                    id = re.Match(url).ToString();
                    id = id.Replace("song/", "");
                }
                string name = "";
                name = await api.GetSingle(id);
            }
            string appPath = Application.StartupPath;
            await api.DownloadAll(appPath);
            textBox1.Text = "Done.";
            #endregion
        }
    }
}
