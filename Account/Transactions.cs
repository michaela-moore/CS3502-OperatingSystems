namespace Account {
    public class Transactions
    {
        public static void Main() {

            /* Account Transactions Using Threads ----
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
            Console.WriteLine(" ----- Account Transactions ----- ");
            double initialBalance = 1000.00;
            Account accountA = new(initialBalance);
            
            double[] transactionsSmall = [100.02, -25.89, 50.00, -75.00, 200.00, -19.30, 12.04, -100000.01, -0.55, 10.22]; //total = 251.52
            double[] transactionsLarge = [-1,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,2,2933414,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,3,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,34,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,234,100,0.1,0.3,0.22,12,43,63.12,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,4.21,34,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,-1,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,234,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,6,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,-3.32,12,12.44,-33.12,-5.31,12,12.55,4.21,3,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,4.21,-1463.3,1023,0.42,-0.55,100,4.31,78.3,-23,4.3,-234.4,100,0.1,0.3,0.22,12,43,63.12];


            Console.WriteLine("Small Batch Processing");
            SingleAccounTransactions(accountA, transactionsSmall);

            Console.WriteLine("Large Batch Processing");
            SingleAccounTransactions(accountA, transactionsLarge);


            ///////////////////////////////////////////////////////////////////////////////
            

            /* Deadlock Detection & Recovery ----
                *    
                * For every transfer stored, a thread is created to process the transfer utilizing the Transfer() method.
                * The Transfer() method is responsible for debiting and crediting the appropriate accounts.
                *  
                * ProcessTransfer_WithDeadlockDetection(accountA, accountB) demonstrates an unmanaged deadlock scenario.
                * ProcessTransfer_WithDeadlockPrevention(accountA, accountB) demonstrates a managed deadlock scenario.

            */

            Console.WriteLine("\n ----- TRANSFERS  ----- ");
            //Intialize secondary bank account
            Account accountB = new(55.02);

            ProcessTransfer_WithDeadlockDetection(accountA, accountB); // remove comment to run active deadlock scenario
            //ProcessTransfer_WithDeadlockPrevention(accountA, accountB); 

        }

        public static void SingleAccounTransactions(Account account, double[] transactions){
            int totalTransactions = transactions.Length; 
            
            // Create a thread for every transaction to be processed
            Thread[] transactionThreads = new Thread[totalTransactions]; 

            for (int i = 0; i < totalTransactions; i++) {
                double transactionAmount = transactions[i];
                transactionThreads[i] = new Thread(() => { ProcessTransaction(transactionAmount, account);}) {Name = $"[T #{i}]"};
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
            PrintBalance(account); 
        }

        public static void ProcessTransfer_WithDeadlockDetection(Account accountA, Account accountB) {

            Console.WriteLine("\nShowing transfers with Deadlock Detection");

            //Create series of threads to process each transfer
            List <Thread> transferThreads = [
                new Thread(() => {accountA.Transfer_WithDeadlockDetection(100.00, accountB);}){Name = "1"}, 
                new Thread(() => {accountB.Transfer_WithDeadlockDetection(10.50, accountA);}){Name = "2"}, 
                new Thread(() => {accountA.Transfer_WithDeadlockDetection(900.00, accountB);}){Name = "3"}, 
                new Thread(() => {accountB.Transfer_WithDeadlockDetection(10000.00, accountA);}){Name = "4"} // Insufficient funds
            ];

            // Start all threads
            foreach (Thread thread in transferThreads) {
                thread.Start();
            }

            //Separate thread for monitoring deadlocks
            DeadlockDetect deadlockDetect = new(); 
            foreach (Thread thread in transferThreads) {
                deadlockDetect.WatchThread(thread);
            }

            Thread monitor = new(deadlockDetect.LogDeadlock);
            monitor.Start();


            // Wait for all threads to complete
            foreach (Thread thread in transferThreads) {
                thread.Join();
            }       
            
            Console.WriteLine("Transfers completed");

        }
        public static void ProcessTransfer_WithDeadlockPrevention(Account accountA, Account accountB) {

            Console.WriteLine("\nShowing transfers with Deadlock Prevention");

            //Create series of threads to process each transfer 
            //Transfer == from.amount.to
                List <Thread> transferThreads = [
                    new Thread(() => {accountA.Transfer_WithDeadlockPrevention(100.00, accountB);}){Name = "1"}, 
                    new Thread(() => {accountB.Transfer_WithDeadlockPrevention(10.50, accountA);}){Name = "2"}, 
                    new Thread(() => {accountA.Transfer_WithDeadlockPrevention(900.00, accountB);}){Name = "3"}, 
                    new Thread(() => {accountB.Transfer_WithDeadlockPrevention(10000.00, accountA);}){Name = "4"} // Insufficient funds
                ];

                // Start all threads
                foreach (Thread thread in transferThreads) {
                    thread.Start();
                }

                //Separate thread for monitoring deadlocks
                DeadlockDetect deadlockDetect = new(); 
                foreach (Thread thread in transferThreads) {
                    deadlockDetect.WatchThread(thread);
                }

                Thread monitor = new(deadlockDetect.LogDeadlock);
                monitor.Start();

                // Wait for all threads to complete
                foreach (Thread thread in transferThreads) {
                    thread.Join();
                }

                Console.WriteLine("Transfers completed");
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
}