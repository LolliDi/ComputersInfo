using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace ComputersInfo
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        List<ComputerModel> computerModels = new List<ComputerModel>();
        object locker = new object(); // заглушка для потоков
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

        public void Search() //запускаем поисковик в отдельном потоке, чтобы не лагал интерфейс
        {
            string room = TextBoxRoom.Text.ToLower(); //берем текст, т.к. в потоке он будет недоступен
            string name = TextBoxName.Text.ToLower();
            string localIp = TextBoxLocalIp.Text.ToLower();
            string nameUser = TextBoxIp.Text.ToLower();
            string inventory = TextBoxNumber.Text.ToLower();
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(new Action(() => Searching(room,
               name, localIp, nameUser, inventory))));
            thread.Start();
        }

        public async void Searching(string room, string name, string localIp, string namePC, string inventory)
        {
            DBCl.db = new Entities();
            List<ComputerModel> cm = new List<ComputerModel>();
            lock (locker) //доступ только для одного потока, остальные ждут очереди
            {
                if (room.Length > 0)
                {
                    foreach (Computers c in DBCl.db.Computers.ToList())
                    {
                        try
                        {
                            string r = c.RoomNumber ?? "";
                            if (r.ToLower() == room)
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
            }
            if (name.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string n = c.Computer.Name ?? "";
                    if (n.ToLower().Contains(name))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            if (localIp.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string ips = c.Computer.IpLocal ?? "";
                    if (ips.ToLower().Contains(localIp))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            if (namePC.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string nameP = c.Computer.PCName ?? "";
                    if (nameP.ToLower().Contains(namePC))
                    {
                        comp.Add(c);
                    }
                }
                cm = comp;
            }
            if (inventory.Length > 0)
            {
                List<ComputerModel> comp = new List<ComputerModel>();
                foreach (ComputerModel c in cm)
                {
                    string inv = c.Computer.InventoryNumber ?? "";
                    if (inv.ToLower().Contains(inventory))
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
            Search();
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
            Search();
        }

        private void PassportSelected_Click(object sender, RoutedEventArgs e) //создание паспорта всех отображаемых компов
        {
            CreatePassport.Create(info.s.SelectedPCModel);

        }

    }
}
