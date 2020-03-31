CREATE TABLE [dbo].[TSS_SiteQuestions] (
    [SiteQuestionId]    NUMERIC (18)    IDENTITY (1, 1) NOT NULL,
    [SiteSurveyId]      NUMERIC (18)    NULL,
    [SiteSectionId]     NUMERIC (18)    NULL,
    [QuestionId]        NUMERIC (18)    NULL,
    [IsRequired]        BIT             NULL,
    [IsNoteRequired]    BIT             NULL,
    [IsImageRequired]   BIT             NULL,
    [IsBarCodeRequired] BIT             NULL,
    [IsAnswered]        BIT             NULL,
    [IsInclude]         BIT             NULL,
    [MapImage]          NVARCHAR (MAX)  NULL,
    [MapZoom]           INT             NULL,
    [Azimuth]           NUMERIC (18, 2) NULL,
    CONSTRAINT [PK_TSS_SiteQuestions] PRIMARY KEY CLUSTERED ([SiteQuestionId] ASC)
);

