using FuX.Core.extend;
using FuX.Model.data;

using System.Linq.Expressions;
using FuX.Core.handler;
using SqlSugar;

using FuX.Model.@interface;
using NetTaste;
using Newtonsoft.Json.Linq;
using FuX.Core.@abstract;


namespace FuX.Core.db
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class DBOperate : DBAbstract<DBOperate, DBData>, IDB
    {
        /// <summary>
        /// 无惨构造函数
        /// </summary>
        public DBOperate() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="param">参数</param>
        public DBOperate(DBData param) : base(param) { }

        /// <inheritdoc/>
        protected override string CD => "基础的动态增删改查";

        /// <inheritdoc/>
        protected override string CN => "数据库操作";

        /// <inheritdoc/>
        public override LanguageModel LanguageOperate { get; set; } = new("Demo.Language", "Language", "Demo.Language.dll");

        #region 私有属性

        /// <summary>
        /// link db 的操作对象
        /// </summary>
        private SqlSugarClient sqlSugar;

        /// <summary>
        /// 连接路径
        /// </summary>
        private string connectionFile => $"{basics.DBFilePath}\\{basics.DBFileName}";

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connectionString => $"data source={connectionFile}";

        #endregion 私有属性

        public override OperateResult Creat<T>()
        {
            BegOperate();
            try
            {
                //判断状态
                if (!(GetStatus().GetDetails(out string? message)))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if ( Exist<T>().GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
                //创建表
                sqlSugar.CodeFirst.InitTables<T>();
                return EndOperate(true, LanguageOperate.GetLanguageValue("创建成功"));
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }


        public override OperateResult Delete<T>(Expression<Func<T, bool>> condition)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!(GetStatus().GetDetails(out string? message)))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if (!(Exist<T>()).GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
                int result =  sqlSugar.Deleteable<T>().Where(condition).ExecuteCommandAsync().Result;
                //执行
                if (result > 0)
                {
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }
      

        /// <inheritdoc/>
        public override void Dispose()
        {
            OffAsync(true).Wait();
            base.Dispose();
        }

        /// <inheritdoc/>
        public override async Task DisposeAsync()
        {
            await OffAsync();
            await base.DisposeAsync();
        }


        public override OperateResult Exist<T>()
        {
            BegOperate();
            try
            {
                if (!(GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                if (sqlSugar.DbMaintenance.IsAnyTable(typeof(T).Name))
                {
                    return EndOperate(true, $"{typeof(T).Name} {LanguageOperate.GetLanguageValue("表已经存在")}", logOutput: false);
                }
                else
                {
                    return EndOperate(false, $"{typeof(T).Name} {LanguageOperate.GetLanguageValue("表不存在")}", logOutput: false);
                }
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

       
        
      

        /// <inheritdoc/>
        public override async Task<OperateResult> GetObjectAsync(CancellationToken token = default)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!(await GetStatusAsync(token)).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                return EndOperate(true, resultData: sqlSugar);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

        /// <inheritdoc/>
       

        public override OperateResult Insert<T>(T obj)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!(GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if (!(Exist<T>()).GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
                //判断键名是否存在
                //添加
                int result =  sqlSugar.Insertable(obj).ExecuteCommandAsync().Result;
                if (result > 0)
                {
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

        public override OperateResult Insert<T>(List<T> objs)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!( GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if (!( Exist<T>()).GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
                //添加
                int result = sqlSugar.Insertable(objs).ExecuteCommandAsync().Result;
                if (result > 0)
                {
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }




        public override OperateResult Query<T>(Expression<Func<T, bool>>? condition = null)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!( GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if (!( Exist<T>()).GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
                ISugarQueryable<T> queryable = sqlSugar.Queryable<T>();
                //添加条件
                if (condition != null)
                {
                    queryable.Where(condition);
                }
                //得到结果
                List<T> result = queryable.ToListAsync().Result;
                //执行
                if (result.Count > 0)
                {
                    return EndOperate(true, resultData: result);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

        /// <inheritdoc/>

        public override OperateResult Update<T>(T obj, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> condition=null)
        {
            BegOperate();
            try
            {
                //判断状态
                if (!(GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                //判断表是否存在
                if (!(Exist<T>()).GetDetails(out message))
                {
                    return EndOperate(false, message);
                }
              //  int result =  sqlSugar.Updateable(obj).UpdateColumns(updateColumns).Where(condition).ExecuteCommandAsync().Result;
                int result = sqlSugar.Updateable(obj).ExecuteCommand();
                //执行
                if (result > 0)
                {
                    return EndOperate(true);
                }
                return EndOperate(false);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }
      

        public override OperateResult On()
        {
            BegOperate();
            try
            {
                if ((GetStatus()).GetDetails(out string? message))
                {
                    return EndOperate(false, message);
                }
                ////创建文件夹
                //if (!System.IO.Directory.Exists(basics.DBFilePath))
                //{
                //    System.IO.Directory.CreateDirectory(basics.DBFilePath);
                //}
                basics.EnsureDbExistsAndMigrateIfNeeded();

                var cs = $"Data Source={basics.DBFullPath};";
                //sqlsugar 对象实例化
                sqlSugar = new SqlSugarClient(new ConnectionConfig
                {
                    DbType = DbType.Sqlite,
                    ConnectionString = cs,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = false
                });
                sqlSugar.Open();
                return EndOperate(true, LanguageOperate.GetLanguageValue("打开成功"));
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

        public override OperateResult Off(bool hardClose = false)
        {
            BegOperate();
            try
            {
                if (!hardClose)
                {
                    if (!(GetStatus()).GetDetails(out string? message))
                    {
                        return EndOperate(false, message);
                    }
                }
                //关闭sqlsugar
                sqlSugar.Close();
                sqlSugar.Dispose();
                sqlSugar = null;
                return EndOperate(true);
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

        public override OperateResult GetStatus()
        {
            BegOperate();
            try
            {
                if (sqlSugar == null)
                {
                    return EndOperate(false, LanguageOperate.GetLanguageValue("未连接"), logOutput: false);
                }
                else
                {
                    return EndOperate(true, LanguageOperate.GetLanguageValue("已连接"), logOutput: false);
                }
            }
            catch (Exception ex)
            {
                return EndOperate(false, ex.Message, ex);
            }
        }

       
    }
}
