using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Communication.constant
{
    public static class LocalizeConstant
    {
        /// <summary>
        /// 提示
        /// </summary>
        public const string INFO = "INFO";

        /// <summary>
        /// 没有软件日志。
        /// </summary>
        public const string ABOUT_NO_SOFTWARE_LOG = "ABOUT_NO_SOFTWARE_LOG";

        /// <summary>
        /// 未设置
        /// </summary>
        public const string Not_Set_Val = "Not_Set_Val";

        /// <summary>
        /// 强度/Count
        /// </summary>
        public const string INTENSITY_YAXIS_TITLE = "INTENSITY_YAXIS_TITLE";

        /// <summary>
        /// 波数/cm-1
        /// </summary>
        public const string RAMAN_SHIFT_XAXIS_TITLE = "RAMAN_SHIFT_XAXIS_TITLE";

        /// <summary>
        /// 像素
        /// </summary>
        public const string PIXEL_XAXIS_TITLE = "PIXEL_XAXIS_TITLE";

        /// <summary>
        /// 波长/nm
        /// </summary>
        public const string WEL_XAXIS_TITLE = "WEL_XAXIS_TITLE";

        /// <summary>
        /// 时间/s
        /// </summary>
        public const string TIME_XAXIS_TITLE = "TIME_XAXIS_TITLE";

        /// <summary>
        /// 反射率/%
        /// </summary>
        public const string REFLECTIVITY_YAXIS_TITLE = "REFLECTIVITY_YAXIS_TITLE";

        /// <summary>
        /// 辐照度/(mW/cm2/nm)
        /// </summary>
        public const string RADIANCE_YAXIS_TITLE = "RADIANCE_YAXIS_TITLE";

        /// <summary>
        /// 透射率/%
        /// </summary>
        public const string TRANSMITTANCE_YAXIS_TITLE = "TRANSMITTANCE_YAXIS_TITLE";

        /// <summary>
        /// 吸光度/Abs
        /// </summary>
        public const string ABSORBANCE_YAXIS_TITLE = "ABSORBANCE_YAXIS_TITLE";

        /// <summary>
        /// 操作成功！
        /// </summary>
        public const string OPERATE_SUCCESS = "OPERATE_SUCCESS";

        /// <summary>
        /// 操作失败！
        /// </summary>
        public const string OPERATE_FAIL = "OPERATE_FAIL";

        /// <summary>
        /// 系统异常：{0}
        /// </summary>
        public const string SYSTEM_EXCEPTION = "SYSTEM_EXCEPTION";

        /// <summary>
        /// 操作取消！
        /// </summary>
        public const string OPERATE_CANCELED = "OPERATE_CANCELED";

        /// <summary>
        /// 设置完成，将为您重新打开程序。
        /// </summary>
        public const string SWITCH_LANGUAGE_ZH = "SWITCH_LANGUAGE_ZH";

        /// <summary>
        /// 语言切换完成。软件将重新启动!
        /// </summary>
        public const string SWITCH_LANGUAGE_EN = "SWITCH_LANGUAGE_EN";

        /// <summary>
        /// 设备连接断开！
        /// </summary>
        public const string DEVICE_DISCONNECTED = "DEVICE_DISCONNECTED";

        /// <summary>
        /// 不正确的输入值！
        /// </summary>
        public const string INVALID_INPUT_VALUE = "INVALID_INPUT_VALUE";

        /// <summary>
        /// 空闲
        /// </summary>
        public const string DEVICE_STATUS = "DEVICE_STATUS_";

        /// <summary>
        /// 任务取消成功！
        /// </summary>
        public const string TASK_CANCEL_SUCCESS = "TASK_CANCEL_SUCCESS";

        /// <summary>
        /// 采集拉曼光谱失败。
        /// </summary>
        public const string DEVICE_ACQUIRE_FAIL = "DEVICE_ACQUIRE_FAIL";

        /// <summary>
        /// 验证密码
        /// </summary>
        public const string CONFIRM_PASSWORD = "CONFIRM_PASSWORD";

        /// <summary>
        /// 1. 此设置会影响仪器波数准确性，请谨慎使用！{Environment.NewLine}2. 校准前请准备乙腈；{Environment.NewLine}3. 密码在帮助文档内；
        /// </summary>
        public const string RAMAN_SHIFT_CALIBRATION_CONFIRM_MSG = "RAMAN_SHIFT_CALIBRATION_CONFIRM_MSG";

        /// <summary>
        /// 此设置会影响载物台相关功能，请谨慎使用！请连续我们获取密码。
        /// </summary>
        public const string MODEL_SETTING_CONFIRM_MSG = "MODEL_SETTING_CONFIRM_MSG";

        /// <summary>
        /// 不正确的密码。
        /// </summary>
        public const string CONFIRM_PWD_FAIL = "CONFIRM_PWD_FAIL";

        /// <summary>
        /// 光谱列表
        /// </summary>
        public const string SPECTRUM_TREE_ROOT_TITLE = "光谱列表";

        /// <summary>
        /// 创建日期
        /// </summary>
        public const string SPECTRUM_CREATED = "SPECTRUM_CREATED";

        /// <summary>
        /// 谱图曲线数超过范围{0}，您可以重新设置。
        /// </summary>
        public const string SPECTRUM_CURVE_NUMBER_EXCLUSIVE = "SPECTRUM_CURVE_NUMBER_EXCLUSIVE";

        /// <summary>
        /// 未找到谱图数据
        /// </summary>
        public const string SPECTRUM_NOT_FOUND = "SPECTRUM_NOT_FOUND";

        /// <summary>
        /// 单次采集
        /// </summary>
        public const string SPECTRUM_ACQUIRETYPE = "SPECTRUM_ACQUIRETYPE_";

        /// <summary>
        /// 快速采集
        /// </summary>
        public const string SPECTRUM_ACQUIREMETHOD = "SPECTRUM_ACQUIREMETHOD_";

        /// <summary>
        /// 光谱
        /// </summary>
        public const string DISPLAYDATATYPE = "DISPLAYDATATYPE_";

        /// <summary>
        /// 没有更多数据
        /// </summary>
        public const string INDEX_QUERY_NO_MORE_DATA = "INDEX_QUERY_NO_MORE_DATA";

        /// <summary>
        /// 是否删除谱图记录？
        /// </summary>
        public const string DELETE_SPECTRUM_CONFIRM = "DELETE_SPECTRUM_CONFIRM";

        /// <summary>
        /// 快速采集需要先采集暗底。
        /// </summary>
        public const string QUICK_ACQUIRE_NO_DARK = "QUICK_ACQUIRE_NO_DARK";

        /// <summary>
        /// 是否确认清除谱图？
        /// </summary>
        public const string CLEAR_SPECTRUM_LIST = "CLEAR_SPECTRUM_LIST";

        /// <summary>
        /// 是否确认清除固定谱图？
        /// </summary>
        public const string CLEAR_SPECTRUM_PINED_LIST = "CLEAR_SPECTRUM_PINED_LIST";

        /// <summary>
        /// 谱图名称为空。
        /// </summary>
        public const string SPECTRUM_NAME_IS_NULL = "SPECTRUM_NAME_IS_NULL";

        /// <summary>
        /// 没有选择数据
        /// </summary>
        public const string NO_DATA_SELECTED = "NO_DATA_SELECTED";

        /// <summary>
        /// 导出记录{0}文件名不正确。
        /// </summary>
        public const string EXPORT_SPECTRUM_INVALID_FILENAME = "EXPORT_SPECTRUM_INVALID_FILENAME";

        /// <summary>
        /// 保存文件夹为空。
        /// </summary>
        public const string EXPORT_SPECTRUM_DIRECTORY_EMPTY = "EXPORT_SPECTRUM_DIRECTORY_EMPTY";

        /// <summary>
        /// None
        /// </summary>
        public const string SPECTRUM_PRETREAT = "SPECTRUM_PRETREAT_";

        /// <summary>
        /// 荧光漂白采集方法-系统默认。
        /// </summary>
        public const string ACQUIRE_METHOD_FLUORESCENCE_QUENCHING = "ACQUIRE_METHOD_FLUORESCENCE_QUENCHING";

        /// <summary>
        /// 是否删除选中的采集方法。
        /// </summary>
        public const string ACQUIRE_METHOD_DELETE_PROMPT = "ACQUIRE_METHOD_DELETE_PROMPT";

        /// <summary>
        /// manual_zh.pdf
        /// </summary>
        public const string USER_MANUAL_PATH = @".\Resource\manual.pdf";

        /// <summary>
        /// 是否下载定标数据？
        /// </summary>
        public const string DEVICE_RAMANSHIFT_CALIBRATION_CONFIRM = "DEVICE_RAMANSHIFT_CALIBRATION_CONFIRM";

        /// <summary>
        /// 修改
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_EDIT = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_EDIT";

        /// <summary>
        /// 详情
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_DETAIL = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_DETAIL";

        /// <summary>
        /// 清除所有
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_CLEARALL = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_CLEARALL";

        /// <summary>
        /// 清除固定
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_CLEAR_PINED = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_CLEAR_PINED";

        /// <summary>
        /// 固定
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_PIN = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_PIN";

        /// <summary>
        /// 移除固定
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_UNPIN = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_UNPIN";

        /// <summary>
        /// 移除
        /// </summary>
        public const string SPECTRUM_LIST_CONTEXT_MENU_ITEM_REMOVE = "SPECTRUM_LIST_CONTEXT_MENU_ITEM_REMOVE";

        /// <summary>
        /// 定点
        /// </summary>
        public const string COLLECTTYPE = "COLLECTTYPE_";

        /// <summary>
        /// 版本提示
        /// </summary>
        public const string VERSION_TIPS = "VERSION_TIPS";

        /// <summary>
        /// 发现了新版本，是否更新？
        /// </summary>
        public const string VERSION_NEW = "VERSION_NEW";

        /// <summary>
        /// 补丁更新提示
        /// </summary>
        public const string PATCH_TIPS = "PATCH_TIPS";

        /// <summary>
        /// 发现了新补丁，是否更新？
        /// </summary>
        public const string PATCH_NEW = "PATCH_NEW";

        /// <summary>
        /// 采集完成
        /// </summary>
        public const string COLLECTION_COMPLETED = "COLLECTION_COMPLETED";

        /// <summary>
        /// 参数设置中
        /// </summary>
        public const string PARAMETER_SETTING = "PARAMETER_SETTING";

        /// <summary>
        /// 请先设置参数
        /// </summary>
        public const string PARAMETER_JUDGMENT = "PARAMETER_JUDGMENT";

        /// <summary>
        /// 采集失败，采集类型发生了改变
        /// </summary>
        public const string COLLECTION_FAILED_TYPE = "COLLECTION_FAILED_TYPE";

        /// <summary>
        /// 采集失败，光栅发生了变更
        /// </summary>
        public const string COLLECTION_FAILED_GRATING = "COLLECTION_FAILED_GRATING";

        /// <summary>
        /// 采集失败，采集频率发生了变更
        /// </summary>
        public const string COLLECTION_FAILED_FREQUENCY = "COLLECTION_FAILED_FREQUENCY";

        /// <summary>
        /// 采集失败，范围类型发生了变更
        /// </summary>
        public const string COLLECTION_FAILED_SCOPETYPE = "COLLECTION_FAILED_SCOPETYPE";

        /// <summary>
        /// 采集失败，范围数值发生了变更
        /// </summary>
        public const string COLLECTION_FAILED_SCOPENUM = "COLLECTION_FAILED_SCOPENUM";

        /// <summary>
        /// 采集初始化失败
        /// </summary>
        public const string COLLECTION_FAILED_INIT = "COLLECTION_FAILED_INIT";

        /// <summary>
        /// 数据采集中
        /// </summary>
        public const string COLLECTION_DATAING = "COLLECTION_DATAING";

        /// <summary>
        /// 请选择采集模式
        /// </summary>
        public const string SELECT_COLLECTION_MODE = "SELECT_COLLECTION_MODE";

        /// <summary>
        /// 谱图【" + node.Name + "】未寻到峰
        /// </summary>
        public const string SELECT_PEAK_NONE = "SELECT_PEAK_NONE";

        /// <summary>
        /// 找不到获取模块
        /// </summary>
        public const string MODULE_NOT_FOUND = "MODULE_NOT_FOUND";

        /// <summary>
        /// 此设置会影响设备功能，请谨慎使用！如需要请联系我们获取密码
        /// </summary>
        public const string PASSWORD_TIP = "PASSWORD_TIP";

        /// <summary>
        /// 未连接相机
        /// </summary>
        public const string CCD_NOT_CONNECTED = "CCD_NOT_CONNECTED";

        /// <summary>
        /// 采集模式
        /// </summary>
        public const string COLLECTION_MODEL = "COLLECTION_MODEL";

        /// <summary>
        /// 定点
        /// </summary>
        public const string COLLECTION_MPOINT = "COLLECTION_MPOINT";

        /// <summary>
        /// 范围
        /// </summary>
        public const string COLLECTION_MRANGE = "COLLECTION_MRANGE";

        /// <summary>
        /// 未采集暗背景，请采集暗背景后再操作
        /// </summary>
        public const string DARK_BACKGROUND_DATANONE = "DARK_BACKGROUND_DATANONE";

        /// <summary>
        /// 未采集参考数据，请采集参考数据后再操作
        /// </summary>
        public const string REFERENCE_DATANONE = "REFERENCE_DATANONE";

        /// <summary>
        /// 请选择处理模式
        /// </summary>
        public const string DEAL_WITH_MODEL = "DEAL_WITH_MODEL";

        /// <summary>
        /// 操作失败！请选择当前采集模式下的数据
        /// </summary>
        public const string OPERATION_FAILED_COLMODEL = "OPERATION_FAILED_COLMODEL";

        /// <summary>
        /// 操作失败！请选择相同类型光谱数据
        /// </summary>
        public const string OPERATION_FAILED_SPECTRUM = "OPERATION_FAILED_SPECTRUM";

        /// <summary>
        /// 操作失败！请选择相同采集类型的数据
        /// </summary>
        public const string OPERATION_FAILED_TYPE = "OPERATION_FAILED_TYPE";

        /// <summary>
        /// 查看半高宽
        /// </summary>
        public const string ZED_MENU_SELECT_HW = "ZED_MENU_SELECT_HW";

        /// <summary>
        /// 添加文字
        /// </summary>
        public const string ZED_MENU_SELECT_ADDTEXT = "ZED_MENU_SELECT_ADDTEXT";

        /// <summary>
        /// 撤销文字输入
        /// </summary>
        public const string ZED_MENU_SELECT_DELTEXT = "ZED_MENU_SELECT_DELTEXT";

        /// <summary>
        /// 原始叠加
        /// </summary>
        public const string ZED_RMENU_ORIGINAL_OVERLAY = "ZED_RMENU_ORIGINAL_OVERLAY";

        /// <summary>
        /// XY平移
        /// </summary>
        public const string ZED_RMENU_XYPAN = "ZED_RMENU_XYPAN";

        /// <summary>
        /// Y波峰平移
        /// </summary>
        public const string ZED_RMENU_YPEAKPAN = "ZED_RMENU_YPEAKPAN";

        /// <summary>
        /// Y轴平移
        /// </summary>
        public const string ZED_RMENU_YPAN = "ZED_RMENU_YPAN";

        /// <summary>
        /// 手动移动谱线
        /// </summary>
        public const string ZED_RMENU_MANUALPAN = "ZED_RMENU_MANUALPAN";

        /// <summary>
        /// 至少选择2条谱线
        /// </summary>
        public const string ZED_SELECT_LINE_TWO = "ZED_SELECT_LINE_TWO";

        /// <summary>
        /// 横轴数据
        /// </summary>
        public const string ZEDBOX_ZEDINFO_XTITLE = "ZEDBOX_ZEDINFO_XTITLE";

        /// <summary>
        /// 纵轴数据
        /// </summary>
        public const string ZEDBOX_ZEDINFO_YTITLE = "ZEDBOX_ZEDINFO_YTITLE";

        /// <summary>
        /// 当前位置（" + ponit.X + "," + ponit.Y + "）
        /// </summary>
        public const string ZEDBOX_ZEDINFO_CURADDRESS = "ZEDBOX_ZEDINFO_CURADDRESS";

        /// <summary>
        /// 所有谱图
        /// </summary>
        public const string ZEDBOX_TM_ALLSPEC = "ZEDBOX_TM_ALLSPEC";

        /// <summary>
        /// 请选择谱图数据
        /// </summary>
        public const string ZEDBOX_SELECTZEDDATA = "ZEDBOX_SELECTZEDDATA";

        /// <summary>
        /// 操作已取消
        /// </summary>
        public const string OPERATION_CANCELED = "OPERATION_CANCELED";

        /// <summary>
        /// 未找到可分析的数据
        /// </summary>
        public const string ANALYZE_DATA_NONE = "ANALYZE_DATA_NONE";

        /// <summary>
        /// 移除操作
        /// </summary>
        public const string REMOVE_OPTIP = "REMOVE_OPTIP";

        /// <summary>
        /// 是否移除此数据
        /// </summary>
        public const string REMOVE_OPTIPINFO = "REMOVE_OPTIPINFO";

        /// <summary>
        /// 成功
        /// </summary>
        public const string SUCCESS_TIP = "SUCCESS_TIP";

        /// <summary>
        /// 请输入谱图名称
        /// </summary>
        public const string SPECTRUM_NAME_VERIFY = "SPECTRUM_NAME_VERIFY";

        /// <summary>
        /// 请输入正确的起始值
        /// </summary>
        public const string RANGE_VERIFY = "RANGE_VERIFY";

        /// <summary>
        /// 原始数据
        /// </summary>
        public const string RADIO_DESCRIPTION = "RADIO_DESCRIPTION_";

        /// <summary>
        /// 请选择采集CCD相机
        /// </summary>
        public const string SELECT_CCDINFO = "SELECT_CCDINFO";

        /// <summary>
        /// 原点不存在，请重新设置原点
        /// </summary>
        public const string ORIGIN_NONE = "ORIGIN_NONE";

        /// <summary>
        /// 光栅设置完成
        /// </summary>
        public const string GRATING_SETTING_END = "GRATING_SETTING_END";

        /// <summary>
        /// 未定标，所有数据将已编码器来处理
        /// </summary>
        public const string NOT_CALIBRATED = "NOT_CALIBRATED";

        /// <summary>
        /// 请选择光栅
        /// </summary>
        public const string SELECT_GRATING = "SELECT_GRATING";

        /// <summary>
        /// 请输入定点波长
        /// </summary>
        public const string POINT_WEL_VERIFY = "POINT_WEL_VERIFY";

        /// <summary>
        /// 请选择采集频率
        /// </summary>
        public const string FREQUENCY_VERIFY = "FREQUENCY_VERIFY";

        /// <summary>
        /// 请增大中心波长
        /// </summary>
        public const string CENTER_WEL_MIN = "CENTER_WEL_MIN";

        /// <summary>
        /// 请选择光栅和激光波长
        /// </summary>
        public const string SELECT_WEL_GRATING = "SELECT_WEL_GRATING";

        /// <summary>
        /// 不能小中心波长【" + minnum + "】
        /// </summary>
        public const string CENTER_WEL_MINNUM = "CENTER_WEL_MINNUM";

        /// <summary>
        /// 中心波长设置错误，请缩小中心波长
        /// </summary>
        public const string CENTER_WEL_MAX = "CENTER_WEL_MAX";

        /// <summary>
        /// 未找到脉冲补偿系数，请先定标
        /// </summary>
        public const string PULSE_COF_NONE = "PULSE_COF_NONE";

        /// <summary>
        /// 光栅设置异常
        /// </summary>
        public const string GRATING_SETTING_ERR = "GRATING_SETTING_ERR";

        /// <summary>
        /// 设置原点中
        /// </summary>
        public const string SETTING_POINTADDRESS = "SETTING_POINTADDRESS";

        /// <summary>
        /// 到中心波长中
        /// </summary>
        public const string SETTING_CENTERWEL = "SETTING_CENTERWEL";

        /// <summary>
        /// 采集类型
        /// </summary>
        public const string TREELIST_COLLECTTYPES = "TREELIST_COLLECTTYPES";

        /// <summary>
        /// 响应时间
        /// </summary>
        public const string TREELIST_RESPONSETIME = "TREELIST_RESPONSETIME";

        /// <summary>
        /// 定点波长
        /// </summary>
        public const string TREELIST_PONITBC = "TREELIST_PONITBC";

        /// <summary>
        /// 采集模式
        /// </summary>
        public const string TREELIST_TIMETYPE = "TREELIST_TIMETYPE";

        /// <summary>
        /// 连续采集
        /// </summary>
        public const string TREELIST_TIMETYPE_CONTINUOUS = "TREELIST_TIMETYPE_CONTINUOUS";

        /// <summary>
        /// 定时（"+pram.ScanningTime+"ms）
        /// </summary>
        public const string TREELIST_TIMETYPE_TIME = "TREELIST_TIMETYPE_TIME";

        /// <summary>
        /// 采集频率
        /// </summary>
        public const string TREELIST_FREQUENCY = "TREELIST_FREQUENCY";

        /// <summary>
        /// 全谱图
        /// </summary>
        public const string TREELIST_TIMETYPE_FULLSPECTRUM = "TREELIST_TIMETYPE_FULLSPECTRUM";

        /// <summary>
        /// 范围（"+pram.RangeMin+"-"+pram.RangeMax+"）
        /// </summary>
        public const string TREELIST_TIMETYPE_RANGE = "TREELIST_TIMETYPE_RANGE";

        /// <summary>
        /// 谱图位置
        /// </summary>
        public const string TREELIST_SPECTRUM_ADDRESS = "TREELIST_SPECTRUM_ADDRESS";

        /// <summary>
        /// 处理方式
        /// </summary>
        public const string TREELIST_SPECTRUM_APPROACH = "TREELIST_SPECTRUM_APPROACH";

        /// <summary>
        /// 【" + pramData.name + "】此参数名已存在.
        /// </summary>
        public const string PRAM_DATA_VERIFY = "PRAM_DATA_VERIFY";

        /// <summary>
        /// 修改参数不存在
        /// </summary>
        public const string PRAM_DATA_NONE = "PRAM_DATA_NONE";

        /// <summary>
        /// 数据已被删除
        /// </summary>
        public const string PRAM_DATA_DEL = "PRAM_DATA_DEL";

        /// <summary>
        /// 用户不存在
        /// </summary>
        public const string USER_NONE = "USER_NONE";

        /// <summary>
        /// 此用户已存在,无法继续操作
        /// </summary>
        public const string USER_EXIST = "USER_EXIST";

        /// <summary>
        /// 账号不存在
        /// </summary>
        public const string ACCOUNT_NONE = "ACCOUNT_NONE";

        /// <summary>
        /// 密码错误
        /// </summary>
        public const string PASSWORD_ERR = "PASSWORD_ERR";

        /// <summary>
        /// 此账号已被禁用
        /// </summary>
        public const string ACCOUNT_DISABLED = "ACCOUNT_DISABLED";

        /// <summary>
        /// 谱图【" + obj.Name + "】暗背景已丢失，无法使用
        /// </summary>
        public const string DARK_BACKGROUND_LOST = "DARK_BACKGROUND_LOST";

        /// <summary>
        /// 谱图【" + obj.Name + "】采集数据已丢失，无法使用
        /// </summary>
        public const string RAW_LOST = "RAW_LOST";

        /// <summary>
        /// 0x + cmd.ToString("X2") + "指令不存在！
        /// </summary>
        public const string COMMAND_NONE = "COMMAND_NONE";

        /// <summary>
        /// 设备未连接
        /// </summary>
        public const string DEV_NOTCONNECTED = "DEV_NOTCONNECTED";

        /// <summary>
        /// 请求超时
        /// </summary>
        public const string REQUEST_TIMED_OUT = "REQUEST_TIMED_OUT";

        /// <summary>
        /// 未找到系数，请先定标
        /// </summary>
        public const string COF_DATA_NONE = "COF_DATA_NONE";

        /// <summary>
        /// 采集状态0-操作成功
        /// </summary>
        public const string COLLECT_STATUS_0 = "COLLECT_STATUS_0";

        /// <summary>
        /// 采集状态1-采样中
        /// </summary>
        public const string COLLECT_STATUS_1 = "COLLECT_STATUS_1";

        /// <summary>
        /// 采集状态2-采集范围超过下限
        /// </summary>
        public const string COLLECT_STATUS_2 = "COLLECT_STATUS_2";

        /// <summary>
        /// 采集状态3-采集精细度不匹配
        /// </summary>
        public const string COLLECT_STATUS_3 = "COLLECT_STATUS_3";

        /// <summary>
        /// 未连接有效设备或者设备未做出厂设置
        /// </summary>
        public const string NOCONNECTDEVICE = "NOCONNECTDEVICE";


        /// <summary>
        /// 没有可存储的数据！
        /// </summary>
        public const string NODATA_STORAGE = "NODATA_STORAGE";


        /// <summary>
        /// 请选择需要处理数据
        /// </summary>
        public const string QT_SELECT_DATA = "QT_SELECT_DATA";

        /// <summary>
        /// 请选择需要删除的行！
        /// </summary>
        public const string NOT_SELECT_ROW_DELETED = "NOT_SELECT_ROW_DELETED";

        /// <summary>
        /// 删除成功！
        /// </summary>
        public const string DELETE_SUCCESSFULLY = "DELETE_SUCCESSFULLY";

        /// <summary>
        /// 密码验证
        /// </summary>
        public const string PASSWORD_AUTHENTICATION = "PASSWORD_AUTHENTICATION";

        /// <summary>
        /// 请选择{0}条需要操作的数据！
        /// </summary>
        public const string SELECT_DATA_OPERATED = "SELECT_DATA_OPERATED";

        /// <summary>
        /// 名称不能为空！
        /// </summary>
        public const string NAME_NONE = "NAME_NONE";

        /// <summary>
        /// 控制台调试
        /// </summary>
        public const string NOT_CONSOLE_DEBUGGING = "NOT_CONSOLE_DEBUGGING";

        /// <summary>
        /// 未找到指定的控制设备！
        /// </summary>
        public const string NOT_FOUND_CONTROL_DEVICE = "NOT_FOUND_CONTROL_DEVICE";

        /// <summary>
        /// 指令未找到！
        /// </summary>
        public const string CMD_NOTFIND = "CMD_NOTFIND";

        /// <summary>
        /// 电机状态获取异常！
        /// </summary>
        public const string C_Electric_machine_EXCEPTION = "C_Electric_machine_EXCEPTION";

        /// <summary>
        /// 无可应用数据！
        /// </summary>
        public const string NOT_NO_APPLICABLE_DATA = "NOT_NO_APPLICABLE_DATA";

        /// <summary>
        /// 请选择操作的电机！
        /// </summary>
        public const string NOT_SELECT_MOTOR_OPERATION = "NOT_SELECT_MOTOR_OPERATION";

        /// <summary>
        /// 请编辑原点信息
        /// </summary>
        public const string NOT_EDIT_ORIGIN_INFORMATION = "NOT_EDIT_ORIGIN_INFORMATION";

        /// <summary>
        /// 信号档位-右侧历史树
        /// </summary>
        public const string COLL_GAIN_MULTIPLE = "COLL_GAIN_MULTIPLE";

        /// <summary>
        /// 滤波权重-右侧历史树
        /// </summary>
        public const string COLL_FILTER_WEIGHTS = "COLL_FILTER_WEIGHTS";

        /// <summary>
        /// 采集精准度-右侧历史树
        /// </summary>
        public const string COLL_ACCURACY = "COLL_ACCURACY";

        /// <summary>
        /// 请输入正确的数值范围【{0}-{1}】！
        /// </summary>
        public const string SET_VAL_RANGE_CHACK = "SET_VAL_RANGE_CHACK";

        /// <summary>
        /// x轴范围设置
        /// </summary>
        public const string X_RANGE_SET = "X_RANGE_SET";

        /// <summary>
        /// Y轴范围设置
        /// </summary>
        public const string Y_RANGE_SET = "Y_RANGE_SET";

        /// <summary>
        /// 缺少配置文件，请在出厂管理中导入*.opcfg文件
        /// </summary>
        public static string CONFIGURATION__MISSING = "CONFIGURATION__MISSING";

        /// <summary>
        /// 缺少配置文件，数据包不完整，导入终止！\r\n"
        /// </summary>
        public static string CONFIGURATION__MISSING_DATA = "CONFIGURATION__MISSING_DATA";

        /// <summary>
        /// 缺少语音类型文件，数据包不完整，导入终止！
        /// </summary>
        public static string CONFIGURATION__SPEED = "CONFIGURATION__SPEED";

        /// <summary>
        /// 缺少指令文件，数据包不完整，导入终止！
        /// </summary>
        public static string CONFIGURATION__COMMAND = "CONFIGURATION__COMMAND";

        /// <summary>
        /// 积分时间(ms)
        /// </summary>
        public const string SPECTRUM_INTEGRATION_TIME = "SPECTRUM_INTEGRATION_TIME";

        /// <summary>
        /// 采集类型
        /// </summary>
        public const string SPECTRUM_MODEL = "SPECTRUM_MODEL";

        /// <summary>
        /// 操作员：{0}
        /// </summary>
        public const string LOGIN_USER = "LOGIN_USER";

        /// <summary>
        /// 账号登录
        /// </summary>
        public const string LOG_LOGIN = "LOG_LOGIN";

        /// <summary>
        /// 光标位置：X: + bf + "um - Y:" + lr + "um"
        /// </summary>
        public const string CURSOR_TIP = "CURSOR_TIP";

        /// <summary>
        /// 自适应控件显示图像
        /// </summary>
        public const string FIT_IMAGE = "FIT_IMAGE";

        /// <summary>
        /// 整个控件布满图像
        /// </summary>
        public const string FULL_IMAGE = "FULL_IMAGE";

        /// <summary>
        /// 原始比例显示图像
        /// </summary>
        public const string ORIGINAL_IMAGE = "ORIGINAL_IMAGE";

        /// <summary>
        /// 图像另存为
        /// </summary>
        public const string SAVE_AS = "SAVE_AS";

        /// <summary>
        /// 添加谱图
        /// </summary>
        public const string ADD_SPECTRUM = "ADD_SPECTRUM";

        /// <summary>
        /// {0}必须大于{1}！
        /// </summary>
        public const string NAME_GREATER_THAN_VAL = "NAME_GREATER_THAN_VAL";

        /// <summary>
        /// {0}必须小于{1}！
        /// </summary>
        public const string NAME_LESS_THAN_VAL = "NAME_LESS_THAN_VAL";

        /// <summary>
        /// 右侧树光栅线数
        /// </summary>
        public const string TREE_GSNUM = "TREE_GSNUM";

        /// <summary>
        /// 右侧树平均次数
        /// </summary>
        public const string TREE_AVGNUM = "TREE_AVGNUM";

        /// <summary>
        /// 右侧树积分时间
        /// </summary>
        public const string TREE_INTEGRATIONTIME = "TREE_INTEGRATIONTIME";

        /// <summary>
        /// 右侧树中心波数
        /// </summary>
        public const string TREE_CENTERWAVE = "TREE_CENTERWAVE";



    }
}
