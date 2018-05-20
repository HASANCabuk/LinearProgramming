using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace MPS_Simpleks_
{
    class Standartlastır
    {
        List<Denklem> denklemler;
       
        string tur,pName;
        internal ArrayList aralikDegiskeniK = new ArrayList();
        internal ArrayList aralikDegiskeniB = new ArrayList();
        internal ArrayList mindegri = new ArrayList();
        internal ArrayList mindegri1 = new ArrayList();
        Denklem.degisken degisken = new Denklem.degisken();
        Iterasyon iter;
        public Standartlastır()
        {
        }
        internal Standartlastır(List<Denklem> denklemler,string tur,string pName)
        {
            this.denklemler = denklemler;
            this.tur = tur;
            this.pName = pName;
            Standart();
            AralikDuzen();     
            iter = new Iterasyon(this.denklemler, this.tur,pName);
        }    
        void AralikDuzen()//gelen degişkenlerin minlerini ve maxlarını düzenler 
        {
            for (int i = 0; i < denklemler.Count; i++)
            {
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {
                    if (denklemler[i].degiskenler[j].min<0)
                    {
                       
                        degisken = denklemler[i].degiskenler[j];
                        denklemler[i].sts -= (degisken.katsayi) * (degisken.min *-1);
                       // Console.WriteLine(degisken.adi+" "+degisken.katsayi * (degisken.min * -1));
                        string sifirla=degisken.adi;
                        aralikDegiskeniK.Add(sifirla);
                        degisken.max += (degisken.min * -1);
                        mindegri.Add((degisken.min));                      
                        degisken.min = 0; 
                        denklemler[i].degiskenler[j] = degisken;
                    }
                    else
                    {
                        if (denklemler[i].degiskenler[j].min > 0)
                        {
                            degisken = denklemler[i].degiskenler[j];
                            denklemler[i].sts -= degisken.katsayi * (degisken.min);
                            string sifirla = degisken.adi;
                            aralikDegiskeniB.Add(sifirla);
                            degisken.max -= (degisken.min);
                            mindegri1.Add(degisken.min);
                            degisken.min = 0;
                            denklemler[i].degiskenler[j] = degisken;
                        }
                    }
                }
            }
            foreach (var item in denklemler)
            {
                Console.WriteLine(item.rows + " " + item.sts);
                foreach (var item2 in item.degiskenler)
                {
                    Console.Write(item2.adi + " " + item2.katsayi +" "+"Min="+item2.min+" "+"Max="+item2.max+ "\n");
                }
            }
            Console.WriteLine("Degişken aralıkları normalleştirilerek yazıldı ");
        }
        void Standart()
        {
            for (int i = 0; i < denklemler.Count; i++)
            {

                if (denklemler[i].esitlik == "L")
                {
                    degisken.adi = "S" + (i);                 
                    degisken.katsayi = 1;
                    degisken.free = false;
                    degisken.min = 0;
                    degisken.max = double.MaxValue;
                    denklemler[i].degiskenler.Add(degisken);
                    degisken.katsayi = 0;
                    denklemler[0].degiskenler.Add(degisken);
                }
                if (denklemler[i].esitlik == "G")
                {
                    degisken.adi = "S" + (i);                 
                    degisken.katsayi = -1;
                    degisken.free = false;
                    degisken.min = 0;
                    degisken.max = double.MaxValue;
                    denklemler[i].degiskenler.Add(degisken);
                    degisken.katsayi = 0;
                    denklemler[0].degiskenler.Add(degisken);
                    degisken.adi = "R" + (i);                   
                    degisken.katsayi = 1;
                    degisken.free = false;
                    degisken.min = 0;
                    degisken.max = double.MaxValue;
                    denklemler[i].degiskenler.Add(degisken);
                    denklemler[0].degiskenler.Add(degisken);
                }
                if (denklemler[i].esitlik == "E")
                {
                    degisken.adi = "R" + (i);                  
                    degisken.katsayi = 1;
                    degisken.free = false;
                    degisken.min = 0;
                    degisken.max = double.MaxValue;
                    denklemler[i].degiskenler.Add(degisken);                  
                    denklemler[0].degiskenler.Add(degisken);
                }
            }
            Free();
            Esit();                 
            foreach (var item in denklemler)
            {
                Console.WriteLine(item.rows + " "+ item.sts);
                foreach (var item2 in item.degiskenler)
                {
                    Console.Write(item2.adi + " " + item2.katsayi + " " +"\n");
                }
                
               
            }
            Console.WriteLine("standartlaştırlmış hali yazildi");
        }//Dolgu ve artık ve yapay  degişken eklendi
        void Free()
        {
            for (int i = 0; i < denklemler.Count; i++)
            {
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {
                   
                    if (denklemler[i].degiskenler[j].free==true)
                    {
                        degisken = denklemler[i].degiskenler[j];
                        degisken.adi += "+";
                        denklemler[i].degiskenler[j] = degisken;
                        degisken.adi= degisken.adi.Remove(degisken.adi.Length - 1);
                        degisken.adi += "-";
                        denklemler[i].degiskenler.Insert(j + 1,degisken);
                        j++;
                    }
                }
            }
        }//sınırlandırılmamış degişken iki parcayla denkleme yazıldı
        void Esit()//olmayan degişkenler 0 katsayısı ile yazıldı
        {
            foreach (var item in denklemler)
            {
                if (item.esitlik == "N")
                {
                    for (int i = 0; i < item.degiskenler.Count; i++)
                    {
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

    }
}
