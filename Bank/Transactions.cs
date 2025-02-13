
namespace Bank;

public class Transactions
{   
    public static void Main() {
     
        Account account = new(1000);
        PrintBalance(account);

        account.Deposit(500);
        PrintBalance(account);

        account.Withdraw(19.21);
        PrintBalance(account);
    }


    public static void PrintBalance(Account account) {
        Console.WriteLine($"Account balance: {account.GetBalance()} for account {account.GetId()}");
    }
}
