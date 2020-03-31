CREATE TABLE [dbo].[Sec_UserCities] (
    [UserId] INT NOT NULL,
    [CityId] INT NOT NULL,
    CONSTRAINT [PK_Sec_UserCities] PRIMARY KEY CLUSTERED ([UserId] ASC, [CityId] ASC)
);

