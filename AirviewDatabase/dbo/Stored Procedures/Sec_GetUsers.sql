-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

/*----MoB!----*/
/*
	[dbo].[Sec_Get_User] 'DeviceLogin','rohan','356095064009947'
exec [dbo].[Sec_GetUsers] 'UserByUserId',11
*/
CREATE PROCEDURE Sec_GetUsers -- 
		@FILTER varchar(max),
		@Value1 varchar(max) = NULL,
		@Value2 varchar(max) = NULL,
		@Value3 varchar(max) = NULL,
		@Value4 varchar(max) = NULL
		 
AS
BEGIN

            IF @FILTER = 'byEmail'
				BEGIN
				Select * from Sec_Users  where Email=@Value1
				END
  --  [dbo].[Sec_GetUsers] 'byUserName','admin',1
		    else IF @FILTER = 'byUserName'
				BEGIN
					Select  u.UserId,u.FirstName,u.LastName,u.Email,u.Picture,u.UserName,u.Password,u.Address,u.Contact,u.IsAdmin
					from Sec_Users  u					
					where UserName=@Value1 and u.IsActive=1
				END
 --  [dbo].[Sec_GetUsers] 'Login','admin',1
				else IF @FILTER = 'Login'
				BEGIN
					Select  u.UserId,u.FirstName,u.LastName,u.Email,u.Picture,u.UserName,u.Password,u.Address,u.Contact,u.IsAdmin,ur.RoleId,r.Name as RoleName,r.DefaultUrl,udr.DaysForward,udr.DaysBack,u.CompanyId,u.IsManager
					from Sec_Users  u
					inner join Sec_UserRoles  ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					left join Sec_UserDateRights udr on udr.UserId=u.UserId
					where UserName=@Value1 and u.IsActive=1
				END

			else IF @FILTER = 'ById'
				BEGIN
					--Select * from Sec_Users as u
					--inner join Sec_UserRoles as ur on ur.UserId=u.UserId
					-- where u.UserId=@Value1
					Select u.*,r.Name as RoleName,r.RoleId from Sec_Users as u
					inner join Sec_UserRoles as ur on ur.UserId=u.UserId
					inner join Sec_Roles as r on r.RoleId = ur.RoleId
					where u.UserId = @Value1
				END

			else IF @FILTER = 'All'
				BEGIN
					Select u.*,r.RoleId,r.Name 'RoleName'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					WHERE u.IsActive=1
				END
				else IF @FILTER = 'UsersForBPMN'
				BEGIN
				select * from Sec_Users where IsAdmin = 0 and IsActive=1
				END
				else IF @FILTER = 'UserByUserId'
				BEGIN
					Select u.*,r.RoleId,r.Name 'RoleName'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					WHERE u.IsActive=1 and ReportToId = CONVERT(numeric(20,0),@Value1)
				END
				else IF @FILTER = 'By_Client_Company'
				BEGIN
					Select u.*,r.RoleId,r.Name 'RoleName'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					WHERE u.IsActive=1 and u.CompanyId=@Value1
				END

				else IF @FILTER = 'ByStatus'
				BEGIN
					Select u.*
					from Sec_Users u
					WHERE u.IsActive=@Value1
					order by u.FirstName, u.LastName
				END

---  [dbo].[Sec_GetUsers] 'Paging',0,10,'tes'
				else IF @FILTER = 'Paging'
				BEGIN
				    
					Select u.*,r.RoleId,r.Name 'RoleName',ac.ClientName 'ClientName',rp.FirstName+' '+rp.LastName 'ReportTo'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					left join Sec_Users rp on u.ReportToId=rp.UserId
					left join AD_Clients ac on ac.ClientId=u.CompanyId
					where (u.UserName like '%'+@Value3+'%' or u.FirstName like '%'+@Value3+'%' or r.Name like '%'+@Value3+'%' or ac.ClientName like '%'+@Value3+'%') 
					and (u.ReportToId = @Value4 or u.UserId = @Value4)
					Order By u.UserId OFFSET cast(@Value1 as int)   ROWS FETCH NEXT cast(@Value2 as int) ROWS ONLY	

					select count(1) 'TotalRecord'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					left join AD_Clients ac on ac.ClientId=u.CompanyId
					WHERE (u.UserName like '%'+@Value3+'%' or u.FirstName like '%'+@Value3+'%' or r.Name like '%'+@Value3+'%' or ac.ClientName like '%'+@Value3+'%') 
					and (u.ReportToId = @Value4 or u.UserId = @Value4)
					
				END

			
			-- [dbo].[Sec_GetUsers] 'byRoleName','Tester'
		    ELSE IF @FILTER='byRoleName'
				BEGIN
		    	 Select * from Sec_Users u inner join Sec_UserRoles ur on ur.UserId=u.UserId where ur.RoleId=(SELECT sr.RoleId FROM Sec_Roles AS sr WHERE sr.Name=@Value1)
				END

			else IF @FILTER = 'byRoleId'
				BEGIN
					Select * from Sec_Users u inner join Sec_UserRoles ur on ur.UserId=u.UserId where ur.RoleId=@Value1
				END
				
