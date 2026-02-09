using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FuX.Unility
{
    public class NetHandler
    {
        public enum IpType
        {
            System,
            Local
        }

        public static string? GetIP(IpType ipType)
        {
            switch (ipType)
            {
                case IpType.System:
                    {
                        NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                        foreach (NetworkInterface networkInterface in allNetworkInterfaces)
                        {
                            if (!networkInterface.Supports(NetworkInterfaceComponent.IPv4))
                            {
                                continue;
                            }
                            UnicastIPAddressInformationCollection unicastAddresses = networkInterface.GetIPProperties().UnicastAddresses;
                            if (unicastAddresses.Count <= 0)
                            {
                                continue;
                            }
                            foreach (UnicastIPAddressInformation item in unicastAddresses)
                            {
                                if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    return item.Address.ToString();
                                }
                            }
                        }
                        return null;
                    }
                case IpType.Local:
                    {
                        string result = string.Empty;
                        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                        foreach (IPAddress iPAddress in addressList)
                        {
                            if (iPAddress.AddressFamily.ToString() == "InterNetwork")
                            {
                                result = iPAddress.ToString();
                                break;
                            }
                        }
                        return result;
                    }
                default:
                    return null;
            }
        }

        public static bool ClearSamePortProcess(int Port)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                List<int> pidByPort = GetPidByPort(process, Port);
                if (pidByPort.Count == 0)
                {
                    return true;
                }
                PidKill(process, pidByPort);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static List<string> GetProcessNameByPid(Process p, List<int> list_pid)
        {
            p.Start();
            List<string> list = new List<string>();
            foreach (int item in list_pid)
            {
                p.StandardInput.WriteLine($"tasklist |find \"{item}\"");
                p.StandardInput.WriteLine("exit");
                StreamReader standardOutput = p.StandardOutput;
                string text = standardOutput.ReadLine();
                while (!standardOutput.EndOfStream)
                {
                    text = text.Trim();
                    if (text.Length > 0 && text.Contains(".exe"))
                    {
                        string[] array = new Regex("\\s+").Split(text);
                        if (array.Length != 0)
                        {
                            list.Add(array[0]);
                        }
                    }
                    text = standardOutput.ReadLine();
                }
                p.WaitForExit();
                standardOutput.Close();
            }
            p.Close();
            return list;
        }

        private static List<int> GetPidByPort(Process p, int port)
        {
            p.Start();
            p.StandardInput.WriteLine($"netstat -ano|find \"{port}\"");
            p.StandardInput.WriteLine("exit");
            StreamReader standardOutput = p.StandardOutput;
            string text = standardOutput.ReadLine();
            List<int> list = new List<int>();
            while (!standardOutput.EndOfStream)
            {
                text = text.Trim();
                if (text.Length > 0 && (text.Contains("TCP") || text.Contains("UDP")))
                {
                    string[] array = new Regex("\\s+").Split(text);
                    if (array.Length >= 4 && int.TryParse(array[3], out var result) && !list.Contains(result))
                    {
                        list.Add(result);
                    }
                }
                text = standardOutput.ReadLine();
            }
            p.WaitForExit();
            standardOutput.Close();
            p.Close();
            return list;
        }

        private static void PidKill(Process p, List<int> list_pid)
        {
            p.Start();
            foreach (int item in list_pid)
            {
                p.StandardInput.WriteLine("taskkill /pid " + item + " /f");
                p.StandardInput.WriteLine("exit");
            }
            p.Close();
        }

        public static bool Ping(string Ip)
        {
            if (string.IsNullOrEmpty(Ip))
            {
                return false;
            }
            try
            {
                if (new Ping().Send(Ip).Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public static bool Telnet(string Ip, int Port)
        {
            if (string.IsNullOrEmpty(Ip) || Port.Equals(0))
            {
                return false;
            }
            try
            {
                IPAddress address = IPAddress.Parse(Ip);
                IPEndPoint point = new IPEndPoint(address, Port);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    bool num = Task.Run(delegate
                    {
                        sock.Connect(point);
                    }).Wait(500);
                    if (num)
                    {
                        sock.Close();
                    }
                    return !num;
                }
                finally
                {
                    if (sock != null)
                    {
                        ((IDisposable)sock).Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string GetUrlIp(string Url)
        {
            try
            {
                return Dns.GetHostAddresses(Url)[0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool IsPortInUse(int port)
        {
            bool result = false;
            try
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                tcpListener.Stop();
                tcpListener.Dispose();
                return result;
            }
            catch (SocketException)
            {
                return true;
            }
        }
    }

}
