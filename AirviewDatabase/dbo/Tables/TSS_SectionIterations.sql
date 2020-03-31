CREATE TABLE [dbo].[TSS_SectionIterations] (
    [SecIterationId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [SiteSurveyId]   NUMERIC (18) NULL,
    [SectionId]      NUMERIC (18) NULL,
    [PSectionId]     NUMERIC (18) NULL,
    [IterationId]    NUMERIC (18) NULL,
    [PIterationId]   NUMERIC (18) NULL,
    [StatusId]       NUMERIC (18) NULL,
    CONSTRAINT [PK_TSS_Sections_Iterations] PRIMARY KEY CLUSTERED ([SecIterationId] ASC)
);

