using Demo.Core.extend;
using Demo.Model.data;
using Demo.Model.entities;
using Demo.Model.@enum;
using Demo.Windows.Core.handler;
using FuX.Core.cache.share;
using FuX.Core.db;
using FuX.Core.extend;
using FuX.Model.data;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Demo.AutoTest.services
{
    public interface ISpectrumService
    {
        Task<SpectrumDto> GetAsync(string spectrumId);

        bool UpdateSpectrum(SpectrumDto spectrumDto);

        /// <summary>
        /// add spectrum to cache for data process trace
        /// </summary>
        /// <param name="spectrumDto"></param>
       // OperateResult AddSpectrumCache(SpectrumDto spectrumDto);

        void RemoveSpectrumCache(IEnumerable<string> ids);

        void ClearSpectrumCache();

        IEnumerable<SpectrumHistoryDto> GetListDto(IndexSpectrumRequest options);

        IEnumerable<Spectrum> GetList(IndexSpectrumRequest options);

        Task<IEnumerable<SpectrumHistoryDto>> GetListDtoAsync(IndexSpectrumRequest options);

        bool DeleteSpectrum(string spectrumId, AcquireType acquireType);

        Task<SpectrumDataDto> GetSpectrumDataAsync(SpectrumDto spectrumDto, int row, int column);
    }

    public class SpectrumService :CoreUnify<SpectrumService,string>,ISpectrumService
    {
        /// <summary>
        /// 光谱缓存为了控件显示；谱图数据处理结果暂存，导出时数据处理不会丢失
        /// </summary>
        private static ConcurrentDictionary<string, SpectrumDto> _spectrumCache = new ConcurrentDictionary<string, SpectrumDto>();

        protected DBOperate _dbOperate;
        private ShareCacheOperate _shareCacheOperate;
        private IDeviceRamanShiftService _deviceRamanShiftService;

        /// <summary>
        /// single spectrum service
        /// </summary>
        private ISingleSpectrumService _singleSpectrumService;


        public SpectrumService() :base(Guid.NewGuid().ToString("N"))
        {                
            _dbOperate = DBOperate.Instance();
            _shareCacheOperate = ShareCacheOperate.Instance();
            _singleSpectrumService=InjectionWpf.GetService<ISingleSpectrumService>();
        }

        public async Task<SpectrumDto> GetAsync(string spectrumId)
        {
            
            var res = default(SpectrumDto);


            if (string.IsNullOrEmpty(spectrumId) || _spectrumCache.TryGetValue(spectrumId, out res))
                return res;

            res = await Task.Run(() => DoGetAsync(spectrumId)).ConfigureAwait(false);

            return res;
        }

        private async Task<SpectrumDto> DoGetAsync(string spectrumId)
        {
            var res = default(SpectrumDto);

            if (_spectrumCache.TryGetValue(spectrumId, out res))
                return res;

            var spectrum = default(Spectrum);
            var spectrumDark = default(SpectrumDataDark);
            var spectrumRaw = default(SpectrumDataRaw);
            var whiteBoard = default(SpectrumDataWhiteBoard);

            #region Get spectrum record

           
             spectrum =_dbOperate.Query<Spectrum>(x => x.Id == spectrumId).GetSource<List<Spectrum>>()?.SingleOrDefault();
             if (spectrum == null) return null;

             var ramanShift = _dbOperate.Query<DeviceRamanShift>(d=>d.Id==spectrum.DeviceRamanShiftId).GetSource<List<DeviceRamanShift>>()?.SingleOrDefault();
             spectrum.DeviceRamanShift = ramanShift;
         

            #endregion

            #region Get spectrum data record

            if (spectrum.AcquireType == AcquireType.Single)
            {
                var singleData = await _singleSpectrumService.GetSpectrumDataAsync(spectrum).ConfigureAwait(false);
                spectrumRaw = singleData.Item1;
                spectrumDark = singleData.Item2;
                whiteBoard = singleData.Item3;
            }
            else
            {
                throw new ArgumentException($"Get spectrum unsupport acquire type {spectrum.AcquireType}");
            }

            if (spectrum.DeviceRamanShift == null)
            {
                spectrum.DeviceRamanShift = new DeviceRamanShift();
            }
            #endregion

            return new SpectrumDto(spectrum, spectrumRaw, spectrumDark, whiteBoard);
        }


        public bool UpdateSpectrum(SpectrumDto spectrumDto)
        {
            if (spectrumDto == null) return false;

            // check spectrum source
            if (spectrumDto.Source == SpectrumSource.ImportSingle) return true;

            if (spectrumDto.AcquireType == AcquireType.Single)
                return UpdateSingleSpectrum(spectrumDto);
            else
                throw new ArgumentException($"Update spectrum unsupport acquire type {spectrumDto.AcquireType}.");
        }


        private bool UpdateSingleSpectrum(SpectrumDto spectrumDto)
        {
            // TODO: check spectrum source          
            {
                var spectrum = _dbOperate.Query<Spectrum>(x => x.Id == spectrumDto.Id).GetSource<Spectrum>();
                if (spectrum == null) return false;

                spectrum.Name = spectrumDto.Name;
                spectrum.Comment = spectrumDto.Comment;

                return  _dbOperate.Update<Spectrum>(spectrum,null).Status;
            }
        }

        /// <summary>
        /// 添加光谱缓存为了控件显示；谱图数据处理结果暂存，导出时数据处理不会丢失
        /// </summary>
        /// <param name="spectrumDto"></param>
        //public OperateResult AddSpectrumCache(SpectrumDto spectrumDto)
        //{
        //    BegOperate();
        //    try
        //    {
        //        if (spectrumDto == null) return EndOperate(false);

        //        if (_spectrumCache.ContainsKey(spectrumDto.Id)) EndOperate(false);

        //        const int maxCacheCount = 500;
        //        if (_spectrumCache.Count > maxCacheCount)
        //            throw new Exception($"Cached spectrum exceed maximum count {maxCacheCount}.");

        //        _spectrumCache.TryAdd(spectrumDto.Id, spectrumDto);
        //        return EndOperate(true);
        //    }
        //    catch(Exception ex)
        //    {
        //        return EndOperate(false, ex.Message, exception: ex);
        //    }

           
        //}

        /// <summary>
        /// 移除光谱缓存
        /// </summary>
        /// <param name="ids"></param>
        public void RemoveSpectrumCache(IEnumerable<string> ids)
        {
            if (ids == null || ids.Count() < 1) return;

            foreach (var id in ids)
            {
                var tmp = default(SpectrumDto);
                _spectrumCache.TryRemove(id, out tmp);
            }
        }

        /// <summary>
        /// 清除光谱缓存
        /// </summary>
        public void ClearSpectrumCache()
        {
            _spectrumCache.Clear();
        }

        public Task<IEnumerable<SpectrumHistoryDto>> GetListDtoAsync(IndexSpectrumRequest options)
        {
            var tsk = Task.Run(() =>
            {
                var lst = GetList(options);

                return lst.Select();
            });

            return tsk;
        }

        public IEnumerable<SpectrumHistoryDto> GetListDto(IndexSpectrumRequest options)
        {
            var lst = GetList(options);

            return lst.Select();
        }

        public IEnumerable<Spectrum> GetList(IndexSpectrumRequest options)
        {
           
            {
                return _dbOperate.Query<Spectrum>(c=>1==1).GetSource<List<Spectrum>>()                   
                    .Filter(options.SortFilterPageOptions)
                    .Sort(options.SortFilterPageOptions)
                    .Skip(options.SortFilterPageOptions.SkipCount)
                    .Take(options.SortFilterPageOptions.PageNumber).ToList();
            }
        }

        public bool DeleteSpectrum(string spectrumId, AcquireType acquireType)
        {
            if (string.IsNullOrEmpty(spectrumId)) return false;

            var succ = false;

            if (acquireType == AcquireType.Single)
                succ = DeleteSingleSpectrum(spectrumId);
            else
                throw new ArgumentException($"Delete spctrum unsupport acquire type {acquireType}.");

            return succ;
        }

        private bool DeleteSingleSpectrum(string spectrumId)
        {
           
            {
                var spectrum = _dbOperate.Query<Spectrum>(x => x.Id == spectrumId).GetSource<List<Spectrum>>()?.SingleOrDefault();
                if (spectrum == null) return false;

                var ramanShift = _dbOperate.Query<DeviceRamanShift>(x => x.Id == spectrum.DeviceRamanShiftId).GetSource<List<DeviceRamanShift>>()?.SingleOrDefault();

                var raw = _dbOperate.Query<SpectrumDataRaw>(x => x.SpectrumId == spectrumId).GetSource<List<SpectrumDataRaw>>()?.SingleOrDefault();

                _dbOperate.Delete<Spectrum>(s=>s.Id==spectrum.Id);
                _dbOperate.Delete<SpectrumDataRaw>(s => s.SpectrumId == raw.SpectrumId);
                if (ramanShift != null)
                _dbOperate.Delete<DeviceRamanShift>(s => s.Id == ramanShift.Id);
                _shareCacheOperate.RemoveCache(raw.IntensityData);
                 var darkCount = _dbOperate.Query<SpectrumDataRaw>(x => x.SpectrumDataDarkId == raw.SpectrumDataDarkId).GetSource<List<SpectrumDataRaw>>()?.Count();
                if (darkCount < 1)
                {
                    var dark = _dbOperate.Query<SpectrumDataDark>(x => x.Id == raw.SpectrumDataDarkId).GetSource<List<SpectrumDataDark>>()?.SingleOrDefault() ;
                    _shareCacheOperate.RemoveCache(dark.IntensityData);
                    _dbOperate.Delete<SpectrumDataDark>(d=>d.Id==dark.Id);
                }

                // var whiteCount = ctx.SpectrumDataRaw.Find(x => x.WhiteBoardId == raw.WhiteBoardId).Count();
                if (darkCount < 1)
                {
                    var whiteBoard = _dbOperate.Query<SpectrumDataWhiteBoard>(x => x.Id == raw.WhiteBoardId).GetSource<List<SpectrumDataWhiteBoard>>()?.SingleOrDefault();
                    if (whiteBoard != null)
                        _dbOperate.Delete<SpectrumDataWhiteBoard>(s=>s.Id==whiteBoard.Id);
                }

                // remove cache
                RemoveSpectrumCache(new List<string>() { spectrumId });


                return true;
            }
        }

        /// <summary>
        /// Get spectrum data if cached spectrum has matched spectrum data return matched otherwise retrieve from db
        /// </summary>
        /// <param name="spectrumDto"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public async Task<SpectrumDataDto> GetSpectrumDataAsync(SpectrumDto spectrumDto, int row, int column)
        {
            if (spectrumDto == null) return null;

            var res = default(SpectrumDataDto);
            var cachedSpectrumDto = default(SpectrumDto);

            if (_spectrumCache.TryGetValue(spectrumDto.Id, out cachedSpectrumDto))
            {
                res = cachedSpectrumDto.Data.FirstOrDefault();

                if (res != null) return res;
            }

            if (spectrumDto.AcquireType == AcquireType.Single)
            {
                var data = await _singleSpectrumService.GetSpectrumDataAsync(spectrumDto);
                res = new SpectrumDataDto(data.Item1, data.Item2, data.Item3);
            }
            else
                throw new Exception($"Unsupport acquire type {spectrumDto.AcquireType}.");

            return res;
        }
    }
}
