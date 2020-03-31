CREATE TABLE [dbo].[Sec_UserSettings] (
    [UserSettingId]     NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UserId]            NUMERIC (18) NULL,
    [TypeId]            NUMERIC (18) NULL,
    [TypeValue]         NUMERIC (18) NULL,
    [EmailPIN]          INT          NULL,
    [MobilePIN]         INT          NULL,
    [PinGenerateDate]   DATETIME     NULL,
    [IsRequested]       BIT          NULL,
    [RequestDate]       DATETIME     NULL,
    [IsRequestApproved] BIT          NULL,
    [ApprovalDate]      DATETIME     NULL,
    [IsDownloaded]      BIT          NULL,
    [DownloadDate]      DATETIME     NULL,
    [UEId]              NUMERIC (18) NULL,
    CONSTRAINT [PK_Sec_UserSettings] PRIMARY KEY CLUSTERED ([UserSettingId] ASC)
);

