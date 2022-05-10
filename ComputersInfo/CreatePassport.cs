using System;
using System.Collections.Generic;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;


namespace ComputersInfo
{
    public static class CreatePassport
    {

        public static void Create(List<ComputerModel> computers) //создаем поток на создание паспорта
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(new Action(() => CreatePas(computers))));
            thread.Start();
        }

        public static void CreatePas(List<ComputerModel> computers)
        {
            if (computers.Count > 0)
            {
                Word.Application app = new Word.Application(); //открываем приложение
                Word.Document doc = app.Documents.Add();

                Word.WdColor color = Word.WdColor.wdColorLightOrange; //цвет для выделения новых пунктов
                foreach (ComputerModel cm in computers)
                {
                    Word.Paragraph paragraph = doc.Paragraphs.Add();
                    Word.Range range = paragraph.Range;
                    range.Font.Size = 15;
                    range.Font.Bold = 1;
                    range.Text = (("Кабинет: " + cm.Computer.RoomNumber ?? "??") + ("\nИмя: " + cm.Computer.Name ?? "??") + ("\nIp: " + cm.Computer.IpInternet ?? "??") +
                        ("\nОписание: " + cm.Computer.Description ?? "??")); //оглавление каждого нового компа
                    range.InsertParagraphAfter();
                    Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    int count = 35 + cm.VideoController.Count * 5 + cm.HardDrive.Count * 5;
                    Word.Table table = doc.Tables.Add(tableRange, count, 2);
                    table.Shading.BackgroundPatternColor = Word.WdColor.wdColorLightTurquoise;
                    table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle; //тип границ
                    table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    table.Cell(1, 1).Range.Text = "Параметр";
                    table.Cell(1, 2).Range.Text = "Характеристика";
                    table.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
                    table.Rows[1].Shading.BackgroundPatternColor = color;
                    table.Rows[1].Range.Bold = 1;
                    table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    #region SetInformation
                    SetHeaderToRow(table, 2, "Материнская плата: " + cm.MotherBoard.Manufacturer ?? "??" + " " + cm.MotherBoard.Model ?? "??", color);
                    SetTextToRow(table, 3, "Производитель", cm.MotherBoard.Manufacturer ?? "??");
                    SetTextToRow(table, 4, "Модель", cm.MotherBoard.Model ?? "??");
                    SetTextToRow(table, 5, "Количество слотов ОЗУ", cm.MotherBoard.SlotsMemory.ToString() ?? "??");
                    SetTextToRow(table, 6, "Тип ОЗУ", cm.MotherBoard.MemoryType ?? "??");
                    SetTextToRow(table, 7, "Максимальное кол. оперативной памяти", (cm.MotherBoard.MaxPhysicalMemoryMB.ToString() ?? "??") + " мб");
                    SetTextToRow(table, 8, "Каналы ОЗУ", (cm.MotherBoard.CanalsMemoryCount.ToString() ?? "??"));
                    SetTextToRow(table, 9, "Сокет", cm.MotherBoard.ChipSet ?? "??");

                    SetHeaderToRow(table, 10, "Процессор " + cm.Processor.Model ?? "??", color);
                    SetTextToRow(table, 11, "Производитель", cm.Processor.Manufacturer ?? "??");
                    SetTextToRow(table, 12, "Модель", cm.Processor.Model ?? "??");
                    SetTextToRow(table, 13, "Тех. процесс", (cm.Processor.TechnicalProcess.ToString() ?? "??") + " нм");
                    SetTextToRow(table, 14, "Стандартная частота", (cm.Processor.StartClockSpeed.ToString() ?? "??") + " Гц");
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
                    foreach (VideoControllers video in cm.VideoController)
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
                        SetTextToRow(table, id, "Память", (hard.SizeGB.ToString() ?? "??") + " гб");
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
                    #endregion
                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
                app.Visible = true;
            }
            else
            {
                MessageBox.Show("Не выбрано ни одного компьютера для составления тех паспорта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region MethodsForSetText
        private static void SetTextToRow(Word.Table table, int idRow, string text1, string text2) //текст для строк
        {
            table.Cell(idRow, 1).Range.Text = text1;
            table.Cell(idRow, 2).Range.Text = text2 ?? "??";
        }
        private static void SetHeaderToRow(Word.Table table, int idRow, string text1, Word.WdColor color) //текст для оглавления
        {
            table.Rows[idRow].Cells.Merge();
            table.Cell(idRow, 1).Range.Text = text1;
            table.Cell(idRow, 1).Range.Bold = 1;
            table.Rows[idRow].Shading.BackgroundPatternColor = color; //цвет строки
            table.Rows[idRow].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
        }
        #endregion
    }
}
