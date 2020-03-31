CREATE TABLE [dbo].[FM_VehicleAssignment] (
    [VehicleAssignmentId] INT  IDENTITY (1, 1) NOT NULL,
    [UserId]              INT  NOT NULL,
    [VehicleId]           INT  NOT NULL,
    [DateAssign]          DATE NOT NULL,
    [DateReturn]          DATE NULL,
    [IsTransfer]          BIT  CONSTRAINT [DF_FM_VehicleAssignment_IsTransfer] DEFAULT ((0)) NOT NULL
);

