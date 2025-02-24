
using System.Security.Principal;

namespace Bank;

    public class Account
    {

        private readonly int Id;
        private static int nextId = 1;
        private double Balance;

        private readonly Mutex mutexLock = new(); //used in single account transactions
        private readonly object lockObject = new();

        public Account (double initialBalance)
        {
            this.Id = nextId;
            nextId += 1;
            this.Balance = initialBalance;

            Console.WriteLine($"Account {Id} created with balance {RoundAmount(Balance)}");
        }

        public int GetId() {
            return this.Id;
        }

        public double GetBalance() {
            return this.Balance;
        }

        public void Deposit(double amount) {

            mutexLock.WaitOne();
            
            try {
                Thread.Sleep(100); //Simulated a delay in processing the transaction
                
                this.Balance += amount;
                
                Console.Write($"{Thread.CurrentThread.Name} ");
                Console.WriteLine($"(+) {amount} | Balance {RoundAmount(Balance)}");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                mutexLock.ReleaseMutex();
            }
        }

        public void Withdraw(double amount) {
            
            mutexLock.WaitOne();

            try {
            
                amount = Math.Abs(amount);  //sanitized to be a positive value for calculation

                Console.Write($"{Thread.CurrentThread.Name} ");
                
                Thread.Sleep(100); //Simulate a delay in processing the transaction

                if (this.Balance < amount) {
                    Console.WriteLine($"Insufficient funds to debit {amount} | Act# {Id} balance {RoundAmount(Balance)}");
                } else {
                    this.Balance -= amount;
                    Console.WriteLine($"(-) {amount}  | Act# {Id} Balance {RoundAmount(Balance)}");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                mutexLock.ReleaseMutex();
            }
            
        } 

        public void Transfer_WithDeadlockDetection(double amount, Account to) {

            //This implementation locks the resources so that a separate process 
            // can monitor and show a case where the threads get blockd indefinitely

            Console.WriteLine($"Thread {Thread.CurrentThread.Name} attempting to transfer {amount} from Act# [{Id}] to [{to.GetId()}]");

            lock (this.lockObject) { //Lock the sending account

                Thread.Sleep(500); //Emmulate a delay

                lock (to.lockObject) {  //Lock the receiving account

                    //Check if the account has sufficient funds to transfer
                    if (amount > this.Balance) {
                        Console.WriteLine($"Insufficient funds to transfer {amount} | Balance: {RoundAmount(Balance)}");
                        return;
                    
                    } else {
                            //Withdraw from the sending account
                            this.Withdraw(amount);

                            //Deposit to the receiving account
                            to.Deposit(amount);

                            Console.WriteLine($"--> {amount} transferred from Act# [{Id}] to [{to.GetId()}]");
                        }
                    }
            }
        }

        public void Transfer_WithDeadlockPrevention(double amount, Account to) {

            int attempts = 0;
            int maxRetries = 8;
            bool firstResourceLocked = false;
            bool secondResourceLocked = false;

            while (attempts < maxRetries) {
                
                attempts++;

                try {
                
                    Console.WriteLine($"\nThread {Thread.CurrentThread.Name} attempting to transfer {amount} from Act# [{Id}] to [{to.GetId()}]");

                    firstResourceLocked = Monitor.TryEnter(this.lockObject, 3000); //Lock the sending account
                        
                    if (firstResourceLocked) {
                        
                        Thread.Sleep(500); //emmulate a delay

                        secondResourceLocked = Monitor.TryEnter(to.lockObject, 3000); //Lock the receiving account

                        if (secondResourceLocked){

                            //Check if the account has sufficient funds       
                            if(amount < this.Balance){

                                //Withdraw from the sending account
                                this.Withdraw(amount);

                                //Deposit to the receiving account
                                to.Deposit(amount);

                                Console.WriteLine($"--> {amount} transferred from Act# [{Id}] to [{to.GetId()}]");
                                
                                //Successful transfer
                                return;                            
                            } 
                            
                            else {
                                Console.WriteLine($"\n--> Insufficient funds to transfer {amount} | Balance: {RoundAmount(Balance)}");
                            }
                             
                        }

                        else {
                            Console.WriteLine($"\n** Unable to acquire (second) lock on receiving account {to.GetId()}");
                        } 

                    }

                    else {
                        Console.WriteLine($"\n** Unable to acquire (first) lock on sending account {Id}");
                    }            
                }
                
                finally {
                    //Release the locks
                    if (firstResourceLocked) {
                        Monitor.Exit(this.lockObject);
                    }

                    if (secondResourceLocked) {
                        Monitor.Exit(to.lockObject);
                    }
                }    
            }  
        }



        private static double RoundAmount(double amount){
            return Math.Round(amount, 2);
        }
    }
