﻿CREATE FUNCTION dbo.PM_UpdateReports(@ProjectId AS NUMERIC(18,0), @ProjectSiteId AS NUMERIC(18,0), @TaskId AS NUMERIC(18,0), @ReportDate AS DATETIME)

RETURNS BIT
BEGIN
	
		RETURN CAST(1 AS BIT);	
END