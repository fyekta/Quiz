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

        Card loggedInCard = null;

        while (true)
        {
            try
            {
                Console.Write("CardNumber: ");
                string cardNumber = Console.ReadLine();

                Console.Write("Password: ");
                string password = Console.ReadLine();

                loggedInCard = bankService.Login(cardNumber, password);

                Console.WriteLine($"Wellcome {loggedInCard.HolderName}!");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eror: {ex.Message}");
            }
        }

        while (true)
        {
            Console.WriteLine("\n=== Menu ===");
            Console.WriteLine("1. Transfer money");
            Console.WriteLine("2. Show Transfer");
            Console.WriteLine("3. Change Password");
            Console.WriteLine("4. Exit");

            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("dst card: ");
                string dstCardNumber = Console.ReadLine();

                string holderName = bankService.GetHolderName(dstCardNumber);
                Console.WriteLine("name: " + holderName);

                string code = bankService.GenerateCode();
                Console.WriteLine($"code: {code}");

                Console.Write("code: ");
                string inputCode = Console.ReadLine();

                if (!bankService.ValidateCode(inputCode))
                {
                    Console.WriteLine("The verification code is incorrect or expired. The transaction was canceled.");
                    continue;
                }

                Console.Write("Amount: ");
                if (!float.TryParse(Console.ReadLine(), out float amount))
                {
                    Console.WriteLine("The amount entered is not valid.");
                    continue;
                }

                string result = bankService.Transfer(loggedInCard.CardNumber, dstCardNumber, amount);
                Console.WriteLine(result);
            }
            else if (choice == "2")
            {
                bankService.ShowTransactions(loggedInCard.CardNumber);
            }
            else if (choice == "3")
            {
                Console.Write("New Password: ");
                string newPass = Console.ReadLine();

                bankService.ChangePassword(loggedInCard.CardNumber, newPass);
            }
            else if (choice == "4")
            {
                Console.WriteLine("Exit");
                break;
            }
            else
            {
                Console.WriteLine("Eror");
            }
        }
    }
}

