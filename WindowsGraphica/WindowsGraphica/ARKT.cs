using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGraphica
{
    public class ARKT
    {
        double ustavka_po_toku;
        double ustavka_const;
        double Chustvitelnost;
        int ustavka_time;
        double Shag_Otpaiki;

        Shema shema;
        int Izmenenie_otpaiki;

        public int Izmenenie_otpaiki1
        {
            get { return Izmenenie_otpaiki; }
            set { Izmenenie_otpaiki = value; }
        }
        int nomer_Vetvi;
        int nomer_Uzla;

        vetv vetka;
        Uzel uzelok;

        int timer_vverh;
        int timer_vniz;

        public int Nomer_Uzla
        {
            get { return nomer_Uzla; }
            set {
                uzelok = shema.Find_Uzel_by_Nomer(value);
                nomer_Uzla = value;}
        }

        public delegate void Nomer_Uzla_Changed(Iconnect_uzel sender, Chanche_nomer_Uzla args);
        public event Nomer_Uzla_Changed Nomer_Uzla_Izmenen;
        public delegate void Nomer_Vetvi_Changed(Iconnect_vetv sender, Chanche_nomer_Vetvi args);
        public event Nomer_Vetvi_Changed Nomer_Vetvi_Izmenen;
        
        public int Nomer_Vetvi
        {
            get { return nomer_Vetvi; }
            set {
                vetka = shema.Find_Vetv_by_Nomer(value);
                Nomer_Uzla = vetka.Nomer_Uzla_Konca;
                nomer_Vetvi = value; }
        }

        

        private void add_timer_verh()
        {
            timer_vverh++;
        }
        private void stop_timer_vverh() { timer_vverh = 0; }

        private bool Is_timer_vverh()
        {
            if (timer_vverh == ustavka_time)
                return true;
            else
                return false;
        }

        private bool Is_timer_vniz()
        {
            if (timer_vniz == ustavka_time)
                return true;
            else
                return false;
        }

        private void stop_timer_vniz() { timer_vniz = 0; }

        public void add_timer_vniz()
        {
            timer_vniz++;
        }


        public void Add_time()
        {
            Izmenenie_otpaiki = 0;
            double delta_U1 = (uzelok.U_mod - uzelok.U_nom) / (uzelok.U_nom)*100;
            double I = vetka.I_Nach*1000;
            double delta_U_dob = ustavka_po_toku * I + ustavka_const;
            double delta_U = -delta_U_dob + delta_U1;
            if (delta_U > Chustvitelnost / 2)
            {
                add_timer_verh();
                stop_timer_vniz();
                if (Is_timer_vverh())
                {
                    Izmenenie_otpaiki = -(int)Math.Round(delta_U / Shag_Otpaiki);
                }
            }
            if (delta_U < -Chustvitelnost / 2)
            {
                add_timer_vniz();
                stop_timer_vverh();
                if (Is_timer_vniz())
                {
                    Izmenenie_otpaiki = -(int)Math.Round(delta_U / Shag_Otpaiki);
                }
            }
        }

        public bool Is_Izmenen()
        {
            if (Izmenenie_otpaiki != 0)
                return true;
            else
                return false;                     
        }

        public ARKT(Shema s)
        {
            ustavka_po_toku = 0.0947;
            ustavka_const = -7.177;
            Chustvitelnost = 2;
            ustavka_time = 3;
            Shag_Otpaiki = 1.78;
            timer_vniz = 0;
            timer_vverh = 0;
            shema = s;
        }

    }
}
