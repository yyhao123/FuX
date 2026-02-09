using CommunityToolkit.Mvvm.Input;
using Snet.Core.cache.share;
using Snet.Windows.Core;
using Snet.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ChannelTest
{
   public class MainWindowViewModel : BindNotify
    {
        ShareCacheOperate _shareCacheOperate;
        public List<treelist> treelistsource=new List<treelist>();
        public MainWindowViewModel()
        {
            Node = new ObservableCollection<OpcUaNodeBrowseStructuralBody> { new OpcUaNodeBrowseStructuralBody() { Name = "hhhh" }};
            Node[0].Children.Add(new OpcUaNodeBrowseStructuralBody() { Name = "ddd二级" });
            Node[0].Children.Add(new OpcUaNodeBrowseStructuralBody() { Name = "ddd二级" });
            Node[0].Children.Add(new OpcUaNodeBrowseStructuralBody() { Name = "ddd二级" });

            ShareCacheData data = new ShareCacheData();
            data.Path =  AppDomain.CurrentDomain.BaseDirectory;
            _shareCacheOperate = ShareCacheOperate.Instance(data);

        }
        public List<treelist> Treelistsource { get; set; }


        public ObservableCollection<OpcUaNodeBrowseStructuralBody> Node
        {
            get
            {
                return GetProperty(() => Node);
            }
            set
            {
                SetProperty<ObservableCollection<OpcUaNodeBrowseStructuralBody>>(() => Node, value);
            }
        }

        public IAsyncRelayCommand TreeViewItem_Expanded => new AsyncRelayCommand<RoutedEventArgs>(TreeViewItem_ExpandedAsync);

        private async Task TreeViewItem_ExpandedAsync(RoutedEventArgs? e)
        {
          
            //if (e?.OriginalSource is TreeViewItem item && item.DataContext is OpcUaNodeBrowseStructuralBody node )
            //{
            //    node.Children.Clear();
            //    node.Children.Add(new OpcUaNodeBrowseStructuralBody() { Name = "ddd" });
            //    // await GetNodeInformAsync((NodeId)reference.NodeId, node, tokenSource);
            //}
        }

        public IAsyncRelayCommand ClickCommand => new AsyncRelayCommand(Click);

        private async Task Click()
        {
            {
                // 创建一个double数组
                double[] doubleArray = new double[3] {1,2,3 };
                double[] doubleArray1 = new double[4] {1,2,3,4};
               
                byte[] byteArray = ConvertToByteArray<double[]>(doubleArray);
                byte[] byteArray1 = ConvertToByteArray<double[]>(doubleArray1);
                _shareCacheOperate.SetCache("测试", byteArray1);
                _shareCacheOperate.SetCache("测试", byteArray);
              
                //  _shareCacheOperate.SetCache("测试2", byteArray);
                var d = _shareCacheOperate.GetCache("测试").GetSource<byte[]>();
                var c = ConvertToObject<double[]>(d);
            }
          
        }

        static byte[] ConvertToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", "输入对象不能为空");
            }

            string jsonString = JsonSerializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

            return byteArray;
        }

        public static T ConvertToObject<T>(byte[] byteArray)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray", "输入字节数组不能为空");
            }

            string jsonString = Encoding.UTF8.GetString(byteArray);

            T obj = JsonSerializer.Deserialize<T>(jsonString);

            return obj;

          
        }



    }

    public class treelist
    {
        public string SN { get; set; } = "1";

        public int Id { get; set; } = 2;
        
    
    }

}
