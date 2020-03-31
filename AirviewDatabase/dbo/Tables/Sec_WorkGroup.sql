CREATE TABLE [dbo].[Sec_WorkGroup] (
    [WorkGroupId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [IsDeleted]   BIT           NULL,
    [CreatedOn]   DATETIME      CONSTRAINT [DF_Sec_WorkGroup_CreatedOn] DEFAULT (getdate()) NULL,
    [CreatedBy]   INT           NULL,
    [ModifiedOn]  DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    CONSTRAINT [PK_Sec_WorkGroup] PRIMARY KEY CLUSTERED ([WorkGroupId] ASC)
);

