CREATE TABLE [dbo].[Sec_UserPermissions] (
    [UserId]       NUMERIC (18) NOT NULL,
    [PermissionId] INT          NOT NULL,
    CONSTRAINT [PK_Sec_UserPermissions] PRIMARY KEY CLUSTERED ([UserId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_Sec_UserPermissions_Sec_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Sec_Permissions] ([Id]),
    CONSTRAINT [FK_Sec_UserPermissions_Sec_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Sec_Users] ([UserId])
);

