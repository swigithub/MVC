CREATE TABLE [dbo].[Sec_DomainConfigurations] (
    [DomainUrl]        NVARCHAR (50)  NULL,
    [FtpUrl]           NVARCHAR (50)  NULL,
    [FtpPort]          NVARCHAR (50)  NULL,
    [FtpUsername]      NVARCHAR (50)  NULL,
    [FtpPassword]      NVARCHAR (50)  NULL,
    [FtpUploadPath]    NVARCHAR (50)  NULL,
    [VideoURL]         NVARCHAR (500) NULL,
    [MessageService]   NVARCHAR (50)  NULL,
    [MediaServer]      NVARCHAR (250) NULL,
    [LTServer]         NVARCHAR (150) NULL,
    [LTServerFilePath] NVARCHAR (150) NULL,
    [SecDomainURL]     NVARCHAR (50)  NULL,
    [IperfServerIP]    NVARCHAR (15)  NULL,
    [IperfServerPort]  NVARCHAR (10)  NULL
);

