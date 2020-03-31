CREATE TABLE [dbo].[AV_Widgets] (
    [WidgetId]   NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [WidgetName] NVARCHAR (50)  NULL,
    [Tilte]      NVARCHAR (50)  NULL,
    [WidgetType] NVARCHAR (50)  NULL,
    [Category]   NVARCHAR (50)  NULL,
    [SqlQuery]   NVARCHAR (MAX) NULL,
    [InPanel]    BIT            NULL,
    [Height]     INT            NULL,
    [Width]      INT            NULL,
    [Icon]       NVARCHAR (50)  NULL,
    [IsActive]   BIT            NULL,
    CONSTRAINT [PK_AV_Widgets] PRIMARY KEY CLUSTERED ([WidgetId] ASC)
);

