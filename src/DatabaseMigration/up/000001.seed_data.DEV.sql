-- instructors and students
set identity_insert dbo.Person on; 

insert dbo.Person
	(Id, LastName, FirstName, HireDate, EnrollmentDate, Discriminator) 
values 
	(1, 'Martinez', 'Rick', '2010-01-01', null, 'Instructor'),
	(2, 'Johanssen', 'Beth', '2002-04-01', null, 'Instructor'),
	(3, 'Lewis', 'Melissa', '2004-06-01', null, 'Instructor'),
	(4, 'Watney', 'Mark', null, '2014-10-01', 'Student'),
	(5, 'Vogel', 'Alex', null, '2015-01-01', 'Student'),
	(6, 'Montrose', 'Annie', null, '2014-06-01', 'Student'),
	(7, 'Henderson', 'Mitch', null, '2015-06-01', 'Student'),
	(8, 'Beck', 'Chris', null, '2014-06-01', 'Student'),
	(9, 'Kapoor', 'Vincent', null, '2015-01-01', 'Student'),
	(10,'Ng', 'Bruce', null, '2015-06-01', 'Student');

set identity_insert dbo.Person off;

-- departments
set identity_insert dbo.Department on; 

insert dbo.Department
	(Id, Name, Budget, StartDate, InstructorId) 
values 
	(1, 'Engineering', 1234567.89, '2010-07-04', 1),
	(2, 'English', 543210.98, '2011-02-03', 3);

set identity_insert dbo.Department off;

-- courses
set identity_insert dbo.Course on; 

insert dbo.Course
	(Id, Number, Title, Credits, DepartmentId) 
values 
	(1, '60A', 'Computer Science I', 3, 1),
	(2, '60B', 'Computer Science II', 3, 1),
	(3, '100A', 'Advanced Algorithms I', 4, 1),
	(4, '100B', 'Advanced Algorithms II', 4, 1),
	(5, '1A', 'English 1A', 3, 2),
	(6, '1B', 'English 1B', 3, 2);

set identity_insert dbo.Course off;

-- office assignments
insert dbo.OfficeAssignment
	(InstructorId, Location) 
values 
	(1, '100 Evans Hall'),
	(2, '200 Cory Hall'),
	(3, '300 Cory Hall')

-- course instructors
insert dbo.CourseInstructor
	(CourseId, InstructorId) 
values 
	(1, 1),
	(1, 2),
	(2, 1),
	(2, 2),
	(3, 1),
	(4, 2),
	(5, 3),
	(6, 3);

-- enrollments
insert dbo.Enrollment
	(CourseId, StudentId, Grade) 
values
	(1, 4, 0),
	(1, 5, 0),
	(1, 6, 1),
	(2, 4, 0),
	(2, 7, 2),
	(2, 8, 1),
	(3, 4, 0),
	(3, 9, 4),
	(4, 4, 0),
	(4, 10,0),
	(5, 4, 0),
	(5, 5, 2),
	(5, 6, 4),
	(5, 7, 0),
	(6, 4, 0),
	(6, 8, 1),
	(6, 9, 0),
	(6, 10,2);
