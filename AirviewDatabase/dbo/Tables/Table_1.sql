CREATE TABLE [dbo].[Table_1] (
    [studid] NUMERIC (18) NULL,
    [subjid] NUMERIC (18) NULL,
    [marks]  NUMERIC (18) NULL,
    CONSTRAINT [IX_Table_1] UNIQUE NONCLUSTERED ([studid] ASC)
);

