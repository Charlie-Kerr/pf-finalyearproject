namespace FYP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

        }

        static int getDegree() {
            int[] probabilities = { 50, 30, 15, 5, 1 };
            Random rand = new Random();

            int i = 0;
            int degree = rand.Next(1, 101);

            foreach (int p in probabilities)
            {
                if (degree >= p)
                {
                    degree = Array.IndexOf(probabilities, p);
                    return degree;
                }
            }
            return -1; // a run should never reach here
        }
    }
}