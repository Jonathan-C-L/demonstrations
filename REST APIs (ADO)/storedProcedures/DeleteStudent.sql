use jle16_ClassTrak
go

if exists(select [name] from sysobjects where [name] = 'DeleteStudent')
	drop procedure DeleteStudent
go

create procedure DeleteStudent
	@studentId	int = null,
	@status		nvarchar(100) = NULL output
as
/* student not found */
	if @studentId is null
		begin
			set @status = concat(@studentId, 'is null')
			return -1
		end
	if not exists (select student_id from jle16_ClassTrak.dbo.Students where student_id = @studentId)
		begin
			set @status = concat('Could not find ', @studentId, ' in database')
			return 0
		end
	delete from jle16_ClassTrak.dbo.Results where student_id = @studentId;
	delete from jle16_ClassTrak.dbo.class_to_student where student_id = @studentId;
	delete from jle16_ClassTrak.dbo.Students where student_id = @studentId;
	if @@error <> 0
		begin
			set @status = 'Unknown issue with deletion operation'
			return -3
		end
	set @status = 'Deletion was successful'
	return 1
go

/* TEST CODE BELOW */
declare @stud int = 646
declare @ret int = 0
declare @message nvarchar(100) = ''

execute DeleteStudent @stud, @message output
select @ret as 'Return Value', @message as 'Status'
go

