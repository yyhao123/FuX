using CommunityToolkit.Mvvm.Input;
using Demo.AutoTest.data;
using Demo.AutoTest.view.userControls.bars;
using Demo.Windows.Core.handler;
using Demo.Windows.Core.mvvm;
using FuX.Core.extend;
using FuX.Core.services;
using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Unility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Demo.AutoTest.viewModel.Module
{
    public class BottomHistoryAreaViewModel: BindNotify,IEvent
    {
        private ILocalize _localize;

        public event EventHandler<EventDataResult> OnDataEvent;
        public event EventHandlerAsync<EventDataResult> OnDataEventAsync;
        public event EventHandler<EventInfoResult> OnInfoEvent;
        public event EventHandlerAsync<EventInfoResult> OnInfoEventAsync;
        public event EventHandler<EventLanguageResult> OnLanguageEvent;
        public event EventHandlerAsync<EventLanguageResult> OnLanguageEventAsync;

        public BottomHistoryAreaViewModel()
        {
            _localize = InjectionWpf.GetService<ILocalize>();
        }

        public AcquireModuleDataInfo AcquireModuleDataInfo { get; set; } = new AcquireModuleDataInfo();
        public ObservableCollection<HistorySpectrumBrowseStructuralBody> HistorySpectrum
        {
            get
            {
                return GetProperty(() => HistorySpectrum);
            }
            set
            {
                SetProperty<ObservableCollection<HistorySpectrumBrowseStructuralBody>>(() => HistorySpectrum, value);
            }
        }

        public void RefreshDataSource()
        {
            HistorySpectrum = new ObservableCollection<HistorySpectrumBrowseStructuralBody>();
           
            foreach (var item in AcquireModuleDataInfo.SpectrumHistory)
            {
                HistorySpectrum.Add(new HistorySpectrumBrowseStructuralBody()
                {
                    SpectrumName = item.Name,
                    CollectTypes = _localize.GetString(item.CollectTypes.ToString()),
                    DisplayDataType = _localize.GetString(item.DisplayDataType.ToString()),
                    CreatedDate = item.Created.ToString(),
                    PixelCount = item.PixelCount.ToString(),
                    Comment = item.Comment,
                    SpectrumId =item.SpectrumId

                });
            }
        }


       

        public HistorySpectrumBrowseStructuralBody SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }



        /// <summary>
        /// 双击
        /// </summary>
        public IAsyncRelayCommand MouseDoubleClickCommand => new AsyncRelayCommand(MouseDoubleClickAsync);

        public async Task MouseDoubleClickAsync()
        {
            OnDataEventHandler(this, new EventDataResult(status: true, "双击事件", SelectedItem));
        }

        protected void OnDataEventHandler(object? sender, EventDataResult e)
        {
            this.OnDataEvent?.Invoke(sender, e);
           
        }
    }
}
