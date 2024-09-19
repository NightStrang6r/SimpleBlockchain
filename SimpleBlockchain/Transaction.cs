using System.Security.Cryptography;
using System.Text;

namespace SimpleBlockchain
{
    public class Transaction
    {
        public string Id { get; private set; }

        public string Sender { get; private set; }

        public string Receiver { get; private set; }

        public decimal Amount { get; private set; }

        public string Comment { get; private set; }

        public decimal Fee { get; private set; }

        public long Timestamp { get; private set; }


        public Transaction(string sender, string receiver, decimal amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = ValidateNumber(amount);
            Fee = 0;
            Comment = "";
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Id = GenerateId();
        }

        public Transaction(string sender, string receiver, decimal amount, decimal fee)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = ValidateNumber(amount);
            Fee = ValidateNumber(fee);
            Comment = "";
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Id = GenerateId();
        }

        public Transaction(string sender, string receiver, decimal amount, decimal fee, string comment)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = ValidateNumber(amount);
            Fee = ValidateNumber(fee);
            Comment = comment;
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Id = GenerateId();
        }

        private decimal ValidateNumber(decimal number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Number cannot be negative");
            }

            return number;
        }

        private string GenerateId()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string transactionData = $"{Sender}{Receiver}{Amount}{Fee}{Comment}{Timestamp}";
                byte[] bytes = Encoding.UTF8.GetBytes(transactionData);
                byte[] hash = sha256.ComputeHash(bytes);
                hash = sha256.ComputeHash(hash);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public override string ToString()
        {
            return $"{{Id: {Id}, sender: {Sender}, receiver: {Receiver}, amount: {Amount}, fee: {Fee}, comment: {Comment}, timestamp: {Timestamp}}}";
        }
    }
}