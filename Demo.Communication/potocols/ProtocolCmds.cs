using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Communication.potocols
{
    public static class ProtocolCmds
    {
        public const string DevName = "Mcon";
        public const string DevATP7330810 = "ATP7330-810";

        /// <summary>
        /// 获取设备信息【获取设备信息】
        /// </summary>
        public const byte Byte_0xFF = 0xFF;

        /// <summary>
        /// 设置设备型号【】
        /// </summary>
        public const byte Byte_0xFE = 0xFE;

        /// <summary>
        /// 设置出厂日期【】
        /// </summary>
        public const byte Byte_0xFC = 0xFC;

        /// <summary>
        /// 读取出厂日期【】
        /// </summary>
        public const byte Byte_0xFD = 0xFD;

        /// <summary>
        /// 光栅电机转动【00是正转  FF是反转】
        /// </summary>
        public const byte Byte_0x3E = 0x3E;

        /// <summary>
        /// 获取编码器值【】
        /// </summary>
        public const byte Byte_0xF0 = 0xF0;

        /// <summary>
        /// 设置光栅电机转动【编码器的最终位置值】
        /// </summary>
        public const byte Byte_0xF1 = 0xF1;

        /// <summary>
        /// 设定原点位置【0-N的原点值】
        /// </summary>
        public const byte Byte_0xF2 = 0xF2;

        /// <summary>
        /// 获取原点位置值【】
        /// </summary>
        public const byte Byte_0xF3 = 0xF3;

        /// <summary>
        /// 光栅电机持续运行【00正转 01反转】
        /// </summary>
        public const byte Byte_0xF4 = 0xF4;

        /// <summary>
        /// 光栅运行至原点【0-N的原点值】
        /// </summary>
        public const byte Byte_0xF5 = 0xF5;

        /// <summary>
        /// 获取光栅状态【】
        /// </summary>
        public const byte Byte_0xB0 = 0xB0;

        /// <summary>
        /// 将当前位置作为原点偏移位置【0-N的原点值】
        /// </summary>
        public const byte Byte_0xF6 = 0xF6;

        /// <summary>
        /// 设置斩波轮开始位置
        /// </summary>
        public const byte Byte_0x53 = 0x53;

        /// <summary>
        /// 获取斩波轮开始位置
        /// </summary>
        public const byte Byte_0x54 = 0x54;

        /// <summary>
        /// 设置斩波轮角度
        /// </summary>
        public const byte Byte_0x55 = 0x55;

        /// <summary>
        /// 获取斩波轮角度
        /// </summary>
        public const byte Byte_0x56 = 0x56;

        /// <summary>
        /// 重置偏移位置【0-N的原点值】
        /// </summary>
        public const byte Byte_0xF7 = 0xF7;

        /// <summary>
        /// 获取光栅数量【】
        /// </summary>
        public const byte Byte_0x06 = 0x06;

        /// <summary>
        /// 1号狭缝电机设置脉冲值【00是正转  FF是反转】
        /// </summary>
        public const byte Byte_0x10 = 0x10;

        /// <summary>
        /// 2号狭缝电机设置脉冲值【00是正转  FF是反转】
        /// </summary>
        public const byte Byte_0x11 = 0x11;

        /// <summary>
        /// 开关状态指示灯【00打开 01关闭】
        /// </summary>
        public const byte Byte_0x2A = 0x2A;

        /// <summary>
        /// 记录反光镜入光口位置【】
        /// </summary>
        public const byte Byte_0x26 = 0x26;

        /// <summary>
        /// 设置反光镜入光口位置【00回零 01到记录位置】
        /// </summary>
        public const byte Byte_0x27 = 0x27;

        /// <summary>
        /// 设置反光镜入光口脉冲【00是正转  FF是反转】
        /// </summary>
        public const byte Byte_0x28 = 0x28;

        /// <summary>
        /// 获取反光镜入光口状态【】
        /// </summary>
        public const byte Byte_0x29 = 0x29;

        /// <summary>
        /// 记录反光镜出光口位置【】
        /// </summary>
        public const byte Byte_0x36 = 0x36;

        /// <summary>
        /// 设置反光镜出光口位置【】
        /// </summary>
        public const byte Byte_0x37 = 0x37;

        /// <summary>
        /// 设置反光镜出光口脉冲【】
        /// </summary>
        public const byte Byte_0x38 = 0x38;

        /// <summary>
        /// 获取反光镜出光口状态【】
        /// </summary>
        public const byte Byte_0x39 = 0x39;

        /// <summary>
        /// 设置波数转像素系数【第一个字节0-N的原点值】
        /// </summary>
        public const byte Byte_0xB5 = 0xB5;

        /// <summary>
        /// 读取波数转像素系数【0-N的原点值】
        /// </summary>
        public const byte Byte_0xB6 = 0xB6;

        /// <summary>
        /// 从下位机获取上次保存系数【None】【String】【】
        /// </summary>
        public const byte GET_COF_PX = 0xB6;

        /// <summary>
        /// 设置像素转脉冲系数【第一个字节0-N的原点值】
        /// </summary>
        public const byte Byte_0xB7 = 0xB7;

        /// <summary>
        /// 读取像素转脉冲系数【0-N的原点值】
        /// </summary>
        public const byte Byte_0xB8 = 0xB8;

        /// <summary>
        /// 设置激光波长系数【（最多20组）】
        /// </summary>
        public const byte Byte_0xB9 = 0xB9;

        /// <summary>
        /// 读取激光波长系数【】
        /// </summary>
        public const byte Byte_0xBA = 0xBA;

        /// <summary>
        /// 获取PN【】
        /// </summary>
        public const byte Byte_0x03 = 0x03;

        /// <summary>
        /// 获取SN【】
        /// </summary>
        public const byte Byte_0xFB = 0xFB;

        /// <summary>
        /// 写入SN【】
        /// </summary>
        public const byte Byte_0xFA = 0xFA;

        /// <summary>
        /// 设置X轴电机步数【00是正转（后）  FF是反转（前）】
        /// </summary>
        public const byte Byte_0x2E = 0x2E;

        /// <summary>
        /// 设置Y轴电机步数【00是正转(左)  FF是反转（右）】
        /// </summary>
        public const byte Byte_0x2F = 0x2F;

        /// <summary>
        /// 设置Z轴电机步数【00正转(下) ff反转（上）】
        /// </summary>
        public const byte Byte_0x2B = 0x2B;

        /// <summary>
        /// 获取载物台电机状态【】
        /// </summary>
        public const byte Byte_0x2C = 0x2C;

        /// <summary>
        /// 设置SP分光片脉冲【00是正转  FF是反转】
        /// </summary>
        public const byte Byte_0x12 = 0x12;

        /// <summary>
        /// 记录SP位置【01记录激光 02记录白光】
        /// </summary>
        public const byte Byte_0x13 = 0x13;

        /// <summary>
        /// 设置SP位置【00回原点 01到激光 02到白光】
        /// </summary>
        public const byte Byte_0x14 = 0x14;

        /// <summary>
        /// 获取SP状态【】
        /// </summary>
        public const byte Byte_0x15 = 0x15;

        /// <summary>
        /// 透传控制激光器【】
        /// </summary>
        public const byte Byte_0x00 = 0x00;

        /// <summary>
        /// 显微镜照明灯设置亮度【0-100值设置】
        /// </summary>
        public const byte Byte_0x16 = 0x16;

        /// <summary>
        /// 紧急停止载物台电机【】
        /// </summary>
        public const byte Byte_0x01 = 0x01;

        /// <summary>
        /// 开启激光功率【】
        /// </summary>
        public const byte Byte_0x17 = 0x17;

        /// <summary>
        /// 设定固定原点定标位置【第一个数值为原点】
        /// </summary>
        public const byte Byte_0xF8 = 0xF8;

        /// <summary>
        /// 设置设备对应关系【532,600,1;532,1800,2】
        /// </summary>
        public const byte Byte_0xD1 = 0xD1;

        /// <summary>
        /// 获取设备对应关系【】
        /// </summary>
        public const string GDEV_I = "GDEV_I";//0xD0;

        /// <summary>
        /// 卤素灯开光【00开启 01关闭】
        /// </summary>
        public const byte Byte_0x18 = 0x18;

        /// <summary>
        /// 获取光栅原点【Int】【Int】【第1个字节表示哪片光栅 01:第一片 02:第二片】
        /// </summary>
        public const byte GS_GET_P = 0xF9;

        /// <summary>
        /// 获取激光器与硬件串口绑定状态【例:0x34则638nm激光器绑定1号串口通道，785激光器绑定2号串口通道】
        /// </summary>
        public const byte Byte_0x24 = 0x24;

        /// <summary>
        /// 设置激光器与硬件串口绑定【第一个字节表示的是哪个激光器：0x01：532激光器 0x02：633激光器 0x03:   638激光器 0x04：785激光器 0x05：830激光器 0x06:  1064激光器 第二个字节表示绑定的是哪个串口通道：0x01：串口1通道 0x02：串口2通道】
        /// </summary>
        public const byte Byte_0x25 = 0x25;

        /// <summary>
        /// 写入激光参数【】
        /// </summary>
        public const byte Byte_0xBC = 0xBC;

        /// <summary>
        /// 读取激光参数【】
        /// </summary>
        public const byte Byte_0xBD = 0xBD;

        /// <summary>
        /// 获取光栅转动进程百分比【0x00~0x64(0%~100%)】
        /// </summary>
        public const byte Byte_0x3F = 0x3F;

        /// <summary>
        /// 设置激光DAC绑定【】
        /// </summary>
        public const byte Byte_0xBE = 0xBE;

        /// <summary>
        /// 获取激光DAC绑定【】
        /// </summary>
        public const byte Byte_0xBF = 0xBF;

        /// <summary>
        /// 设置光栅回零【（单元探测器用）0x01:光栅1回零 0x02:光栅2回零 0x03:光栅3回零】
        /// </summary>
        public const byte Byte_0x40 = 0x40;

        /// <summary>
        /// 写入相机参数
        /// </summary>
        public const byte Byte_0x41 = 0x41;

        /// <summary>
        /// 获取相机参数
        /// </summary>
        public const byte Byte_0x42 = 0x42;

        /// <summary>
        /// 记录光栅终点位置【0x01：光栅1.....Byte2-byte5：光栅原点数据】
        /// </summary>
        public const byte Byte_0x45 = 0x45;

        /// <summary>
        /// 获取光栅电机状态【第1Byte第0位表示GR电机状态；状态为0表示电机停止，状态为1表示电机运行中。】
        /// </summary>
        public const byte Byte_0x46 = 0x46;

        /// <summary>
        /// 设置定点采样采样点间隔时间【】
        /// </summary>
        public const byte Byte_0x4F = 0x4F;

        /// <summary>
        /// 设置采集步进值【】
        /// </summary>
        public const byte Byte_0x47 = 0x47;

        /// <summary>
        /// 获取光栅终点位置【Int】【Int】【指定光栅 1和2】
        /// </summary>
        public const byte GS_GET_E = 0x48;

        /// <summary>
        /// 停止采集【】
        /// </summary>
        public const byte Byte_0x49 = 0x49;

        /// <summary>
        /// 选择范围单次采集【Byte1-byte4：起始位置 Byte5-byte8：终止位置 Byte9：采集范围 0x00：选择范围采集 0x01：全范围采集】
        /// </summary>
        public const string RANGE_COL = "RANGE_COL"; //0x4a

        /// <summary>
        /// 定点采集【Byte1-byte4：采集位置   Byte5-byte8：采集时间】
        /// </summary>
        public const byte Byte_0x4B = 0x4B;

        /// <summary>
        /// 检测光栅零点【】
        /// </summary>
        public const byte Byte_0x4E = 0x4E;

        /// <summary>
        /// 写入光栅间隔数据【】
        /// </summary>
        public const byte Byte_0x51 = 0x51;

        /// <summary>
        /// 读取光栅间隔数据【】
        /// </summary>
        public const byte Byte_0x52 = 0x52;

        /// <summary>
        /// 设置光栅数【】
        /// </summary>
        public const byte Byte_0x4C = 0x4C;

        /// <summary>
        /// 获取光栅数【】
        /// </summary>
        public const byte Byte_0x4D = 0x4D;

        /// <summary>
        /// 设置CCD放大增益【第1个字节表示几号CCD（最多2个）  第2个字节表示第几档增益（1-5档）】
        /// </summary>
        public const byte Byte_0x50 = 0x50;

        /// <summary>
        /// 记录光栅终点 ATP7100
        /// </summary>
        public const byte Byte_0x86 = 0x86;


        /// <summary>
        /// 获取光栅终点 ATP7100
        /// </summary>
        public const byte Byte_0x87 = 0x87;

        /// <summary>
        /// 获取聚焦镜状态
        /// </summary>
        public const byte Get_FocusState = 0x1D;

        /// <summary>
        /// 记录聚焦镜位置
        /// </summary>
        public const byte RecordFocus = 0x1A;

        /// <summary>
        /// 设置到聚焦镜位置
        /// </summary>
        public const byte Set_FocusPoint = 0x1B;

        /// <summary>
        /// 设置聚焦镜步数
        /// </summary>
        public const byte Set_FocusPulse = 0x1C;

        /// <summary>
        /// 设置采集精细度【Int】【Bool】【01~03 1~3档】
        /// </summary>
        public const string Accuracy_Set = "Accuracy_Set";//0x22

        /// <summary>
        /// 设置放大增益
        /// </summary>
        public const string Gear_Set = "Gear_Set";//0x20

        /// <summary>
        /// 电机使用状态的存储
        /// </summary>
        public const string Motor_Sta = "Motor_Sta";

        /// <summary>
        /// 转动电机位置
        /// </summary>
        public const string RUN = "RUN";

        /// <summary>
        /// 转绝对位置
        /// </summary>
        public const string RUNJ = "RUNJ";

        /// <summary>
        /// 转到指定的点位
        /// </summary>
        public const string TO = "TO";

        /// <summary>
        ///  获取原点值
        /// </summary>
        public const string GETP = "GETP";

        /// <summary>
        /// 获取当前编码器值
        /// </summary>
        public const string GETA = "GETA";

        /// <summary>
        /// 判断电机状态
        /// </summary>
        public const string STA_ALL = "STA_ALL";

        /// <summary>
        /// 判读电机百分比
        /// </summary>
        public const string RATE_GS = "RATE_GS";

        /// <summary>
        /// 写入标定系数【String】【Bool】【】
        /// </summary>
        public const byte SET_COF_PX = 0xB5;

        /// <summary>
        /// 写入标定系数2【String】【Bool】【】
        /// </summary>
        public const byte SET_COF_PX2 = 0x57;

        /// <summary>
        /// 设置光栅原点【Int】【Bool】【第1个字节表示哪片光栅 01:第一片 02:第二片 第2~5字节表示原点的编码】
        /// </summary>
        public const byte GS_SET_P = 0xF8;

        /// <summary>
        /// 记录光栅终点位置【Int】【Bool】【0x01：表示第一片 0x02：表示第二片】
        /// </summary
        public const string GS_SET_E = "GS_SET_E";

        /// <summary>
        /// 设置光栅原点【Int】【Bool】【第1个字节表示哪片光栅 01:第一片 02:第二片 第2~5字节表示原点的编码】
        /// </summary>
        public const byte GS_SET_EV = 0x0c;

        /// <summary>
        /// 获取光栅切换位置【None】【Int】
        /// </summary>
        public const byte GET_GratingAddressSwitch = 0x0A;

        /// <summary>
        /// 设置光栅电机旋转【Int】【Motor】【第一个字节代表方向 00是顺时针 FF是逆时针 第2~5字节表示脉冲数】
        /// </summary>
        public const byte GS_RUN = 0x3E;

        /// <summary>
        /// 获取电机状态【Int】【Int】【光栅电机状态位:0x01 滤光片电机状态位:0x02】
        /// </summary>
        public const byte MAC_GET_S = 0x0B;

        /// <summary>
        /// 获取编码器的值【None】【Int】【发送一次返回一个当前位置编码器值】
        /// </summary>
        public const byte GET_ADDRESS = 0xF0;

        /// <summary>
        /// 设置光栅回零【Int】【Bool】【】
        /// </summary>
        public const byte GS_RUN_P = 0x40;

        /// <summary>
        /// 紧急停止电机【None】【Bool】【】
        /// </summary>
        public const byte Stop_Mac = 0x01;

        /// <summary>
        /// 定标模拟采集【None】【Bool】【】
        /// </summary>
        public const byte Byte_COL_360 = 0x0d;

        /// <summary>
        /// 获取相机参数【None】【String】【】
        /// </summary>
        public const byte GET_CCD_CHANGE = 0x42;

        /// <summary>
        /// 停止采集【None】【Bool】【】
        /// </summary>
        public const byte STOP_GS_RUN = 0x49;

        /// <summary>
        /// 设置光栅切换位置【Int】【Int】【】
        /// </summary>
        public const byte SET_GratingAddressSwitch = 0x08;

        /// <summary>
        /// 定点采集【Int】【Bool】【相对位置(1~4byte)】
        /// </summary>
        public const byte PONIT_COL = 0x4B;

        /// <summary>
        /// 获取温度【Int】【Int】【0x01 ：CCD温度ADC值 0x02 ：设备外壳温度ADC值】
        /// </summary>
        public const byte Get_Temp = 0x23;

        /// <summary>
        /// 获取最大积分时间【None】【Int】【】
        /// </summary>
        public const string Get_CCD_IntegralTime_MAX_ATDCCD = "Get_CCD_IntegralTime_MAX_ATDCCD";

        /// <summary>
        /// 获取最小积分时间【None】【Int】【】
        /// </summary>
        public const string Get_CCD_IntegralTime_MIN_ATDCCD = "Get_CCD_IntegralTime_MIN_ATDCCD";

        /// <summary>
        /// 设置积分时间【Int】【Int】【】
        /// </summary>
        public const string Cmd_Set_CCD_IntegralTime = "Cmd_Set_CCD_IntegralTime";
    }
}
