namespace FYP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double[] probabilities = { 50, 30, 15, 5, 1 };
            Random rand = new Random();

            int i = 0;
            while (rand.Next(1, 101) >= probabilities[i]) {
                i++;
            }
            int degree = i;


        }
    }
}