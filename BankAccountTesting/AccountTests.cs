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
    }
}