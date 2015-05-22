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
    public class Shema 
    {
        List<Uzel> _uzli;
        List<vetv> _vetvi;

        
        List<int> _nomeraRaionov;        
        double[,] _y;
        double[,] _alf;
        double[] _yBalanc;
        double[] _alfBalanc;
        HashSpareMatrix _G;
        HashSpareMatrix _B;
        double[] _Gb;
        double[] _Bb;
        double[] _Gbb;
        double[] _Bbb;
        Complex[,] Y_1;
        double _deltaPSumm;
        List<int> _nomeraUzlov;

        public List<int> NomeraUzlov
        {
            get { return _nomeraUzlov; }
            set { _nomeraUzlov = value; }
        }
        int _nomerBalancUzla;



        #region Свойства
        internal List<Uzel> Uzli
        {
            get { return _uzli; }
            set { _uzli = value; }
        }

        internal List<vetv> Vetvi
        {
            get { return _vetvi; }
            set { _vetvi = value; }
        }


        public List<int> Nomera_raionov
        {
            get { return _nomeraRaionov; }
            set { _nomeraRaionov = value; }
        }

        #endregion


        public Shema()
        {
            _uzli = new List<Uzel>();
            _vetvi = new List<vetv>();           
            _nomeraRaionov = new List<int>();
        }

        //возвращает ссылку на узел с заданным номером
        public Uzel Find_Uzel_by_Nomer(int _nomer)
        {

            foreach (Uzel u in _uzli)
            {
                if (u.NomerUzla == _nomer)
                    return u;
            }
            return null;
        }

        //возвращает список не добавленных узлов
        public List<int> SpisokNedobavlennihUzlov(List<int> _list)
        {
            List<int> list = new List<int>();
            bool p = false;//показывает есть ли данный узел среди уже добавленных
            foreach (Uzel item in _uzli)
            {
                p = false;
                foreach (int i in _list)
                {
                    if (item.NomerUzla == i)
                    {
                        p = true;
                    }
                }
                if (!p)
                {
                    list.Add(item.NomerUzla);
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

            foreach (vetv v in _vetvi)
            {
                if (v.Nomer == _nomer)
                    return v;
            }
            return null;
        }

        //возвращает ссылку на ветвь с заданным номером узла и номером из списка узлов
        public List<vetv> Find_Nedobav_Vetv_po_Uzlu(List<int> spisokDobUzlov, int nomerUzla)
        {
            List<vetv> spis = new List<vetv>();
            foreach (vetv v in _vetvi)
            {
                foreach (int i in spisokDobUzlov)
                {
                    if (((v.Nomer_Uzla_Konca == nomerUzla) & (v.Nomer_Uzla_Nachal == i)) || ((v.Nomer_Uzla_Nachal == nomerUzla) & (v.Nomer_Uzla_Konca == i)))
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
            foreach (Uzel item in _uzli)
            {
                int n = 0;
                foreach (Uzel uzel in _uzli)
                {
                    if (item.NomerUzla == uzel.NomerUzla)
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
            int nomerBalancUzla = 0;
            double uBalancUzla = 0;
            List<string> spisokPoUzli = new List<string>();
            foreach (Uzel item in _uzli)
            {
                spisokPoUzli.AddRange(item.SaveToCDU());
                if (item.TipUzla == -1)
                {
                    nomerBalancUzla = item.NomerUzla;
                    uBalancUzla = item.UZad;
                }
            }
            string s = "0102                    ";
            s = s + Stroki(nomerBalancUzla.ToString()) + Stroki(uBalancUzla.ToString());
            spisokPoUzli.Add(s);

            List<string> spisokPoVetvi = new List<string>();
            foreach (vetv item in _vetvi)
            {
                spisokPoVetvi.AddRange(item.SaveToCDU());
            }
            spisokPoUzli.Sort();
            spisokPoVetvi.Sort();
            spisokPoUzli.AddRange(spisokPoVetvi);

            var spisokNamePoUzli = new List<string>();
            foreach (Uzel item in _uzli)
            {
                if (item.NameSaveToCDU() != "")
                    spisokNamePoUzli.Add(item.NameSaveToCDU());
            }
            if (spisokPoUzli.Count != 0)
            {
                if (spisokNamePoUzli.Count == 0)
                {
                    string str = "0000";
                    spisokPoUzli.Add(str);
                    foreach (string stroki in spisokPoUzli)
                        writer.WriteLine(stroki);
                }
                else
                {
                    foreach (string stroki in spisokPoUzli)
                        writer.WriteLine(stroki);
                    writer.WriteLine("7777");
                    foreach (string strok in spisokNamePoUzli)
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
            int nomerBalancUzla = 0;
            double uBalancUzla = 0;
            string str = "";
            Uzel u;
            bool flag;
            do
            {
                str = reader.ReadLine();
                if (string.IsNullOrEmpty(str))
                    break;
                if (str == "0000")
                    break;
                switch (str.Substring(0, 4))
                {
                    case "0201":
                        var item = new Uzel();
                        if (item.LoadFromCDU(str))
                            _uzli.Add(item);
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
                                _vetvi.Add(v);
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
                        nomerBalancUzla = (int)Proverka_pust_stroki(str.Substring(24, 8));
                        uBalancUzla = Proverka_pust_stroki(str.Substring(32, 8));
                        break;
                }
            }
            while (true);

            Uzel item1 = Find_Uzel_by_Nomer(nomerBalancUzla);
            if (item1 != null)
            {
                item1.TipUzla = -1;
                item1.UZad = uBalancUzla;
                res = true;
            }
            Numeracia_Vetvi();

            return res;
        }

        private void Numeracia_Vetvi()
        {
            List<int> nomeraVetvei = new List<int>();
            foreach (vetv item in _vetvi)
            {
                if (item.Nomer != 0)
                {
                    nomeraVetvei.Add(item.Nomer);
                }
            }
            if (nomeraVetvei.Count < _vetvi.Count)
            {
                foreach (vetv item in _vetvi)
                {
                    if (item.Nomer == 0)
                    {
                        int i = 1;
                        bool p = true;
                        do
                        {
                            if (nomeraVetvei.IndexOf(i) == (-1))
                            {
                                item.Nomer = i;
                                p = false;
                                nomeraVetvei.Add(i);
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
            foreach (Uzel u in _uzli)
            {
                if (!_nomeraRaionov.Contains(u.NomerRaiona))
                {
                    _nomeraRaionov.Add(u.NomerRaiona);
                }
            }
            foreach (vetv v in _vetvi)
            {
                if (!_nomeraRaionov.Contains(v.Nomer_raiona))
                {
                    _nomeraRaionov.Add(v.Nomer_raiona);
                }
            }
        }

        public void Raschet(bool Ploskiy_Start, bool Start, bool Tip_Rascheta)   //если тип расчета истина, то в полярных координатах, иначе - в прямоугольных
        { //если плоский старт истина, то от номинальных значений расчет, иначе - от предыдущего режима. если зейдель истина, то используется стартовый алгоритм
            _nomeraUzlov = new List<int>();
            List<Uzel> opornie_GU = new List<Uzel>();
            _nomerBalancUzla = 0;

            //proverka
            int i = 0;
            int chetchik = 0;
            foreach (Uzel u in _uzli)
            {
                if ((u.TipUzla) == -1)
                    i++;
                else
                    chetchik++;

            }
            if (i == 1)
            {
                #region Определение размерностей массивов
                _y = new double[chetchik, chetchik];
                _alf = new double[chetchik, chetchik];
                _yBalanc = new double[chetchik];
                _alfBalanc = new double[chetchik];
                _G = new HashSpareMatrix(chetchik);
                _B = new HashSpareMatrix(chetchik);
                _Gb = new double[chetchik];
                _Bb = new double[chetchik];
                _Gbb = new double[chetchik + 1];
                _Bbb = new double[chetchik + 1];
                Y_1 = new Complex[chetchik, chetchik];
                #endregion

                #region Расчет Матриц Y
                _uzli.Sort();
                foreach (Uzel item in _uzli)
                {
                    switch (item.TipUzla)
                    {
                        case 0:
                            _nomeraUzlov.Add(item.NomerUzla);
                            break;
                        case 1:
                            _nomeraUzlov.Add(item.NomerUzla);
                            break;
                        case -1:
                            _nomeraUzlov.Add(item.NomerUzla);
                            _nomerBalancUzla = item.NomerUzla;
                            break;
                    }
                }
                //private void Raschet_Matrici_Y(List<int> nomera_uzlov, int nomer_balanc_uzla, ref double[,] y, ref double[,] alf, ref double[] y_balanc, ref double[] alf_balanc, ref double[,] _g, ref double[,] _b, ref double[] _gb, ref double[] _bb, ref double[] _gbb, ref double[] _bbb)
                Raschet_Matrici_Y(_nomeraUzlov, ref _G, ref _B, ref _Gb, ref _Bb, ref _Gbb, ref _Bbb,ref Y_1);
                _nomeraUzlov.Remove(_nomerBalancUzla);
                SaveMatrci_Y();
                #endregion

                List<double> U = new List<double>();
                List<double> delta = new List<double>();
                List<double> Pi = new List<double>();
                List<double> Qi = new List<double>();
                List<int> index_opor_uzlov = new List<int>();
                double Ubasis = 0;
                foreach (Uzel item in _uzli)
                {
                    switch (item.TipUzla)
                    {
                        case 0:
                            if (Ploskiy_Start == true)
                            {
                                U.Add(item.UNom);
                                delta.Add(0.0);
                            }
                            else
                            {
                                if (item.UMod != 0)
                                {
                                    U.Add(item.UMod);
                                    delta.Add((item.Angle));
                                }
                                else
                                {
                                    U.Add(item.UNom);
                                    delta.Add(0.0);
                                }
                            }
                            Pi.Add((item.PGen - item.PLoad));
                            Qi.Add((item.QGen - item.QLoad));
                            break;
                        case 1:
                            opornie_GU.Add(item);
                            //        index_opor_uzlov.Add(nomera_uzlov.IndexOf(item.Nomer_uzla));  //тут нужно будет крепко подумать по поводу старта с генераторными узлами

                            if (Ploskiy_Start == true)
                            {
                                delta.Add(0.0);
                                U.Add(item.UZad);
                                index_opor_uzlov.Add(_nomeraUzlov.IndexOf(item.NomerUzla));
                                if (Tip_Rascheta == true)
                                    Qi.Add(((item.QMax + item.QMin) / 2 - item.QLoad));
                                else
                                    Qi.Add((item.UZad * item.UZad));
                            }
                            else
                            {
                                if (item.UMod != 0)
                                {
                                    U.Add(item.UMod);
                                    //          U.Add(item.U_zad);
                                    delta.Add((item.Angle));
                                    if (Tip_Rascheta == true)
                                        Qi.Add((item.QGen - item.QLoad));
                                    else
                                        if (item.UMod == item.UZad)
                                            Qi.Add((item.UZad * item.UZad));
                                        else
                                            Qi.Add((item.QGen - item.QLoad));
                                    if (item.UMod == item.UZad)
                                        index_opor_uzlov.Add(_nomeraUzlov.IndexOf(item.NomerUzla));
                                }
                                else
                                {
                                    delta.Add(0.0);
                                    index_opor_uzlov.Add(_nomeraUzlov.IndexOf(item.NomerUzla));
                                    U.Add(item.UZad);
                                    if (Tip_Rascheta == true)
                                        Qi.Add(((item.QMax + item.QMin) / 2 - item.QLoad));
                                    else
                                        Qi.Add((item.UZad * item.UZad));
                                }
                            }

                            Pi.Add((item.PGen - item.PLoad));

                            break;
                        case -1:
                            Ubasis = item.UZad;
                            item.UMod = item.UZad;
                            break;
                    }
                }

                regim Reg = new regim(U, delta, Pi, Qi, index_opor_uzlov, Ubasis, _G, _B, _Gb, _Bb, opornie_GU, _Gbb, _Bbb, Y_1);   //может, не передавать мощность балансирующего, а считать в схеме?
                if (Tip_Rascheta == true)
                {
                    Complex[] result = Reg.Raschet_Polarniy(0.0002, _nomeraUzlov);
                    for (int n = 0; n < _nomeraUzlov.Count; n++)
                    {
                        Find_Uzel_by_Nomer(_nomeraUzlov[n]).UMod = result[n].Real;
                        Find_Uzel_by_Nomer(_nomeraUzlov[n]).Angle = result[n].Imaginary;
                        Find_Uzel_by_Nomer(_nomeraUzlov[n]).AngleDegree = (result[n].Imaginary * 180 / Math.PI);
                    }
                    Find_Uzel_by_Nomer(_nomerBalancUzla).PGen = result[_nomeraUzlov.Count].Real + Find_Uzel_by_Nomer(_nomerBalancUzla).PLoad;
                    Find_Uzel_by_Nomer(_nomerBalancUzla).QGen = result[_nomeraUzlov.Count].Imaginary + Find_Uzel_by_Nomer(_nomerBalancUzla).QLoad;
                    foreach (Uzel item in opornie_GU)
                    {
                        Find_Uzel_by_Nomer(item.NomerUzla).QGen = item.QGen;
                    }
                }
                else
                {
                    //Complex[] result1 = Reg.Raschet(0.0002, _nomeraUzlov);
                    //for (int n = 0; n < _nomeraUzlov.Count; n++)
                    //{

                    //    Find_Uzel_by_Nomer(_nomeraUzlov[n]).UMod = result1[n].Magnitude;
                    //    Find_Uzel_by_Nomer(_nomeraUzlov[n]).Angle = result1[n].Phase;
                    //    Find_Uzel_by_Nomer(_nomeraUzlov[n]).AngleDegree = (result1[n].Phase * 180 / Math.PI);
                    //}
                    //Find_Uzel_by_Nomer(_nomerBalancUzla).PGen = result1[_nomeraUzlov.Count].Real + Find_Uzel_by_Nomer(_nomerBalancUzla).PLoad;
                    //Find_Uzel_by_Nomer(_nomerBalancUzla).QGen = result1[_nomeraUzlov.Count].Imaginary + Find_Uzel_by_Nomer(_nomerBalancUzla).QLoad;
                }

                

                Raschet_moshnosti();

                //ДОБАВЛЕНО МНОЙ. НУЖНО ДЛЯ ТОГО, ЧТОБЫ РЕЗУЛЬТАТ ВЫВОДИЛСЯ ПО ВОЗРАСТАНИЮ НОМЕРОВ УЗЛОВ. Т.Е. УПОРЯДОЧИВАНИЕ
                bool fl0 = true;
                while (fl0)
                {
                    fl0 = false;
                    for (int ii = 0; ii < Uzli.Count - 1; ii++)
                    {
                        if (Uzli[ii].NomerUzla > Uzli[ii + 1].NomerUzla)
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
            for (int i = 0; i < _nomeraUzlov.Count; i++)
            {
                writer.Write(_nomeraUzlov[i].ToString() + '	');
            }

            writer.WriteLine(_nomerBalancUzla.ToString() + '	');
            for (int i = 0; i < _nomeraUzlov.Count; i++)
            {
                writer.Write(_nomeraUzlov[i].ToString() + '	');
                for (int j = 0; j < _nomeraUzlov.Count; j++)
                {
                    writer.Write(_G.getValue(i, j).ToString() + '	');

                }
                writer.WriteLine(_Gb[i].ToString() + '	');
            }
            writer.Write("B" + '	');
            writer.Write("Номера" + '	');
            for (int i = 0; i < _nomeraUzlov.Count; i++)
            {
                writer.Write(_nomeraUzlov[i].ToString() + '	');
            }

            writer.WriteLine(_nomerBalancUzla.ToString() + '	');
            for (int i = 0; i < _nomeraUzlov.Count; i++)
            {
                writer.Write(_nomeraUzlov[i].ToString() + '	');
                for (int j = 0; j < _nomeraUzlov.Count; j++)
                {
                    writer.Write(_B.getValue(i, j).ToString() + '	');

                }
                writer.WriteLine(_Bb[i].ToString() + '	');
            }
            writer.Close();
        }


        private void Raschet_Matrici_Y(List<int> nomera_uzlov, ref HashSpareMatrix g, ref HashSpareMatrix b, ref double[] _gb, ref double[] _bb, ref double[] _gbb, ref double[] _bbb, ref Complex[,] YY)
        {
            int i = 0;
            Complex[,] matrica_Y = new Complex[nomera_uzlov.Count, nomera_uzlov.Count];
            Complex[] vector_Y = new Complex[nomera_uzlov.Count - 1];

            foreach (vetv item in _vetvi)
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
               
            }

            foreach (Uzel u in _uzli)
            {

                if ((u.GSh != 0) || (u.BSh != 0))
                {
                    i = nomera_uzlov.IndexOf(u.NomerUzla);
                    matrica_Y[i, i] += new Complex(u.GSh * 0.000001, -u.BSh * 0.000001);
                }
            }

            for (int x = 0; x < nomera_uzlov.Count - 1; x++)
            {
                for (int j = 0; j < nomera_uzlov.Count - 1; j++)
                {
                    YY[x, j] = matrica_Y[x, j];
                    // alf[x, j] = (Math.PI) / 2 - matrica_Y[x, j].Phase;
                    g.setValue(x, j, matrica_Y[x, j].Real);
                    b.setValue(x, j, -matrica_Y[x, j].Imaginary);
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
            _deltaPSumm = 0;
            foreach (vetv item in _vetvi)
            {
                Z = new Complex(item.R, item.X);
                if (((item.Kt1 != 0)) || ((item.Kt2 != 0)))
                {
                    Y_nach = new Complex(item.Gc * 0.000001, item.Bc * 0.000001);
                    Complex k_t = new Complex(item.Kt1, item.Kt2);
                    Complex U_n = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).UMod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).UMod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle));
                    Complex U_k = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).UMod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).UMod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle));
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
                    Complex U_n = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).UMod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).UMod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Nachal).Angle));
                    Complex U_k = new Complex(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).UMod * Math.Cos(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle), Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).UMod * Math.Sin(Find_Uzel_by_Nomer(item.Nomer_Uzla_Konca).Angle));
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
                _deltaPSumm = _deltaPSumm + item.Delta_P;
                

            }
        }

        public List<vetv> Find_vetvi_Svyas_s_Uzlom(int nomerUzla)
        {
            var list = new List<vetv>();
            foreach (vetv v in _vetvi)
            {
                if ((v.Nomer_Uzla_Nachal == nomerUzla) || (v.Nomer_Uzla_Konca == nomerUzla))
                    list.Add(v);

            }
            return list;
        }

        

        #region XML

        public void SaveToXml(XmlTextWriter xmlOut)
        {
            xmlOut.WriteStartElement("Shema");
            xmlOut.WriteAttributeString("Version", "2");

            foreach (Uzel item in _uzli)
            {
                item.SaveToXml(xmlOut);
            }
            foreach (vetv item in Vetvi)
            {
                item.SaveToXml(xmlOut);
            }
            

            if (_uzli.Count == 0)
            {
                xmlOut.WriteStartElement("Null");
                xmlOut.WriteEndElement();
            }

            xmlOut.WriteEndElement();
        }

        public void LoadFromFile(XmlTextReader xmlIn)
        {
            //Очистка
            _uzli = new List<Uzel>();
            _vetvi = new List<vetv>();

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
                    _uzli.Add(u);
                    u.LoadFromFile(xmlIn);
                }
                if (xmlIn.Name == "Vetv")
                {
                    vetv v = new vetv();
                    _vetvi.Add(v);
                    v.LoadFromFile(xmlIn);
                }
                


            } while (!xmlIn.EOF);
        }

        #endregion
    }
}

