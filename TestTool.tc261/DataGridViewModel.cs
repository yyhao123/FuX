using Demo.Windows.Controls.filterDataGrid;
using CommunityToolkit.Mvvm.Input;
using PacketDotNet;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Windows.Core.mvvm;
using Wpf.Ui.Controls;

namespace TestTool.tc261
{
    public class DataGridViewModel : NotifyObject
    {
        public ObservableCollection<Employe> Employes { get; set; }

        public ObservableCollection<Employe> DisplayEmployes { get; set; }

        private FilterDataGridOperate filterDataGridOperate;

        public List<FooterItem> FooterList { get; set; } = new List<FooterItem>()
        {
            new FooterItem()
            {
                FieldName = "Age", TotalType = TotalType.Ave, FormatString = "平均:0.00"
            } ,
            new FooterItem()
            {
                FieldName = "Salary", TotalType = TotalType.Sum, FormatString = "总分数:0"
            }  ,
            new FooterItem()
            {
                FieldName = "LastName", TotalType = TotalType.Count, FormatString = "计数:0"
            }
        };
        public DataGridViewModel()
        {
            Employes = new ObservableCollection<Employe>();
            Employes.Add(new Employe("33","33",3,2,null) );
            Employes.Add(new Employe("22", "33", 3, 3, null));
            DisplayEmployes = new ObservableCollection<Employe>(Employes);
            filterDataGridControl.ItemsSource = DisplayEmployes;

            filterDataGridOperate = FilterDataGridOperate.Instance(new FilterDataGridData.Basics
            {
               
                FilterDataGrid = filterDataGridControl,
                

            });
            filterDataGridOperate.On();

        }

        /// <summary>
        /// 控件
        /// </summary>
        public FilterDataGrid FilterDataGridControl
        {
            get => filterDataGridControl;
            set => SetProperty(ref filterDataGridControl, value);
        }
        private FilterDataGrid filterDataGridControl = new FilterDataGrid();

       


    }

     

    public class Employe
    {
        #region Public Constructors

        public Employe(string lastName, string firstName, double? salary, int? age, DateTime? startDate,
            bool? manager = false)
        {
            LastName = lastName;
            FirstName = firstName;
            Salary = salary;
            Age = age;
            StartDate = startDate;
            Manager = manager;
        }

        #endregion Public Constructors

        #region Public Properties


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Manager { get; set; }
        public double? Salary { get; set; }
        public int? Age { get; set; }
        public DateTime? StartDate { get; set; }

        #endregion Public Properties
    }
}
