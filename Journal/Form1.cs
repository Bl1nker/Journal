using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using ACadSharp;
using ACadSharp.Blocks;
using ACadSharp.Entities;
using ACadSharp.IO;
using OfficeOpenXml;

namespace Journal
{
    
    public partial class Form1 : Form
    {
        readonly System.Drawing.Color warningColor = System.Drawing.Color.Pink; // Цвет для выделеления ошибки в таблице
        List<string> dwgFiles = new List<string>(); // список файлов, в которых производится поиск
        List<Cabel> findedCabels = new List<Cabel>(); // список найденных кабелей

        bool shortPath = true; // определяет будет ли указываться только название файлов или полный путь

        ToolStripMenuItem removeFile = new ToolStripMenuItem("Убрать файл");
        ToolStripMenuItem showFullPath = new ToolStripMenuItem("Показывать полный путь к файлам");
        ToolStripMenuItem showShortPath = new ToolStripMenuItem("Показывать только названия файлов");
        ToolStripMenuItem removeAllFiles = new ToolStripMenuItem("Очистить список");

        public Form1()
        {
            InitializeComponent();
            
            showFullPath.Click += ShowFullPath_Click;
            showShortPath.Click += ShowShortPath_Click;
            removeFile.Click += RemoveFile_Click;
            removeAllFiles.Click += RemoveAllFiles_Click;
        }
        
        void But_selectFiles_Click(object sender, EventArgs e) // выбор файлов для поиска кабелей
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Чертежи AutoCAD (*.dwg)|*.dwg";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;

            // запись всех выбранных файлов в список

            string[] fileName = openFileDialog.FileNames;
            
            foreach (string file in fileName)
            {
                if(!dwgFiles.Contains(file))
                {
                    dwgFiles.Add(file);
                }
            }
            
            ShowCabelSources(dwgFiles);           
        }

        void But_findCabels(object sender, EventArgs e) // Поиск кабелей в файлах
        {
            findedCabels.Clear();

            if(dwgFiles.Count == 0)
            {
                MessageBox.Show("Не выбраны файлы для поиска.");
                return;
            }

            string unreadedFiles = "";
            foreach(string file in dwgFiles)
            {
                FindCabel(file, out bool fileUnreadable);

                if (fileUnreadable)
                {
                    unreadedFiles += file + "\n";
                    foreach(DataGridViewRow row in Table1.Rows)
                    {
                        if(file.Contains((string)row.Cells[1].Value)) row.DefaultCellStyle.BackColor = warningColor;
                    }
                }
            }
            foreach (Cabel cabel in findedCabels)
            {
                CheckErr(cabel);
            }

            ShowCabels();

            int countOfErr = findedCabels.Where(n => n.errMsg != null).Count();

            string message = $"Поиск завершен.\n\nКабелей найдено: {findedCabels.Count}\n\nКабелей содержащих ошибки: {countOfErr}\n\n";
            if (unreadedFiles != "") message += $"Не удалось открыть файлы:\n{unreadedFiles}\nУбедитесь, что файлы не заняты другими процессами и не повреждены.";
            
            MessageBox.Show(message);

            if(findedCabels.Count > 0) button_ToExcel.Enabled = true;
        }

        void But_ToExcel_Click(object sender, EventArgs e) // Выбор файла Excel с последующей записью в него кабелей
        {
            string fileName;
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Файлы Excel (*.xlsx)|*.XLSX";

                if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
                fileName = openFileDialog.FileName;
            }
                        
