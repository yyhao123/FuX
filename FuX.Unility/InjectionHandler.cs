using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace FuX.Unility;

//
// 摘要:
//     注入处理，可继承
public class InjectionHandler
{
    //
    // 摘要:
    //     锁
    private static readonly object _lock;

    //
    // 摘要:
    //     注入服务收集器
    private static ServiceCollection? services;

    //
    // 摘要:
    //     注入服务提供器
    private static ServiceProvider? provider;

    //
    // 摘要:
    //     注入处理
    static InjectionHandler()
    {
        _lock = new object();
        Init();
    }

    //
    // 摘要:
    //     初始化
    //
    // 返回结果:
    //     是否成功
    private static bool Init()
    {
        try
        {
            if (services != null)
            {
                throw new Exception("已初始化");
            }

            services = new ServiceCollection();
            services.BuildServiceProvider();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("初始化异常：" + ex.Message, ex);
        }
    }

    //
    // 摘要:
    //     重建服务提供器
    private static void BuildProvider(ServiceCollection serviceDescriptors)
    {
        provider?.Dispose();
        provider = serviceDescriptors?.BuildServiceProvider();
    }

    //
    // 摘要:
    //     添加服务
    //     重复注册的话会覆盖之前的注册
    //
    // 参数:
    //   serviceCollection:
    //     服务收集器动作
    //     service=>
    //     {
    //     service.AddSingleton... （单例生命周期）只会创建一个实例，并且每次请求都会返回该实例
    //     service.AddTransient... （瞬态生命周期）每次请求都会创建新的实例
    //     }
    //
    // 返回结果:
    //     是否成功
    public static bool AddService(Action<IServiceCollection> serviceCollection)
    {
        lock (_lock)
        {
            try
            {
                if (services == null)
                {
                    throw new Exception("尚未初始化");
                }

                serviceCollection(services);
                BuildProvider(services);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("添加服务异常：" + ex.Message, ex);
            }
        }
    }

    //
    // 摘要:
    //     获取服务提供器
    //
    // 返回结果:
    //     服务提供器
    public static ServiceProvider? GetProvider()
    {
        return provider;
    }

    //
    // 摘要:
    //     判断服务是否存在
    //
    // 类型参数:
    //   T:
    //     服务对象
    //
    // 返回结果:
    //     true:存在
    //     false:不存在
    public static bool ExistService<T>() where T : class
    {
        try
        {
            if (services == null)
            {
                throw new Exception("尚未初始化");
            }

            return services.Any((ServiceDescriptor d) => d.ServiceType == typeof(T));
        }
        catch (Exception ex)
        {
            throw new Exception("判断服务是否存在异常：" + ex.Message, ex);
        }
    }

    //
    // 摘要:
    //     获取服务
    //
    // 类型参数:
    //   T:
    //     服务对象
    //
    // 返回结果:
    //     服务对象
    public static T GetService<T>() where T : class
    {
        try
        {
            if (provider == null)
            {
                throw new Exception("尚未初始化");
            }

            return provider.GetRequiredService<T>();
        }
        catch (Exception ex)
        {
            throw new Exception("获取服务异常：" + ex.Message, ex);
        }
    }

    //
    // 摘要:
    //     移除指定服务
    //
    // 返回结果:
    //     是否成功
    public static bool RemoveService<T>() where T : class
    {
        lock (_lock)
        {
            try
            {
                if (services == null)
                {
                    throw new Exception("尚未初始化");
                }

                ServiceDescriptor serviceDescriptor = services.FirstOrDefault((ServiceDescriptor d) => d.ServiceType == typeof(T));
                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                    BuildProvider(services);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("移除指定服务异常：" + ex.Message, ex);
            }
        }
    }

    //
    // 摘要:
    //     清除所有已注册服务
    //     如需再次使用，请先添加服务
    //
    // 返回结果:
    //     是否成功
    public static bool ClearService()
    {
        lock (_lock)
        {
            try
            {
                if (services == null)
                {
                    throw new Exception("尚未初始化");
                }

                provider?.Dispose();
                provider = null;
                services.Clear();
                services = null;
                Init();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("清除服务异常：" + ex.Message, ex);
            }
        }
    }
}