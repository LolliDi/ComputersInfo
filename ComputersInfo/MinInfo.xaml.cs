using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                    ItemsControlComputers.Items.Refresh();
                    break;
                default:
                    return;
            }
            
        }
    }
}
