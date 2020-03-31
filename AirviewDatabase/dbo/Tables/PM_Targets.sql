CREATE TABLE [dbo].[PM_Targets] (
    [SrId]           NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [ProjectId]      NUMERIC (18)  NULL,
    [MilestoneId]    NUMERIC (18)  NULL,
    [StageId]        NUMERIC (18)  NULL,
    [TargetType]     NVARCHAR (15) NULL,
    [TargetDate]     DATETIME      NULL,
    [TargetValue]    NVARCHAR (15) NULL,
    [SiteCount]      INT           NULL,
    [UserId]         NUMERIC (18)  NULL,
    [CreatedOn]      DATETIME      NULL,
    [RevisionId]     INT           NULL,
    [TargetYear]     INT           NULL,
    [TargetFromDate] DATETIME      NULL,
    [TargetEndDate]  DATETIME      NULL,
    CONSTRAINT [PK_PM_Targets] PRIMARY KEY CLUSTERED ([SrId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Daily,Weekly,Monthly,Quarterly,Yearly', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PM_Targets', @level2type = N'COLUMN', @level2name = N'TargetType';

