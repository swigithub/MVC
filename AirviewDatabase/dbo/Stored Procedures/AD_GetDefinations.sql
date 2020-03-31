-- =============================================
-- Author:		/*----MoB!----*/
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


/*
 [dbo].[AD_GetDefinations] 'AllCities'
 [dbo].[AD_GetDefinations] 'WO Status'
 [dbo].[AD_GetDefinations] 'NetworkModes'
 [dbo].[AD_GetDefinations] 'Bands'
 [dbo].[AD_GetDefinations] 'Carriers'
 [dbo].[AD_GetDefinations] 'States'
 [dbo].[AD_GetDefinations] 'Regions'
 [dbo].[AD_GetDefinations] 'Cities'
 [dbo].[AD_GetDefinations] 'Types'
 
 */
CREATE PROCEDURE AD_GetDefinations
	@Filter nvarchar(100),
	@value nvarchar(100)=NULL
	
	
AS
BEGIN
	
	
    --if @Filter='AllActive'
    --BEGIN
	   --select y.DefinationId, y.DefinationName, y.PDefinationId,
	   --       y.DefinationTypeId, y.KeyCode, y.DisplayType, y.ColorCode,
	   --       y.InputType, y.MaxLength, y.SortOrder, y.IsActive
	   --  from AD_Definations y WHERE IsActive=1 AND DefinationTypeId NOT IN(6)
	   --UNION ALL
	   --select x.DefinationId, ad.DefinationName+' '+x.DefinationName 'DefinationName', x.PDefinationId,
	   --       x.DefinationTypeId, x.KeyCode, x.DisplayType, x.ColorCode,
	   --       x.InputType, x.MaxLength, x.SortOrder, x.IsActive
	   --from AD_Definations x
	   --INNER JOIN AD_Definations AS ad ON ad.DefinationId=x.PDefinationId
	   --WHERE x.IsActive=1 AND x.DefinationTypeId IN(6)
    --END
    
    --ELSE 
	if @Filter='Countries'
		BEGIN
			select def.* from AD_Definations def
			 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
			 where adt.DefinationType='Country'
		END
    ELSE if @Filter='ByParentId'
    BEGIN
	   select * from AD_Definations
	   WHERE IsActive=1 AND DefinationTypeId=(SELECT adt.PDefinationTypeId FROM AD_DefinationTypes AS adt WHERE adt.DefinationTypeId=@value)
    END
    else if @Filter='AllCities'
    begin
	   select * from AD_Definations where DefinationTypeId=7

	 end
	  else if @Filter='CitiesWithPDefinationName'
    begin
	  select def.*,ad.DefinationName as PDefinationName,ad.PDefinationId from AD_Definations def
	     INNER JOIN AD_Definations AS ad on ad.DefinationId=def.PDefinationId
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		
		 where adt.DefinationType='City'
	 end

--	 [dbo].[AD_GetDefinations] 'UserCities',10101
	 else if @Filter='UserCities'
	 begin
		select def.* from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		inner join Sec_UserCities uc on uc.CityId=def.DefinationId
		where adt.DefinationType='City' and uc.UserId=@value
	 END
	 else if @Filter='UserLocations'
	 begin
		--select def.* from AD_Definations def
		--INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		--INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=def.PDefinationId
		--inner join Sec_UserCities uc on uc.CityId=def.DefinationId
		--where adt.DefinationType='City' and uc.UserId=@value
		--UNION ALL
		--select rgn.* from AD_Definations def
		--INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		--INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=def.PDefinationId
		--inner join Sec_UserCities uc on uc.CityId=def.DefinationId
		--where adt.DefinationType='City' and uc.UserId=@value

		SELECT DISTINCT def.*, rgn.DefinationName 'PDefinationName'
			from AD_Definations def
			INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId			
			inner join Sec_UserCities uc on uc.CityId=def.DefinationId
			INNER JOIN AD_Definations AS rgn ON rgn.DefinationId=def.PDefinationId
			INNER JOIN AD_Definations AS stt ON stt.DefinationId=rgn.PDefinationId
			where adt.DefinationType='City' and uc.UserId=@Value AND def.IsActive=1
		ORDER BY def.PDefinationId,def.DefinationId
	 end

