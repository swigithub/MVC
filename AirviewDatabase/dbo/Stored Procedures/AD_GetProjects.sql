--exec AD_GetProjects 'ById', 1

CREATE PROCEDURE AD_GetProjects
	@Filter NVARCHAR(50),
	@value  NVARCHAR(50)=NULL,
	@value1 NVARCHAR(50)=NULL,
	@value2 NVARCHAR(50)=NULL,
	@value3 NVARCHAR(50)=NULL,
	@value4 NVARCHAR(50)=NULL,
	@value5 NVARCHAR(50)=NULL,

	@pageIndex INT = 0,  
	@pageSize INT = 0 

AS
BEGIN
		
--  [dbo].[AD_GetProjects] 'All'		
  if @Filter='All'
	BEGIN 		
		declare @Count int=(SELECT count(*) as totalCount FROM AD_Projects AS ap
														INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
														INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
														INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
														WHERE (ap.isActive=1 OR ap.isActive=0) AND 
															ap.ProjectID IN (SELECT * FROM dbo.ConvertCSVToTable(@value))  OR
															ap.CompanyID IN (SELECT * FROM dbo.ConvertCSVToTable(@value1)) OR
															ap.VendorID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value2)) OR
															ap.StatusID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value3)) OR					
															(
																ap.StartDate  >=CAST(@value4 as datetime) AND ap.StartDate  <=CAST(@value5 as datetime)
															))
				
		SELECT  ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status',@Count 'totalCount' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		WHERE (ap.isActive=1 OR ap.isActive=0) AND 
		
			ap.ProjectID IN (SELECT * FROM dbo.ConvertCSVToTable(@value))  OR
			ap.CompanyID IN (SELECT * FROM dbo.ConvertCSVToTable(@value1)) OR
			ap.VendorID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value2)) OR
			ap.StatusID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value3)) OR					
			(
				ap.StartDate  >=CAST(@value4 as datetime) AND ap.StartDate  <=CAST(@value5 as datetime)
			)

		--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId WHERE ap.isActive=1
		
		ORDER BY ap.ProjectID OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY;  
	END

	ELSE if @Filter='ProjectsName'
	BEGIN 		
						
		SELECT ap.ProjectID,ap.ProjectName
		FROM AD_Projects AS ap
		inner join Sec_UserProjects u on u.ProjectId=ap.ProjectID
		where u.UserId=CONVERT(numeric(20,0),@value)

	END

	Else if	@Filter='SearchAll'
	BEGIN

	    SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		WHERE ap.isActive=1 and 
		
			ap.ProjectID IN (SELECT * FROM dbo.ConvertCSVToTable(@value))  OR
			ap.CompanyID IN (SELECT * FROM dbo.ConvertCSVToTable(@value1)) OR
			ap.VendorID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value2)) OR
			ap.StatusID  IN (SELECT * FROM dbo.ConvertCSVToTable(@value3))
			OR					
			(
				ap.StartDate  >=CAST(@value4 as datetime) AND ap.StartDate  <=CAST(@value5 as datetime)
			)
			
			
	--SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
	--	FROM AD_Projects AS ap
	--	INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
	--	INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
	--	INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
	--	--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId
	--	WHERE  ap.isActive=1
	--	 AND (
	--		 Charindex(cast(ap.ProjectID as varchar(max))+',', @value)  > 0 OR  
	--		 Charindex(cast(ap.CompanyID as varchar(max))+',', @value1) > 0	OR  
	--		 Charindex(cast(ap.VendorID as varchar(max))+',',  @value2) > 0	OR  
	--		 Charindex(cast(ap.StatusID as varchar(max))+',',  @value3) > 0
	--		)		
	END

	ELSE IF	@Filter='ById'
	BEGIN

	SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId
		WHERE ProjectID=@value and ap.isActive=1
	END

	ELSE IF @Filter='ScopeByProject'
	BEGIN
	SELECT ap.ProjectId,ap.ProjectName,sc.DefinationName 'Scope' 
		FROM AD_Projects ap		
		INNER JOIN AD_ProjectConfigurations pc ON pc.ProjectID= ap.ProjectID
		INNER JOIN AD_Definations AS sc ON sc.DefinationId=pc.TypeValue 
		Inner join AD_DefinationTypes  AS dt ON  dt.DefinationTypeId=pc.TypeId
		where ap.projectid=@value and pc.TypeId =12
	END

	ELSE IF @Filter='MarketByProject'
	BEGIN
	SELECT ap.ProjectId,ap.ProjectName,sc.DefinationName 'Market' 
		FROM AD_Projects ap		
		INNER JOIN AD_ProjectConfigurations pc ON pc.ProjectID= ap.ProjectID
		INNER JOIN AD_Definations AS sc ON sc.DefinationId=pc.TypeValue 
		Inner join AD_DefinationTypes  AS dt ON  dt.DefinationTypeId=pc.TypeId
		where ap.projectid=@value and pc.TypeId =7
	END
	
	ELSE IF	@Filter='ProjectName'
	BEGIN

	SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId
		WHERE ap.ProjectName Like '%'+ @value+ '%' 

	END

	ELSE IF	@Filter='VendorName'
	BEGIN
	SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId
		WHERE ac.ClientName Like '%'+ @value+ '%' 
	END

		ELSE IF	@Filter='Status'
	BEGIN
	SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		--INNER JOIN AD_Definations AS sc ON sc.DefinationId=ap.ProjectTypeId
		WHERE st.DefinationName Like '%'+ @value+ '%' 
	END
	 
	
END