using FuX.Core.db;
using FuX.Core.extend;
using FuX.Model.data;
using FuX.Model.entities;
using FuX.Model.Specenum;
using FuX.Unility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuX.Core.handler;
using FuX.Model.@enum;


namespace FuX.Core.services
{
    public interface ILocalizeProvider
    {
        string GetString(string name);

       

        bool SaveCfg(string key, object value, bool istemp = false);

        T GetCfg<T>(string str);

        string GetFormatString(string name, params object[] values);
    }
    public class SharpConfigLocalizeProvider:CoreUnify<SharpConfigLocalizeProvider, UserCfg> , ILocalizeProvider
    {

        public SharpConfigLocalizeProvider()
        {
          
        }
        private IUserCfg _userCfg;
      

        public override LanguageModel LanguageOperate { get; set; } = new("Demo.AutoTest.Language", "Language", "Demo.AutoTest.Language.dll");

        public SharpConfigLocalizeProvider(IUserCfg clientCfg):base()
        {
           
            _userCfg = clientCfg;
           _userCfg.UserCfgInfo = DBOperate.Instance().Query<ConfigInfo>(c => 1 == 1).GetSource<List<ConfigInfo>>();
        }
        
        public T GetCfg<T>(string str)
        {
            try
            {
                var obj = _userCfg.UserCfgInfo.FirstOrDefault(t => t.KeyName == str);
                if (obj != null)
                {
                    switch (obj.dataType)
                    {
                        case CfgDataType.Int:
                        case CfgDataType.String:
                        case CfgDataType.Double:
                        case CfgDataType.Bool:
                            return (T)Convert.ChangeType(obj.CfgVal, typeof(T));
                        case CfgDataType.IntList:
                            return (T)Convert.ChangeType(CommonFuncHandler.JsonStrToList<int>(obj.CfgVal), typeof(T));
                        case CfgDataType.DoubleList:
                            return (T)Convert.ChangeType(CommonFuncHandler.JsonStrToList<double>(obj.CfgVal), typeof(T));
                        case CfgDataType.Enumerate:
                            return (T)Convert.ChangeType(CommonFuncHandler.GetEnumVal(obj.DataInfo, obj.CfgVal), typeof(T));
                        case CfgDataType.DataObject:
                            return (T)Convert.ChangeType(CommonFuncHandler.GetClassObjVal(obj.DataInfo, obj.CfgVal), typeof(T));
                        case CfgDataType.DataObjectList:
                            return (T)Convert.ChangeType(CommonFuncHandler.GetClassObjList(obj.DataInfo, obj.CfgVal), typeof(T));
                        default:
                            //throw new AlterException("类型不存在！");
                            throw new Exception("类型不存在！");

                    }
                }
                else
                {
                    throw new Exception("键值【" + str + "】在库中不存在！");

                    //throw new AlterException("键值【" + str + "】在库中不存在！");
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public string GetString(string name)
        {
           // OnLanguage();
            return LanguageOperate.GetLanguageValue(name);
        }

        
        public bool SaveCfg(string key, object value, bool istemp = false)
        {
            if (_userCfg.UserCfgInfo.Count <= 0) return false;

            var obj = _userCfg.UserCfgInfo.FirstOrDefault(t => t.KeyName == key);
            if (obj != null)
            {
                switch (obj.dataType)
                {
                    case CfgDataType.Int:
                    case CfgDataType.String:
                    case CfgDataType.Double:
                    case CfgDataType.Enumerate:
                    case CfgDataType.Bool:
                        obj.CfgVal = value.ToString();
                        break;
                    case CfgDataType.IntList:
                        obj.CfgVal = CommonFuncHandler.ListToJsonStr<int>(value as List<int>);
                        break;
                    case CfgDataType.DoubleList:
                        obj.CfgVal = CommonFuncHandler.ListToJsonStr<double>(value as List<double>);
                        break;
                    case CfgDataType.DataObject:
                    case CfgDataType.DataObjectList:
                        obj.CfgVal = JsonConvert.SerializeObject(value);
                        break;
                    default:
                        return false;
                }
                if (!istemp)
                    DBOperate.Instance().Update<ConfigInfo>(obj,c=>1==1,c=>1==1);
                return true;
            }
            else
            {
                throw new Exception("键值【" + key + "】在库中不存在！");
            }
        }

        public string GetFormatString(string name, params object[] values)
        {  
            return string.Format(LanguageOperate.GetLanguageValue(name), values);
        }

       

       
    }

    public interface ILocalize
    {
        
       
        /// <summary>
        /// 保存配置数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="istemp">是否作为临时，true情况下将不存储，默认为自动存储</param>
        /// <returns></returns>
        bool SaveCfg(string key, object value, bool istemp = false);

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键名</param>
        /// <returns></returns>
        T GetCfg<T>(string key);

        /// <summary>
        /// 多语言下获取提示语句
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="values">占位值</param>
        /// <returns></returns>
        string GetFormatString(string name, params object[] values);

        /// <summary>
        /// 多语言下获取提示语句
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns></returns>
        string GetString(string name);

        /// <summary>
        /// 多语言下获取提示语句
        /// </summary>
        /// <param name="name">键名</param>
        /// <param name="value">拼接值</param>
        /// <returns></returns>
        string GetString(string name, int value);

    }

    public class Localize : ILocalize
    {
        private ILocalizeProvider _provider;

        public Localize()
        {
            _provider = SharpConfigLocalizeProvider.Instance();          
        }

        public bool SaveCfg(string key, object value, bool istemp = false)
        {
            return _provider.SaveCfg(key, value, istemp);
        }
        public T GetCfg<T>(string key)
        {
            return _provider.GetCfg<T>(key);
        }

        public string GetFormatString(string name, params object[] values)
        {
            return _provider.GetFormatString(name, values);
        }

        public string GetString(string name)
        {
            return _provider.GetString(name);
        }

        public string GetString(string name, int value)
        {
            var key = name + value.ToString().ToUpper();
            return _provider.GetString(name);
        }
    }

}
