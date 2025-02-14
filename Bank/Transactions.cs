
namespace Bank;

public class Transactions
{

  
    public static void Main() {
     
        //Create an account with an initial balance
        double initialBalance = 1000.00;
        Account account = new(initialBalance);
        PrintBalance(account);


        double[] transactions = [100.00, -25.00, 50.00, -75.00, 200.00, -19.30, 12.04, -100000.01, -0.55, 10.22]; //expected balance 1252.79
        int totalTransactions = transactions.Length; 
        Thread[] threads = new Thread[totalTransactions]; 
        
        /* THREAD CREATION --------------------
            *   A thread is created for every transaction, which is based on the total number of transactions 
            *   The new threads are then stored in the array variable 'threads'
            *
            *   ProcessTransaction() assess if the amount is positive/negative to determine if the 
            *   transaction amount is debited or credited to the account.
        */
        
        for (int i = 0; i < totalTransactions; i++) {
            double transactionAmount = transactions[i];

            threads[i] = new Thread(() => { ProcessTransaction(transactionAmount, account);}) {Name = $"THREAD # {i}"};
        }


        /* THREAD EXECUTION -------------------*/
        // Start all threads
        foreach (Thread thread in threads) {
            thread.Start();
        }

        // Wait for completion of all threads 
        foreach (Thread thread in threads) {
            thread.Join();
        } 

        PrintBalance(account); //expected balance 1286.99

    }

    //Helper function to determine if the transaction is a deposit or withdrawal
    public static void ProcessTransaction(double transactionAmount, Account account) {
        if (transactionAmount > 0) {
                account.Deposit(transactionAmount);
            } else {
                account.Withdraw(transactionAmount);
            }
    }

    //Helper function to print account summary
    public static void PrintBalance(Account account) {
        Console.WriteLine($"Account balance: {account.GetBalance()} for account ID: {account.GetId()}");
    }
}
