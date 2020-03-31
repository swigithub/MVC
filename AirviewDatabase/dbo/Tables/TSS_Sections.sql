CREATE TABLE [dbo].[TSS_Sections] (
    [SectionId]           NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [PSectionId]          NUMERIC (18)   NULL,
    [SurveyId]            NUMERIC (18)   NOT NULL,
    [SectionTitle]        NVARCHAR (150) NOT NULL,
    [Description]         NVARCHAR (500) NULL,
    [SortOrder]           INT            CONSTRAINT [DF_TSS_Sections_SortOrder] DEFAULT ((0)) NOT NULL,
    [IsActive]            BIT            CONSTRAINT [DF_TSS_Sections_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]           DATETIME       CONSTRAINT [DF_TSS_Sections_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]           NUMERIC (18)   NOT NULL,
    [IsRepeatable]        BIT            NULL,
    [IsApplicable]        BIT            NOT NULL,
    [IsSignatureRequired] BIT            CONSTRAINT [DF_TSS_Sections_IsSignatureRequired] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TSS_Sections] PRIMARY KEY CLUSTERED ([SectionId] ASC)
);

