CREATE TABLE [dbo].[AL_AlertRoleSubscription] (
    [AlertRoleSubsId]      INT      IDENTITY (1, 1) NOT NULL,
    [AlertConfigId]        INT      NOT NULL,
    [RoleId]               INT      NOT NULL,
    [IsSubscribed]         BIT      NOT NULL,
    [IsPushAlertRequired]  BIT      NOT NULL,
    [IsEmailAlertRequired] BIT      NOT NULL,
    [ModifiedOn]           DATETIME NOT NULL,
    [ModifiedBY]           INT      NOT NULL,
    CONSTRAINT [PK_AL_AlertRoleSubscription] PRIMARY KEY CLUSTERED ([AlertRoleSubsId] ASC)
);

