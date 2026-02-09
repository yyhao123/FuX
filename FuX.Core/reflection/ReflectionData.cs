using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.reflection
{
    public class ReflectionData
    {
        public class Basics
        {
            public List<DllData> DllDatas { get; set; }
        }

        public class DllData
        {
            public string DllPath { get; set; }

            public bool IsAbsolutePath { get; set; }

            public List<NamespaceData> NamespaceDatas { get; set; }
        }

        public class NamespaceData
        {
            public string Namespace { get; set; }

            public List<ClassData> ClassDatas { get; set; }
        }

        public class ClassData
        {
            public string SN { get; set; }

            public string ClassName { get; set; }

            public object[]? ConstructorParam { get; set; }

            public List<MethodData>? MethodDatas { get; set; }

            public List<EventData>? EventDatas { get; set; }
        }

        public class MethodData
        {
            public string SN { get; set; }

            public bool WhetherExecute { get; set; }

            public string MethodName { get; set; }

            public object[]? MethodParam { get; set; }
        }

        public class EventData
        {
            public string SN { get; set; }

            public string EventName { get; set; }
        }

        public class ReflectionMethodResult
        {
            public MethodInfo Method { get; set; }

            public object InstanceObject { get; set; }
        }

        public class ReflectionEventResult
        {
            public EventInfo Event { get; set; }

            public object InstanceObject { get; set; }
        }
    }
}
