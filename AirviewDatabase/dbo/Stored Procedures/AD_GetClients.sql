-- =============================================
		-- Author:		<Author,,Name>
		-- Create date: <Create Date,,>
		-- Description:	<Description,,>
		-- =============================================
		CREATE PROCEDURE AD_GetClients
		@Filter NVARCHAR(50),
		@value NVARCHAR(50)=NULL,
		@value1 nvarchar(100)=null,
		@Value2 nvarchar(100)=null,
		@Value3 nvarchar(100)=null
		AS
		BEGIN
	
		--  [dbo].[AD_GetClients] 'ByCountryId',1
		IF @Filter='ByCountryId'
		BEGIN
		SELECT ac.*,aca.CountryId
		FROM AD_Clients AS ac
		INNER JOIN AD_ClientAddress AS aca ON ac.ClientId=aca.ClientId
		WHERE aca.CountryId=@value AND aca.IsHeadOffice=1
		END
	
	
		--  [dbo].[AD_GetClients] 'All'		
		else if	@Filter='All'
		BEGIN
		Select cli.* 
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		--WHERE ad.KeyCode='CLIENT' AND cli.IsActive=1
		END
		else if	@Filter='BY_PROFILE_TYPE'
		BEGIN
		Select cli.* 
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		WHERE ad.KeyCode=@value
		END

		--  [dbo].[AD_GetClients] 'AllVendors'		
		else if	@Filter='AllVendors'
		BEGIN
		Select cli.* 
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		WHERE ad.KeyCode='VENDOR'
		END
	
		else if	@Filter='AllRecords'
		BEGIN
		Select cli.*,ad.DefinationName 'ClientType',cl.ClientName 'PClient'
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		left join AD_Clients as cl on cl.ClientId=cli.PClientId
		END
		else if	@Filter='Company'
		BEGIN
		Select cli.*,ad.DefinationName 'ClientType',cl.ClientName 'PClient'
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		left join AD_Clients as cl on cl.ClientId=cli.PClientId
		where ad.DefinationName ='Company'
		END
		else if	@Filter='ById'
		BEGIN
		Select cli.* 
		from AD_Clients cli
		
		WHERE cli.ClientId=@value
		END
		--  [dbo].[AD_GetClients] 'UserClients',10101
		else if	@Filter='UserClients'
		BEGIN
		Select cli.* 
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		inner join UserClients uc on uc.ClientId=cli.ClientId
		WHERE ad.KeyCode='CLIENT' and uc.UserId=@value
		END
		else if	@Filter='ProjectClients'
		BEGIN
		SELECT DISTINCT x.*
		FROM
		(
			SELECT c.*  FROM PM_Projects p inner join AD_Clients c on p.ClientId=c.ClientId
			where ProjectId=@value
			union all
			SELECT c.*  FROM PM_Projects p inner join AD_Clients c on p.EndClientId=c.ClientId
			where ProjectId=@value
		) x
		--Select DISTINCT cli.* 
		--from AD_Clients cli	
		--Inner join PM_Projects P on cli.ClientId = p.ClientId 
		--Inner join PM_Projects PP on  p.ClientId  = Pp.EndClientId	
		--WHERE 
		--select * from PM_Projects
		END

		else if	@Filter='byStatus'
		BEGIN
		--Select cli.* from AD_Clients cli
		--INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		--WHERE ad.KeyCode='CLIENT' AND cli.IsActive=@Value

		Select distinct cli.* 
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		left join UserClients uc on uc.ClientId=cli.ClientId
		WHERE ad.KeyCode='CLIENT'  and cli.IsActive=@Value and (uc.UserId=@value1 or @value1=0)
		END
		else IF @FILTER = 'Paging'
				BEGIN
				    
					Select cli.*,ad.DefinationName 'ClientType',cl.ClientName 'PClient'
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		left join AD_Clients as cl on cl.ClientId=cli.PClientId
					where cli.ClientName like '%'+@Value3+'%' 
					Order By cli.ClientId OFFSET cast(@value as int)   ROWS FETCH NEXT cast(@Value2 as int) ROWS ONLY

					select count(1) 'TotalRecord'
		from AD_Clients cli
		INNER JOIN AD_Definations AS ad ON ad.DefinationId=cli.ClientTypeId
		left join AD_Clients as cl on cl.ClientId=cli.PClientId
					where cli.ClientName like '%'+@Value3+'%' 
				END
		END