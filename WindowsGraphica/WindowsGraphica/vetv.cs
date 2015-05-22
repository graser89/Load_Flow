using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WindowsGraphica
{

    public class vetv
    {
        int nomer;
        int nomer_Uzla_Nachal;
        int nomer_Uzla_Konca;
        double _R;
        double _X;
        double _Bc;
        double _Gc;
        double kt1;
        double kt2;
        double _P_Nach;
        double _P_Konc;
        double _I_Nach;
        double _I_Konc;
        double _delta_P;
        double _delta_Q;
        double delta_ij;
        double delta_0_ij;
        double alfa;
        double y;
        int nomer_raiona = 0;
        

        #region Свойства

        public int Nomer_raiona
        {
            get { return nomer_raiona; }
            set { nomer_raiona = value; }
        }
        public int Nomer
        {
            get { return nomer; }
            set { nomer = value; }
        }
        public int Nomer_Uzla_Nachal
        {
            get { return nomer_Uzla_Nachal; }
            set { nomer_Uzla_Nachal = value; }
        }
        public int Nomer_Uzla_Konca
        {
            get { return nomer_Uzla_Konca; }
            set { nomer_Uzla_Konca = value; }
        }
        public double R
        {
            get { return _R; }
            set { _R = value; }
        }
        public double X
        {
            get { return _X; }
            set { _X = value; }
        }
        public double Bc
        {
            get { return _Bc; }
            set { _Bc = value; }
        }
        public double Gc
        {
            get { return _Gc; }
            set { _Gc = value; }
        }
        public double Kt1
        {
            get { return kt1; }
            set { kt1 = value; }
        }
        public double Kt2
        {
            get { return kt2; }
            set { kt2 = value; }
        }
        public double P_Nach
        {
            get { return _P_Nach; }
            set { _P_Nach = value; }
        }
        public double P_Konc
        {
            get { return _P_Konc; }
            set { _P_Konc = value; }
        }
        public double I_Nach
        {
            get { return _I_Nach; }
            set { _I_Nach = value; }
        }
        public double I_Konc
        {
            get { return _I_Konc; }
            set { _I_Konc = value; }
        }
        public double Delta_P
        {
            get { return _delta_P; }
            set { _delta_P = value; }
        }
        public double Delta_Q
        {
            get { return _delta_Q; }
            set { _delta_Q = value; }
        }
        public double Delta_ij
        {
            get { return delta_ij; }
            set { delta_ij = value; }
        }
        public double Delta_0_ij
        {
            get { return delta_0_ij; }
            set { delta_0_ij = value; }
        }
        public double Alfa
        {
            get { return alfa; }
            set { alfa = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }
      
        #endregion

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

        public bool LoadFromCDU(string s)
        {
            var flagRes = false;
            string str = "";
            const string pusto = "        ";
            switch (s.Substring(0, 4))
            {
                case "0301":
                    {
                        if (s.Length > 41)
                        {
                            s = s.Replace(".", ",");
                            nomer_Uzla_Nachal = (int)Convert.ToDecimal(s.Substring(8, 8));
                            Nomer_Uzla_Konca = (int)Convert.ToDecimal(s.Substring(16, 8));
                            if (s.Substring(24, 8) != pusto)
                                R = Convert.ToDouble(s.Substring(24, 8));
                            else
                                R = 0;
                            if (s.Substring(32, 8) != pusto)
                                X = Convert.ToDouble(s.Substring(32, 8));
                            else
                                X = 0;
                            if (s.Substring(40, 8) != pusto)
                                Bc = Convert.ToDouble(s.Substring(40, 8));
                            else
                                Bc = 0;
                            if (s.Substring(48, 8) != pusto)
                                Kt1 = Convert.ToDouble(s.Substring(48, 8));
                            else
                                Kt1 = 0;
                            if (s.Substring(56, 8) != pusto)
                                Kt2 = Convert.ToDouble(s.Substring(56, 8));
                            else
                                Kt2 = 0;
                            if (s.Length >= (64 + 8))
                            {
                                if ((s.Substring(64, 8) != pusto) & (s.Substring(64, 8) != "") & (s.Substring(64, 8) != null))
                                    Gc = Convert.ToDouble(s.Substring(64, 8));
                                else
                                    Gc = 0;
                            }
                            flagRes = true;
                            if (R == 0 && X == 0)
                                X = 0.01;
                        }
                    }
                    break;
                default:
                    flagRes = false;
                    break;
            }
            return flagRes;

        }

        public List<string> SaveToCDU()
        {
            List<string> spisok = new List<string>();
            string s = "";

            s = "0301    ";
            string str = nomer_Uzla_Nachal.ToString();
            s = s + Stroki(str);

            str = nomer_Uzla_Konca.ToString();
            s = s + Stroki(str);
            str = R.ToString();
            s = s + Stroki(str);
            str = X.ToString();
            s = s + Stroki(str);
            str = Bc.ToString();
            s = s + Stroki(str);
            str = Kt1.ToString();
            s = s + Stroki(str);
            str = Kt2.ToString();
            s = s + Stroki(str);
            s.Replace(",", ".");
            spisok.Add(s);


            return spisok;
        }

        #endregion

        public vetv()
        { }

        #region XML
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("Vetv");
            XmlOut.WriteAttributeString("Nomer", nomer.ToString());
            XmlOut.WriteAttributeString("nomer_Uzla_Nachal", Nomer_Uzla_Nachal.ToString());
            XmlOut.WriteAttributeString("Nomer_Uzla_Konca", nomer_Uzla_Konca.ToString());
            XmlOut.WriteAttributeString("_R", _R.ToString());
            XmlOut.WriteAttributeString("_X", _X.ToString());
            XmlOut.WriteAttributeString("_Bc", _Bc.ToString());
            XmlOut.WriteAttributeString("_Gc", _Gc.ToString());
            XmlOut.WriteAttributeString("kt1", kt1.ToString());
            XmlOut.WriteAttributeString("kt2", kt2.ToString());
            XmlOut.WriteAttributeString("nomer_raiona", nomer_raiona.ToString());
            XmlOut.WriteAttributeString("_P_Nach", _P_Nach.ToString());
            XmlOut.WriteAttributeString("_P_Konc", _P_Konc.ToString());
            //XmlOut.WriteAttributeString("nomer_raiona", nomer_raiona.ToString());
            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            try
            {
                nomer = Convert.ToInt32(xmlIn.GetAttribute("Nomer"));
                nomer_Uzla_Nachal = Convert.ToInt32(xmlIn.GetAttribute("nomer_Uzla_Nachal"));
                nomer_Uzla_Konca = Convert.ToInt32(xmlIn.GetAttribute("Nomer_Uzla_Konca"));
                _R = Convert.ToDouble(xmlIn.GetAttribute("_R"));
                _X = Convert.ToDouble(xmlIn.GetAttribute("_X"));
                _Bc = Convert.ToDouble(xmlIn.GetAttribute("_Bc"));
                _Gc = Convert.ToDouble(xmlIn.GetAttribute("_Gc"));
                kt1 = Convert.ToDouble(xmlIn.GetAttribute("kt1"));
                kt2 = Convert.ToDouble(xmlIn.GetAttribute("kt2"));
                nomer_raiona = Convert.ToInt32(xmlIn.GetAttribute("nomer_raiona"));
                _P_Nach = Convert.ToDouble(xmlIn.GetAttribute("_P_Nach"));
                _P_Konc = Convert.ToDouble(xmlIn.GetAttribute("_P_Konc"));
                //nomer_raiona = Convert.ToInt32(xmlIn.GetAttribute("nomer_raiona"));

            }
            catch (Exception)
            { }
        }
        #endregion

    }
}
