CREATE TABLE [dbo].[TSS_Responses] (
    [ResponseId]    NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [QuestionId]    NUMERIC (18)   NOT NULL,
    [ResponseText]  NVARCHAR (250) NULL,
    [ResponseValue] NVARCHAR (MAX) NULL,
    [SortOrder]     INT            CONSTRAINT [DF_TSS_Responses_SortOrder] DEFAULT ((0)) NOT NULL,
    [IsPassed]      BIT            CONSTRAINT [DF_TSS_Responses_IsPassed] DEFAULT ((0)) NOT NULL,
    [MinValue]      FLOAT (53)     NULL,
    [MaxValue]      FLOAT (53)     NULL,
    [IsGps]         BIT            CONSTRAINT [DF_TSS_Responses_IsGps] DEFAULT ((0)) NOT NULL,
    [IsActive]      BIT            CONSTRAINT [DF_TSS_Responses_IsActive] DEFAULT ((1)) NOT NULL,
    [IsReadOnly]    BIT            NULL,
    [UserValues]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TSS_Responses] PRIMARY KEY CLUSTERED ([ResponseId] ASC)
);

