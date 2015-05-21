using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Numerics;

namespace WindowsGraphica
{
    public class Shema : IXmlControl
    {
        List<Uzel> uzli;
        List<vetv> vetvi;
        List<RPN> rpni;
        List<GrafikRaboti> _Grafiki;
        
        List<int> nomera_raionov;        
        double[,] _Y;
        double[,] _Alf;
        double[] _Y_balanc;
        double[] _Alf_balanc;
        Hash_Spare_Matrix _G;
        Hash_Spare_Matrix _B;
        double[] _Gb;
        double[] _Bb;
        double[] _Gbb;
        double[] _Bbb;
        Complex[,] Y_1;
        double delta_P_summ;
        List<int> nomera_uzlov;

        public List<int> Nomera_uzlov
        {
            get { return nomera_uzlov; }
            set { nomera_uzlov = value; }
        }
        int nomer_balanc_uzla;



        #region Свойства
        public List<GrafikRaboti> Grafiki
        {
            get { return _Grafiki; }
            set { _Grafiki = value; }
        }
        internal List<Uzel> Uzli
        {
            get { return uzli; }
            set { uzli = value; }
        }

        internal List<vetv> Vetvi
        {
            get { return vetvi; }
            set { vetvi = value; }
        }

        internal List<RPN> Rpni
        {
            get { return rpni; }
            set { rpni = value; }
        }

        public List<int> Nomera_raionov
        {
            get { return nomera_raionov; }
            set { nomera_raionov = value; }
        }

        #endregion


        public Shema()
        {
            uzli = new List<Uzel>();
            vetvi = new List<vetv>();
            rpni = new List<RPN>();
            _Grafiki = new List<GrafikRaboti>();
            nomera_raionov = new List<int>();
        }

        //возвращает ссылку на узел с заданным номером
        public Uzel Find_Uzel_by_Nomer(int _nomer)
        {

            foreach (Uzel u in uzli)
            {
                if (u.Nomer_uzla == _nomer)
                    return u;
            }
            return null;
        }

        //возвращает список не добавленных узлов
        public List<int> SpisokNedobavlennihUzlov(List<int> _list)
        {
            List<int> list = new List<int>();
            bool p = false;//показывает есть ли данный узел среди уже добавленных
            foreach (Uzel item in uzli)
            {
                p = false;
                foreach (int i in _list)
                {
                    if (item.Nomer_uzla == i)
                    {
                        p = true;
                    }
                }
                if (!p)
                {
                    list.Add(item.Nomer_uzla);
                }
            }
            if (list.Count != 0)
                return list;
            else
                return null;
        }

        //возвращает ссылку на ветвь с заданным номером
        public vetv Find_Vetv_by_Nomer(int _nomer)
        {

            foreach (vetv v in vetvi)
            {
                if (v.Nomer == _nomer)
                    return v;
            }
            return null;
        }

        //возвращает ссылку на ветвь с заданным номером узла и номером из списка узлов
        public List<vetv> Find_Nedobav_Vetv_po_Uzlu(List<int> spisok_dob_uzlov, int nomer_uzla)
        {
            List<vetv> spis = new List<vetv>();
            foreach (vetv v in vetvi)
            {
                foreach (int i in spisok_dob_uzlov)
                {
                    if (((v.Nomer_Uzla_Konca == nomer_uzla) & (v.Nomer_Uzla_Nachal == i)) || ((v.Nomer_Uzla_Nachal == nomer_uzla) & (v.Nomer_Uzla_Konca == i)))
                    {
                        spis.Add(v);
                    }
                }
            }
            if (spis.Count != 0)
                return spis;
            else
                return null;

        }

        #region CDU
        //проверка узлов на уникальность номеров
        public bool ProverkaUzlov()
        {
            foreach (Uzel item in uzli)
            {
                int n = 0;
                foreach (Uzel uzel in uzli)
                {
                    if (item.Nomer_uzla == uzel.Nomer_uzla)
                    { n++; }
                }
                if (n > 1)
                    return false;
            }
            return true;
        }

        //нужно для SaveToCDU
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

        private double Proverka_pust_stroki(string str)
        {
            double res;
            str = str.Replace('.', ',');
            const string pusto = "        ";
            if (str != pusto)
                res = Convert.ToDouble(str);
            else res = 0;
            return res;
        }

