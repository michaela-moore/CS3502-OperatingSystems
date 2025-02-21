
namespace Bank;

public class Transactions
{

    public static void Main() {

        /* Single Account Transactions ----
            *    
            * For every transaction stored, a thread is created to process each transaction.
            * Depending on the transaction amount (+/-), ProcessTransaction() determies whether to 
            * debit or credit the funds from the remaining balance. 
            
            * Thread management is maintained in the main() program, and mutex locks are 
            * implemented in the Account class to ensure thread safety and prevent race conditions from 
            * incorrectly calculating the remaining balance.
            *  
        */
     
        //Create an account with an initial balance
        Console.WriteLine(" ----- TRANSACTIONS ----- ");
        double initialBalance = 1000.00;
        Account accountA = new(initialBalance);
        
        double[] transactions = [100.02, -25.89, 50.00, -75.00, 200.00, -19.30, 12.04, -100000.01, -0.55, 10.22]; //total = 251.52
        int totalTransactions = transactions.Length; 
        
        // Create a thread for every transaction to be processed
        Thread[] transactionThreads = new Thread[totalTransactions]; 

        for (int i = 0; i < totalTransactions; i++) {
            double transactionAmount = transactions[i];
            transactionThreads[i] = new Thread(() => { ProcessTransaction(transactionAmount, accountA);}) {Name = $"[T #{i}]"};
        }

        // Start all threads
        foreach (Thread thread in transactionThreads) {
            thread.Start();
        }

        // Wait for all threads to complete
        foreach (Thread thread in transactionThreads) {
            thread.Join();
        } 

        // Print the final balance after all transactions are completed
        Console.WriteLine();
        PrintBalance(accountA); //expected balance 1251.52

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /* Cross Account Transfers ----
            *    
            * For every transfer stored, a thread is created to process the transfer utilizing the Transfer() method.
            * The Transfer() method is responsible for debiting and crediting the appropriate accounts.
            *  
        */

        Console.WriteLine("\n ----- TRANSFERS  ----- ");

        //Intialize secondary bank account
        Account accountB = new(55.02);

        //Create series of threads to process each transfer
        List <Thread> transferThreads = [
            new Thread(() => {accountA.Transfer(100.00, accountB);}), // A = 1151.52 | B = 155.02
            new Thread(() => {accountB.Transfer(10.50, accountA);}), // A = 1162.02 | B = 144.52
            new Thread(() => {accountA.Transfer(900.00, accountB);}), // A = 262.02  | B = 1044.52
            new Thread(() => {accountB.Transfer(10000.00, accountA);}) // Insufficient funds
        ];

        // Start all threads
        foreach (Thread thread in transferThreads) {
            thread.Start();
        }

        // Wait for all threads to complete
        foreach (Thread thread in transferThreads) {
            thread.Join();
        }

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
        Console.WriteLine($"Account ID: {account.GetId()} | Balance: {Math.Round(account.GetBalance())}");
    }
}
