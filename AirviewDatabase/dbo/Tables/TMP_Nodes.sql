CREATE TABLE [dbo].[TMP_Nodes] (
    [NodeId]      NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [TemplateId]  NUMERIC (18)  NOT NULL,
    [NodeTitle]   NVARCHAR (50) NULL,
    [Height]      INT           NOT NULL,
    [Width]       INT           NOT NULL,
    [x_axis]      INT           NOT NULL,
    [y_axis]      INT           NOT NULL,
    [PageTyppeId] NUMERIC (18)  NULL,
    [NodeUrl]     NVARCHAR (50) NULL,
    [NodeSQL]     NVARCHAR (50) NULL,
    [IsActive]    BIT           NOT NULL,
    CONSTRAINT [PK_TMP_Nodes] PRIMARY KEY CLUSTERED ([NodeId] ASC)
);

