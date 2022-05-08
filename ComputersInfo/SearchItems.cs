using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputersInfo
{
    public class SearchItems : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        List<ComputerModel> pcModel;
        List<ComputerModel> selectedPCModel;

        public List<ComputerModel> SelectedPCModel
        {
            get => selectedPCModel;
            set
            {
                selectedPCModel = value;
                OnPropertyChanged(nameof(SelectedPCModel));
            }
        }
        public List<ComputerModel> PCModel
        {
            get => pcModel;
            set => pcModel = value;
        }
    }
}
