using Microsoft.Data.SqlClient;

namespace ica9Service
{
    public class ClassTrakADO
    {
        private static string classTrakConn = "Server=YOUR_SERVER;Database=CLASSTRAK_DB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=false";

        public static List<List<string>> GetAllStudents()
        {
            List<List<string>> returnData = new();

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                string query = "SELECT * FROM Students WHERE first_name LIKE '[EF]%' ORDER BY last_name";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> colNames = new();

                        for (int i = 0; i < reader.FieldCount; i++)
                            colNames.Add(reader.GetName(i));
                        returnData.Add(colNames);

                        // read each line
                        while (reader.Read())
                        {
                            List<string> temp = new();
                            for (int i = 0; i < reader.FieldCount; i++)
                                temp.Add(reader[i].ToString());
                            returnData.Add(temp);
                        }
                        System.Diagnostics.Trace.WriteLine("Pause"); // breakpoint here to see what's going on
                    }
                }
            }
            return returnData;
        }
        public static List<List<string>> GetClassInfo(int id)
        {

            List<List<string>> returnData = new();

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "GetClassInfo";
                    command.Connection = connection;

                    // setting up parameters
                    SqlParameter studentId = new SqlParameter();
                    studentId.ParameterName = "@studentId"; // must match the stored procedure param
                    studentId.SqlDbType = System.Data.SqlDbType.Int;
                    studentId.Value = id;
                    studentId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(studentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // storing the returned information from the database
                        List<string> colNames = new();

                        for (int i = 0; i < reader.FieldCount; i++)
                            colNames.Add(reader.GetName(i));
                        returnData.Add(colNames);

                        // read each line
                        while (reader.Read())
                        {
                            List<string> temp = new();
                            for (int i = 0; i < reader.FieldCount; i++)
                                temp.Add(reader[i].ToString());
                            returnData.Add(temp);
                        }
                        System.Diagnostics.Trace.WriteLine("Pause"); // breakpoint here to see what's going on
                    }
                }
            }

            return returnData;
        }
        public static string DeleteStudent(int id)
        {
            string returnMessage = "";

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    // configuring command
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "DeleteStudent";
                    command.Connection = connection;

                    // IN parameteres
                    SqlParameter studentId = new SqlParameter();
                    studentId.ParameterName = "@studentId";
                    studentId.SqlDbType = System.Data.SqlDbType.Int;
                    studentId.Value = id;
                    studentId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(studentId);

                    // OUT parameters
                    SqlParameter status = new SqlParameter();
                    status.ParameterName = "@status"; // must match the stored procedure param
                    status.SqlDbType = System.Data.SqlDbType.NVarChar;
                    status.Size = 100;
                    status.Value = string.Empty; // variable is supposed to catch return value -> start it empty
                    status.Direction = System.Data.ParameterDirection.Output; // set as output variable -> out param in sql
                    command.Parameters.Add(status);

                    SqlParameter ret = new SqlParameter();
                    ret.ParameterName = "@ret";
                    ret.SqlDbType = System.Data.SqlDbType.Int;
                    //countryOld.Size = 100; -> int does not require a size
                    ret.Value = 0; // don't need this, but set to 0
                    ret.Direction = System.Data.ParameterDirection.ReturnValue; // this is a return value (an int)
                    command.Parameters.Add(ret);

                    // BELOW IS FOR NON-QUERY 
                    try
                    {
                        command.ExecuteNonQuery(); // this can have some issues that haven't been accounted for (not specified*)
                        returnMessage = status.Value.ToString(); // status.Value is an object returned, ToString will turn it into a string
                        returnMessage += $" | Rows affected: {ret.Value}";
                        //returnMessage = command.Parameters["status"].ToString(); -> can do this too
                    }
                    catch (Exception e)
                    {
                        returnMessage = e.Message;
                    }
                }
            }
            return returnMessage;
        }
        // update with stored procedure
        public static string ChangeStudentInfo(int studId, string firstName, string lastName, int schoolId)
        {
            string returnMessage = "";

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    // configuring command
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "ChangeStudentInfo";
                    command.Connection = connection;

                    // IN parameteres
                    SqlParameter studentId = new SqlParameter();
                    studentId.ParameterName = "@studentId";
                    studentId.SqlDbType = System.Data.SqlDbType.Int;
                    studentId.Value = studId;
                    studentId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(studentId);

                    SqlParameter newFirst = new SqlParameter();
                    newFirst.ParameterName = "@newFirst";
                    newFirst.SqlDbType = System.Data.SqlDbType.NVarChar;
                    newFirst.Size = 30;
                    newFirst.Value = firstName;
                    newFirst.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newFirst);

                    SqlParameter newLast = new SqlParameter();
                    newLast.ParameterName = "@newLast";
                    newLast.SqlDbType = System.Data.SqlDbType.NVarChar;
                    newLast.Size = 30;
                    newLast.Value = lastName;
                    newLast.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newLast);

                    SqlParameter newSchoolId = new SqlParameter();
                    newSchoolId.ParameterName = "@newSchoolId";
                    newSchoolId.SqlDbType = System.Data.SqlDbType.Int;
                    newSchoolId.Value = schoolId;
                    newSchoolId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newSchoolId);

                    // OUT parameters
                    SqlParameter status = new SqlParameter();
                    status.ParameterName = "@status"; // must match the stored procedure param
                    status.SqlDbType = System.Data.SqlDbType.NVarChar;
                    status.Size = 100;
                    status.Value = string.Empty; // variable is supposed to catch return value -> start it empty
                    status.Direction = System.Data.ParameterDirection.Output; // set as output variable -> out param in sql
                    command.Parameters.Add(status);

                    SqlParameter ret = new SqlParameter();
                    ret.ParameterName = "@ret";
                    ret.SqlDbType = System.Data.SqlDbType.Int;
                    //countryOld.Size = 100; -> int does not require a size
                    ret.Value = 0; // don't need this, but set to 0
                    ret.Direction = System.Data.ParameterDirection.ReturnValue; // this is a return value (an int)
                    command.Parameters.Add(ret);

                    // BELOW IS FOR NON-QUERY 
                    try
                    {
                        command.ExecuteNonQuery(); // this can have some issues that haven't been accounted for (not specified*)
                        returnMessage = status.Value.ToString(); // status.Value is an object returned, ToString will turn it into a string
                        returnMessage += $" | Rows affected: {ret.Value}";
                        //returnMessage = command.Parameters["status"].ToString(); -> can do this too
                    }
                    catch (Exception e)
                    {
                        returnMessage = e.Message;
                    }

                }

            }
            
            return returnMessage;
        }
        // Update with no stored procedure
        //public static string ChangeStudentInfo(int studId, string firstName, string lastName, int schoolId)
        //{
        //    string returnMessage = "Rows affected: ";
        //    using (SqlConnection connection = new SqlConnection(classTrakConn))
        //    {
        //        connection.Open();
        //        string query = $"update jle16_ClassTrak.dbo.Students set first_name = '{firstName}', last_name = '{lastName}', school_id = {schoolId} where student_id = {studId}";
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            returnMessage += command.ExecuteNonQuery();
        //        }
        //    }
        //    return returnMessage;
        //}
        public static string AddStudentToClass(int studentId, int classId)
        {
            string returnMessage = "";

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    // configuring command
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "AddStudentToClass";
                    command.Connection = connection;

                    // IN parameteres
                    SqlParameter checkId = new SqlParameter();
                    checkId.ParameterName = "@studentId";
                    checkId.SqlDbType = System.Data.SqlDbType.Int;
                    checkId.Value = studentId;
                    checkId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(checkId);

                    SqlParameter newClassId = new SqlParameter();
                    newClassId.ParameterName = "@classId";
                    newClassId.SqlDbType = System.Data.SqlDbType.Int;
                    newClassId.Value = classId;
                    newClassId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newClassId);

                    // OUT parameters
                    SqlParameter status = new SqlParameter();
                    status.ParameterName = "@status"; // must match the stored procedure param
                    status.SqlDbType = System.Data.SqlDbType.NVarChar;
                    status.Size = 100;
                    status.Value = string.Empty; // variable is supposed to catch return value -> start it empty
                    status.Direction = System.Data.ParameterDirection.Output; // set as output variable -> out param in sql
                    command.Parameters.Add(status);

                    SqlParameter ret = new SqlParameter();
                    ret.ParameterName = "@ret";
                    ret.SqlDbType = System.Data.SqlDbType.Int;
                    //countryOld.Size = 100; -> int does not require a size
                    ret.Value = 0; // don't need this, but set to 0
                    ret.Direction = System.Data.ParameterDirection.ReturnValue; // this is a return value (an int)
                    command.Parameters.Add(ret);

                    // BELOW IS FOR NON-QUERY 
                    try
                    {
                        command.ExecuteNonQuery(); // this can have some issues that haven't been accounted for (not specified*)
                        returnMessage = status.Value.ToString(); // status.Value is an object returned, ToString will turn it into a string
                        //returnMessage = command.Parameters["status"].ToString(); -> can do this too
                    }
                    catch (Exception e)
                    {
                        returnMessage = e.Message;
                    }
                }
            }
            return returnMessage;
        }
        public static int AddStudent(string firstName, string lastName, int? schoolId)
        {
            int returnId;

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    // configuring command
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "AddStudent";
                    command.Connection = connection;

                    // IN parameteres
                    SqlParameter newFirstName = new SqlParameter();
                    newFirstName.ParameterName = "@newFirst";
                    newFirstName.SqlDbType = System.Data.SqlDbType.NVarChar;
                    newFirstName.Size = 30;
                    newFirstName.Value = firstName;
                    newFirstName.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newFirstName);

                    SqlParameter newLastName = new SqlParameter();
                    newLastName.ParameterName = "@newLast";
                    newLastName.SqlDbType = System.Data.SqlDbType.NVarChar;
                    newLastName.Size = 30;
                    newLastName.Value = lastName;
                    newLastName.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newLastName);

                    SqlParameter newSchoolId = new SqlParameter();
                    newSchoolId.ParameterName = "@newSchoolId";
                    newSchoolId.SqlDbType = System.Data.SqlDbType.Int;
                    newSchoolId.Value = schoolId;
                    newSchoolId.Direction = System.Data.ParameterDirection.Input;
                    command.Parameters.Add(newSchoolId);

                    // OUT parameters
                    SqlParameter newStudentId = new SqlParameter();
                    newStudentId.ParameterName = "@newStudentId"; // must match the stored procedure param
                    newStudentId.SqlDbType = System.Data.SqlDbType.Int;
                    newStudentId.Size = 100;
                    newStudentId.Value = string.Empty; // variable is supposed to catch return value -> start it empty
                    newStudentId.Direction = System.Data.ParameterDirection.Output; // set as output variable -> out param in sql
                    command.Parameters.Add(newStudentId);

                    SqlParameter ret = new SqlParameter();
                    ret.ParameterName = "@ret";
                    ret.SqlDbType = System.Data.SqlDbType.Int;
                    //countryOld.Size = 100; -> int does not require a size
                    ret.Value = 0; // don't need this, but set to 0
                    ret.Direction = System.Data.ParameterDirection.ReturnValue; // this is a return value (an int)
                    command.Parameters.Add(ret);

                    // BELOW IS FOR NON-QUERY 
                    try
                    {
                        command.ExecuteNonQuery(); // this can have some issues that haven't been accounted for (not specified*)
                        returnId = (int)newStudentId.Value; // status.Value is an object returned, ToString will turn it into a string
                        //returnMessage = command.Parameters["status"].ToString(); -> can do this too
                    }
                    catch (Exception e)
                    {
                        returnId = -1;
                    }
                }
            }
            return returnId;
        }
        public static List<List<string>> GetAllClasses()
        {
            List<List<string>> returnData = new();

            using (SqlConnection connection = new SqlConnection(classTrakConn))
            {
                connection.Open();
                string query = "SELECT class_id, class_desc FROM Classes ORDER BY class_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> colNames = new();

                        for (int i = 0; i < reader.FieldCount; i++)
                            colNames.Add(reader.GetName(i));
                        returnData.Add(colNames);

                        // read each line
                        while (reader.Read())
                        {
                            List<string> temp = new();
                            for (int i = 0; i < reader.FieldCount; i++)
                                temp.Add(reader[i].ToString());
                            returnData.Add(temp);
                        }
                        System.Diagnostics.Trace.WriteLine("Pause"); // breakpoint here to see what's going on
                    }
                }
            }
            return returnData;
        }
    }
}
