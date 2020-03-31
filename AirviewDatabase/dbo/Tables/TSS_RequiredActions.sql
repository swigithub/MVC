CREATE TABLE [dbo].[TSS_RequiredActions] (
    [ActionId]       NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]         NUMERIC (18)   NULL,
    [SiteSurveyId]   NUMERIC (18)   NULL,
    [SiteQuestionId] NUMERIC (18)   NULL,
    [IterationId]    NUMERIC (18)   NULL,
    [PIterationId]   NUMERIC (18)   NULL,
    [ActionTypeId]   NUMERIC (18)   NOT NULL,
    [ActionValue]    NVARCHAR (MAX) NOT NULL,
    [Remarks]        NVARCHAR (500) NULL,
    [Azimuth]        VARCHAR (50)   NULL,
    [Latitude]       VARCHAR (50)   NULL,
    [Longitude]      VARCHAR (50)   NULL,
    [ObjectView]     VARCHAR (50)   NULL,
    [Altitude]       VARCHAR (50)   NULL,
    [GPSAccuracy]    VARCHAR (50)   NULL,
    CONSTRAINT [PK_TSS_RequiredActions] PRIMARY KEY CLUSTERED ([ActionId] ASC)
);

