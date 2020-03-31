CREATE PROCEDURE TMP_GetNodes
	@Filter NVARCHAR(50),
	@Value NVARCHAR(50)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @Filter='ByTemplateId'
	BEGIN
		SELECT * FROM TMP_Nodes WHERE TemplateId = @Value AND IsActive = 'true' ORDER BY y_axis asc
	END


	IF @Filter='ByNodeId'
	BEGIN
		SELECT * FROM TMP_Nodes WHERE NodeId = @Value AND IsActive = 'true' 
	END


END