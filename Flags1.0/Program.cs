using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyNamespace
{

    class Program
    {

        
        public static void Main(string[] args)
        {

            Flags1.T_x_ t_x = new Flags1.T_x_();
            t_x.StartTimer();
            while (!t_x.GetFinish()) { }
            //Console.WriteLine("T(X) has done");
            //Console.ReadLine();
            Flags1.R_x_ r_x = new Flags1.R_x_();
            
            r_x.StartTimer();
            

            
            
            Console.ReadLine();
            r_x.ClearDestinationFolder();
            
            

        }

        
    }
}