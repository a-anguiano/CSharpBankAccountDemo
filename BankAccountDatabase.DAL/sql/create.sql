﻿use master;
go
ALTER DATABASE BankOfKlueberia SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
drop database BankOfKlueberia;
go
create database BankOfKlueberia;
go
use BankOfKlueberia;
go

create table BankAccounts (
	Id int primary key identity(1,1),
	AccountHolder nvarchar(255),
	CurrentBalance decimal(8,2)
);

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

