CREATE TABLE [dbo].[TSS_SiteContacts] (
    [SrId]          NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SiteId]        NUMERIC (18)   NULL,
    [Title]         NVARCHAR (25)  NOT NULL,
    [Gender]        NVARCHAR (15)  NULL,
    [FullName]      NVARCHAR (150) NULL,
    [GateNo]        NVARCHAR (50)  NULL,
    [ContactNo]     NVARCHAR (250) NULL,
    [ContactTypeID] NUMERIC (18)   NULL,
    [DesignationID] NUMERIC (18)   NULL,
    [IsHoldingKeys] BIT            NULL,
    [Comment]       NVARCHAR (500) NULL,
    CONSTRAINT [PK_TSS_SiteContacts] PRIMARY KEY CLUSTERED ([SrId] ASC)
);

