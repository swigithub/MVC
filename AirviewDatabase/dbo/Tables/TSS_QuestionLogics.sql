CREATE TABLE [dbo].[TSS_QuestionLogics] (
    [LogicId]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SurveyId]       NUMERIC (18)   NULL,
    [SectionId]      NUMERIC (18)   NOT NULL,
    [FromQuestionId] NUMERIC (18)   NOT NULL,
    [ToQuestionId]   NVARCHAR (500) NOT NULL,
    [ConditionId]    NUMERIC (18)   NOT NULL,
    [ResponseId]     NUMERIC (18)   NOT NULL,
    [ActionId]       NUMERIC (18)   NOT NULL,
    [IsActive]       BIT            CONSTRAINT [DF_TSS_QuestionLogics_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_TSS_QuestionLogics] PRIMARY KEY CLUSTERED ([LogicId] ASC)
);

