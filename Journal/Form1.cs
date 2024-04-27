using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        System.Drawing.Color warningColor = System.Drawing.Color.Pink; // ���� ������ � �������
        List<string> dwgFiles = new List<string>();
        List<Cabel> findedCabels = new List<Cabel>();

        bool shortPath = true;

        ToolStripMenuItem removeFile = new ToolStripMenuItem("������ ����");
        ToolStripMenuItem showFullPath = new ToolStripMenuItem("���������� ������ ���� � ������");
        ToolStripMenuItem showShortPath = new ToolStripMenuItem("���������� ������ �������� ������");
        ToolStripMenuItem removeAllFiles = new ToolStripMenuItem("�������� ������");

        public Form1()
        {
            InitializeComponent();

            showFullPath.Click += ShowFullPath_Click;
            showShortPath.Click += ShowShortPath_Click;
            removeFile.Click += RemoveFile_Click;
            removeAllFiles.Click += RemoveAllFiles_Click;

        }
        
        void But_selectFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "������� AutoCAD (*.dwg)|*.dwg";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
            // �������� ��������� ����
            
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

        void But_findCabels(object sender, EventArgs e) 
        {
            findedCabels.Clear();

            if(dwgFiles.Count == 0)
            {
                MessageBox.Show("�� ������� ����� ��� ������.");
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

            string message = $"����� ��������.\n\n������� �������: {findedCabels.Count}\n\n������� ���������� ������: {countOfErr}\n\n";
            if (unreadedFiles != "") message += $"�� ������� ������� �����:\n{unreadedFiles}\n���������, ��� ����� �� ������ ������� ���������� � �� ����������.";
            
            MessageBox.Show(message);

            if(findedCabels.Count > 0) button_ToExcel.Enabled = true;
        }

        void But_ToExcel_Click(object sender, EventArgs e)
        {
            string fileName;
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "����� Excel (*.xlsx)|*.XLSX";

                if (openFileDialog.ShowDialog() == DialogResult.Cancel) return;
                fileName = openFileDialog.FileName;
            }
                        
            try
            {
                WriteInExcel(findedCabels, fileName);
            }
            catch
            {
                MessageBox.Show($"�� ������� ��������� ������ � ����:\n{fileName}\n���������, ��� ���� �� ����� ������� ���������� � �� ��������.");
            }
        }

        // �������� ������������ ���� ������� � ���������� ������
        void ShowFullPath_Click(object? sender, EventArgs e)
        {
            shortPath = false;

            ShowCabelSources(dwgFiles);

            contextForTable1.Items.Remove(showFullPath);
            contextForTable1.Items.Add(showShortPath);            

        }
        void ShowShortPath_Click(object? sender, EventArgs e)
        {
            shortPath = true;

            ShowCabelSources(dwgFiles);

            contextForTable1.Items.Remove(showShortPath);
            contextForTable1.Items.Add(showFullPath);            
        }
        void RemoveFile_Click(object? sender, EventArgs e)
        {
            foreach (DataGridViewRow row in Table1.SelectedRows)
            {                
                Table1.Rows.RemoveAt(row.Index);
                string file = (string) row.Cells[1].Value;
                string markedFile = "";

                foreach(string str in dwgFiles)
                {
                    if(str.Contains(file)) markedFile = str;                    
                }
                dwgFiles.Remove(markedFile);
                ShowCabelSources(dwgFiles);
            }
        }
        void RemoveAllFiles_Click(object? sender, EventArgs e)
        {
            Table1.Rows.Clear();
            dwgFiles.Clear();

            contextForTable1.Items.Remove(removeFile);
            contextForTable1.Items.Remove(removeAllFiles);
            contextForTable1.Items.Remove(showFullPath);
            contextForTable1.Items.Remove(showShortPath);
        }
        //-----------------------------------------------------

        void FindCabel(string file, out bool unreadable) // ����� ������� � �����
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
                                   where kbl.HasAttributes && kbl.Attributes[0].Tag == "�����" && kbl.Attributes[1].Tag == "�����"
                                   select kbl;
                                
                foreach (Insert block in listOfBlocks)
                {
                    // ���������: 0 - �����; 1 - �����; 2 - ���; 3 - ����� ��� � �������; 4 - ����������� (������ ������ ����������� ������ ���� ��������� ���������)

                    bool exist = false;
                    string adr = "";

                    for (int j = 4; j < block.Attributes.Count; j++)
                    {
                        //�������� ������ �������� � ����� ������ ������.������� �� �����.������ ����������� ��� �������� � �����.
                        //string tmp = block.Attributes[j].Value;
                        //while (tmp.EndsWith(' '))
                        //{
                        //    tmp = tmp[..^2];
                        //}

                        adr += block.Attributes[j].Value + " ";
                    }

                    foreach (Cabel cabel in findedCabels)
                    {
                        if (cabel.mark == block.Attributes[0].Value && cabel.number == block.Attributes[1].Value)
                        {
                            string type = block.Attributes[2].Value.Contains(' ') ? block.Attributes[2].Value.Remove(' ') : block.Attributes[2].Value;
                            string pol = block.Attributes[3].Value.Contains(' ') ? block.Attributes[3].Value.Remove(' ') : block.Attributes[3].Value;

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

        void CheckErr(Cabel cabel) // ����������� ��������� �� ������ � ������, ��� �� �������.
        {
            string err = $"{cabel.mark}-{cabel.number} :\n";

            if (cabel.errType)
            {
                err += "\t�� ������, ������� ��� ������ ���� ������.\n";
            }

            if (cabel.errSize)
            {
                err += "\t�� ������, ������� ������ �������, ��� ������ ���������� ���.\n";
            }

            if (cabel.directionInfo.Count == 1)
            {
                if (cabel.directionInfo[0].direction == " ")
                {
                    err += $"\t� ������, � ����� {cabel.directionInfo[0].file}, �� ������� �� ������ �����������";
                        
                }
                else err += $"\t������� ������ ���� ����������� ������:\n\t\t- {cabel.directionInfo[0].direction} \t� �����: {cabel.directionInfo[0].file}";
                cabel.errMsg = err;

            }
            else if (cabel.directionInfo.Count == 2 && (cabel.errSize || cabel.errType))
            {
                err += $"\t��������� ����������� ������:\n\t\t- {cabel.directionInfo[0].direction} \t� �����: {cabel.directionInfo[0].file}\n\t\t- {cabel.directionInfo[1].direction} \t� �����: {cabel.directionInfo[1].file}";
                cabel.errMsg = err;
                    
            }
            else if (cabel.directionInfo.Count > 2)
            {
                err += $"\t������� ����� ���� ����������� ������:\n";
                foreach (var (direction, file) in cabel.directionInfo)
                {
                    err += $"\t\t- {direction} \t� �����: {file}\n";
                }
                cabel.errMsg = err;                    
            }
        }

        static void WriteInExcel(List<Cabel> cabels, string fileExcel)
        {
            var package = new ExcelPackage(new FileInfo(fileExcel));

            ExcelWorksheet ws = package.Workbook.Worksheets[0]; ;

            if (ws.ToString() == "_����������_")
            {
                int countLine = 3;

                foreach (var cabel in cabels)
                {
                    ws.Cells[$"D{countLine}"].Value = cabel.mark + "-" + cabel.number; // ����� + "-" + �����
                    ws.Cells[$"E{countLine}"].Value = cabel.typeOfC; // ��� ������
                    ws.Cells[$"F{countLine}"].Value = cabel.numOfPol; // ����� ��� � ������� ������

                    int cellMaxLength = 45;  //������������ ����� �������� � ������ ������
                    int count1 = 0;
                    int count2 = 0;

                    // ������ ���� ������
                    string tmpStr = cabel.directionInfo[0].direction;
                    while (tmpStr.Length > cellMaxLength) // ��������� ������ �� ������, ���� ����� ������ ������ ������
                    {
                        int index = tmpStr[..cellMaxLength].LastIndexOf(' ') >= (cellMaxLength - 4) ? tmpStr[..cellMaxLength].LastIndexOf(' ') + 1 : cellMaxLength;
                        ws.Cells[$"H{countLine + count1}"].Value = tmpStr[..index] + (tmpStr[..index].LastIndexOf(' ') == tmpStr[..index].Length - 1 ? "" : "-");
                        tmpStr = tmpStr[index..];
                        count1++;
                    }
                    ws.Cells[$"H{countLine + count1}"].Value = tmpStr;

                    // ���� ���� ������
                    if (cabel.directionInfo.Count > 1)
                    {
                        tmpStr = cabel.directionInfo[1].direction;
                        while (tmpStr.Length > cellMaxLength) // ��������� ������ �� ������, ���� ����� ������ ������ ������
                        {
                            int index = tmpStr[..cellMaxLength].LastIndexOf(' ') >= (cellMaxLength - 4) ? tmpStr[..cellMaxLength].LastIndexOf(' ') + 1 : cellMaxLength;
                            ws.Cells[$"I{countLine + count2}"].Value = tmpStr[..index] + (tmpStr[..index].LastIndexOf(' ') == tmpStr[..index].Length - 1 ? "" : "-");
                            tmpStr = tmpStr[index..];
                            count2++;
                        }
                        ws.Cells[$"I{countLine + count2}"].Value = tmpStr;
                    }

                    countLine += (count1 >= count2 ? count1 : count2) + 1; //������� �� ��������� ��������� ������ � Excel
                }

                package.Save();
                
                MessageBox.Show($"������ ������� �������� � ���� {fileExcel}");
            }
            else MessageBox.Show("� ��������� ����� Excel, �� ������� ����� ���� \"_����������_\".\n������ �� ���������."); 
        }

        void ShowCabelSources(List<string> sourses) // �����������, � �������1, ������ dwg ��������� ��� ������ �������
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
                if(shortPath)
                {
                    name = file[(file.LastIndexOf('\\')+1)..];
                } else { name = file; }

                DataGridViewCell nameOfFile = new DataGridViewTextBoxCell
                {
                    Value = name
                };
                row.Cells.AddRange(numberOfRow, nameOfFile);
                Table1.Rows.Add(row);
                countOfRows++;
            }

            if (!contextForTable1.Items.Contains(showShortPath) && !contextForTable1.Items.Contains(showFullPath)) contextForTable1.Items.Add(showFullPath);

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
                
        void ShowCabels() // ����������� ��������� ������� � �������2 
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

            tabControl1.SelectTab(tabPage2); // ������������ ������� �� ������� � ��������

        }
    }
}