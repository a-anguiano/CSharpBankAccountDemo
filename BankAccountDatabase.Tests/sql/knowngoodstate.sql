CREATE PROCEDURE [SetKnownGoodState]
AS
BEGIN
    ALTER TABLE Transactions DROP CONSTRAINT FK_Transactions_BankAccountId;
    TRUNCATE TABLE Transactions;
    TRUNCATE TABLE BankAccounts;
    ALTER TABLE Transactions add constraint FK_Transactions_BankAccountId
	           foreign key (AccountId)
			   references BankAccounts(Id);
    
    INSERT INTO BankAccounts(AccountHolder, CurrentBalance)
                      VALUES('Alice', 50.00);

    INSERT INTO BankAccounts(AccountHolder, CurrentBalance)
                      VALUES('Bob', 0.00);

    INSERT INTO Transactions(AccountId, TransactionType, [Timestamp], Amount, Note)
                      VALUES(1, 'DEPOSIT', CURRENT_TIMESTAMP, 50.00, 'Initial Deposit');
END;