-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_GetUserEquipments]
 @Filter nvarchar(50)=''
,@Value nvarchar(50)=null
,@Value2 nvarchar(50)=null
,@Value3 nvarchar(50)=null

AS
BEGIN
-- [dbo].[AD_GetUserEquipments] 'BySerialNo','356095062725114'
	if @Filter='BySerialNo'
	begin
		select * 
		from AD_UserEquipments
		where SerialNo=@Value and IsActive=1
	end

--  [dbo].[AD_GetUserEquipments] 'ById','1'
	else if @Filter='ById'
	begin
		select * 
		from AD_UserEquipments
		where UEId=@Value and IsActive=1
	end

--  [dbo].[AD_GetUserEquipments] 'All','1'
	else if @Filter='All'
	begin
		select ue.UEId, def.DefinationName AS 'Device', ue.Manufacturer, ue.Model,ue.SerialNo, ue.MAC, ue.UENumber,ue.IsActive, ue.UEStatusId,ue.Token 
		from AD_UserEquipments  ue
		Inner join AD_Definations def On def.DefinationId= ue.UETypeId
		where ue.IsActive=@Value
	end

--  [dbo].[AD_GetUserEquipments] 'All','1'
	else if @Filter='byUEStatusId'
	begin
		select * 
		from AD_UserEquipments
		where UEStatusId=@Value
	end

--  [dbo].[AD_GetUserEquipments] 'ReturnedDevice','1'
	else if @Filter='ReturnedDevice'
	begin
		select * 
		from AD_UserEquipments
		where UETypeId=@Value and UEStatusId=(select DefinationId from AD_Definations where DefinationName='Return')
	end

--  [dbo].[AD_GetUserEquipments] 'ReturnedDevice','1'
	else if @Filter='IssuedDevice'
	begin
		select * 
		from AD_UserEquipments
		where UETypeId=@Value and UEStatusId=(select DefinationId from AD_Definations where DefinationName='Issue')
	end

--- [dbo].[AD_GetUserEquipments] 'Paging',0,10,''
	else IF @FILTER = 'Paging'
	BEGIN
	    select * 
		from AD_UserEquipments ue
		where ue.SerialNo like '%'+@Value3+'%' or ue.MAC like '%'+@Value3+'%' or ue.Model like '%'+@Value3+'%' or ue.UENumber like '%'+@Value3+'%'
		Order By ue.UEId OFFSET cast(@Value as int)   ROWS FETCH NEXT cast(@Value2 as int) ROWS ONLY


		select count(1) 'TotalRecord'
		from AD_UserEquipments ue
		where ue.SerialNo like '%'+@Value3+'%' or ue.MAC like '%'+@Value3+'%' or ue.Model like '%'+@Value3+'%'
	END

	ELSE IF @Filter='AssignedDevices'
	BEGIN
		
		SELECT udev.UEId,ue.UETypeId,
		def.DefinationName AS 'Device',udev.IMEI, ue.Manufacturer, ue.Model, ue.SerialNo, ue.MAC, ue.UENumber,us.UserId,us.FirstName+' '+us.LastName 'UserFullName'
		FROM Sec_UserDevices AS udev
		INNER JOIN Sec_Users AS us ON us.UserId = udev.UserId
		INNER JOIN AD_UserEquipments AS ue ON ue.UEId =udev.UEId
		INNER JOIN AD_Definations def On def.DefinationId= ue.UETypeId	
		where udev.isActive=1
		
	END

	ELSE IF @Filter='AvailableDevices'
	BEGIN
		SELECT ue.UEId,ue.UETypeId, 
		def.DefinationName AS 'Device', ue.Manufacturer, ue.Model,ue.SerialNo, ue.MAC, ue.UENumber,ue.IsActive, ue.UEStatusId,ue.Token 
		from AD_UserEquipments ue
		inner join AD_Definations def On def.DefinationId= ue.UETypeId			
		where UEStatusId  is null
		--where def.DefinationName='Return' ---AND KeyCode='UE_STATUS'
	END

END