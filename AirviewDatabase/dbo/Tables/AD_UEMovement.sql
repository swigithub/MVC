CREATE TABLE [dbo].[AD_UEMovement] (
    [UEMovementId] NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [UEId]         NUMERIC (18) NOT NULL,
    [UserId]       NUMERIC (18) NOT NULL,
    [UEStatusId]   NUMERIC (18) NOT NULL,
    [Date]         DATETIME     NOT NULL,
    CONSTRAINT [PK_AD_UEMovement] PRIMARY KEY CLUSTERED ([UEMovementId] ASC)
);

