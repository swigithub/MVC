create PROCEDURE AD_GetIssue   
    @Filter nvarchar(50),
	@IssueTitle nvarchar(150) = null,
	@IssueDescription nvarchar(max) = null,
	@IssueId int =null,
	@IssueFeature nvarchar(50) = null, 
	@IssueStatus nvarchar(50) =null
AS
BEGIN	
	if @Filter='CreateIssue'
	BEGIN 
		Insert Into AD_Issue(IssueTitle, IssueDescription, IssueFeature, IssueStatus) values (@IssueTitle, @IssueDescription, @IssueFeature, @IssueStatus);
	End

	if @Filter='GetEditIssue'
	BEGIN 
		Select * from AD_Issue Where IssueId = @IssueId
	End

	if @Filter='SetEditIssue'
	BEGIN 
		Select * from AD_Issue Where IssueId = @IssueId
		Update AD_Issue
		Set IssueTitle = @IssueTitle,
		IssueDescription = @IssueDescription,
		IssueFeature = @IssueFeature,
		IssueStatus = @IssueStatus

		Where IssueId = @IssueId
	End
End