-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PM_GetProjectResources]
	@Filter NVARCHAR(50)
	,@Value1 NVARCHAR(50)
AS
BEGIN
	IF @Filter = 'byTaskId'
	BEGIN
	    SELECT pr.*
	    FROM   PM_ProjectResources AS pr
	    INNER JOIN Sec_Users AS u ON u.UserId=pr.AssignToId
	    WHERE pr.TaskId=@Value1
	END
	
	
	else IF @Filter = 'byProjectId'
	BEGIN
	    SELECT pr.*
	    FROM   PM_ProjectResources AS pr
	    INNER JOIN Sec_Users AS u ON u.UserId=pr.AssignToId
	    WHERE pr.ProjectId=@Value1
	END
END