if exists(select [name] from sysobjects where [name] = 'GetClassInfo')
	drop procedure GetClassInfo
go

create procedure GetClassInfo
	@studentId int
as
	select	cts.class_id as 'Class Id', 
			class_desc as 'Class Desc', 
			coalesce(days, 0) as 'Days', 
			start_date as 'Start Date', 
			Instructors.instructor_id as 'Instructor ID', 
			first_name as 'Instructor First Name', 
			last_name as 'Instructor Last Name'
	from class_to_student as cts
		join Classes
		on cts.class_id = Classes.class_id
		join Instructors
		on Classes.instructor_id = Instructors.instructor_id
	where cts.student_id = @studentId
go

/* TEST CODE BELOW */
declare @stud int = 646
execute GetClassInfo @stud
go

