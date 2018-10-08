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
        string studentConnectionString = "Server=localhost; Database=studentdb; UID=testuser; Password=dummypass864";
        string saleCoConnectionString = "Server=localhost; Database=saleco; UID=testuser; Password=dummypass864";

        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public List<Student> GetStudents()
        {
            string sql = "SELECT * FROM student";
            List<Student> students = new List<Student>();

            using (MySqlConnection connection = new MySqlConnection(studentConnectionString))
            {
                connection.Open();
                students = connection.Query<Student>(sql).ToList();
                connection.Close();
            }

            if (students.Count == 0)
                return null;
            return students;
        }

        public List<Invoice> GetInvoices()
        {
            string sql = "SELECT invoice.INV_NUMBER AS InvoiceNumber, invoice.INV_DATE AS InvoiceDate, " + 
                "line.LINE_UNITS AS LineUnits, line.P_CODE AS ProductCode, line.LINE_PRICE AS LinePrice " +
                "FROM invoice " +
                "INNER JOIN line ON invoice.INV_NUMBER = line.INV_NUMBER";
            List<Invoice> invoices = new List<Invoice>();

            using (MySqlConnection connection = new MySqlConnection(saleCoConnectionString))
            {
                Dictionary<int, Invoice> invoiceDictionary = new Dictionary<int, Invoice>();

                invoices = connection.Query<Invoice, LineItem, Invoice>(
                    sql,
                    (invoice, lineItem) =>
                    {
                        if (!invoiceDictionary.TryGetValue(invoice.InvoiceNumber, out Invoice invoiceEntry))
                        {
                            invoiceEntry = invoice;
                            invoiceEntry.LineItems = new List<LineItem>();
                            invoiceDictionary.Add(invoiceEntry.InvoiceNumber, invoiceEntry);
                        }

                        invoiceEntry.LineItems.Add(lineItem);
                        return invoiceEntry;
                    },
                    splitOn: "LineUnits")
                .Distinct()
                .ToList();
            }

            if (invoices.Count == 0)
                return null;
            return invoices;
        }

        public List<Customer> GetCustomers()
        {
            string sql = "SELECT customer.CUS_CODE AS CustomerCode, customer.CUS_FNAME AS FirstName, customer.CUS_LNAME As LastName, " +
                "invoice.INV_NUMBER AS InvoiceNumber, invoice.INV_DATE AS InvoiceDate, " +
                "line.LINE_UNITS AS LineUnits, line.P_CODE AS ProductCode, line.LINE_PRICE AS LinePrice " +
                "FROM customer " +
                "INNER JOIN invoice ON customer.CUS_CODE = invoice.CUS_CODE " +
                "INNER JOIN line ON invoice.INV_NUMBER = line.INV_NUMBER " +
                "ORDER BY CustomerCode";
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection connection = new MySqlConnection(saleCoConnectionString))
            {
                Dictionary<int, Invoice> invoiceDictionary = new Dictionary<int, Invoice>();
                Dictionary<int, Customer> customerDictionary = new Dictionary<int, Customer>();

                customers = connection.Query<Customer, Invoice, LineItem, Customer>(
                    sql,
                    (customer, invoice, lineItem) =>
                    {
                        if (!invoiceDictionary.TryGetValue(invoice.InvoiceNumber, out Invoice invoiceEntry))
                        {
                            invoiceEntry = invoice;
                            invoiceEntry.LineItems = new List<LineItem>();
                            invoiceDictionary.Add(invoiceEntry.InvoiceNumber, invoiceEntry);
                        }

                        invoiceEntry.LineItems.Add(lineItem);

                        if (!customerDictionary.TryGetValue(customer.CustomerCode, out Customer customerEntry))
                        {
                            customerEntry = customer;
                            customerEntry.Invoices = new List<Invoice>();
                            customerDictionary.Add(customer.CustomerCode, customerEntry);
                        }

                        customerEntry.Invoices.Add(invoice);
                        return customerEntry;
                    },
                    splitOn: "InvoiceNumber, LineUnits")
                .Distinct()
                .ToList();
            }

            if (customers.Count == 0)
                return null;
            return customers;
        }

        public Student GetStudentById(string id)
        {
            string sql = $"SELECT * FROM student WHERE id={id}";
            List<Student> student = new List<Student>();

            using (MySqlConnection connection = new MySqlConnection(studentConnectionString))
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

            using (MySqlConnection connection = new MySqlConnection(studentConnectionString))
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

            using (MySqlConnection connection = new MySqlConnection(studentConnectionString))
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

            using (MySqlConnection connection = new MySqlConnection(studentConnectionString))
            {
                connection.Open();
                rowsDeleted = connection.Execute(sql);
                connection.Close();
            }

            return rowsDeleted;
        }
    }
}