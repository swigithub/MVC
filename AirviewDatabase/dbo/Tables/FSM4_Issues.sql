CREATE TABLE [dbo].[FSM4_Issues] (
    [FA Code]  NVARCHAR (255) NULL,
    [eNB]      NVARCHAR (255) NULL,
    [U/A]      NVARCHAR (255) NULL,
    [Schedule] DATETIME       NULL,
    [Actual]   NVARCHAR (255) NULL,
    [MW]       NVARCHAR (255) NULL,
    [Status]   NVARCHAR (255) NULL,
    [Alarm]    NVARCHAR (255) NULL,
    [AOTS CR#] NVARCHAR (255) NULL,
    [Notes]    NVARCHAR (MAX) NULL,
    [Issues]   NVARCHAR (255) NULL
);

