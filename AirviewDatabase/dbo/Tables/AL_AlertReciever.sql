CREATE TABLE [dbo].[AL_AlertReciever] (
    [AlertRecieverId]  INT NOT NULL,
    [AlertId]          INT NOT NULL,
    [UserId]           INT NOT NULL,
    [IsPushAlertSent]  BIT NOT NULL,
    [IsPushAlertRead]  BIT NOT NULL,
    [IsEmailAlertSent] BIT NOT NULL
);

