using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace MPS_Simpleks_
{

    class Iterasyon
    {
        List<Denklem> denklemler;

        int s = 0;//r denkleminde r olup olmadıgını kontrol sayacı
        int x = 0;//indis attıgım degisken
        ArrayList girencikanD = new ArrayList();
        int iter,itersyc=0;
        double gecici = 0;
        double gecicists = double.MaxValue;
        string tur,pName;
        Denklem r = new Denklem();
        Denklem row = new Denklem();
        Denklem.degisken degisken, degisken2, degisken3 = new Denklem.degisken();
        Standartlastır ss = new Standartlastır();
        public Iterasyon(List<Denklem> denklemler, string tur,string pName)
        {
            this.denklemler = denklemler;
            this.tur = tur;
            this.pName = pName;
            IlkAsama();
        }
        void OlanDegisken()
        {
            foreach (var item in denklemler)
            {
                if (item.esitlik != "N")
                {
                    foreach (var item2 in item.degiskenler)
                    {

                        if (item2.adi.Contains("S") || item2.adi.Contains("R") && item2.katsayi == 1)
                        {
                            if (!girencikanD.Contains(item2.adi))
                            {
                                girencikanD.Add(item2.adi);
                            }

                        }
                    }
                }
            }
        }//  R ve S degişkenlerini katsayisi 1 olanları listeye attım
        void GirenEkle(string ad,int nerden)
        {
            if (nerden==1)//eger girensutundan geldıyse
            {
                girencikanD.Add(ad);
            }
            else //cikan satirdan geldiyse
            {
                for (int i = 0; i < girencikanD.Count; i++)
                {
                    if (girencikanD[i].ToString() == ad)                                     {
                        girencikanD.Remove((girencikanD[i]));
                        i--;
                    }
                }                                
            }         
        }//giren degişkeni listeye atıp cıkan degişkeni listeden sildim
        void IlkAsama()// Min r oluşturuldu
        {       
            r.rows = "r";
            r.esitlik = "N";
            r.sts = 0;
            foreach (var item in denklemler[0].degiskenler)
            {
                degisken = item;
                if (degisken.adi.Contains("R"))
                {                   
                    degisken.katsayi = -1;
                }
                else
                {
                    degisken.katsayi = 0;
                }
                r.degiskenler.Add(degisken);
            }
            for (int i = 0; i < r.degiskenler.Count; i++)
            {
                if (r.degiskenler[i].adi.Contains("R"))
                {
                    s++;
                }
            }         
            if (s != 0)
            {
                RSıfırla();               
                foreach (var item in r.degiskenler)
                {
                    Console.Write(item.adi + " " + item.katsayi + "\n");
                }
                Console.WriteLine(r.sts);
                Console.WriteLine("r yazıldı");          
                BirinciAsamaGaussJordan();               
                Console.WriteLine("birinci asama bitti");
                foreach (object item in girencikanD)
                {
                    Console.WriteLine(item.ToString());
                }
                RYokSay();
                ZSatiriDonusum();
                Console.Write("İstenilen iterasyon sayisini girin= ");
                iter = Convert.ToInt32(Console.ReadLine());
                IkinciAsamaGaussJordan();            
            }
            else
            {
                Console.Write("İstenilen iterasyon sayisini girin= ");
                iter = Convert.ToInt32(Console.ReadLine());
                IkinciAsamaGaussJordan();
                foreach (object item in girencikanD)
                {
                    Console.WriteLine(item.ToString());
                }
            }

        }
        void RSıfırla()//oluşturulan r nin R degerleri sıfırlandı
        {
            for (int i = 1; i < denklemler.Count; i++)
            {
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {
                    if (denklemler[i].degiskenler[j].adi.Contains("R"))
                    {
                        if (denklemler[i].degiskenler[j].katsayi == 1)
                        {
                            r.sts += denklemler[i].sts;
                            for (int k = 0; k < r.degiskenler.Count; k++)
                            {
                                degisken = r.degiskenler[k];
                                degisken.katsayi += denklemler[i].degiskenler[k].katsayi;
                                r.degiskenler[k] = degisken;
                            }
                        }
                    }
                }
            }
        }
        void BirinciAsamaGaussJordan()//min r üzerinden gaus jordan yapıldı
        {
          
            RliGirenSutun();           
            if (degisken.katsayi!=0)
            {
                CikanSatir();         
                for (int i = 1; i < denklemler.Count; i++)
                {
                    if (denklemler[i].rows == row.rows)
                    {
                        denklemler[i].sts = gecicists;
                        for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                        {
                            degisken2 = denklemler[i].degiskenler[j];
                            degisken2.katsayi /= degisken.katsayi;
                            denklemler[i].degiskenler[j] = degisken2;
                        }
                    }
                }
                for (int i = 0; i < r.degiskenler.Count; i++)
                {
                    degisken2 = r.degiskenler[i];
                    degisken2.katsayi = degisken2.katsayi - gecici * row.degiskenler[i].katsayi;
                    r.degiskenler[i] = degisken2;
                }
                r.sts -= gecici * gecicists;
                for (int i = 0; i < r.degiskenler.Count; i++)
                {
                    Console.WriteLine(" r  {0}   {1}  {2} ", r.degiskenler[i].adi, r.degiskenler[i].katsayi, r.sts);
                }
                for (int i = 1; i < denklemler.Count; i++)
                {
                    if (denklemler[i].rows != row.rows)
                    {
                        double a = denklemler[i].degiskenler[x].katsayi;
                        denklemler[i].sts -= a * gecicists;
                        for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                        {
                            degisken2 = denklemler[i].degiskenler[j];
                            degisken2.katsayi = degisken2.katsayi - a * row.degiskenler[j].katsayi;
                            denklemler[i].degiskenler[j] = degisken2;
                        }
                    }
                }
            }           
            for (int i = 1; i < denklemler.Count; i++)
            {      
                    Console.WriteLine(denklemler[i].rows+" "+ denklemler[i].sts);
                    for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                    {
                        Console.WriteLine("  {0}   {1}   ", denklemler[i].degiskenler[j].adi, denklemler[i].degiskenler[j].katsayi);
                    }           

            }
            for (int i = 0; i < r.degiskenler.Count; i++)

            {
                if (r.degiskenler[i].katsayi > 0)
                {
                    BirinciAsamaGaussJordan();
                }
            }
        }
        void RYokSay()//İkinci aşama icin cost(z) amaç fonksyonundan R degişkenleri silindi
        {
            for (int i = 0; i < denklemler.Count; i++)
            {
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {  
                    if (denklemler[i].degiskenler[j].adi.Contains("R"))
                    {
                        
                        denklemler[i].degiskenler.RemoveAt(j);
                        j--;

                    }
                  
                }
            }
            for (int i = 0; i < girencikanD.Count; i++)
            {
                if (girencikanD[i].ToString().Contains("R"))//girencikan degişkende R var ise silidindi
                {
                    girencikanD.RemoveAt(i);
                    i--;
                }
            }

        }
        void ZSatiriDonusum()
        {

            for (int i = 1; i < denklemler.Count; i++)
            {
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {
                    if (!denklemler[i].degiskenler[j].adi.Contains("S"))
                    {
                        if (denklemler[i].degiskenler[j].katsayi == 1)
                        {
                            denklemler[i].sts *= (denklemler[0].degiskenler[j].katsayi * -1);
                            denklemler[0].sts += denklemler[i].sts;
                            degisken = denklemler[i].degiskenler[j];
                            degisken.katsayi = degisken.katsayi * (denklemler[0].degiskenler[j].katsayi * -1);
                            denklemler[i].degiskenler[j] = degisken;
                            degisken2 = denklemler[0].degiskenler[j];
                            degisken2.katsayi += degisken.katsayi;
                            denklemler[0].degiskenler[j] = degisken2;
                            for (int k = 0; k < denklemler[i].degiskenler.Count; k++)
                            {
                                if (degisken.adi != denklemler[i].degiskenler[k].adi)
                                {
                                    degisken2 = denklemler[i].degiskenler[k];
                                    degisken2.katsayi *= degisken.katsayi;
                                    denklemler[i].degiskenler[k] = degisken2;

                                    degisken3 = denklemler[0].degiskenler[k];
                                    degisken3.katsayi += degisken2.katsayi;
                                    denklemler[0].degiskenler[k] = degisken3;
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in denklemler)
            {
                Console.WriteLine("{0}  {1}", item.rows, item.sts);
                foreach (var item2 in item.degiskenler)
                {
                    Console.WriteLine("{0}  {1}    ", item2.adi, item2.katsayi);
                }
            }
        }
        void IkinciAsamaGaussJordan()
        {           
            itersyc++;           
            if (tur == "MIN")
            {
                RsizGirenSutun();
                if (degisken.katsayi!=0)
                {
                    CikanSatir();
                    for (int i = 1; i < denklemler.Count; i++)
                    {
                        if (denklemler[i].rows == row.rows)
                        {
                            denklemler[i].sts = gecicists;
                            for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                            {
                                degisken2 = denklemler[i].degiskenler[j];
                                degisken2.katsayi /= degisken.katsayi;
                                denklemler[i].degiskenler[j] = degisken2;
                            }
                        }
                    }
                    for (int i = 0; i < denklemler.Count; i++)
                    {
                        if (denklemler[i].rows != row.rows)
                        {
                            double a = denklemler[i].degiskenler[x].katsayi;
                            denklemler[i].sts -= a * gecicists;
                            for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                            {

                                degisken2 = denklemler[i].degiskenler[j];
                                degisken2.katsayi = degisken2.katsayi - a * row.degiskenler[j].katsayi;
                                denklemler[i].degiskenler[j] = degisken2;
                            }
                        }                
               
                    }
                }

              Yaz();
                          
                for (int i = 0; i < denklemler[0].degiskenler.Count; i++)
                {
                    if (denklemler[0].degiskenler[i].katsayi > 0)
                    {
                        IkinciAsamaGaussJordan();
                                       
                    }
                }
            }
            else //max
            {
                RsizGirenSutun();
                if (degisken.katsayi!=0)
                {
                    CikanSatir();
                    for (int i = 1; i < denklemler.Count; i++)
                    {
                        if (denklemler[i].rows == row.rows)
                        {
                            denklemler[i].sts = gecicists;
                            for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                            {
                                degisken2 = denklemler[i].degiskenler[j];
                                degisken2.katsayi /= degisken.katsayi;
                                denklemler[i].degiskenler[j] = degisken2;
                            }
                        }
                    }
                    for (int i = 0; i < denklemler.Count; i++)
                    {
                        if (denklemler[i].rows != row.rows)
                        {
                            double a = denklemler[i].degiskenler[x].katsayi;
                            denklemler[i].sts -= a * gecicists;
                            for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                            {
                                degisken2 = denklemler[i].degiskenler[j];
                                degisken2.katsayi -= a * row.degiskenler[j].katsayi;
                                denklemler[i].degiskenler[j] = degisken2;
                            }
                        }
                    }
                    if (itersyc == iter)
                    {
                        Yaz();
                    }
                    for (int i = 0; i < denklemler[0].degiskenler.Count; i++)
                    {
                        if (denklemler[0].degiskenler[i].katsayi < 0)
                        {
                            IkinciAsamaGaussJordan();
                        }
                    }
                }                         
              }
            }        
        void RliGirenSutun()
        {
            gecici = 0;
            for (int i = 0; i < r.degiskenler.Count; i++)
            {
                if (r.degiskenler[i].katsayi > 0)
                {
                    if (r.degiskenler[i].katsayi > gecici)
                    {
                        gecici = r.degiskenler[i].katsayi;
                        degisken = r.degiskenler[i];
                    }
                }
            }
            GirenEkle(degisken.adi, 1);
            if (degisken.katsayi==0)
            {
                Console.WriteLine("Problemin uygun cözümü yok");
            }else
            Console.WriteLine("giren degisken  " + degisken.adi+" "+degisken.katsayi);
        }
        void CikanSatir()
        {
            gecicists = double.MaxValue;
            if (gecici > 0)
            {
                for (int i = 1; i < denklemler.Count; i++)
                {
                    for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                    {
                        if (denklemler[i].degiskenler[j].adi == degisken.adi)
                        {
                            if ((denklemler[i].sts / denklemler[i].degiskenler[j].katsayi) > 0 && (denklemler[i].sts / denklemler[i].degiskenler[j].katsayi) <= gecicists)
                            {
                                gecicists = (denklemler[i].sts / denklemler[i].degiskenler[j].katsayi);
                                row = denklemler[i];
                                degisken = denklemler[i].degiskenler[j];
                                x = j;
                            }

                        }
                    }

                }
             //   GirenEkle(degisken.adi, 2);
                Console.WriteLine("cikan degisken  {0}  {1}  ", row.rows, row.sts);
            }
        }
        void RsizGirenSutun()
        {
            gecici = 0;
            if (tur=="MIN")
            {
                for (int i = 0; i < denklemler[0].degiskenler.Count; i++)
                {
                    if (denklemler[0].degiskenler[i].katsayi > 0)
                    {
                        if (denklemler[0].degiskenler[i].katsayi > gecici)
                        {
                            gecici = denklemler[0].degiskenler[i].katsayi;
                            degisken = denklemler[0].degiskenler[i];
                        }
                    }
                }              
            }
            else
            {
                for (int i = 0; i < denklemler[0].degiskenler.Count; i++)
                {

                    if (denklemler[0].degiskenler[i].katsayi < 0)
                    {

                        if (Math.Abs(denklemler[0].degiskenler[i].katsayi) > gecici)
                        {
                            gecici = Math.Abs(denklemler[0].degiskenler[i].katsayi);
                            degisken = denklemler[0].degiskenler[i];
                        }
                    }
                }           
            }
         //   GirenEkle(degisken.adi, 1);
            if (degisken.katsayi==0)
            {
                Console.WriteLine("Problemin uygun cözümü yok");                               
            }else
            Console.WriteLine("giren degisken  " + degisken.adi +" "+ degisken.katsayi);
        }    
        void Yaz()
        {
            Console.WriteLine(pName);
            for (int j = 0; j < denklemler[0].degiskenler.Count; j++)
            {
                Console.Write("             {0}", denklemler[0].degiskenler[j].adi);
            }
            Console.WriteLine();
            for (int i = 0; i < denklemler.Count; i++)
                {
                Console.Write(denklemler[i].rows);
                for (int j = 0; j < denklemler[i].degiskenler.Count; j++)
                {
                    Console.Write("             {0}",denklemler[i].degiskenler[j].katsayi);
                }
                Console.Write(denklemler[i].sts);
                Console.WriteLine();
                // Console.WriteLine(denklemler[i].rows+" "+denklemler[i].sts);



            }             
        }
    }
}