        public bool SaveToCDU(StreamWriter writer)
        {

            //0201,0202
            int nomer_balanc_uzla = 0;
            double U_balanc_uzla = 0;
            List<string> spisok_po_uzli = new List<string>();
            foreach (Uzel item in uzli)
            {
                spisok_po_uzli.AddRange(item.SaveToCDU());
                if (item.Tip_uzla == -1)
                {
                    nomer_balanc_uzla = item.Nomer_uzla;
                    U_balanc_uzla = item.U_zad;
                }
            }
            string s = "0102                    ";
            s = s + Stroki(nomer_balanc_uzla.ToString()) + Stroki(U_balanc_uzla.ToString());
            spisok_po_uzli.Add(s);

            List<string> spisok_po_vetvi = new List<string>();
            foreach (vetv item in vetvi)
            {
                spisok_po_vetvi.AddRange(item.SaveToCDU());
            }
            spisok_po_uzli.Sort();
            spisok_po_vetvi.Sort();
            spisok_po_uzli.AddRange(spisok_po_vetvi);

            List<string> spisok_name_po_uzli = new List<string>();
            foreach (Uzel item in uzli)
            {
                if (item.NameSaveToCDU() != "")
                    spisok_name_po_uzli.Add(item.NameSaveToCDU());
            }
            if (spisok_po_uzli.Count != 0)
            {
                if (spisok_name_po_uzli.Count == 0)
                {
                    string str = "0000";
                    spisok_po_uzli.Add(str);
                    foreach (string stroki in spisok_po_uzli)
                        writer.WriteLine(stroki);
                }
                else
                {
                    foreach (string stroki in spisok_po_uzli)
                        writer.WriteLine(stroki);
                    writer.WriteLine("7777");
                    foreach (string strok in spisok_name_po_uzli)
                        writer.WriteLine(strok);
                    writer.WriteLine("0000");
                }
            }
            else
                return false;
            return true;
        }

        public bool LoadFromCDU(StreamReader reader)
        {
            bool res = false;
            int nomer_balanc_uzla = 0;
            double U_balanc_uzla = 0;
            string str = "";
            Uzel u;
            bool flag;
            do
            {
                str = reader.ReadLine();
                if ((str == null) || (str == ""))
                    break;
                if (str == "0000")
                    break;
                switch (str.Substring(0, 4))
                {
                    case "0201":
                        Uzel item = new Uzel();
                        if (item.LoadFromCDU(str))
                            uzli.Add(item);
                        res = true;
                        break;
                    case "0301":
                        int nomer = (int)Proverka_pust_stroki(str.Substring(8, 8));
                        int nomer1 = (int)Proverka_pust_stroki(str.Substring(16, 8));
                        if (nomer * nomer1 == 0)
                        {
                            u = Find_Uzel_by_Nomer(nomer + nomer1);
                            flag = u.LoadFromCDU(str);
                        }
                        else
                        {
                            vetv v = new vetv();
                            if (v.LoadFromCDU(str))
                                vetvi.Add(v);
                        }
                        res = true;
                        break;
                    case "0202":
                        nomer = (int)Proverka_pust_stroki(str.Substring(8, 8));
                        u = Find_Uzel_by_Nomer(nomer);
                        flag = u.LoadFromCDU(str);
                        res = true;
                        break;
                    case "0290":
                        nomer = (int)Proverka_pust_stroki(str.Substring(8, 8));
                        u = Find_Uzel_by_Nomer(nomer);
                        flag = u.LoadFromCDU(str);
                        res = true;
                        break;
                    case "7777":
                        break;
                    case "0102":
                        nomer_balanc_uzla = (int)Proverka_pust_stroki(str.Substring(24, 8));
                        U_balanc_uzla = Proverka_pust_stroki(str.Substring(32, 8));
                        break;
                }
            }
            while (true);

            Uzel item1 = Find_Uzel_by_Nomer(nomer_balanc_uzla);
            if (item1 != null)
            {
                item1.Tip_uzla = -1;
                item1.U_zad = U_balanc_uzla;
                res = true;
            }
            Numeracia_Vetvi();

            return res;
        }

        private void Numeracia_Vetvi()
        {
            List<int> nomera_vetvei = new List<int>();
            foreach (vetv item in vetvi)
            {
                if (item.Nomer != 0)
                {
                    nomera_vetvei.Add(item.Nomer);
                }
            }
            if (nomera_vetvei.Count < vetvi.Count)
            {
                foreach (vetv item in vetvi)
                {
                    if (item.Nomer == 0)
                    {
                        int i = 1;
                        bool p = true;
                        do
                        {
                            if (nomera_vetvei.IndexOf(i) == (-1))
                            {
                                item.Nomer = i;
                                p = false;
                                nomera_vetvei.Add(i);
                            }
                            i++;
                        }
                        while (p);
                    }
                }
            }
        }

        #endregion


        public void Opredelenie_rainov()
        {
            foreach (Uzel u in uzli)
            {
                if (!nomera_raionov.Contains(u.Nomer_raiona))
                {
                    nomera_raionov.Add(u.Nomer_raiona);
                }
            }
            foreach (vetv v in vetvi)
            {
                if (!nomera_raionov.Contains(v.Nomer_raiona))
                {
                    nomera_raionov.Add(v.Nomer_raiona);
                }
            }
        }

