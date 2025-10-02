using quiz_fatemehabolhasani.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Repo
{
    public interface ICardRepository
    {
        Card GetByCardNumber(string cardNumber);
        void Update(Card card);
        bool Exists(string cardNumber);
    }
}
