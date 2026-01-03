use jle16_ClassTrak
go

if exists(select [name] from sysobjects where [name] = 'AddStudentToClass')
	drop procedure AddStudentToClass
go

create procedure AddStudentToClass
	@classId		int = null,
	@studentId		int = null,
	@status			nvarchar(100) = NULL output
as
	if @studentId is null
		begin
			set @status = 'Student ID is null'
			return -1
		end
	if @classId is null
		begin
			set @status = 'Class ID is null'
			return -2
		end
/* procedure */
	begin
		insert into jle16_ClassTrak.dbo.class_to_student (class_id, student_id)
		values (@classId, @studentId)
	end
/* error check after procedure completed */
	if @@error <> 0
		begin
			set @status = 'Unknown issue with insert operation'
			return -3
		end
	set @status = 'Insert was successful'
	return 1
go

/* TEST CODE BELOW */
use jle16_ClassTrak
go

declare @class int = 54
declare @stud int = 200349551
declare @message nvarchar(100) = ''

execute AddStudentToClass @class, @stud, @message output
select @message as 'Status'
go