CREATE TABLE [dbo].[TSS_SiteAttendees] (
    [SiteAttendeeId] NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SiteId]         NUMERIC (18)  NULL,
    [SiteSurveyId]   NUMERIC (18)  NULL,
    [Name]           VARCHAR (100) NULL,
    [Designation]    VARCHAR (50)  NULL,
    [Company]        VARCHAR (100) NULL,
    [Signature]      VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TSS_SiteAttendees] PRIMARY KEY CLUSTERED ([SiteAttendeeId] ASC)
);

