CREATE TABLE [dbo].[UserClients] (
    [UserId]   NUMERIC (18) NOT NULL,
    [ClientId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_UserClients] PRIMARY KEY CLUSTERED ([UserId] ASC, [ClientId] ASC),
    CONSTRAINT [FK_UserClients_Sec_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Sec_Users] ([UserId])
);

