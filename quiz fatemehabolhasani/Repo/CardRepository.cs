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

        public void Update(Card card)
        {
            _context.Cards.Update(card);
            _context.SaveChanges();
        }

        public bool Exists(string cardNumber)
        {
            return _context.Cards.Any(c => c.CardNumber == cardNumber);
        }
        //
        public List<Card> GetAll()
        {
            return _context.Cards.ToList(); 
        }
    }
}
