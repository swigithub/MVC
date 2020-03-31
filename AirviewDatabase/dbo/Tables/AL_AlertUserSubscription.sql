CREATE TABLE [dbo].[AL_AlertUserSubscription] (
    [AlertConfigId]        INT      NOT NULL,
    [UserId]               INT      NOT NULL,
    [IsSubscribed]         BIT      NOT NULL,
    [IsPushAlertRequired]  BIT      NOT NULL,
    [IsEmailAlertRequired] BIT      NOT NULL,
    [ModifiedOn]           DATETIME NULL,
    [ModifiedBy]           INT      NULL
);

