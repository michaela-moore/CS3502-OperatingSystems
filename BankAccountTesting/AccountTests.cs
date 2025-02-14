using Bank;

namespace BankAccountTesting {

    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Withdraw_WithValidAmount_UpdatesBalance()
        {
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
    }
}