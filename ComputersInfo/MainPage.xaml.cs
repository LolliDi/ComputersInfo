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
            if (info.s.CountSelected > 0)
            {
                Word.Application app = new Word.Application();
                Word.Document doc = app.Documents.Add();

                Word.WdColor color = Word.WdColor.wdColorAqua; //цвет для выделения новых пунктов
                foreach (ComputerModel cm in info.s.SelectedPCModel)
                {
                    Word.Paragraph paragraph = doc.Paragraphs.Add();
                    Word.Range range = paragraph.Range;
                    range.Font.Size = 15;
                    range.Font.Bold = 1;
                    range.Text = (("Кабинет: " + cm.Computer.RoomNumber ?? "??") + ("\nИмя: " + cm.Computer.Name ?? "??") + ("\nIp: " + cm.Computer.IpInternet ?? "??") +
                        ("\nОписание: " + cm.Computer.Description ?? "??"));
                    range.InsertParagraphAfter();
                    Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    int count = 35 + cm.VideoController.Count * 5 + cm.HardDrive.Count * 5;
                    Word.Table table = doc.Tables.Add(tableRange, count, 2);
                    table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle; //тип границ
                    table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(1, 1).Range.Text = "Параметр";
                    table.Cell(1, 2).Range.Text = "Характеристика";
                    table.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
                    table.Rows[1].Range.Bold = 1;
                    table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    SetHeaderToRow(table, 2, "Материнская плата: " + cm.MotherBoard.Manufacturer ?? "??" + " " + cm.MotherBoard.Model ?? "??", color);
                    SetTextToRow(table, 3, "Производитель", cm.MotherBoard.Manufacturer ?? "??");
                    SetTextToRow(table, 4, "Модель", cm.MotherBoard.Model ?? "??");
                    SetTextToRow(table, 5, "Количество слотов ОЗУ", cm.MotherBoard.SlotsMemory.ToString() ?? "??");
                    SetTextToRow(table, 6, "Тип ОЗУ", cm.MotherBoard.MemoryType ?? "??");
                    SetTextToRow(table, 7, "Максимальное кол. оперативной памяти", (cm.MotherBoard.MaxPhysicalMemoryMB.ToString() ?? "??") + " мб");
                    SetTextToRow(table, 8, "Каналы ОЗУ", (cm.MotherBoard.CanalsMemoryCount.ToString() ?? "??"));
                    SetTextToRow(table, 9, "Сокет", cm.MotherBoard.ChipSet ?? "??");

                    SetHeaderToRow(table, 10, "Процессор " + cm.Processor.Model ?? "??",color);
                    SetTextToRow(table, 11, "Производитель", cm.Processor.Manufacturer ?? "??");
                    SetTextToRow(table, 12, "Модель", cm.Processor.Model ?? "??");
                    SetTextToRow(table, 13, "Тех. процесс", (cm.Processor.TechnicalProcess.ToString() ?? "??") + " нм");
                    SetTextToRow(table, 14, "Стандартная частота", (cm.Processor.StartClockSpeed.ToString() ?? "??")+" Гц");
                    SetTextToRow(table, 15, "Количество ядер", cm.Processor.NumberOfCores.ToString() ?? "??");
                    SetTextToRow(table, 16, "Количество потоков", cm.Processor.ThreadCount.ToString() ?? "??");
                    SetTextToRow(table, 17, "Кеш 1-го уровня", (cm.Processor.L1CacheMB.ToString() ?? "??") + " мб");
                    SetTextToRow(table, 18, "Кеш 2-го уровня", (cm.Processor.L2CacheMB.ToString() ?? "??") + " мб");
                    SetTextToRow(table, 19, "Кеш 3-го уровня", (cm.Processor.L3CacheMB.ToString() ?? "??") + " мб");

                    SetHeaderToRow(table, 20, "Оперативная память", color);
                    SetTextToRow(table, 21, "Количество памяти", (cm.MemoryCountMB.ToString() ?? "??") + "мб");
                    SetTextToRow(table, 22, "Частота", (cm.PhysicalMemory[0].Frequency.ToString() ?? "??") + "Гц");
                    SetTextToRow(table, 23, "Тип", cm.MotherBoard.MemoryType ?? "??");

                    int id = 25;
                    SetHeaderToRow(table, 24, "Видеокарты", color);
                    foreach(VideoControllers video in cm.VideoController)
                    {
                        SetHeaderToRow(table, id, ((video.Manufacturer ?? "??") + " " + (video.Model ?? "??")), color);
                        id++;
                        SetTextToRow(table, id, "Производитель", video.Manufacturer ?? "??");
                        id++;
                        SetTextToRow(table, id, "Модель", video.Model ?? "??");
                        id++;
                        SetTextToRow(table, id, "Видеопроцессор", video.VideoProcessor ?? "??");
                        id++;
                        SetTextToRow(table, id, "Количество видеопамяти", (video.AdapterRAMMB.ToString() ?? "??") + " мб");
                        id++;
                    }
                    SetHeaderToRow(table, id, "Жесткие диски", color);
                    id++;
                    foreach (HardDrives hard in cm.HardDrive)
                    {
                        SetHeaderToRow(table, id, (hard.Manufacturer ?? "??" + " " + hard.Model ?? "??"), color);
                        id++;
                        SetTextToRow(table, id, "Производитель", hard.Manufacturer ?? "??");
                        id++;
                        SetTextToRow(table, id, "Модель", hard.Model ?? "??");
                        id++;
                        SetTextToRow(table, id, "Память", (hard.SizeGB.ToString() ?? "??")+" гб");
                        id++;
                        SetTextToRow(table, id, "Интерфейс", hard.Interface ?? "??");
                        id++;
                    }

                    SetHeaderToRow(table, id, "Операционная система", color);
                    id++;
                    SetTextToRow(table, id, "Название", cm.OS.Title ?? "??");
                    id++;
                    SetTextToRow(table, id, "Архитектура", cm.OS.Architecture ?? "??");
                    id++;
                    SetTextToRow(table, id, "Номер продукта", cm.OS.NumberProduct ?? "??");
                    id++;

                    SetHeaderToRow(table, id, "О компьютере", color);
                    id++;
                    SetTextToRow(table, id, "Название", cm.Computer.PCName ?? "??");
                    id++;
                    SetTextToRow(table, id, "Глобальный ip", cm.Computer.IpInternet ?? "??");
                    id++;
                    SetTextToRow(table, id, "Локальный ip", cm.Computer.IpLocal ?? "??");
                    id++;
                    SetTextToRow(table, id, "Мак адрес", cm.Computer.MacAdress ?? "??");
                    id++;
                    SetTextToRow(table, id, "Пользователи", cm.Computer.Users ?? "??");
                    id++;

                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
                app.Visible = true;
            }
            else
            {
                MessageBox.Show("Не выбрано ни одного компьютера для составления тех паспорта","Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SetTextToRow(Word.Table table, int idRow, string text1, string text2)
        {
            table.Cell(idRow,1).Range.Text = text1;
            table.Cell(idRow, 2).Range.Text = text2 ?? "??";
        }
        public void SetHeaderToRow(Word.Table table, int idRow, string text1, Word.WdColor color)
        {
            table.Rows[idRow].Cells.Merge();
            table.Cell(idRow, 1).Range.Text = text1;
            table.Cell(idRow, 1).Range.Bold = 1;
            table.Rows[idRow].Shading.BackgroundPatternColor = color; //цвет строки
            table.Rows[idRow].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        }
    }
}
