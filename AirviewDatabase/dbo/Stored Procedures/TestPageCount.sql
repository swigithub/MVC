
Create procedure [dbo].[TestPageCount]
(
	 @PageIndex INT,  
	 @pageSize INT 
)
As  
 Begin  

	SELECT ap.*,cmp.ClientName 'Company',ac.ClientName 'Vendor',st.DefinationName 'Status' --, sc.DefinationName 'Scope'
		FROM AD_Projects AS ap
		INNER JOIN AD_Clients AS ac ON ac.ClientId=ap.VendorId
		INNER JOIN AD_Clients AS cmp ON cmp.ClientId=ap.CompanyId
		INNER JOIN AD_Definations AS st ON st.DefinationId=ap.StatusID
		ORDER BY ProjectID OFFSET @PageSize*(@PageIndex-1) ROWS FETCH NEXT @PageSize ROWS ONLY; 
		
	SELECT count(*) as totalCount FROM AD_Projects;

END