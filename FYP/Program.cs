using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FYP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Encoder encoder = new Encoder("text.txt", SolitonDistributionType.ISD); //from bin\debug\net6.0\text.txt
            var watch = System.Diagnostics.Stopwatch.StartNew();
            List<Drop> drops = encoder.ISDGenerateDroplets();
            watch.Stop();
            var generateTime = watch.ElapsedMilliseconds;

            Decoder decoder = new Decoder(encoder.getByteSize());
            watch.Restart();
            //test decode and rebuilding plaintext functions by printing decoded text to console
            Console.WriteLine(decoder.ISDRebuildPlaintext(drops));
            watch.Stop();
            var totalDecodetime = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to generate: " + generateTime + "\nTime taken to decode: " + totalDecodetime);
            
        }

    }
}