using DevsBank.ApplicationServices;
using DevsBank.ApplicationServices.ReadModels;
using DevsBank.ApplicationServices.Validators;
using DevsBank.Storage;
using FluentAssertions;

namespace DevsBank.WebApi.Tests.Services;

public class BankUserAccountServiceTests
{
    [Fact]
    public async Task When_opening_an_account_with_initial_credit_it_should_save_to_database()
    {
        // this test is an example of choosing a testing scope that includes the
        // actual implementation of the dependencies used by the BankUserAccountService.
        // explained at: https://www.continuousimprover.com/2023/04/unit-testing-scope.html

        // Arrange
        var initialCredit = 20;
        var storageContext = new StorageContext();
        var bankUserValidator = new BankUserValidator(storageContext);
        var existingBankUser = (await storageContext.DbUsers).First();
        BankUserAccountService bankUserAccountService = new BankUserAccountService(bankUserValidator, storageContext);

        // Act
        await bankUserAccountService.OpenAccount(existingBankUser.Id, initialCredit);

        // Assert
        AccountEntity savedAccount = (await storageContext.Accounts).Single();
        savedAccount.BankUserId.Should().Be(existingBankUser.Id);
        savedAccount.Balance.Should().Be(initialCredit);

        var savedTransaction = (await storageContext.Transactions).Single();
        savedTransaction.TransactedAmount.Should().Be(initialCredit);
        savedTransaction.AccountId.Should().Be(savedAccount.Id);
    }

    [Fact]
    public async Task When_opening_an_account_without_any_credit_it_should_save_to_database()
    {
        // Arrange
        var initialCredit = 0;
        var storageContext = new StorageContext();
        var bankUserValidator = new BankUserValidator(storageContext);
        var existingBankUser = (await storageContext.DbUsers).First();
        BankUserAccountService bankUserAccountService = new BankUserAccountService(bankUserValidator, storageContext);

        // Act
        await bankUserAccountService.OpenAccount(existingBankUser.Id, initialCredit);

        // Assert
        AccountEntity savedAccount = (await storageContext.Accounts).Single();
        savedAccount.BankUserId.Should().Be(existingBankUser.Id);
        savedAccount.Balance.Should().Be(initialCredit);

        (await storageContext.Transactions).Should().BeEmpty();
    }

    [Fact]
    public async Task When_getting_existing_user_accounts_it_should_get_them_with_their_transactions()
    {
        // Arrange
        var initialCredit = 20;
        var storageContext = new StorageContext();
        var bankUserValidator = new BankUserValidator(storageContext);
        var existingBankUser = (await storageContext.DbUsers).First();
        BankUserAccountService bankUserAccountService = new BankUserAccountService(bankUserValidator, storageContext);
        await bankUserAccountService.OpenAccount(existingBankUser.Id, initialCredit);

        // Act
        var readModels = await bankUserAccountService.GetUserAccounts(existingBankUser.Id);

        // Assert
        AccountReadModel readModel = readModels.Single();
        readModel.Balance.Should().Be(initialCredit);
        readModel.Name.Should().Be(existingBankUser.Name);
        readModel.Surname.Should().Be(existingBankUser.Surname);
        readModel.Transactions.Single().TransactedAmount.Should().Be(initialCredit);
    }

    [Fact]
    public async Task When_opening_an_account_for_an_unknown_user_it_should_throw()
    {
        // Arrange
        var initialCredit = 0;
        var storageContext = new StorageContext();
        var bankUserValidator = new BankUserValidator(storageContext);
        var unknownUser = Guid.NewGuid();
        BankUserAccountService bankUserAccountService = new BankUserAccountService(bankUserValidator, storageContext);

        // Act
        Func<Task> whenOpeningAccount = async () =>  await bankUserAccountService.OpenAccount(unknownUser, initialCredit);

        // Assert
        await whenOpeningAccount.Should().ThrowAsync<InvalidOperationException>();
    }
}