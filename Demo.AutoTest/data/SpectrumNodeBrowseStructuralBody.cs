using Demo.Windows.Core.mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Demo.AutoTest.data
{
    public class SpectrumNodeBrowseStructuralBody : BindNotify
    {
        public string Name
        {
            get
            {
                return GetProperty(() => Name);
            }
            set
            {
                SetProperty(() => Name, value);
            }
        }

        public object NodeID
        {
            get
            {
                return GetProperty(() => NodeID);
            }
            set
            {
                SetProperty(() => NodeID, value);
            }
        }

        public ObservableCollection<SpectrumNodeBrowseStructuralBody> Children
        {
            get
            {
                return GetProperty(() => Children);
            }
            set
            {
                SetProperty<ObservableCollection<SpectrumNodeBrowseStructuralBody>>(() => Children, value);
            }
        }
        public SpectrumNodeBrowseStructuralBody()
        {
            Children = new ObservableCollection<SpectrumNodeBrowseStructuralBody>();
        }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public string SpectrumId { get; set; }

        public string NodeText { get; set; }

        public Color Color { get; set; }
    }
}
