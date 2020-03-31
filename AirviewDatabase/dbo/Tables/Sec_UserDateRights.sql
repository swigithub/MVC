CREATE TABLE [dbo].[Sec_UserDateRights] (
    [Id]          NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]      NUMERIC (18) NOT NULL,
    [DaysForward] INT          NULL,
    [DaysBack]    INT          NULL,
    [AssignDate]  DATETIME     NULL,
    [IsActive]    BIT          NULL,
    CONSTRAINT [PK_UserDateRights] PRIMARY KEY CLUSTERED ([Id] ASC)
);

