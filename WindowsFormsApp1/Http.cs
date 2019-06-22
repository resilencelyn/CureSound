﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net;

namespace WindowsFormsApp1
{
    class Http
    {
        public int DownloadSize { get; set; } = 0;

        public async Task<string> GetFromUrl(string url)
        {

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            try
            {
                string response = await client.GetStringAsync(url);

                return response;
            }
            catch
            {
                return "{\"code\":404}";
            }
        }

        public async Task<string> DownloadFormUrl(string url, string path)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            try
            {
                var response = await client.GetAsync(url);

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var fileInfo = new FileInfo(path);
                    using (var fileStream = fileInfo.OpenWrite())
                    {
                        await stream.CopyToAsync(fileStream);
                        return null;
                    }
                }
            }

            catch
            {
                return null;
            }
        }
    }
}
