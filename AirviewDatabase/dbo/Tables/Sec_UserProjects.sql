CREATE TABLE [dbo].[Sec_UserProjects] (
    [UserId]    NUMERIC (18) NOT NULL,
    [ProjectId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_Sec_UserProjects] PRIMARY KEY CLUSTERED ([UserId] ASC, [ProjectId] ASC)
);

