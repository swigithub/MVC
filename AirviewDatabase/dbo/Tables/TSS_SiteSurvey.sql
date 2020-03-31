CREATE TABLE [dbo].[TSS_SiteSurvey] (
    [SiteSurveyId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SiteId]       NUMERIC (18) NOT NULL,
    [SurveyId]     NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_TSS_SiteSurvey] PRIMARY KEY CLUSTERED ([SiteSurveyId] ASC, [SiteId] ASC, [SurveyId] ASC)
);

