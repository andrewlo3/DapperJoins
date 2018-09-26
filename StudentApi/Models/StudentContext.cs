/* 
 * Student API Project
 * 
 * Developed by Andrew Overshiner
 */

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Dapper;


namespace StudentApi.Models
{
    public class StudentContext : DbContext
    {
        string connectionString = "Server=localhost; Database=studentdb; UID=testuser; Password=dummypass864";

        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public List<Student> GetAllStudents()
        {
            string sql = "SELECT * FROM student";
            List<Student> students = new List<Student>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                students = connection.Query<Student>(sql).ToList();
                connection.Close();
            }

            if (students.Count == 0)
                return null;
            return students;
        }

        public Student GetStudent(string id)
        {
            string sql = $"SELECT * FROM student WHERE id={id}";
            List<Student> student = new List<Student>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                student = connection.Query<Student>(sql).ToList();
                connection.Close();
            }

            return student[0];
        }

        public List<float> GetGpaRange()
        {
            string sql = "SELECT MAX(gpa), MIN(gpa) FROM student";
            List<float> gpas = new List<float>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                gpas = connection.Query<float>(sql).ToList();
                connection.Close();
            }

            return gpas;
        }

        public void AddStudent(Student student)
        {
            string sql = "INSERT INTO student VALUES (@id, @name, @gpa)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(sql, student);
                connection.Close();
            }
        }

        public int DeleteStudent(string id)
        {
            string sql = $"DELETE FROM student WHERE id = {id}";
            int rowsDeleted = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                rowsDeleted = connection.Execute(sql);
                connection.Close();
            }

            return rowsDeleted;
        }
    }
}