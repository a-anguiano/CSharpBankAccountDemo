drop database if exists BankOfKlueberia;
create database BankOfKlueberia;
use BankOfKlueberia;

-- Bank Account
-- public int Id { get; set; }
-- public string AccountHolder { get; set; }
-- public decimal CurrentBalance { get; set; }
create table BankAccounts (
	Id int primary key identity(1,1),
	AccountHolder nvarchar(255),
	CurrentBalance decimal(8,2)
);

-- Transactions
-- public int Id { get; set; }
-- public int AccountId { get; set; }
-- public TransactionType Type { get; set; }
-- public DateTime Timestamp { get; set; }
-- public decimal Amount { get; set; }
-- public string Note { get; set; }

create table Transactions (
	Id int primary key identity(1,1),
	AccountId int not null,
	TransactionType nvarchar(10),
	[Timestamp] DateTime,
	Amount decimal(8,2),
	Note nvarchar(255),
	constraint FK_Transactions_BankAccountId
	           foreign key (AccountId)
			   references BankAccounts(Id)
);