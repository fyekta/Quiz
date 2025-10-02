using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public float Balance { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public List<Transaction> TransactionsAsSource { get; set; } = new List<Transaction>();
        public List<Transaction> TransactionsAsDestination { get; set; } = new List<Transaction>();
    }
}
