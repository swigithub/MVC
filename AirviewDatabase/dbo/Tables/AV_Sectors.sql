CREATE TABLE [dbo].[AV_Sectors] (
    [SectorId]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SectorCode]        VARCHAR (50)  NOT NULL,
    [NetworkModeId]     NUMERIC (18)  NOT NULL,
    [ScopeId]           NUMERIC (18)  NOT NULL,
    [BandId]            NUMERIC (18)  NOT NULL,
    [CarrierId]         NUMERIC (18)  NOT NULL,
    [Antenna]           VARCHAR (50)  NOT NULL,
    [BeamWidth]         FLOAT (53)    NOT NULL,
    [Azimuth]           FLOAT (53)    NOT NULL,
    [PCI]               INT           NOT NULL,
    [SiteId]            NUMERIC (18)  NOT NULL,
    [TestStatus]        INT           NULL,
    [isActive]          BIT           CONSTRAINT [DF_AV_Sectors_isActive_1] DEFAULT ((1)) NULL,
    [RFHeight]          INT           NULL,
    [MTilt]             INT           NULL,
    [ETilt]             INT           NULL,
    [BandWidth]         INT           NULL,
    [sectorColor]       NVARCHAR (10) NULL,
    [CellId]            NVARCHAR (10) NULL,
    [MRBTS]             NVARCHAR (50) NULL,
    [SectorLatitude]    FLOAT (53)    NULL,
    [SectorLongitude]   FLOAT (53)    NULL,
    [SiteClusterId]     NUMERIC (18)  NULL,
    [VerticalBeamwidth] FLOAT (53)    NULL,
    [AntennaDownTilt]   FLOAT (53)    NULL,
    CONSTRAINT [PK_AV_Sectors] PRIMARY KEY CLUSTERED ([SectorId] ASC),
    CONSTRAINT [FK_AV_Sectors_AV_Sites] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[AV_Sites] ([SiteId])
);


GO
CREATE TRIGGER [dbo].[AV_InsertSectorLog] ON [dbo].[AV_Sectors]
AFTER UPDATE
AS
BEGIN
	DECLARE @SectorId INT
	DECLARE @SectorCode NVARCHAR(50)	
	DECLARE @NetworkModeId NUMERIC
	DECLARE @ScopeId NUMERIC
	DECLARE @BandId NUMERIC
	DECLARE @CarrierId NUMERIC
	DECLARE @Antenna NVARCHAR(50)
	DECLARE @BeamWidth FLOAT
	DECLARE @Azimuth FLOAT
	DECLARE @PCI INT
	DECLARE @SiteId NUMERIC
	DECLARE @TestStatus INT
	
	
	SELECT @SectorId = INSERTED.SectorId, @SiteId=INSERTED.SiteId, @SectorCode = INSERTED.SectorCode, @NetworkModeId=INSERTED.NetworkModeId, @BandId=INSERTED.BandId,
	@CarrierId=INSERTED.CarrierId, @ScopeId=INSERTED.ScopeId, @BeamWidth=INSERTED.BeamWidth, @Azimuth=INSERTED.Azimuth, @PCI=INSERTED.PCI
	FROM INSERTED;
		
		
    --INSERT INTO AirViewLogs.dbo.AV_Sectors(SectorId, SectorCode, NetworkModeId, ScopeId, BandId,
    --            CarrierId, Antenna, BeamWidth, Azimuth, PCI, SiteId, TestStatus,LogType,LogDate,HostName)
    --SELECT as1.SectorId, as1.SectorCode, as1.NetworkModeId, as1.ScopeId,
    --       as1.BandId, as1.CarrierId, as1.Antenna, as1.BeamWidth, as1.Azimuth,
    --       as1.PCI, as1.SiteId, as1.TestStatus,'Update',GETDATE(),HOST_NAME()
    --  FROM AV_Sectors AS as1
    --WHERE as1.SectorId=@SectorId
    
 --   UPDATE AV_Sectors
	--SET SectorLatitude = (SELECT x.Latitude FROM AV_Sites x WHERE x.SiteId=AV_Sectors.SiteId),
	--SectorLongitude =  (SELECT x.Longitude FROM AV_Sites x WHERE x.SiteId=AV_Sectors.SiteId),
	--NetworkModeId=@NetworkModeId, BandId = @BandId, CarrierId = @CarrierId, ScopeId = @ScopeId, PCI = @PCI, BeamWidth = @BeamWidth, Azimuth = @Azimuth
 --   WHERE SiteId=@SiteId AND (ISNULL(SectorLatitude,0)=0 AND ISNULL(SectorLongitude,0)=0)
 
 
UPDATE AV_SiteTestSummary 
SET PCIid = @PCI, BeamWidth = @BeamWidth, Azimuth = @Azimuth
 WHERE SiteId=@SiteId AND SectorId=@SectorId
  

END