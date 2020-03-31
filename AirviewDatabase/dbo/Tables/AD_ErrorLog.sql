CREATE TABLE [dbo].[AD_ErrorLog] (
    [ErrorID]        NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [LoginID]        NUMERIC (18)   NULL,
    [ControllerName] NVARCHAR (50)  NULL,
    [ActionName]     NVARCHAR (50)  NULL,
    [ErrorMessage]   NVARCHAR (250) NULL,
    [StackTrace]     NVARCHAR (MAX) NULL,
    [ErrorTimestamp] DATETIME       CONSTRAINT [DF_AD_ErrorLog_ErrorTimestamp] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AD_ErrorLog] PRIMARY KEY CLUSTERED ([ErrorID] ASC)
);

