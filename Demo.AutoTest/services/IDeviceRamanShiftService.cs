using Demo.Model.data;
using Demo.Model.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.services
{
    public interface IDeviceRamanShiftService
    {
        DeviceRamanShift Get(string Id);

        DeviceRamanShift GetTmpRamanShift(string dbPath, string Id);
        bool SaveToDB(DeviceRamanShift model);

        DeviceRamanShift GetFromDB(double[] ramanShift);

        /// <summary>
        /// 获取光谱数据
        /// </summary>
        /// <returns></returns>
        List<QuantitativeSpecData> GetQtSpecData();


        /// <summary>
        /// 获取谱图的强度值
        /// </summary>
        /// <returns></returns>
        List<double[]> GetSpectrumData(List<string> sid);
    }
}
