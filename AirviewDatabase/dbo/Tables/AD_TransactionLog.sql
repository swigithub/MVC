CREATE TABLE [dbo].[AD_TransactionLog] (
    [LogId]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [TableId]         NUMERIC (18)   NOT NULL,
    [RecordId]        NUMERIC (18)   NOT NULL,
    [LoginId]         NUMERIC (18)   NOT NULL,
    [TransactionDate] ROWVERSION     NOT NULL,
    [TransactionType] NVARCHAR (50)  NOT NULL,
    [TransactionData] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AD_TransactionLog] PRIMARY KEY CLUSTERED ([LogId] ASC)
);