            try
            {
                WriteInExcel(findedCabels, fileName);
            }
            catch
            {
                MessageBox.Show($"Не удалось выполнить запись в файл:\n{fileName}\nУбедитесь, что файл не занят другими процессами и не повреждён.");
            }
        }

        // Элементы контекстного меню таблицы с названиями файлов
        void ShowFullPath_Click(object? sender, EventArgs e) // нажатие на "Показывать полный путь к файлам"
        {
            shortPath = false;

            ShowCabelSources(dwgFiles);

            contextForTable1.Items.Remove(showFullPath);
            contextForTable1.Items.Add(showShortPath);            

        }
        void ShowShortPath_Click(object? sender, EventArgs e) // нажатие на "Показывать только название файлов"
        {
            shortPath = true;

            ShowCabelSources(dwgFiles);

            contextForTable1.Items.Remove(showShortPath);
            contextForTable1.Items.Add(showFullPath);            
        }
        void RemoveFile_Click(object? sender, EventArgs e) // нажатие на "Убрать файл"
        {
            foreach (DataGridViewRow row in Table1.SelectedRows)
            {                
                Table1.Rows.RemoveAt(row.Index);
                dwgFiles = dwgFiles.Where( x => !x.Contains((string)row.Cells[1].Value)).ToList();
                ShowCabelSources(dwgFiles);
            }
        }
        void RemoveAllFiles_Click(object? sender, EventArgs e) // нажатие на "Очистить список"
        {
            Table1.Rows.Clear();
            dwgFiles.Clear();

            contextForTable1.Items.Remove(removeFile);
            contextForTable1.Items.Remove(removeAllFiles);
            contextForTable1.Items.Remove(showFullPath);
            contextForTable1.Items.Remove(showShortPath);
        }
        //-----------------------------------------------------

        void FindCabel(string file, out bool unreadable) // Поиск кабелей в полученном файле. Возвращает true, если при чтении файла возникла ошибка
        {
            CadDocument doc;
            
            try
            {
                using DwgReader reader = new DwgReader(file);
                doc = reader.Read();
            } catch 
            {
                unreadable = true;
                return;
            }

            unreadable = false;


            if (doc.BlockRecords.Contains("kabelz"))
            {
                var listOfBlocks = from blc in doc.Entities
                                   where blc is Insert
                                   select blc as Insert
                                     into kbl
                                   where kbl.HasAttributes && kbl.Attributes[0].Tag == "МАРКА" && kbl.Attributes[1].Tag == "НОМЕР"
                                   select kbl;
                                
                foreach (Insert block in listOfBlocks)
                {
                    // Аттрибуты: 0 - марка; 1 - номер; 2 - тип; 3 - число жил и сечение; 4 - направление (каждая строка направления кабеля идет отдельным атрибутом)

                    bool exist = false;
                    string adr = "";

                    for (int j = 4; j < block.Attributes.Count; j++)
                    {
                        adr += block.Attributes[j].Value.Trim(' ') + " ";
                    }

                    foreach (Cabel cabel in findedCabels)
                    {
                        if (cabel.mark == block.Attributes[0].Value && cabel.number == block.Attributes[1].Value)
                        {
                            string type = block.Attributes[2].Value.Trim(' ');
                            string pol = block.Attributes[3].Value.Trim(' ');

                            if (cabel.typeOfC != type) cabel.errType = true;
                            if (cabel.numOfPol != pol) cabel.errSize = true;

                            if (!cabel.directionInfo.Any(t => t.direction == adr)) cabel.directionInfo.Add((adr, file[(file.LastIndexOf('\\') + 1)..]));
                            exist = true;
                        }
                    }
                    if (!exist) findedCabels.Add(new Cabel(block.Attributes[0].Value, block.Attributes[1].Value, block.Attributes[2].Value, block.Attributes[3].Value, adr, file[(file.LastIndexOf('\\') + 1)..]));
                } 
            }
        }

        void CheckErr(Cabel cabel) // Проверка кабеля на наличие ошибок и последующее составление сообщения об ошибке.
        {
            StringBuilder err = new StringBuilder();
            err.Append($"{cabel.mark}-{cabel.number} :\n");

            if (cabel.errType)
            {
                err.Append("\tНа схемах, указаны два разных типа кабеля\n");
            }

            if (cabel.errSize)
            {
                err.Append("\tНа схемах, указаны разные сечения, или разное количество жил.\n");
            }

            if (cabel.directionInfo.Count == 1)
            {
                if (cabel.directionInfo[0].direction == " ")
                {
                    err.Append($"\tУ кабеля, в файле {cabel.directionInfo[0].file}, не указано ни одного направления");
                        
                }
                else err.Append($"\tУказано только одно направление кабеля:\n\t\t- {cabel.directionInfo[0].direction} \tв файле: {cabel.directionInfo[0].file}");
                cabel.errMsg = err.ToString();

            }
            else if (cabel.directionInfo.Count == 2 && (cabel.errSize || cabel.errType))
            {
                err.Append($"\tУказанные направления кабеля:\n\t\t- {cabel.directionInfo[0].direction} \tв файле: {cabel.directionInfo[0].file}\n\t\t- {cabel.directionInfo[1].direction} \tв файле: {cabel.directionInfo[1].file}");
                cabel.errMsg = err.ToString();
                    
            }
            else if (cabel.directionInfo.Count > 2)
            {
                err.Append($"\tУказано более двух направлений кабеля:\n");
                foreach (var (direction, file) in cabel.directionInfo)
                {
                    err.Append($"\t\t- {direction} \tв файле: {file}\n");
                }
                cabel.errMsg = err.ToString();                    
            }
        }

        static void WriteInExcel(List<Cabel> cabels, string fileExcel)
        {
            var package = new ExcelPackage(new FileInfo(fileExcel));

            ExcelWorksheet ws = package.Workbook.Worksheets[0]; ;

            if (ws.ToString() == "_ЗАПОЛНЕНИЕ_")
            {
                int countLine = 3;

                foreach (var cabel in cabels)
                {
                    ws.Cells[$"D{countLine}"].Value = cabel.mark + "-" + cabel.number; // марка + "-" + номер
                    ws.Cells[$"E{countLine}"].Value = cabel.typeOfC; // тип кабеля
                    ws.Cells[$"F{countLine}"].Value = cabel.numOfPol; // число жил и сечение кабеля

                    int cellMaxLength = 45;  //максимальное число символов в ячейке адреса, для перехода на следующую строку
                    int count1 = 0;
                    int count2 = 0;

                    // откуда идет кабель
                    string tmpStr = cabel.directionInfo[0].direction;
                    while (tmpStr.Length > cellMaxLength) // Переход на новую строку в написании адреса, если адрес больше ширины ячейки
                    {
                        int index = tmpStr[..cellMaxLength].LastIndexOf(' ') >= (cellMaxLength - 4) ? tmpStr[..cellMaxLength].LastIndexOf(' ') + 1 : cellMaxLength;
                        ws.Cells[$"H{countLine + count1}"].Value = tmpStr[..index] + (tmpStr[..index].LastIndexOf(' ') == tmpStr[..index].Length - 1 ? "" : "-");
                        tmpStr = tmpStr[index..];
                        count1++;
                    }
                    ws.Cells[$"H{countLine + count1}"].Value = tmpStr;

                    // куда идет кабель
                    if (cabel.directionInfo.Count > 1)
                    {
                        tmpStr = cabel.directionInfo[1].direction;
                        while (tmpStr.Length > cellMaxLength) // Переход на новую строку в написании адреса, если адрес больше ширины ячейки
                        {
                            int index = tmpStr[..cellMaxLength].LastIndexOf(' ') >= (cellMaxLength - 4) ? tmpStr[..cellMaxLength].LastIndexOf(' ') + 1 : cellMaxLength;
                            ws.Cells[$"I{countLine + count2}"].Value = tmpStr[..index] + (tmpStr[..index].LastIndexOf(' ') == tmpStr[..index].Length - 1 ? "" : "-");
                            tmpStr = tmpStr[index..];
                            count2++;
                        }
                        ws.Cells[$"I{countLine + count2}"].Value = tmpStr;
                    }

                    countLine += (count1 >= count2 ? count1 : count2) + 1; //переход на следующую свободную строку в Excel
                }

                package.Save();
                
                MessageBox.Show($"Кабели успешно записаны в файл {fileExcel}");
            }
            else MessageBox.Show("В указанном файле Excel, не удалось найти лист \"_ЗАПОЛНЕНИЕ_\".\nЗапись не выполнена."); 
        }

        void ShowCabelSources(List<string> sourses) // Отображение, в таблице1, файлов dwg выбранных для поиска кабелей
        {
            Table1.Rows.Clear();
            
            int countOfRows = 1;
            foreach(string file  in sourses)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewCell numberOfRow = new DataGridViewTextBoxCell
                {
                    Value = countOfRows
                };

                string name;
                // Отображение полного пути к файлу или только названия файла
                if (shortPath)
                {
                    name = file[(file.LastIndexOf('\\') + 1)..];
                }
                else { name = file; }

                DataGridViewCell nameOfFile = new DataGridViewTextBoxCell
                {
                    Value = name
                };
                row.Cells.AddRange(numberOfRow, nameOfFile);
                Table1.Rows.Add(row);
                countOfRows++;
            }

            // Добавляем, в контекстное меню, строку "Показывать полный путь к файлу" Если не выбрано ничего другого.
            if (!contextForTable1.Items.Contains(showShortPath) && !contextForTable1.Items.Contains(showFullPath)) contextForTable1.Items.Add(showFullPath);

            // Добавление или удаление строк из контакстного меню, в зависимости от наличия элементов в таблице
            if (Table1.Rows.Count != 0)
            {
                contextForTable1.Items.Add(removeFile);                
                contextForTable1.Items.Add(removeAllFiles);
            }
            else
            {
                contextForTable1.Items.Remove(removeFile);
                contextForTable1.Items.Remove(removeAllFiles);
            }
        }
                
        void ShowCabels() // Отображение найденных кабелей в таблице2 
        {
            int count = 1;
            Table2.Rows.Clear();

            foreach (Cabel cabel in findedCabels)
            {
                DataGridViewRow row = new DataGridViewRow();

                DataGridViewCell number = new DataGridViewTextBoxCell
                {
                    Value = count                                 
                };

                DataGridViewCell mark = new DataGridViewTextBoxCell
                {
                    Value = $"{cabel.mark}-{cabel.number}"                    
                };

                DataGridViewCell typeOfCab = new DataGridViewTextBoxCell
                {
                    Value = cabel.typeOfC
                };

                DataGridViewCell size = new DataGridViewTextBoxCell
                {
                    Value = cabel.numOfPol
                };
                DataGridViewCell adr1 = new DataGridViewTextBoxCell
                {

                    Value = cabel.directionInfo.Count > 0 ? cabel.directionInfo[0].direction : ""
                };

                DataGridViewCell adr2 = new DataGridViewTextBoxCell
                {
                    Value = cabel.directionInfo.Count > 1 ? cabel.directionInfo[1].direction : ""
                };

                if (cabel.errMsg != null)
                {
                    number.ToolTipText = cabel.errMsg;
                    number.Style.BackColor = warningColor;
                    
                    mark.ToolTipText = cabel.errMsg;
                    mark.Style.BackColor = warningColor;

                    if(cabel.errType)
                    {                        
                        typeOfCab.Style.BackColor = warningColor;
                    }

                    if(cabel.errSize)
                    {                        
                        size.Style.BackColor= warningColor;
                    }

                    if(cabel.directionInfo.Count == 1) 
                    { 
                        adr2.Style.BackColor = System.Drawing.Color.Yellow;
                    }

                    if(cabel.directionInfo[0].direction == " " || cabel.directionInfo.Count > 2)
                    {
                        adr1.Style.BackColor = warningColor;
                        adr2.Style.BackColor = warningColor;
                    }
                    
                }
                
                row.Cells.AddRange(number, mark, typeOfCab, size, adr1, adr2);
                
                Table2.Rows.Add(row);
                count++;
            }

            tabControl1.SelectTab(tabPage2); // Переключение вкладки на таблицу с кабелями
        }
    }
}