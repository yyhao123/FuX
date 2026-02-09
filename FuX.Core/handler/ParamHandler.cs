using FuX.Model.attribute;
using FuX.Model.data;
using FuX.Unility;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.handler
{
    //
    // 摘要:
    //     参数处理
    public class ParamHandler
    {
        //
        // 摘要:
        //     获取参数详情
        //
        // 参数:
        //   obj:
        //     类型对象
        //
        //   name:
        //     名称
        //
        //   description:
        //     描述
        //
        //   properties:
        //     附加属性项集合；如果存在 AutoAllocatingTagAttribute 每个之类中都会添加 附加属性项集合
        //
        // 类型参数:
        //   T:
        //     数据类型
        //
        // 返回结果:
        //     统一返回
        public static OperateResult Get<T>(T obj, string name, string description, List<ParamModel.propertie>? properties = null)
        {
            TimeHandler timeHandler = TimeHandler.Instance(Guid.NewGuid().ToUpperNString());
            timeHandler.StartRecord();
            try
            {
                List<ReflexHandler.LibInstanceParam> classAllPropertyData = ReflexHandler.GetClassAllPropertyData<T>();
                Tuple<Type, string, ReflexHandler.LibInstanceParam> tuple = classAllPropertyData.Select(delegate (ReflexHandler.LibInstanceParam c)
                {
                    AutoAllocatingTagAttribute customAttribute7 = typeof(T).GetProperty(c.Name).GetCustomAttribute<AutoAllocatingTagAttribute>();
                    return (customAttribute7 != null) ? new Tuple<Type, string, ReflexHandler.LibInstanceParam>(customAttribute7.EnumType, c.Name, c) : null;
                }).FirstOrDefault((Tuple<Type, string, ReflexHandler.LibInstanceParam> c) => c != null);
                if (tuple != null)
                {
                    ParamModel paramModel = new ParamModel
                    {
                        Name = name,
                        Description = description,
                        Subset = new List<ParamModel.subset>()
                    };
                    Array array = Enum.GetValues(tuple.Item1);
                    int i;
                    for (i = 0; i < array.Length; i++)
                    {
                        FieldInfo? field = tuple.Item1.GetField(array.GetValue(i).ToString());
                        AutoAllocatingAttribute obj2 = (((object)field != null) ? field.GetCustomAttributes(typeof(AutoAllocatingAttribute), inherit: false)[0] : null) as AutoAllocatingAttribute;
                        Tuple<string, string, object> tuple2 = (tuple.Item3.EnumArray as List<object>)?.Select((dynamic c) => (c.Name == array.GetValue(i).ToString()) ? new Tuple<string, string, object>(c.Name, c.Describe, c.Value) : null).FirstOrDefault((Tuple<string, string, object> c) => c != null);
                        paramModel.Subset.Add(new ParamModel.subset
                        {
                            Name = tuple2.Item1,
                            Description = tuple2.Item2,
                            Propertie = new List<ParamModel.propertie>()
                        });
                        if (properties != null && properties.Count > 0)
                        {
                            paramModel.Subset[i].Propertie.AddRange(properties);
                        }

                        paramModel.Subset[i].Propertie.Add(new ParamModel.propertie
                        {
                            PropertyName = tuple.Item2,
                            Description = tuple.Item3.Describe,
                            Default = tuple2.Item3,
                            Show = false,
                            Use = false,
                            MustFillIn = false,
                            DataCate = null,
                            DetailsTips = null,
                            Pattern = null,
                            FailTips = null
                        });
                        string[] propertyNameArray = obj2.PropertyNameArray;
                        foreach (string item in propertyNameArray)
                        {
                            ReflexHandler.LibInstanceParam libInstanceParam = classAllPropertyData.FirstOrDefault((ReflexHandler.LibInstanceParam c) => c.Name == item);
                            if (libInstanceParam == null)
                            {
                                continue;
                            }

                            string modelValue = ReflexHandler.GetModelValue(libInstanceParam.Name, obj);
                            DisplayAttribute customAttribute = typeof(T).GetProperty(libInstanceParam.Name).GetCustomAttribute<DisplayAttribute>();
                            VerifyAttribute customAttribute2 = typeof(T).GetProperty(libInstanceParam.Name).GetCustomAttribute<VerifyAttribute>();
                            UnitAttribute customAttribute3 = typeof(T).GetProperty(libInstanceParam.Name).GetCustomAttribute<UnitAttribute>();
                            string text = libInstanceParam.Describe;
                            if (customAttribute3 != null && !string.IsNullOrWhiteSpace(customAttribute3.Unit))
                            {
                                text = text + "(" + customAttribute3.Unit + ")";
                            }

                            ParamModel.propertie propertie = new ParamModel.propertie
                            {
                                PropertyName = libInstanceParam.Name,
                                Description = text,
                                Show = (customAttribute?.Show ?? false),
                                Use = (customAttribute?.Use ?? false),
                                MustFillIn = (customAttribute?.MustFillIn ?? false),
                                DataCate = customAttribute?.DataCate,
                                DetailsTips = (customAttribute?.DetailsTips ?? null),
                                Pattern = (customAttribute2?.Pattern ?? null),
                                FailTips = (customAttribute2?.FailTips ?? null)
                            };
                            switch (customAttribute?.DataCate)
                            {
                                case ParamModel.dataCate.select:
                                    propertie.Options = new List<ParamModel.options>();
                                    foreach (dynamic item2 in libInstanceParam.EnumArray as List<object>)
                                    {
                                        string text2 = item2.Describe;
                                        if (!string.IsNullOrEmpty(text2))
                                        {
                                            text2 = $"({(object?)item2.Describe})";
                                        }

                                        propertie.Options.Add(new ParamModel.options
                                        {
                                            Key = item2.Name + text2,
                                            Value = item2.Value
                                        });
                                    }

                                    break;
                                case ParamModel.dataCate.radio:
                                    propertie.Options = new List<ParamModel.options>();
                                    propertie.Options.Add(new ParamModel.options
                                    {
                                        Key = "是",
                                        Value = true
                                    });
                                    propertie.Options.Add(new ParamModel.options
                                    {
                                        Key = "否",
                                        Value = false
                                    });
                                    break;
                            }

                            propertie.Default = modelValue;
                            paramModel.Subset[i].Propertie.Add(propertie);
                        }
                    }

                    return new OperateResult(status: true, paramModel.ToJson(formatting: true), timeHandler.StopRecord().milliseconds, paramModel);
                }

                ParamModel paramModel2 = new ParamModel
                {
                    Name = name,
                    Description = description,
                    Subset = new List<ParamModel.subset>
                {
                    new ParamModel.subset
                    {
                        Description = description,
                        Name = name,
                        Propertie = new List<ParamModel.propertie>()
                    }
                }
                };
                if (properties != null && properties.Count > 0)
                {
                    paramModel2.Subset[0].Propertie.AddRange(properties);
                }

                foreach (ReflexHandler.LibInstanceParam item3 in classAllPropertyData)
                {
                    string modelValue2 = ReflexHandler.GetModelValue(item3.Name, obj);
                    DisplayAttribute customAttribute4 = typeof(T).GetProperty(item3.Name).GetCustomAttribute<DisplayAttribute>();
                    VerifyAttribute customAttribute5 = typeof(T).GetProperty(item3.Name).GetCustomAttribute<VerifyAttribute>();
                    UnitAttribute customAttribute6 = typeof(T).GetProperty(item3.Name).GetCustomAttribute<UnitAttribute>();
                    string text3 = item3.Describe;
                    if (customAttribute6 != null && !string.IsNullOrWhiteSpace(customAttribute6.Unit))
                    {
                        text3 = text3 + "(" + customAttribute6.Unit + ")";
                    }

                    ParamModel.propertie propertie2 = new ParamModel.propertie
                    {
                        PropertyName = item3.Name,
                        Description = text3,
                        Show = (customAttribute4?.Show ?? false),
                        Use = (customAttribute4?.Use ?? false),
                        MustFillIn = (customAttribute4?.MustFillIn ?? false),
                        DataCate = customAttribute4?.DataCate,
                        DetailsTips = (customAttribute4?.DetailsTips ?? null),
                        Pattern = (customAttribute5?.Pattern ?? null),
                        FailTips = (customAttribute5?.FailTips ?? null)
                    };
                    switch (customAttribute4?.DataCate)
                    {
                        case ParamModel.dataCate.select:
                            propertie2.Options = new List<ParamModel.options>();
                            foreach (dynamic item4 in item3.EnumArray as List<object>)
                            {
                                string text4 = item4.Describe;
                                if (!string.IsNullOrEmpty(text4))
                                {
                                    text4 = $"({(object?)item4.Describe})";
                                }

                                propertie2.Options.Add(new ParamModel.options
                                {
                                    Key = item4.Name + text4,
                                    Value = item4.Value
                                });
                            }

                            break;
                        case ParamModel.dataCate.radio:
                            propertie2.Options = new List<ParamModel.options>();
                            propertie2.Options.Add(new ParamModel.options
                            {
                                Key = "是",
                                Value = true
                            });
                            propertie2.Options.Add(new ParamModel.options
                            {
                                Key = "否",
                                Value = false
                            });
                            break;
                    }

                    propertie2.Default = modelValue2;
                    paramModel2.Subset[0].Propertie.Add(propertie2);
                }

                return new OperateResult(status: true, paramModel2.ToJson(formatting: true), timeHandler.StopRecord().milliseconds, paramModel2);
            }
            catch (Exception ex)
            {
                return new OperateResult(status: false, ex.Message, timeHandler.StopRecord().milliseconds);
            }
        }
    }
}
