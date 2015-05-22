using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace WindowsGraphica
{
    public class Uzel :  IComparable
    {
        int nomer_uzla;
        string name_uzla;
        int _tipUzla;//-1 - базисный;  0 - нагрузочный;  1 - опорный;
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


        public Uzel()
        {
            name_uzla = "";
            _tipUzla = 0;
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
                        if (UZad != 0 & (QMin != 0 || QMax != 0))
                            _tipUzla = 1;
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
                            UMod = Proverka_pust_stroki(s, 16, 8);
                            //angle = Proverka_pust_stroki(s, 24, 8);
                            AngleDegree = Proverka_pust_stroki(s, 24, 8);
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

            str = UNom.ToString();
            s = s + Stroki(str);
            str = PLoad.ToString();
            s = s + Stroki(str);
            str = QLoad.ToString();
            s = s + Stroki(str);
            str = PGen.ToString();
            s = s + Stroki(str);
            str = QGen.ToString();
            s = s + Stroki(str);
            if (UZad != 0)
            {
                str = UZad.ToString();
                s = s + Stroki(str);
            }
            else
                s = s + "        ";
            str = QMin.ToString();
            s = s + Stroki(str);
            str = QMax.ToString();
            s = s + Stroki(str);
            s.Replace(",", ".");
            spisok.Add(s);
            if (((UMod != 0) & (Angle != 0)) || TipUzla == -1)
            {
                s = "0202    ";
                str = nomer_uzla.ToString();
                s = s + Stroki(str);
                str = UMod.ToString();
                s = s + Stroki(str);
                str = AngleDegree.ToString();
                s = s + Stroki(str);
                s.Replace(",", ".");
                spisok.Add(s);
            }
            if ((GSh != 0) || (BSh != 0))
            {
                s = "0301    ";
                str = nomer_uzla.ToString();
                s = s + Stroki(str);
                str = 0.ToString();
                s = s + Stroki(str);
                str = g_sh.ToString();
                s = s + Stroki(str);
                str = BSh.ToString();
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
            if (NameUzla != null)
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
                str = NameUzla.ToString();
                s = s + str;
            }
            return s;

        }

        #endregion

        #region Свойства

        public double PMaxGen
        {
            get { return p_max_gen; }
            set { p_max_gen = value; }
        }

        public double PMaxLoad
        {
            get { return p_max_load; }
            set { p_max_load = value; }
        }



        public int NomerUzla
        {
            get { return nomer_uzla; }
            set { nomer_uzla = value; }
        }


        public string NameUzla
        {
            get { return name_uzla; }
            set { name_uzla = value; }
        }


        public int TipUzla
        {
            get { return _tipUzla; }
            set { _tipUzla = value; }
        }


        public int TipCxn
        {
            get { return tip_cxn; }
            set { tip_cxn = value; }
        }


        public double UNom
        {
            get { return u_nom; }
            set { u_nom = value; }
        }


        public double UMod
        {
            get { return u_mod; }
            set 
            {                
                u_mod = value; 
            }
        }


        public double PLoad
        {
            get { return p_load; }
            set { p_load = value; }
        }


        public double QLoad
        {
            get { return q_load; }
            set { q_load = value; }
        }


        public double PGen
        {
            get { return p_gen; }
            set { p_gen = value; }
        }


        public double QGen
        {
            get { return q_gen; }
            set { q_gen = value; }
        }


        public double QMin
        {
            get { return q_min; }
            set { q_min = value; }
        }


        public double QMax
        {
            get { return q_max; }
            set { q_max = value; }
        }


        public double GSh
        {
            get { return g_sh; }
            set { g_sh = value; }
        }


        public double BSh
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


        public double AngleDegree
        {
            get { return angle_degree; }
            set 
            { 
                angle_degree = value;
                angle = value * Math.PI / 180;
            }
        }


        public double UZad
        {
            get { return u_zad; }
            set { u_zad = value; }
        }

        public double Q_load_max
        {
            get { return q_load_max; }
            set { q_load_max = value; }
        }


        public int NomerRaiona
        {
            get { return nomer_raiona_u; }
            set { nomer_raiona_u = value; }
        }

        #endregion

        #region XML
        public void SaveToXml(XmlTextWriter xmlOut)
        {
            xmlOut.WriteStartElement("Uzel");
            xmlOut.WriteAttributeString("nomer_uzla", nomer_uzla.ToString());
            xmlOut.WriteAttributeString("name_uzla", name_uzla.ToString());
            xmlOut.WriteAttributeString("tip_uzla", _tipUzla.ToString());
            xmlOut.WriteAttributeString("tip_cxn", tip_cxn.ToString());
            xmlOut.WriteAttributeString("U_nom", u_nom.ToString());
            xmlOut.WriteAttributeString("u_mod", u_mod.ToString());
            xmlOut.WriteAttributeString("p_load", p_load.ToString());
            xmlOut.WriteAttributeString("q_load", q_load.ToString());
            xmlOut.WriteAttributeString("p_gen", p_gen.ToString());
            xmlOut.WriteAttributeString("q_gen", q_gen.ToString());
            xmlOut.WriteAttributeString("q_min", q_min.ToString());
            xmlOut.WriteAttributeString("q_max", q_max.ToString());

            xmlOut.WriteAttributeString("g_sh", g_sh.ToString());
            xmlOut.WriteAttributeString("b_sh", b_sh.ToString());
            xmlOut.WriteAttributeString("angle", angle.ToString());
            xmlOut.WriteAttributeString("p_max_load", p_max_load.ToString());
            xmlOut.WriteAttributeString("p_max_gen", p_max_gen.ToString());
            xmlOut.WriteAttributeString("u_zad", u_zad.ToString());
            xmlOut.WriteAttributeString("nomer_raiona_u", nomer_raiona_u.ToString());
            xmlOut.WriteAttributeString("Q_load_max", Q_load_max.ToString());


            xmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                nomer_uzla = Convert.ToInt32(xmlIn.GetAttribute("nomer_uzla"));
                name_uzla = Convert.ToString(xmlIn.GetAttribute("name_uzla"));
                _tipUzla = Convert.ToInt32(xmlIn.GetAttribute("tip_uzla"));
                tip_cxn = Convert.ToInt32(xmlIn.GetAttribute("tip_cxn"));
                u_nom = Convert.ToDouble(xmlIn.GetAttribute("u_nom"));
                u_mod = Convert.ToDouble(xmlIn.GetAttribute("u_mod"));
                p_load = Convert.ToDouble(xmlIn.GetAttribute("p_load"));
                q_load = Convert.ToDouble(xmlIn.GetAttribute("q_load"));
                
                p_gen = Convert.ToDouble(xmlIn.GetAttribute("p_gen"));

                
                q_gen = Convert.ToDouble(xmlIn.GetAttribute("q_gen"));
                q_min = Convert.ToInt32(xmlIn.GetAttribute("q_min"));
                q_max = Convert.ToInt32(xmlIn.GetAttribute("q_max"));

                g_sh = Convert.ToDouble(xmlIn.GetAttribute("g_sh"));
                b_sh = Convert.ToDouble(xmlIn.GetAttribute("b_sh"));
                Angle = Convert.ToDouble(xmlIn.GetAttribute("angle"));
                
                p_max_load = Convert.ToDouble(xmlIn.GetAttribute("p_max_load"));
                p_max_gen = Convert.ToDouble(xmlIn.GetAttribute("p_max_gen"));
                
                u_zad = Convert.ToDouble(xmlIn.GetAttribute("u_zad"));
                nomer_raiona_u=Convert.ToInt16(xmlIn.GetAttribute("nomer_raiona_u"));         
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
            if (obje._tipUzla == -1)
            { return -1; }
            else
            {

                if (this._tipUzla == -1)
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


   
    
}
