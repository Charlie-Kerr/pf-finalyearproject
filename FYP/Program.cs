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
            int TEST_SIZE = Encoding.ASCII.GetBytes(File.ReadAllText("text.txt")).Length;
            int TEST_SIZE_10K = Encoding.ASCII.GetBytes(File.ReadAllText("10000text.txt")).Length;

            Console.WriteLine("Welcome to my Digital Fountain!");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Please press enter to test decoding algorithms");
            Console.ReadLine();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int dropsUsed = encodeSoliton(SolitonDistributionType.ISD);
            watch.Stop();
            var totalTime = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to generate and decode using ISD: " + totalTime + "ms");
            Console.WriteLine("Amount of drops used to decode using ISD: " + dropsUsed);
            Console.WriteLine("Overhead drops: {0} Overhead percentage: {1}", dropsUsed - TEST_SIZE_10K, Math.Round((decimal)dropsUsed / TEST_SIZE_10K, 2));
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("Please press enter to decode using RSD");
            Console.ReadLine();
            watch.Restart();
            dropsUsed = encodeSoliton(SolitonDistributionType.RSD);
            watch.Stop();
            totalTime = watch.ElapsedMilliseconds;
            Console.WriteLine("Time taken to generate and decode using RSD: " + totalTime + "ms");
            Console.WriteLine("Amount of drops used to decode using RSD: " + dropsUsed);
            Console.WriteLine("Overhead drops: {0} Overhead percentage: {1}", dropsUsed - TEST_SIZE_10K, Math.Round((decimal)dropsUsed / TEST_SIZE_10K, 2));
            Console.WriteLine("------------------------------------------------------------");
        }

        static int encodeSoliton(SolitonDistributionType type)
        {
            Encoder encoder = new Encoder("10000text.txt", type); //from bin\debug\net6.0\text.txt
            List<Drop> drops = encoder.generateDroplets(encoder.getByteSize() / 100, 1);//(int)(encoder.getByteSize()))
            return decode(encoder, drops);
        }

        static int decode(Encoder encoder, List<Drop> drops) 
        {
            Decoder decoder = new Decoder(encoder.getByteSize(), drops);
            Console.WriteLine(decoder.improvedRebuildPlaintext(encoder));
            return decoder.getGobletSize();
        }

    }
}