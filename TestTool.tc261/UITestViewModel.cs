
using Demo.Driver.atp;
using Demo.Driver.ccd.ATP2000SH;
using FuX.Core.Communication.serial;
using FuX.Core.db;
using FuX.Core.extend;
using FuX.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTool.tc261.template;

namespace TestTool.tc261
{
    public class UITestViewModel: CommunicationClientTemplateViewModel<ATPData.Basics>
    {
        public  UITestViewModel()
        {
            base.BasicsData = new ATPData.Basics();
            base.FileName = typeof(ATPData).Name;
         
            //var atpoperate = CoreUnify<ATPOperate, ATPData.Basics>.Instance(base.BasicsData);
         
        
            //atpoperate.On();
            //base.Key = "Serial";
            //atpoperate.ComSerialPortAsk(0xd0, "hh", null);1

            var db = CoreUnify<DBOperate, DBData>.Instance();
            db.On();
            db.Creat<ConfigInfo>() ;
        }
    }
}
