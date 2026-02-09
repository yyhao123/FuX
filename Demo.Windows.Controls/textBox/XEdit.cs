using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FuX.Unility;


namespace Demo.Windows.Controls.textBox
{
    /// <summary>
    /// 继承文本输入，
    /// 
    /// 添加按键输入事件处理
    /// 
    /// 添加范围处理，焦点移入，记录值，输入错误，恢复记录值
    /// 
    /// </summary>
    public class XEdit : TextBox
    {
        private const bool DEBUG = false;//Sdk.DEBUG;

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="thiz"></param>
        /// <param name="val"></param>
        ///-------------------------------------------------------------------------------------------------------------
        public delegate void EditUpdateEvent(XEdit thiz,string txt,double val, double min, double max, object msg);
        public event EditUpdateEvent OnEditUpdate;

        /// <summary>
        /// 最小值
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof(MinValue),
            typeof(double), typeof(XEdit), new PropertyMetadata(0.0, null));

        /// <summary>
        /// 最大值
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof(MaxValue),
            typeof(double), typeof(XEdit), new PropertyMetadata(0.0, null));

        /// <summary>
        /// 最小范围
        /// </summary>
        public double MinValue
        {
            get => (double) GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// 最大范围
        /// </summary>
        public double MaxValue
        {
            get => (double) GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        ///  标记
        /// </summary>
        private static readonly DependencyProperty IsAutoSizeProperty = DependencyProperty.Register(nameof(IsAutoSize),
            typeof(bool), typeof(XEdit), new PropertyMetadata(true, OnValuesChanged));

        /// <summary>
        /// 标记
        /// </summary>
        public bool IsAutoSize
        {
            get => (bool) GetValue(IsAutoSizeProperty);
            set => SetValue(IsAutoSizeProperty, value);
        }

        /// <summary>
        ///  标记
        /// </summary>
        private static readonly DependencyProperty IsTextProperty = DependencyProperty.Register(nameof(IsText),
            typeof(bool), typeof(XEdit), new PropertyMetadata(false, OnValuesChanged));

        /// <summary>
        /// 标记
        /// </summary>
        public bool IsText
        {
            get => (bool) GetValue(IsTextProperty);
            set => SetValue(IsTextProperty, value);
        }

        /// ------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     OnValuesChanged
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// ------------------------------------------------------------------------------------------------------------
        private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is XEdit v)) return;
            v.InvalidateVisual();
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 初始化
        /// </summary>
        ///-------------------------------------------------------------------------------------------------------------
        public XEdit()
        {
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // 回车，验证
            KeyUp += XEdit_KeyUp;
            // 获取焦点
            GotFocus += XEdit_GotFocus;
            // 丢失焦点
            LostFocus += XEdit_LostFocus;
            // 鼠标按下
            PreviewMouseDown += XEdit_PreviewMouseDown;

            var lab = new Label();
            
            var strRange = "范围";
            var strFrom = "从";
            var strTo = "到";

            lab.Content = strRange + "(" + strFrom + ":" + MinValue + "," + strTo + ":" + MaxValue + ")";
            //  有效范围，才显示
            if (MaxValue > MinValue)
            {
                ToolTip = lab;
            }
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        ///-------------------------------------------------------------------------------------------------------------
        [Obsolete]
        protected override Size MeasureOverride(Size constraint)
        {
            //Debug.Print($"MeasureOverride(){constraint}");

            if (constraint.Height <= 0)
                return constraint;
            
            if (IsAutoSize)
            {
                //  字体大小
                var newFontSize = constraint.Height / 2;
                //  字体
                var tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
                //  文字
                var curText = Text;
                // 文字
                var f = new FormattedText(curText,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    tf,
                    newFontSize,
                    Foreground);

                //  宽度对比
                if ((constraint.Width < f.Width) && (newFontSize > 0))
                {
                    //  缩放
                    var newFontSize2 = newFontSize * constraint.Width / f.Width;
                    if (newFontSize2 < newFontSize)
                    {
                        newFontSize = newFontSize2;
                    }
                }

                //  差异过大
                if (Math.Abs(FontSize - newFontSize) > 0.01)
                {
                    FontSize = newFontSize;
                }
            }

            return base.MeasureOverride(constraint);
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 丢失焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        private void XEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DEBUG)
            {
                Debug.Print("-->LostFocus(" + e + ")");
            }

            //  丢失焦点
            PreviewMouseDown += XEdit_PreviewMouseDown;
            //  唤起验证
            CheckAndUpdate();
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 鼠标按下之前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        private void XEdit_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 获取焦点
            Focus();
            e.Handled = true;
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        private void XEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DEBUG)
            {
                Debug.Print("-->GotFocus(" + e + ")");
            }

            // 全选
            SelectAll();
            //  预览按下
            PreviewMouseDown -= XEdit_PreviewMouseDown;
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 处理回车键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        private void XEdit_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (DEBUG)
            {
                Debug.Print("-->KeyDown(" + e.Key + ")");
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // 结束
                CheckAndUpdate();
                //  取消选中
                SelectedText = "";
            }
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 按键抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///-------------------------------------------------------------------------------------------------------------
        private void XEdit_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (DEBUG)
            {
                //Debug.Print("-->KeyUp(" + e.Key + ")");
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // 结束
                CheckAndUpdate();
            }
        }

        ///-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 更新数据
        /// </summary>
        ///-------------------------------------------------------------------------------------------------------------
        private void CheckAndUpdate()
        {
            if (IsText)
            {
                // 数据更新
                OnEditUpdate?.Invoke(this, Text,0, MinValue, MaxValue, null);
                return;
            }

            try
            {
                //   数据
                var t = double.Parse(Text.GetNumber());
                var newValue = t;

                // 设定了范围
                if (MinValue < MaxValue)
                {
                    if (t < MinValue)
                    {
                        newValue = MinValue;
                        Debug.Print(Name + ":太小(" + t + " < " + MinValue + ")");
                    }

                    if (t > MaxValue)
                    {
                        newValue = MaxValue;
                        Debug.Print(Name + ":太大(" + t + " > " + MaxValue + ")");
                    }

                    if (DEBUG)
                    {
                        //Debug.Print("(val:" + newValue + ",min:" + MinValue + ",max:" + MaxValue + ")");
                    }

                    Text = "" + newValue;

                    var bindingExpression = this.GetBindingExpression(XEdit.TextProperty);

                    // 纠正
                    if (newValue != t)
                    {
                        //  范围错误，重新更新内容
                        if (bindingExpression != null)
                        {
                            // 重新读取
                            bindingExpression.UpdateTarget();
                            if (DEBUG)
                            {
                                Debug.Print("-->UpdateTarget()");
                            }
                        }
                    }
                    else
                    {
                        if (bindingExpression != null)
                        {
                            // 更新目标
                            bindingExpression.UpdateSource();

                            if (DEBUG)
                            {
                                Debug.Print("-->UpdateSource()");
                            }
                        }
                    }

                    // 数据更新
                    OnEditUpdate?.Invoke(this,Text, newValue, MinValue, MaxValue, bindingExpression);
                }
                else
                {
                    if (DEBUG)
                    {
                        Debug.Print("没设范围!");
                    }
                }
            }
            catch (FormatException e)
            {
                if (DEBUG)
                {
                    Debug.Print(e.ToString());
                }

                var bindingExpression = this.GetBindingExpression(XEdit.TextProperty);
                if (bindingExpression != null)
                {
                    // 重新读取
                    bindingExpression.UpdateTarget();
                    if (DEBUG)
                    {
                        Debug.Print("-->UpdateTarget()");
                    }
                }
            }
            catch (Exception e)
            {
                if (DEBUG)
                {
                    Debug.Print(e.ToString());
                }

                var bindingExpression = this.GetBindingExpression(XEdit.TextProperty);
                if (bindingExpression != null)
                {
                    // 重新读取
                    bindingExpression.UpdateTarget();
                    if (DEBUG)
                    {
                        Debug.Print("-->UpdateTarget()");
                    }
                }
            }
        }
    }
}