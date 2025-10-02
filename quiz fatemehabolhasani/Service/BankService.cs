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
        private readonly ICardRepository _cardRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BankService(ICardRepository cardRepository, ITransactionRepository transactionRepository)
        {
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;
        }

        public Card Authenticate(string cardNumber, string password)
        {
            var card = _cardRepository.GetByCardNumber(cardNumber);

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
                    _cardRepository.Update(card);
                    throw new Exception("Your card has been blocked due to entering the wrong password 3 times.");
                }
                _cardRepository.Update(card);
                throw new Exception($"The password is incorrect. Number of attempts remaining: {3 - card.FailedAttempts}");
            }

            card.FailedAttempts = 0;
            _cardRepository.Update(card);

            return card;
        }

        public void Transfer(string sourceCardNumber, string destinationCardNumber, float amount)
        {
            if (sourceCardNumber == destinationCardNumber)
                throw new Exception("The origin and destination cards cannot be the same.");

            if (amount <= 0)
                throw new Exception("The transfer amount must be greater than zero.");

            if (sourceCardNumber.Length != 16 || destinationCardNumber.Length != 16)
                throw new Exception("The card number must be 16 digits.");

            var sourceCard = _cardRepository.GetByCardNumber(sourceCardNumber);
            var destinationCard = _cardRepository.GetByCardNumber(destinationCardNumber);

            if (sourceCard == null)
                throw new Exception("Origin card not found.");

            if (destinationCard == null)
                throw new Exception("Destination card not found.");

            if (!sourceCard.IsActive)
                throw new Exception("The originating card is blocked.");

            if (!destinationCard.IsActive)
                throw new Exception("Destination card is blocked.");

            if (sourceCard.Balance < amount)
                throw new Exception("Not Money.");

            sourceCard.Balance -= amount;
            destinationCard.Balance += amount;

            _cardRepository.Update(sourceCard);
            _cardRepository.Update(destinationCard);

            var transaction = new Transaction
            {
                SourceCardNumber = sourceCardNumber,
                DestinationCardNumber = destinationCardNumber,
                Amount = amount,
                TransactionDate = DateTime.Now,
                IsSuccessful = true
            };

            _transactionRepository.Add(transaction);
        }

        public List<Transaction> GetTransactions(string cardNumber)
        {
            return _transactionRepository.GetByCardNumber(cardNumber);
        }
    }
}
