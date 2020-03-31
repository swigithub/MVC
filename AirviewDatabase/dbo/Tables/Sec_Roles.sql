CREATE TABLE [dbo].[Sec_Roles] (
    [RoleId]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (500) NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Sec_Roles_IsActive] DEFAULT ((1)) NULL,
    [ModifyDate]  DATETIME       NULL,
    [DefaultUrl]  NVARCHAR (150) NULL,
    CONSTRAINT [PK_Sec_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

