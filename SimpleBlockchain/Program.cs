using System.Text;

namespace SimpleBlockchain
{
    class Program
    {
        public static void Main()
        {
            PrintHeader();

            List<Transaction> genesisTransactions = new List<Transaction> 
            {
                new Transaction("Network", "Alice", 50),
                new Transaction("Network", "Bob", 10),
                new Transaction("Network", "Charlie", 30)
            };

            Blockchain blockchain = new Blockchain(genesisTransactions, 4);

            PrintColoredLine("================= Blocks adding test =================");
            List<Transaction> firstTransactions = new List<Transaction>
            {
                new Transaction("Alice", "Bob", 50),
                new Transaction("Charlie", "Bob", 20)
            };
            AddBlock(blockchain, firstTransactions);

            List<Transaction> secondTransactions = new List<Transaction>
            {
                new Transaction("Charlie", "Alice", 10),
                new Transaction("Alice", "Bob", 5)
            };
            AddBlock(blockchain, secondTransactions);

            PrintColoredLine("================= Invalid transactions test =================");
            List<Transaction> invalidTransactions = new List<Transaction>
            {
                new Transaction("Alice", "Bob", 100),
                new Transaction("Bob", "Charlie", 100)
            };
            try
            {
                AddBlock(blockchain, invalidTransactions);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"{e.Message}\n");
            }


            PrintColoredLine("================= Blockchain output test =================");
            for (int i = 0; i < blockchain.Chain.Count; i++)
            {
                Console.WriteLine($"Block {i}: ");
                Console.WriteLine(blockchain.Chain[i].ToString());
            }

            PrintColoredLine("================= Blockchain validity test =================");
            Console.WriteLine($"Is blockchain valid? {blockchain.IsChainValid()}\n");

            PrintColoredLine("================= Blockchain tampering test =================");
            blockchain.Chain[1].Transactions[0] = new Transaction("Alice", "Leo", 1000000000);
            Console.WriteLine($"Is blockchain valid after tampering? {blockchain.IsChainValid()}\n");
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

        public static void PrintColoredLine(string message, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void AddBlock(Blockchain blockchain, List<Transaction> transactions)
        {
            Console.WriteLine("Mining block...");
            Block block = new Block(blockchain.GetLatestBlock().Hash, transactions);
            blockchain.AddBlock(block);
            Console.WriteLine($"Block with hash {block.Hash} was successfully added to the blockchain.\n");
        }
    }
}