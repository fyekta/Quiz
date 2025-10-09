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
        Card GetByNumber(string cardNumber);
        bool Exists(string cardNumber);
        List<Card> GetAll();
        void SetIsActive(string cardNumber, bool isActive);
        void IncrementFailedAttempts(string cardNumber);
        void ResetFailedAttempts(string cardNumber);
        void UpdatePassword(string cardNumber, string newPassword);
        void UpdateBalance(string cardNumber, float newBalance);
        int IncrementFailedAttemptsAndGet(string cardNumber);
    }
}
