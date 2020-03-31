CREATE TABLE [dbo].[AL_Alert] (
    [AlertId]       INT           IDENTITY (1, 1) NOT NULL,
    [AlertConfigId] INT           NOT NULL,
    [EntityId]      INT           NOT NULL,
    [CreatedOn]     DATETIME      NULL,
    [Notification]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AL_Alert] PRIMARY KEY CLUSTERED ([AlertId] ASC)
);

