namespace Bank;

    public class Account
    {

        private readonly int Id;
        private static int nextId = 1;
        private double Balance;

        private readonly Mutex mutexLock = new();

        public Account (double initialBalance)
        {
            this.Id = nextId;
            nextId += 1;
            this.Balance = initialBalance;

            Console.WriteLine($"Account {Id} created with balance {Round(Balance)}");
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
                Thread.Sleep(500); //Simulated a delay in processing the transaction
                
                this.Balance += amount;
                
                Console.Write($"{Thread.CurrentThread.Name} ");
                Console.WriteLine($"(+) {amount} | Act# {Id} Balance {Round(Balance)}");
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
                
                Thread.Sleep(500); //Simulate a delay in processing the transaction

                if (this.Balance < amount) {
                    Console.WriteLine($"Insufficient funds to debit {amount} | Act# {Id} balance {Round(Balance)}");
                } else {
                    this.Balance -= amount;
                    Console.WriteLine($"(-) {amount}  | Act# {Id} Balance {Round(Balance)}");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                mutexLock.ReleaseMutex();
            }
            
        } 

        public void Transfer(double amount, Account to) {
            
            Console.WriteLine($"Transfer {amount} from Act# [{Id}] to Act# [{to.GetId()}]");

            //Check if the account has sufficient funds to transfer
            if (amount > this.Balance) {
                Console.WriteLine($"Insufficient funds to transfer {amount} | Balance: {Round(Balance)}");
                return;
            }

            //Withdraw from the sending account
            this.Withdraw(amount);

            //Deposit to the receiving account
            to.Deposit(amount);
        }

        private static double Round(double amount){
            return Math.Round(amount, 2);
        }
    }
