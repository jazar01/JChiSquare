using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JChiSquare
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                Console.WriteLine("Usage:  JChiSquare filename");

            string fname = args[0];
            if (File.Exists(fname))
            {
                DateTime tstart = DateTime.Now;
                Console.Write("ChiSquare = " + ChiSquare(fname));
                DateTime tend = DateTime.Now;
                TimeSpan t = tend.Subtract(tstart);
                Console.Write("     time: " + t.TotalSeconds.ToString("0.000") + " seconds ");
            }
            else
                Console.Write("File does not exist: " + fname);
        }

        /// <summary>
        /// computes Chi for a file
        /// </summary>
        /// <param name="filename">string filename (full path)</param>
        /// <returns>chi</returns>
        public static double ChiSquare(string filename)
        {
            // this limits the sample size to 13K.  remove this to use entire file
            int SampleSize = 13 * 1024;
            int bufferLength = SampleSize;
            double Expected = SampleSize / 256;  

            long[] map = new long[256];

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, bufferLength);

            Byte[] fBuffer = new Byte[bufferLength];

            int count = 0;

            count = fs.Read(fBuffer, 0, bufferLength);  // fill buffer from file

            for (int i = 0; i < count; i++)  // iterate through buffer and update map counts
                map[fBuffer[i]]++;

            fs.Close();

            double chi = 0.0;
            double one, two, three;

            for (int i = 0; i < 256; i++)
            {
                if (map[i] > 0)
                {
                    one = map[i] - Expected;
                    two = one * one;
                    three = two / Expected;
                    chi += three;
                }
            }

            return chi;
        }
        
    }
}
