CREATE TABLE [dbo].[AD_Applications] (
    [AppId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [AppName]     NVARCHAR (100) NULL,
    [PackageName] NVARCHAR (100) NULL,
    [ModuleId]    NUMERIC (18)   NULL,
    [Version]     NVARCHAR (15)  NULL,
    [AppURL]      NVARCHAR (150) NULL,
    [ImageURL]    NVARCHAR (500) NULL,
    CONSTRAINT [PK_AD_Applications] PRIMARY KEY CLUSTERED ([AppId] ASC)
);

