CREATE TABLE [dbo].[FM_Vehicle] (
    [VehicleId]          INT           IDENTITY (1, 1) NOT NULL,
    [TypeId]             INT           NOT NULL,
    [ManuId]             INT           NOT NULL,
    [ModelId]            INT           NOT NULL,
    [SubModelId]         INT           NOT NULL,
    [Year]               VARCHAR (10)  NOT NULL,
    [ChassisNumber]      VARCHAR (50)  NOT NULL,
    [RegistrationNumber] VARCHAR (50)  NOT NULL,
    [IsActive]           BIT           CONSTRAINT [FM_VehicleIsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]          BIT           CONSTRAINT [FM_VehicleIsDeleted] DEFAULT ((0)) NOT NULL,
    [IsAssign]           BIT           CONSTRAINT [DF_FM_Vehicle_IsAssign] DEFAULT ((0)) NOT NULL,
    [AssignTo]           INT           NULL,
    [VehicleImage]       VARCHAR (200) NULL,
    [VehicleGroupId]     INT           NULL,
    [IMEIId]             INT           NULL,
    [IMEI]               VARCHAR (50)  CONSTRAINT [DF_FM_Vehicle_IMEI] DEFAULT ((0)) NOT NULL,
    UNIQUE NONCLUSTERED ([ChassisNumber] ASC),
    UNIQUE NONCLUSTERED ([RegistrationNumber] ASC)
);

