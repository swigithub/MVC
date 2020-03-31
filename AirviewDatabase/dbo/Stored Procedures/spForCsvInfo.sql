create procedure spForCsvInfo
@FileID int
as 
begin
select * from tsvFileInformation tsvFile where tsvFile.fileID_Fk=@FileID
end