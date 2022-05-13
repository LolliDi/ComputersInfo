using System.Collections.Generic;
using System.ComponentModel;

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
        int countSelected = 0;

        public List<ComputerModel> SelectedPCModel
        {
            get => selectedPCModel;
            set
            {
                selectedPCModel = value;
                CountSelected = selectedPCModel.Count;
                OnPropertyChanged(nameof(SelectedPCModel));
            }
        }
        public List<ComputerModel> PCModel
        {
            get => pcModel;
            set => pcModel = value;
        }
        public int CountSelected
        {
            get => countSelected;
            set
            {
                countSelected = value;
                OnPropertyChanged(nameof(CountSelected));
            }
        }
    }
}
