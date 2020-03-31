-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sec_GetUserSettings]
@Filter NVARCHAR(50)
,@Value1 NVARCHAR(50)=NULL
,@Value2 NVARCHAR(50)=NULL
,@Value3 NVARCHAR(50)=NULL
,@Value4 NVARCHAR(50)=NULL
AS
BEGIN
--	 [dbo].[Sec_GetUserSettings] 'UserApplications',10053
	IF	@Filter='UserApplications'
	BEGIN
		--@Value1: UserId
		SELECT DISTINCT sus.UserId,sus.TypeId, sus.TypeValue, ad.DefinationName 'ApplicationName', '/Content/Images/Excel.jpg' 'IconURL'
		FROM Sec_UserSettings AS sus
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sus.TypeValue AND ad.PDefinationId=sus.TypeId
		WHERE sus.UserId=@Value1
	END
	
	ELSE IF	@Filter='Download_App_Bundle'
	BEGIN
		--@Value1: AppId
		--@Value2: UserId
		--@Value3: EPin
		--@Value4: TPin
		SELECT DISTINCT sus.UserId,sus.TypeId, sus.TypeValue, aa.AppName, aa.PackageName,
		       aa.ModuleId, aa.Version,aa.AppURL 'URL'
		FROM Sec_UserSettings AS sus
		INNER JOIN AD_Applications AS aa ON aa.ModuleId=sus.TypeValue
		WHERE sus.UserId=@Value2
		AND aa.ModuleId=@Value1
		AND sus.EmailPIN=@Value3 AND sus.MobilePIN=@Value4
		AND sus.IsRequested=1 AND sus.IsRequestApproved=1 AND sus.IsDownloaded=0 
		--AND DATEDIFF(hour,sus.PinGenerateDate,GETDATE())<24
	END
	--  [dbo].[Sec_GetUserSettings] 'Pendding_Apps',10053
	ELSE IF	@Filter='Pendding_Apps'
	BEGIN
		--@Value1: UserId
		
		SELECT sus.UserId,sus.TypeId, sus.TypeValue, ad.DefinationName 'ApplicationName', '/Content/Images/Excel.jpg' 'IconURL'
		FROM Sec_UserSettings AS sus
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=sus.TypeValue AND ad.PDefinationId=sus.TypeId
		WHERE sus.UserId=@Value1 AND sus.IsRequestApproved=1 AND sus.IsRequested=1 AND sus.IsDownloaded=0
		
	END
	
--	[dbo].[Sec_GetUserSettings] 'Pendding_Request'
	ELSE IF	@Filter='Pendding_Request'
	BEGIN
		--@Value1: UserId
		
		select sec.*,df.DefinationName 'App_Category',def.DefinationName 'App_Name',sc.FirstName 'FirstName',sc.LastName 'LastName'
		from Sec_UserSettings sec
		inner join AD_Definations def on def.DefinationId=sec.TypeValue
		inner join AD_Definations df on df.DefinationId=sec.TypeId
		inner join Sec_Users sc on sc.UserId=sec.UserId
		where sec.IsRequested=1 and sec.IsDownloaded=0 and sec.IsRequestApproved=0
		
	END
	
	--[dbo].[Sec_GetUserSettings] 'Requests_By_Date'
	ELSE IF	@Filter='Requests_By_Date'
	BEGIN
		--@Value1: UserId
		
		select sec.*,df.DefinationName 'App_Category',def.DefinationName 'App_Name',sc.FirstName 'FirstName',sc.LastName 'LastName'
		from Sec_UserSettings sec
		inner join AD_Definations def on def.DefinationId=sec.TypeValue
		inner join AD_Definations df on df.DefinationId=sec.TypeId
		inner join Sec_Users sc on sc.UserId=sec.UserId
		where sec.IsRequested=0 and sec.IsDownloaded=1 and sec.IsRequestApproved=1 and sec.PinGenerateDate=@Value1
		
	END
	-- [dbo].[AD_GetUserEquipments]	 'Get_UserAppToken',10053
	ELSE IF @Filter = 'Get_UserAppToken'
	     BEGIN
	         SELECT ue.Token,us.EmailPIN,us.MobilePIN
	         FROM   AD_UserEquipments ue
	         INNER JOIN Sec_UserSettings AS us ON us.UEId=ue.UEId
	         WHERE  us.UserId= @Value1 AND us.TypeValue=@Value2  AND ue.IsActive= 1
	     END
--	[dbo].[Sec_GetUserSettings] 'Pending_Notifications',10053     
	     ELSE IF @Filter = 'Pending_Notifications'
	          BEGIN
	             SELECT  ad.DefinationName 'ApplicationName',sus.EmailPIN,sus.MobilePIN, '/Content/Images/Excel.jpg' 'IconURL', sus.ApprovalDate
					FROM Sec_UserSettings AS sus
					INNER JOIN AD_Definations AS ad ON ad.DefinationId=sus.TypeValue AND ad.PDefinationId=sus.TypeId
					WHERE sus.UserId=@Value1 AND sus.IsRequested=1 AND sus.IsRequestApproved=1 AND sus.IsDownloaded=0
	          END
			  ELSE IF @Filter = 'UserProjects'
	     BEGIN
	         SELECT * 
	         FROM   PM_Projects pp
	         INNER JOIN Sec_UserProjects AS us ON us.ProjectId=PP.ProjectId
			 
			 left JOIN PM_ProjectEntity AS pe ON pe.Id=IsNull(PP.EntityId, 1)

	         WHERE  us.UserId= @Value1
	     END
		  ELSE IF @Filter = 'All_Projects'
	     BEGIN
	         --SELECT *
	         --FROM   PM_Projects pp
		
			 SELECT distinct pp.*
	         FROM   PM_Projects pp
			 --INNER JOIN Sec_UserProjects AS us ON us.ProjectId=PP.ProjectId
			 --inner join Sec_Users as u on u.UserId = us.UserId 
			 WHERE pp.IsActive=1
			 --where u.UserId = @Value1 or @Value1 = 0
	     END

		 ELSE IF @Filter = 'byUserId'
	     BEGIN
	          	SELECT sus.UserId,sus.TypeId, sus.TypeValue,sus.UEId,sus.IsDownloaded, ad.DefinationName 'ApplicationName', '/Content/Images/Excel.jpg' 'IconURL'
				FROM Sec_UserSettings AS sus
				INNER JOIN AD_Definations AS ad ON ad.DefinationId=sus.TypeValue AND ad.DefinationId=sus.TypeValue --AND ad.PDefinationId=sus.TypeId
				WHERE sus.UserId=@Value1
	     END



		 ELSE IF	@Filter='ResetPin'
			BEGIN
		--@Value1: UserId
		--@Value2: IMEI

		Select 11 'UserId', '356095064009947' IMEI, '123' EPin,'456' TPin, 'msraza_173@yahoo.com' 'Email';
		--SELECT sus.UserId,sus.TypeId, sus.TypeValue, aa.AppName, aa.PackageName,
		--       aa.ModuleId, aa.Version,aa.AppURL 'URL'
		--FROM Sec_UserSettings AS sus
		--INNER JOIN AD_Applications AS aa ON aa.ModuleId=sus.TypeValue
		--WHERE sus.UserId=@Value2
		--AND sus.EmailPIN=@Value3 AND sus.MobilePIN=@Value4
		--AND sus.IsRequested=1 AND sus.IsRequestApproved=1 AND sus.IsDownloaded=0 

	END


		
END