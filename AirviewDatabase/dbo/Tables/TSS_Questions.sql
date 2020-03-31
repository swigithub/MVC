CREATE TABLE [dbo].[TSS_Questions] (
    [QuestionId]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [SectionId]              NUMERIC (18)   NOT NULL,
    [QuestionTypeId]         NUMERIC (18)   NOT NULL,
    [Question]               NVARCHAR (250) NOT NULL,
    [Description]            NVARCHAR (500) NULL,
    [Weightage]              FLOAT (53)     CONSTRAINT [DF_TSS_Questions_Weightage] DEFAULT ((0)) NOT NULL,
    [SortOrder]              INT            CONSTRAINT [DF_TSS_Questions_SortOrder] DEFAULT ((0)) NOT NULL,
    [IsRequired]             BIT            CONSTRAINT [DF_TSS_Questions_IsRequired] DEFAULT ((0)) NOT NULL,
    [IsNoteRequired]         BIT            CONSTRAINT [DF_TSS_Questions_IsNoteRequired] DEFAULT ((0)) NOT NULL,
    [IsImageRequired]        BIT            CONSTRAINT [DF_TSS_Questions_IsImageRequired] DEFAULT ((0)) NOT NULL,
    [IsBarCodeRequired]      BIT            CONSTRAINT [DF_TSS_Questions_IsBarCodeRequired] DEFAULT ((0)) NOT NULL,
    [CreatedOn]              DATETIME       NOT NULL,
    [CreatedBy]              NUMERIC (18)   NOT NULL,
    [IsRepeatable]           BIT            NULL,
    [UnitTypeId]             NUMERIC (18)   NULL,
    [UnitId]                 NUMERIC (18)   NULL,
    [UnitSystemId]           NUMERIC (18)   NULL,
    [IsActive]               BIT            CONSTRAINT [DF_TSS_Questions_IsActive] DEFAULT ((1)) NULL,
    [IsVerificationRequired] BIT            NOT NULL,
    [IsVideoRequired]        BIT            NOT NULL,
    [IsAudioRequired]        BIT            NOT NULL,
    [IsDocumentRequired]     BIT            NOT NULL,
    [TotalColumn]            INT            NULL,
    [TotalRows]              INT            NULL,
    [DynamicRows]            BIT            NULL,
    [DynamicRowsCount]       INT            NULL,
    [IsImageDetailRequired]  BIT            NULL,
    [IsMultiLocation]        BIT            NULL,
    [SurveyEntity]           VARCHAR (50)   NULL,
    [Prefix]                 VARCHAR (50)   NULL,
    CONSTRAINT [PK_TSS_Questions] PRIMARY KEY CLUSTERED ([QuestionId] ASC)
);


GO
create TRIGGER AV_UpdateSectionParams ON TSS_Questions
AFTER INSERT
AS
BEGIN
	DECLARE @IsRepeated int=-1
	DECLARE @SectionId NUMERIC
	DECLARE @IsRepeatable BIT
	
	
	SELECT @SectionId=INSERTED.SectionId, @IsRepeatable=INSERTED.IsRepeatable
	FROM INSERTED; 
	
	SET @IsRepeated=ISNULL((SELECT COUNT(tq.SectionId) FROM TSS_Questions AS tq WHERE tq.SectionId=@SectionId AND tq.IsRepeatable=1),0)
	   
    IF @IsRepeated>=0 AND @IsRepeatable=1
    BEGIN
		UPDATE TSS_Sections
		SET IsRepeatable = @IsRepeatable
		WHERE SectionId=@SectionId
    END	
END