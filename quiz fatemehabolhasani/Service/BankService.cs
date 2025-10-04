using quiz_fatemehabolhasani.Entities;
using quiz_fatemehabolhasani.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_fatemehabolhasani.Service
{
    public class BankService : IBankService
    {
        private readonly ICardRepository _cardRepo;
        private readonly ITransactionRepository _transRepo;

        public BankService(ICardRepository cardRepo, ITransactionRepository transRepo)
        {
            _cardRepo = cardRepo;
            _transRepo = transRepo;
        }

        public Card Login(string cardNumber, string password)
        {
            var card = _cardRepo.GetByNumber(cardNumber);

            if (card == null)
                throw new Exception("Card not found.");

            if (!card.IsActive)
                throw new Exception("The card is blocked.");

            if (card.Password != password)
            {
                card.FailedAttempts++;
                if (card.FailedAttempts >= 3)
                {
                    card.IsActive = false;
                    _cardRepo.Update(card);
                    throw new Exception("Your card has been blocked due to entering the wrong password 3 times.");
                }
                _cardRepo.Update(card);
                throw new Exception($"The password is incorrect. Number of attempts remaining: {3 - card.FailedAttempts}");
            }

            card.FailedAttempts = 0;
            _cardRepo.Update(card);

            return card;
        }

        
        public string Transfer(string srcNumber, string dstNumber, float amount)
        {
            if (srcNumber == dstNumber)
                
                throw new Exception("The origin and destination cards cannot be the same.");

            if (amount <= 0)
                throw new Exception("The transfer amount must be greater than zero.");

            if (srcNumber.Length != 16 || dstNumber.Length != 16)
                throw new Exception("The card number must be 16 digits.");

            var src = _cardRepo.GetByNumber(srcNumber);
            var dst = _cardRepo.GetByNumber(dstNumber);

            

             if (srcNumber == null)
                throw new Exception("Origin card not found.");

            if (dstNumber == null)
                throw new Exception("Destination card not found.");

            if (!src.IsActive)
                throw new Exception("The originating card is blocked.");

            if (!dst.IsActive)
                throw new Exception("Destination card is blocked.");

            if (src.Balance < amount)
                throw new Exception("Not Money.");

            float todayTotal = _transRepo.GetTodayTotal(srcNumber);
            if (todayTotal + amount > 250000)
                return "saqf 250000";

            float fee = amount >= 1000 ? amount * 0.015f : amount * 0.005f;
            float total = amount + fee;

            if (src.Balance < total)
                return "not mony";

            src.Balance -= total;
            dst.Balance += amount;

            _cardRepo.Update(src);
            _cardRepo.Update(dst);

            var transaction = new Transaction
            {
                SourceCardNumber = srcNumber,
                DestinationCardNumber = dstNumber,
                Amount = amount,
                TransactionDate = DateTime.Now,
                IsSuccessful = true
            };

            try
            {
                _transRepo.Add(transaction);
                return $" The transfer was successful.Amount: {amount} | fee: {fee}";
            }
            catch
            {
               
                src.Balance += total;
                dst.Balance -= amount;

                _cardRepo.Update(src);
                _cardRepo.Update(dst);

                transaction.IsSuccessful = false;
                _transRepo.Add(transaction);

                return "Error in recording transaction. Amount was returned to the originating card.";
            }
        }
        public void ChangePassword(string cardNumber, string newPassword)
        {
            var card = _cardRepo.GetByNumber(cardNumber);
            if (card != null && card.IsActive)
            {
                card.Password = newPassword;
                _cardRepo.Update(card);
                Console.WriteLine("Password changed successfully.");
            }
            else
            {
                Console.WriteLine("Eror");
            }
        }

        public void ShowTransactions(string cardNumber)
        {
            var transactions = _transRepo.GetByCardNumber(cardNumber);
            if (transactions.Count == 0)
            {
                Console.WriteLine("Transaction not found.");
                return;
            }

            foreach (var t in transactions)
            {
                Console.WriteLine($"[{t.TransactionDate}] From {t.SourceCardNumber} to {t.DestinationCardNumber} | Amount: {t.Amount} | IsSuccessful: {t.IsSuccessful}");
            }
        }

        public string GenerateCode()
        {
            string code = new Random().Next(10000, 99999).ToString();
            File.WriteAllText("code.txt", $"{code}|{DateTime.Now}");
            return code;
        }

        public bool ValidateCode(string codeInput)
        {
            if (!File.Exists("code.txt")) return false;

            var data = File.ReadAllText("code.txt").Split('|');
            string code = data[0];
            DateTime timestamp = DateTime.Parse(data[1]);

            return code == codeInput && (DateTime.Now - timestamp).TotalMinutes <= 5;
        }

        public string GetHolderName(string cardNumber)
        {
            var card = _cardRepo.GetByNumber(cardNumber);
            return card != null ? card.HolderName : "Not Found card";
        }
    }
   
}
