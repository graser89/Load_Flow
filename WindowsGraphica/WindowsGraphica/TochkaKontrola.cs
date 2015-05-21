using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGraphica;
using System.IO;
using System.Xml;

namespace WindowsGraphica
{
    public class Chanche_nomer_Uzla:EventArgs
    {
        public Chanche_nomer_Uzla(int New_nomer_Uzla)
        {
            New_Nomer_Uzla = New_nomer_Uzla;
 
        }

        public int New_Nomer_Uzla { get; set; }
    }

    public class Chanche_nomer_Vetvi : EventArgs
    {
        public Chanche_nomer_Vetvi(int New_nomer_vetvi)
        {
            New_Nomer_Vetvi = New_nomer_vetvi;

        }

        public int New_Nomer_Vetvi { get; set; }
    }

    public interface Iconnect_vetv
    {
        void connect(vetv v);
    }

    public interface Iconnect_uzel
    {
        void connect(Uzel u);
    }

    public class TochkaKontrola:Iconnect_uzel
    {
        List<double> Arhiv;
        int _nomer_uzla;
        int nomer;

        public int Nomer
        {
            get { return nomer; }
            set { nomer = value; }
        }
        
        Uzel connect_uzel;
        double u_verh;
        double u_niz;
        double u_nom;
       
        double h;//шаг приведения
        double chustvitelnost = 1.0;        
        int Truble;
        int _Nominal_moshnost;


        public double Chustvitelnost
        {
            get { return chustvitelnost; }
            set { chustvitelnost = value; }
        }
        public int Nominal_moshnost
        {
            get { return _Nominal_moshnost; }
            set { _Nominal_moshnost = value; }
        }

        public double U_verh
        {
            get { return u_verh; }
            set { u_verh = value; }
        }

        public double U_niz
        {
            get { return u_niz; }
            set { u_niz = value; }
        }

        public double U_nom
        {
            get { return u_nom; }
            set { u_nom = value; }
        }
        public int Nomer_uzla
        {
            get { return _nomer_uzla; }
            set
            {
                if (Nomer_Uzla_Izmenen != null)
                    Nomer_Uzla_Izmenen(this, new Chanche_nomer_Uzla(value));
                _nomer_uzla = value;
            }

        }

        public delegate void Nomer_Uzla_Changed(Iconnect_uzel sender, Chanche_nomer_Uzla args);
        public event Nomer_Uzla_Changed Nomer_Uzla_Izmenen;

        public TochkaKontrola()
        {
            Arhiv = new List<double>();
            u_nom = 380;
            u_verh = u_nom * 1.05;
            u_niz = u_nom * 1.00;
            Truble = 1;
            h = 0;
            _Nominal_moshnost = 250;
        }

        

        public void connect(Uzel u)
        {
            connect_uzel = u;
 
        }

        public void Start()
        {
            connect_uzel.U_mod_izmenilos += new Uzel.Changed_U_mod_v_Uzle(Reaction_na_Izmenenie_U_mod);
        }

        private void Reaction_na_Izmenenie_U_mod(Object sender, Chanche_U_mod_v_Uzle args)
        {
            if (Arhiv.Count > 0)
            {
                double U_1 = Arhiv[Arhiv.Count - 1] + h;
                double U_2 = args.New_U_mod_v_Uzle;
                double delta_U = Math.Abs((U_1 - U_2) / u_nom) * 100.0;
                if (delta_U >= chustvitelnost)
                {
                    h += U_2- U_1;
                }
                Arhiv.Add(U_2 - h);


            }
            else
            {
                Arhiv.Add(args.New_U_mod_v_Uzle);
                h = 0; 
            }

            
        }

        public void Stop()
        {
            connect_uzel.U_mod_izmenilos -= new Uzel.Changed_U_mod_v_Uzle(Reaction_na_Izmenenie_U_mod);
        }

        public void raschet(ref List<double> nabor, List<double> znachen1, List<double> znachen2)
        {
            nabor = new List<double>();
            int razmer1 = znachen1.Count;
            int razmer2 = znachen2.Count;
            for (int i = 0; i < razmer1 - razmer2 + 1; i++)
            {
                double[] x = znachen2.ToArray();
                double[] y = znachen1.GetRange(i, razmer2).ToArray();
                double znach = alglib.pearsoncorrelation(x, y, razmer2);
                nabor.Add(znach);
            }
        }