        private bool proverka_ishodn_dannih()
        {
            bool flag = true;
            foreach (vetv v in vetvi)
            {
                if (Find_Uzel_by_Nomer(v.Nomer_Uzla_Nachal).U_nom < Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca).U_nom)
                {
                    flag = false;
                    MessageBox.Show("ошибка данных проверьте U_nom для линии " + v.Nomer.ToString());
                }
                if ((v.Kt1 == 0) && (v.Kt2 == 0))
                {
                    double U1 = Find_Uzel_by_Nomer(v.Nomer_Uzla_Nachal).U_nom;
                    double U2 = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca).U_nom;
                    if (Math.Abs((U1 - U2) / U1) > 0.2)
                    {
                        flag = false;
                        MessageBox.Show("ошибка данных проверьте Кт для линии " + v.Nomer.ToString());
                    }
                }
                else
                {
                    double kt = Math.Sqrt(v.Kt1 * v.Kt1 + v.Kt2 * v.Kt2);
                    double U1 = Find_Uzel_by_Nomer(v.Nomer_Uzla_Nachal).U_nom;
                    double U11 = U1 * kt;
                    double U2 = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca).U_nom;
                    if ((Math.Abs(U11 - U2) / U2) > 0.2)
                    {
                        flag = false;
                        MessageBox.Show("ошибка данных проверьте Кт для линии " + v.Nomer.ToString());
                    }
                }
            }
            return flag;
        }

        public void Raschet(bool Ploskiy_Start, bool Start, bool Tip_Rascheta)   //если тип расчета истина, то в полярных координатах, иначе - в прямоугольных
        { //если плоский старт истина, то от номинальных значений расчет, иначе - от предыдущего режима. если зейдель истина, то используется стартовый алгоритм
            nomera_uzlov = new List<int>();
            List<Uzel> opornie_GU = new List<Uzel>();
            nomer_balanc_uzla = 0;

            //proverka
            int i = 0;
            int chetchik = 0;
            foreach (Uzel u in uzli)
            {
                if ((u.Tip_uzla) == -1)
                    i++;
                else
                    chetchik++;

            }
            if (i == 1)
            {
                #region Определение размерностей массивов
                _Y = new double[chetchik, chetchik];
                _Alf = new double[chetchik, chetchik];
                _Y_balanc = new double[chetchik];
                _Alf_balanc = new double[chetchik];
                _G = new Hash_Spare_Matrix(chetchik);
                _B = new Hash_Spare_Matrix(chetchik);
                _Gb = new double[chetchik];
                _Bb = new double[chetchik];
                _Gbb = new double[chetchik + 1];
                _Bbb = new double[chetchik + 1];
                Y_1 = new Complex[chetchik, chetchik];
                #endregion

                #region Расчет Матриц Y
                uzli.Sort();
                foreach (Uzel item in uzli)
                {
                    switch (item.Tip_uzla)
                    {
                        case 0:
                            nomera_uzlov.Add(item.Nomer_uzla);
                            break;
                        case 1:
                            nomera_uzlov.Add(item.Nomer_uzla);
                            break;
                        case -1:
                            nomera_uzlov.Add(item.Nomer_uzla);
                            nomer_balanc_uzla = item.Nomer_uzla;
                            break;
                    }
                }
                //private void Raschet_Matrici_Y(List<int> nomera_uzlov, int nomer_balanc_uzla, ref double[,] y, ref double[,] alf, ref double[] y_balanc, ref double[] alf_balanc, ref double[,] _g, ref double[,] _b, ref double[] _gb, ref double[] _bb, ref double[] _gbb, ref double[] _bbb)
                Raschet_Matrici_Y(nomera_uzlov, nomer_balanc_uzla, ref _Y, ref _Alf, ref _Y_balanc, ref _Alf_balanc, ref _G, ref _B, ref _Gb, ref _Bb, ref _Gbb, ref _Bbb,ref Y_1);
                nomera_uzlov.Remove(nomer_balanc_uzla);
                SaveMatrci_Y();
                #endregion

                List<double> U = new List<double>();
                List<double> delta = new List<double>();
                List<double> Pi = new List<double>();
                List<double> Qi = new List<double>();
                List<int> index_opor_uzlov = new List<int>();
                double Ubasis = 0;
                foreach (Uzel item in uzli)
                {
                    switch (item.Tip_uzla)
                    {
                        case 0:
                            if (Ploskiy_Start == true)
                            {
                                U.Add(item.U_nom);
                                delta.Add(0.0);
                            }
                            else
                            {
                                if (item.U_mod != 0)
                                {
                                    U.Add(item.U_mod);
                                    delta.Add((item.Angle));
                                }
                                else
                                {
                                    U.Add(item.U_nom);
                                    delta.Add(0.0);
                                }
                            }
                            Pi.Add((item.P_gen - item.P_load));
                            Qi.Add((item.Q_gen - item.Q_load));
                            break;
                        case 1:
                            opornie_GU.Add(item);
                            //        index_opor_uzlov.Add(nomera_uzlov.IndexOf(item.Nomer_uzla));  //тут нужно будет крепко подумать по поводу старта с генераторными узлами

                            if (Ploskiy_Start == true)
                            {
                                delta.Add(0.0);
                                U.Add(item.U_zad);
                                index_opor_uzlov.Add(nomera_uzlov.IndexOf(item.Nomer_uzla));
                                if (Tip_Rascheta == true)
                                    Qi.Add(((item.Q_max + item.Q_min) / 2 - item.Q_load));
                                else
                                    Qi.Add((item.U_zad * item.U_zad));
                            }
                            else
                            {
                                if (item.U_mod != 0)
                                {
                                    U.Add(item.U_mod);
                                    //          U.Add(item.U_zad);
                                    delta.Add((item.Angle));
                                    if (Tip_Rascheta == true)
                                        Qi.Add((item.Q_gen - item.Q_load));
                                    else
                                        if (item.U_mod == item.U_zad)
                                            Qi.Add((item.U_zad * item.U_zad));
                                        else
                                            Qi.Add((item.Q_gen - item.Q_load));
                                    if (item.U_mod == item.U_zad)
                                        index_opor_uzlov.Add(nomera_uzlov.IndexOf(item.Nomer_uzla));
                                }
                                else
                                {
                                    delta.Add(0.0);
                                    index_opor_uzlov.Add(nomera_uzlov.IndexOf(item.Nomer_uzla));
                                    U.Add(item.U_zad);
                                    if (Tip_Rascheta == true)
                                        Qi.Add(((item.Q_max + item.Q_min) / 2 - item.Q_load));
                                    else
                                        Qi.Add((item.U_zad * item.U_zad));
                                }
                            }

                            Pi.Add((item.P_gen - item.P_load));

                            break;
                        case -1:
                            Ubasis = item.U_zad;
                            item.U_mod = item.U_zad;
                            break;
                    }
                }

                regim Reg = new regim(U, delta, Pi, Qi, index_opor_uzlov, Ubasis, _G, _B, _Gb, _Bb, opornie_GU, _Gbb, _Bbb, Y_1);   //может, не передавать мощность балансирующего, а считать в схеме?
                if (Tip_Rascheta == true)
                {
                    Complex[] result = Reg.Raschet_Polarniy(0.0002, nomera_uzlov);
                    for (int n = 0; n < nomera_uzlov.Count; n++)
                    {
                        Find_Uzel_by_Nomer(nomera_uzlov[n]).U_mod = result[n].Real;
                        Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle = result[n].Imaginary;
                        Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle_degree = (result[n].Imaginary * 180 / Math.PI);
                    }
                    Find_Uzel_by_Nomer(nomer_balanc_uzla).P_gen = result[nomera_uzlov.Count].Real + Find_Uzel_by_Nomer(nomer_balanc_uzla).P_load;
                    Find_Uzel_by_Nomer(nomer_balanc_uzla).Q_gen = result[nomera_uzlov.Count].Imaginary + Find_Uzel_by_Nomer(nomer_balanc_uzla).Q_load;
                    foreach (Uzel item in opornie_GU)
                    {
                        Find_Uzel_by_Nomer(item.Nomer_uzla).Q_gen = item.Q_gen;
                    }
                }
                else
                {
                    Complex[] result1 = Reg.Raschet(0.0002, nomera_uzlov);
                    for (int n = 0; n < nomera_uzlov.Count; n++)
                    {

                        Find_Uzel_by_Nomer(nomera_uzlov[n]).U_mod = result1[n].Magnitude;
                        Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle = result1[n].Phase;
                        Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle_degree = (result1[n].Phase * 180 / Math.PI);
                    }
                    Find_Uzel_by_Nomer(nomer_balanc_uzla).P_gen = result1[nomera_uzlov.Count].Real + Find_Uzel_by_Nomer(nomer_balanc_uzla).P_load;
                    Find_Uzel_by_Nomer(nomer_balanc_uzla).Q_gen = result1[nomera_uzlov.Count].Imaginary + Find_Uzel_by_Nomer(nomer_balanc_uzla).Q_load;
                }

                /*           if (Start == true)    //ТЕСТИРОВАНИЕ ЗЕЙДЕЛЯ
                           {
                               Complex[] result2 = Reg.metod_Zeidela(nomera_uzlov, 50000);
                               for (int n = 0; n < nomera_uzlov.Count; n++)
                               {
                                   Find_Uzel_by_Nomer(nomera_uzlov[n]).U_mod = (decimal)result2[n].Magnitude;
                                   Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle = (decimal)result2[n].Phase;
                                   Find_Uzel_by_Nomer(nomera_uzlov[n]).Angle_degree = (decimal)(result2[n].Phase * 180 / Math.PI);
                               }
                           }      */

                Raschet_moshnosti();

                //ДОБАВЛЕНО МНОЙ. НУЖНО ДЛЯ ТОГО, ЧТОБЫ РЕЗУЛЬТАТ ВЫВОДИЛСЯ ПО ВОЗРАСТАНИЮ НОМЕРОВ УЗЛОВ. Т.Е. УПОРЯДОЧИВАНИЕ
                bool fl0 = true;
                while (fl0)
                {
                    fl0 = false;
                    for (int ii = 0; ii < Uzli.Count - 1; ii++)
                    {
                        if (Uzli[ii].Nomer_uzla > Uzli[ii + 1].Nomer_uzla)
                        {
                            Uzel buf = new Uzel();
                            buf = Uzli[ii];
                            Uzli[ii] = Uzli[ii + 1];
                            Uzli[ii + 1] = buf;
                            fl0 = true;
                        }
                    }
                }



            }

        }

        public void SaveMatrci_Y()
        {
            StreamWriter writer = new StreamWriter("result1.txt");
            writer.Write("Номера" + '	');
            for (int i = 0; i < nomera_uzlov.Count; i++)
            {
                writer.Write(nomera_uzlov[i].ToString() + '	');
            }

            writer.WriteLine(nomer_balanc_uzla.ToString() + '	');
            for (int i = 0; i < nomera_uzlov.Count; i++)
            {
                writer.Write(nomera_uzlov[i].ToString() + '	');
                for (int j = 0; j < nomera_uzlov.Count; j++)
                {
                    writer.Write(_G.getValue(i, j).ToString() + '	');

                }
                writer.WriteLine(_Gb[i].ToString() + '	');
            }
            writer.Write("B" + '	');
            writer.Write("Номера" + '	');
            for (int i = 0; i < nomera_uzlov.Count; i++)
            {
                writer.Write(nomera_uzlov[i].ToString() + '	');
            }

            writer.WriteLine(nomer_balanc_uzla.ToString() + '	');
            for (int i = 0; i < nomera_uzlov.Count; i++)
            {
                writer.Write(nomera_uzlov[i].ToString() + '	');
                for (int j = 0; j < nomera_uzlov.Count; j++)
                {
                    writer.Write(_B.getValue(i, j).ToString() + '	');

                }
                writer.WriteLine(_Bb[i].ToString() + '	');
            }
            writer.Close();
        }


        /*
        private void Raschet_Matrici_Y(List<int> nomera_uzlov, int nomer_balanc_uzla, ref double[,] y, ref double[,] alf, ref double[] y_balanc, ref double[] alf_balanc, ref Hash_Spare_Matrix _g, ref Hash_Spare_Matrix _b, ref double[] _gb, ref double[] _bb, ref double[] _gbb, ref double[] _bbb)
        {
            int i = 0;
            Complex[,] matrica_Y = new Complex[nomera_uzlov.Count, nomera_uzlov.Count];
            Complex[] vector_Y = new Complex[nomera_uzlov.Count - 1];

            foreach (vetv item in vetvi)
            {
                Complex c = 1 / (new Complex(item.R, item.X));
                i = nomera_uzlov.IndexOf(item.Nomer_Uzla_Nachal);
                int j = nomera_uzlov.IndexOf(item.Nomer_Uzla_Konca);
                if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom == Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                {
                    matrica_Y[i, j] += (-c);
                    matrica_Y[i, i] += c;
                    matrica_Y[j, j] += c;
                    matrica_Y[i, i] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);           //Yc>0
                    matrica_Y[j, j] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);
                    matrica_Y[j, i] += (-c);//= matrica_Y[i, j];  
                }
                if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom > Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                {
                    matrica_Y[i, j] -= c / (new Complex(item.Kt1, item.Kt2));
                    matrica_Y[i, i] += c;
                    matrica_Y[j, j] += c / (new Complex(item.Kt1, item.Kt2) * new Complex(item.Kt1, item.Kt2));
                    matrica_Y[i, i] -= new Complex(item.Gc * 0.000001, item.Bc * 0.000001);           //Yc>0
                    matrica_Y[j, i] -= c / (new Complex(item.Kt1, -item.Kt2));

                }

                if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom < Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                {
                    matrica_Y[i, j] -= c / (new Complex(item.Kt1, -item.Kt2));
                    matrica_Y[j, j] += c;
                    matrica_Y[i, i] += c / (new Complex(item.Kt1, item.Kt2) * new Complex(item.Kt1, item.Kt2));
                    matrica_Y[j, j] -= new Complex(item.Gc * 0.000001, item.Bc * 0.000001);
                    matrica_Y[j, i] -= c / (new Complex(item.Kt1, item.Kt2));
                }


            }
            foreach (Uzel u in uzli)
            {

                if ((u.G_sh != 0) || (u.B_sh != 0))
                {
                    i = nomera_uzlov.IndexOf(u.Nomer_uzla);
                    matrica_Y[i, i] += new Complex(u.G_sh * 0.000001, -u.B_sh * 0.000001);
                }
            }

            for (int x = 0; x < nomera_uzlov.Count - 1; x++)
            {
                for (int j = 0; j < nomera_uzlov.Count - 1; j++)
                {
        //            y[x, j] = (decimal)matrica_Y[x, j].Magnitude;
          //          alf[x, j] = (decimal)(Math.PI) / 2 - (decimal)matrica_Y[x, j].Phase;
                    _g.setValue(x, j, matrica_Y[x, j].Real);
                    _b.setValue(x, j, -matrica_Y[x, j].Imaginary);
                }
     //           y_balanc[x] = (decimal)matrica_Y[x, nomera_uzlov.Count - 1].Magnitude;
       //         alf_balanc[x] = (decimal)(Math.PI) / 2 - (decimal)matrica_Y[x, nomera_uzlov.Count - 1].Phase;
                _gb[x] = matrica_Y[x, nomera_uzlov.Count - 1].Real;
                _bb[x] = -matrica_Y[x, nomera_uzlov.Count - 1].Imaginary;
                _gbb[x] = matrica_Y[nomera_uzlov.Count - 1, x].Real;
                _bbb[x] = -matrica_Y[nomera_uzlov.Count - 1, x].Imaginary;
            }
            _gbb[nomera_uzlov.Count-1] = matrica_Y[nomera_uzlov.Count - 1, nomera_uzlov.Count - 1].Real;
            _bbb[nomera_uzlov.Count-1] = -matrica_Y[nomera_uzlov.Count - 1, nomera_uzlov.Count - 1].Imaginary;

        }
        */

        private void Raschet_Matrici_Y(List<int> nomera_uzlov, int nomer_balanc_uzla, ref double[,] y, ref double[,] alf, ref double[] y_balanc, ref double[] alf_balanc, ref Hash_Spare_Matrix _g, ref Hash_Spare_Matrix _b, ref double[] _gb, ref double[] _bb, ref double[] _gbb, ref double[] _bbb, ref Complex[,] YY)
        {
            int i = 0;
            Complex[,] matrica_Y = new Complex[nomera_uzlov.Count, nomera_uzlov.Count];
            Complex[] vector_Y = new Complex[nomera_uzlov.Count - 1];

            foreach (vetv item in vetvi)
            {
                Complex c = 1 / (new Complex(item.R, item.X));
                i = nomera_uzlov.IndexOf(item.Nomer_Uzla_Nachal);
                int j = nomera_uzlov.IndexOf(item.Nomer_Uzla_Konca);
                if ((item.Kt1 == 0) && (item.Kt2 == 0))
                {
                    matrica_Y[i, j] += (-c);
                    matrica_Y[i, i] += c;
                    matrica_Y[j, j] += c;
                    matrica_Y[i, i] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);           //Yc>0
                    matrica_Y[j, j] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);
                    matrica_Y[j, i] += (-c);//= matrica_Y[i, j];  
                }
                else
                {
                    matrica_Y[i, j] -= c / (new Complex(item.Kt1, item.Kt2));
                    matrica_Y[i, i] += c;
                    matrica_Y[j, j] += c / (new Complex(item.Kt1, item.Kt2) * new Complex(item.Kt1, -item.Kt2));
                    matrica_Y[i, i] -= new Complex(item.Gc * 0.000001, item.Bc * 0.000001);           //Yc>0
                    matrica_Y[j, i] -= c / (new Complex(item.Kt1, -item.Kt2));
                }
                #region
                /*
                if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom == Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                {
                    matrica_Y[i, j] += (-c);
                    matrica_Y[i, i] += c;
                    matrica_Y[j, j] += c;
                    matrica_Y[i, i] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);           //Yc>0
                    matrica_Y[j, j] -= new Complex(item.Gc * 0.0000005, item.Bc * 0.0000005);
                    matrica_Y[j, i] += (-c);//= matrica_Y[i, j];  
                }
                
                
                    if (((Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom == Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom) & (Find_Vetv_by_Nomer(item.Nomer).Kt2 != 0)))
                    {
                        matrica_Y[i, j] /= new Complex((double)item.Kt1, (double)item.Kt2);
                        matrica_Y[i, i] += c;
                        matrica_Y[j, j] += c / (new Complex((double)item.Kt1, (double)item.Kt2) * new Complex((double)item.Kt1, (double)item.Kt2));
                        matrica_Y[i, i] -= new Complex((double)item.Gc * 0.000001, (double)item.Bc * 0.000001);           //Yc>0
                        matrica_Y[j, i] += (-c);//изменилм(добавили)
                        matrica_Y[j, i] /= new Complex((double)item.Kt1, -(double)item.Kt2);
                    }

                    if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom > Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                    {
                        matrica_Y[i, j] -= c / (new Complex((double)item.Kt1, (double)item.Kt2));
                        matrica_Y[i, i] += c;
                        matrica_Y[j, j] += c / (new Complex((double)item.Kt1, (double)item.Kt2) * new Complex((double)item.Kt1, (double)item.Kt2));
                        matrica_Y[i, i] -= new Complex((double)item.Gc * 0.000001, (double)item.Bc * 0.000001);           //Yc>0
                        matrica_Y[j, i] -= c / (new Complex((double)item.Kt1, -(double)item.Kt2));

                    }

                    if (Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_nom < Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_nom)
                    {
                        matrica_Y[i, j] -= c / (new Complex((double)item.Kt1, -(double)item.Kt2));
                        matrica_Y[j, j] += c;
                        matrica_Y[i, i] += c / (new Complex((double)item.Kt1, (double)item.Kt2) * new Complex((double)item.Kt1, (double)item.Kt2));
                        matrica_Y[j, j] -= new Complex((double)item.Gc * 0.000001, (double)item.Bc * 0.000001);
                        matrica_Y[j, i] -= c / (new Complex((double)item.Kt1, (double)item.Kt2));
                    }
                 */
                #endregion
            }

            foreach (Uzel u in uzli)
            {

                if ((u.G_sh != 0) || (u.B_sh != 0))
                {
                    i = nomera_uzlov.IndexOf(u.Nomer_uzla);
                    matrica_Y[i, i] += new Complex(u.G_sh * 0.000001, -u.B_sh * 0.000001);
                }
            }

            for (int x = 0; x < nomera_uzlov.Count - 1; x++)
            {
                for (int j = 0; j < nomera_uzlov.Count - 1; j++)
                {
                    YY[x, j] = matrica_Y[x, j];
                    // alf[x, j] = (Math.PI) / 2 - matrica_Y[x, j].Phase;
                    _g.setValue(x, j, matrica_Y[x, j].Real);
                    _b.setValue(x, j, -matrica_Y[x, j].Imaginary);
                }
                // y_balanc[x] = matrica_Y[x, nomera_uzlov.Count - 1].Magnitude;
                // alf_balanc[x] = (Math.PI) / 2 - matrica_Y[x, nomera_uzlov.Count - 1].Phase;
                _gb[x] = matrica_Y[x, nomera_uzlov.Count - 1].Real;
                _bb[x] = -matrica_Y[x, nomera_uzlov.Count - 1].Imaginary;
                _gbb[x] = matrica_Y[nomera_uzlov.Count - 1, x].Real;
                _bbb[x] = -matrica_Y[nomera_uzlov.Count - 1, x].Imaginary;
            }
            _gbb[nomera_uzlov.Count - 1] = matrica_Y[nomera_uzlov.Count - 1, nomera_uzlov.Count - 1].Real;
            _bbb[nomera_uzlov.Count - 1] = -matrica_Y[nomera_uzlov.Count - 1, nomera_uzlov.Count - 1].Imaginary;

        }


        private void Raschet_moshnosti()
        {
            Complex I = new Complex();
            Complex I_nach = new Complex();
            Complex I_konc = new Complex();
            Complex S_nach = new Complex();
            Complex S_konc = new Complex();
            Complex delta_S = new Complex();
            Complex delta_U = new Complex();
            Complex Y_nach = new Complex();
            Complex Y_konc = new Complex();
            Complex Z = new Complex();
            delta_P_summ = 0;
            foreach (vetv item in vetvi)
            {
                Z = new Complex(item.R, item.X);
                if (((item.Kt1 != 0)) || ((item.Kt2 != 0)))
                {
                    Y_nach = new Complex(item.Gc * 0.000001, item.Bc * 0.000001);
                    Complex k_t = new Complex(item.Kt1, item.Kt2);
                    Complex U_n = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_mod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_mod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle));
                    Complex U_k = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_mod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_mod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle));
                    Complex U_k1 = U_k / k_t;
                    delta_U = U_k1 - U_n;
                    I = delta_U / Z;
                    Complex I_nc = U_n * Y_nach;
                    I_nach = I - I_nc;
                    I_konc = I / new Complex(k_t.Real, -k_t.Imaginary);
                    S_nach = U_n * (new Complex(I_nach.Real, -I_nach.Imaginary));
                    S_konc = U_k * (new Complex(I_konc.Real, -I_konc.Imaginary));
                    delta_S = I.Magnitude * I.Magnitude * Z;
                }
                else
                {
                    Y_nach = new Complex(item.Gc / 2 * 0.000001, -item.Bc / 2 * 0.000001);
                    Y_konc = new Complex(item.Gc / 2 * 0.000001, -item.Bc / 2 * 0.000001);
                    Complex U_n = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_mod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).U_mod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle));
                    Complex U_k = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_mod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).U_mod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle));
                    delta_U = U_k - U_n;
                    I = delta_U / Z;
                    Complex I_nc = U_n * Y_nach;
                    Complex I_kc = U_k * Y_konc;
                    I_nach = I - I_nc;
                    I_konc = I + I_kc;
                    S_nach = U_n * (new Complex(I_nach.Real, -I_nach.Imaginary));
                    S_konc = U_k * (new Complex(I_konc.Real, -I_konc.Imaginary));
                    delta_S = I.Magnitude * I.Magnitude * Z;

                }
                item.I_Nach = (I_nach.Magnitude / Math.Sqrt(3));
                item.I_Konc = (I_konc.Magnitude / Math.Sqrt(3));
                item.P_Nach = S_nach.Real;
                item.P_Konc = S_konc.Real;
                item.Delta_P = delta_S.Real;
                item.Delta_Q = delta_S.Imaginary;
                delta_P_summ = delta_P_summ + item.Delta_P;
                if (item.P_Konc < 0)
                    item.Napravlenie_moshnosti = false;
                else
                    item.Napravlenie_moshnosti = true;

            }
        }

        public List<vetv> Find_vetvi_Svyas_s_Uzlom(int NomerUzla)
        {
            List<vetv> list = new List<vetv>();
            foreach (vetv v in vetvi)
            {
                if ((v.Nomer_Uzla_Nachal == NomerUzla) || (v.Nomer_Uzla_Konca == NomerUzla))
                    list.Add(v);

            }
            return list;
        }

        public void ADD_Grafiki()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "*.*|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            string filename = dialog.FileName;

            StreamReader reader = new StreamReader(filename, Encoding.Default);
            GrafikRaboti grafik = new GrafikRaboti();

            if (!grafik.LoadFromFile(reader))
                MessageBox.Show("Ошибка чтения файла");
            else
            {
                _Grafiki.Add(grafik);
            }
            Numeracia_Grafikof();
            reader.Close();
        }
        private void Numeracia_Grafikof()
        {
            List<int> nomera_Grafikof = new List<int>();
            foreach (GrafikRaboti item in _Grafiki)
            {
                if (item.Nomer != 0)
                {
                    nomera_Grafikof.Add(item.Nomer);
                }
            }
            if (nomera_Grafikof.Count < _Grafiki.Count)
            {
                foreach (GrafikRaboti item in _Grafiki)
                {
                    if (item.Nomer == 0)
                    {
                        int i = 1;
                        bool p = true;
                        do
                        {
                            if (nomera_Grafikof.IndexOf(i) == (-1))
                            {
                                item.Nomer = i;
                                p = false;
                                nomera_Grafikof.Add(i);
                            }
                            i++;
                        }
                        while (p);
                    }
                }
            }
        }

        private GrafikRaboti Find_Grafik_by_nomer(int nomer)
        {
            foreach (GrafikRaboti g in _Grafiki)
            {
                if (g.Nomer == nomer)
                    return g;
            }
            return null;
        }

        public void Rascet_Regima_po_Grafikam(int h)
        {

            foreach (Uzel u in uzli)
            {
                if ((u.Nomer_Grafika_P_load != 0) && (u.P_max_load != 0))
                {
                    u.P_load = u.P_max_load * Find_Grafik_by_nomer(u.Nomer_Grafika_P_load).otn_znac(h);
                    u.Q_load = u.P_load * 0.4843221;
                }
                if ((u.Nomer_Grafika_P_gen != 0) && (u.P_max_gen != 0))
                {
                    u.P_gen = u.P_max_gen * Find_Grafik_by_nomer(u.Nomer_Grafika_P_gen).otn_znac(h);
                }
                if ((u.Nomer_Grafika_Q_load != 0) && (u.Q_load_max != 0))
                {
                    u.Q_load = u.P_load * 0.4843221;
                }
            }
            Raschet(false, false, true);
            

        }

        public void Nomer_Uzla_Changed(Iconnect_uzel sender, Chanche_nomer_Uzla args)
        {
            int nomer = args.New_Nomer_Uzla;            
            Uzel u = Find_Uzel_by_Nomer(nomer);
            if (u != null)
                sender.connect(u);

        }

        public void Nomer_Vetvi_Changed(Iconnect_vetv sender, Chanche_nomer_Vetvi args)
        {
            int nomer = args.New_Nomer_Vetvi;
            //Iconnect_vetv i = (Iconnect_vetv)sender;
            vetv v = Find_Vetv_by_Nomer(nomer);
            if (v != null)
                sender.connect(v);

        }

        #region XML

        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("Shema");
            XmlOut.WriteAttributeString("Version", "2");

            foreach (Uzel item in uzli)
            {
                item.SaveToXml(XmlOut);
            }
            foreach (vetv item in Vetvi)
            {
                item.SaveToXml(XmlOut);
            }
            foreach (GrafikRaboti item in _Grafiki)
            {
                item.SaveToXml(XmlOut);
            }
            foreach (RPN item in rpni)
            {
                item.SaveToXml(XmlOut);
            }

            if (uzli.Count == 0)
            {
                XmlOut.WriteStartElement("Null");
                XmlOut.WriteEndElement();
            }

            XmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            //Очистка
            uzli = new List<Uzel>();
            vetvi = new List<vetv>();

            do
            {
                if (!xmlIn.Read())
                    throw new ArgumentException("Ошибочка");

                if ((xmlIn.NodeType == XmlNodeType.EndElement) &&
                    (xmlIn.Name == "Shema"))
                    break;

                if (xmlIn.NodeType == XmlNodeType.EndElement)
                    continue;

                if (xmlIn.Name == "Uzel")
                {
                    Uzel u = new Uzel();
                    uzli.Add(u);
                    u.LoadFromFile(xmlIn);
                }
                if (xmlIn.Name == "Vetv")
                {
                    vetv v = new vetv();
                    vetvi.Add(v);
                    v.LoadFromFile(xmlIn);
                }
                if (xmlIn.Name == "GrafikRaboti")
                {
                    GrafikRaboti gr = new GrafikRaboti();
                    _Grafiki.Add(gr);
                    gr.LoadFromFile(xmlIn);
                }
                if (xmlIn.Name == "RPN")
                {
                    RPN r = new RPN();
                    r.Nomer_Vetvi_Izmenen += new RPN.Nomer_Vetvi_Changed(this.Nomer_Vetvi_Changed);
                    rpni.Add(r);
                    r.LoadFromFile(xmlIn);
                }


            } while (!xmlIn.EOF);
        }

        #endregion
    }
}

