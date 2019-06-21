using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NeteaseMusicAPI;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading;
using VaderSharp;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            String[] Comments = Program.GetComments(25749043);
            String[] Sentences = Program.GetLyrics();
            ArrayList SArrayList = new ArrayList(Sentences);
            SArrayList.RemoveAt(0);
            Sentences = (string[])SArrayList.ToArray(typeof(string));
            double[] score = Program.LoopTest(Sentences);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(score, Comments));
        }
        private static String[] GetComments(int music_id)
        {
            NeteaseMusicAPI.NeteaseMusicAPI NMAPI = new NeteaseMusicAPI.NeteaseMusicAPI();
            int offset = 0;
            JObject Response = NMAPI.GetSongComments(id:music_id,offset:offset);
            String[] C = new string[(int)Response["total"]];
            JToken Comments;
            while (true)
            {
                Comments = Response["comments"];
                for (int i = offset; i < offset + Comments.Count(); i++) 
                {
                    C[i] = (String)Comments[i-offset]["content"];
                }
                if (!(Boolean)Response["more"])
                {
                    break;
                }
                offset += 20;
                Response = NMAPI.GetSongComments(id: music_id, offset: offset);
            }
            return C;
        }
        private static String[] GetLyrics()
        {
            NeteaseMusicAPI.NeteaseMusicAPI NMAPI = new NeteaseMusicAPI.NeteaseMusicAPI();
            JObject User = NMAPI.GetUserPlaylists(112321715);
            JObject Playlist = NMAPI.GetPlaylistDetail(456306470);
            
            JObject Music = NMAPI.GetLyric(438456552);
            String Lyric = (String)Music["lrc"]["lyric"];
            String[] Sentences = Lyric.Split('\n');
            return Sentences;
        }
        static double GetScore(String Input)
        {
            SentimentIntensityAnalyzer SIA = new SentimentIntensityAnalyzer();
            SentimentAnalysisResults SAR = SIA.PolarityScores(Input);
            double CS = SAR.Compound;
            return CS;
        }
        static double[] LoopTest(String[] Input)
        {
            String Temp = "";
            double[] score = new double[Input.Length];

            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i].Length > 10)
                {
                    Temp = Input[i].Substring(10);

                }
                else
                {
                    Temp = "";
                }
                double CS = Program.GetScore(Temp);
                score[i] = CS;
                Console.WriteLine("歌词：" + Temp + "\n情绪评分：" + CS.ToString());
            }
            return score;
        }
    }
}