        public bool Proval_perenapr(ref List<double> U, double raznost, double U_Nom)
        {            
            int Pl = 0;
            int Pr = 0;
            int n = U.Count;
            for (int i = 0; i < U.Count; i++)
            {
                U[i] += raznost;
                bool remove = false;
                if (U[i] < 0.9 * U_Nom)
                {
                    Pl++;
                    U.RemoveAt(i);
                    remove = true;
                }
                else
                    if (U[i] > 1.1 * U_Nom)
                    {
                        Pr++;
                        U.RemoveAt(i);
                        remove = true;
                    }
                if (remove)
                    i--;
            }
            if ((Pl + Pr) * 100.0 / n < 5)
            {
                return true;
            }
            else
                return false;
        }

        public List<int> Zdop(List<double> U_i, int Z_max, int Z_min, double dZ, int n, double Uv, double Un)
        {
            List<int> rez = new List<int>();
            List<double> T = new List<double>();
            double Tz = 0;
            int T11 = 0;
            int T12 = 0;
            for (int z = Z_min; z <= Z_max; z++)
            {
                T11 = 0;
                T12 = 0;
                foreach (double U in U_i)
                {
                    if (U*(1 + dZ * z )> Uv)
                        T11++;
                    else
                        if (U*(1 + dZ * z )< Un)
                            T12++;
                }
                Tz = 1.0 * (T11 + T12) / n * 100;
                if (Tz <= 5)
                    rez.Add(z);
                T.Add(Tz);
            }
            if (rez.Count == 0)
            {
                double min = T.Min();
                int j = 0;
                for (int z = Z_min; z <= Z_max; z++)
                {
                    if (T[j] == min)
                        rez.Add(z);
                    j++;
                }
            }
            return rez;


        }

