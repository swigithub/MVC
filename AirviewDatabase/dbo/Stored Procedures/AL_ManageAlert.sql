-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AL_ManageAlert
	-- Add the parameters for the stored procedure here
	@Filter varchar(50) = '',
	@KeyCode varchar(50) = '',
	@AlertId int = 0,
	@StatusId int = 0,
	@AlertSender int = 0,
	@EntityId int = 0,
	@ParentEntityId int = 0,
	@ChildEntityId int = 0,
	@Alert varchar(50) = '',
	
    @ConfigId varchar(50) = '',
	@Notification varchar(MAX) = '',

	-- User Subscription Model
	@AlertConfigId int = 0,

	@UserId int = 0,
	@AlertRecieverId int = 0,
	@RoleId int = 0,
	@IsSubscribed bit = false,
	@IsPushAlertRequired bit = true,
	@IsEmailAlertRequired bit = true,

	@IsEmailAlertSent bit = false,
	@IsPushAlertRead bit = false,
	@IsPushAlertSent bit = false

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--If @Filter = 'IsSubscribed'
	--Begin
	--	select ALS.IsSubscribed from AL_AlertSubscription ALS
	--	Inner Join AL_AlertConfig ALC
	--	On ALC.AlertConfigId = ALS.AlertConfigId
	--	Where
	--	ALC.IsEnabled = 1 and
	--	ALS.IsSubscribed = 1 and
	--	ALS.ParentEntityId = @ParentEntityId and
	--	ALS.ChildEntityId = @ChildEntityId and
	--	ALC.Name = @Alert
	--End
	--ELSE
	--If @Filter = 'Insert_Alert'
	--Begin
		
	--	Declare @TempUserId varchar(max)
	--	Declare @TempName varchar(50)
	--	Declare @TempBeforeStatausId int
	--	Declare @TempBeforeStataus varchar(max)
	--	Declare @TempAfterStataus varchar(max)
	--	Declare @TempNotification varchar(max)

	--	select @TempName = ProjectName, @TempBeforeStatausId = StatusId from PM_Projects
	--	where ProjectId = @EntityId-- and StatusId = @StatusId

	--	If @TempBeforeStatausId != @StatusId
	--	Begin
	--		select @TempBeforeStataus = (Select DefinationName from AD_Definations where DefinationId = @TempBeforeStatausId)
	--		select @TempAfterStataus = (Select DefinationName from AD_Definations where DefinationId = @StatusId)

	--		Select @TempNotification = 'Project "'+@TempName+'" Status Changed from "'+@TempBeforeStataus+'" to "'+@TempAfterStataus+'"'

	--		Select @TempUserId = (SELECT 
	--		STUFF((
	--		SELECT ', ' + cast(UserId as varchar(max) )
	--		FROM Sec_UserProjects 
	--		WHERE (ProjectId = a.ProjectId) 
	--		FOR XML PATH (''))
	--		,1,2,'') AS UserId
	--		FROM Sec_UserProjects a where ProjectId = @EntityId
	--		GROUP BY ProjectId)

	--		INSERT INTO AL_Alert (AlertTypeId, AlertCategoryId, EntityId, AlertReceiver, AlertSender, CreatedOn, Notification)
	--		select ALC.AlertTypeId, ALC.AlertCategoryId, @EntityId as EntityId, @TempUserId as AlertReceiver, ALS.UserId as AlertSender, GETDATE() as CreatedOn, @TempNotification as Notification from AL_AlertSubscription ALS
	--		Inner Join AL_AlertConfig ALC
	--		On ALC.AlertConfigId = ALS.AlertConfigId
	--		Where
	--		ALC.IsEnabled = 1 and
	--		ALS.IsSubscribed = 1 and
	--		ALS.ParentEntityId = @ParentEntityId and
	--		ALS.ChildEntityId = @ChildEntityId and
	--		ALC.Name = @Alert
	--	End

		

	--	--@TempNotification = 'Project "'+@TempName+'" Status Changed from "" to "'+@TempStataus+'"'
	--	--Select @TempUserId = (SELECT 
	--	--STUFF((
	--	--SELECT ', ' + cast(UserId as varchar(max) )
	--	--FROM Sec_UserProjects 
	--	--WHERE (ProjectId = a.ProjectId) 
	--	--FOR XML PATH (''))
	--	--,1,2,'') AS UserId
	--	--FROM Sec_UserProjects a where ProjectId = @EntityId
	--	--GROUP BY ProjectId)

	--	--INSERT INTO AL_Alert (AlertTypeId, AlertCategoryId, EntityId, AlertReceiver, AlertSender, CreatedOn)
	--	--select ALC.AlertTypeId, ALC.AlertCategoryId, @EntityId as EntityId, @TempUserId as AlertReceiver, ALS.UserId as AlertSender, GETDATE() as CreatedOn from AL_AlertSubscription ALS
	--	--Inner Join AL_AlertConfig ALC
	--	--On ALC.AlertConfigId = ALS.AlertConfigId
	--	--Where
	--	--ALC.IsEnabled = 1 and
	--	--ALS.IsSubscribed = 1 and
	--	--ALS.ParentEntityId = @ParentEntityId and
	--	--ALS.ChildEntityId = @ChildEntityId and
	--	--ALC.Name = @Alert
	--End
	-- Story #1698 Update Alert Configurations with Subscription
	IF @Filter = 'Get_Alert_Role_Configurations'
	BEGIN
		If @KeyCode = ''
		Begin
			--select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.Name as AlertCategoryName, ALCon.Name, ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired,
			--ALS.IsEmailAlertRequired, ALC.ParentId, ALS.RoleId from AL_AlertCategory ALC
			--Inner Join AL_AlertConfig ALCon
			--On ALCon.AlertCategoryId = ALC.AlertCategoryId
			--left Join AL_AlertRoleSubscription ALS 
			--On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.RoleId = @RoleId
			--where ALCon.IsEnabled = 1
			WITH AlertCat_Cte
				AS
				(
						Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
						from AL_AlertCategory
						Where AL_AlertCategory.IsEnabled = 1
						/*
						  Get all the children 
						*/
						UNION ALL
						SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
						FROM  AL_AlertCategory d
						JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
				) --select * from AlertCat_Cte


				Select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.AlertCategoryName, ALCon.Name,
				ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired, ALS.IsEmailAlertRequired,
				ALC.ParentId, ALS.RoleId 
				from AlertCat_Cte ALC
				Inner Join AL_AlertConfig ALCon
				On ALCon.AlertCategoryId = ALC.AlertCategoryId
				left Join AL_AlertRoleSubscription ALS
				On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.RoleId = @RoleId
				where ALCon.IsEnabled = 1

				union
				Select 0'AlertConfigId', AlertCategoryId, AlertCategoryName, '' as Name, '' as Description, 0'IsSubscribed',0'IsPushAlertRequired', 0'IsEmailAlertRequired',ParentId, 0'RoleId' from AlertCat_Cte where ParentId = 0;
		END
		ELSE
		-- Story 1697
		Begin
			WITH AlertCat_Cte
				AS
				(
						Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
						from AL_AlertCategory
						Where AL_AlertCategory.KeyCode = @KeyCode and AL_AlertCategory.IsEnabled = 1
						/*
						  Get all the children 
						*/
						UNION ALL
						SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
						FROM  AL_AlertCategory d
						JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
				)
				Select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.AlertCategoryName, ALCon.Name,
				ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired, ALS.IsEmailAlertRequired,
				ALC.ParentId, ALS.RoleId 
				from AlertCat_Cte ALC
				Inner Join AL_AlertConfig ALCon
				On ALCon.AlertCategoryId = ALC.AlertCategoryId
				left Join AL_AlertRoleSubscription ALS
				On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.RoleId = @RoleId
				where ALCon.IsEnabled = 1
		END
	END
	ELSE
	IF @Filter = 'Update_Alert_Role_Configurations'
	BEGIN
		IF EXISTS(select * from AL_AlertRoleSubscription where RoleId = @RoleId and AlertConfigId = @AlertConfigId)
		   update AL_AlertRoleSubscription set IsSubscribed = @IsSubscribed, IsPushAlertRequired = @IsPushAlertRequired, IsEmailAlertRequired = @IsEmailAlertRequired, ModifiedBY = @UserId, ModifiedOn = GETDATE()
		   where RoleId = @RoleId and AlertConfigId = @AlertConfigId
		ELSE
		   insert into AL_AlertRoleSubscription(AlertConfigId, RoleId, IsSubscribed, IsPushAlertRequired, IsEmailAlertRequired, ModifiedBY, ModifiedOn)
		   values(@AlertConfigId, @RoleId, @IsSubscribed, @IsPushAlertRequired, @IsEmailAlertRequired, @UserId, GETDATE());
	END
	ELSE 
	IF @Filter = 'Update_Alert_Configurations'
	BEGIN
		Update AL_AlertSubscription
		Set 
		IsSubscribed = @IsSubscribed,
		IsPushAlertRequired = @IsPushAlertRequired,
		IsEmailAlertRequired = @IsEmailAlertRequired,
		ModifiedOn = GETDATE(),
		ModifiedBy = @UserId
		Where AlertConfigId = @AlertConfigId 
	END
	ELSE
	-- Story 1696 Get All Alert Configurations with Subs OR Story 1697 Get All Alert Configurations with Subs By Alert Category
	IF @Filter = 'Get_Alert_Configurations' 
	BEGIN
		-- Story 1696
		If @KeyCode = ''
		Begin
			select ALCon.AlertConfigId, ALC.Name as AlertCategoryName, ALCon.Name, ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired,
			ALS.IsEmailAlertRequired, ALC.ParentId  from AL_AlertCategory ALC
			Inner Join AL_AlertConfig ALCon
			On ALCon.AlertCategoryId = ALC.AlertCategoryId
			left Join AL_AlertSubscription ALS
			On ALS.AlertConfigId = ALCon.AlertConfigId
			where ALCon.IsEnabled = 1
		END
		ELSE
		-- Story 1697
		Begin
			WITH AlertCat_Cte
				AS
				(
						Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
						from AL_AlertCategory
						Where AL_AlertCategory.KeyCode = @KeyCode and AL_AlertCategory.IsEnabled = 1
						/*
						  Get all the children 
						*/
						UNION ALL
						SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
						FROM  AL_AlertCategory d
						JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
				)
				Select ALCon.AlertConfigId, ALC.AlertCategoryName, ALCon.Name, ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired, ALS.IsEmailAlertRequired,
				ALC.ParentId
				from AlertCat_Cte ALC
				Inner Join AL_AlertConfig ALCon
				On ALCon.AlertCategoryId = ALC.AlertCategoryId
				left Join AL_AlertSubscription ALS
				On ALS.AlertConfigId = ALCon.AlertConfigId
				where ALCon.IsEnabled = 1
		END
	END
	ELSE
	-- Story 1649
	IF @Filter = 'Get_User_Alert_Configurations'
	BEGIN
		If @KeyCode = ''
		Begin
			--select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.Name as AlertCategoryName, ALCon.Name, ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired,
			--ALS.IsEmailAlertRequired, ALC.ParentId, ALS.UserId  from AL_AlertCategory ALC
			--Inner Join AL_AlertConfig ALCon
			--On ALCon.AlertCategoryId = ALC.AlertCategoryId
			--left Join AL_AlertUserSubscription ALS
			--On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.UserId = @UserId
			--where ALCon.IsEnabled = 1

			--WITH AlertCat_Cte
			--	AS
			--	(
			--			Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
			--			from AL_AlertCategory
			--			Where AL_AlertCategory.IsEnabled = 1
			--			/*
			--			  Get all the children 
			--			*/
			--			UNION ALL
			--			SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
			--			FROM  AL_AlertCategory d
			--			JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
			--	) --select * from AlertCat_Cte


			--	Select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.AlertCategoryName, ALCon.Name,
			--	ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired, ALS.IsEmailAlertRequired,
			--	ALC.ParentId, ALS.UserId 
			--	from AlertCat_Cte ALC
			--	Inner Join AL_AlertConfig ALCon
			--	On ALCon.AlertCategoryId = ALC.AlertCategoryId
			--	left Join AL_AlertUserSubscription ALS
			--	On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.UserId = @UserId
			--	where ALCon.IsEnabled = 1

			--	union
			--	Select 0'AlertConfigId', AlertCategoryId, AlertCategoryName, '' as Name, '' as Description, 0'IsSubscribed',0'IsPushAlertRequired', 0'IsEmailAlertRequired',ParentId, 0'RoleId' from AlertCat_Cte where ParentId = 0;

			----
				select @RoleId = RoleId from Sec_UserRoles where UserId = @UserId;
				WITH AlertCat_Cte
				AS
				(
						Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
						from AL_AlertCategory
						Where AL_AlertCategory.IsEnabled = 1
						/*
						  Get all the children 
						*/
						UNION ALL
						SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
						FROM  AL_AlertCategory d
						JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
				) --select * from AlertCat_Cte


				Select ALCon.AlertConfigId, ALC.AlertCategoryId, ALC.AlertCategoryName, ALCon.Name,
				ALCon.Description, ALUS.IsSubscribed, ALUS.IsPushAlertRequired, ALUS.IsEmailAlertRequired,
				ALC.ParentId, ALS.RoleId, ALUS.UserId 
				from AlertCat_Cte ALC
				Inner Join AL_AlertConfig ALCon
				On ALCon.AlertCategoryId = ALC.AlertCategoryId
				left Join AL_AlertRoleSubscription ALS
				On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.RoleId = @RoleId and ALS.IsSubscribed = 1
				left Join AL_AlertUserSubscription ALUS
				On ALUS.AlertConfigId = ALS.AlertConfigId and ALUS.UserId = @UserId
				where ALCon.IsEnabled = 1
				union
				Select 0'AlertConfigId', AlertCategoryId, AlertCategoryName, '' as Name, '' as Description, 0'IsSubscribed',0'IsPushAlertRequired', 0'IsEmailAlertRequired',ParentId, 0'RoleId', 0'UserId' from AlertCat_Cte where ParentId = 0;
		END
		ELSE
		Begin
			WITH AlertCat_Cte
				AS
				(
						Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode, AL_AlertCategory.Name as AlertCategoryName, AL_AlertCategory.ParentId
						from AL_AlertCategory
						Where AL_AlertCategory.KeyCode = @KeyCode and AL_AlertCategory.IsEnabled = 1
						/*
						  Get all the children 
						*/
						UNION ALL
						SELECT d.AlertCategoryId , d.KeyCode, d.Name as CategoryName, d.ParentId
						FROM  AL_AlertCategory d
						JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId and d.IsEnabled = 1
		   
				)
				Select ALCon.AlertConfigId, ALC.AlertCategoryName, ALCon.Name, ALCon.Description, ALS.IsSubscribed, ALS.IsPushAlertRequired, ALS.IsEmailAlertRequired,
				ALC.ParentId, ALS.UserId
				from AlertCat_Cte ALC
				Inner Join AL_AlertConfig ALCon
				On ALCon.AlertCategoryId = ALC.AlertCategoryId
				left Join AL_AlertUserSubscription ALS
				On ALS.AlertConfigId = ALCon.AlertConfigId and ALS.UserId = @UserId
				where ALCon.IsEnabled = 1
		END
	END
	ELSE
	IF @Filter = 'GET_Configurations_List'
	BEGIN
	select ALCon.AlertConfigId, ALC.Name as AlertCategoryName, ALCon.Name, ALCon.Description, ALUS.IsSubscribed, ALUS.IsPushAlertRequired, ALUS.IsEmailAlertRequired  from AL_AlertCategory ALC
	Inner Join AL_AlertConfig ALCon
	On ALCon.AlertCategoryId = ALC.AlertCategoryId
	left Join AL_AlertUserSubscription ALUS
	On ALUS.AlertConfigId = ALCon.AlertConfigId
	where ALCon.IsEnabled = 1
	END
	ELSE
	IF @Filter = 'Update_User_Alert_Configurations'
	BEGIN
		IF EXISTS(select * from AL_AlertUserSubscription where UserId = @UserId and AlertConfigId = @AlertConfigId)
		   update AL_AlertUserSubscription set IsSubscribed = @IsSubscribed, IsPushAlertRequired = @IsPushAlertRequired, IsEmailAlertRequired = @IsEmailAlertRequired where UserId = @UserId and AlertConfigId = @AlertConfigId
		ELSE
		   insert into AL_AlertUserSubscription(AlertConfigId, UserId, IsSubscribed, IsPushAlertRequired, IsEmailAlertRequired)
		   values(@AlertConfigId, @UserId, @IsSubscribed, @IsPushAlertRequired, @IsEmailAlertRequired);
	END
	--ELSE
	--IF @Filter = 'Subscribe_Insert'
	--BEGIN
		-- Get Category by KeyCode & ConfigId
		--WITH AlertCat_Cte
		--AS
		--(
		--   /*
		--	  Root Node Anchor
		--   */
		--   Select AL_AlertCategory.AlertCategoryId, AL_AlertCategory.KeyCode
		--   from AL_AlertCategory
		--   Where AL_AlertCategory.ParentId = 0 and AL_AlertCategory.KeyCode = @KeyCode
		--   /*
		--	  Get all the children 
		--   */
		--   UNION ALL
		--   SELECT d.AlertCategoryId , d.KeyCode
		--   FROM  AL_AlertCategory d
		--	  JOIN AlertCat_Cte p on d.ParentId=p.AlertCategoryId
		--)

		--INSERT INTO AL_AlertSubscription(AlertConfigId, EntityId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired)
		--Select ALCon.AlertConfigId, @EntityId, @IsSubscribed, @IsPushAlertRequired, @IsEmailAlertRequired
		--from AlertCat_Cte
		--Inner Join AL_AlertConfig ALCon
		--On ALCon.AlertCategoryId = AlertCat_Cte.AlertCategoryId
		--WHERE ALCon.AlertConfigId IN (SELECT * FROM dbo.CSVToTable(@ConfigId));

		--INSERT INTO AL_AlertSubscriptionUser(AlertSubsId, UserId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired)
		--Select AlertSubId,@UserId, @IsSubscribed,@IsPushAlertRequired,@IsEmailAlertRequired from AL_AlertSubscription;

		--exec AL_ManageAlert @Filter=N'Subscribe_Insert',@KeyCode=N'PM',@EntityId=1,@ConfigId=N'1,4,',@UserId=1
		--INSERT INTO AL_AlertSubscription(AlertConfigId, EntityId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired) VALUES (@AlertConfigId, @EntityId, @IsSubscribed, @IsPushAlertRequired, @IsEmailAlertRequired);
		--Select * from AL_AlertSubscription
		
		--VALUES (IDENT_CURRENT('AL_AlertSubscription'), @UserId, @IsSubscribed,@IsPushAlertRequired,@IsEmailAlertRequired);
		--Select * from AL_AlertSubscription ALS
		--inner join AL_AlertSubscriptionUser ALSU
		--on ALS.AlertSubId = ALSU.AlertSubsId
		--where ALS.EntityId = 1 and ALS.AlertConfigId in ('4') and ALSU.UserId = 1
		--Select * from AL_AlertSubscription
		--Select * from AL_AlertSubscriptionUser

		--truncate table AL_AlertSubscription
		--truncate table AL_AlertSubscriptionUser

		--Select * from AL_AlertCategory ALCat
		--Inner Join AL_AlertConfig ALCon
		--ON ALCat.AlertCategoryId = ALCon.AlertCategoryId
		--where ALCat.KeyCode = @KeyCode and ALCon.AlertConfigId IN (SELECT * FROM dbo.CSVToTable(@ConfigId))

		----select * from AL_AlertConfig where AlertConfigId in (@ConfigId)

		--SELECT * FROM AL_AlertConfig WHERE AlertConfigId IN (SELECT * FROM dbo.CSVToTable(@ConfigId))


		--SELECT * FROM AL_AlertSubscriptionUser



		--INSERT INTO AL_AlertSubscription(AlertConfigId, EntityId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired) VALUES (AlertConfigId, EntityId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired);
		--INSERT INTO AL_AlertSubscriptionUser(AlertSubsId, UserId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired) VALUES (AlertSubsId, UserId, IsSubscribed,IsPushAlertRequired,IsEmailAlertRequired);
	--END

	ELSE
	IF @Filter = 'NotificationBell'
	BEGIN
		Select top 10
		AAL.CreatedOn,
		AAL.AlertConfigId,
		ALLC.AlertCategoryId,
		ALLCat.ParentId,
		(Select Name from AL_AlertCategory where AlertCategoryId = ALLCat.ParentId) as ParentName,
		AAL.Notification,
		AALR.AlertId,
		AALR.UserId,
		AALR.IsPushAlertRead,
		SU.FirstName +' '+ SU.LastName as UserName,
		SU.Picture
		from AL_Alert AAL

		Inner Join AL_AlertReciever AALR
		ON AALR.AlertId = AAL.AlertId
		
		Inner Join Sec_Users SU
		On SU.UserId = AALR.UserId
		
		Inner Join AL_AlertConfig ALLC
		On ALLC.AlertConfigId = AAL.AlertConfigId

		Inner Join AL_AlertCategory ALLCat
		On ALLCat.AlertCategoryId = ALLC.AlertCategoryId

		Where AlertRecieverId = @AlertRecieverId Order by CreatedOn desc 
	END
	ELSE
	IF @Filter = 'InsertNotification'
	BEGIN
		--Select @IsSubscribed = IsSubscribed from AL_AlertUserSubscription where AlertConfigId = @AlertConfigId and UserId = @AlertRecieverId;
		--IF @IsSubscribed  = 1
		--BEGIN

			Insert into AL_Alert (AlertConfigId, EntityId, CreatedOn, Notification) values (@AlertConfigId,@EntityId,GETDATE(), @Notification);

			-- Insert Only those users as Receiver which have permissions 
			Insert into AL_AlertReciever (AlertRecieverId,AlertId, UserId, IsPushAlertSent, IsPushAlertRead, IsEmailAlertSent) -- values (@AlertRecieverId,IDENT_CURRENT( 'AL_Alert' ),@UserId, @IsPushAlertSent,@IsPushAlertRead,@IsEmailAlertSent);
			select distinct SU.UserId, IDENT_CURRENT( 'AL_Alert' ), @UserId, @IsPushAlertSent, @IsPushAlertRead, @IsEmailAlertSent from Sec_Users SU
			Inner Join AL_AlertUserSubscription ALUS
			On ALUS.UserId = SU.UserId AND ALUS.IsSubscribed = 1
			Inner Join AL_AlertRoleSubscription ALRS
			On ALRS.AlertConfigId = ALUS.AlertConfigId AND ALRS.IsSubscribed = 1 AND ALRS.RoleId = (select RoleId from Sec_UserRoles where UserId = SU.UserId)
			where SU.UserId!=@UserId;

			select * from AL_AlertReciever where AlertId = IDENT_CURRENT( 'AL_Alert' );

		--END
	END
	ELSE
	IF @Filter = 'NotificationBellUpdate'
	BEGIN
		Update AL_AlertReciever
		Set IsPushAlertRead = @IsPushAlertRead
		Where AlertRecieverId = @AlertRecieverId and AlertId = @AlertId;
	END
END