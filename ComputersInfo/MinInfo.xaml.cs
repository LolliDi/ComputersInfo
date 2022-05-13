using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ComputersInfo
{
    /// <summary>
    /// Логика взаимодействия для MinInfo.xaml
    /// </summary>
    public partial class MinInfo : Page
    {
        public MinInfo()
        {
            InitializeComponent();
            DataContext = info.s;
        }

        private void SelectItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Uid);
            FrameClass.fr.Navigate(new EditComputer(id));
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as Button).Uid);
            Computers c = DBCl.db.Computers.FirstOrDefault(x => x.id == id);
            switch (MessageBox.Show("Вы действительно хотите удалить компьютер c\nИмя: " + c.Name + "\nip: " + c.IpInternet + "\nlocal ip: " + c.IpLocal, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes:

                    DBCl.db.Computers.Remove(c);
                    DBCl.db.SaveChanges();
                    info.s.SelectedPCModel.Remove(info.s.SelectedPCModel.FirstOrDefault(x => x.Computer == c));
                    info.s.CountSelected = info.s.SelectedPCModel.Count;
                    ItemsControlComputers.Items.Refresh();
                    break;
                default:
                    return;
            }

        }

        private void CreatePassport_Click(object sender, RoutedEventArgs e)
        {
            List<ComputerModel> comps = new List<ComputerModel>();
            int id = Convert.ToInt32((sender as Button).Uid);
            Computers c = DBCl.db.Computers.FirstOrDefault(x => x.id == id);
            comps.Add(new ComputerModel(c));
            CreatePassport.Create(comps);
        }
    }
}
