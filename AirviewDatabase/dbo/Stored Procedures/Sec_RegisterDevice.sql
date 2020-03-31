CREATE PROCEDURE [dbo].[Sec_RegisterDevice]
	@Fullname AS NVARCHAR(150),
	@Username AS NVARCHAR(50),
	@Password AS NVARCHAR(50),
	@Email AS NVARCHAR(50),
	@IMEI AS NVARCHAR(50),
	@Manufacturer AS NVARCHAR(50),
	@Model AS NVARCHAR(50),
	@MAC AS NVARCHAR(50)
AS	
	IF EXISTS(select UserName from Sec_Users where UserName=@Username AND [Password]=@Password AND Email=@Email)
	BEGIN
		IF EXISTS(select x.DeviceId from Sec_UserDevices x where x.IMEI=@IMEI)
		BEGIN
			RETURN -1; --Device already registered
		END
		ELSE
		BEGIN
			DECLARE @UserId AS INT	
	
			SELECT @UserId=su.UserId
			FROM Sec_Users AS su
			WHERE UserName=@UserName AND [Password]=@Password AND Email=@Email;
	
			INSERT INTO Sec_UserDevices(UserId,IMEI,MAC,Manufacturer,Model)
			VALUES(@UserId,@IMEI,@MAC,@Manufacturer,@Model);
		END
	END
	ELSE
	BEGIN
		RETURN -2; --Invalid User
	END