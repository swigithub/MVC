CREATE TABLE [dbo].[AD_FilterQuery] (
    [FilterId] NUMERIC (18)   NOT NULL,
    [sqlQuery] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AD_FilterQuery] PRIMARY KEY CLUSTERED ([FilterId] ASC)
);

