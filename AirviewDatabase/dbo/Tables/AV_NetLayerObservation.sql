CREATE TABLE [dbo].[AV_NetLayerObservation] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [LayerStatusId] NUMERIC (18)   NULL,
    [CWComments]    NVARCHAR (MAX) NULL,
    [CCWComments]   NVARCHAR (MAX) NULL,
    [PDSCHComments] NVARCHAR (MAX) NULL,
    [PUSCHComments] NVARCHAR (MAX) NULL
);

