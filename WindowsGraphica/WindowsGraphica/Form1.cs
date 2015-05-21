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

        DrawPanel panel1;
        Grahpica g;
        Shema shema1, shema2, shema3;
        int pWidth = 800;
        int pHeight = 800;
        Automatic_RN ARN;
        ARKT AR;
        Uzel u;
        //public event U_uzla_Change Izmenenie_naprig_uzla;


        public Form1()
        {
            
            g = new Grahpica();
            // 
            // panel1
            //            
            this.panel1 = new DrawPanel();
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.TabIndex = 0;
            panel1.Holst_width = pWidth;
            panel1.Holst_height = pHeight;
            panel1.Scale(g.Scale);
            
            panel1.AutoSize = false;


            InitializeComponent();
            this.tabPage1.Controls.Add(this.panel1);
            tabControl1.TabPages[0].AutoScroll = true;


            shema1 = new Shema();        
            
            ARN = new Automatic_RN(shema1);
            AR = new ARKT(shema1);
 
            bindingSource1.DataSource = shema1.Uzli;
            bindingSource2.DataSource = shema1.Vetvi;
            bindingSource3.DataSource = shema1.Grafiki;
            bindingSource4.DataSource = shema1.Rpni;
            bindingSource5.DataSource = ARN.Tochki_Kontrola;
           // bindingSource6.DataSource = shema1.ARKTi;

            //Определяем размер холста
//            panel1.Size = new Size(pWidth, pHeight);
            

            //Поведение кнопки Add_Img_uzel
            g.Shema = shema1;
            g.connectGui(panel1, button1);

            

            
            //
            button2.Click += button_Save_Click;
            button3.Click += button_Open_Click;
        }
                       

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
             g.Scale = (float)numericUpDown1.Value / 100;
             panel1.Scale(g.Scale);
             panel1.Invalidate();
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
            g.SaveToXml(XmlOut);
            panel1.SaveToXml(XmlOut);
            ARN.SaveToXml(XmlOut);

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
                    bindingSource3.DataSource = shema1.Grafiki;
                    bindingSource4.DataSource = shema1.Rpni;
                    //bindingSource6.DataSource = shema1.ARKTi;
                }                
                
                if (xmlIn.Name == "Graphica")
                {
                    g.disconnect();
                    g = new Grahpica();
                    g.Shema = shema1;
                    g.LoadFromFile(xmlIn);                    
                }

                if (xmlIn.Name == "DrawPanel")
                {
                    panel1 = new DrawPanel();
                    panel1.LoadFromFile(xmlIn);
                    this.tabPage1.Controls.RemoveAt(0);
                    this.tabPage1.Controls.Add(this.panel1);
                    g.connectGui(panel1, button1);
                    panel1.Scale(g.Scale);
                }

                if (xmlIn.Name == "Automatic_RN")
                {
                    ARN = new Automatic_RN(shema1);
                    ARN.LoadFromFile(xmlIn);
                    bindingSource5.DataSource = ARN.Tochki_Kontrola;
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
            AR = new ARKT(shema1);
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
            
            panel1.Invalidate();

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
                g.Shema = shema1;
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
            shema1.Raschet(Flat_Start.Checked, Start_Algorithm.Checked, polar_SK.Checked);
            //MessageBox.Show(shema1.Nomera_uzlov.Count.ToString());
            shema1.SaveMatrci_Y();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            tabPage1.Refresh();
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

                    shema3.Raschet(Flat_Start.Checked, Start_Algorithm.Checked, polar_SK.Checked);
                    double error_amplitude = Math.Abs(shema2.Uzli[0].U_mod - shema3.Find_Uzel_by_Nomer(shema2.Uzli[0].Nomer_uzla).U_mod);
                    double error_angle = Math.Abs(shema2.Uzli[0].Angle - shema3.Find_Uzel_by_Nomer(shema2.Uzli[0].Nomer_uzla).Angle_degree);
                    double er_Qgen=0, er_amp_average=0;
                    string er_amp, er_angle, er_Pbas=null, er_Qbas=null, er_Qgen_string, er_amp_av;
                    int kolvo_OGU = 0, kolvo_OGU1 = 0;
                    foreach (Uzel item in shema2.Uzli)
                    {
                        er_amp_average=er_amp_average+Math.Abs(item.U_mod - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).U_mod);
                        if (Math.Abs(item.U_mod - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).U_mod) > error_amplitude)
                            error_amplitude = Math.Abs(item.U_mod - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).U_mod);
                        if (Math.Abs(item.Angle - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).Angle_degree) > error_angle)
                            error_angle = Math.Abs(item.Angle - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).Angle_degree);
                        if (item.Tip_uzla == -1)
                        {
                            er_Pbas = Math.Abs(item.P_gen - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).P_gen).ToString();
                            er_Qbas = Math.Abs(item.Q_gen - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).Q_gen).ToString();
                        }
                        if (item.Tip_uzla == 1)
                        {
                            if (item.Q_gen <= item.Q_max && item.Q_gen >= item.Q_min)
                                kolvo_OGU++;
                            if (Math.Abs(item.Q_gen - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).Q_gen) > er_Qgen)
                                er_Qgen = Math.Abs(item.Q_gen - shema3.Find_Uzel_by_Nomer(item.Nomer_uzla).Q_gen);
                        }

                    }
                    foreach (Uzel item in shema3.Uzli)
                    {
                    if (item.Tip_uzla == 1 && item.Q_gen <= item.Q_max && item.Q_gen >= item.Q_min)
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

        private void расчетАдрессностиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);

            shema1.Raschet(false, false, true);
            Digraph di = new Digraph(shema1);
            di.Formirov_Matrix_A();
            di.save(fs);
            fs.Close();

            fs.Close();
            fs1.Close();
        }

        private void расчетАдресностиПоПодсистемамаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);

            shema1.Raschet(false, false, true);
            shema1.Opredelenie_rainov();
            foreach (int i in shema1.Nomera_raionov)
            {
                Digraph_subsystem di = new Digraph_subsystem(shema1, i);
                di.Raschet_A();
                di.Save(fs);
            }

            fs.Close();
            fs1.Close();
        }

        private void адресностьПоЧасамИПодсистемамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);
            shema1.Opredelenie_rainov();
            List<dinamic_mass_d> A_gener = new List<dinamic_mass_d>();
            List<dinamic_mass_d> A_nagr = new List<dinamic_mass_d>();
            List<dinamic_mass_d> A_post = new List<dinamic_mass_d>();//постовлялось
            List<dinamic_mass_d> A_per = new List<dinamic_mass_d>();//передано
            List<dinamic_mass_d> A_pot = new List<dinamic_mass_d>();
            List<dinamic_mass_d> A_pot_rai = new List<dinamic_mass_d>();
            for (int i = 0; i < shema1.Nomera_raionov.Count; i++)
            {
                A_gener.Add(new dinamic_mass_d(shema1.Nomera_raionov,"Поступление система №"+shema1.Nomera_raionov[i].ToString()));
                A_nagr.Add(new dinamic_mass_d(shema1.Nomera_raionov, "Отпуск системы №" + shema1.Nomera_raionov[i].ToString()));
                A_post.Add(new dinamic_mass_d("Передавалось по системе №" + shema1.Nomera_raionov[i].ToString()));
                A_per.Add(new dinamic_mass_d("Передано по системе №" + shema1.Nomera_raionov[i].ToString()));
                A_pot.Add(new dinamic_mass_d("Потери в системе №" + shema1.Nomera_raionov[i].ToString()));
                A_pot_rai.Add(new dinamic_mass_d("Потери в системе №" + shema1.Nomera_raionov[i].ToString()+" при прердачи по районам"));
            }

            for (int h = 0; h < 24; h++)
            {
                shema1.Rascet_Regima_po_Grafikam(h);

                fs.WriteLine("/////");
                fs.WriteLine("/////");
                fs.WriteLine("///// час номер "+h.ToString());
                fs.WriteLine("/////");
                fs.WriteLine("/////");
                foreach (int i in shema1.Nomera_raionov)
                {
                    Digraph_subsystem di = new Digraph_subsystem(shema1, i);
                    di.Raschet_A();
                    di.Save(fs);
                    A_gener[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_generator1);
                    A_nagr[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_nagr1);
                    A_post[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_peredano1);
                    A_per[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_postupilo1);
                    A_pot[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_poteri1);
                    A_pot_rai[shema1.Nomera_raionov.IndexOf(i)].Add_dinamic_mass_d(di.A_poteri_raion1);
                }
            }


            fs.WriteLine("/////");
            fs.WriteLine("/////");
            fs.WriteLine("///// Cумма " );
            fs.WriteLine("/////");
            fs.WriteLine("/////");
            for (int i = 0; i < shema1.Nomera_raionov.Count; i++)
            {
                A_gener[i].Save(fs);
                A_pot_rai[i].Save(fs);
            }

            /*
            foreach (int i in shema1.Nomera_raionov)
            {
                A_gener[shema1.Nomera_raionov.IndexOf(i)].Save(fs);

            }
            */


            fs.Close();
            fs1.Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigator3_RefreshItems(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;

            GrafikRaboti rab = new GrafikRaboti(shema1.Grafiki.Count);
            StreamReader reader = new StreamReader(filename);
            rab.LoadFromFile(reader);
            bindingSource3.Add(rab);
            //shema1.Grafiki.Add(rab);
            //bindingSource3.DataSource = shema1.Grafiki;
            
            bindingNavigator3.Update();
            dataGridView3.Update();
            panel1.Invalidate();
            //dataGridView3.ind
        }

        private void bindingNavigatorAddNewItem2_Click(object sender, EventArgs e)
        {  }

        private void bindingNavigatorAddNewItem3_Click(object sender, EventArgs e)
        {
            RPN r = new RPN();
            r.Nomer_Vetvi_Izmenen += new RPN.Nomer_Vetvi_Changed(shema1.Nomer_Vetvi_Changed);
            bindingSource4.Add(r);
        }

        private void расчетУРСАРНПоГрафикамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);

            int min=shema1.Grafiki[0].count;
            foreach (GrafikRaboti g in shema1.Grafiki)
                if (g.count < min)
                    min = g.count;

            fs.Write("time ;");
            foreach (Uzel u in shema1.Uzli)
            {
                fs.Write(u.Nomer_uzla.ToString() + ";");
            }
            fs.WriteLine();
            ARN.Start();

            for (int h = 0; h < min; h++)
            {
                shema1.Rascet_Regima_po_Grafikam(h);                
                fs.Write(h.ToString() + ";");
                foreach (Uzel u in shema1.Uzli)
                {
                    fs.Write(u.U_mod.ToString() + ";");
                }
                fs.Write(shema1.Rpni[0].Nomer_Otpaiki.ToString() + ";");
                fs.WriteLine();
                ARN.Add_time();
                label1.Text = h.ToString();
                
            }
            ARN.Stop();

            fs.Close();
            fs1.Close();

        }

        private void bindingNavigatorAddNewItem5_Click(object sender, EventArgs e)
        {
            
        }

        private void расчетУРСАРКТПоГрафикамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "csv files|*.csv";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;
            FileStream fs1 = new FileStream(filename, FileMode.CreateNew);
            StreamWriter fs = new StreamWriter(fs1, System.Text.Encoding.Default);

            int min = shema1.Grafiki[0].count;
            foreach (GrafikRaboti g in shema1.Grafiki)
                if (g.count < min)
                    min = g.count;

            fs.Write("time ;");
            foreach (Uzel u in shema1.Uzli)
            {
                fs.Write(u.Nomer_uzla.ToString() + ";");
            }
            fs.WriteLine();

            AR.Nomer_Vetvi = 2;


            for (int h = 0; h < min; h++)
            {
                shema1.Rascet_Regima_po_Grafikam(h);

                fs.Write(h.ToString() + ";");
                foreach (Uzel u in shema1.Uzli)
                {
                    fs.Write(u.U_mod.ToString() + ";");
                }
                fs.Write(shema1.Rpni[0].Nomer_Otpaiki.ToString() + ";");
                fs.Write((shema1.Vetvi[1].I_Nach * 1000).ToString() + ";");
                fs.WriteLine();

                AR.Add_time();

                if (AR.Is_Izmenen())
                {
                    shema1.Rpni[0].Izmenit_otpaiku(AR.Izmenenie_otpaiki1);
                }                
                label1.Text = h.ToString();
            }

            
            fs.Close();
            fs1.Close();

        }

        private void bindingNavigatorAddNewItem5_Click_1(object sender, EventArgs e)
        {


        }
       

        

        



    }
}
