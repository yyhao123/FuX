using Demo.Model.data;
using Demo.Model.entities;
using Demo.Model.@enum;
using FuX.Core.cache.process;
using FuX.Core.cache.share;
using FuX.Core.db;
using FuX.Core.extend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.services
{
    /// <summary>
    /// 单次采集光谱Service
    /// </summary>
    public interface ISingleSpectrumService
    {
        /// <summary>
        /// 获取光谱谱图数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        Task<Tuple<SpectrumDataRaw, SpectrumDataDark, SpectrumDataWhiteBoard>> GetSpectrumDataAsync(Spectrum spectrum);

        /// <summary>
        /// 获取光谱谱图数据
        /// </summary>
        /// <param name="spectrumDto"></param>
        /// <returns></returns>
        Task<Tuple<SpectrumDataRaw, SpectrumDataDark, SpectrumDataWhiteBoard>> GetSpectrumDataAsync(SpectrumDto spectrumDto);
    }

    // <summary>
    // 单次采集光谱Service
    // </summary>
    public class SingleSpectrumService : CoreUnify<SingleSpectrumService, string>, ISingleSpectrumService
    {
        private ProcessCacheOperate _cacheOperate;
        private ShareCacheOperate _shareCacheOperate;
        private DBOperate _dbOperate;

        public SingleSpectrumService() : base(Guid.NewGuid().ToString("N"))
        {
            _cacheOperate = ProcessCacheOperate.Instance();
            _shareCacheOperate = ShareCacheOperate.Instance();
            _dbOperate = DBOperate.Instance();
        }

        /// <summary>
        /// 获取光谱数据
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public async Task<Tuple<SpectrumDataRaw, SpectrumDataDark, SpectrumDataWhiteBoard>> GetSpectrumDataAsync(Spectrum spectrum)
        {
            BegOperate("GetSpectrumDataAsync");
            try
            {
                if (spectrum == null) return null;

                if (spectrum.AcquireType != AcquireType.Single)
                    throw new Exception($"Spectrum {spectrum.Name} acquire type is no mapping.");

                // TODO: cache support

                var data = await Task.Run(() =>
                {

                    {
                        var raw = _dbOperate.Query<SpectrumDataRaw>(x => x.SpectrumId == spectrum.Id).GetSource<List<SpectrumDataRaw>>()?.SingleOrDefault();

                        if (raw == null)
                            throw new Exception($"Spectrum raw data not found.");

                        if (!string.IsNullOrEmpty(raw.IntensityData))                         
                            raw.Intensity = _shareCacheOperate.GetCache<double[]>(raw.IntensityData).GetSource<double[]>();
                        if (!string.IsNullOrEmpty(raw.IntensityDataLe))                          
                            raw.IntensityLe = _shareCacheOperate.GetCache<double[]>(raw.IntensityDataLe).GetSource<double[]>();
                        var dark = _dbOperate.Query<SpectrumDataDark>(x => x.Id == raw.SpectrumDataDarkId).GetSource<List<SpectrumDataDark>>()?.SingleOrDefault();
                        if (dark == null)
                            throw new Exception($"Spectrum dark data not found.");

                        if (!string.IsNullOrEmpty(dark.IntensityData))                         
                            dark.Intensity = _shareCacheOperate.GetCache<double[]>(dark.IntensityData).GetSource<double[]>();

                        if (!string.IsNullOrEmpty(dark.IntensityDataLe))
                          
                            dark.IntensityLe =  _shareCacheOperate.GetCache<double[]>(dark.IntensityDataLe).GetSource<double[]>();

                        var white = _dbOperate.Query<SpectrumDataWhiteBoard>(x => x.Id == raw.WhiteBoardId).GetSource<List<SpectrumDataWhiteBoard>>()?.SingleOrDefault();
                        if (white != null)
                        {
                            if (!string.IsNullOrEmpty(white.IntensityData))
                                white.Intensity = JsonConvert.DeserializeObject<double[]>(white.IntensityData);
                        }

                        var res = Tuple.Create(raw, dark, white);
                        return res;
                    }
                });

                return data;
            }
            catch (Exception ex)
            {
                EndOperate(false,ex.Message,null,ex,false,true);
                return null;
            }

        }

        /// <summary>
        /// 获取光谱数据
        /// </summary>
        /// <param name="spectrumDto"></param>
        /// <returns></returns>
        public async Task<Tuple<SpectrumDataRaw, SpectrumDataDark, SpectrumDataWhiteBoard>> GetSpectrumDataAsync(SpectrumDto spectrumDto)
        {
            try
            {
                if (spectrumDto == null) return null;

                if (spectrumDto.AcquireType != AcquireType.Single)
                    throw new Exception($"Spectrum {spectrumDto.Name} acquire type is no mapping.");

                // TODO: cache support

                var data = await Task.Run(() =>
                {
                    
                    {
                        var raw = _dbOperate.Query<SpectrumDataRaw>(x => x.SpectrumId == spectrumDto.Id).GetSource<List<SpectrumDataRaw>>()?.SingleOrDefault();

                        if (raw == null)
                            throw new Exception($"Spectrum raw data not found.");

                        if (!string.IsNullOrEmpty(raw.IntensityData))
                           
                            raw.Intensity = _shareCacheOperate.GetCache<double[]>(raw.IntensityData).GetSource<double[]>();
                        if (!string.IsNullOrEmpty(raw.IntensityDataLe))                          
                            raw.IntensityLe = _shareCacheOperate.GetCache<double[]>(raw.IntensityDataLe).GetSource<double[]>();
                        var dark = _dbOperate.Query<SpectrumDataDark>(x => x.Id == raw.SpectrumDataDarkId).GetSource<List<SpectrumDataDark>>()?.SingleOrDefault();
                        if (dark == null)
                            throw new Exception($"Spectrum dark data not found.");

                        if (!string.IsNullOrEmpty(dark.IntensityData))
                          
                            dark.Intensity = _shareCacheOperate.GetCache<double[]>(dark.IntensityData).GetSource<double[]>();
                        if (!string.IsNullOrEmpty(dark.IntensityDataLe))
                           
                            dark.IntensityLe = _shareCacheOperate.GetCache<double[]>(dark.IntensityDataLe).GetSource<double[]>();

                        var white = _dbOperate.Query<SpectrumDataWhiteBoard>(x => x.Id == raw.WhiteBoardId).GetSource<List<SpectrumDataWhiteBoard>>()?.SingleOrDefault();
                        if (white != null)
                        {
                            if (!string.IsNullOrEmpty(white.IntensityData))
                                white.Intensity = JsonConvert.DeserializeObject<double[]>(white.IntensityData);
                        }

                        var res = Tuple.Create(raw, dark, white);
                        return res;
                    }
                });

                return data;
            }
            catch(Exception ex)
            {
                EndOperate(false);
                return null;
            }
           
        }
    }
}
