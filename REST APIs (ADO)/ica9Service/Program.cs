using ica9Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(); // switches from minimal APIs to controller-based APIs (alternative approach)
var app = builder.Build();

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allows any origin
                  ); // 'UserCors()' allows calling the webservice(ws) from any website

app.UseDeveloperExceptionPage(); // developer error messages displayed

// onload -> return a list of students to display in the primary table
app.MapGet("/", () =>
{
    List<List<string>> returnData = ClassTrakADO.GetAllStudents();

    return new
    {
        data = returnData,
        rows = returnData.Count - 1,
        status = (returnData.Count - 1 > 0) ? $"Retrieved rows: {returnData.Count - 1}" : "No records found",
    };
});
// onload -> return all of the classes from the database
app.MapGet("/Classes", () =>
{
    return new 
    {
        data = ClassTrakADO.GetAllClasses()
    };
});
// if a specific student is picked, return the classes that they belong in with a count of rows
app.MapGet("/GetClassInfo/{id}", (int id) =>
{
    List<List<string>> returnData = ClassTrakADO.GetClassInfo(id);

    return new
    {
        data = returnData,
        rows = returnData.Count - 1,
        status = (returnData.Count - 1 > 0) ? $"Retrieved rows: {returnData.Count - 1}" : "No records found"
    };
});
// /{id} binds the data from the url in the https request and allows the information to be used in the specific method
// when delete button is clicked for a specific student, remove their data from the database. Returning the affected rows count
app.MapDelete("/DeleteStudent/{id}", (int id) =>
{
    string returnMessage = ClassTrakADO.DeleteStudent(id);
    List<List<string>> returnData = ClassTrakADO.GetAllStudents();

    return new
    {
        data = returnData,
        rows = returnData.Count - 1,
        status = returnMessage
    };
});
// for UPDATE
// when the update button is pressed, change the names and/or school id accordingly
app.MapPut("/ChangeStudentInfo", (StudentInfo student) =>
{
    string returnMessage = ""; // return message initialization
    // error handling in the data sent in
    bool error = false;
    bool studIdIsNum = int.TryParse(student.id, out int studId);
    bool schoolIdIsNum = int.TryParse(student.schoolId, out int schoolId);

    // data validation
    // must have at least 1 character in the names input
    if (string.IsNullOrEmpty(student.firstName) || string.IsNullOrEmpty(student.lastName))
    {
        returnMessage = "<p>Name values cannot be empty</p>";
        error = true;
    }
    // student id must be a positive integer
    if (studId <= 0 || !studIdIsNum)
    {
        returnMessage += "<p>Student Id must be a positive integer</p>";
        error = true;
    }
    // school id must be a positive integer
    if (schoolId <= 0 || !schoolIdIsNum)
    {
        returnMessage += "<p>School Id must be a positive integer</p>";
        error = true;
    }
    // if there were no error, proceed to connect to the database and grab the information
    if (!error)
        returnMessage = ClassTrakADO.ChangeStudentInfo(studId, student.firstName, student.lastName, schoolId);
    
    // get all students to re-render the students display table
    List<List<string>> returnData = ClassTrakADO.GetAllStudents();

    return new
    {
        data = returnData,
        rows = returnData.Count - 1,
        status = returnMessage,
    };
});
// for INSERT
// when the new student form is filled out and the add student button is clicked, add the student into the database
app.MapPost("/AddStudent", (NewStudent student) =>
{
    string returnMessage = ""; // initalizing the return string
    int studentId = 0; // store the studentId temporarily to chain 2 stored procedures for adding a new student
    // error handling
    bool error = false;
    bool isNum = int.TryParse(student.schoolId, out int id);

    // data validation
    // names must have at least 1 character in the input
    if (string.IsNullOrEmpty(student.firstName) || string.IsNullOrEmpty(student.lastName))
    {
        returnMessage = "<p>Name values cannot be empty</p>";
        error = true;
    }
    // id must be a positive integer
    if (id <= 0 || !isNum)
    {
        returnMessage += "<p>School Id must be a positive integer</p>";
        error = true;
    }
    // at least 1 class must be chosen for the new student
    if (student.classes.Length <= 0)
    {
        returnMessage += "<p>Must select at least one class</p>";
        error = true;
    }
    // add student if no errors and save the returned student id for second stored procedure
    if (!error)
    {
        studentId = ClassTrakADO.AddStudent(student.firstName, student.lastName, id);
        // if student could not be added, return an error
        if(studentId <= 0)
        {
            error = true;
            returnMessage = "<p>Error in adding student</p>";
        }
    }
    // if student id successfully added, add the student into each class they selected
    if (!error && studentId > 0)
    {
        foreach (int classId in student.classes)
        {
            returnMessage = ClassTrakADO.AddStudentToClass(studentId, classId);
        }
    }
    return new
    {
        data = ClassTrakADO.GetAllClasses(),
        status = returnMessage
    };
});

app.Run();

// data binding classes
record class StudentInfo(string id, string firstName, string lastName, string schoolId);
record class NewStudent(string firstName, string lastName, string? schoolId, int[] classes);