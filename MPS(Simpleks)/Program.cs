using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MPS_Simpleks_
{
    class Program
    { 
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();//nekadar zaman da çalıştıgını hesaplıyor
            watch.Start();
            MPSRead r = new MPSRead();               
            r.Oku("ornek2.mps");          
            watch.Stop();
            Console.WriteLine("Bağlantı kurulma süresi: {0}", watch.Elapsed.Milliseconds);
            Console.ReadLine();

        }
    }
}
