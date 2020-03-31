CREATE TABLE [dbo].[TSS_SiteSections] (
    [SiteSectionId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteSurveyId]      NUMERIC (18)   NULL,
    [SectionId]         NUMERIC (18)   NULL,
    [QuestionsAnswered] INT            NULL,
    [TotalQuestions]    INT            NULL,
    [StatusId]          NUMERIC (18)   NULL,
    [IsActive]          BIT            NULL,
    [RepeatCount]       INT            NULL,
    [PSectionId]        INT            NULL,
    [IsRepeatable]      BIT            NULL,
    [ChildTitle]        NVARCHAR (50)  NULL,
    [IsApplicable]      BIT            NOT NULL,
    [Signature]         NVARCHAR (MAX) NULL,
    [IsInclude]         BIT            NULL,
    [PSiteSectionId]    NUMERIC (18)   NULL,
    [IsDeletable]       BIT            NULL,
    CONSTRAINT [PK_TSS_SiteSections] PRIMARY KEY CLUSTERED ([SiteSectionId] ASC)
);

