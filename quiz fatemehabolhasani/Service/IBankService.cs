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
        Card Login(string cardNumber, string password);
        string Transfer(string srcNumber, string dstNumber, float amount);
        void ChangePassword(string cardNumber, string newPassword);
        void ShowTransactions(string cardNumber);
        string GenerateCode();
        bool ValidateCode(string codeInput);
        string GetHolderName(string cardNumber);
    }
}
