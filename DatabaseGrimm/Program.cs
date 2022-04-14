using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace DatabaseGrimm
{
    class Program
    {

        public static List<Employee> GetEmployeesFromDatabase(DbCommand cmd)
        {
            List<Employee> results = new List<Employee>();
            cmd.CommandText = "select * from Employees";
            Employee empl;
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    empl = new Employee(Convert.ToInt32(reader["EmployeeNum"]), Convert.ToString(reader["EmployeeName"]), Convert.ToString(reader["EmployeeShift"]), Convert.ToSingle(reader["HourlyRate"]), Convert.ToString(reader["ClearenceLvl"]));
                    results.Add(empl);
                }
            }
            return results;
        }

        public static int GetNextID(List<Employee> employees)
        {
            int NewID = 0;
            foreach (Employee employee in employees)
            {
                if (employee.ID > NewID)
                {
                    NewID = employee.ID;
                }
            }
            return NewID +1;
        }

        static void Main(string[] args)
        {
            string provider = ConfigurationManager.AppSettings["provider"];
            string connectionString = ConfigurationManager.AppSettings["connectionString"];
            List<Employee> employees;

            DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
            using (DbConnection conn = factory.CreateConnection())
            {
                if (conn == null) 
                {
                    Console.WriteLine("There was an error connecting to database.");
                    Console.ReadLine();
                    return;
                }
                conn.ConnectionString = connectionString;
                conn.Open();

                DbCommand cmd = conn.CreateCommand();

                bool quit = false;
                while (quit == false){

                    int choice=0;
                    try
                    {
                        Console.WriteLine("What would you like to do: ");
                        Console.WriteLine("View database \t\t= 1");
                        Console.WriteLine("Add to database \t= 2");
                        Console.WriteLine("Delete from database \t= 3");
                        Console.WriteLine("Exit \t\t\t= 4");
                        Console.Write("Enter the number of your selection: ");
                        choice = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("");
                    }
                    catch
                    {
                        Console.WriteLine("Hum, that input was invalid. Please try again.");
                    }

                    if (choice == 1){
                        //Print Employee Names
                        employees = GetEmployeesFromDatabase(cmd);
                        Writer.WriteEmployeesToScreen(employees);
                        Console.WriteLine("");
                    }
                    else if(choice == 2)
                    {
                        //Add Employee
                        Console.Write("Enter an employees name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter an employees shift: ");
                        string shift = Console.ReadLine();
                        Console.Write("Enter hourly rate: ");
                        double HRate = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter an employees clearence: ");
                        string clearence = Console.ReadLine();
                        Console.WriteLine("");

                        employees = GetEmployeesFromDatabase(cmd);
                        int newID = GetNextID(employees);

                        string query = String.Format("insert into employees values ({0}, '{1}', '{2}', {3:F2}, '{4}')", newID, name, shift, HRate, clearence);
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();

                        employees = GetEmployeesFromDatabase(cmd);
                        Writer.WriteEmployeesToScreen(employees);
                        Console.WriteLine("");
                    }
                    else if(choice == 3)
                    {
                        Console.WriteLine("\nThe current employees are: ");
                        employees = GetEmployeesFromDatabase(cmd);
                        Writer.WriteEmployeesToScreen(employees);
                        Console.WriteLine("");
                        Console.Write("Enter an employee number to delete that employee: ");
                        int delcho = Convert.ToInt32(Console.ReadLine());

                        cmd.CommandText = String.Format("delete from employees where EmployeeNum='{0}'", delcho);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("The employee has been removed: \n");
                        employees = GetEmployeesFromDatabase(cmd);
                        Writer.WriteEmployeesToScreen(employees);
                        Console.WriteLine("");


                    }
                    else if (choice == 4)
                    {
                        Console.WriteLine("Thank you for using this program. :) Goodbye.");
                        quit = true;
                        Console.ReadLine();
                    }


                }

            }
        }
    }
}
