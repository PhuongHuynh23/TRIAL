internal class Program
{
    private static void Main(string[] args)
    {
        //int[][] a = new int[4][];
        //a[0] = new int[] { 1, 1, 1, 1, 1};
        //    a[1] = new int[] { 2, 2 };
        //a[2] = new int[] { 3, 3, 3, 3 };
        //a[3] = new int[] { 4, 4 };

        
            Console.Write("Enter the number of row ");
            int rows = Convert.ToInt16(Console.ReadLine());
        int[][] a = new int[rows][];
        inputrandom(a, rows);
        print(a);
        maxinthearray(a);
        maxintherow(a);
        sortArray(a);
        print(a);
        
        isPrimeinArray(a);

        Console.Write("Enter a number you want to find ");
        int num = int.Parse(Console.ReadLine());

        }
    static void inputrandom(int[][] a, int rows)
    {
        Random random = new Random();
        for(int i = 0; i < rows; i++) 
        {
            Console.Write($"Enter the col in {i}th row "); int col = int.Parse(Console.ReadLine());
            a[i] = new int[col];
            for(int j = 0; j < col; j++)
            {
                a[i][j] = random.Next(10, 50);
            }

        }
    }
    static void print(int[][] a)
    {
        for(int i = 0;i < a.Length;i++)
        {
            for (int j = 0;j < a[i].Length;j++)
            {
                Console.Write(a[i][j] + " ");
            }
            Console.WriteLine();

        }
    }

    static void maxinthearray(int[][] a)
    {
        int max = 0;
        for(int i = 0; i < a.Length ; i++)
        {
            for(int j = 0; j <  a[i].Length;j++)
            {
                if (a[i][j] > max)
                    max = a[i][j];
            }
        }

        Console.WriteLine($"The biggest number in an array is {max}");
    }

    static void maxintherow(int[][] a)
    {
        int[] maxrow = new int[a.Length];
        for(int i = 0; i < a.Length ; i++)
        {
            for(int j = 0  ; j < a[i].Length ; j++)
            {
                int rowma = 0;
                if (a[i][j] > rowma)
                {
                    maxrow[i]  = a[i][j];
                    
                }
            }

        }
        for(int k = 0; k < a.Length ; k++) 
        {
            Console.Write($"The largest number in row {k} is {maxrow[k]} ");
            Console.WriteLine();
        }
    }

    static void sortrow(int[] a)
    {
        
        for(int i = 0; i < a.Length - 1; i++)
        {
            for(int j = 0; j < a.Length - i - 1 ; j++)
            {
                if (a[j] > a[j + 1])
                {
                    int temp = a[j];
                    a[j] = a[j + 1];
                    a[j + 1] = temp;   
                }
            }
        }
    }
    static void sortArray(int[][] a)
    {
        for(int i = 0;i < a.Length;i++)
        {
            sortrow(a[i]);
        }
    }

    static bool isPrime(int a)
    {
        
        for (int i = 2; i < a ; i++)
        {
            if (a % i == 0)
            {
                return false;
            }
        }
        return true;
    }
    static void isPrimeinArray(int[][] a)
    {
        Console.Write("Prime numbers are ");
        for(int i = 0; i <a.Length;i++)
        {
            for (int j = 0; j < a[i].Length; j++)
            {
                if (isPrime(a[i][j]))
                {
                    Console.Write(a[i][j] + " ");
                }
            }
        }
        
    }

    static int[] PositionRow(int[] a, int numToFind)
    {
        int[] la;
        for(int i = 0; i < a.Length; i++)
        {
            if (a[i] == numToFind)
            {
                return la[i];
            }
        }

        
    }
    static void PositionArray(int[][] a, int numToFind)
    {
        for(int i = 0; i <a.Length; i++)
        {
            for(int j = 0; j < a[i].Length; j++)
            {

            }
        }
    }
}
