
namespace Bank;

public class Transactions
{   
    public static void Main() {
     
        Account account = new(1000);
        PrintBalance(account);


        Thread t1 = new(()=> {account.Deposit(500);}) {Name = "Thread 1"};
        Thread t2 = new(()=> {account.Withdraw(200);}) {Name = "Thread 2"};
        
        t1.Start();
        t2.Start();
        t1.Join();  // Wait for t1 to finish    
        t2.Join();  // Wait for t2 to finish    

        PrintBalance(account);
    }


    public static void PrintBalance(Account account) {
        Console.WriteLine($"Account balance: {account.GetBalance()} for account {account.GetId()}");
    }
}