--	[dbo].[Sec_GetUsers] 'DeviceLogin','tester','357377051948959'
			else IF @FILTER = 'DeviceLogin'
				BEGIN
					-- @Value1 = UserName , @Value2 = IMEI
					Select u.UserId 'Id', u.UserId,u.FirstName,u.LastName,u.Email,u.Picture,u.UserName,u.Password,u.Address,u.Contact,u.IsAdmin,ur.RoleId,r.Name as RoleName,u.IsManager,ud.MAC,
					ud.IMEI
					  from Sec_Users  u
					inner join Sec_UserRoles  ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					inner join Sec_UserDevices ud on ud.UserId=u.UserId
					where UserName=@Value1 and u.IsActive=1 AND ud.isActive=1 AND ud.IMEI=@Value2
				END

--    [dbo].[Sec_GetUsers] 	'User_Assinged_Testers',10096			
			ELSE IF @FILTER = 'UserAssinged_Testers_Devices'
			BEGIN
				--DESC: Get All Testers Assigned to User
				SELECT DISTINCT usr.UserId, usr.FirstName, usr.LastName, usr.UserName
				FROM Sec_Users usr
				INNER JOIN Sec_UserCities cty ON cty.UserId=cty.UserId
				INNER JOIN Sec_UserRoles rol ON rol.UserId=usr.UserId AND rol.UserId=cty.UserId
				WHERE rol.RoleId=13 AND usr.IsActive=1
				AND cty.CityId IN
				(
					SELECT DISTINCT CityId
					FROM Sec_Users usr
					INNER JOIN Sec_UserCities AS cty ON cty.UserId=cty.UserId 
					WHERE usr.UserId=@Value1 AND cty.UserId=@Value1
				)
				
				
				-- User Devices
				SELECT sud.DeviceId,sud.UserId,sud.IMEI,sud.Model,sud.Manufacturer
				  FROM Sec_UserDevices AS sud WHERE sud.UserId IN (SELECT DISTINCT usr.UserId FROM Sec_Users usr
						INNER JOIN Sec_UserCities cty ON cty.UserId=cty.UserId
						INNER JOIN Sec_UserRoles rol ON rol.UserId=usr.UserId AND rol.UserId=cty.UserId
						WHERE rol.RoleId=13 AND usr.IsActive=1
						AND cty.CityId IN ( SELECT DISTINCT CityId FROM Sec_Users usr
						INNER JOIN Sec_UserCities AS cty ON cty.UserId=cty.UserId 
						WHERE usr.UserId=@Value1 AND cty.UserId=@Value1)
				) AND sud.isActive=1
				
			END

--      [dbo].[Sec_GetUsers] 'ByCityId',5
		ELSE IF @FILTER = 'ByCityId'
		     BEGIN
		         -- @Value1 = CityId
		         SELECT u.UserId,
		                u.FirstName,
		                u.LastName
		         FROM   Sec_Users u
		                INNER JOIN Sec_UserCities uc
		                     ON  uc.UserId = u.UserId
		         WHERE  uc.CityId = @Value1
		                AND u.IsActive = 1
		     END
		 ELSE IF @FILTER = 'ByCompanyId'
		      BEGIN
		          SELECT *,u.ReportToId 'Reporting'
		          FROM   Sec_Users AS u
		          WHERE  u.CompanyId = @Value1
		      END
		      
	---	[dbo].[Sec_GetUsers] --'ByProjectId',0,1	      
		     ELSE IF @FILTER = 'ByProjectId'
		          BEGIN
						SELECT cl.ClientId, cl.ClientName, rl.RoleId, rl.Name 'RoleName',u.*
						FROM   Sec_Users AS u
						INNER JOIN Sec_UserProjects AS sup ON  sup.UserId = u.UserId
						inner join AD_Clients cl on cl.ClientId=u.CompanyId
						inner join Sec_UserRoles sur on sur.UserId=u.UserId
						inner join Sec_Roles rl on rl.RoleId=sur.RoleId
		              WHERE  sup.ProjectId = @Value1
		          END
				  else IF @FILTER = 'hierarchy'
				BEGIN
					Select u.*,r.RoleId,u.Designation 'title',u.FirstName +' '+u.LastName 'name'
					from Sec_Users u
					inner join Sec_UserRoles ur on ur.UserId=u.UserId
					inner join Sec_Roles r on r.RoleId=ur.RoleId
					WHERE u.IsActive=1
				END
				-- sitecode  10054 select * from AD_Definations Where DefinationId = 167
				-- [dbo].[Sec_GetUsers] 'GetUserSummary','2017-01-02','2017-1-4','10054' 
				  else IF @FILTER = 'GetUserSummary'
				BEGIN
				
				SELECT DISTINCT  sit.SiteCode,d.DefinationName 'Status',sit.ScheduledOn,d.ColorCode Color,df.DefinationName ,df.DefinationId,dff.DefinationName as PDefinationName,df.PDefinationId
				from AV_Sites sit
				inner JOIN AV_NetLayerStatus AS nls ON nls.SiteId=sit.SiteId
				inner join AD_Definations d on d.DefinationId=sit.[Status]
				inner join AD_Definations df on df.DefinationId=sit.CityId
				inner join AD_Definations dff on dff.DefinationId=df.PDefinationId
				WHERE nls.TesterId=@Value3 and sit.ScheduledOn between CAST(@Value1 AS DATETIME) AND CAST(@Value2 AS DATETIME) 
				END
END