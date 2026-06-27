using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace api_auditor.Classes
{
    internal class ResultRow
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public int? StatusCode { get; set; }
        public string Accessible { get; set; }
        public string TokenRequired { get; set; }
        public string Notes { get; set; }
        public long Ms { get; set; }
    }

    static class EndpointTester
    {
        public static async Task<List<ResultRow>> GenerateReportAsync(string routesFilePath)
        {
            var lines = await Task.Run(() => File.ReadAllLines(routesFilePath));
            var rows = new List<ResultRow>();
            var client = new HttpClient();

            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var parts = lines[i].Split('|');
                if (parts.Length < 6) continue;

                var row = new ResultRow
                {
                    Controller = parts[0],
                    Action = parts[1],
                    HttpMethod = parts[2],
                    Url = parts[4]
                };

                var sw = Stopwatch.StartNew();
                try
                {
                    var req = new HttpRequestMessage(new HttpMethod(row.HttpMethod), row.Url);
                    var resp = await client.SendAsync(req);
                    sw.Stop();

                    row.StatusCode = (int)resp.StatusCode;
                    row.Ms = sw.ElapsedMilliseconds;
                    row.Accessible = resp.IsSuccessStatusCode ? "SI" : "NO";
                    row.TokenRequired = resp.StatusCode == System.Net.HttpStatusCode.Unauthorized || resp.StatusCode == System.Net.HttpStatusCode.Forbidden ? "SI" : "NO";
                    row.Notes = resp.ReasonPhrase ?? "";
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    row.Ms = sw.ElapsedMilliseconds;
                    row.Accessible = "NO";
                    row.TokenRequired = "DESCONOCIDO";
                    row.Notes = ex.GetType().Name;
                }

                rows.Add(row);
            }

            var swOut = new StreamWriter($"endpoints_report_{DateTime.Now:M:d:mm:ss}.csv");
            await swOut.WriteLineAsync("Controller,Action,HttpMethod,Url,StatusCode,Accessible,TokenRequired,Notes,Ms");
            foreach (var r in rows)
            {
                await swOut.WriteLineAsync($"\"{r.Controller}\",\"{r.Action}\",\"{r.HttpMethod}\",\"{r.Url}\",\"{r.StatusCode}\",\"{r.Accessible}\",\"{r.TokenRequired}\",\"{r.Notes.Replace("\"", "'")}\",\"{r.Ms}\"");
            }

            Console.WriteLine($"CSV generado con {rows.Count} resultados.");
            return rows;
        }

    }

}
