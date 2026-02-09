using CommunityToolkit.Mvvm.Input;
using Snet.Unility;
using Snet.Windows.Core.data;
using Snet.Windows.Core.handler;
using Snet.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using static Microsoft.ClearScript.V8.V8CpuProfile;

namespace ChannelTest
{
    public class OpcUaNodeBrowseStructuralBody : BindNotify
    {
        public object Icon
        {
            get
            {
                return GetProperty(() => Icon);
            }
            set
            {
                SetProperty(() => Icon, value);
            }
        }

        public string IconKey { get; set; }

        public string Name
        {
            get
            {
                return GetProperty(() => Name);
            }
            set
            {
                SetProperty(() => Name, value);
            }
        }

        public object NodeID
        {
            get
            {
                return GetProperty(() => NodeID);
            }
            set
            {
                SetProperty(() => NodeID, value);
            }
        }

        public string Count
        {
            get
            {
                return GetProperty(() => Count);
            }
            set
            {
                SetProperty(() => Count, value);
            }
        }

        public ObservableCollection<OpcUaNodeBrowseStructuralBody> Children
        {
            get
            {
                return GetProperty(() => Children);
            }
            set
            {
                SetProperty<ObservableCollection<OpcUaNodeBrowseStructuralBody>>(() => Children, value);
            }
        }

        public OpcUaNodeBrowseStructuralBody()
        {
            Children = new ObservableCollection<OpcUaNodeBrowseStructuralBody>();
        }
        //SkinHandler.OnSkinEventAsync += SkinHandler_OnSkinEventAsync;
       


        private async Task SkinHandler_OnSkinEventAsync(object? sender, EventSkinResult e)
        {
            if (Application.Current == null)
            {
                return;
            }
            await Application.Current.Dispatcher.InvokeAsync(delegate
            {
                if (!IconKey.IsNullOrWhiteSpace())
                {
                    Icon = (DrawingImage)Application.Current.FindResource(IconKey);
                }
            }, DispatcherPriority.Loaded);
        }
    }
}
