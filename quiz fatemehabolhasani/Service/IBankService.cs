using quiz_fatemehabolhasani.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Service
{
    public interface IBankService
    {
        Card Authenticate(string cardNumber, string password);
        void Transfer(string sourceCardNumber, string destinationCardNumber, float amount);
        List<Transaction> GetTransactions(string cardNumber);
    }
}
