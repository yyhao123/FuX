using FuX.Core.extend;
using FuX.Model.data;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.script
{
    public class ScriptOperate : CoreUnify<ScriptOperate, ScriptData.Basics>, IDisposable
    {
        private V8ScriptEngine Engine { get; set; }

        public ScriptOperate()
        {
            if (Engine == null)
            {
                Engine = new V8ScriptEngine();
            }
        }

        public ScriptOperate(ScriptData.Basics basics)
            : base(basics)
        {
            if (Engine == null)
            {
                Engine = new V8ScriptEngine();
            }
        }

        public override void Dispose()
        {
            Engine.Dispose();
            Engine = null;
            base.Dispose();
        }

        public override async Task DisposeAsync()
        {
            Dispose();
            await base.DisposeAsync();
        }

        public OperateResult Execute(object[]? ScriptParam)
        {
            return Execute(base.basics.ScriptType, base.basics.ScriptCode, base.basics.ScriptFunction, ScriptParam);
        }

        public async Task<OperateResult> ExecuteAsync(object[]? ScriptParam)
        {
            object[] ScriptParam2 = ScriptParam;
            return await Task.Run(() => Execute(ScriptParam2));
        }

        public OperateResult Execute(ScriptData.ScriptType ScriptType, string? ScriptCode, string? ScriptFunction, object[]? ScriptParam)
        {
            BegOperate("Execute");
            try
            {
                string resultData = string.Empty;
                if (ScriptType == ScriptData.ScriptType.JavaScript)
                {
                    Engine.Execute(ScriptCode);
                    resultData = Engine.Invoke(ScriptFunction, ScriptParam).ToString();
                }
                return EndOperate(status: true, null, resultData, null, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\script\\ScriptOperate.cs", "Execute", 100);
            }
            catch (Exception ex)
            {
                return EndOperate(status: false, ex.Message, null, ex, logOutput: true, consoleOutput: true, "F:\\Shunnet\\Demo\\Demo.Core\\script\\ScriptOperate.cs", "Execute", 104);
            }
        }

        public async Task<OperateResult> ExecuteAsync(ScriptData.ScriptType ScriptType, string ScriptCode, string ScriptFunction, object[]? ScriptParam)
        {
            string ScriptCode2 = ScriptCode;
            string ScriptFunction2 = ScriptFunction;
            object[] ScriptParam2 = ScriptParam;
            return await Task.Run(() => Execute(ScriptType, ScriptCode2, ScriptFunction2, ScriptParam2));
        }
    }
}
