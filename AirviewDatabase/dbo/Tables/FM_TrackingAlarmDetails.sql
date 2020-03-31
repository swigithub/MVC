CREATE TABLE [dbo].[FM_TrackingAlarmDetails] (
    [AlarmDetailID]     INT           IDENTITY (1, 1) NOT NULL,
    [TrackingID]        INT           NOT NULL,
    [AlarmCode]         NVARCHAR (15) NULL,
    [AlarmThresholdVal] FLOAT (53)    NULL,
    [AlarmCurrentVal]   FLOAT (53)    NULL,
    [Status]            BIT           NULL
);

