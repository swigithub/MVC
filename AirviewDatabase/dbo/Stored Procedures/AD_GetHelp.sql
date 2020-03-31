create PROCEDURE AD_GetHelp   
    @Filter nvarchar(50),
	@Title nvarchar(150) = null,
	@Description nvarchar(max) = null,
	@ComponentID int = 0,
	@ModuleID int = 0,
	@HelpID int = 0,
	@FeatureID int = null, 
	@IsActive bit =null
AS
BEGIN	
	if @Filter='Get_Permissions'
	BEGIN 
		SELECT  Sec_Permissions.id as FeatureID,
		Sec_Permissions.ParentId,
		Sec_Permissions.Title,
		Sec_Permissions.URL,
		AD_Help.FeatureID,
		AD_Help.ModuleID
		FROM    Sec_Permissions
				Left JOIN AD_Help
					ON Sec_Permissions.id IN (AD_Help.FeatureID)
		WHERE  AD_Help.FeatureID IS NULL;
	End
	Else
	if @Filter='Get_Module_list'
	BEGIN 
		/*SELECT  Sec_Permissions.id as ModuleId,
		Sec_Permissions.Title as ModuleName
		FROM    Sec_Permissions
				
		WHERE  Sec_Permissions.ModuleId = @ModuleID and Sec_Permissions.IsModule = 1 */

		WITH Module_CTE (ModuleId,ModuleName, ParentId)
		AS                              
		(                               
			SELECT
			Sec_Permissions.id as ModuleId,
			Sec_Permissions.Title as ModuleName,
			Sec_Permissions.ParentId
			FROM Sec_Permissions             
			WHERE Sec_Permissions.ModuleId = @ModuleID and Sec_Permissions.IsModule = 1 and Sec_Permissions.ParentId = 0
	       
			UNION ALL                    
    
			SELECT
			A.id as ModuleId,
			A.Title as ModuleName,
			A.ParentId 
			FROM Sec_Permissions A, Module_CTE B
			WHERE A.ParentId = B.ModuleId
		)                               
		SELECT ModuleId,ModuleName,ParentId 
		FROM Module_CTE
	End
	Else
	if @Filter='Get_Feature_list'
	BEGIN 
		SELECT  Sec_Permissions.id as FeatureId,
		Sec_Permissions.ParentId,
		Sec_Permissions.ModuleId,
		Sec_Permissions.Title as FeatureName,
		Sec_Permissions.URL,
		AD_Help.FeatureID,
		AD_Help.ModuleID
		FROM    Sec_Permissions
				Left JOIN AD_Help
					ON Sec_Permissions.id IN (AD_Help.FeatureID)
		WHERE  AD_Help.FeatureID IS NULL AND Sec_Permissions.ModuleId = @ComponentID AND Sec_Permissions.ParentId = @ModuleID and Sec_Permissions.isUsed = 1
	End
	Else
	if @Filter='Get_HelpPermissions'
		BEGIN 
			SELECT  Sec_Permissions.id ,
			Sec_Permissions.Title,
			Sec_Permissions.URL,
			AD_Help.FeatureID,
			AD_Help.ModuleID
			FROM    Sec_Permissions
					LEFT JOIN AD_Help
						ON Sec_Permissions.id IN (AD_Help.ModuleID)
			WHERE  AD_Help.ModuleID IS NOT NULL;
		End
	ELSE if @Filter='Get_Component_list'
	BEGIN 
	Select Distinct ad.definationname As ComponentName, ad.DefinationId AS ComponentId FROM Sec_Permissions AS sp INNER JOIN AD_Definations AS ad ON sp.ModuleId=ad.DefinationId
	End
	ELSE if @Filter='Create_Help'
	BEGIN 
	Insert Into AD_Help (ComponentId, ModuleId, FeatureId, Title, Description, IsActive) values (@ComponentID, @ModuleID, @FeatureID, @Title, @Description, @IsActive);
	End
	ELSE if @Filter='Read_Help'
	BEGIN 
	Select
	AD_Help.HelpId,
	AD_Definations.DefinationId,
	AD_Definations.DefinationName as ComponentName,
	Sec_Permissions.ParentId as ModuleId,
	ModuleName  = (SELECT Title FROM Sec_Permissions WHERE Id IN (AD_Help.ModuleId)),
	Sec_Permissions.Id as FeatureId,
	Sec_Permissions.Title as FeatureName,
	AD_Help.Title,
	AD_Help.Description,
	AD_Help.IsActive
	from AD_Definations
	Inner Join Sec_Permissions
	On AD_Definations.DefinationId = Sec_Permissions.ModuleId
	LEFT JOIN AD_Help
	ON Sec_Permissions.id IN (AD_Help.FeatureID)
	where AD_Definations.DefinationId IN (AD_Help.ComponentId)
	AND AD_Help.HelpId = @FeatureID
	End
	ELSE if @Filter='Edit_Help'
	BEGIN 
	Update AD_Help set Description = @Description, Title = @Title Where HelpId = @HelpID
	End
	ELSE if @Filter='List_Help'
	BEGIN 
	Select
	AD_Help.HelpId,
	AD_Definations.DefinationName as ComponentName,
	ModuleName  = (SELECT Title FROM Sec_Permissions WHERE Id IN (AD_Help.ModuleId)),
	Sec_Permissions.Title as FeatureName,
	AD_Help.Title,
	AD_Help.IsActive
	from AD_Definations
	Inner Join Sec_Permissions
	On AD_Definations.DefinationId = Sec_Permissions.ModuleId
	LEFT JOIN AD_Help
	ON Sec_Permissions.id IN (AD_Help.FeatureID)
	where AD_Definations.DefinationId IN (AD_Help.ComponentId)
	AND AD_Help.IsActive = @IsActive;
	End
	ELSE if @Filter='ListRow_Help'
	BEGIN 
	Update AD_Help SET IsActive = @IsActive where AD_Help.HelpId = @FeatureID
	End

End