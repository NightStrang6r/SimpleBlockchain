using System;
using System.Collections.Generic;

namespace SimpleBlockchain
{
    public class Blockchain
    {
        public List<Block> Chain { get; set; }

        public int Difficulty { get; set; }

        public Blockchain(int difficulty)
        {
            Chain = new List<Block> {
                CreateGenesisBlock()
            };
            Difficulty = difficulty;
        }

        public Block CreateGenesisBlock()
        {
            return new Block("0", new List<Transaction>());
        }

        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void AddBlock(Block newBlock)
        {
            if (!IsBalancesValid(newBlock)) throw new InvalidOperationException("Invalid balances");
            if (!IsBlockValid(newBlock, GetLatestBlock())) throw new InvalidOperationException("Invalid block");

            newBlock.PreviousHash = GetLatestBlock().Hash;
            newBlock.MineBlock(Difficulty);
            Chain.Add(newBlock);
        }

        public void SetDifficulty(int difficulty)
        {
            Difficulty = difficulty;
        }

        public bool IsChainValid()
        {
            for (int i = 0; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];

                if (i > 0)
                {
                    Block previousBlock = Chain[i - 1];
                    if (!IsBlockValid(currentBlock, previousBlock)) return false;
                }

                if (!IsBalancesValid(currentBlock)) return false;
            }

            return true;
        }

        public bool IsBlockValid(Block block, Block previousBlock)
        {
            if (block.Hash != block.CalculateHash())
            {
                return false;
            }

            if (block.PreviousHash != previousBlock.Hash)
            {
                return false;
            }

            return true;
        }

        public bool IsBalancesValid(Block block)
        {
            return true;
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