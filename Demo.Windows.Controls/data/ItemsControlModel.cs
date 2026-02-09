using CommunityToolkit.Mvvm.Input;
using Demo.Windows.Core.mvvm;
using FuX.Model.data;
using FuX.Model.@interface;
using FuX.Unility;
using System.Windows.Input;

namespace Demo.Windows.Controls.data
{
    public class ItemsControlModel : NotifyObject
    {
        public ItemsControlModel(string key, object icon, LanguageModel model, EventHandler buttonClickHandler = null, bool isChecked = false, bool isEnabled = true)
        {
            Key = key;
            Title = FuX.Core.handler.LanguageHandler.GetLanguageValue(key, model);
            Icon = icon;
            IsEnabled = isEnabled;
            IsChecked = isChecked;
            if (buttonClickHandler != null)
            {
                OnClickEvent -= buttonClickHandler;
                OnClickEvent += buttonClickHandler;
            }
        }
        /// <summary>
        /// 通过Key过去中英文
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 选中
        /// </summary>
        public bool IsChecked
        {
            get => GetProperty(() => IsChecked);
            set => SetProperty(() => IsChecked, value);
        }

        /// <summary>
        /// 启用
        /// </summary>
        public bool IsEnabled
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
        }
        private bool isChecked = true;

        
        public event EventHandler OnClickEvent;


        /// <summary>
        /// 图标
        /// </summary>
        public object Icon
        {
            get => GetProperty(() => Icon);
            set => SetProperty(() => Icon, value);
        }

        /// <summary>
        /// 显示文字
        /// </summary>
        public string Title
        {
            get => GetProperty(() => Title);
            set => SetProperty(() => Title, value);
        }

        /// <summary>
        /// 预留属性
        /// </summary>
        public object? Content
        {
            get => GetProperty(() => Content);
            set => SetProperty(() => Content, value);
        }

        public AsyncRelayCommand LeftMouseClickCommand => new AsyncRelayCommand(LeftMouseClick);
        private Task   LeftMouseClick()
        {
            OnClickEvent?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }


    }
}
