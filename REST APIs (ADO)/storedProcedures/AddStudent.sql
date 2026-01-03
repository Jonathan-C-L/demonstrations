use jle16_ClassTrak
go

if exists(select [name] from sysobjects where [name] = 'AddStudent')
	drop procedure AddStudent
go

create procedure AddStudent
	@newFirst		nvarchar(30) = NULL,
	@newLast		nvarchar(30) = NULL,
	@newSchoolId	int = NULL,
	@newStudentId	int = NULL output,
	@status			nvarchar(100) = NULL output
as
/* arguments are null */
	if @newFirst is null
		begin
			set @status = 'First name is null'
			return -1
		end
	if @newLast is null
		begin
			set @status = 'Last name is null'
			return -2
		end
	if @newSchoolId is null
		begin
			set @status = 'School ID is null'
			return -3
		end		
/* procedure */
	insert into jle16_ClassTrak.dbo.Students (first_name, last_name, school_id)
	values (@newFirst, @newLast, @newSchoolId)
	set @newStudentId = 
		(select student_id 
		from jle16_ClassTrak.dbo.Students 
		where first_name = @newFirst and last_name = @newLast and school_id = @newSchoolId)
/* error check after procedure completed */
	if @@error <> 0
		begin
			set @status = 'Unknown issue with insert operation'
			return -5
		end
	set @status = 'Insert was successful'
	return 1
go

/* TEST CODE BELOW */
use jle16_ClassTrak
go

declare @first nvarchar(30) = 'Jon'
declare @last nvarchar(30) = 'Le' 
declare @school int = 69
declare @studId int = null

execute AddStudent @first, @last, @school, @studId output
select @studId as 'New Student ID'
go