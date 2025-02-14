namespace Bank;

    public class Account
    {

        private readonly int Id = 0;
        private double Balance;

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

            this.Balance += amount;

            Console.WriteLine(Thread.CurrentThread.Name);
            Console.WriteLine($"Credit {amount} | Balance {Balance} \n");

        }

        public void Withdraw(double amount) {
            
            //Amount is sanitized to be a non-negative value for proper calculation
            amount = Math.Abs(amount);

            Console.WriteLine(Thread.CurrentThread.Name);

            if (this.Balance < amount) {
                Console.WriteLine($"Insufficient funds - debit {amount}  | Balance {Balance} \n");
            } else {
                Console.WriteLine($"Debit {amount}  | Balance {Balance} \n");
                this.Balance -= amount;
            }
        } 

    }
