CREATE TYPE [dbo].[TaskStages] AS TABLE (
    [StageId]     INT            NULL,
    [Title]       NVARCHAR (255) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [SortOrder]   INT            NULL,
    [ProjectId]   NUMERIC (18)   NULL,
    [IsDeleted]   BIT            NULL,
    [TaskId]      NUMERIC (18)   NULL);

