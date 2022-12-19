using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using Dapper;
using StudentManagement.Models;

namespace StudentManagement.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly IConfiguration _configuration;
        public StudentServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DefaultConnectionString");
            providerName = "System.Data.SqlClient";

        }

        public string ConnectionString { get; }
        public string providerName { get; }

        public IDbConnection Connection
        {
            get { return new SqlConnection(ConnectionString); }
        }

        public List<Student> GetStudents()
        {
            List<Student> result = new List<Student>();
            try
            {

                using (IDbConnection con = Connection)
                {
                    con.Open();
                    result = con.Query<Student>("sp_getStudentRecord", commandType: CommandType.StoredProcedure).ToList();
                    con.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return result;
            }

        }
        public string InsertStudent(Student model)
        {
            string result = string.Empty;
            try
            {

                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var std = con.Query<Student>("sp_postStudentRecord", new { FullName = model.FullName, EmailAddress = model.EmailAddress, City = model.City, CreatedBy = 1 }, commandType: CommandType.StoredProcedure).ToList();
                    if (std != null && std.FirstOrDefault().Response == "Saved Successfully")
                    {
                        result = "Saved Successfully";

                    }
                    con.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return errormsg;
            }

        }

        public string UpdateStudent(Student model)
        {
            string result = string.Empty;
            try
            {

                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var std = con.Query<Student>("sp_putStudentRecord", new { StudentId=model.StudentId, FullName = model.FullName, EmailAddress = model.EmailAddress, City = model.City}, commandType: CommandType.StoredProcedure).ToList();
                    if (std != null && std.FirstOrDefault().Response == "Updated Successfully")
                    {
                        result = "Updated Successfully";

                    }
                    con.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return errormsg;
            }

        }

        public string DeleteStudent(int studentId)
        {
            string result = string.Empty;
            try
            {

                using (IDbConnection con = Connection)
                {
                    con.Open();
                    var std = con.Query<Student>("sp_deleteStudentRecord", new { StudentId = studentId }, commandType: CommandType.StoredProcedure).ToList();
                    if (std != null && std.FirstOrDefault().Response == "Deleted Successfully")
                    {
                        result = "Deleted Successfully";
                    }
                    con.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                return errormsg;
            }

        }
    }

    public interface IStudentServices
    {
        public List<Student> GetStudents();

        public string InsertStudent(Student model);
        public string UpdateStudent(Student model);
        public string DeleteStudent(int studentId);

    }
}

