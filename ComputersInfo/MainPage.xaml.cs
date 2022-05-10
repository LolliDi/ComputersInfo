using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Word = Microsoft.Office.Interop.Word;

namespace ComputersInfo
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        List<ComputerModel> computerModels = new List<ComputerModel>();
        public MainPage()
        {
            InitializeComponent();
            computerModels = GetComputers();
            info.s.PCModel = computerModels;
            info.s.SelectedPCModel = computerModels;
            MinRadio.IsChecked = true;
            FrameClass.main = this;
        }

        public List<ComputerModel> GetComputers()
        {
            List<ComputerModel> cm = new List<ComputerModel>();
            foreach (Computers c in DBCl.db.Computers.ToList())
            {
                cm.Add(new ComputerModel(c));
            }
            return cm;
        }

        public void Searching()
        {
            DBCl.db = new Entities();
            List<ComputerModel> cm = new List<ComputerModel>();
            string searchParameter = TextBoxRoom.Text.ToLower();
            if (searchParameter.Length > 0)
            {
                foreach (Computers c in DBCl.db.Computers.ToList())
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
            if (searchParameter.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string n = c.Computer.Name ?? "";
                    if (n.ToLower().Contains(searchParameter))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            searchParameter = TextBoxLocalIp.Text.ToLower();
            if (searchParameter.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string ip = c.Computer.IpLocal ?? "";
                    if (ip.ToLower().Contains(searchParameter))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
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

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Searching();
        }

        private void PassportSelected_Click(object sender, RoutedEventArgs e) //создание паспорта всех отображаемых компов
        {
            
        }
    }
}
