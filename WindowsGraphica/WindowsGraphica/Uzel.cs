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
    public class GrafikRaboti
    {

        //private double[] g = new double[24];
        List<double> g;
        int nomer;
        double max = 0;

        




        #region Cвойства
        public int Nomer
        {
            get { return nomer; }
            set { nomer = value; }
        }

        /*
        public double G1
        {
            get { return g[1]; }
            set { g[1] = value; }
        }
        public double G2
        {
            get { return g[2]; }
            set { g[2] = value; }
        }
        public double G3
        {
            get { return g[3]; }
            set { g[3] = value; }
        }
        public double G4
        {
            get { return g[4]; }
            set { g[4] = value; }
        }
        public double G5
        {
            get { return g[5]; }
            set { g[5] = value; }
        }
        public double G6
        {
            get { return g[6]; }
            set { g[6] = value; }
        }
        public double G7
        {
            get { return g[7]; }
            set { g[7] = value; }
        }
        public double G8
        {
            get { return g[8]; }
            set { g[8] = value; }
        }
        public double G9
        {
            get { return g[9]; }
            set { g[9] = value; }
        }
        public double G10
        {
            get { return g[10]; }
            set { g[10] = value; }
        }
        public double G11
        {
            get { return g[11]; }
            set { g[11] = value; }
        }
        public double G12
        {
            get { return g[12]; }
            set { g[12] = value; }
        }
        public double G13
        {
            get { return g[13]; }
            set { g[13] = value; }
        }
        public double G14
        {
            get { return g[14]; }
            set { g[14] = value; }
        }
        public double G15
        {
            get { return g[15]; }
            set { g[15] = value; }
        }
        public double G16
        {
            get { return g[16]; }
            set { g[16] = value; }
        }
        public double G17
        {
            get { return g[17]; }
            set { g[17] = value; }
        }
        public double G18
        {
            get { return g[18]; }
            set { g[18] = value; }
        }
        public double G19
        {
            get { return g[19]; }
            set { g[19] = value; }
        }
        public double G20
        {
            get { return g[20]; }
            set { g[20] = value; }
        }
        public double G21
        {
            get { return g[21]; }
            set { g[21] = value; }
        }
        public double G22
        {
            get { return g[22]; }
            set { g[22] = value; }
        }
        public double G23
        {
            get { return g[23]; }
            set { g[23] = value; }
        }
        public double G0
        {
            get { return g[0]; }
            set { g[0] = value; }
        }
        */
        public int count
        {
            get { return g.Count; }
        }
        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        #endregion

        #region Конструкторы
        public GrafikRaboti()
        {
            g = new List<double>();
        }

        public GrafikRaboti(int _nomer)
        {
            nomer = _nomer;
            g = new List<double>();
        }

        public GrafikRaboti(double[] _g, int razmer)
        {
            if (razmer == 24)
            {
                for (int i = 0; i < razmer; i++)
                {
                    g[i] = _g[i];
                }
            }
        }
        #endregion

        public bool LoadFromFile(StreamReader reader)
        {
            bool result = false;
            string stroka = "";
            int i = 0;
            string[] nabor;

            do
            {
                stroka = reader.ReadLine();
                if (stroka == null)
                    break;
                if (stroka.Split(';')[0] == "конец")
                    break;
                nabor = stroka.Split(';');
                //g[i] = Convert.ToDouble(nabor[0]);
                g.Add(Convert.ToDouble(nabor[0]));
                i++;
            }
            while (true);
            if (i >= 24)
            {
                max = 0;
                foreach (double item in g)
                {
                    if (max < item)
                        max = item;
                }
                result = true;
            }

            return result;
        }

        public double raschet_kf()
        {
            double res = 0;
            double summa_kv = 0;
            double summa = 0;
            foreach (double item in g)
            {
                summa_kv = summa_kv + item * item;
                summa = summa + item;
            }
            res = (summa_kv / (summa * summa / 24));
            return res;
        }

        private void maximum()
        {
            foreach (double item in g)
            {
                if (max < item)
                    max = item;
            }
            
        }

        public double otn_znac(int h)
        {
            if (h >= g.Count)
                return 0;
            if (max == 0)
                maximum();
            return g[h] / max;
        }

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("GrafikRaboti");
            XmlOut.WriteAttributeString("nomer", Nomer.ToString());
            XmlOut.WriteAttributeString("count", g.Count.ToString());

            //XmlOut.WriteStartElement("znachen");
            for (int i = 0; i < g.Count; i++)
            {
                XmlOut.WriteAttributeString("G"+i.ToString(), g[i].ToString());
            }

            //XmlOut.WriteEndElement();
            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                nomer = Convert.ToInt32(xmlIn.GetAttribute("nomer"));
                int count = Convert.ToInt32(xmlIn.GetAttribute("count"));
                for (int i = 0; i < count; i++)
                {
                    g.Add(Convert.ToDouble(xmlIn.GetAttribute("G" + i.ToString())));
                }
                maximum();
            }
            catch (Exception)
            { }
        }
        #endregion
    }



    public class Uzel : IXmlControl, IComparable
    {
        int nomer_uzla;
        string name_uzla;
        int tip_uzla;//-1 - базисный;  0 - нагрузочный;  1 - опорный;
        int tip_cxn;
        double u_nom;
        double u_mod;
        double p_load;
        double q_load;
        double p_gen;
        double q_gen;
        double q_min;
        double q_max;
        double g_sh;
        double b_sh;
        double angle;
        double angle_degree;
        double u_zad;
        double p_max_load;//поле отвечающее за мощность max при расчете по суточным графикам
        double p_max_gen;        
        int nomer_raiona_u = 0;
        double q_load_max;              
        double Q;
        GrafikRaboti _Grafik_P_load;
        GrafikRaboti _Grafik_P_Gen;
        GrafikRaboti _Grafik_Q_load;
        int nomer_Grafika_P_load;
        int nomer_Grafika_P_gen;
        int nomer_Grafika_Q_load;


        public delegate void Changed_U_mod_v_Uzle(Object sender, Chanche_U_mod_v_Uzle args);
        public event Changed_U_mod_v_Uzle U_mod_izmenilos;



        public Uzel()
        {
            name_uzla = "";
            tip_uzla = 0;
            tip_cxn = 0;
            angle = 0;
            g_sh = 0;
            b_sh = 0;
            p_max_load = 0;
        }

        #region CDU
        private string Stroki(string s)
        {
            int legth = s.Length;
            if (legth < 8)
            {
                for (int i = 0; i < (8 - legth); i++)
                {
                    s = " " + s;
                }
            }
            return s.Substring(0, 8);
        }

        private double Proverka_pust_stroki(string str, int Index, int Lenght)
        {
            double res = 0; ;
            const string pusto = "        ";
            if (str.Length >= (Index + Lenght))
                if (str.Substring(Index, Lenght) != pusto)
                    res = Convert.ToDouble(str.Substring(Index, Lenght));
                else res = 0;
            return res;
        }


        public bool LoadFromCDU(string s)
        {
            bool flag_res = false;
            switch (s.Substring(0, 4))
            {
                case "0201":
                    {
                        s = s.Replace(".", ",");
                        nomer_uzla = (int)Convert.ToDecimal(s.Substring(8, 8));
                        u_nom = Convert.ToDouble(s.Substring(16, 8));
                        p_load = Proverka_pust_stroki(s, 24, 8);
                        q_load = Proverka_pust_stroki(s, 32, 8);
                        p_gen = Proverka_pust_stroki(s, 40, 8);
                        q_gen = Proverka_pust_stroki(s, 48, 8);
                        u_zad = Proverka_pust_stroki(s, 56, 8);
                        q_min = (int)Proverka_pust_stroki(s, 64, 8);
                        q_max = (int)Proverka_pust_stroki(s, 72, 8);
                        if (U_zad != 0 & (Q_min != 0 || Q_max != 0))
                            tip_uzla = 1;
                        flag_res = true;
                    }
                    break;
                case "0202":
                    {
                        //0202 LLL узел U угол Рн Qн Qpасч
                        s = s.Replace(".", ",");
                        int nomer = (int)Convert.ToDecimal(s.Substring(8, 8));
                        if (nomer == nomer_uzla)
                        {
                            U_mod = Proverka_pust_stroki(s, 16, 8);
                            //angle = Proverka_pust_stroki(s, 24, 8);
                            Angle_degree = Proverka_pust_stroki(s, 24, 8);
                            flag_res = true;
                        }
                    }
                    break;
                case "0301":
                    {
                        int nomer = (int)Convert.ToDecimal(s.Substring(8, 8));
                        if (nomer == nomer_uzla)
                        {
                            if (s.Length <= 41)
                            {
                                s = s.Replace(".", ",");
                                g_sh = Proverka_pust_stroki(s, 24, 8);
                                b_sh = Proverka_pust_stroki(s, 32, 8);
                                flag_res = true;
                            }
                        }
                    }
                    break;
                case "0290":
                    {
                        int nomer = (int)Convert.ToDecimal(s.Substring(8, 8));
                        if (nomer == nomer_uzla)
                        {
                            name_uzla = s.Substring(16);
                            flag_res = true;
                        }
                    }
                    break;
                default:
                    flag_res = false;
                    break;
            }
            return flag_res;
        }

        //при сохранениии возвращаем список строк все равно их потом сортировать
        public List<string> SaveToCDU()
        {
            List<string> spisok = new List<string>();
            string s = "";
            //пока без схн
            s = "0201    ";
            string str = nomer_uzla.ToString();
            s = s + Stroki(str);

            str = U_nom.ToString();
            s = s + Stroki(str);
            str = P_load.ToString();
            s = s + Stroki(str);
            str = Q_load.ToString();
            s = s + Stroki(str);
            str = P_gen.ToString();
            s = s + Stroki(str);
            str = Q_gen.ToString();
            s = s + Stroki(str);
            if (U_zad != 0)
            {
                str = U_zad.ToString();
                s = s + Stroki(str);
            }
            else
                s = s + "        ";
            str = Q_min.ToString();
            s = s + Stroki(str);
            str = Q_max.ToString();
            s = s + Stroki(str);
            s.Replace(",", ".");
            spisok.Add(s);
            if (((U_mod != 0) & (Angle != 0)) || Tip_uzla == -1)
            {
                s = "0202    ";
                str = nomer_uzla.ToString();
                s = s + Stroki(str);
                str = U_mod.ToString();
                s = s + Stroki(str);
                str = Angle_degree.ToString();
                s = s + Stroki(str);
                s.Replace(",", ".");
                spisok.Add(s);
            }
            if ((G_sh != 0) || (B_sh != 0))
            {
                s = "0301    ";
                str = nomer_uzla.ToString();
                s = s + Stroki(str);
                str = 0.ToString();
                s = s + Stroki(str);
                str = g_sh.ToString();
                s = s + Stroki(str);
                str = B_sh.ToString();
                s = s + Stroki(str);
                s.Replace(",", ".");
                spisok.Add(s);
            }


            return spisok;
        }

        public string NameSaveToCDU()
        {
            string s = "";
            string str = "";
            if (Name_uzla != null)
            {
                s = "0290    ";
                str = nomer_uzla.ToString();
                int legth = str.Length;
                if (legth < 8)
                {
                    for (int i = 0; i < (8 - legth); i++)
                    {
                        str = str + " ";
                    }
                }
                str = str.Substring(0, 8);
                s = s + str;
                str = Name_uzla.ToString();
                s = s + str;
            }
            return s;

        }

        #endregion

        #region Свойства

        public double P_max_gen
        {
            get { return p_max_gen; }
            set { p_max_gen = value; }
        }

        public double P_max_load
        {
            get { return p_max_load; }
            set { p_max_load = value; }
        }



        public int Nomer_uzla
        {
            get { return nomer_uzla; }
            set { nomer_uzla = value; }
        }


        public string Name_uzla
        {
            get { return name_uzla; }
            set { name_uzla = value; }
        }


        public int Tip_uzla
        {
            get { return tip_uzla; }
            set { tip_uzla = value; }
        }


        public int Tip_cxn
        {
            get { return tip_cxn; }
            set { tip_cxn = value; }
        }


        public double U_nom
        {
            get { return u_nom; }
            set { u_nom = value; }
        }


        public double U_mod
        {
            get { return u_mod; }
            set 
            {
                if (U_mod_izmenilos != null)
                    U_mod_izmenilos(this, new Chanche_U_mod_v_Uzle(value));
                u_mod = value; 
            }
        }


        public double P_load
        {
            get { return p_load; }
            set { p_load = value; }
        }


        public double Q_load
        {
            get { return q_load; }
            set { q_load = value; }
        }


        public double P_gen
        {
            get { return p_gen; }
            set { p_gen = value; }
        }


        public double Q_gen
        {
            get { return q_gen; }
            set { q_gen = value; }
        }


        public double Q_min
        {
            get { return q_min; }
            set { q_min = value; }
        }


        public double Q_max
        {
            get { return q_max; }
            set { q_max = value; }
        }


        public double G_sh
        {
            get { return g_sh; }
            set { g_sh = value; }
        }


        public double B_sh
        {
            get { return b_sh; }
            set { b_sh = value; }
        }


        public double Angle
        {
            get { return angle; }
            set 
            { 
                angle = value;
                angle_degree = value * 180 / Math.PI;
            }
        }


        public double Angle_degree
        {
            get { return angle_degree; }
            set 
            { 
                angle_degree = value;
                angle = value * Math.PI / 180;
            }
        }


        public double U_zad
        {
            get { return u_zad; }
            set { u_zad = value; }
        }

        public double Q_load_max
        {
            get { return q_load_max; }
            set { q_load_max = value; }
        }


        public GrafikRaboti Grafik_P_load
        {
            get { return _Grafik_P_load; }
            set { _Grafik_P_load = value; }
        }

        public GrafikRaboti Grafik_P_Gen
        {
            get { return _Grafik_P_Gen; }
            set { _Grafik_P_Gen = value; }
        }



        public GrafikRaboti Grafik_Q_load
        {
            get { return _Grafik_Q_load; }
            set { _Grafik_Q_load = value; }
        }



        public int Nomer_Grafika_P_load
        {
            get { return nomer_Grafika_P_load; }
            set { nomer_Grafika_P_load = value; }
        }


        public int Nomer_Grafika_P_gen
        {
            get { return nomer_Grafika_P_gen; }
            set { nomer_Grafika_P_gen = value; }
        }



        public int Nomer_Grafika_Q_load
        {
            get { return nomer_Grafika_Q_load; }
            set { nomer_Grafika_Q_load = value; }
        }

        public int Nomer_raiona
        {
            get { return nomer_raiona_u; }
            set { nomer_raiona_u = value; }
        }

        #endregion

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("Uzel");
            XmlOut.WriteAttributeString("nomer_uzla", nomer_uzla.ToString());
            XmlOut.WriteAttributeString("name_uzla", name_uzla.ToString());
            XmlOut.WriteAttributeString("tip_uzla", tip_uzla.ToString());
            XmlOut.WriteAttributeString("tip_cxn", tip_cxn.ToString());
            XmlOut.WriteAttributeString("U_nom", u_nom.ToString());
            XmlOut.WriteAttributeString("u_mod", u_mod.ToString());
            XmlOut.WriteAttributeString("p_load", p_load.ToString());
            XmlOut.WriteAttributeString("q_load", q_load.ToString());
            XmlOut.WriteAttributeString("p_gen", p_gen.ToString());
            XmlOut.WriteAttributeString("q_gen", q_gen.ToString());
            XmlOut.WriteAttributeString("q_min", q_min.ToString());
            XmlOut.WriteAttributeString("q_max", q_max.ToString());

            XmlOut.WriteAttributeString("g_sh", g_sh.ToString());
            XmlOut.WriteAttributeString("b_sh", b_sh.ToString());
            XmlOut.WriteAttributeString("angle", angle.ToString());
            XmlOut.WriteAttributeString("Nomer_Grafika_P_load", nomer_Grafika_P_load.ToString());
            XmlOut.WriteAttributeString("Nomer_Grafika_P_gen", nomer_Grafika_P_gen.ToString());
            XmlOut.WriteAttributeString("p_max_load", p_max_load.ToString());
            XmlOut.WriteAttributeString("p_max_gen", p_max_gen.ToString());
            XmlOut.WriteAttributeString("u_zad", u_zad.ToString());
            XmlOut.WriteAttributeString("nomer_raiona_u", nomer_raiona_u.ToString());
            XmlOut.WriteAttributeString("Nomer_Grafika_Q_load", Nomer_Grafika_Q_load.ToString());
            XmlOut.WriteAttributeString("Q_load_max", Q_load_max.ToString());


            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                nomer_uzla = Convert.ToInt32(xmlIn.GetAttribute("nomer_uzla"));
                name_uzla = Convert.ToString(xmlIn.GetAttribute("name_uzla"));
                tip_uzla = Convert.ToInt32(xmlIn.GetAttribute("tip_uzla"));
                tip_cxn = Convert.ToInt32(xmlIn.GetAttribute("tip_cxn"));
                u_nom = Convert.ToDouble(xmlIn.GetAttribute("U_nom"));
                u_mod = Convert.ToDouble(xmlIn.GetAttribute("u_mod"));
                p_load = Convert.ToDouble(xmlIn.GetAttribute("p_load"));
                q_load = Convert.ToDouble(xmlIn.GetAttribute("q_load"));
                
                p_gen = Convert.ToDouble(xmlIn.GetAttribute("p_gen"));

                
                q_gen = Convert.ToDouble(xmlIn.GetAttribute("q_gen"));
                q_min = Convert.ToInt32(xmlIn.GetAttribute("q_min"));
                q_max = Convert.ToInt32(xmlIn.GetAttribute("q_max"));
/*
                if ((q_min > -10) && (q_min < 0))
                    q_min = -10;
                if ((q_max < 10) && (q_max > 0))
                    q_max = 10;
 */ 
                g_sh = Convert.ToDouble(xmlIn.GetAttribute("g_sh"));
                b_sh = Convert.ToDouble(xmlIn.GetAttribute("b_sh"));
                Angle = Convert.ToDouble(xmlIn.GetAttribute("angle"));
                nomer_Grafika_P_load = Convert.ToInt32(xmlIn.GetAttribute("Nomer_Grafika_P_load"));
                nomer_Grafika_P_gen = Convert.ToInt32(xmlIn.GetAttribute("Nomer_Grafika_P_gen"));
                p_max_load = Convert.ToDouble(xmlIn.GetAttribute("p_max_load"));
                p_max_gen = Convert.ToDouble(xmlIn.GetAttribute("p_max_gen"));
                
                u_zad = Convert.ToDouble(xmlIn.GetAttribute("u_zad"));
                nomer_raiona_u=Convert.ToInt16(xmlIn.GetAttribute("nomer_raiona_u"));
                Nomer_Grafika_Q_load = Convert.ToInt16(xmlIn.GetAttribute("Nomer_Grafika_Q_load"));                
                Q_load_max = Convert.ToDouble(xmlIn.GetAttribute("Q_load_max"));

                if (p_gen != 0)
                    p_gen = p_max_gen;
            }
            catch (Exception)
            { }
        }
        #endregion


        public int CompareTo(object obj)
        {
            Uzel obje = (Uzel)obj;
            if (obje.tip_uzla == -1)
            { return -1; }
            else
            {

                if (this.tip_uzla == -1)
                {
                    return 1;
                }
                else
                {
                    if (this.u_nom == obje.u_nom)
                    { return 0; }
                    else
                    {
                        if (this.u_nom < obje.u_nom)
                            return 1;
                        else
                            return -1;
                    }
                }
            }

        }

    }


    public class Chanche_U_mod_v_Uzle : EventArgs
    {
        public Chanche_U_mod_v_Uzle(double new_U_mod_v_Uzle)
        {
            New_U_mod_v_Uzle = new_U_mod_v_Uzle;

        }

        public double New_U_mod_v_Uzle { get; set; }
    }


    class RPN:Iconnect_vetv
    {
        int _Nomer;
        //int _Chislo_Otpaik; //число в каждую сторону от 0    
        int nomer_min_otpaiki;
        int nomer_max_otpaiki;        
        int _Nomer_Otpaiki;//номер со знаком но не больше чем _Chislo_Otpaik
        double _Shag_Otpaik;
        double _U_regulir;
        double _U_neregulir;
        //decimal _U_Gelaemoe;
        int nomer_vetvi;

        vetv vetv;


        #region Свойства
        public int Nomer
        {
            get { return _Nomer; }
            set { _Nomer = value; }
        }
        public int Nomer_min_otpaiki
        {
            get { return nomer_min_otpaiki; }
            set { nomer_min_otpaiki = value; }
        }
        public int Nomer_max_otpaiki
        {
            get { return nomer_max_otpaiki; }
            set { nomer_max_otpaiki = value; }
        }
        public int Nomer_Otpaiki
        {
            get { return _Nomer_Otpaiki; }
            set { _Nomer_Otpaiki = value; }
        }

        public double Shag_Otpaik
        {
            get { return _Shag_Otpaik; }
            set { _Shag_Otpaik = value; }
        }

        public double U_regulir
        {
            get { return _U_regulir; }
            set { _U_regulir = value; }
        }

        public double U_neregulir
        {
            get { return _U_neregulir; }
            set { _U_neregulir = value; }
        }

        public int Nomer_vetvi
        {
            get { return nomer_vetvi; }
            set 
            {
                if (Nomer_Vetvi_Izmenen != null)
                    Nomer_Vetvi_Izmenen(this, new Chanche_nomer_Vetvi(value));
                nomer_vetvi = value;
            }
        }
        

       

        #endregion

        public RPN()
        {
            _Shag_Otpaik = 0.0178;
            nomer_max_otpaiki = 9;
            nomer_min_otpaiki = -9;
            _Nomer_Otpaiki = 0;

        }


        //_U_deistvitelnoe - Это напряжение на регулироемой стороне 
        public double Opredelenie_Kt()
        {
            double res = (_U_regulir + Nomer_Otpaiki * _Shag_Otpaik * _U_regulir) / (_U_neregulir);
            return res;
        }

        private void Nomer_vverh()
        {
            if (Nomer_Otpaiki < nomer_max_otpaiki)            
                Nomer_Otpaiki++;           
        }

        private void Nomer_vniz()
        {
            if (Nomer_Otpaiki > nomer_min_otpaiki)            
                Nomer_Otpaiki--;      
      
        }

        public void Izmenit_otpaiku(int i)
        {
            _Nomer_Otpaiki += i;
            if (_Nomer_Otpaiki < nomer_min_otpaiki)
                _Nomer_Otpaiki = nomer_min_otpaiki;
            if (_Nomer_Otpaiki > nomer_max_otpaiki)
                _Nomer_Otpaiki = nomer_max_otpaiki;
            vetv.Kt1 = Opredelenie_Kt();
        }

        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("RPN");
            XmlOut.WriteAttributeString("Nomer", Nomer.ToString());
            XmlOut.WriteAttributeString("Nomer_min_otpaiki", Nomer_min_otpaiki.ToString());
            XmlOut.WriteAttributeString("Nomer_max_otpaiki", Nomer_max_otpaiki.ToString());
            XmlOut.WriteAttributeString("Nomer_Otpaiki", Nomer_Otpaiki.ToString());
            XmlOut.WriteAttributeString("Shag_Otpaik", Shag_Otpaik.ToString());
            XmlOut.WriteAttributeString("U_regulir", U_regulir.ToString());
            XmlOut.WriteAttributeString("U_neregulir", U_neregulir.ToString());
            //XmlOut.WriteAttributeString("U_Gelaemoe", U_Gelaemoe.ToString());
            XmlOut.WriteAttributeString("Nomer_vetvi", vetv.Nomer.ToString());

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                Nomer = Convert.ToInt32(xmlIn.GetAttribute("Nomer"));
                Nomer_min_otpaiki = Convert.ToInt32(xmlIn.GetAttribute("Nomer_min_otpaiki"));
                Nomer_max_otpaiki = Convert.ToInt32(xmlIn.GetAttribute("Nomer_max_otpaiki"));
                Nomer_Otpaiki = Convert.ToInt32(xmlIn.GetAttribute("Nomer_Otpaiki"));
                Shag_Otpaik = Convert.ToDouble(xmlIn.GetAttribute("Shag_Otpaik"));
                U_regulir = Convert.ToDouble(xmlIn.GetAttribute("U_regulir"));
                U_neregulir = Convert.ToDouble(xmlIn.GetAttribute("U_neregulir"));
                Nomer_vetvi = Convert.ToInt32(xmlIn.GetAttribute("Nomer_vetvi"));
                
            }
            catch (Exception)
            { }
        }

        public delegate void Nomer_Vetvi_Changed(Iconnect_vetv sender, Chanche_nomer_Vetvi args);
        public event Nomer_Vetvi_Changed Nomer_Vetvi_Izmenen;

        public void connect(vetv v)
        {
            vetv = v;
        }
    }
}
