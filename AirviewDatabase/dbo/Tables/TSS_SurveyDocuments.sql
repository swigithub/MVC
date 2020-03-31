CREATE TABLE [dbo].[TSS_SurveyDocuments] (
    [SurveyId]      NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ClientId]      NUMERIC (18)   NULL,
    [CityId]        NUMERIC (18)   NULL,
    [SurveyTitle]   NVARCHAR (150) NOT NULL,
    [Description]   NVARCHAR (500) NULL,
    [CategoryId]    NUMERIC (18)   NOT NULL,
    [SubCategoryId] NUMERIC (18)   NOT NULL,
    [UnitSystemId]  NUMERIC (18)   NULL,
    [IsActive]      BIT            CONSTRAINT [DF_TSS_SurveyDocuments_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]     DATETIME       CONSTRAINT [DF_TSS_SurveyDocuments_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]     NUMERIC (18)   NOT NULL,
    [IsPublished]   BIT            NULL,
    [SurveyCode]    NVARCHAR (20)  NULL,
    CONSTRAINT [PK_TSS_SurveyDocuments] PRIMARY KEY CLUSTERED ([SurveyId] ASC)
);

