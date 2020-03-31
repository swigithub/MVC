CREATE TABLE [dbo].[Sec_UserScopes] (
    [UserId]  NUMERIC (18) NOT NULL,
    [ScopeId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Sec_UserScopes] PRIMARY KEY CLUSTERED ([UserId] ASC, [ScopeId] ASC)
);

