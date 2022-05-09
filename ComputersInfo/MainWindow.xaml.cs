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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputersInfo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ComputerModel> computerModels = new List<ComputerModel>();
        public MainWindow()
        {
            InitializeComponent();
            computerModels = GetComputers();
            info.s.PCModel = computerModels;
            info.s.SelectedPCModel = computerModels;
            MinRadio.IsChecked = true;


        }

        #region EditInformationAndViews

        public List<ComputerModel> GetComputers()
        {
            List<ComputerModel> cm = new List<ComputerModel>();
            foreach (Computers c in DBCl.db.Computers.ToList())
            {
                cm.Add(new ComputerModel(c));
            }
            return cm;
        }

        private void ShowPanel(ListView sp, ref bool toggle, int height, Button btn)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (!toggle)
            {
                da.To = height;
                da.Duration = TimeSpan.FromSeconds(0.25);
                sp.BeginAnimation(Border.HeightProperty, da);
                toggle = true;
                btn.Content = "/\\";
            }
            else
            {
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(0.25);
                sp.BeginAnimation(Border.HeightProperty, da);
                toggle = false;
                btn.Content = "\\/";
            }
        }
        private void ShowPanel(GroupBox sp, ref bool toggle, int height, Button btn)
        {
            DoubleAnimation da = new DoubleAnimation();
            if (!toggle)
            {
                da.To = height;
                da.Duration = TimeSpan.FromSeconds(0.25);
                sp.BeginAnimation(Border.HeightProperty, da);
                toggle = true;
                btn.Content = "/\\";
            }
            else
            {

                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(0.25);
                sp.BeginAnimation(Border.HeightProperty, da);
                toggle = false;
                btn.Content = "\\/";
            }
        }
        #endregion

        public void Searching()
        {
            List<ComputerModel> cm = new List<ComputerModel>();
            string searchParameter = TextBoxRoom.Text.ToLower();
            if (searchParameter.Length>0)
            {
                foreach(Computers c in DBCl.db.Computers.ToList())
                {
                    try
                    {
                        string r = c.RoomNumber ?? "";
                        if (r.ToLower() == searchParameter)
                        {
                            cm.Add(new ComputerModel(c));
                        }
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                cm = GetComputers();
            }
            searchParameter = TextBoxName.Text.ToLower();
            if(searchParameter.Length>0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach(ComputerModel c in cm)
                {
                    string n = c.Computer.Name ?? "";
                    if(n.ToLower().Contains(searchParameter))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            searchParameter = TextBoxLocalIp.Text.ToLower();
            if(searchParameter.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string ip = c.Computer.IpLocal ?? "";
                    if(ip.ToLower().Contains(searchParameter))
                    {
                        comp.Add(c);
                    }
                }
                cm= comp;
            }
            searchParameter = TextBoxIp.Text.ToLower();
            if (searchParameter.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string ip = c.Computer.IpInternet ?? "";
                    if (ip.ToLower().Contains(searchParameter))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            info.s.SelectedPCModel = cm;
        }

        private void Room_Changed(object sender, TextChangedEventArgs e)
        {
            Searching();
        }

        private void MinRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (MinRadio.IsChecked == true)
            {
                FrameModePreview.Navigate(new MinInfo());
            }
            else
            {
                FrameModePreview.Navigate(new FullInfo());
            }
        }

        public void ShowOptions()
        {
            DoubleAnimation da = new DoubleAnimation();
            if (StackOptions.Width == 0)
            {
                da.To = 220;
                da.Duration = TimeSpan.FromSeconds(0.25);
                StackOptions.BeginAnimation(Border.WidthProperty, da);
                StackBlur.Visibility = Visibility.Visible;
            }
            else
            {
                da.To = 0;
                da.Duration = TimeSpan.FromSeconds(0.25);
                StackOptions.BeginAnimation(Border.WidthProperty, da);
                StackBlur.Visibility = Visibility.Collapsed;
            }
        }


        private void Options_Click(object sender, RoutedEventArgs e)
        {
            ShowOptions();
        }

        private void StackBlur_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowOptions();
        }
    }
}
