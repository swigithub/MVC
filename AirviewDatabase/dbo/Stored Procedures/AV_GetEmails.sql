
CREATE PROCEDURE [dbo].[AV_GetEmails]
 @Filter nvarchar(50)
,@Value nvarchar(50)
AS
BEGIN
	
	if @Filter='MarketPOC'
	begin
		select url.UserId, usr.Email
		from Sec_UserCities cty
		inner join Sec_UserRoles url on cty.UserId=url.UserId
		inner join Sec_Users usr on usr.UserId=url.UserId
		WHERE cty.CityId=(SELECT sit.CityId FROM AV_Sites sit where sit.SiteId=@Value)
		and url.RoleId=10018
	end
END