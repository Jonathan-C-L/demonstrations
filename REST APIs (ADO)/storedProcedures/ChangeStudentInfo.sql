use jle16_ClassTrak
go

if exists(select [name] from sysobjects where [name] = 'ChangeStudentInfo')
	drop procedure ChangeStudentInfo
go

create procedure ChangeStudentInfo
	@studentId		int = null,
	@newFirst		nvarchar(30) = NULL,
	@newLast		nvarchar(30) = NULL,
	@newSchoolId	int = NULL,
	@status			nvarchar(100) = NULL output
as
/* student not found */
	if @studentId is null
		begin
			set @status = 'Student ID is null'
			return -1
		end
	if not exists (select student_id from jle16_ClassTrak.dbo.Students where student_id = @studentId)
		begin
			set @status = concat('Could not find ', @studentId, ' in database')
			return 0
		end
/* arguments are null */
	if @newFirst is null
		begin
			set @status = 'First name is null'
			return -2
		end
	if @newLast is null
		begin
			set @status = 'Last name is null'
			return -3
		end
	if @newSchoolId is null
		begin
			set @status = 'School ID is null'
			return -4
		end		
/* procedure */
	update jle16_ClassTrak.dbo.Students 
	set 
		first_name = @newFirst,
		last_name = @newLast,
		school_id = @newSchoolId
	where student_id = @studentId
/* error check after procedure completed */
	if @@error <> 0
		begin
			set @status = 'Unknown issue with update operation'
			return -5
		end
	set @status = 'Update was successful'
	return 1
go

/* TEST CODE BELOW */
