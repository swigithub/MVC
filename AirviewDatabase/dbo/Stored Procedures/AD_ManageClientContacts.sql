-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AD_ManageClientContacts]
	@Filter varchar(max),
	@ContactId decimal=NULL,
	@ContactPerson nvarchar(50)=NULL,
	@Designation  nvarchar(50)=NULL,	
	@Gender nvarchar(50)=NULL,
	@Title varchar(50)=NULL,
	@ContactNo numeric(18, 0)=null,
	@ContactType varchar(50)=null,
	@IsPrimary bit=0,
	@ClientId decimal=0,
	@UserId decimal=NULL,
	@RegionId decimal=NULL,
	@CityId decimal=NULL,
	@IsActive bit=0,
	@ReportToId decimal=NULL
	,@List List READONLY
AS
BEGIN
	 DECLARE @RETURN_VALUE int = 0 

	IF @Filter='Insert'
	BEGIN
		insert into AD_ClientContacts(ContactPerson,Designation,Gender,Title,ContactNo,ContactType,IsPrimary,ClientId,UserId,RegionId,CityId,IsActive,ReportToId) values (@ContactPerson,@Designation,@Gender,@Title,@ContactNo,@ContactType,@IsPrimary,@ClientId,@UserId,@RegionId,@CityId,@IsActive,@ReportToId)
		set @RETURN_VALUE = SCOPE_IDENTITY()	
	END
	else IF @Filter='InsertBulk'
	BEGIN
		insert into AD_ClientContacts(ContactPerson,Designation,Gender,Title,ContactNo,ContactType,IsPrimary,ClientId,UserId,RegionId,CityId,IsActive,ReportToId)
		SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13 FROM @List l
	END
	ELSE IF @Filter='Update'
	BEGIN
		update AD_ClientContacts set ContactPerson=@ContactPerson , Designation=@Designation, Gender=@Gender,Title=@Title,ContactNo=@ContactNo,ContactType=@ContactType, IsPrimary=@IsPrimary,ClientId=@ClientId,UserId=@UserId,RegionId=@RegionId,CityId=@CityId,IsActive=@IsActive,ReportToId=@ReportToId where ContactId=@ContactId
		set	@RETURN_VALUE=@ContactId
	END
	ELSE IF @Filter='UpdateBulk'
	BEGIN
	declare @Id decimal=NULL
	set @Id =(SELECT TOP 1 l.Value8 FROM @List l)
	update AD_ClientContacts set IsActive=0 where ClientId=@Id
	 --DECLARE THE CURSOR FOR A QUERY.
		  DECLARE UpdateClient CURSOR READ_ONLY
		  FOR
		  SELECT l.Value1,l.Value2,l.Value3,l.Value4,l.Value5,l.Value6,l.Value7,l.Value8,l.Value9,l.Value10,l.Value11,l.Value12,l.Value13,l.Value14 FROM @List AS l 
		  --OPEN CURSOR.
		  OPEN UpdateClient 
		  --FETCH THE RECORD INTO THE VARIABLES.
		  FETCH NEXT FROM UpdateClient INTO @ContactPerson,@Designation,@Gender,@Title,@ContactNo,@ContactType,@IsPrimary,@ClientId,@UserId,@RegionId,@CityId,@IsActive,@ReportToId,@ContactId
		  --LOOP UNTIL RECORDS ARE AVAILABLE.
		  WHILE @@FETCH_STATUS = 0
		  BEGIN
				IF ISNULL((SELECT TOP 1 x.ContactId FROM AD_ClientContacts x WHERE x.ContactId=@ContactId and x.ClientId=@ClientId),0)=0 
				BEGIN
				print 'abc'
					insert into AD_ClientContacts(ContactPerson,Designation,Gender,Title,ContactNo,ContactType,IsPrimary,ClientId,UserId,RegionId,CityId,IsActive,ReportToId) values (@ContactPerson,@Designation,@Gender,@Title,@ContactNo,@ContactType,@IsPrimary,@ClientId,@UserId,@RegionId,@CityId,@IsActive,@ReportToId)
			END 
				else 
				BEGIN
				print 'update'
				update AD_ClientContacts set ContactPerson=@ContactPerson , Designation=@Designation, Gender=@Gender,Title=@Title,ContactNo=@ContactNo,ContactType=@ContactType, IsPrimary=@IsPrimary,ClientId=@ClientId,UserId=@UserId,RegionId=@RegionId,CityId=@CityId,IsActive=@IsActive,ReportToId=@ReportToId where ContactId=@ContactId
				END
				 --FETCH THE NEXT RECORD INTO THE VARIABLES.
				 FETCH NEXT FROM UpdateClient INTO @ContactPerson,@Designation,@Gender,@Title,@ContactNo,@ContactType,@IsPrimary,@ClientId,@UserId,@RegionId,@CityId,@IsActive,@ReportToId,@ContactId
		  END
 
		  --CLOSE THE CURSOR.
		  CLOSE UpdateClient
		  DEALLOCATE UpdateClient

		--update AD_ClientContacts set ContactPerson=@ContactPerson , Designation=@Designation, Gender=@Gender,Title=@Title,ContactNo=@ContactNo,ContactType=@ContactType, IsPrimary=@IsPrimary,ClientId=@ClientId,UserId=@UserId,RegionId=@RegionId,CityId=@CityId,IsActive=@IsActive,ReportToId=@ReportToId where ContactId=@ContactId
		--set	@RETURN_VALUE=@ContactId
	END
	ELSE IF @Filter='Delete'
	BEGIN
		
		delete from AD_ClientContacts where ContactId=@ContactId
	END
	
	RETURN @RETURN_VALUE;
END