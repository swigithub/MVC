-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- [dbo].[AV_rptSubmittedOnWOStatus]  'SubmittedOn_WO_Status','s.SubmittedOn between 2017-01-01 and 2017-02-02'
CREATE PROCEDURE [dbo].[AV_rptSubmittedOnWOStatus] 
@Filter varchar(200),
@Where nvarchar(500)
As
IF @Filter='SubmittedOn_WO_Status'
BEGIN
DECLARE @sql as nvarchar(max)=('
select x.Market,x.Date,x.TotalSites
from
(
	select d.DefinationName as Market,CAST(s.SubmittedOn as date) AS Date,COUNT(s.SiteId) AS TotalSites 
	from dbo.AV_Sites as s
	inner join dbo.AD_Definations as d on s.CityId=d.DefinationId
	where s.IsActive=1 and '+@Where+'
	group by d.DefinationName,CAST(s.SubmittedOn as date)
) x
order by x.Market,x.Date
')
EXEC(@sql)
END