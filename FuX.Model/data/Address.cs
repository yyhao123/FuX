using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    //
    // 摘要:
    //     地址，包含多个地址
    public class Address
    {
        //
        // 摘要:
        //     可以理解成唯一标识符（可以存机台号、组名、车间、厂）
        public string SN { get; set; } = Guid.NewGuid().ToUpperNString();


        //
        // 摘要:
        //     所有地址 也可以是一个地址 此数组里面的地址应是唯一的
        public List<AddressDetails> AddressArray { get; set; }

        //
        // 摘要:
        //     创建时间
        public DateTime CreationTime { get; set; } = DateTime.Now;


        //
        // 摘要:
        //     带参构造函数
        //
        // 参数:
        //   sn:
        //     唯一标识符
        //
        //   addressDetails:
        //     地址详情集合
        //
        //   creationTime:
        //     创建时间
        public Address(string sn, List<AddressDetails> addressDetails, DateTime creationTime)
        {
            SN = sn;
            AddressArray = addressDetails;
            CreationTime = creationTime;
        }

        //
        // 摘要:
        //     带参构造函数
        //
        // 参数:
        //   addressDetails:
        //     地址详情集合
        public Address(List<AddressDetails> addressDetails)
        {
            AddressArray = addressDetails;
        }

        //
        // 摘要:
        //     带参构造函数
        //
        // 参数:
        //   addressDetails:
        //     地址详情
        public Address(AddressDetails addressDetails)
        {
            if (AddressArray == null)
            {
                AddressArray = new List<AddressDetails>();
            }

            AddressArray.Add(addressDetails);
        }

        //
        // 摘要:
        //     无参构造函数
        public Address()
        {
        }

        //
        // 摘要:
        //     获取地址信息
        //
        // 参数:
        //   AddressName:
        //     PLC地址
        //
        //   AddressAnotherName:
        //     地址别名
        //
        // 返回结果:
        //     地址详情
        public AddressDetails? GetAddressInfo(string? AddressName = null, string? AddressAnotherName = null)
        {
            string AddressName2 = AddressName;
            string AddressAnotherName2 = AddressAnotherName;
            if (AddressName2 != null && AddressAnotherName2 != null)
            {
                return AddressArray?.Find((AddressDetails c) => c.AddressName.Equals(AddressName2) && c.AddressAnotherName.Equals(AddressAnotherName2));
            }

            if (AddressName2 != null && AddressAnotherName2 == null)
            {
                return AddressArray?.Find((AddressDetails c) => c.AddressName.Equals(AddressName2));
            }

            if (AddressName2 == null && AddressAnotherName2 != null)
            {
                return AddressArray?.Find((AddressDetails c) => c.AddressAnotherName.Equals(AddressAnotherName2));
            }

            return null;
        }

        //
        // 摘要:
        //     检查地址是否存在无效数据
        //
        // 返回结果:
        //     true:一切正常
        //     false:存在无效数据
        public bool CheckAddress()
        {
            if (AddressArray != null && AddressArray.Count > 0 && AddressArray.Where((AddressDetails c) => string.IsNullOrWhiteSpace(c.AddressName)).Count() == 0)
            {
                return true;
            }

            return false;
        }

        //
        // 摘要:
        //     移除虚拟点，用于订阅
        //
        // 返回结果:
        //     地址详情集合
        public List<AddressDetails> RemoveVirtualAddress()
        {
            if (AddressArray.Count > 0)
            {
                return ((Address)MemberwiseClone()).AddressArray.Where((AddressDetails c) => c.AddressType.Equals(AddressType.Reality)).ToList();
            }

            return AddressArray;
        }

        //
        // 摘要:
        //     获取实际地址数量
        //
        // 返回结果:
        //     实际地址数量
        public int GetRealityAddressCount()
        {
            return RemoveVirtualAddress().Count();
        }

        //
        // 摘要:
        //     重写ToString；
        //     响应 json 字符串
        //
        // 返回结果:
        //     json字符串
        public override string ToString()
        {
            return this.ToJson(formatting: true) ?? string.Empty;
        }

        //
        // 摘要:
        //     重写Equals
        //
        // 参数:
        //   o:
        //     对象
        //
        // 返回结果:
        //     是否一致
        public override bool Equals(object? o)
        {
            if (o == null)
            {
                return false;
            }

            return this.Comparer(o as Address).result;
        }
    }
}