--	 [dbo].[AD_GetDefinations] 'UserRegions',10101
	 else if @Filter='UserRegions'
	 begin
		select distinct pdef.* from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId		
		inner join AD_Definations pdef on pdef.DefinationId=def.PDefinationId
		inner join Sec_UserCities uc on uc.CityId=def.DefinationId
		
		where adt.DefinationType='City' and uc.UserId=@value
	 end

--	 [dbo].[AD_GetDefinations] 'UserStates',10101
	 else if @Filter='UserStates'
	 begin
		select distinct stdef.* from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId		
		inner join AD_Definations pdef on pdef.DefinationId=def.PDefinationId
		inner join AD_Definations stdef on stdef.DefinationId=pdef.PDefinationId
		inner join Sec_UserCities uc on uc.CityId=def.DefinationId
		
		where adt.DefinationType='City' and uc.UserId=@value
	 end
--	 [dbo].[AD_GetDefinations] 'UserScopes',10101
	 ELSE if @Filter='UserScopes'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 inner join Sec_UserScopes us on us.ScopeId=def.DefinationId
		 where adt.DefinationType='Scope' and us.UserId=@value
	END

--   [dbo].[AD_GetDefinations] 'WO Status'
	 ELSE if @Filter='WO Status'
	 begin
	    select * from AD_Definations where DefinationTypeId=17
	end
	 ELSE if @Filter='NetworkModes'
	BEGIN
		select def.* from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='NetworkMode'
	END
		 
	
	ELSE if @Filter='Bands'
		BEGIN
			select def.* from AD_Definations def
			 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
			 where adt.DefinationType='Band'
		END
		 
	ELSE if @Filter='Carriers'
		BEGIN
			select def.* from AD_Definations def
			 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
			 where adt.DefinationType='Carrier' AND def.IsActive=1
		END
		 
		 
	ELSE if @Filter='States'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 where adt.DefinationType='State' 
	END
	
	ELSE if @Filter='Regions'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 where adt.DefinationType='Region'
	END
	ELSE if @Filter='RegionParent'
	 BEGIN
		select def.* ,dt.DefinationType,pdef.DefinationName 'PDefinationName'
		from AD_Definations def
		inner join AD_DefinationTypes dt on dt.DefinationTypeId=def.DefinationTypeId
		left join AD_Definations pdef on pdef.DefinationId=def.PDefinationId
		where dt.DefinationType='Region'
	 END
	ELSE if @Filter='Cities'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId

		 where adt.DefinationType='City'
	END
	ELSE if @Filter='CitiesByPId'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 where adt.DefinationType='City' And def.PDefinationId=@value
	END
		-- [dbo].[AD_GetDefinations] 'Scopes' 
	ELSE if @Filter='Scopes'
	BEGIN
		--select def.* from AD_Definations def
		-- INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		-- where adt.DefinationType='Scope'
		 select Distinct def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 inner join Sec_UserScopes us on us.ScopeId=def.DefinationId
		 where adt.DefinationType='Scope' and (us.UserId=@value or @value = 0)
	END
	
	
	ELSE if @Filter='Sectors'
	BEGIN
		select * from AD_Definations def
		where KeyCode='SECTOR'
	END
	
	ELSE if @Filter='By_KeyCode'
	BEGIN
		select * from AD_Definations def
		where KeyCode='ISSUE_STATUS'
	END
	
	ELSE if @Filter='IssueTypes'
	BEGIN
		select def.* from AD_Definations def
		 INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		 where adt.DefinationType='Issue Type'
	END
	
	
	ELSE if @Filter='Reasons'
	BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='Reasons'
	END
	
	ELSE if @Filter='RedriveTypes'
	BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='Redrive Types'
	END

	ELSE if @Filter='ReportTypes'
	BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='Report'
	END
	ELSE if @Filter='SiteTypes'
	BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='SiteType'
	END
	
	ELSE if @Filter='SiteClasses'
	BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='Site Class'
	END
	-- [dbo].[AD_GetDefinations] 'Colors' 
	 ELSE if @Filter='Colors'
	 BEGIN
		  select def.* 
		  from AD_Definations def
		  INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		  where adt.DefinationType='Color'
	 END

	--  [dbo].[AD_GetDefinations] 'byDefinationTypeId',39
	 ELSE if @Filter='byDefinationTypeId'
	 BEGIN
		select def.* ,dt.DefinationType,pdef.DefinationName 'PDefinationName'
		from AD_Definations def
		inner join AD_DefinationTypes dt on dt.DefinationTypeId=def.DefinationTypeId
		left join AD_Definations pdef on pdef.DefinationId=def.PDefinationId
		where def.DefinationTypeId=@value
	 END



