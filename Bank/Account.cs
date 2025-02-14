namespace Bank;

    public class Account
    {

        private readonly int Id = 0;
        private double Balance;

        private readonly Mutex mutexLock = new();

        public Account (double initialBalance)
        {
            this.Id = Id + 1;
            this.Balance = initialBalance;
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
                this.Balance += amount;
                Console.WriteLine(Thread.CurrentThread.Name);
                Console.WriteLine($"Credit {amount} | New Balance {Balance} \n");
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

                Console.WriteLine(Thread.CurrentThread.Name);

                if (this.Balance < amount) {
                    Console.WriteLine($"Insufficient funds - debit {amount}  | Balance {Balance} \n");
                } else {
                    this.Balance -= amount;
                    Console.WriteLine($"Debit {amount}  | New Balance {Balance} \n");
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            } finally {
                mutexLock.ReleaseMutex();
            }
            
        } 

    }
