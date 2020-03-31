CREATE TABLE [dbo].[TSS_SiteResponses] (
    [QReponseId]     NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteQuestionId] NUMERIC (18)   NULL,
    [ResponseId]     NUMERIC (18)   NULL,
    [ResponseText]   NVARCHAR (250) NULL,
    [ResponseValue]  NVARCHAR (MAX) NULL,
    [IterationId]    NUMERIC (18)   NULL,
    [PIterationId]   NUMERIC (18)   NULL,
    [MinValue]       FLOAT (53)     NULL,
    [MaxValue]       FLOAT (53)     NULL,
    [IsGps]          BIT            NULL,
    [Signature]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TSS_SiteResponses] PRIMARY KEY CLUSTERED ([QReponseId] ASC)
);

