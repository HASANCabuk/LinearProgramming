using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
namespace MPS_Simpleks_
{
    class MPSRead
    {
        Standartlastır S;
        Denklem D;
        List<Denklem> denklemler = new List<Denklem>();
        string tur, tut,pName;
        ArrayList satir = new ArrayList();
        string[] bosluk = new string[2] { " ", "\t" };
        Denklem.degisken degisken = new Denklem.degisken();
        public void Oku(string dosya)
        {      
            StreamReader oku = new StreamReader(dosya);
            tut = oku.ReadLine().Trim();
            while (tut != "ENDATA")
            {
                string[] bos = tut.Split(' ');
                if (bos[0] != "*")
                {

                    if (tut == "OBJSENSE")
                    {
                        OBJSENSE(oku);
                    }
                    else
                    {
                        if (tut == "ROWS")
                        {
                            ROWS(oku);
                        }
                        else
                        {
                            if (tut == "COLUMNS")
                            {
                                COLUMNS(oku);
                            }
                            else
                            {
                                if (tut == "RHS")
                                {
                                    RHS(oku);
                                }
                                else
                                {
                                    if (tut == "BOUNDS")
                                    {
                                        BOUNDS(oku);
                                    }
                                    else
                                    if (tut.Split(' ')[0] == "NAME")
                                    {
                                    
                                        pName = tut.Trim().Replace(" ",string.Empty).Split(' ')[0];
                                        tut = oku.ReadLine().Trim();
                                    }
                                }
                            }
                        }
                    }

                }else
                    tut = oku.ReadLine().Trim();


            }
            S = new Standartlastır(denklemler, tur,pName);
        }
        void OBJSENSE(StreamReader O)
        {
            tur = O.ReadLine().Trim();
            tut = O.ReadLine();
        }
        void ROWS(StreamReader O)
        {
            while ((tut = O.ReadLine().Trim()) != "COLUMNS")
            {
                D = new Denklem();
                string[] gecici = tut.Split(bosluk, StringSplitOptions.RemoveEmptyEntries);
                D.esitlik = gecici[0];
                D.rows = gecici[1];
                denklemler.Add(D);
            }
        }
        void COLUMNS(StreamReader O)
        {

            while ((tut = O.ReadLine().Trim()) != "RHS")
            {
                tut = tut.Replace('.', ',');
                string[] satir = tut.Split(bosluk, StringSplitOptions.RemoveEmptyEntries);
                if (satir.Length == 3)
                {
                    foreach (var item in denklemler)
                    {
                        if (item.rows == satir[1])
                        {
                            degisken.adi = satir[0];
                            degisken.katsayi = Convert.ToDouble(satir[2]);
                            item.degiskenler.Add(degisken);
                        }
                    }
                }
                else if (satir.Length == 5)
                {
                    foreach (var item in denklemler)
                    {
                        if (item.rows == satir[1])
                        {
                            degisken.adi = satir[0];
                            degisken.katsayi = Convert.ToDouble(satir[2]);
                            item.degiskenler.Add(degisken);
                        }
                        if (item.rows == satir[3])
                        {
                            degisken.adi = satir[0];
                            degisken.katsayi = Convert.ToDouble(satir[4]);
                            item.degiskenler.Add(degisken);
                        }
                    }
                }
            }
            foreach (var item in denklemler)
            {
                if (item.esitlik == "N")
                {
                    for (int i = 0; i < item.degiskenler.Count; i++)
                    {
                        degisken = item.degiskenler[i];
                        degisken.katsayi *= -1;
                        item.degiskenler[i] = degisken;
                        foreach (var item2 in denklemler)
                        {
                            if (i < item2.degiskenler.Count)
                            {
                                if (item.degiskenler[i].adi != item2.degiskenler[i].adi)
                                {
                                    degisken.adi = item.degiskenler[i].adi;
                                    degisken.katsayi = 0;
                                    item2.degiskenler.Insert(i, degisken);
                                }
                            }

                            if (i >= item2.degiskenler.Count)
                            {
                                degisken.adi = item.degiskenler[i].adi;
                                degisken.katsayi = 0;
                                item2.degiskenler.Add(degisken);
                            }
                        }
                    }
                }
            }
        }
        void RHS(StreamReader O)
        {
            while ((tut = O.ReadLine().Trim()) != "BOUNDS")
            {
                tut = tut.Replace('.', ',');
                string[] satir = tut.Split(bosluk, StringSplitOptions.RemoveEmptyEntries);
                if (satir.Length == 3)
                {
                    foreach (var item in denklemler)
                    {
                        if (item.rows == satir[1])
                        {
                            item.sts = Convert.ToDouble(satir[2]);
                        }
                    }
                }
                else if (satir.Length == 5)
                {
                    foreach (var item in denklemler)
                    {
                        if (item.rows == satir[1])
                        {
                            item.sts = Convert.ToDouble(satir[2]);
                        }
                        if (item.rows == satir[3])
                        {
                            item.sts = Convert.ToDouble(satir[4]);
                        }
                    }
                }
            }
        }
        void BOUNDS(StreamReader O)
        {
            int sayac = 0;
            while ((tut = O.ReadLine().Trim()) != "ENDATA")
            {
                sayac++;
                tut = tut.Replace('.', ',');
                string[] satir = tut.Split(bosluk, StringSplitOptions.RemoveEmptyEntries);
                if (satir[0] == "FR")
                {
                    for (int i = 0; i < denklemler.Count; i++)
                    {
                        for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                        {
                            if (denklemler[i].degiskenler[j].adi == satir[2])
                            {
                                degisken = denklemler[i].degiskenler[j];
                                degisken.free = true;
                                denklemler[i].degiskenler[j] = degisken;
                            }

                        }
                    }
                }

                else
                {
                    for (int i = 0; i < denklemler.Count; i++)
                    {
                        for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                        {
                            if (satir[0] == "UP")
                            {
                                if (denklemler[i].degiskenler[j].adi == satir[2])
                                {
                                    degisken = denklemler[i].degiskenler[j];
                                    degisken.max = Convert.ToDouble(satir[3]);
                                    denklemler[i].degiskenler[j] = degisken;
                                }
                                else// Eger adi yok ise max en yuksek olsun
                                {
                                    if (denklemler[i].degiskenler[j].max == 0)//ama önceden bir deger atıldıysa degişmesin
                                    {
                                        degisken = denklemler[i].degiskenler[j];
                                        degisken.max = double.MaxValue;
                                        denklemler[i].degiskenler[j] = degisken;
                                    }

                                }
                            }
                            if (satir[0] == "LO")
                            {
                                if (denklemler[i].degiskenler[j].adi == satir[2])
                                {
                                    degisken = denklemler[i].degiskenler[j];
                                    degisken.min = Convert.ToDouble(satir[3]);
                                    denklemler[i].degiskenler[j] = degisken;
                                }
                                else
                                {
                                    if (degisken.min == 0)//eger daha once bir deger atıldıysa degiştirilmesin diye
                                    {
                                        degisken = denklemler[i].degiskenler[j];
                                        degisken.min = 0;
                                        denklemler[i].degiskenler[j] = degisken;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            if (sayac == 0)//BOUNDS bölümüne birşey yazılmamış ise while çalışmaz sayaç sıfır isedegişkenlerin hepsine min ve max degeri atılır
            {
                if (tut != "FR" || tut != "UP" || tut != "LO")
                {
                    foreach (var item in denklemler)
                    {
                        for (int i = 0; i < item.degiskenler.Count; i++)
                        {
                            degisken = item.degiskenler[i];
                            degisken.min = 0;
                            degisken.max = double.MaxValue;
                            item.degiskenler[i] = degisken;
                        }
                    }

                }

            }
        }
    }
}

