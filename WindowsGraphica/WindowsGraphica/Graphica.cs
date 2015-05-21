using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace WindowsGraphica
{

 
    struct S_imgUzel
    {
        public int Nomber;
        public int x;
        public int y;
        public int radius;
        public int colorLine;
        public int colorFona;
    }



    //Класс графики который реализует основное поведение
    class Grahpica : IXmlControl
    {

        private string xmlNameElement = "Graphica";

       
        bool Add_Img_uzel;//режим работы
        List<IDraw> ris;
        List<imgUzel> imgUzlov;
        List<imgVetv> imgVetvei;
        List<IXmlControl> xmlControls;
        bool Selected;
        IDraw SelectObject;
        int nomer;//номер добавляемого узла
        Uzel u; //добавляемый узел
        List<vetv> dob_vetvi;//добовляемые ветви
        float scale = 1;
        bool dragging = false;
        IDraw DragObject;

        Shema shema;

        internal Shema Shema
        {
            get { return shema; }
            set { shema = value; }
        }

        //поля для связи с GUI
        DrawPanel panel;
        Button button;
        




        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public void Draw(Graphics g)
        {
            ris.Sort();
            foreach (IDraw item in ris)
                item.Draw(g);
        }

        private void Ochistka() 
        {
            ris = new List<IDraw>();
            imgUzlov = new List<imgUzel>();
            imgVetvei = new List<imgVetv>();
            xmlControls = new List<IXmlControl>();
            Add_Img_uzel = false;
            Selected = false;
            SelectObject = null;
            DragObject = null;
        }

        public Grahpica()
        {
            Ochistka();            
        }

     /*   public void Add(IDraw i)
        {
            ris.Add(i);
        }
*/


        public IDraw GetItemAt(Point p)
        {            
            foreach (IDraw q in ris)            
                if (q.isXY(p))                
                    return q;                                                            
            return null;
        }

        //добавление ветви
        public void Add(imgVetv _imgVetv)
        {
            imgVetvei.Add(_imgVetv);
            ris.Add(_imgVetv);
            xmlControls.Add(_imgVetv);
        }
        //добавление узла
        public void Add(imgUzel _imgUzel)
        {
            imgUzlov.Add(_imgUzel);
            ris.Add(_imgUzel);
            xmlControls.Add(_imgUzel);
        }
        //проверка если узла с указанным носером на изображении схемы нет то true тоесть проверка уникальности добавляемого узла
        private bool proverkaUzla(int _nomer)
        {            
            foreach (imgUzel item in imgUzlov)
            {
                if (item.Nomer == _nomer)
                    return false;                
            }
            return true;
        }

        private List<int> SpisokDobavlennihUzlov()
        {
            List<int> list = new List<int>();
            foreach (imgUzel item in imgUzlov)
            {
                list.Add(item.Nomer);
            }
           /* if (list.Count != 0)
                return list;
            else
                return null;
            */
            return list;
        }

        //возвращает изображение узла найденное по номеру
        private imgUzel Find_ImgUzla_by_nomer(int _nomer)
        {
            foreach (imgUzel u in imgUzlov)
            {
                if (u.Nomer == _nomer)
                    return u;
            }
            return null;
        }

        private List<imgVetv> Spisok_img_vetv(imgUzel uzel,Point p)
        {
            
            List<imgVetv> spis = new List<imgVetv>();
            if (dob_vetvi != null)
            {
                bool flag = true;
                foreach (vetv v in dob_vetvi)
                {
                    if (uzel.Nomer==v.Nomer_Uzla_Konca)
                    {
                        imgVetv vetka = new imgVetv(Find_ImgUzla_by_nomer(v.Nomer_Uzla_Nachal), uzel, v);
                        if (vetka.Length < 40)                        
                            flag = false;                        
                    }
                    if (uzel.Nomer == v.Nomer_Uzla_Nachal)
                    {
                        imgVetv vetka = new imgVetv(uzel,Find_ImgUzla_by_nomer(v.Nomer_Uzla_Konca),  v);
                        if (vetka.Length < 40)
                            flag = false;
                    }
                }
                if (!flag)
                {  }
            }

            return spis;
        }
        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {

            XmlOut.WriteStartElement(xmlNameElement);
            XmlOut.WriteAttributeString("Version", "1");
            XmlOut.WriteAttributeString("scale", scale.ToString());
            

            foreach (IXmlControl item in xmlControls)
            {
                item.SaveToXml(XmlOut);
            }
            if (xmlControls.Count == 0)
            {
                XmlOut.WriteStartElement("Null");
                XmlOut.WriteEndElement();
            }

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {               
            Scale = (float)Convert.ToDecimal(xmlIn.GetAttribute("scale"));

            do
            {                
                if (!xmlIn.Read())
                    //break;
                    throw new ArgumentException("Ошибочка");

                if ((xmlIn.NodeType == XmlNodeType.EndElement) &&
                       (xmlIn.Name == xmlNameElement))
                    break;

                if (xmlIn.NodeType == XmlNodeType.EndElement)
                    continue;

                //поправить не работает  
                if (xmlIn.Name == "imgUzel")
                {
                    imgUzel u = new imgUzel();                    
                    u.LoadFromFile(xmlIn);
                    Uzel uzel = shema.Find_Uzel_by_Nomer(u.Nomer);
                    u.connectUzel(uzel);
                    Add(u);
                }
                if (xmlIn.Name == "imgVetv")
                {
                    imgVetv v = new imgVetv();
                    int _nomer_vetv;
                    int _nomer_uzla_n;
                    int _nomer_uzla_k;
                    v.LoadFromFile(xmlIn, out _nomer_vetv, out _nomer_uzla_n, out _nomer_uzla_k);
                    imgUzel img_u_n = Find_ImgUzla_by_nomer(_nomer_uzla_n);
                    imgUzel img_u_k = Find_ImgUzla_by_nomer(_nomer_uzla_k);
                    vetv vetv = shema.Find_Vetv_by_Nomer(_nomer_vetv);
                    if ((vetv != null) & (img_u_n != null) & (img_u_k != null))
                    {
                        v.connect(vetv, img_u_n, img_u_k);
                        Add(v);
                    }
                }

            } while (!xmlIn.EOF);
        }

        #endregion

        #region Обработчики событий

        public void connectGui(DrawPanel panel1, Button button1)
        {
            button1.Click +=button_Add_Img_uzel_Click;
            panel1.MouseClick += panel1_MouseClick;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            panel1.Paint += panel1_Paint;
            panel = panel1;
            button = button1;            
        }

        public void disconnect()
        {
            button.Click -= button_Add_Img_uzel_Click;
            panel.MouseClick -= button_Add_Img_uzel_Click;
            panel.MouseClick -= panel1_MouseClick;
            panel.MouseDown -= panel1_MouseDown;
            panel.MouseMove -= panel1_MouseMove;
            panel.MouseUp -= panel1_MouseUp;
            panel.Paint -= panel1_Paint;
        }

        //обработчик события щелчек по кнопке Add_Img_uzel
        public void button_Add_Img_uzel_Click(object sender, EventArgs e)
        {
            Form2 Forma = new Form2();            
            List<int> spisok = shema.SpisokNedobavlennihUzlov(SpisokDobavlennihUzlov());
            if (spisok != null)
            {
                Forma.SpisokNedovalennihUzlov=spisok;
                
                if (Forma.ShowDialog() != DialogResult.OK)
                    return;
                
                u = shema.Find_Uzel_by_Nomer(Forma.Nomer);
                dob_vetvi = shema.Find_Nedobav_Vetv_po_Uzlu(SpisokDobavlennihUzlov(), Forma.Nomer);
                if (u != null)
                {
                    if (proverkaUzla(Forma.Nomer))
                    {
                        Add_Img_uzel = true;
                    }
                    else
                    {
                        MessageBox.Show("Такой узел уже расположен на схеме");
                    }
                }
                else
                {
                    MessageBox.Show("Узла  не существует");
                }

            }
            else
                 MessageBox.Show("Нет узлов для добавления");

        }

        //обработчик события щелчок по панели
        public void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Panel s = (Panel)sender;
            Point p = new Point((int)(e.X / scale), (int)(e.Y / scale));
            if (Add_Img_uzel)
            {
                imgUzel uzel = new imgUzel(u, p);
                
                List<imgVetv> spis = new List<imgVetv>();
                if (imgUzlov.Count == 0)
                {
                    Add(uzel);
                    Add_Img_uzel = false;
                    u = null;
                    s.Invalidate();
                }
                else
                {
                    if (dob_vetvi != null)
                    {
                        bool flag = true;
                        foreach (vetv v in dob_vetvi)
                        {
                            if (uzel.Nomer == v.Nomer_Uzla_Konca)
                            {
                                imgVetv vetka = new imgVetv(Find_ImgUzla_by_nomer(v.Nomer_Uzla_Nachal), uzel, v);
                                if (vetka.Length < 40)
                                    flag = false;
                                else
                                    spis.Add(vetka);
                            }
                            if (uzel.Nomer == v.Nomer_Uzla_Nachal)
                            {
                                imgVetv vetka = new imgVetv(uzel, Find_ImgUzla_by_nomer(v.Nomer_Uzla_Konca), v);
                                if (vetka.Length < 40)
                                    flag = false;
                                else
                                    spis.Add(vetka);
                            }
                        }
                        if (flag)
                        {
                            Add(uzel);
                            foreach (imgVetv item in spis)
                                Add(item);
                            Add_Img_uzel = false;
                            u = null;
                            dob_vetvi = null;
                            s.Invalidate();
                        }
                        else
                        {
                            uzel = null;
                            spis = null;
                        }
                    }
                    else
                    {
                        Add(uzel);
                        Add_Img_uzel = false;
                        u = null;
                        s.Invalidate();
                    }
                    
                }
                /*
                Add(uzel);
                Add_Img_uzel = false;
                u = null;
                s.Invalidate();
                 */ 
            }
            else
            {
                IDraw i = GetItemAt(p);
                if (i != null)
                {
                    //выделить
                    if (!Selected)
                    {                        
                        Selected = true;
                    }
                    else
                    {
                        SelectObject.aVidelenie();
                    }
                    SelectObject = i;
                    SelectObject.Videlenie();
                    s.Invalidate();
                }
                else 
                {
                    //снять выделение
                    if (Selected)
                    {
                        SelectObject.aVidelenie();
                        SelectObject = null;
                        Selected = false;
                        s.Invalidate();
                    }
                }
            }
        }

        //обработчик события нажатие клавиши по панели
        public void panel1_MouseDown(object sender, MouseEventArgs e)
        {            
            Point p = new Point((int)(e.X / scale), (int)(e.Y / scale));
            if (Selected)
            {
                IDraw i = GetItemAt(p);
                if (SelectObject.Equals(i))
                {                    
                    dragging = true;
                    DragObject = i;
                    i.DragPoin(p);
                }
            }
        }

        //обработчик события перемещения мыши по панели
        public void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Panel s = (Panel)sender;
            Point p = new Point((int)(e.X / scale), (int)(e.Y / scale));
            
            if (dragging)
            {
                DragObject.MoveTo(p);
                s.Invalidate();
            }
        }

        //обработчик события отпускания клавиши на панели
        public void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Point p = new Point((int)(e.X / scale), (int)(e.Y / scale));
            if (dragging)
            {
                DragObject = null;
                dragging = false;
            }
        }

        //обработчик события рисования на панели
        public void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.PageScale = Scale;
            Draw(e.Graphics);
        }

        #endregion
    }

    interface IDraw:IComparable
    {
        void Draw(Graphics g);
        bool isXY(Point p);
        bool isXY(int X, int Y);
        void Videlenie();
        void aVidelenie();
        void MoveTo(Point p);
        void DragPoin(Point p);
        int CompareTo(object obj);
    }

    interface IXmlControl
    {
        void SaveToXml(XmlTextWriter XmlOut);
        //void LoadFromFile(XmlTextReader xmlIn);
    }
}
