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
            int[] parts;
            List<Drop> drops = new List<Drop>();



            for (int i = 0; i < data.Length*2; i++) 
            {
                degree = getDegree();
                drop = new string[degree];
                parts = new int[degree];
                for (int j = 0; j < degree; j++)
                {
                    if (j == 0)
                    {
                        drop[0] = data[rand.Next(0, data.Length)];
                        parts[0] = Array.IndexOf(data, drop[0]);
                    }
                    else
                    {
                        drop[j] = (data[rand.Next(0, data.Length)]);
                    }
                    drops.Add(new Drop(parts, drop));
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

    class Drop
    {
        public int[] parts;
        public String[] data;

        public Drop(int[] parts, String[] data) 
        {
            this.parts = parts;
            this.data = data;
        }
    
    }
}