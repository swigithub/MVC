CREATE TABLE [dbo].[AV_LogsInfo] (
    [fileID]        INT            IDENTITY (1, 1) NOT NULL,
    [fileName]      NVARCHAR (100) NULL,
    [createDate]    DATETIME       NULL,
    [pathFile]      NVARCHAR (200) NULL,
    [siteID]        INT            NULL,
    [sectorID]      INT            NULL,
    [networkModeID] INT            NULL,
    [bandID]        INT            NULL,
    [carrierID]     INT            NULL,
    [scopeID]       INT            NULL,
    [fileType]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_flieInformation] PRIMARY KEY CLUSTERED ([fileID] ASC)
);

