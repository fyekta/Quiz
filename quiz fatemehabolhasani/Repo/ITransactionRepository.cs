using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using quiz_fatemehabolhasani.Entities;
namespace quiz_fatemehabolhasani.Repo
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        List<Transaction> GetByCardNumber(string cardNumber);
    }
}
