using FuX.Model.@interface;
using FuX.Core.extend;
using FuX.Model.data;
using NetTaste;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Core.@abstract
{
    /// <summary>
    /// 数据库抽象类；<br/> 
    /// 外部必须实现抽象方法；<br/> 
    /// 实现接口的异步方法 
    /// <typeparam name="O">操作类</typeparam>
    /// <typeparam name="D">基础数据类，构造参数类</typeparam>
    /// </summary>
    public abstract class DBAbstract<O, D> : CoreUnify<O, D>, IDB
       where O : class
       where D : class
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DBAbstract() : base() { }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="param"></param>
        public DBAbstract(D param) : base(param) { }



        /// <inheritdoc/>
        public override void Dispose()
        {
            Off(true);
            base.Dispose();
        }

        /// <inheritdoc/>
        public override async Task DisposeAsync()
        {
            await OffAsync(true);
            await base.DisposeAsync();
        }

        public abstract OperateResult Creat<T>();
        public async Task<OperateResult> CreateAsync<T>(CancellationToken token = default) where T : class 
          => await Task.Run(() => Creat<T>(), token);
        
        public abstract OperateResult Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();

        public async Task<OperateResult> DeleteAsync<T>(Expression<Func<T, bool>> condition, CancellationToken token = default) where T : class, new()
          => await Task.Run(() => Delete<T>(condition), token);

        public abstract OperateResult Exist<T>();

        public async Task<OperateResult> ExistAsync<T>(CancellationToken token = default) where T : class
            => await Task.Run(() => Exist<T>(), token);

        


        public abstract Task<OperateResult> GetObjectAsync(CancellationToken token = default);


        public abstract OperateResult GetStatus();
        

        public async Task<OperateResult> GetStatusAsync(CancellationToken token = default)
        => await Task.Run(() => GetStatus(), token);

        public abstract OperateResult Insert<T>(T obj) where T : class, new();
        public async Task<OperateResult> InsertAsync<T>(T obj, CancellationToken token = default) where T : class, new()
        => await Task.Run(()=>Insert<T>(obj), token);

        public abstract OperateResult Insert<T>(List<T> objs);
        public async Task<OperateResult> InsertAsync<T>(List<T> objs, CancellationToken token = default) where T : class, new()
         => await Task.Run(() => Insert<T>(objs), token);

        public abstract OperateResult Off(bool hardClose = false);
        

        public async Task<OperateResult> OffAsync(bool hardClose = false, CancellationToken token = default)
        => await Task.Run(()=>Off(hardClose), token);

        public abstract OperateResult On();
       
        public async Task<OperateResult> OnAsync(CancellationToken token = default)
        => await Task.Run(()=>On(), token);


        public abstract OperateResult Query<T>(Expression<Func<T, bool>>? condition = null);
        public async Task<OperateResult> QueryAsync<T>(Expression<Func<T, bool>>? condition = null, CancellationToken token = default) where T : class, new()
        => await Task.Run(() => Query<T>(condition), token);

        public abstract OperateResult Update<T>(T obj, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> condition) where T : class, new();

        public async Task<OperateResult> UpdateAsync<T>(T obj, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> condition, CancellationToken token = default) where T : class, new()
        =>await Task.Run(()=>Update<T>(obj, updateColumns, condition), token);
    }

}
