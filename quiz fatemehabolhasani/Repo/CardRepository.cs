using Microsoft.EntityFrameworkCore;
using quiz_fatemehabolhasani.Data;
using quiz_fatemehabolhasani.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Repo
{
    
    public class CardRepository : ICardRepository
    {
        private readonly AppDbContext _context;

        public CardRepository(AppDbContext context)
        {
            _context = context;
        }

        public Card GetByNumber(string cardNumber)
        {
            return _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
        }

        public bool Exists(string cardNumber)
        {
            return _context.Cards.Any(c => c.CardNumber == cardNumber);
        }

        public List<Card> GetAll()
        {
            return _context.Cards.ToList();
        }

        public void SetIsActive(string cardNumber, bool isActive)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.IsActive, isActive));
        }

        public void IncrementFailedAttempts(string cardNumber)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.FailedAttempts, c => c.FailedAttempts + 1));
        }

        public void ResetFailedAttempts(string cardNumber)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.FailedAttempts, 0));
        }

        public void UpdatePassword(string cardNumber, string newPassword)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.Password, newPassword));
        }

        public void UpdateBalance(string cardNumber, float newBalance)
        {
            _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .ExecuteUpdate(setters => setters
                    .SetProperty(c => c.Balance, newBalance));
        }
        public int IncrementFailedAttemptsAndGet(string cardNumber)
        {
            var card = _context.Cards.FirstOrDefault(c => c.CardNumber == cardNumber);
           
            card.FailedAttempts++;
            _context.SaveChanges();

            return card.FailedAttempts;
        }
       
    }
}
