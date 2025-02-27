
namespace AccountTesting {

    using Account;


    [TestClass]
    public sealed class AccountTests {

        [TestMethod]
        public void CreateAccount_WithValidInitialBalance_ShouldHaveCorrectBalance()
        {
            // Create an account with an initial balance of 1000.00
            
            // Arrange
            double initialBalance = 1000.00;

            // Act
            Account account = new(initialBalance);

            // Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(initialBalance, actualBalance);
        }

        [TestMethod]
        public void CreateAccount_WithUniqueAccountId()
        {

            // Create two accounts, accountA and accountB,
            // the two accounts should have different Ids

            // Arrange
            double initialBalance = 1000.00;

            // Act
            Account accountA = new(initialBalance);
            Account accountB = new(initialBalance);

            // Assert
            Assert.AreNotEqual(accountA.GetId(), accountB.GetId());
        }

        [TestMethod]
        public void Withdraw_WithValidAmount_UpdatesBalance()
        {

            //Withdraw 19.21 from account with initial balance of 1000,
            //account should have 980.79 after withdrawal

            double initialBalance = 1000.00;
            double withdrawAmount = 19.21;
            double expectedBalance = initialBalance - withdrawAmount; // 1000 - 19.21 = 980.79

            // Act
            Account account = new(initialBalance);
            account.Withdraw(withdrawAmount);

            // Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void Withdraw_WithInsufficientFunds_ShouldNotChangeBalance(){

            //Attempt to withdraw 150 from account with initial balance of 100,
            //balance should remain 100 and an error message should be printed
            //indicating insufficient funds

            double initialBalance = 100.00;
            double withdrawAmount = 150.00;
            string expectedOutputMessage = $"Insufficient funds";

            //Act
            Account account = new(initialBalance);
            var stringWriter  = new StringWriter();
            Console.SetOut(stringWriter);
            account.Withdraw(withdrawAmount);

            //Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(initialBalance, actualBalance); //Assumes the balance does not change since there isn't enough funds to withdraw the amount

            var actualOutPut = stringWriter.ToString().Trim();
            Console.WriteLine(actualOutPut);
            Assert.IsTrue(actualOutPut.Contains(expectedOutputMessage)); //Confirms the Insufficient funds message is printed
        }

        [TestMethod]
        public void Deposit_WithValidAmount_ShouldIncreaseBalance(){

            //deposit 150 to account with initial balance of 100,
            //account should have 250 after deposit

            double initialBalance = 100.00;
            double depositAmount = 150.00;
            double expectedBalance = initialBalance + depositAmount; // 100 + 150 = 250

            //Act
            Account account = new(initialBalance);
            account.Deposit(depositAmount);

            //Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(expectedBalance, actualBalance);
        }
        
        [TestMethod]
        public void Transfer_WithValidAmount_UpdateBothAccountBalances(){

            // Transfer 200 from accountA (1000) to accountB (500), 
            // accountA should have 800 and accountB should have 700

            Account accountA = new(1000.00);
            Account accountB = new(500.00);
            double transferAmount = 200.00;
            double expectedBalanceA = accountA.GetBalance() - transferAmount; // 1000 - 200 = 800
            double expectedBalanceB = accountB.GetBalance() + transferAmount; // 500 + 200 = 700

            //Act
            accountA.Transfer_WithDeadlockPrevention(transferAmount, accountB);
        

            //Assert
            double actualBalanceA = accountA.GetBalance();
            double actualBalanceB = accountB.GetBalance();
            Assert.AreEqual(expectedBalanceA, actualBalanceA);
            Assert.AreEqual(expectedBalanceB, actualBalanceB);
        }

        [TestMethod]
        public void Transfer_WithInvalidAmount_NoChangeToBalances(){

            // Transfer 2000 from accountA (1000) to accountB (500), 
            // accountA has insufficient funds to transfer the amount
            // accountA should have 1000 and accountB should have 500

            double transferAmount = 2000.00;
            string expectedOutputMessage = $"Insufficient funds";


            //Act
            Account accountA = new(1000.00);
            Account accountB = new(500.00);
            double expectedBalanceA = accountA.GetBalance();
            double expectedBalanceB = accountB.GetBalance();

            var stringWriter  = new StringWriter();
            Console.SetOut(stringWriter);
            accountA.Transfer_WithDeadlockPrevention(transferAmount, accountB);


            //Assert
            double actualBalanceA = accountA.GetBalance();
            double actualBalanceB = accountB.GetBalance();
            Assert.AreEqual(expectedBalanceA, actualBalanceA);
            Assert.AreEqual(expectedBalanceB, actualBalanceB);

            var actualOutPut = stringWriter.ToString().Trim();
            Console.WriteLine(actualOutPut);
            Assert.IsTrue(actualOutPut.Contains(expectedOutputMessage)); //Confirms the Insufficient funds message is printed
        }
    
    }    

}