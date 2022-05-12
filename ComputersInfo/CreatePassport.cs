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
                Word.Paragraph paragraph = doc.Paragraphs.Add();
                Word.Range range = paragraph.Range;
                Word.InlineShape image = range.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "\\header.jfif");
               
                foreach (ComputerModel cm in computers)
                {
                    Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                    Word.Range tableRange = tableParagraph.Range;
                    Word.Table table = doc.Tables.Add(tableRange, 28, 3);
                    table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle; //тип границ
                    #region SetInformation
                    SetTextToRowNormal(table, 1, "Номер кабинета", cm.Computer.RoomNumber ?? "??");
                    SetTextToRowNormal(table, 2, "ФИО работника", cm.Computer.Name ?? "??");
                    SetTextToRowNormal(table, 3, "Имя пользователя в домене", cm.Computer.UserNick ?? "??");
                    SetTextToRowNormal(table, 4, "Электронная почта", "");
                    SetTextToRowNormal(table, 5, "ЭП", "");
                    SetTextToRowNormal(table, 6, "Тип АРМ", "Компьютерный комплект");
                    SetTextToRowNormal(table, 7, "IP-адрес", cm.Computer.IpLocal ?? "??");
                    SetTextToRowNormal(table, 8, "Инвентарный номер", cm.Computer.InventoryNumber ?? "??");
                    SetTextToRowNormal(table, 9, "Материнская плата", (cm.MotherBoard.Manufacturer ?? "??") + " " +(cm.MotherBoard.Model ?? "??"));
                    SetTextToRowNormal(table, 10, "Процессор", cm.Processor.Model ?? "??");
                    SetTextToRowNormal(table, 11, "RAM", (cm.MemoryCountMB.ToString() ?? "??" + "Мб"));
                    SetTextToRowNormal(table, 12, "Количество HDD", cm.HardDrive.Count.ToString());
                    SetTextToRowDouble(table, 13, "HDD1/SN", "");
                    SetTextToRowDouble(table, 14, "HDD2/SN", "");
                    SetTextToRowDouble(table, 15, "HDD3/SN", "");
                    int i = 13;
                    foreach (HardDrives h in cm.HardDrive)
                    {
                        table.Cell(i, 2).Range.Text = h.Model??"";
                        i++;
                    }
                    SetTextToRowNormal(table, 16, "Видеокарта", cm.VC.Model ?? "??");
                    SetTextToRowNormal(table, 17, "Модель клавиатуры", "");
                    SetTextToRowNormal(table, 18, "Модель мышки", "");
                    SetTextToRowNormal(table, 19, "Количество мониторов", "");
                    SetTextToRowDouble(table, 20, "Модель монитора 1/ИНВ", "");
                    SetTextToRowDouble(table, 21, "Модель монитора 2/ИНВ", "");
                    SetTextToRowDouble(table, 22, "Модель монитора 3/ИНВ", "");
                    SetTextToRowNormal(table, 23, "Модель ИБП", "");
                    SetTextToRowNormal(table, 24, "Операционная система", cm.OS.Title ?? "??");
                    SetTextToRowNormal(table, 25, "Офисный пакет:", "");
                    SetTextToRowNormal(table, 26, "СКЗИ: ", "");
                    SetTextToRowNormal(table, 27, "Установленное программное обеспечение:", "");
                    SetTextToRowNormal(table, 28, "Используемые информационные системы:", "");
                    table.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightAtLeast;
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
        private static void SetTextToRowNormal(Word.Table table, int idRow, string text1, string text2) //текст для строк
        {
            table.Cell(idRow, 1).Range.Text = text1;
            table.Cell(idRow, 2).Range.Text = text2 ?? "??";
            table.Cell(idRow, 2).Merge(table.Cell(idRow, 3));
        }
        private static void SetTextToRowDouble(Word.Table table, int idRow, string text1, string text2) //текст для строк
        {
            table.Cell(idRow, 1).Range.Text = text1;
            table.Cell(idRow, 2).Range.Text = text2 ?? "??";
        }
        #endregion
    }
}
