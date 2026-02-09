using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.Specenum
{
    //
    // 摘要:
    //     地址类型
    public enum AddressType
    {
        //
        // 摘要:
        //     实际地址
        [Description("实际地址")]
        Reality,
        //
        // 摘要:
        //     虚拟静态地址
        [Description("虚拟静态地址")]
        VirtualStatic,
        //
        // 摘要:
        //     虚拟动态随机变化地址；
        //     地址后添加格式；
        //     {100}
        //     {更新间隔}
        [Description("虚拟动态随机变化地址")]
        VirtualDynamic_Random,
        //
        // 摘要:
        //     虚拟动态随机范围变化地址；
        //     地址后添加格式；
        //     {100,1^100}
        //     {100,1.1^9.9}
        //     {500,00:00:00^12:00:00}
        //     {500,2000-01-01^2024-01-01}
        //     {更新间隔,最小值^最大值}
        [Description("虚拟动态随机范围变化地址")]
        VirtualDynamic_RandomScope,
        //
        // 摘要:
        //     虚拟动态顺序变化地址；
        //     地址后添加格式；
        //     {100,1}
        //     {更新间隔,增长比例}
        [Description("虚拟动态顺序变化地址")]
        VirtualDynamic_Order,
        //
        // 摘要:
        //     虚拟动态顺序范围变化地址；
        //     地址后添加格式；
        //     {100,1,1^100}
        //     {100,0.1,1.1^9.9}
        //     {500,1,00:00:00^12:00:00}
        //     {500,1,2000-01-01^2024-01-01}
        //     {更新间隔,增长比例,最小值^最大值}
        [Description("虚拟动态顺序范围变化地址")]
        VirtualDynamic_OrderScope
    }
}
