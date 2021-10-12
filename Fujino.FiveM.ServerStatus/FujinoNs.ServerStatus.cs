using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace FujinoNs.ServerStatus
{
    public class ServerStatus
    {
        bool _status { get; set; }
        string _playerCount { get; set; }

        dataAPI data = new dataAPI();
        public ServerStatus(string ip)
        {
            try
            {
                InternetShortcut shortcut = new InternetShortcut();
                shortcut.CreateShortcut("Fujino.FiveM.ServerStatus2", "https://www.nuget.org/packages/Fujino.FiveM.ServerStatus2");

                if (File.Exists("Newtonsoft.Json.dll"))
                {
                    if (IsInternetConnection(ip) == true)
                    {
                        _status = true;
                        data = JsonConvert.DeserializeObject<dataAPI>(new WebClient().DownloadString("http://" + ip + "/fujino_fivem_server_status"));
                        _playerCount = data.kc_playercount;
                    }
                    else
                    {
                        _status = false;
                        _playerCount = "0";
                    }
                }
                else
                {
                    Console.WriteLine("\n\n[Fujino.FiveM.ServerStatus2.dll] - [Error: Please install Packages: Newtonsoft.Json Version: 13.0.1]");
                    Console.WriteLine("[Fujino.FiveM.ServerStatus2.dll] - [Please install in Package Manager: Install-Package Newtonsoft.Json -Version 13.0.1]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Fujino.FiveM.ServerStatus2.dll] - [Error:" + ex.Message + "]");
            }
        }

        public bool Status()
        {
            return _status;
        }

        public string PlayerCount()
        {
            return _playerCount;
        }

        static bool IsInternetConnection(string ip)
        {
            WebRequest req = WebRequest.Create("http://" + ip + "/fujino_fivem_server_status");
            WebResponse resp;
            try
            {
                resp = req.GetResponse();
                resp.Close();
                req = null;
                return true;
            }
            catch (Exception)
            {
                req = null;
                return false;
            }
        }

    }

    internal class dataAPI
    {
        public string kc_playercount { get; set; }
    }

    class InternetShortcut
    {
        public void CreateShortcut(string name, string url)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("./" + name + ".url"))
                {
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=" + url);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
