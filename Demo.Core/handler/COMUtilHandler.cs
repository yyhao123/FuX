using Demo.Model.data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Demo.Core.handler
{
    public class COMUtilHandler
    {
        public static List<ComInfo> GetComInfos()
        {
            var res = new List<ComInfo>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();

                var ports = searcher.Get()
                    .Cast<ManagementBaseObject>()
                    .ToList()
                    .Select(p => new
                    {
                        Caption = p["Caption"].ToString(),
                        DeviceID = p["DeviceID"].ToString(),
                        Name = p["Name"].ToString()
                    });

                foreach (var com in portnames)
                {
                    var matchInfo = ports.FirstOrDefault(x => x.Caption.Contains(com));
                    if (matchInfo == null || matchInfo.Name.ToLower().Contains("jlink")) continue;

                    Match match = Regex.Match(matchInfo.DeviceID, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                    if (!match.Success) continue;

                    var theVendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);

                    var theProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16);

                    var comInfo = new ComInfo()
                    {
                        VID = theVendorID,
                        PID = theProductID,
                        COM = com,
                        Display = $"{com}-{matchInfo.Name}"
                    };

                    res.Add(comInfo);
                }
            }

            return res;
        }
    }
}
