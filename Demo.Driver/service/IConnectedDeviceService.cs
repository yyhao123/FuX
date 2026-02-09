using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Driver.service
{
    public interface IConnectedDeviceService
    {
        event EventHandler DeviceCollectionChanged;

        IDeviceService DefaultService { get; }

        string id {  get; }
      
    }

    public class ConnectedDeviceService :IConnectedDeviceService
    {
        private static readonly object LOCK_PAD = new object();

        private bool _isRefreshing = false;

        private IList<IDeviceService> ConnectedDevices { get; set; } = new List<IDeviceService>();

        // Declare the event.
        public event EventHandler DeviceCollectionChanged;

        public Func<IDeviceService> DeviceServiceCreator { get; set; }

        public string id { get; set; } = Guid.NewGuid().ToString();
        public IDeviceService DefaultService
        {
            get
            {
                if (ConnectedDevices.Count < 1)
                {
                    lock (LOCK_PAD)
                    {
                        if (ConnectedDevices.Count < 1)
                            ConnectedDevices.Add(DeviceServiceCreator());
                    }
                }

                return ConnectedDevices.FirstOrDefault();
            }
        }

   

        public ConnectedDeviceService()
        {

        }

       

     
    }
}
