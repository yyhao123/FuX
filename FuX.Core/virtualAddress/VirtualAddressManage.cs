using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.@enum;
using FuX.Model.Specenum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.virtualAddress
{
    public class VirtualAddressManage : IDisposable
    {
        private ConcurrentDictionary<string, (VirtualAddress virtualAddress, AddressType addressType, DataType dataType)> VirtualAddressIocContainer = new ConcurrentDictionary<string, (VirtualAddress, AddressType, DataType)>();

        private void SetVirtualAddress(VirtualAddressData addressData)
        {
            VirtualAddressData addressData2 = addressData;
            bool flag = false;
            if ((from c in VirtualAddressIocContainer
                 where c.Key == addressData2.AddressName
                 where c.Value.addressType != addressData2.AddressType || c.Value.dataType != addressData2.DataType
                 select c).Count() > 0)
            {
                if (VirtualAddressIocContainer.Remove<string, (VirtualAddress, AddressType, DataType)>(addressData2.AddressName, out var value))
                {
                    value.Item1.Dispose();
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                add(new VirtualAddressData
                {
                    AddressName = addressData2.AddressName,
                    AddressType = addressData2.AddressType,
                    DataType = addressData2.DataType
                });
            }
        }

        private void add(VirtualAddressData addressData)
        {
            VirtualAddress item = CoreUnify<VirtualAddress, VirtualAddressData>.Instance(addressData);
            (VirtualAddress virtualAddress, AddressType addressType, DataType dataType) dVlaue = (item, addressData.AddressType, addressData.DataType);
            VirtualAddressIocContainer.AddOrUpdate(addressData.AddressName, dVlaue, (string k, (VirtualAddress virtualAddress, AddressType addressType, DataType dataType) v) => dVlaue);
        }

        private VirtualAddress? GetVirtualAddress(string addressName)
        {
            if (VirtualAddressIocContainer != null)
            {
                return VirtualAddressIocContainer[addressName].virtualAddress;
            }
            return null;
        }

        public void InitVirtualAddress(AddressDetails details, out bool IsVA)
        {
            if (IsVirtualAddress(details))
            {
                SetVirtualAddress(new VirtualAddressData
                {
                    AddressName = details.AddressName,
                    DataType = details.AddressDataType,
                    AddressType = details.AddressType
                });
                IsVA = true;
            }
            else
            {
                IsVA = false;
            }
        }

        public bool IsVirtualAddress(AddressDetails details)
        {
            if (!details.AddressType.Equals(AddressType.VirtualStatic) && !details.AddressType.Equals(AddressType.VirtualDynamic_Random) && !details.AddressType.Equals(AddressType.VirtualDynamic_RandomScope) && !details.AddressType.Equals(AddressType.VirtualDynamic_Order))
            {
                return details.AddressType.Equals(AddressType.VirtualDynamic_OrderScope);
            }
            return true;
        }

        public bool IsVirtualAddress(string AddressName)
        {
            if (VirtualAddressIocContainer.ContainsKey(AddressName))
            {
                return true;
            }
            return false;
        }

        public object? Read(AddressDetails details)
        {
            return GetVirtualAddress(details.AddressName)?.Read();
        }

        public object? Read(string AddressName)
        {
            return GetVirtualAddress(AddressName)?.Read();
        }

        public bool Write(AddressDetails details, object Value)
        {
            return GetVirtualAddress(details.AddressName)?.Write(Value) ?? false;
        }

        public bool Write(string AddressName, object Value)
        {
            return GetVirtualAddress(AddressName)?.Write(Value) ?? false;
        }

        public void Dispose()
        {
            if (VirtualAddressIocContainer.Count > 0)
            {
                foreach (KeyValuePair<string, (VirtualAddress, AddressType, DataType)> item in VirtualAddressIocContainer)
                {
                    item.Value.Item1.Dispose();
                }
            }
            VirtualAddressIocContainer.Clear();
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }

}
