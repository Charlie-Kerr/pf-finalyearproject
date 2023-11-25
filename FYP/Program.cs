namespace FYP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            String[] data = { "First", "House", "Mouse", "Shelf", "Books"};
            String[] drop;
            int degree;



            for (int i = 0; i < data.Length*2; i++) 
            {
                degree = getDegree();
                for (int j = 0; j < degree; j++)
                {
                    if (j == 0)
                    {
                        drop = new string[degree];
                        drop[0] = data[rand.Next(0, data.Length)];
                    }
                    else
                    {
                        //drop.Append(data[rand.Next(0, data.Length)]);
                    }
                    //XOR(drop, degree);
                }

            }

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