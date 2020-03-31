-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec AD_GetDefinationTypes @FILTER=N'Paging',@Value=N'0',@Value2=N'10',@Value3=N''

CREATE PROCEDURE AD_GetDefinationTypes
	@filter NVARCHAR(50),
		@Value varchar(50) = NULL
		,@Value2 varchar(50) = NULL
		,@Value3 varchar(50) = NULL
		
		
AS
BEGIN
	IF	@Filter='All'
	BEGIN
		--SELECT * FROM AD_DefinationTypes
		SELECT distinct ad.* 
		FROM AD_DefinationTypes AS ad
		Inner Join Sec_UserDefinationType AS sd ON sd.DefinationTypeId =  ad.DefinationTypeId
		Inner join Sec_Users u on u.UserId = sd.UserId
		where  u.UserId= @Value or @Value=0
	END	
	else IF @FILTER = 'Paging'
				BEGIN
				if @Value2 = '-1'
				BEGIN
				
				  select  p.DefinationTypeId,p.DefinationType,p.IsActive,p.SortOrder,(Select dt.DefinationType from AD_DefinationTypes dt where dt.DefinationTypeId = p.PDefinationTypeId) as PDefinationTypeId
		from AD_DefinationTypes p
		where p.DefinationType like '%'+@Value3+'%' 


		select count(1) 'TotalRecord'
		from AD_DefinationTypes  p
		where p.DefinationType like '%'+@Value3+'%'
		End
				Else
				BEGIN

				
				  select  p.DefinationTypeId,p.DefinationType,p.IsActive,p.SortOrder,(Select dt.DefinationType from AD_DefinationTypes dt where dt.DefinationTypeId = p.PDefinationTypeId) as PDefinationTypeId
		from AD_DefinationTypes p
		where p.DefinationType like '%'+@Value3+'%' 
		Order By p.DefinationTypeId OFFSET cast(@Value as int)   ROWS FETCH NEXT cast(@Value2 as int) ROWS ONLY


		select count(1) 'TotalRecord'
		from AD_DefinationTypes  p
		where p.DefinationType like '%'+@Value3+'%'
		End

				END
				else IF @FILTER = 'Single'
				BEGIN 
					SELECT * FROM AD_DefinationTypes where DefinationTypeId = CONVERT(int, @Value) 
				END
				else IF @FILTER = 'Delete'
				BEGIN 
					delete  FROM AD_DefinationTypes where DefinationTypeId = CONVERT(int, @Value) 
				END
				else IF @FILTER = 'ChanceStatus'
				BEGIN 
					UPDATE [dbo].[AD_DefinationTypes]
          SET [IsActive] =case  
		                 when [IsActive]= 0 then 1 
						 when [IsActive]= 1 then 0 
						  end 
    
          where DefinationTypeId = CONVERT(int, @Value) 
				END
		
END