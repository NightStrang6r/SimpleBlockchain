namespace SimpleBlockchain
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }

        public int Difficulty { get; }

        public Blockchain(int difficulty)
        {
            Chain = new List<Block> {
                CreateGenesisBlock()
            };
            Difficulty = difficulty;
        }

        public Blockchain(Block genesisBlock, int difficulty)
        {
            Difficulty = difficulty;
            Chain = new List<Block>
            {
                genesisBlock
            };
        }

        public Blockchain(List<Transaction> transactions, int difficulty)
        {
            Difficulty = difficulty;
            Chain = new List<Block>
            {
                CreateGenesisBlock(transactions)
            };
        }

        public Block CreateGenesisBlock()
        {
            Block genesisBlock = new Block("0", new List<Transaction>());
            genesisBlock.MineBlock(Difficulty);
            return genesisBlock;
        }

        public Block CreateGenesisBlock(List<Transaction> transactions)
        {
            Block genesisBlock = new Block("0", transactions);
            genesisBlock.MineBlock(Difficulty);
            return genesisBlock;
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block newBlock)
        {
            if (!AreBalancesValid(newBlock)) throw new InvalidOperationException("Invalid balances");
            if (!IsBlockValid(newBlock, GetLatestBlock())) throw new InvalidOperationException("Invalid block");

            if (!IsSolutionValid(newBlock))
            {
                newBlock.PreviousHash = GetLatestBlock().Hash;
                newBlock.MineBlock(Difficulty);
            }

            Chain.Add(newBlock);
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (!IsBlockValid(currentBlock, previousBlock))
                {
                    return false;
                }

                if (!IsSolutionValid(currentBlock))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsBlockValid(Block block, Block previousBlock)
        {
            if (block.PreviousHash != previousBlock.Hash)
            {
                return false;
            }

            return true;
        }

        public bool IsSolutionValid(Block block)
        {
            string hashPrefix = new string('0', Difficulty);

            if (block.Hash.Length < hashPrefix.Length || block.Hash.Substring(0, Difficulty) != hashPrefix || block.Hash != block.CalculateHash())
            {
                return false;
            }

            return true;
        }

        public bool AreBalancesValid(Block block)
        {
            Dictionary<string, decimal> balances = new Dictionary<string, decimal>();

            foreach (var transaction in block.Transactions)
            {
                if (!balances.ContainsKey(transaction.Sender))
                {
                    balances[transaction.Sender] = GetBalance(transaction.Sender);
                }

                if (balances[transaction.Sender] < transaction.Amount)
                {
                    return false;
                }

                balances[transaction.Sender] -= transaction.Amount;
                if (!balances.ContainsKey(transaction.Receiver))
                {
                    balances[transaction.Receiver] = 0;
                }
                balances[transaction.Receiver] += transaction.Amount;
            }

            return true;
        }


        public decimal GetBalance(string user)
        {
            decimal balance = 0;

            foreach (var block in Chain)
            {
                foreach (var transaction in block.Transactions)
                {
                    if (transaction.Receiver == user)
                    {
                        balance += transaction.Amount;
                    }
                    if (transaction.Sender == user)
                    {
                        balance -= transaction.Amount;
                    }
                }
            }

            return balance;
        }
    }
}