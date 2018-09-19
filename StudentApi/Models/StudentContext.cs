/* 
 * Student API Project
 * 
 * Developed by Andrew Overshiner
 */

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using System.Linq;

namespace StudentApi.Models
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public List<Student> GetAllStudents()
        {
            string[] data = File.ReadAllLines(@"students.csv");
            List<Student> students = new List<Student>();
            foreach (string line in data)
            {
                Student student = new Student();
                string[] splitLine = line.Split(',');
                student.Id = splitLine[0];
                student.Name = splitLine[1];
                student.Gpa = float.Parse(splitLine[2], CultureInfo.InvariantCulture.NumberFormat);
                students.Add(student);
            }

            if (students.Count == 0)
                return null;
            return students;
        }

        public Student GetStudent(string id)
        {
            List<Student> students = GetAllStudents();
            int index = students.FindIndex(student => student.Id == id);

            if (index == -1)
                return null;
            return students[index];
        }

        public float[] GetGpaRange()
        {
            List<Student> students = GetAllStudents();
            if (students == null)
                return null;

            float lowGpa = students.Min(student => student.Gpa);
            float highGpa = students.Max(student => student.Gpa);

            return new float[] { highGpa, lowGpa };
        }

        public void AddStudent(Student student)
        {
            List<Student> students = GetAllStudents();
            students.Add(student);

            StreamWriter file = new StreamWriter(@"students.csv", false);
            foreach (Student stu in students)
            {
                file.WriteLine(stu.ToString());
            }
            file.Close();
        }

        public bool DeleteStudent(string id)
        {
            List<Student> students = GetAllStudents();
            bool deleted = false;
            int index = students.FindIndex(student => student.Id == id);
            if (index == -1)
                return deleted;
            students.RemoveAt(index);

            StreamWriter file = new StreamWriter(@"students.csv", false);
            foreach (Student student in students)
            {
                file.WriteLine(student.ToString());
            }
            file.Close();
            deleted = true;

            return deleted;
        }
    }
}