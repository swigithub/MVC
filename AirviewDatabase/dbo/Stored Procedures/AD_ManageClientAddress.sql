
CREATE PROCEDURE [dbo].[AD_ManageClientAddress]

	@List List READONLY,
	@Filter varchar(max)='',
	@Address nvarchar(max)=NULL,	
	@Street nvarchar(max)=NULL,
	@CityId decimal=NULL,
	@StateId decimal=NULL,
	 @CountryId decimal=NULL,
	 @ZipCode nvarchar(max)=NULL,
	 @IsHeadOffice bit=0,
	 @ClientId decimal=NULL,
	 @IsActive  bit=0,
	 @AddressId decimal=NULL

AS
BEGIN
--select 0
IF @Filter='Insert'
	BEGIN
	insert into AD_ClientAddress(Address,Street,CityId,StateId,CountryId,ZipCode,IsHeadOffice,ClientId,IsActive)
    SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9 FROM @List l
	END
	ELSE IF @Filter='UpdateBulk'
	BEGIN
	declare @Id decimal=NULL
	set @Id =(SELECT TOP 1 l.Value8 FROM @List l)
	update AD_ClientAddress set IsActive=0 where ClientId=@Id
	 --DECLARE THE CURSOR FOR A QUERY.
		  DECLARE UpdateClient CURSOR READ_ONLY
		  FOR
		  SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10 FROM @List AS l 
		  --OPEN CURSOR.
		  OPEN UpdateClient 
		  --FETCH THE RECORD INTO THE VARIABLES.
		  FETCH NEXT FROM UpdateClient INTO @Address,@Street,@CityId,@StateId,@CountryId,@ZipCode,@IsHeadOffice,@ClientId,@IsActive,@AddressId
		  --LOOP UNTIL RECORDS ARE AVAILABLE.
		  WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF ISNULL((SELECT TOP 1 x.AddressId FROM AD_ClientAddress x WHERE x.AddressId=@AddressId and x.ClientId=@ClientId),0)=0 
				BEGIN
				print 'abc'
					insert into AD_ClientAddress([Address],Street,CityId,StateId,CountryId,ZipCode,IsHeadOffice,ClientId,IsActive)
					 values (@Address,@Street,@CityId,@StateId,@CountryId,@ZipCode,@IsHeadOffice,@ClientId,@IsActive)
			END 
				else 
				BEGIN
				print 'update'
				--//ClientId=@ClientId,
				update AD_ClientAddress set [Address]=@Address,Street=@Street,CityId=@CityId,StateId=@StateId,CountryId=@CountryId,ZipCode=@ZipCode,IsHeadOffice=@IsHeadOffice,IsActive=@IsActive
				where AddressId=@AddressId
				END
				 --FETCH THE NEXT RECORD INTO THE VARIABLES.
				 FETCH NEXT FROM UpdateClient INTO  @Address,@Street,@CityId,@StateId,@CountryId,@ZipCode,@IsHeadOffice,@ClientId,@IsActive,@AddressId
		  END
 
		  --CLOSE THE CURSOR.
		  CLOSE UpdateClient
		  DEALLOCATE UpdateClient

		--update AD_ClientContacts set ContactPerson=@ContactPerson , Designation=@Designation, Gender=@Gender,Title=@Title,ContactNo=@ContactNo,ContactType=@ContactType, IsPrimary=@IsPrimary,ClientId=@ClientId,UserId=@UserId,RegionId=@RegionId,CityId=@CityId,IsActive=@IsActive,ReportToId=@ReportToId where ContactId=@ContactId
		--set	@RETURN_VALUE=@ContactId
	END

END