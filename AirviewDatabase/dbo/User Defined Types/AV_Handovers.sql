CREATE TYPE [dbo].[AV_Handovers] AS TABLE (
    [SiteId]        INT           NOT NULL,
    [NetworkModeId] INT           NOT NULL,
    [BandId]        INT           NOT NULL,
    [CarrierId]     INT           NOT NULL,
    [prvPCI]        INT           NOT NULL,
    [nxtPCI]        INT           NOT NULL,
    [DriveType]     NVARCHAR (15) NOT NULL);

