CREATE PROCEDURE [dbo].[TSS_CreateDemoWorkOrder]
@Filter varchar(50)=NULL,
@SurveyId NUMERIC(18,0)
AS
BEGIN
declare @p2 dbo.Tb_AV_Workorder
DECLARE @ClientId as numeric=0


SELECT @ClientId=ClientId from TSS_SurveyDocuments
where SurveyId=@SurveyId

DECLARE @RevisionId int=0
IF  EXISTS(select 1 from AV_Sites where SiteCode='DEMO'+Convert(varchar,@SurveyId)+'0')
BEGIN
	Select @RevisionId=ISNULL(MAX((RevisionId)),0)+1 From AV_Sites where SiteCode='DEMO'+Convert(varchar,@SurveyId)+'0' 
	Update AV_Sites SET RevisionId=@RevisionId where SiteCode='DEMO'+Convert(varchar,@SurveyId)+'0'
END

--insert into @p2 
--values(NULL,N'102',N'103',N'1',N'DEMO'+Convert(varchar,@SurveyId)+CONVERT(varchar,@RevisionId),N'40.749410',N'-74.373297',N'-',NULL,NULL,N'83295',NULL,NULL,NULL,NULL,NULL,NULL,
--'2018-11-08 00:00:00',N'0',N'0',N'0',N'0',NULL,N'DEMO'+Convert(varchar,@SurveyId)+CONVERT(varchar,@RevisionId),N'3132',N'3133',N'-',N'0',0,0,0,N'0',N'0',N'',N'',@SurveyId,10005,N'0',0,N'0',0)



insert into @p2 
values(NULL,N'102',N'103',CAST(@ClientId as nvarchar(50)),N'DEMO'+Convert(varchar,@SurveyId)+CONVERT(varchar,@RevisionId),N'40.749410',N'-74.373297',N'-',NULL,NULL,N'83295',NULL,NULL,NULL,NULL,NULL,NULL,
'2018-11-08 00:00:00',N'0',N'82',N'4',N'4',NULL,N'DEMO'+Convert(varchar,@SurveyId)+CONVERT(varchar,@RevisionId),N'3132',N'3133',N'-',N'0',0,0,0,N'0',N'0',N'',N'',@SurveyId,10005,N'0',0,N'4',0,N'4',N'',N'',N'N/A','N/A')


EXEC AV_InsertWorkorder @Filter='NewWorkOrder',@Workorder=@p2,@SubmittedById=11
END