using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Controls.data;
using Demo.Windows.Controls.property;
using Demo.Windows.Core.mvvm;
using FuX.Core.db;
using FuX.Core.handler;
using FuX.Model.data;
using FuX.Model.entities;
using ScottPlot.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo.Windows.Controls.pages
{
    /// <summary>
    /// CMDManagerView.xaml 的交互逻辑
    /// </summary>
    public partial class CMDManagerView : UserControl
    {
        public CMDManagerView()
        {
            InitializeComponent();
        }
    }

    public partial class CMDManagerViewModel:NotifyObject
    {
        [ObservableProperty]
        private List<CmdInfoModel> _cmdDisplay;

        
       

        
        private DBOperate dbOperate;

        public LanguageModel LanguageOperate { get; set; } = new("Demo.Windows.Controls", "Language", "Demo.Windows.Controls.dll");

        public FuXPropertyGridControlFactory CustomPropertyGridControlFactory { get; set; }

        public FuXPropertyGridOperator CustomPropertyGridOperator { get; set; }

        public CMDManagerViewModel()
        {
            CustomPropertyGridControlFactory= new FuXPropertyGridControlFactory();
            CustomPropertyGridOperator= new FuXPropertyGridOperator();
             dbOperate = DBOperate.Instance();
            LoadData();
            EditCmdData = new CmdInfoModel();
        }

        public CmdInfoModel SelectedItem
        {
            get
            {
                return GetProperty(() => SelectedItem);
            }
            set
            {
                SetProperty(() => SelectedItem, value);
            }
        }

        public CmdInfoModel EditCmdData
        {
            get
            {
                return GetProperty(() => EditCmdData);
            }
            set
            {
                SetProperty(() => EditCmdData, value);
            }
        }

        [RelayCommand]
        public async Task LoadData()
        {
            var configinfos = dbOperate.Query<c_Command>(c => 1 == 1);

            var data = configinfos.GetSource<List<c_Command>>();
            var temp = new List<CmdInfoModel>(data.Count);
            foreach (var item in data)
            {
                temp.Add(
                    new CmdInfoModel
                    {
                        CmdName = item.cmdName,
                        CmdNum = item.cmdNum,
                        TimeOut = item.timeout,
                        KeyName = item.KeyName,
                        InType = item.InType,
                        RetType = item.retType,
                        DevName = item.devName,
                        StrNum = item.strnum,
                        Remark = item.remark,
                        Created = item.Created,
                        Id = item.id,


                    });
            }
              CmdDisplay = temp;


        }



        [RelayCommand]
        private void OnSelectionChanged(object obj)
        {
            if (SelectedItem == null) return;
            EditCmdData.DevName = SelectedItem.DevName;
            EditCmdData.CmdName = SelectedItem.CmdName;
            EditCmdData.CmdNum = SelectedItem.CmdNum;
            EditCmdData.InType = SelectedItem.InType;
            EditCmdData.KeyName = SelectedItem.KeyName;
            EditCmdData.Remark = SelectedItem.Remark;
            EditCmdData.RetType = SelectedItem.RetType;
            EditCmdData.StrNum = SelectedItem.StrNum;
            EditCmdData.TimeOut = SelectedItem.TimeOut;
            EditCmdData.Id = SelectedItem.Id;
        }

        /// <summary>
        /// 新增
        /// </summary>
        [RelayCommand]
        private async void OnAdd()
        {
            var cmd = (c_Command)await DataCheck();
            if (cmd != null)
            {
                var ret = dbOperate.Insert(EditCmdData);
                if (ret.Status)
                {
                    await LoadData();
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
                c_Command cmd = new c_Command()
                {
                    cmdName = EditCmdData.CmdName,
                    cmdNum = EditCmdData.CmdNum,
                    KeyName=EditCmdData.KeyName,
                    InType = EditCmdData.InType,
                    retType = EditCmdData.RetType,
                    remark = EditCmdData.Remark,
                    timeout= EditCmdData.TimeOut,
                    devName = EditCmdData.DevName,
                    strnum = EditCmdData.StrNum,
                    id=EditCmdData.Id,


                };
                var ret = dbOperate.Update<c_Command>(cmd, c => 1 == 1, c => 1 == 1);
                if (ret.Status)
                {
                    await LoadData();
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
            var ret = dbOperate.Delete<ConfigInfo>(c => c.KeyName == SelectedItem.KeyName);
            if (ret.Status)
            {
                await LoadData();
                await Demo.Windows.Controls.message.MessageBox.Show("操作成功！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

            }
            else
                await Demo.Windows.Controls.message.MessageBox.Show("操作失败！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

        }

        private async Task<c_Command> DataCheck()
        {
            c_Command cmd = new c_Command();
            var cmdName = EditCmdData.CmdName;
            var cmdNum = EditCmdData.CmdNum;
            var memo = EditCmdData.Remark;
            var rettype = EditCmdData.RetType;
            var intype = EditCmdData.InType;
            var strnum = EditCmdData.StrNum;
            var devname = EditCmdData.DevName;
            var keyname = EditCmdData.KeyName;
            var id = EditCmdData.Id;
            if (string.IsNullOrEmpty(devname))
            {
                await Demo.Windows.Controls.message.MessageBox.Show("请输入设备名！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }
          
            if (string.IsNullOrEmpty(cmdName))
            {
                await Demo.Windows.Controls.message.MessageBox.Show("名称不能为空！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }
            //if (dbOperate.Query<c_Command>().GetSource<List<c_Command>>().Where(c => c.KeyName == keyname).Count() > 0)
            //{
            //    await Demo.Windows.Controls.message.MessageBox.Show("键名已存在!", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);
            //    return null;
            //}
            if (string.IsNullOrEmpty(cmdNum))
            {
                await Demo.Windows.Controls.message.MessageBox.Show("指令编号不能为空！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }
            if (string.IsNullOrEmpty(rettype.ToString()))
            {
                await Demo.Windows.Controls.message.MessageBox.Show("请选择返回类型！", LanguageOperate.GetLanguageValue("提示"), Demo.Windows.Controls.@enum.MessageBoxButton.OK, Demo.Windows.Controls.@enum.MessageBoxImage.Information);

                return null;
            }

            cmd.cmdNum = cmdNum;
            cmd.cmdName = cmdName;
            cmd.devName = devname;
            cmd.InType = intype;
            cmd.retType = rettype;
            cmd.remark = memo;
            cmd.strnum = strnum;
            cmd.KeyName = keyname;
            cmd.id =id==null? cmd.id:id;
            return cmd;
        }
    }

}
