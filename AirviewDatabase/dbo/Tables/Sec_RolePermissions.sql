CREATE TABLE [dbo].[Sec_RolePermissions] (
    [RoleId]       NUMERIC (18) NOT NULL,
    [PermissionId] INT          NOT NULL,
    CONSTRAINT [PK_Sec_RolePermissions] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC),
    CONSTRAINT [FK_Sec_RolePermissions_Sec_Permissions] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Sec_Permissions] ([Id]),
    CONSTRAINT [FK_Sec_RolePermissions_Sec_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Sec_Roles] ([RoleId])
);

