using Bank;

namespace BankAccountTesting {

    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void Withdraw_WithValidAmount_UpdatesBalance()
        {
            double initialBalance = 1000;
            double withdrawAmount = 19.21;
            double expectedBalance = 980.79;

            // Act
            Account account = new(initialBalance);
            account.Withdraw(withdrawAmount);

            // Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(expectedBalance, actualBalance);
        }

        [TestMethod]
        public void Withdraw_WithInsufficientFunds_ShouldNotChangeBalance(){
            double initialBalance = 100;
            double withdrawAmount = 150;
            double expectedBalance = 100;
            string expectedOutputMessage = "Insufficient funds";

            //Act
            Account account = new(initialBalance);
            var stringWriter  = new StringWriter();
            Console.SetOut(stringWriter);
            account.Withdraw(withdrawAmount);

            //Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(expectedBalance, actualBalance);

            var outPut = stringWriter.ToString().Trim();
            Assert.AreEqual(expectedOutputMessage, outPut);
        }

        [TestMethod]
        public void Deposit_WithValidAmount_ShouldIncreaseBalance(){
            double initialBalance = 100;
            double depositAmount = 150;
            double expectedBalance = initialBalance + depositAmount;

            //Act
            Account account = new(initialBalance);
            account.Deposit(depositAmount);

            //Assert
            double actualBalance = account.GetBalance();
            Assert.AreEqual(expectedBalance, actualBalance);
        }
    }
}