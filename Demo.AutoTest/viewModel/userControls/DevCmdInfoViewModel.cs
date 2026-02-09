using Demo.AutoTest.view.Module;
using Demo.Model.data;
using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.viewModel.userControls
{
    public class DevCmdInfoViewModel: NotifyObject
    {
        public DevCmdInfo DevCmdInfo
        {
            get
            {
                return GetProperty(() => DevCmdInfo);
            }
            set
            {
                SetProperty(() => DevCmdInfo, value);
            }
        }

        public DevCmdInfoViewModel()
        {
            DevCmdInfo = new DevCmdInfo();
        }
    }

    
}
