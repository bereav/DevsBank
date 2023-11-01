using DevsBank.Domain;
using DevsBank.Domain.DomainValidators;
using FakeItEasy;
using FluentAssertions;

namespace DevsBank.WebApi.Tests.Domain;

public class BankUserAccountTests
{
    [Fact]
    public void When_adding_money_to_account_it_should_update_the_balance()
    {
        // Arrange
        var userAccount = new BankUser(Guid.NewGuid(), "Valentin", "Berea");
        var validator = A.Fake<IBankUserValidator>();
        A.CallTo(() => validator.BankUserExists(userAccount)).Returns(true);
        var bankAccount = new BankUserAccount(Guid.NewGuid(), userAccount, validator);

        // Act
        bankAccount.AddMoney(24);

        // Assert
        bankAccount.Balance.Should().Be(24);
    }

    [Fact]
    public void When_retrieving_money_from_account_it_should_update_the_balance()
    {
        // Arrange
        var userAccount = new BankUser(Guid.NewGuid(), "Valentin", "Berea");
        var validator = A.Fake<IBankUserValidator>();
        A.CallTo(() => validator.BankUserExists(userAccount)).Returns(true);
        var bankAccount = new BankUserAccount(Guid.NewGuid(), userAccount, validator);

        // Act
        bankAccount.AddMoney(24);
        bankAccount.AddMoney(-10);

        // Assert
        bankAccount.Balance.Should().Be(14);
    }

    [Fact]
    public void When_an_unknown_bank_user_wants_to_use_a_bank_account_it_should_throw()
    {
        // Arrange
        var userAccount = new BankUser(Guid.NewGuid(), "Evil", "Creature");
        var validator = A.Fake<IBankUserValidator>();
        A.CallTo(() => validator.BankUserExists(userAccount)).Returns(false);


        // Act
        Action bankAccountInstantiation = () => new BankUserAccount(Guid.NewGuid(), userAccount, validator);

        // Assert
        bankAccountInstantiation.Should().Throw<InvalidOperationException>("Unknown bank account user");
    }
}