using System.Text;

namespace SimpleBlockchain
{
    class Program
    {
        public static void Main()
        {
            PrintHeader();

            Blockchain akaBitcoin = new Blockchain(4);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================= Blocks adding test =================");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Mining block " + i + "...");
                List<Transaction> transactions = new List<Transaction>
                {
                    new Transaction("Alice", "Bob", 50 * i, i),
                    new Transaction("Bob", "Charlie", 25 + i)
                };
                Block block = new Block(akaBitcoin.GetLatestBlock().Hash, transactions);
                akaBitcoin.AddBlock(block);
                Console.WriteLine($"Block with hash {block.Hash} was successfully added to the blockchain.\n");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================= Blockchain output test =================");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < akaBitcoin.Chain.Count; i++)
            {
                Console.WriteLine($"Block {i}: ");
                Console.WriteLine(akaBitcoin.Chain[i].ToString());
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================= Blockchain validity test =================");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Is blockchain valid? {akaBitcoin.IsChainValid()}\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("================= Blockchain tampering test =================");
            Console.ForegroundColor = ConsoleColor.White;
            akaBitcoin.Chain[1].Transactions[0] = new Transaction("Alice", "Leo", 1000000000);
            Console.WriteLine($"Is blockchain valid after tampering? {akaBitcoin.IsChainValid()}\n");
        }

        public static void PrintHeader()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Технологія блокчейн та машинне навчання");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Практична робота №1: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Створення простого блокчейну\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Виконав: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("ст. групи ПЗПІ-21-3, Царук Леонід\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}