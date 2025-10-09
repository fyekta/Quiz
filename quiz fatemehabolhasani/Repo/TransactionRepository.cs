using quiz_fatemehabolhasani.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quiz_fatemehabolhasani.Data;


namespace quiz_fatemehabolhasani.Repo
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public List<Transaction> GetByCardNumber(string cardNumber)
        {
            return _context.Transactions
                .Where(t => t.SourceCardNumber == cardNumber || t.DestinationCardNumber == cardNumber)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();
        }
        public float GetTodayTotal(string cardNumber)
        {
            var today = DateTime.Today;
            return _context.Transactions
                .Where(t => t.SourceCardNumber == cardNumber && t.TransactionDate.Date == today && t.IsSuccessful)
                .Sum(t => t.Amount);
        }
    }
}
