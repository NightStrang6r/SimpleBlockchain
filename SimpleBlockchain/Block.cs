using System.Security.Cryptography;
using System.Text;

namespace SimpleBlockchain
{
    public class Block
    {
        public long Timestamp { get; set; }

        public string PreviousHash { get; set; }

        public string Hash { get; set; }

        public List<Transaction> Transactions { get; set; }

        public int Nonce { get; set; } = 0;

        public Block(string previousHash, List<Transaction> transactions)
        {
            PreviousHash = previousHash;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Transactions = transactions;
            Hash = CalculateHash();
        }

        public Block(string previousHash, long timestamp, List<Transaction> transactions)
        {
            PreviousHash = previousHash;
            Timestamp = timestamp;
            Transactions = transactions;
            Hash = CalculateHash();
        }

        public string CalculateHash()
        {
            string blockData = $"{PreviousHash}{Timestamp}{TransactionsToString()}{Nonce}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(blockData));
                hashBytes = sha256.ComputeHash(hashBytes);

                StringBuilder hash = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }

        private string TransactionsToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var transaction in Transactions)
            {
                sb.Append(transaction.ToString());
            }
            return sb.ToString();
        }

        public void MineBlock(int difficulty)
        {
            string hashPrefix = new string('0', difficulty);
            while (Hash.Substring(0, difficulty) != hashPrefix)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Timestamp: {Timestamp}\n");
            sb.Append($"Previous hash: {PreviousHash}\n");
            sb.Append($"Hash: {Hash}\n");
            sb.Append("Transactions:\n");
            foreach (var transaction in Transactions)
            {
                sb.Append($"{transaction.ToString()}\n");
            }
            return sb.ToString();
        }
    }
}