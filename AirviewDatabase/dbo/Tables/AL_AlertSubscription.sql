CREATE TABLE [dbo].[AL_AlertSubscription] (
    [AlertSubId]           INT      IDENTITY (1, 1) NOT NULL,
    [AlertConfigId]        INT      NOT NULL,
    [IsSubscribed]         BIT      NOT NULL,
    [EntityId]             INT      NULL,
    [IsPushAlertRequired]  BIT      NULL,
    [IsEmailAlertRequired] BIT      NULL,
    [ModifiedOn]           DATETIME NULL,
    [ModifiedBy]           INT      NULL,
    CONSTRAINT [PK_AL_AlertSubscription] PRIMARY KEY CLUSTERED ([AlertSubId] ASC)
);

