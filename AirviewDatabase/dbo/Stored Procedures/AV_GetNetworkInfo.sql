
CREATE PROCEDURE [dbo].[AV_GetNetworkInfo]
	
AS
BEGIN
Select DISTINCT Modes.DefinationId NetworkModeId, Modes.DefinationName NetworkModeName, Bands.DefinationId BandId, Bands.DefinationName BandName,crr.DefinationId CarrierId,
crr.DefinationName Carrier
from AD_Definations Modes
Inner Join AD_Definations Bands ON Modes.DefinationId = Bands.PDefinationId
Inner Join AD_Definations crr ON crr.PDefinationId = Bands.DefinationId
Inner Join AD_DefinationTypes DEF_TY ON DEF_TY.DefinationTypeId = Modes.DefinationTypeId 
Where DEF_TY.DefinationType = 'NetworkMode' 
ORDER BY Modes.DefinationName, Bands.DefinationName, crr.DefinationName
END