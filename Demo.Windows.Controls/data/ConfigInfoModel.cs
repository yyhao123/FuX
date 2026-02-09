using CommunityToolkit.Mvvm.ComponentModel;
using Demo.Windows.Core.mvvm;
using FuX.Model.@enum;
using FuX.Model.Specenum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Windows.Controls.data
{
    public partial class ConfigInfoModel:NotifyObject
    {
        [ObservableProperty]
        public string _id ;

        //键名
        [ObservableProperty]
        public string _keyName = string.Empty;

        //值
        [ObservableProperty]
        public string _cfgVal = string.Empty;

        //说明
        [ObservableProperty]
        public string _tipInfo = string.Empty;

        //数据类型
        [ObservableProperty]
        public CfgDataType _cfgDataType = CfgDataType.String;

        //数据对象
        [ObservableProperty]
        public string _dataInfo = string.Empty;
    }
}
