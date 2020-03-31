CREATE TABLE [dbo].[FM_TrackingVState] (
    [VStateID]   INT           IDENTITY (1, 1) NOT NULL,
    [TrackingID] INT           NULL,
    [AlarmCode]  NVARCHAR (15) NULL,
    [Status]     BIT           NULL
);

