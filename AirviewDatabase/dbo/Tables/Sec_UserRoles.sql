CREATE TABLE [dbo].[Sec_UserRoles] (
    [UserId] NUMERIC (18) NOT NULL,
    [RoleId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Sec_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_Sec_UserRoles_Sec_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Sec_Roles] ([RoleId]),
    CONSTRAINT [FK_Sec_UserRoles_Sec_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Sec_Users] ([UserId])
);

