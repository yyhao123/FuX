using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Model.data
{
    /// <summary>
    /// 组包模型
    /// </summary>
    public class PackageModel
    {
       

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public PackageModel()
        { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="data">数据</param>
        public PackageModel(byte command, byte[]? data = null)
        {
            this.Command = command;
            this.Data = data;
        }

        /// <summary>
        /// 校验 <br/> 一个字节，自动计算
        /// </summary>
        public byte Cheaksum { get; set; }

        /// <summary>
        /// 命令 <br/> 一个字节
        /// </summary>
        public byte Command { get; set; }

        /// <summary>
        /// 数据 <br/> N字节，可为空
        /// </summary>
        public byte[]? Data { get; set; } = null;

        /// <summary>
        /// 接受到的数据
        /// </summary>
        public byte[]? lDatas { get; set; }

        /// <summary>
        /// 解析后的数据
        /// </summary>
        public object? pDatas{ get; set; }

        /// <summary>
        /// 长度 <br/> 两个字节，自动计算
        /// </summary>
      //  public byte[] Length { get; set; } = { 0x00, (byte)FixedModel.FixedByteLength };

        /// <summary>
        /// 校验数据的完整性 <br/> 通过长度命令数据校验和取低八位来计算
        /// </summary>
        /// <returns>抛出校验的结果值</returns>
        public byte Check(ByteArray byteArr,int position)
        {
            int checksum = 0;
            int oldPosition = byteArr.ReadPosition;
            byteArr.ReadPosition = position;
            int count = byteArr.Buffer.Length - 1;
            for (int i = position; i < count; i++)
            {
                checksum += byteArr.ReadU8();
                checksum &= 0xff;
            }
            byteArr.ReadPosition = oldPosition;
            return (byte)checksum;
        }

        /// <summary>
        /// 获取完整的字节数组 <br/> 自动计算长度和校验字节
        /// </summary>
        /// <param name="packageModel">抛出组包模型的全属性赋值对象</param>
        /// <returns>返回包体字节</returns>
        public byte[] GetBytes(out PackageModel packageModel)
        {
            ByteArray byteArr;
            byte checkSum;
            //字符是否为空
            if (Data == null)
            {
                Data = new byte[0];
            }
            var len = Data.Length + 6;
            byteArr = new ByteArray(len);
            byteArr.Write(FixedModel.HEAD_FIRST);
            byteArr.Write(FixedModel.HEAD_SECOND);

            //数据位置长度
            byte[] lenb = BitConverter.GetBytes(len - 2);
            byteArr.Write(lenb[1]);
            byteArr.Write(lenb[0]);

            //指令
            byteArr.Write(Command);

            //数据位
            foreach (var b in Data)
            {
                byteArr.Write(b);
            }

            //校验位
            checkSum = Check(byteArr, 2);

            byteArr.Write(checkSum);
            packageModel = this;
            return byteArr.Buffer;
        }

        ///// <summary>
        ///// 字节赋值寻位 <br/> 底层上来的数据自动赋值给个个属性
        ///// </summary>
        ///// <param name="bytes">字节数据</param>
        ///// <returns>返回组包模型</returns>
        public PackageModel SetBytes(byte[] bytes)
        {
            var receive = new List<byte>();
            for(var i=0; i < bytes.Count(); i++)
            {
                receive.Add(bytes[i]);
            }
            if (receive.Count > 4
                   && receive[0] == FixedModel.HEAD_FIRST
                   && receive[1] == FixedModel.HEAD_SECOND)
            {
                var length = (receive[2] << 8) + receive[3];
                if (receive.Count == length + FixedModel.HEAD_LENGTH)
                {
                    Command = receive[4];
                    lDatas = receive.Skip(5).Take(receive.Count - 6).ToArray();
                    
                }
                return this;
            }
            else
            {
                throw new Exception($"数据不完整或校验失败：{bytes.ToHexString()}");
            }

                
        }
    }
}
