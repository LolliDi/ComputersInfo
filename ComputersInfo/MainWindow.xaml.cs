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
            foreach(Computers c in DBCl.db.Computers.ToList())
            {
                computerModels.Add(new ComputerModel(c));
            }
            ItemsControlComputers.ItemsSource = computerModels;
            
        }

        #region EditInformationAndViews

        public int GetIdInList(object sender)
        {
            int id = Convert.ToInt32((sender as TextBox).Name);
            int idm = computerModels.IndexOf(computerModels.FirstOrDefault(x => x.Computer.id == id));
            return idm;
        }

        private bool isToggleProcessor;
        private bool isToggleMemory;
        bool isToggleBoard;

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
        bool isToggleVideo;
        private void ManufacturerBoard_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.Manufacturer = (sender as TextBox).Text;
        }
        private void Socket_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.ChipSet = (sender as TextBox).Text;
        }
        private void ModelBoard_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.Model = (sender as TextBox).Text;
        }
        public double? SetNumerableValue(double? m, TextBox tb)
        {
            try
            {
                if (tb.Text.Length > 0)
                    return Convert.ToDouble(tb.Text);
                else
                    return null;

            }
            catch
            {
                tb.Text = m.ToString();
                return m;
            }
        }
        public int? SetNumerableValue(int? m, TextBox tb)
        {
            try
            {
                if (tb.Text.Length > 0)
                    return Convert.ToInt32(tb.Text);
                else
                    return null;
            }
            catch
            {
                tb.Text = m.ToString();
                return m;
            }
        }
        private void MaxPhysicalMemoryMB_Changed(object sender, TextChangedEventArgs e)
        {

            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.MaxPhysicalMemoryMB = SetNumerableValue(computerModels[id].MotherBoard.MaxPhysicalMemoryMB, sender as TextBox);
        }
        private void SlotsMemory_Changed(object sender, TextChangedEventArgs e)
        {

            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.SlotsMemory = SetNumerableValue(computerModels[id].MotherBoard.SlotsMemory, sender as TextBox);
        }
        private void MemoryType_Changed(object sender, TextChangedEventArgs e)
        {

            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.MemoryType = (sender as TextBox).Text;
        }
        private void CanalsMemoryCount_Changed(object sender, TextChangedEventArgs e)
        {

            int id = GetIdInList(sender);
            computerModels[id].MotherBoard.CanalsMemoryCount = SetNumerableValue(computerModels[id].MotherBoard.CanalsMemoryCount, sender as TextBox);
        }
        private void ManufacturerProcessor_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.Manufacturer = (sender as TextBox).Text;
        }
        private void ModelProcessor_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.Model = (sender as TextBox).Text;
        }
        private void NumberOfCores_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.NumberOfCores = SetNumerableValue(computerModels[id].Processor.NumberOfCores, sender as TextBox);
        }
        private void ThreadCount_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.ThreadCount = SetNumerableValue(computerModels[id].Processor.ThreadCount, sender as TextBox);
        }
        private void StartClockSpeed_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.StartClockSpeed = SetNumerableValue(computerModels[id].Processor.StartClockSpeed, sender as TextBox);
        }
        private void TechnicalProcess_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.TechnicalProcess = SetNumerableValue(computerModels[id].Processor.TechnicalProcess, sender as TextBox);
        }
        private void L1CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.L1CacheMB = SetNumerableValue(computerModels[id].Processor.L1CacheMB, sender as TextBox);
        }
        private void L2CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.L2CacheMB = SetNumerableValue(computerModels[id].Processor.L2CacheMB, sender as TextBox);
        }
        private void L3CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Processor.L3CacheMB = SetNumerableValue(computerModels[id].Processor.L3CacheMB, sender as TextBox);
        }
        private void Board_Click(object sender, RoutedEventArgs e)
        {
            //ShowPanel(GroupBoard, ref isToggleBoard, 125, BtnBoard);
        }
        private void Processor_Click(object sender, RoutedEventArgs e)
        {
            //ShowPanel(GroupProcessor, ref isToggleProcessor, 210, BtnProcessor);
        }
        private void MemoryStck_Click(object sender, RoutedEventArgs e)
        {
            //ShowPanel(ListViewMemory, ref isToggleMemory, physicalMemories.Count * 92, BtnRAM);
        }
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            //ShowPanel(ListViewVideo, ref isToggleVideo, videoControllers.Count * 135, BtnVideo);
        }
        private void ManufacturerVideo_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Manufacturer = tb.Text;
        }
        private void VideoProc_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).VideoProcessor = tb.Text;
        }
        private void AdapterRAMMB_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            VideoControllers vc = computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.AdapterRAMMB = SetNumerableValue(vc.AdapterRAMMB, tb);
        }
        private void MaxFPS_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            VideoControllers vc = computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.MaxRefreshRate = SetNumerableValue(vc.MaxRefreshRate, tb);
        }
        private void Vertical_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            VideoControllers vc = computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.CurrentVerticalResolution = SetNumerableValue(vc.CurrentVerticalResolution, tb);

        }
        private void Horizontal_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            VideoControllers vc = computerModels[id].VideoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.CurrentVHorizontalResolution = SetNumerableValue(vc.CurrentVHorizontalResolution, tb);
        }
        private void MemFrequency_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            PhysicalMemory vc = computerModels[id].PhysicalMemory.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.Frequency = SetNumerableValue(vc.Frequency, tb);
        }
        private void MemSize_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            PhysicalMemory vc = computerModels[id].PhysicalMemory.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SizeMB = SetNumerableValue(vc.SizeMB, tb);
        }
        private void MemType_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].PhysicalMemory.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).MemoryType = tb.Text;
        }
        bool isToggleHard;
        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            //ShowPanel(ListViewHard, ref isToggleHard, hardDrives.Count * 220, BtnHard);
        }
        private void Manufacturer_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Manufacturer = tb.Text;
        }
        private void Type_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Type = tb.Text;
        }
        private void SizeGB_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            HardDrives vc = computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SizeGB = SetNumerableValue(vc.SizeGB, tb);
        }
        private void Interface_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Interface = tb.Text;
        }
        private void SpeedWrite_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            HardDrives vc = computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SpeedWriteMBS = SetNumerableValue(vc.SpeedWriteMBS, tb);
        }
        private void SpeedRead_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            HardDrives vc = computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SpeedReadMBS = SetNumerableValue(vc.SpeedReadMBS, tb);
        }
        private void Buffer_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int id = GetIdInList(sender);
            HardDrives vc = computerModels[id].HardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.BufferMB = SetNumerableValue(vc.BufferMB, tb);
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DBCl.db.SaveChanges();
            MessageBox.Show("Данные сохранены");
        }
        private void CompName_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Computer.Name = (sender as TextBox).Text;
        }
        private void Room_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Computer.RoomNumber = (sender as TextBox).Text;
        }
        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            int id = GetIdInList(sender);
            computerModels[id].Computer.Description = (sender as TextBox).Text;
        }
        #endregion
    }
}
