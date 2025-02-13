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
        }

        public void Withdraw(double amount) {
            if (this.Balance < amount) {
                Console.WriteLine("Insufficient funds");
            }
            this.Balance -= amount;
        } 

    }
