using FuX.Model.data;
using FuX.Model.@interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.@interface
{
    /// <summary>
    /// 数据操作接口
    /// </summary>
    public interface IDB : IOn, IOff, IGetStatus, ILanguage, IDisposable 
    {
        /// <summary>
        /// 异步创建表
        /// </summary>
        /// <typeparam name="T">
        /// 表对象
        /// </typeparam>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> CreateAsync<T>(CancellationToken token = default) where T : class;


        /// <summary>
        /// 异步删除
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="condition">
        /// 条件<br/>
        /// <code>
        /// x => x.age > 18 &amp;&amp; x.name == "test"
        /// </code>
        /// </param>
        /// <param name="token">传播应取消操作的通知</param>
        /// <returns>统一操作结果</returns>
        Task<OperateResult> DeleteAsync<T>(Expression<Func<T, bool>> condition, CancellationToken token = default) where T : class, new();

        /// <summary>
        /// 异步判断表是否存在
        /// </summary>
        /// <typeparam name="T">
        /// 对象
        /// </typeparam>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> ExistAsync<T>(CancellationToken token = default) where T : class;

        /// <summary>
        /// 异步获取基础数据库操作对象 <br/> 当有复杂的查询可以直接拿到数据库对象进行操作
        /// </summary>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> GetObjectAsync(CancellationToken token = default);

        /// <summary>
        /// 异步获取状态
        /// </summary>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> GetStatusAsync(CancellationToken token = default);

        /// <summary>
        /// 异步添加
        /// </summary>
        /// <typeparam name="T">
        /// 指定的操作对象
        /// </typeparam>
        /// <param name="obj">
        /// 对象
        /// </param>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> InsertAsync<T>(T obj, CancellationToken token = default) where T : class, new();

        /// <summary>
        /// 异步添加集合
        /// </summary>
        /// <typeparam name="T">
        /// 指定的操作对象
        /// </typeparam>
        /// <param name="objs">
        /// 对象集合
        /// </param>
        /// <param name="token">
        /// 传播应取消操作的通知
        /// </param>
        /// <returns>
        /// 统一操作结果
        /// </returns>
        Task<OperateResult> InsertAsync<T>(List<T> objs, CancellationToken token = default) where T : class, new();

      

       

        /// <summary> 异步查询 </summary> 
        /// <typeparam name="T">指定的操作对象</typeparam> 
        /// <param name="condition">
        /// 条件<br/> 
        /// <code>
        /// x => x.age > 18 &amp;&amp; x.name == "test"
        /// </code> 
        /// </param>
        /// <param name="token">传播应取消操作的通知</param> 
        /// <returns>统一操作结果</returns>
        Task<OperateResult> QueryAsync<T>(Expression<Func<T, bool>>? condition = null, CancellationToken token = default) where T : class, new();

        /// <summary> 异步修改 </summary> 
        /// <typeparam name="T">指定的操作对象</typeparam> 
        /// <param name="obj">修改后的值</param> 
        /// <param name="updateColumns">
        /// 修改的列<br/>
        /// <code>
        /// u => new { u.Name, u.Description }
        /// </code>
        /// </param> 
        /// <param name="condition">
        /// 条件<br/> 
        /// <code>c=>c.id == 1</code> 
        /// </param>
        /// <param name="token">传播应取消操作的通知</param> 
        /// <returns>统一操作结果</returns>
        Task<OperateResult> UpdateAsync<T>(T obj, Expression<Func<T, object>> updateColumns, Expression<Func<T, bool>> condition, CancellationToken token = default) where T : class, new();
    }
}
