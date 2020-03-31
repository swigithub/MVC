CREATE TABLE [dbo].[FM_VehicleGroup] (
    [VehicleGroupId] INT           IDENTITY (1, 1) NOT NULL,
    [Title]          VARCHAR (50)  NOT NULL,
    [Description]    VARCHAR (500) NOT NULL,
    [IsActive]       BIT           NULL,
    [IsDelete]       BIT           NULL
);

