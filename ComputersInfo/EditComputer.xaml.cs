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
    /// Логика взаимодействия для EditComputer.xaml
    /// </summary>
    public partial class EditComputer : Page
    {
        int idPC;
        Computers computer;
        Processors processor;
        List<VideoControllers> videoControllers = new List<VideoControllers>();
       List<PhysicalMemory> physicalMemories = new List<PhysicalMemory>();
        List<HardDrives> hardDrives = new List<HardDrives>();
        MotherBoards motherBoard = new MotherBoards();
        OS oS;

        public EditComputer(int id)
        {
            InitializeComponent();
            idPC = id;
            ReloadData();
        }

        public void UpdateContexts() //обновляем данные для отображения
        {
            OSinfo.DataContext = oS;
            StackPanelMather.DataContext = motherBoard;
            StckComputer.DataContext = computer;
            StackPanelProcessor.DataContext = processor;
            ListViewMemory.Items.Refresh();
            ListViewMemory.ItemsSource = physicalMemories;
            ListViewHard.Items.Refresh();
            ListViewHard.ItemsSource = hardDrives;
            ListViewVideo.Items.Refresh();
            ListViewVideo.ItemsSource = videoControllers;
        }

        public void ReloadData()
        {
            try
            {
                DBCl.db = new ComputersInfoEntities();
                computer = DBCl.db.Computers.FirstOrDefault(x => x.id == idPC);
                motherBoard = DBCl.db.MotherBoards.FirstOrDefault(x => x.Id == computer.MotherBoardId);
                processor = DBCl.db.Processors.FirstOrDefault(x => x.Id == computer.ProcessorId);
                physicalMemories = DBCl.db.PhysicalMemory.Where(x => x.IdPC == idPC).ToList();
                hardDrives.Clear();
                foreach (ComputerHard ch in DBCl.db.ComputerHard.Where(x => x.IdPC == idPC).ToList())
                {
                    hardDrives.Add(DBCl.db.HardDrives.FirstOrDefault(x => x.Id == ch.IdHard));
                }
                videoControllers.Clear();
                foreach (ComputersVideo ch in DBCl.db.ComputersVideo.Where(x => x.IdPC == idPC).ToList())
                {
                    videoControllers.Add(DBCl.db.VideoControllers.FirstOrDefault(x => x.Id == ch.IdVideo));
                }
                oS = DBCl.db.OS.FirstOrDefault(OS => OS.IdPC == idPC);
                UpdateContexts();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка:\n" + ex, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region EditInformationAndViews
        private bool isToggleProcessor;
        private bool isToggleMemory;
        bool isToggleBoard;

        private void ShowPanel(ItemsControl sp, ref bool toggle, int height, Button btn)
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
        bool isToggleOSInfo;
        private void BtnOSInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(GroupOSInfo, ref isToggleOSInfo, 210, BtnOSInfo);
        }
        private void ManufacturerBoard_Changed(object sender, TextChangedEventArgs e)
        {
            motherBoard.Manufacturer = (sender as TextBox).Text;
        }
        private void Socket_Changed(object sender, TextChangedEventArgs e)
        {
            motherBoard.ChipSet = (sender as TextBox).Text;
        }
        private void CompNumber_Changed(object sender, TextChangedEventArgs e)
        {
            computer.InventoryNumber = (sender as TextBox).Text;
        }
        private void ModelBoard_Changed(object sender, TextChangedEventArgs e)
        {
            motherBoard.Model = (sender as TextBox).Text;
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

            motherBoard.MaxPhysicalMemoryMB = SetNumerableValue(motherBoard.MaxPhysicalMemoryMB, sender as TextBox);
        }
        private void SlotsMemory_Changed(object sender, TextChangedEventArgs e)
        {

            motherBoard.SlotsMemory = SetNumerableValue(motherBoard.SlotsMemory, sender as TextBox);
        }
        private void MemoryType_Changed(object sender, TextChangedEventArgs e)
        {

            motherBoard.MemoryType = (sender as TextBox).Text;
        }
        private void CanalsMemoryCount_Changed(object sender, TextChangedEventArgs e)
        {

            motherBoard.CanalsMemoryCount = SetNumerableValue(motherBoard.CanalsMemoryCount, sender as TextBox);
        }
        private void ManufacturerProcessor_Changed(object sender, TextChangedEventArgs e)
        {
            processor.Manufacturer = (sender as TextBox).Text;
        }
        private void ModelProcessor_Changed(object sender, TextChangedEventArgs e)
        {
            processor.Model = (sender as TextBox).Text;
        }
        private void NumberOfCores_Changed(object sender, TextChangedEventArgs e)
        {
            processor.NumberOfCores = SetNumerableValue(processor.NumberOfCores, sender as TextBox);
        }
        private void ThreadCount_Changed(object sender, TextChangedEventArgs e)
        {
            processor.ThreadCount = SetNumerableValue(processor.ThreadCount, sender as TextBox);
        }
        private void StartClockSpeed_Changed(object sender, TextChangedEventArgs e)
        {
            processor.StartClockSpeed = SetNumerableValue(processor.StartClockSpeed, sender as TextBox);
        }
        private void TechnicalProcess_Changed(object sender, TextChangedEventArgs e)
        {
            processor.TechnicalProcess = SetNumerableValue(processor.TechnicalProcess, sender as TextBox);
        }
        private void L1CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            processor.L1CacheMB = SetNumerableValue(processor.L1CacheMB, sender as TextBox);
        }
        private void L2CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            processor.L2CacheMB = SetNumerableValue(processor.L2CacheMB, sender as TextBox);
        }
        private void L3CacheMB_Changed(object sender, TextChangedEventArgs e)
        {
            processor.L3CacheMB = SetNumerableValue(processor.L3CacheMB, sender as TextBox);
        }
        private void Board_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(GroupBoard, ref isToggleBoard, 125, BtnBoard);
        }
        private void Processor_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(GroupProcessor, ref isToggleProcessor, 210, BtnProcessor);
        }
        private void MemoryStck_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(ListViewMemory, ref isToggleMemory, physicalMemories.Count * 92, BtnRAM);
        }
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(ListViewVideo, ref isToggleVideo, videoControllers.Count * 135, BtnVideo);
        }
        private void ManufacturerVideo_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            videoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Manufacturer = tb.Text;
        }
        private void VideoProc_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            videoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).VideoProcessor = tb.Text;
        }
        private void AdapterRAMMB_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            VideoControllers vc = videoControllers.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.AdapterRAMMB = SetNumerableValue(vc.AdapterRAMMB, tb);
        }
        private void MemFrequency_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            PhysicalMemory vc = physicalMemories.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.Frequency = SetNumerableValue(vc.Frequency, tb);
        }
        private void MemSize_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            PhysicalMemory vc = physicalMemories.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SizeMB = SetNumerableValue(vc.SizeMB, tb);
        }
        private void MemType_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            physicalMemories.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).MemoryType = tb.Text;
        }
        bool isToggleHard;
        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            ShowPanel(ListViewHard, ref isToggleHard, hardDrives.Count * 130, BtnHard);
        }
        private void Manufacturer_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            hardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Manufacturer = tb.Text;
        }
        private void Type_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            hardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid)).Type = tb.Text;
        }
        private void SizeGB_Changed(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            HardDrives vc = hardDrives.FirstOrDefault(x => x.Id == Convert.ToInt32(tb.Uid));
            vc.SizeGB = SetNumerableValue(vc.SizeGB, tb);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DBCl.db.SaveChanges();
            MessageBox.Show("Данные сохранены");
        }
        private void CompName_Changed(object sender, TextChangedEventArgs e)
        {
            computer.Name = (sender as TextBox).Text;
        }
        private void Room_Changed(object sender, TextChangedEventArgs e)
        {
            computer.RoomNumber = (sender as TextBox).Text;
        }
        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            computer.Description = (sender as TextBox).Text;
        }

        #endregion

        private void ReloadInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Сейчас сбросятся несохраненные изменения");
            ReloadData();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DBCl.db = new ComputersInfoEntities();
            FrameClass.main.Search();
            FrameClass.fr.Navigate(FrameClass.main);
        }

        private void CreatePassport_Click(object sender, RoutedEventArgs e)
        {
            List<ComputerModel> comps = new List<ComputerModel>();
            comps.Add(new ComputerModel(computer));
            CreatePassport.Create(comps);
        }
    }
}
