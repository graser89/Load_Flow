using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsGraphica
{
    class config_imgUzel : IXmlControl
    {
        bool otobrash = false;
        int x_smech = 0;
        int y_smech = 0;
        string _font_name = "Helvetica";
        int _font_size = 10;
        String name_polia;

        #region Свойства
        public bool Otobrash
        {
            get { return otobrash; }
            set { otobrash = value; }
        }        

        public int X_smech
        {
            get { return x_smech; }
            set { x_smech = value; }
        }        

        public int Y_smech
        {
            get { return y_smech; }
            set { y_smech = value; }
        }

        public string Font_name
        {
            get { return _font_name; }
            set { _font_name = value; }
        }

        public int Font_size
        {
            get { return _font_size; }
            set { _font_size = value; }
        }

        public String Name_polia
        {
            get { return name_polia; }
            set { name_polia = value; }
        }
#endregion

        public config_imgUzel(String _Name_polia)
        {
            name_polia = _Name_polia;
        }

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("config_imgUzel");
            XmlOut.WriteAttributeString("otobrash", otobrash.ToString());
            XmlOut.WriteAttributeString("x_smech", x_smech.ToString());
            XmlOut.WriteAttributeString("y_smech", y_smech.ToString());
            XmlOut.WriteAttributeString("font_name", _font_name.ToString());
            XmlOut.WriteAttributeString("font_size", _font_size.ToString());
            XmlOut.WriteAttributeString("name_polia", name_polia.ToString());            

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                otobrash = Convert.ToBoolean(xmlIn.GetAttribute("otobrash"));
                x_smech = Convert.ToInt32(xmlIn.GetAttribute("x_smech"));
                y_smech = Convert.ToInt32(xmlIn.GetAttribute("y_smech"));
                _font_name = Convert.ToString(xmlIn.GetAttribute("font_name"));
                _font_size = Convert.ToInt32(xmlIn.GetAttribute("font_size"));
                //font = Convert.ToString(xmlIn.GetAttribute("font"));
                name_polia = Convert.ToString(xmlIn.GetAttribute("name_polia"));                
            }
            catch (Exception)
            { }
        }
        #endregion
    }

    //Класс изображение узла
    class imgUzel : IDraw, IXmlControl
    {

        int nomer_uzla;        
        int radius;
        int colorLine;
        int colorFona;
        int x;
        int y;
        bool Vidilenie=false;
        Uzel uzel;
        Point draggin_point;
        
        /*
         public static void SaveToXml()
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
            XmlOut.WriteComment("Пример и отработка навыков сохранения файла конфигурации ");
            XmlOut.WriteStartElement("Config");

            foreach (config_imgUzel item in config_list)
            {
                item.SaveToXml(XmlOut);
            }            

            XmlOut.WriteEndElement();
            XmlOut.WriteEndDocument();
            XmlOut.Close();
            fs.Close();
        }

        //открытие из XML файла
        public static void OpenXml(string filename)
        {
            //не забыть очистить обьекты перед открытием

            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlTextReader xmlIn = new XmlTextReader(fs);
            xmlIn.WhitespaceHandling = WhitespaceHandling.None;

            //xmlIn.Read();
            xmlIn.MoveToContent();

            if (xmlIn.Name != "Config")
                throw new ArgumentException("Incorrect file format.");
            string version = xmlIn.GetAttribute(0);

            do
            {
                if (!xmlIn.Read())
                    throw new ArgumentException("Ошибочка");


                if ((xmlIn.NodeType == XmlNodeType.EndElement) &&
                    (xmlIn.Name == "Config"))
                    break;

                if (xmlIn.NodeType == XmlNodeType.EndElement)
                    continue;

                if (xmlIn.Name == "config_imgUzel")
                {
                    config_imgUzel conf = new config_imgUzel("");
                    conf.LoadFromFile(xmlIn);
                    config_list.Add(conf);
                    //bindingSource1.DataSource = shema1.Uzli;
                }
                /*
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


                                foreach (IXmlControl ITEM in nabor)
                                {
                                    if (xmlIn.Name == ITEM.XmlNameElement)
                                    {
                                        ITEM.LoadFromFile(xmlIn);
                                    }
                                }
                 */
        /*
            } while (!xmlIn.EOF);


            xmlIn.Close();
            fs.Close();
        }

        public static void Init()
        {
            config_list = new List<config_imgUzel>();
        }
        */


        #region Свойства
        public int Nomer
        {
            get { return nomer_uzla; }
            set { nomer_uzla = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        #endregion

        public void connectUzel(Uzel _uzel)
        {
            uzel = _uzel;
        }

        public void Draw(Graphics g)
        {
            Color c = Color.FromArgb(colorLine);
            Pen p = new Pen(c, 2);
            Brush b = new SolidBrush(Color.FromArgb(colorFona));
            Pen p1 = new Pen(c, 1);

            g.DrawString(uzel.Name_uzla, SystemFonts.DefaultFont, SystemBrushes.WindowText, x, y + radius);
            g.DrawString(uzel.Nomer_uzla.ToString(), SystemFonts.DefaultFont, SystemBrushes.WindowText, x-3*radius, y - 3*radius);
            if (!Vidilenie)
            {
                //круг
                g.DrawEllipse(p, x - radius, y - radius, 2 * radius, 2 * radius);
                g.FillEllipse(b, x - radius, y - radius, 2 * radius, 2 * radius);
            }
            else
            {
                //квадрат
                g.DrawRectangle(p, x - radius, y - radius, 2 * radius, 2 * radius);
                g.FillRectangle(b, x - radius, y - radius, 2 * radius, 2 * radius);
            }
            if (uzel.P_load != 0)
            {
                g.DrawLine(p1, x, y + radius, x, y + radius + 8);
                g.DrawLine(p1, x, y + radius + 8, x + 3, y + radius + 2);
                g.DrawLine(p1, x, y + radius + 8, x - 3, y + radius + 2);
            }
            if ((uzel.P_gen != 0)||(uzel.Tip_uzla==-1))
            {
                g.DrawBezier(p1, x - 24, y, x - 22, y - 4, x - 18, y + 4, x - 16, y);
                g.DrawEllipse(p1, x - 26, y - 6, 12, 12);
                g.DrawLine(p1, x - 14, y, x - radius, y);
            }
        }

        public imgUzel() 
        {
            radius = 5;
            Color c = Color.FromArgb(0, 0, 255);
            colorLine = c.ToArgb();
            c = Color.FromArgb(255, 255, 0);
            colorFona = c.ToArgb();
        }

        public imgUzel(Uzel _uzel, Point p)
        {
            uzel = _uzel;
            nomer_uzla = uzel.Nomer_uzla;
            radius = 5;
            Color c = Color.FromArgb(0, 0, 255);
            colorLine = c.ToArgb();
            c = Color.FromArgb(255, 255, 0);
            colorFona = c.ToArgb();
            x = p.X;
            y = p.Y;
        }

        public imgUzel(Uzel _uzel, int X, int Y)
        {
            uzel = _uzel;
            nomer_uzla = uzel.Nomer_uzla;
            radius = 5;
            Color c = Color.FromArgb(0, 0, 255);
            colorLine = c.ToArgb();
            c = Color.FromArgb(255, 255, 0);
            colorFona = c.ToArgb();
            x = X;
            y = Y;
        }

        public imgUzel(S_imgUzel g)
        {
            nomer_uzla = g.Nomber;
            x = g.x;
            y = g.y;
            colorLine = g.colorLine;
            colorFona = g.colorFona;
            radius = g.radius;
        }  

        public void MoveTo(Point p)
        {
            x = p.X;
            y = p.Y;
        }

        public void MoveTo(int X, int Y)
        {
            x = X;
            y = Y;
        }

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("imgUzel");
            XmlOut.WriteAttributeString("nomer_uzla", nomer_uzla.ToString());
            XmlOut.WriteAttributeString("radius", radius.ToString());
            XmlOut.WriteAttributeString("colorLine", colorLine.ToString());
            XmlOut.WriteAttributeString("colorFona", colorFona.ToString());
            XmlOut.WriteAttributeString("x", x.ToString());
            XmlOut.WriteAttributeString("y", y.ToString());
            
            XmlOut.WriteEndElement();
        }        

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                nomer_uzla = Convert.ToInt32(xmlIn.GetAttribute("nomer_uzla"));
                radius = Convert.ToInt32(xmlIn.GetAttribute("radius"));
                colorLine = Convert.ToInt32(xmlIn.GetAttribute("colorLine"));
                colorFona = Convert.ToInt32(xmlIn.GetAttribute("colorFona"));
                x = Convert.ToInt32(xmlIn.GetAttribute("x"));
                y = Convert.ToInt32(xmlIn.GetAttribute("y"));                
            }
            catch (Exception)
            { }
        }
        #endregion

        // Определяет принадлежит ли данная точка этой фигуре
        public bool isXY(Point p)
        {
            bool t = false;
            if ((x - p.X) * (x - p.X) + (y - p.Y) * (y - p.Y) < (radius * radius))
                t = true;
            return t;
        }
        public bool isXY(int X, int Y)
        {
            bool t = false;
            if ((x - X) * (x - X) + (y - Y) * (y - Y) < (radius * radius))
                t = true;
            return t;
        }

        public void Videlenie()
        {
            //radius++;
            Vidilenie = true;
        }
        public void aVidelenie()
        {
            //radius--;
            Vidilenie = false;
        }
        public void DragPoin(Point p)
        {
            draggin_point = p;
        }
        public string ToString()
        {
            return "imguzel";
        }
        public int CompareTo(object obj)
        {
            if (obj.ToString()=="imguzel")
                return 0;
            if (obj.ToString()=="imgvetv")
                return -1;
            return 1;
        }

        //неверно
        


    }

    class imgVetv : IDraw, IXmlControl
    {
        int nomer;
        int tolshina;
        int colorLine;                
        bool Vidilenie = false;
        vetv vetv;
        imgUzel uzel_Nach;
        imgUzel uzel_Konca;
        GraphicsPath path;
        Point draggin_point;
        int _Length = 0;//длинна

        public int Length
        {
            get { return _Length; }
            set { _Length = value; }
        }


        public imgVetv()
        {
        }

        public imgVetv(imgUzel _uzel_n, imgUzel _uzel_k,vetv v)
        {
            uzel_Nach = _uzel_n;
            uzel_Konca = _uzel_k;
            Color c = Color.FromArgb(0, 0, 255);
            colorLine = c.ToArgb();
            tolshina=2;
            vetv = v;
            _Length = (int)Math.Sqrt((uzel_Nach.X - uzel_Konca.X) * (uzel_Nach.X - uzel_Konca.X) + (uzel_Nach.Y - uzel_Konca.Y) * (uzel_Nach.Y - uzel_Konca.Y));
            path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(uzel_Nach.X, uzel_Nach.Y, uzel_Konca.X, uzel_Konca.Y);
            
        }

        public void Draw(Graphics g)
        {           
            Pen p = new Pen( Color.FromArgb(colorLine),tolshina);
            if ((vetv.Kt1 + vetv.Kt2) != 0)
            {
                float Xc = (float)((uzel_Nach.X + uzel_Konca.X) / 2.0);
                float Yc = (float)((uzel_Nach.Y + uzel_Konca.Y) / 2.0);
                PointF point_c = new PointF(Xc, Yc);
                float length = (float)Math.Sqrt((uzel_Nach.X - uzel_Konca.X) * (uzel_Nach.X - uzel_Konca.X) + (uzel_Nach.Y - uzel_Konca.Y) * (uzel_Nach.Y - uzel_Konca.Y));
                PointF point_c1 = new PointF(Xc + (uzel_Nach.X - uzel_Konca.X) * 6 / length, Yc + (uzel_Nach.Y - uzel_Konca.Y) * 6 / length);
                PointF point_c2 = new PointF(Xc - (uzel_Nach.X - uzel_Konca.X) * 6 / length, Yc - (uzel_Nach.Y - uzel_Konca.Y) * 6 / length);
                PointF point_c3 = new PointF(Xc - (uzel_Nach.X - uzel_Konca.X) * 14 / length, Yc - (uzel_Nach.Y - uzel_Konca.Y) * 14 / length);
                PointF point_c4 = new PointF(Xc + (uzel_Nach.X - uzel_Konca.X) * 14 / length, Yc + (uzel_Nach.Y - uzel_Konca.Y) * 14 / length);
                g.DrawEllipse(p, point_c1.X - 8, point_c1.Y - 8, 16, 16);
                g.DrawEllipse(p, point_c2.X - 8, point_c2.Y - 8, 16, 16);

                g.DrawLine(p, uzel_Nach.X, uzel_Nach.Y, point_c4.X, point_c4.Y);
                g.DrawLine(p, point_c3.X, point_c3.Y, uzel_Konca.X, uzel_Konca.Y);
                g.DrawString(vetv.Nomer.ToString(), new Font("Helvetica", 10), Brushes.Black, Xc+10, Yc+10);
            }
            else
            {
                g.DrawLine(p, uzel_Nach.X, uzel_Nach.Y, uzel_Konca.X, uzel_Konca.Y);
                g.DrawString(vetv.Nomer.ToString(), new Font("Helvetica", 10), Brushes.Black, (uzel_Nach.X + uzel_Konca.X) / 2, (uzel_Nach.Y + uzel_Konca.Y) / 2);
            }
        }

        //присоединение ветви и изображенией узлов к изображению ветви
        public void connect(vetv _vetv, imgUzel _uzel_Nach, imgUzel _uzel_Konca)
        {
            vetv = _vetv;
            uzel_Nach = _uzel_Nach;
            uzel_Konca = _uzel_Konca;
            path = new GraphicsPath();
            path.AddLine(uzel_Nach.X, uzel_Nach.Y, uzel_Konca.X, uzel_Konca.Y);
        }

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("imgVetv");
            XmlOut.WriteAttributeString("nomer_uzla", nomer.ToString());
            XmlOut.WriteAttributeString("tolshina", tolshina.ToString());
            XmlOut.WriteAttributeString("colorLine", colorLine.ToString());            
            XmlOut.WriteAttributeString("vetv_nomer", vetv.Nomer.ToString());
            XmlOut.WriteAttributeString("uzel_Nach_nomer", uzel_Nach.Nomer.ToString());
            XmlOut.WriteAttributeString("uzel_Konca_nomer", uzel_Konca.Nomer.ToString());
            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn,out int _nomer_vetvi,out int _nomer_Uzla_nach,out int _nomer_Uzla_konca)
        {
            try
            {
                nomer = Convert.ToInt32(xmlIn.GetAttribute("nomer_uzla"));
                tolshina = Convert.ToInt32(xmlIn.GetAttribute("tolshina"));
                colorLine = Convert.ToInt32(xmlIn.GetAttribute("colorLine"));
                _nomer_vetvi = Convert.ToInt32(xmlIn.GetAttribute("vetv_nomer"));
                _nomer_Uzla_nach = Convert.ToInt32(xmlIn.GetAttribute("uzel_Nach_nomer"));
                _nomer_Uzla_konca = Convert.ToInt32(xmlIn.GetAttribute("uzel_Konca_nomer"));
            }
            catch (Exception)
            {
                _nomer_vetvi = 0;
                _nomer_Uzla_nach = 0;
                _nomer_Uzla_konca = 0;
            }

            
        }

        #endregion

        public bool isXY(Point p)
        {            
            
            if (path.IsVisible(p))
                return true;
            return false;
        }
        public bool isXY(int X, int Y)
        {
            if (path.IsOutlineVisible(X,Y, new Pen(Color.FromArgb(colorLine), (tolshina + 1))))
                return true;
            return false;
        }
        public void Videlenie()
        {
            //radius++;
            Vidilenie = true;
        }
        public void aVidelenie()
        {
            //radius--;
            Vidilenie = false;
        }
        public void MoveTo(Point p)
        {
            uzel_Nach.X = uzel_Nach.X + (p.X - draggin_point.X);
            uzel_Nach.Y = uzel_Nach.Y + (p.Y - draggin_point.Y);
            uzel_Konca.X = uzel_Konca.X + (p.X - draggin_point.X);
            uzel_Konca.Y = uzel_Konca.Y + (p.Y - draggin_point.Y);            
        }
        public void DragPoin(Point p)
        {
            draggin_point = p;
        }
        public string ToString()
        {
            return "imgvetv";
        }
        public int CompareTo(object obj)
        {
            if (obj.ToString() == "imguzel")
                return 1;
            if (obj.ToString() == "imgvetv")
                return 0;
            return -1;
        }

    }

}
