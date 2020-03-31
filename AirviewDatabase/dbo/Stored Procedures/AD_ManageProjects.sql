
--Select *from AD_Projects


CREATE PROCEDURE [dbo].[AD_ManageProjects]
	 @Filter varchar(max)
	,@ProjectID INT=NULL
	,@ProjectName nvarchar(50)=NULL
	,@ProjectScopeID nvarchar(100)=NULL
	,@CompanyID int=NULL
    ,@VendorID int=NULL
    ,@StartDate datetime=NULL
    ,@EndDate datetime=NULL
    ,@StatusID int=NULL
    ,@Color nvarchar(20)=NULL
	,@Description nvarchar(256)=NULL	
	,@IsActive BIT=NULL

AS
BEGIN
    DECLARE @RETURN_VALUE int = 0 

	IF @Filter='Insert'
	BEGIN
		insert into AD_Projects(
				ProjectName
			   ,ProjectScopeID
			   ,CompanyID
			   ,VendorID
			   ,StartDate
			   ,EndDate
			   ,StatusID
			   ,Color
			   ,Description
		   ) 
		   values (
				@ProjectName
			   ,@ProjectScopeID
			   ,@CompanyID
			   ,@VendorID
			   ,@StartDate
			   ,@EndDate
			   ,@StatusID
			   ,@Color
			   ,@Description)

		set @RETURN_VALUE = SCOPE_IDENTITY()
	
	END
	
	ELSE IF @Filter='Update'
	BEGIN
		update AD_Projects set 
		ProjectName=@ProjectName , 
		ProjectScopeID = @ProjectScopeID,
		CompanyID=@CompanyID, 
		VendorID=@VendorID, 
		StartDate=@StartDate, 
		EndDate=@EndDate, 
		StatusID=@StatusID, 
		Color=@Color, 
		Description=@Description
		where ProjectID=@ProjectID

		set	@RETURN_VALUE=@ProjectID
	END

	ELSE IF @Filter='Delete'
	BEGIN
		delete from AD_Projects where ProjectID=@ProjectID

	END
	

	else IF @Filter='UpdateStatus'
	BEGIN
	   update AD_Projects set IsActive=@IsActive where ProjectID=@ProjectID
	END


	RETURN @RETURN_VALUE;
END