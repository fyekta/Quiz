using quiz_fatemehabolhasani.Data;
using quiz_fatemehabolhasani.Entities;
using quiz_fatemehabolhasani.Repo;
using quiz_fatemehabolhasani.Service;
using quiz_fatemehabolhasani.Data;

class Program
{
    static void Main(string[] args)
    {
        var context = new AppDbContext();
        var cardRepo = new CardRepository(context);
        var transactionRepo = new TransactionRepository(context);
        var bankService = new BankService(cardRepo, transactionRepo);

        Console.WriteLine("Bank Service");

        Card currentUser = null;

        while (true)
        {
            try
            {
                Console.Write("CardNumber: ");
                string cardNumber = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                currentUser = bankService.Authenticate(cardNumber, password);

                Console.WriteLine($"Wellcome {currentUser.HolderName}!");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eror: {ex.Message}");
            }
        }

      
        while (true)
        {
            Console.WriteLine("\n--------- Menu ---------");
            Console.WriteLine("1. Transfer Money");
            Console.WriteLine("2. Show Transactoins");
            Console.WriteLine("3. Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            try
            {
                if (choice == "1")
                {
                    Console.Write("Destination card number: ");
                    string destCard = Console.ReadLine();

                    Console.Write("Amount: ");
                    string inputAmount = Console.ReadLine();

                    if (!float.TryParse(inputAmount, out float amount))
                    {
                        Console.WriteLine("Eror");
                        continue;
                    }

                    bankService.Transfer(currentUser.CardNumber, destCard, amount);
                    Console.WriteLine("The transfer was successful.");
                }
                else if (choice == "2")
                {
                    var transactions = bankService.GetTransactions(currentUser.CardNumber);

                    if (transactions.Count == 0)
                    {
                        Console.WriteLine("No transactions found.");
                    }
                    else
                    {
                        Console.WriteLine("Transaction list:");
                        foreach (var t in transactions)
                        {
                            Console.WriteLine($"from: {t.SourceCardNumber} to: {t.DestinationCardNumber} | Amount: {t.Amount} | Date: {t.TransactionDate} | Successful: {(t.IsSuccessful ? "YES" : "NO")}");
                        }
                    }
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Exit");
                    break;
                }
                else
                {
                    Console.WriteLine("Eror.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Eror: " + ex.Message);
            }
        }
    }
}