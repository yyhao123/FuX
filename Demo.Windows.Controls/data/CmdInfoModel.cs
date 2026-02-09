using CommunityToolkit.Mvvm.ComponentModel;
using Demo.Windows.Core.mvvm;
using FuX.Model.attribute;
using FuX.Model.data;
using FuX.Model.Specenum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Windows.Controls.data
{
    public partial class CmdInfoModel:NotifyObject
    {
        //指令名
        [DisplayName("指令名称")]
        public String CmdName { get{ return GetProperty(() => CmdName); } set{SetProperty(() => CmdName, value);}}


        //指令编号
        [DisplayName("指令编号")]
        public String CmdNum { get { return GetProperty(() => CmdNum); } set { SetProperty(() => CmdNum, value); } }

        //超时时长
        [DisplayName("超时时长")]
        public int TimeOut { get { return GetProperty(() => TimeOut); } set { SetProperty(() => TimeOut, value); } }
     

        //入参类型
        [DisplayName("入参类型")]
        public CmdInDataType InType { get { return GetProperty(() => InType); } set { SetProperty(() => InType, value); } }

        //入参长度
        [DisplayName("入参长度")]
        public int StrNum { get { return GetProperty(() => StrNum); } set { SetProperty(() => StrNum, value); } }
   

        //出参类型
        [DisplayName("出参类型")]
        public CmdRetDataType RetType { get { return GetProperty(() => RetType); } set { SetProperty(() => RetType, value); } }

        //键名
        [DisplayName("键名")]
        public String KeyName { get { return GetProperty(() => KeyName); } set { SetProperty(() => KeyName, value); } }

        //设备名
        [DisplayName("设备名")]
        public String DevName { get { return GetProperty(() => DevName); } set { SetProperty(() => DevName, value); } }

        //备注
        [DisplayName("备注")]
        public String Remark { get { return GetProperty(() => Remark); } set { SetProperty(() => Remark, value); } }

        //创建时间
        [DisplayName("创建时间")]
        public DateTime Created { get { return GetProperty(() => Created); } set { SetProperty(() => Created, value); } }

        //ID
        [DisplayName("ID")]
        public string Id { get { return GetProperty(() => Id); } set { SetProperty(() => Id, value); } }

        
    }
}
