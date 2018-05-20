using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace MPS_Simpleks_
{
    class Denklem
    {
      internal  string rows, esitlik;
        internal struct degisken
        {
           internal string adi;          
           internal  double katsayi;
           internal double max, min;
           internal bool free;
        }
      internal  List<degisken> degiskenler = new List<degisken>();
      internal double sts;
     
  
    }
}
