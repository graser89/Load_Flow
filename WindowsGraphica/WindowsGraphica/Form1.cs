using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace WindowsGraphica
{
    public partial class Form1 : Form
    {
        //public delegate void Nomer_Vetvi_Changed(Object sender, Chanche_nomer_Vetvi args);
        //public delegate void Nomer_Uzla_Changed(Object sender, Chanche_nomer_Uzla args);

        
        Shema shema1, shema2, shema3;

        //public event U_uzla_Change Izmenenie_naprig_uzla;


        public Form1()
        {
            
            
            InitializeComponent();
            
            tabControl1.TabPages[0].AutoScroll = true;


            shema1 = new Shema();        
            
            
            bindingSource1.DataSource = shema1.Uzli;
            bindingSource2.DataSource = shema1.Vetvi;
            
           // bindingSource6.DataSource = shema1.ARKTi;

            //Определяем размер холста
//            panel1.Size = new Size(pWidth, pHeight);
            

            //Поведение кнопки Add_Img_uzel
            
            //
        }
                       

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
             
        }

        #region XML

        string StartElemen = "XmlDocument_Graser";
        int version = 2;

        //Сохранение в XML файл
        private void SaveXml()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "XML files|*.xml";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs = new FileStream(filename, FileMode.Create);
            XmlTextWriter XmlOut = new XmlTextWriter(fs, Encoding.Unicode);

            XmlOut.Formatting = Formatting.Indented;

            //начало документа
            XmlOut.WriteStartDocument();
            XmlOut.WriteComment("Пример и отработка навыков сохранения ");
            XmlOut.WriteStartElement(StartElemen);
            XmlOut.WriteAttributeString("Version", version.ToString());

            

            shema1.SaveToXml(XmlOut);
            
            XmlOut.WriteEndElement();
            XmlOut.WriteEndDocument();
            XmlOut.Close();
            fs.Close();
        }

        //открытие из XML файла
        private void OpenXml(string filename)
        {
            //не забыть очистить обьекты перед открытием

            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlTextReader xmlIn = new XmlTextReader(fs);
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;

            //xmlIn.Read();
            xmlIn.MoveToContent();

            if (xmlIn.Name != StartElemen)
                throw new ArgumentException("Incorrect file format.");
            string version = xmlIn.GetAttribute(0);

            do
            {
                if (!xmlIn.Read())
                    throw new ArgumentException("Ошибочка");


                if ((xmlIn.NodeType == XmlNodeType.EndElement) &&
                    (xmlIn.Name == StartElemen))
                    break;

                if (xmlIn.NodeType == XmlNodeType.EndElement)
                    continue;

                if (xmlIn.Name == "Shema")
                {
                    shema1 = new Shema();
                    shema1.LoadFromFile(xmlIn);
                    bindingSource1.DataSource = shema1.Uzli;
                    bindingSource2.DataSource = shema1.Vetvi;
                    
                    //bindingSource6.DataSource = shema1.ARKTi;
                }                
                
                

/*                foreach (IXmlControl ITEM in nabor)
                {
                    if (xmlIn.Name == ITEM.XmlNameElement)
                    {
                        ITEM.LoadFromFile(xmlIn);
                    }
                }
 */ 

            } while (!xmlIn.EOF);

            
            xmlIn.Close();
            fs.Close();
            
        }


        //обработчик события щелчек по кнопке Открыть
        public void button_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML files|*.xml";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;


            OpenXml(filename);
            
            

        }

        //обработчик события щелчек по кнопке Сохранить
        public void button_Save_Click(object sender, EventArgs e)
        {
            if (!shema1.ProverkaUzlov())
            {
                MessageBox.Show("Ошибка в схеме есть узлы с одинаковыми номерами");
            }
            else 
                SaveXml();
        }


        #endregion

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void открытьЦДУToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "ЦДУ files|*.cdu";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;

            StreamReader reader= new StreamReader(filename,Encoding.Default);
            shema1 = new Shema();
            
            if (!shema1.LoadFromCDU(reader))
                MessageBox.Show("Ошибка чтения файла");
            else
            {
                filename = @filename.Substring(filename.LastIndexOf(@"\"), filename.Length - filename.LastIndexOf(@"\"));
                filename = filename.Substring(1, filename.Length - 1);
                this.Text = "Программа расчета УР.      Сейчас открыт файл   " + filename;
                bindingSource1.DataSource = shema1.Uzli;
                bindingSource2.DataSource = shema1.Vetvi;
                
            }
        }

        private void сохранитьЦДУToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "ЦДУ files|*.cdu";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            StreamWriter writer = new StreamWriter(filename);
            if (!shema1.SaveToCDU(writer))
            {
                MessageBox.Show("Ошибка записи файла");
            }

            writer.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            shema1.Raschet(true, true, true);
            //MessageBox.Show(shema1.Nomera_uzlov.Count.ToString());
            shema1.SaveMatrci_Y();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            
        }

        private void сравнитьРезультатыСЦДУToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "ЦДУ files|*.cdu";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;

            StreamReader reader = new StreamReader(filename, Encoding.Default);
            shema2 = new Shema();
            shema3 = new Shema();
            StreamReader reader1 = new StreamReader(filename, Encoding.Default);
            shema3.LoadFromCDU(reader1);
            if (!shema2.LoadFromCDU(reader))
                MessageBox.Show("Ошибка чтения файла");
            else
            {

                    shema3.Raschet(true, true, true);
                    double error_amplitude = Math.Abs(shema2.Uzli[0].UMod - shema3.Find_Uzel_by_Nomer(shema2.Uzli[0].NomerUzla).UMod);
                    double error_angle = Math.Abs(shema2.Uzli[0].Angle - shema3.Find_Uzel_by_Nomer(shema2.Uzli[0].NomerUzla).AngleDegree);
                    double er_Qgen=0, er_amp_average=0;
                    string er_amp, er_angle, er_Pbas=null, er_Qbas=null, er_Qgen_string, er_amp_av;
                    int kolvo_OGU = 0, kolvo_OGU1 = 0;
                    foreach (Uzel item in shema2.Uzli)
                    {
                        er_amp_average=er_amp_average+Math.Abs(item.UMod - shema3.Find_Uzel_by_Nomer(item.NomerUzla).UMod);
                        if (Math.Abs(item.UMod - shema3.Find_Uzel_by_Nomer(item.NomerUzla).UMod) > error_amplitude)
                            error_amplitude = Math.Abs(item.UMod - shema3.Find_Uzel_by_Nomer(item.NomerUzla).UMod);
                        if (Math.Abs(item.Angle - shema3.Find_Uzel_by_Nomer(item.NomerUzla).AngleDegree) > error_angle)
                            error_angle = Math.Abs(item.Angle - shema3.Find_Uzel_by_Nomer(item.NomerUzla).AngleDegree);
                        if (item.TipUzla == -1)
                        {
                            er_Pbas = Math.Abs(item.PGen - shema3.Find_Uzel_by_Nomer(item.NomerUzla).PGen).ToString();
                            er_Qbas = Math.Abs(item.QGen - shema3.Find_Uzel_by_Nomer(item.NomerUzla).QGen).ToString();
                        }
                        if (item.TipUzla == 1)
                        {
                            if (item.QGen <= item.QMax && item.QGen >= item.QMin)
                                kolvo_OGU++;
                            if (Math.Abs(item.QGen - shema3.Find_Uzel_by_Nomer(item.NomerUzla).QGen) > er_Qgen)
                                er_Qgen = Math.Abs(item.QGen - shema3.Find_Uzel_by_Nomer(item.NomerUzla).QGen);
                        }

                    }
                    foreach (Uzel item in shema3.Uzli)
                    {
                    if (item.TipUzla == 1 && item.QGen <= item.QMax && item.QGen >= item.QMin)
                            kolvo_OGU1++;
                    }
                    er_amp_average = er_amp_average / shema2.Uzli.Count;
                    er_amp_av = er_amp_average.ToString();
                    kolvo_OGU = Math.Abs(kolvo_OGU - kolvo_OGU1);
                    er_amp = error_amplitude.ToString();
                    er_angle = error_angle.ToString();
                    er_Qgen_string=er_Qgen.ToString();
                if (er_amp.Length > 6)
                        er_amp = er_amp.Substring(0, 6);
                    if (er_angle.Length > 6)
                        er_angle = er_angle.Substring(0, 6);
                    if (er_Pbas.Length > 6)
                        er_Pbas = er_Pbas.Substring(0, 6);
                    if (er_Qbas.Length > 6)
                        er_Qbas = er_Qbas.Substring(0, 6);
                    if (er_Qgen_string.Length > 6)
                        er_Qgen_string = er_Qgen_string.Substring(0, 6);
                    if (er_amp_av.Length > 6)
                        er_amp_av = er_amp_av.Substring(0, 6);
                    filename = @filename.Substring(filename.LastIndexOf(@"\"), filename.Length - filename.LastIndexOf(@"\"));
                    filename = filename.Substring(1, filename.Length - 1);
                    this.Text = "Программа расчета УР.      Сейчас открыт файл   " + filename;    
                    MessageBox.Show("Ниже приведена информация по следующему файлу:  " + filename + ".\n" +"\n"
                        + "Максимальная ошибка по напряжению составляет:                        " + er_amp + " кВ.\n"
                        + "Средняя ошибка по напряжению составляет:                                    " + er_amp_av + " кВ.\n"
                        + "Максимальная ошибка по углу составляет:                                        " + er_angle
                        + " гр.\n" + "Отличие по активной мощности в базисном узле составляет:       " + er_Pbas
                        + " МВт.\n" + "Отличие по реактивной мощности в базисном узле составляет:   " + er_Qbas
                        + " Мвар.\n" + "Отличие в количестве ОПОРНЫХ генераторных узлов составляет:     "+kolvo_OGU
                        + ".\n" + "Максимальная ошибка по Q_г в генераторных узлах составляет:  " + er_Qgen_string + " Мвар.");
            }
                
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigator3_RefreshItems(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem2_Click(object sender, EventArgs e)
        {  }

        private void bindingNavigatorAddNewItem5_Click(object sender, EventArgs e)
        {
            
        }

        private void bindingNavigatorAddNewItem5_Click_1(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
       

        

        



    }
}
