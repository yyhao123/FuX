using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Controls.data;
using Demo.Windows.Core.mvvm;
using FuX.Core.db;
using FuX.Core.extend;
using FuX.Core.handler;
using FuX.Model.data;
using FuX.Model.entities;
using FuX.Model.@enum;
using FuX.Model.Specenum;
using FuX.Unility;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;


namespace Demo.Windows.Controls.pages
{
    /// <summary>
    /// ConfigManagerView.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigManagerView : UserControl
    {
        public ConfigManagerView()
        {
            InitializeComponent();
        }

        private void DataGrid_CustomizeColumns_GlobalSearch(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_CustomizeColumns_CancelGlobalSearch(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_MainWindow_PageChanged(object sender, RoutedEventArgs e)
        {

        }
    }


    public partial class ConfigManagerViewModel:NotifyObject
    {
     
        private DBOperate dbOperate;

        public ObservableCollection<ConfigInfo> ConfigInfos { get; set; }

        public LanguageModel LanguageOperate { get; set; } = new("Demo.Windows.Controls", "Language", "Demo.Windows.Controls.dll");

        public ConfigManagerViewModel()
        {
            dbOperate = CoreUnify<DBOperate, DBData>.Instance();
            dbOperate.On();

          //  LoadData();

        }
        #region 界面绑定属性
        //键名
        [ObservableProperty]
        string _keyName = string.Empty;

        //值
        [ObservableProperty]
        string _cfgVal = string.Empty;

        //说明
        [ObservableProperty]
        string _tipInfo = string.Empty;

        [ObservableProperty]
        private List<ConfigInfoModel> _configDisplay;

        [ObservableProperty]
        private ConfigInfoModel _selectedItem;

        [ObservableProperty]
        private CfgDataType? _cfgDataType;

        //数据对象
        [ObservableProperty]
        string _dataInfo = string.Empty;

        #endregion

        #region 界面绑定命令

        /// <summary>
        /// 双击
        /// </summary>
        public IAsyncRelayCommand LoadDataCommand => new AsyncRelayCommand(LoadDataAsync);

        public async Task LoadDataAsync()
        {
            var configinfos = dbOperate.Query<ConfigInfo>(c => 1 == 1);

            var data = configinfos.GetSource<List<ConfigInfo>>();
            var temp = new List<ConfigInfoModel>(data.Count);
            foreach (var item in data)
            {
                temp.Add(
                    new ConfigInfoModel
                    {
                        Id = item.id,
                        KeyName = item.KeyName,
                        CfgVal = item.CfgVal,
                        CfgDataType = item.dataType,
                        TipInfo = item.tipInfo,
                        DataInfo = item.DataInfo,

                    });
            }
            ConfigDisplay = temp;
        

        }


        [RelayCommand]
        private void OnSelectionChanged(object obj)
        {
            KeyName = SelectedItem?.KeyName;
            CfgVal= SelectedItem?.CfgVal;
            TipInfo = SelectedItem?.TipInfo;
            DataInfo = SelectedItem?.DataInfo;
            CfgDataType= SelectedItem?.CfgDataType;

        }



        /// <summary>
        /// 采集
        /// </summary>
        [RelayCommand]
        private async  void  OnAdd()
        {
            ConfigInfo configInfo = new ConfigInfo();
            if ( await DataCheck(configInfo) != null)
            {
                var ret = dbOperate.Insert(configInfo);
                if (ret.Status)
                {
                   await LoadDataAsync();
                   await Demo.Windows.Controls.message.MessageBox.Show("操作成功！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                }
               
                else
                {
                   await Demo.Windows.Controls.message.MessageBox.Show("操作失败！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                }

            }

        }

        [RelayCommand]
        private async void OnModify()
        {
            try
            {
                ConfigInfo cfg = new ConfigInfo()
                {
                    CfgVal = CfgVal,
                    id = SelectedItem.Id,
                    KeyName = KeyName,
                    tipInfo = TipInfo,
                    dataType = (CfgDataType)CfgDataType,
                    DataInfo = DataInfo,

                };
                var ret = dbOperate.Update<ConfigInfo>(cfg,c=>1==1,c=>1==1);
                if (ret.Status)
                {
                    await LoadDataAsync();
                    await Demo.Windows.Controls.message.MessageBox.Show("操作成功！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                }
                else
                {
                    await Demo.Windows.Controls.message.MessageBox.Show("操作失败！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                }


            }
            catch { }
        }
       

       

        [RelayCommand]
        private async void OnDelete()
        {
            var ret = dbOperate.Delete<ConfigInfo>(c=>c.KeyName==SelectedItem.KeyName);
            if (ret.Status)
            {
                await LoadDataAsync();
                await Demo.Windows.Controls.message.MessageBox.Show("操作成功！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

            }
            else
                await Demo.Windows.Controls.message.MessageBox.Show("操作失败！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

        }

        #endregion

        private async Task<ConfigInfo> DataCheck(ConfigInfo configInfo)
        {
            var key = KeyName;
            var val = CfgVal;
            var type = CfgDataType.ToString();
            var obj =DataInfo;
            var tipInfo =TipInfo;
            if (string.IsNullOrEmpty(key))
            {
               await Demo.Windows.Controls.message.MessageBox.Show("未输入键值！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }
            if(dbOperate.Query<ConfigInfo>().GetSource<List<ConfigInfo>>().Where(c=>c.KeyName==key).Count() > 0)
            {
               await Demo.Windows.Controls.message.MessageBox.Show("键名已存在!", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);
               return null;
            }
            if (string.IsNullOrEmpty(val))
            {
               await Demo.Windows.Controls.message.MessageBox.Show("数值为必填项！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }
            if (string.IsNullOrEmpty(type))
            {
               await Demo.Windows.Controls.message.MessageBox.Show("类型为必填项！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);
            
                return null;
            }
            var etype = (CfgDataType)Enum.Parse(typeof(CfgDataType), type.ToString());
            if (etype == FuX.Model.Specenum.CfgDataType.DataObject || etype == FuX.Model.Specenum.CfgDataType.Enumerate)
            {
                if (string.IsNullOrEmpty(obj))
                {
                   await Demo.Windows.Controls.message.MessageBox.Show("数据对象为必填项！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                    return null;
                }
            }

            configInfo.CfgVal = val;
            configInfo.KeyName = key;
            configInfo.tipInfo = tipInfo;
            configInfo.dataType = etype;
            configInfo.DataInfo = obj;
            return configInfo;
        }

    }


}
