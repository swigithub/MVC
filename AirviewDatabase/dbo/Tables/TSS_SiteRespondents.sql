CREATE TABLE [dbo].[TSS_SiteRespondents] (
    [RespondentId]  NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteSurveyId]  NUMERIC (18)   NULL,
    [SiteSectionId] NUMERIC (18)   NULL,
    [Signature]     NVARCHAR (250) NULL,
    [UserId]        NUMERIC (18)   NULL,
    CONSTRAINT [PK_TSS_SiteRespondents] PRIMARY KEY CLUSTERED ([RespondentId] ASC)
);

