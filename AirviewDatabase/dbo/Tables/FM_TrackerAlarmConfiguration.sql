CREATE TABLE [dbo].[FM_TrackerAlarmConfiguration] (
    [AlarmConfigID]   INT            IDENTITY (1, 1) NOT NULL,
    [AlarmCode]       NVARCHAR (15)  NULL,
    [TrackerId]       INT            NULL,
    [IsEnabled]       BIT            NULL,
    [ThresholdValues] NUMERIC (6, 2) NULL,
    [IsApplied]       BIT            NULL,
    [ModifiedBy]      INT            NULL,
    [ModifiedOn]      DATETIME       NULL,
    [SendAlert]       BIT            NULL,
    CONSTRAINT [PK_FM_TrackerAlarmConfiguration] PRIMARY KEY CLUSTERED ([AlarmConfigID] ASC)
);