        public otchet Zapros(int period_prognoza)
        {
            int Frame = 1440 - period_prognoza;
            bool flag = false;
            List<int> Z_dop = new List<int>();
            if (Arhiv.Count > Frame * 2)
            {                

                List<double> kross_korell = new List<double>();
                raschet(ref kross_korell, Arhiv.GetRange(0, Arhiv.Count - Frame), Arhiv.GetRange(Arhiv.Count - Frame - 1, Frame));
                double r_max = kross_korell.Max();
                int t_max = kross_korell.IndexOf(r_max);
                if (r_max >= 0.9)
                {
                    List<double> U_t = Arhiv.GetRange(t_max + Frame, period_prognoza);
                    double H = Arhiv[Arhiv.Count - 1] - Arhiv[t_max + Frame];
                    if (Proval_perenapr(ref U_t, H + h, u_nom))
                    {
                        Z_dop = Zdop(U_t, 19, -19, 0.0178, period_prognoza, u_verh, u_niz);
                    }                    
                    
                }
                
            }

            otchet rez = new otchet(Truble, Z_dop);

            return rez;

        }

        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("TochkaKontrola");
            XmlOut.WriteAttributeString("Nomer", Nomer.ToString());
            XmlOut.WriteAttributeString("Chustvitelnost", Chustvitelnost.ToString());
            XmlOut.WriteAttributeString("Nominal_moshnost", Nominal_moshnost.ToString());
            XmlOut.WriteAttributeString("U_verh", U_verh.ToString());
            XmlOut.WriteAttributeString("U_niz", U_niz.ToString());
            XmlOut.WriteAttributeString("U_nom", U_nom.ToString());
            XmlOut.WriteAttributeString("Nomer_uzla", Nomer_uzla.ToString());
            //XmlOut.WriteAttributeString("U_Gelaemoe", U_Gelaemoe.ToString());
            //XmlOut.WriteAttributeString("Nomer_vetvi", vetv.Nomer.ToString());

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                Nomer = Convert.ToInt32(xmlIn.GetAttribute("Nomer"));
                Chustvitelnost = Convert.ToDouble(xmlIn.GetAttribute("Chustvitelnost"));
                Nominal_moshnost = Convert.ToInt32(xmlIn.GetAttribute("Nominal_moshnost"));
                U_verh = Convert.ToDouble(xmlIn.GetAttribute("U_verh"));
                U_niz = Convert.ToDouble(xmlIn.GetAttribute("U_niz"));
                U_nom = Convert.ToDouble(xmlIn.GetAttribute("U_nom"));
                Nomer_uzla = Convert.ToInt32(xmlIn.GetAttribute("Nomer_uzla"));
               

            }
            catch (Exception)
            { }
        }

    }

    public class otchet
    {
        List<int> _Zdop;
        int _Tr;
        

        public List<int> Zdop
        {
            get { return _Zdop; }
            set { _Zdop = value; }
        }
        

        public int Tr
        {
            get { return _Tr; }
            set { _Tr = value; }
        }


        public otchet(int Trub, List<int> Z)
        {
            Tr = Trub;
            Zdop = Z;
        }
    }


    public class Automatic_RN
    {
        Shema shema_connect;
        List<TochkaKontrola> _Tochki_Kontrola;

        public List<TochkaKontrola> Tochki_Kontrola
        {
            get { return _Tochki_Kontrola; }
            set { _Tochki_Kontrola = value; }
        }

        int S_max;
        int t_sutok;
        int t_perekl;
        int Time_do_perecl;
        int S;//число перключений в эти сутки

        public void Start()
        {
            t_perekl = 0;
            t_sutok = 0;
            foreach (TochkaKontrola item in _Tochki_Kontrola)
            {
                item.Start();
            }
            S = 0;
            Time_do_perecl = (int)(1440 / (32.8-S));            
        }
        public void Stop()
        {
           
            foreach (TochkaKontrola item in _Tochki_Kontrola)
            {
                item.Stop();
            }
           
        }

        public Automatic_RN(Shema s)
        {
            shema_connect = s;
            _Tochki_Kontrola = new List<TochkaKontrola>();
            S_max = 300000;

        }

        public int Raschet_izm_otpaiki(int period_prognoza)
        {
            SortedList<int, int> F_raspred = new SortedList<int, int>();
            foreach (TochkaKontrola item in _Tochki_Kontrola)
            {
                otchet o = item.Zapros(period_prognoza);
                if (o.Tr == 1)
                {
                    foreach (int z in o.Zdop)
                    {
                        if (F_raspred.Keys.Contains(z))
                        {
                            int index = F_raspred.IndexOfKey(z);
                            int mosh = item.Nominal_moshnost + F_raspred.Values[index];
                            F_raspred.RemoveAt(index);
                            F_raspred.Add(z, mosh);                            
                        }
                        else
                        {
                            F_raspred.Add(z,item.Nominal_moshnost);
                        }
                    }
                }
            }
            int rez=0;
            if (F_raspred.Count == 0)
                rez = 0;
            else
            {
                bool flag=true;
                int Max = F_raspred.Values.Max();
                foreach(int i in F_raspred.Values)
                    if ((i>Max*0.95)&&flag)
                    {
                        int ind=F_raspred.IndexOfValue(i);
                        rez=F_raspred.Keys[ind];
                        flag=false;
                    }
                
            }
            return rez;
        }




        public void Add_time()
        {
            t_sutok++;
            t_perekl++;
            if (t_perekl == Time_do_perecl)
            {
                S++;
                Time_do_perecl = (int)((1440 - t_sutok) / (32.8 - S));
                t_perekl = 0;
                int izmenen = Raschet_izm_otpaiki(Time_do_perecl);
                if (izmenen == 0)
                    S--;
                Time_do_perecl = (int)((1440 - t_sutok) / (32.8 - S));
                shema_connect.Rpni[0].Izmenit_otpaiku(izmenen);

            }
            if (t_sutok == 1440)
            {
                t_sutok = 0;
                t_perekl = 0;
                S = 0;
                Time_do_perecl = (int)((1440 - t_sutok) / (32.8 - S));
            }
        }

        public void Add_Tochka_kontrola(TochkaKontrola t)
        {
            t.Nomer_Uzla_Izmenen += new TochkaKontrola.Nomer_Uzla_Changed(shema_connect.Nomer_Uzla_Changed);
            _Tochki_Kontrola.Add(t);
        }


        #region XML

        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("Automatic_RN");
            XmlOut.WriteAttributeString("Version", "2");

            foreach (TochkaKontrola item in Tochki_Kontrola)
            {
                item.SaveToXml(XmlOut);
            }


            if (Tochki_Kontrola.Count == 0)
            {
                XmlOut.WriteStartElement("Null");
                XmlOut.WriteEndElement();
            }

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            //Очистка
            Tochki_Kontrola = new List<TochkaKontrola>();
            
            do
            {
                if (!xmlIn.Read())
                    throw new ArgumentException("Ошибочка");

                if ((xmlIn.NodeType == XmlNodeType.EndElement) &&
                    (xmlIn.Name == "Automatic_RN"))
                    break;

                if (xmlIn.NodeType == XmlNodeType.EndElement)
                    continue;

                if (xmlIn.Name == "TochkaKontrola")
                {
                    TochkaKontrola TK = new TochkaKontrola();
                    TK.Nomer_Uzla_Izmenen += new TochkaKontrola.Nomer_Uzla_Changed(shema_connect.Nomer_Uzla_Changed);
                    Tochki_Kontrola.Add(TK);
                    TK.LoadFromFile(xmlIn);
                }
                               
            } while (!xmlIn.EOF);
        }

        #endregion

    }

}