--   [dbo].[AD_GetDefinations] 'byDefinationType','DataType'
	 ELSE if @Filter='byDefinationType'
	 BEGIN
		select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType=@value AND def.IsActive=1
	 END



	 ELSE if @Filter='byUnitType'
	 BEGIN
	 	select def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='UnitType' and def.PDefinationId= @value 
	 END


 -- [dbo].[AD_GetDefinations] 'ReportTree'
	 ELSE if @Filter='ReportTree'
	 BEGIN
		select DISTINCT def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType in( 'Report','Module') and def.IsActive=1
			
	 END
	  -- [dbo].[AD_GetDefinations] 'ParentDefinations',15
	 ELSE if @Filter='ParentDefinations'
	 BEGIN
		select Distinct def.*,pdef.DefinationName 'PDefinationName'
		from AD_Definations def
		INNER JOIN AD_DefinationTypes  AS adt on adt.PDefinationTypeId= def.DefinationTypeId
		left join AD_Definations pdef on pdef.DefinationId=def.PDefinationId
		where adt.DefinationTypeId=@value
		--Group By def.DefinationName,def.PDefinationId,def.DefinationId
	 END



 -- [dbo].[AD_GetDefinations] 'ReportFilters_byReportId',33246
	 ELSE if @Filter='ReportFilters_byReportId'
	 BEGIN
		select def.*
		from AD_ReportFilters rf
		inner join AD_Definations def on def.DefinationId=rf.FilterId
		where rf.ReportId=@Value and rf.IsActive=1
	 END


 -- [dbo].[AD_GetDefinations] 'PDefinationId',122
	 ELSE if @Filter='PDefinationId'
	 BEGIN
		select def.* 
		from AD_Definations def
		where def.PDefinationId=@Value and def.IsActive=1

	 END

	 -- [dbo].[AD_GetDefinations] 'GetDisplayTypes',' '
	 ELSE if @Filter='GetDisplayTypes'
	 BEGIN
		SELECT def.DefinationId, def.DefinationName,def.DisplayType
		FROM AD_Definations AS def
		WHERE def.DefinationTypeId=33 AND def.DisplayType='select'

	 END
	 -- [dbo].[AD_GetDefinations] 'GetFloors',' '
	 ELSE if @Filter='GetFloors'
	 BEGIN
		select DISTINCT def.* 
		from AD_Definations def
		INNER JOIN AD_DefinationTypes AS adt on adt.DefinationTypeId=def.DefinationTypeId
		where adt.DefinationType='Floor no.'
	 END


	 	ELSE IF @FILTER = 'byModuleApps'
			BEGIN
				--SELECT * FROM AD_Definations AS ad
				--WHERE ad.PDefinationid IN(103308)

				SELECT * FROM AD_Definations AS ad
				WHERE ad.PDefinationid IN(103308,113308,33230)
			END

END