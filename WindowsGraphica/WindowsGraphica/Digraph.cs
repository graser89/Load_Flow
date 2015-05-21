using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace WindowsGraphica
{
    class Digraph
    {
        protected Hashtable vetvi;
        protected Hashtable uzli;
        //protected double[,] A_per;
        //protected double[,] A_postav;
        //protected double[,] A_poter;
        //protected List<int> Nomera_Uzlov;
        //protected List<int> Nomera_Gen_Uzlov;
        //protected List<int> Nomera_Load_Uzlov;
        protected dinamic_mass_d A_peredano;
        protected dinamic_mass_d A_postupilo;
        protected dinamic_mass_d A_poteri;

        

        #region Свойства
        public Hashtable Vetvi
        {
            get { return vetvi; }
            set { vetvi = value; }
        }

        public Hashtable Uzli
        {
            get { return uzli; }
            set { uzli = value; }
        }
        #endregion


        public Digraph()
        {
            A_peredano = new dinamic_mass_d("Передавалось");
            A_postupilo = new dinamic_mass_d("Поступило");
            A_poteri=new dinamic_mass_d("Потери");

        }

        public Digraph(Shema s)
        {
            vetvi = new Hashtable();
            uzli = new Hashtable();

            //rows - генераторные узлы col- нагрузочные
            A_peredano = new dinamic_mass_d("Передавалось");
            A_postupilo = new dinamic_mass_d("Поступило");
            A_poteri = new dinamic_mass_d("Потери");

            //Nomera_Uzlov = new List<int>();
            //Nomera_Load_Uzlov = new List<int>();
            //Nomera_Gen_Uzlov = new List<int>();
            foreach (Uzel u in s.Uzli)
            {
                double P = 0;
                List<vetv> list = s.Find_vetvi_Svyas_s_Uzlom(u.Nomer_uzla);
                foreach (vetv v in list)
                {
                    if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                    {
                        if (v.P_Nach > 0)
                        {
                            P = P + v.P_Nach;
                        }
                    }
                    if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                    {
                        if (v.P_Konc < 0)
                        {
                            P = P - v.P_Konc;
                        }
                    }
                }
                if (u.P_gen > 0)
                    P = P + u.P_gen;
                Digrph_uzel DiUzel = new Digrph_uzel(u, P);
                uzli.Add(u.Nomer_uzla, DiUzel);
                foreach (vetv v in list)
                {
                    if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                    {
                        if (v.P_Nach < 0)
                        {
                            Digraph_vetv vet = new Digraph_vetv(v, P);
                            vetvi.Add(vet.Nomer_Uzla_Nach * 10000 + v.Nomer, vet);
                        }
                    }
                    if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                    {
                        if (v.P_Konc > 0)
                        {
                            Digraph_vetv vet = new Digraph_vetv(v, P);
                            vetvi.Add(vet.Nomer_Uzla_Nach * 10000 + v.Nomer, vet);
                        }
                    }
                }

                if ((u.P_gen > 0))
                {
                    A_peredano.ADD_row(u.Nomer_uzla);
                    A_postupilo.ADD_row(u.Nomer_uzla);
                    A_poteri.ADD_row(u.Nomer_uzla);
                    //Nomera_Gen_Uzlov.Add(u.Nomer_uzla);
                }
                if ((u.P_load > 0))
                {
                    A_peredano.ADD_col(u.Nomer_uzla);
                    A_postupilo.ADD_col(u.Nomer_uzla);
                    A_poteri.ADD_col(u.Nomer_uzla);
                    //Nomera_Load_Uzlov.Add(u.Nomer_uzla);
                }
                    


                //Nomera_Uzlov.Add(u.Nomer_uzla);
            }
            //A_per = new double[Nomera_Gen_Uzlov.Count, Nomera_Load_Uzlov.Count];
            //A_postav = new double[Nomera_Gen_Uzlov.Count, Nomera_Load_Uzlov.Count];
            //A_poter = new double[Nomera_Gen_Uzlov.Count, Nomera_Load_Uzlov.Count];
        }

        public Digrph_uzel Find_Uzel_by_Nomer(int nomer)
        {
            if (uzli.Contains(nomer))
                return (Digrph_uzel)uzli[nomer];
            return null;
        }

        public List<Digraph_vetv> Find_vetvi_pitaia_ot_Uzla(int nomer)
        {
            List<Digraph_vetv> list = new List<Digraph_vetv>();
            ICollection col = vetvi.Keys;
            foreach (int i in col)
            {
                if ((i >= nomer * 10000) && (i < (nomer + 1) * 10000))
                {
                    list.Add((Digraph_vetv)vetvi[i]);
                }
            }
            return list;
        }

        public List<Digrph_uzel> Find_uzli_pitiuch_ot_Uzla(int nomer)
        {
            List<Digrph_uzel> list = new List<Digrph_uzel>();
            foreach (Digraph_vetv v in Find_vetvi_pitaia_ot_Uzla(nomer))
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca);
                if (list.IndexOf(u) == -1)
                {
                    list.Add(u);
                }
                foreach (Digrph_uzel item in Find_uzli_pitiuch_ot_Uzla(v.Nomer_Uzla_Konca))
                {
                    if (list.IndexOf(item) == -1)
                    {
                        list.Add(item);
                    }
                }
                //list.AddRange(Find_uzli_pitiuch_ot_Uzla(v.Nomer_Uzla_Konca));                
            }
            return list;
        }

        protected void Zapolnenie_Matrx_A(int nomer, int nomer_gen, double P_konc, double P_nach)
        {
            int index1 = A_peredano.Index_row.IndexOf(nomer_gen); //Nomera_Gen_Uzlov.IndexOf(nomer_gen);
            List<Digrph_uzel> list = new List<Digrph_uzel>();
            foreach (Digraph_vetv v in Find_vetvi_pitaia_ot_Uzla(nomer))
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca);
                double P_nach1 = P_nach * v.P_nach_otn;
                double P_konc1 = P_konc * v.P_konc_otn;
                //int index2 = A_peredano.Index_col.IndexOf(v.Nomer_Uzla_Konca); //Nomera_Load_Uzlov.IndexOf(v.Nomer_Uzla_Konca);
                if (A_peredano.Index_col.Contains(v.Nomer_Uzla_Konca))
                {
                    A_peredano.Add_elemen(nomer_gen, v.Nomer_Uzla_Konca, u.P_load_otn * P_nach1);
                    A_postupilo.Add_elemen(nomer_gen, v.Nomer_Uzla_Konca, u.P_load_otn * P_konc1);
                    //A_per[index1, index2] += u.P_load_otn * P_nach1;
                    //A_postav[index1, index2] += u.P_load_otn * P_konc1;
                }
                Zapolnenie_Matrx_A(u.Nomer, nomer_gen, P_konc1, P_nach1);
            }
 
        }

        public void Formirov_Matrix_A()
        {
            foreach (int i in A_peredano.Index_row)
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(i);
                Zapolnenie_Matrx_A(i, i, u.P_gen, u.P_gen);
            }
            for (int k = 0; k < A_peredano.Index_row.Count; k++)
            {
                for (int l = 0; l < A_peredano.Index_col.Count; l++)
                {
                    int i1 = A_peredano.Index_row[k];
                    int i2 = A_peredano.Index_col[l];
                    A_poteri.Add_elemen(i1, i2, A_peredano.get_value(i1,i2) - A_postupilo.get_value(i1, i2));
                    //A_poter[k, l] = A_per[k, l] - A_postav[k, l];
                }
            }
        }

        #region XML
        
        /*
        public void SaveToXml(XmlTextWriter XmlOut)
        {
            XmlOut.WriteStartElement("Digraph");
            XmlOut.WriteAttributeString("Nomera_Load_Uzlov", Nomera_Load_Uzlov.ToString());
            int j = 0;
            foreach (int i in Nomera_Gen_Uzlov)
            {
                XmlOut.WriteStartElement("generator");
                XmlOut.WriteAttributeString("nomer_uzla", i.ToString());
                int l = 0;
                foreach (int k in Nomera_Load_Uzlov)
                {
                    XmlOut.WriteAttributeString("peredav_to" + k.ToString(), A_per[j, l].ToString());
                    XmlOut.WriteAttributeString("postupilo_to" + k.ToString(), A_postav[j, l].ToString());
                    XmlOut.WriteAttributeString("poteri_to" + k.ToString(), A_poter[j, l].ToString());
                    l++;
                }
                j++;
                XmlOut.WriteEndElement();
            }
            XmlOut.WriteEndElement();
        }
        */
        public void save(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Вывод данных по направленному графу");
            writer.WriteLine();
            A_peredano.Save(writer);
            A_postupilo.Save(writer);
            A_poteri.Save(writer);      
        }
        /*
        public double Poteri_dlya_Uzla(int nomer)
        {
            double p = 0;
            if (Nomera_Load_Uzlov.IndexOf(nomer) != -1)
            {
                p = 0;
                for (int i = 0; i < Nomera_Gen_Uzlov.Count; i++)
                {
                    p = p + A_poter[i, Nomera_Load_Uzlov.IndexOf(nomer)];
                }
            }
            return p;

        }

        public double Postavleno_v_uzel(int nomer)
        {
            double p = 0;
            if (Nomera_Load_Uzlov.IndexOf(nomer) != -1)
            {
                p = 0;
                for (int i = 0; i < Nomera_Gen_Uzlov.Count; i++)
                {
                    p = p + A_postav[i, Nomera_Load_Uzlov.IndexOf(nomer)];
                }
            }
            return p;
        }

        public double summ_poteri()
        {
            double p = 0;
            for (int j = 0; j < Nomera_Load_Uzlov.Count; j++)
            {
                p = 0;
                for (int i = 0; i < Nomera_Gen_Uzlov.Count; i++)
                {
                    p = p + A_poter[i, j];
                }
            }
            return p;
        }

        public double otpusk()
        {
            double p = 0;
            for (int j = 0; j < Nomera_Load_Uzlov.Count; j++)
            {
                p = 0;
                for (int i = 0; i < Nomera_Gen_Uzlov.Count; i++)
                {
                    p = p + A_per[i, j];
                }
            }
            return p;
        }

        public double nagruz_suum()
        {
            double p = 0;
            for (int j = 0; j < Nomera_Load_Uzlov.Count; j++)
            {
                p = 0;
                for (int i = 0; i < Nomera_Gen_Uzlov.Count; i++)
                {
                    p = p + A_postav[i, j];
                }
            }
            return p;
        }
        */
        #endregion

    }

    class dinamic_mass_d
    {
        List<int> index_row;//массив интдексов(номеров того по чему обращаются) у строк        
        List<int> index_col;
        string nazvania;

        public List<int> Index_row
        {
            get { return index_row; }
            set { index_row = value; }
        }
        public List<int> Index_col
        {
            get { return index_col; }
            set { index_col = value; }
        }

        List<List<double>> Massiv;

        public dinamic_mass_d(List<int> index_rows,string nazvan)
        {
            nazvania = nazvan;
            index_row = new List<int>();
            index_row.AddRange(index_rows);
            index_col = new List<int>();
            Massiv = new List<List<double>>();
            for (int i = 0; i < index_row.Count; i++)
            {
                Massiv.Add(new List<double>(index_col.Count));
            }
        }
        public dinamic_mass_d(List<int> index_rows, List<int> index_cols, string nazvan)
        {
            nazvania = nazvan;
            index_row = new List<int>();
            index_row.AddRange(index_rows);
            index_col = new List<int>();
            index_col.AddRange(index_cols);
            Massiv = new List<List<double>>();
            for (int i = 0; i < index_row.Count; i++)
            {
                Massiv.Add(new List<double>());
                for (int j = 0; j < index_col.Count; j++)
                {
                    Massiv[i].Add(0);
                }
            }


        }

        public dinamic_mass_d(dinamic_mass_d mass, string nazvan)
        {

            nazvania = nazvan;
            index_row = new List<int>();
            index_row.AddRange(mass.Index_row);
            index_col = new List<int>();
            index_col.AddRange(mass.Index_col);
            Massiv = new List<List<double>>();
            for (int i = 0; i < index_row.Count; i++)
            {
                Massiv.Add(new List<double>());
                for (int j = 0; j < index_col.Count; j++)
                {
                    Massiv[i].Add(mass.get_value(mass.Index_row[i],mass.Index_col[j]));
                }
            }

        }

        public void Add_dinamic_mass_d(dinamic_mass_d mass)
        {
            foreach (int i in mass.Index_row)
            {
                foreach (int j in mass.Index_col)
                {
                    Add_elemen(i, j, mass.get_value(i, j));
                }
            }
 
        }

        public dinamic_mass_d(string nazvan)
        {
            nazvania = nazvan;
            index_row = new List<int>();            
            index_col = new List<int>();
            Massiv = new List<List<double>>();            
        }



        public void ADD_col(int index_col_elem)//добовляем столбец
        {
            if (!index_col.Contains(index_col_elem))
            {
                index_col.Add(index_col_elem);
                for (int j = 0; j < index_row.Count; j++)
                {
                    Massiv[j].Add(0);
                }
            }
        }
        public void ADD_row(int index_row_elem)//добавляем строку
        {
            if (!index_row.Contains(index_row_elem))
            {
                index_row.Add(index_row_elem);
                Massiv.Add(new List<double>());
                for (int j = 0; j < index_col.Count; j++)
                {
                    Massiv[index_row.IndexOf(index_row_elem)].Add(0);
                }
            }
        }

        public void Add_elemen(int index_rows, int index_cols, double element)
        {
            if (index_row.Contains(index_rows))
            {
                if (index_col.Contains(index_cols))
                {
                    Massiv[index_row.IndexOf(index_rows)][index_col.IndexOf(index_cols)] += element;
                }
                else
                {
                    ADD_col(index_cols);
                    Massiv[index_row.IndexOf(index_rows)][index_col.IndexOf(index_cols)] += element;
                }
            }
            else
            {
                ADD_row(index_rows);
                if (index_col.Contains(index_cols))
                {
                    Massiv[index_row.IndexOf(index_rows)][index_col.IndexOf(index_cols)] += element;
                }
                else
                {
                    ADD_col(index_cols);
                    Massiv[index_row.IndexOf(index_rows)][index_col.IndexOf(index_cols)] += element;
                }
            }
        }

        public double get_value(int index_r, int index_c)
        {
            if ((index_row.Contains(index_r))&&(index_col.Contains(index_c)))
                return Massiv[index_row.IndexOf(index_r)][index_col.IndexOf(index_c)];
            return 0;
        }

        public double get_summ_col(int index_col)
        {
            double s = 0;
            foreach (int i in Index_row)
            {
                s += get_value(i, index_col);
            }
            return s;
        }
        public double get_summ_row(int index_row)
        {
            double s = 0;
            foreach (int i in Index_col)
            {
                s += get_value(index_row,i);
            }
            return s;
        }



        public void Save(StreamWriter writer)
        {
            writer.WriteLine("");
            writer.WriteLine(nazvania);
            writer.WriteLine("");
            writer.Write(" ;");
            for (int j=0;j<index_col.Count;j++)
                writer.Write(index_col[j].ToString()+" ;");
            writer.WriteLine("");

            for (int i = 0; i < index_row.Count ; i++)
            {
                writer.Write(index_row[i].ToString()+" ;");
                for (int j = 0; j < index_col.Count; j++)
                    writer.Write(Massiv[i][j].ToString() + " ;");
                writer.WriteLine("");
            }
        }

        
    }

    class Digraph_subsystem 
    {
        protected Hashtable vetvi;
        protected Hashtable uzli;
        protected dinamic_mass_d A_peredano;

        internal dinamic_mass_d A_peredano1
        {
            get { return A_peredano; }
            set { A_peredano = value; }
        }
        protected dinamic_mass_d A_postupilo;

        internal dinamic_mass_d A_postupilo1
        {
            get { return A_postupilo; }
            set { A_postupilo = value; }
        }
        protected dinamic_mass_d A_poteri;

        internal dinamic_mass_d A_poteri1
        {
            get { return A_poteri; }
            set { A_poteri = value; }
        }
        dinamic_mass_d A_generator;

        internal dinamic_mass_d A_generator1
        {
            get { return A_generator; }
            set { A_generator = value; }
        }
        dinamic_mass_d A_gen_otn;
        dinamic_mass_d A_nagr;

        internal dinamic_mass_d A_nagr1
        {
            get { return A_nagr; }
            set { A_nagr = value; }
        }
        dinamic_mass_d A_nagr_otn;
        int nomer_raiona=0;
        private dinamic_mass_d A_poteri_raion;

        public dinamic_mass_d A_poteri_raion1
        {
            get { return A_poteri_raion; }
            set { A_poteri_raion = value; }
        }
        dinamic_mass_d A_promesutok;


        #region Свойства
        public Hashtable Vetvi
        {
            get { return vetvi; }
            set { vetvi = value; }
        }

        public Hashtable Uzli
        {
            get { return uzli; }
            set { uzli = value; }
        }
        #endregion



        public Digraph_subsystem(Shema s, int nomer_rai)
        {
            nomer_raiona=nomer_rai;
            vetvi = new Hashtable();
            uzli = new Hashtable();

            //rows - генераторные узлы col- нагрузочные
            A_peredano = new dinamic_mass_d("Передавалось");
            A_postupilo = new dinamic_mass_d("Поступило");
            A_poteri = new dinamic_mass_d("Потери");

            //rows - номера районов col- генераторные узлы
            A_generator= new dinamic_mass_d(s.Nomera_raionov,"Поступление");           
            //rows - номера районов col- нагрузочные узлы
            A_nagr=new dinamic_mass_d(s.Nomera_raionov,"Отпуск");

            foreach (Uzel u in s.Uzli)
            {
                bool flag = false;//ФЛаг указывает на то соеденины с даным узлом ветви нашей подсистемы
                double P = 0;//summ
                double P_gen;
                double P_nagr;
                double P_nagr_s=0;
                List<vetv> list = s.Find_vetvi_Svyas_s_Uzlom(u.Nomer_uzla);
                foreach (vetv v in list)
                {
                    if (v.Nomer_raiona == nomer_raiona)
                    {
                        flag = true;
                    }
                    else 
                    {
                        if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                        {
                            if (v.P_Nach < 0)
                            {
                                P_nagr_s += (-v.P_Nach);
                            }
                        }
                        if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                        {
                            if (v.P_Konc > 0)
                            {
                                P_nagr_s += v.P_Konc;
                            }
                        }
                    }


                    if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                    {
                        if (v.P_Nach > 0)
                        {
                            P = P + v.P_Nach;
                        }
                    }
                    if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                    {
                        if (v.P_Konc < 0)
                        {
                            P = P - v.P_Konc;
                        }
                    }
                }
                if (u.P_gen > 0)
                    P = P + u.P_gen;
                
                
                P_nagr_s += u.P_load;
                

                /*
                if ((u.Nomer_raiona == nomer_raiona) || (flag))
                {
                    Digrph_uzel DiUzel = new Digrph_uzel(u, P);
                    uzli.Add(u.Nomer_uzla, DiUzel);
                }
                Digrph_uzel DiUzel = new Digrph_uzel(u, P);
                uzli.Add(u.Nomer_uzla, DiUzel);
                
                 */
                if ((u.Nomer_raiona == nomer_raiona) || (flag))
                {
                    //заносим генрацию и нагрузку в таблици отпуска поступления
                    if (u.P_gen > 0)
                    {
                        A_generator.Add_elemen(u.Nomer_raiona, u.Nomer_uzla, u.P_gen );
                    }
                    if (u.P_load > 0)
                    {
                        A_nagr.Add_elemen(u.Nomer_raiona, u.Nomer_uzla, u.P_load );
                    }
                    P_gen = u.P_gen;
                    P_nagr = u.P_load;
                    foreach (vetv v in list)
                    {
                        if (v.Nomer_raiona == nomer_raiona)
                        {
                            if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                            {
                                if (v.P_Nach < 0)
                                {
                                    Digraph_vetv vet = new Digraph_vetv(v, P);
                                    if (!vetvi.ContainsKey(vet.Nomer_Uzla_Nach * 10000 + v.Nomer))
                                    vetvi.Add(vet.Nomer_Uzla_Nach * 10000 + v.Nomer, vet);
                                }
                            }
                            if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                            {
                                if (v.P_Konc > 0)
                                {
                                    Digraph_vetv vet = new Digraph_vetv(v, P);
                                    if (!vetvi.ContainsKey(vet.Nomer_Uzla_Nach * 10000 + v.Nomer))
                                        vetvi.Add(vet.Nomer_Uzla_Nach * 10000 + v.Nomer, vet);
                                }
                            }
                        }
                        else //надо поступление и отпуск добавлять в соотвед таблицы и увеличивать генерация о потребление
                        {
                            if (v.Nomer_Uzla_Nachal == u.Nomer_uzla)
                            {
                                if (v.P_Nach < 0)//мощность вытекает из узла
                                {
                                    A_nagr.Add_elemen(v.Nomer_raiona, u.Nomer_uzla, -v.P_Nach);
                                    P_nagr += (-v.P_Nach);
                                }
                                else
                                {
                                    A_generator.Add_elemen(v.Nomer_raiona, u.Nomer_uzla, v.P_Nach);
                                    P_gen += v.P_Nach;
                                }
                            }
                            if (v.Nomer_Uzla_Konca == u.Nomer_uzla)
                            {
                                if (v.P_Konc > 0)
                                {
                                    A_nagr.Add_elemen(v.Nomer_raiona, u.Nomer_uzla, v.P_Konc );
                                    P_nagr += (v.P_Konc);
                                }
                                else
                                {
                                    A_generator.Add_elemen(v.Nomer_raiona, u.Nomer_uzla, -v.P_Konc);
                                    P_gen += (-v.P_Konc);
                                }
                            }

                        }
                    }

                    Digrph_uzel DiUzel = new Digrph_uzel(u.Nomer_uzla, P, P_gen, P_nagr, u.Nomer_raiona);
                    uzli.Add(u.Nomer_uzla, DiUzel);

                    if ((P_gen > 0))
                    {
                        A_peredano.ADD_row(u.Nomer_uzla);
                        A_postupilo.ADD_row(u.Nomer_uzla);
                        A_poteri.ADD_row(u.Nomer_uzla);
                        //Nomera_Gen_Uzlov.Add(u.Nomer_uzla);
                    }
                    if ((P_nagr > 0))
                    {
                        A_peredano.ADD_col(u.Nomer_uzla);
                        A_postupilo.ADD_col(u.Nomer_uzla);
                        A_poteri.ADD_col(u.Nomer_uzla);
                        //Nomera_Load_Uzlov.Add(u.Nomer_uzla);
                    }
                }
            }

            //rows - номера районов col- генераторные узлы            
            A_gen_otn = new dinamic_mass_d(A_generator.Index_row,A_generator.Index_col, "Поступление отн.ед");
            //rows - номера районов col- нагрузочные узлы            
            A_nagr_otn = new dinamic_mass_d(A_nagr.Index_row, A_nagr.Index_col, "Отпуск отн.ед");

            foreach (int i in A_generator.Index_col)
            {
                double summ = A_generator.get_summ_col(i);
                foreach (int j in A_generator.Index_row)
                {
                    A_gen_otn.Add_elemen(j, i, A_generator.get_value(j, i) / summ);
                }
            }
            foreach (int i in A_nagr.Index_col)
            {
                double summ = A_nagr.get_summ_col(i);
                foreach (int j in A_nagr.Index_row)
                {
                    A_nagr_otn.Add_elemen(j, i, A_nagr.get_value(j, i) / summ);
                }
            }

        }

        public Digrph_uzel Find_Uzel_by_Nomer(int nomer)
        {
            if (uzli.Contains(nomer))
                return (Digrph_uzel)uzli[nomer];
            return null;
        }

        public List<Digraph_vetv> Find_vetvi_pitaia_ot_Uzla(int nomer)
        {
            List<Digraph_vetv> list = new List<Digraph_vetv>();
            ICollection col = vetvi.Keys;
            foreach (int i in col)
            {
                if ((i >= nomer * 10000) && (i < (nomer + 1) * 10000))
                {
                    list.Add((Digraph_vetv)vetvi[i]);
                }
            }
            return list;
        }

        public List<Digrph_uzel> Find_uzli_pitiuch_ot_Uzla(int nomer)
        {
            List<Digrph_uzel> list = new List<Digrph_uzel>();
            foreach (Digraph_vetv v in Find_vetvi_pitaia_ot_Uzla(nomer))
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca);
                if (list.IndexOf(u) == -1)
                {
                    list.Add(u);
                }
                foreach (Digrph_uzel item in Find_uzli_pitiuch_ot_Uzla(v.Nomer_Uzla_Konca))
                {
                    if (list.IndexOf(item) == -1)
                    {
                        list.Add(item);
                    }
                }
                //list.AddRange(Find_uzli_pitiuch_ot_Uzla(v.Nomer_Uzla_Konca));                
            }
            return list;
        }

        protected void Zapolnenie_Matrx_A(int nomer, int nomer_gen, double P_konc, double P_nach)
        {
            int index1 = A_peredano.Index_row.IndexOf(nomer_gen); //Nomera_Gen_Uzlov.IndexOf(nomer_gen);
            List<Digrph_uzel> list = new List<Digrph_uzel>();
            foreach (Digraph_vetv v in Find_vetvi_pitaia_ot_Uzla(nomer))
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(v.Nomer_Uzla_Konca);
                double P_nach1 = P_nach * v.P_nach_otn;
                double P_konc1 = P_konc * v.P_konc_otn;
                //int index2 = A_peredano.Index_col.IndexOf(v.Nomer_Uzla_Konca); //Nomera_Load_Uzlov.IndexOf(v.Nomer_Uzla_Konca);
                if (A_peredano.Index_col.Contains(v.Nomer_Uzla_Konca))
                {
                    A_peredano.Add_elemen(nomer_gen, v.Nomer_Uzla_Konca, u.P_load_otn * P_nach1);
                    A_postupilo.Add_elemen(nomer_gen, v.Nomer_Uzla_Konca, u.P_load_otn * P_konc1);
                    //A_per[index1, index2] += u.P_load_otn * P_nach1;
                    //A_postav[index1, index2] += u.P_load_otn * P_konc1;
                }
                Zapolnenie_Matrx_A(u.Nomer, nomer_gen, P_konc1, P_nach1);
            }
        }

        public void Formirov_Matrix_A()
        {
            foreach (int i in A_peredano.Index_row)
            {
                Digrph_uzel u = Find_Uzel_by_Nomer(i);
                Zapolnenie_Matrx_A(i, i, u.P_gen, u.P_gen);
            }
            for (int k = 0; k < A_peredano.Index_row.Count; k++)
            {
                for (int l = 0; l < A_peredano.Index_col.Count; l++)
                {
                    int i1 = A_peredano.Index_row[k];
                    int i2 = A_peredano.Index_col[l];
                    A_poteri.Add_elemen(i1, i2, A_peredano.get_value(i1, i2) - A_postupilo.get_value(i1, i2));
                    //A_poter[k, l] = A_per[k, l] - A_postav[k, l];
                }
            }
        }

        public void Raschet_A()
        {
            Formirov_Matrix_A();
            A_promesutok = new dinamic_mass_d(A_gen_otn.Index_row, A_poteri.Index_col, "Pomesutok");
            for (int i = 0; i < A_gen_otn.Index_row.Count; i++)
            {
                for (int j = 0; j < A_poteri.Index_col.Count; j++)
                {
                    for (int k = 0; k < A_gen_otn.Index_col.Count; k++)
                    {
                        int i1 = A_gen_otn.Index_row[i];
                        int i2 = A_poteri.Index_col[j];
                        int i3 = A_gen_otn.Index_col[k];
                        A_promesutok.Add_elemen(i1, i2, A_gen_otn.get_value(i1, i3) * A_poteri.get_value(i3, i2));
                    }
                }
            }

            A_poteri_raion = new dinamic_mass_d(A_gen_otn.Index_row, A_gen_otn.Index_row, "Poteri_при передачи между районами");

            for (int i = 0; i < A_promesutok.Index_row.Count; i++)
            {
                for (int j = 0; j < A_nagr.Index_row.Count; j++)
                {
                    for (int k = 0; k < A_promesutok.Index_col.Count; k++)
                    {
                        int i1 = A_promesutok.Index_row[i];
                        int i2 = A_nagr_otn.Index_row[j];
                        int i3 = A_promesutok.Index_col[k];
                        A_poteri_raion.Add_elemen(i1, i2, A_promesutok.get_value(i1, i3) * A_nagr_otn.get_value(i2, i3));
                    }
                }
            }

        }

        public new void Save(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Вывод данных по направленному графу подсистема №" + nomer_raiona.ToString() );
            writer.WriteLine();
            A_generator.Save(writer);
            A_gen_otn.Save(writer);
            A_nagr.Save(writer);
            A_nagr_otn.Save(writer);
            A_peredano.Save(writer);
            A_postupilo.Save(writer);
            A_poteri.Save(writer);
            A_promesutok.Save(writer); 
            A_poteri_raion.Save(writer);
        }
    }

    class Digrph_uzel
    {
        double _P_summ;//суммарная втикающая или генерируемая в узел мощность;
        double _P_load;
        double _P_gen;

        public double P_gen
        {
            get { return _P_gen; }
            set { _P_gen = value; }
        }
        double _P_gen_otn;
        double _P_load_otn;

        public double P_load_otn
        {
            get { return _P_load_otn; }
            set { _P_load_otn = value; }
        }
        int nomer;
        int nomer_raiona;


        public int Nomer
        {
            get { return nomer; }
            set { nomer = value; }
        }

        public Digrph_uzel(int nom, double P_sum, double P_gen, double P_load,int nom_raion)
        {
            nomer_raiona = nom_raion;
            nomer = nom;
            _P_summ = P_sum;
            _P_load = P_load;
            _P_gen = P_gen;
            _P_load_otn = P_load / P_sum;
            _P_gen_otn = P_gen / P_sum;
        }

        public Digrph_uzel(Uzel u, double P_sum)
        {
            nomer_raiona = u.Nomer_raiona;
            nomer = u.Nomer_uzla;
            _P_summ = P_sum;
            _P_load = u.P_load;
            _P_load_otn = _P_load / _P_summ;
            _P_gen = u.P_gen;
            _P_gen_otn = _P_gen / _P_summ;
        }
    }

    class Digraph_vetv
    {
        double _P_nach_otn;        
        double _P_konc_otn;        
        double _P_nach;
        double _P_konc;
        int nomer;
        int nomer_Uzla_Nach;
        int nomer_Uzla_Konca;
        int nomer_raiona;

        #region Свойства

        public double P_nach_otn
        {
            get { return _P_nach_otn; }
            set { _P_nach_otn = value; }
        }

        public double P_konc_otn
        {
            get { return _P_konc_otn; }
            set { _P_konc_otn = value; }
        }

        public int Nomer_Uzla_Nach
        {
            get { return nomer_Uzla_Nach; }
            set { nomer_Uzla_Nach = value; }
        }

        public int Nomer_Uzla_Konca
        {
            get { return nomer_Uzla_Konca; }
            set { nomer_Uzla_Konca = value; }
        }
        #endregion

        public Digraph_vetv(vetv v, double P_sum_uzla)
        {
            nomer = v.Nomer;
            if (v.P_Nach > 0)
            {
                nomer_Uzla_Nach = v.Nomer_Uzla_Konca;
                nomer_Uzla_Konca = v.Nomer_Uzla_Nachal;
                _P_nach = v.P_Konc;
                _P_konc = v.P_Nach;
            }
            if (v.P_Nach < 0)
            {
                nomer_Uzla_Nach = v.Nomer_Uzla_Nachal;
                nomer_Uzla_Konca = v.Nomer_Uzla_Konca;
                _P_nach = -v.P_Nach;
                _P_konc = -v.P_Konc;
            }
            _P_konc_otn = _P_konc / P_sum_uzla;
            _P_nach_otn = _P_nach / P_sum_uzla;
        }
    }

    class Poteri_report_raion
    {
        double _P_postuplenie;
        double _P_otpuck_sob_potrebit;
        double _P_otpuck_transit;

    }
}
