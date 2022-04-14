using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseGrimm
{
    class Employee
    {
        //declare variables and set get/set
        public int ID { get; set; }

        public string Name { get; set; }
        public string Shift { get; set; }
        public float Pay { get; set; }
        public string Clearence { get; set; }

        //default constructor
        public Employee()
        {
            ID = 0;
            Name = "Employee Name";
            Shift = "Employee Shift";
            Pay = 0;
            Clearence = "Employee Clearence LVL";
        }

        //constructor
        public Employee(int id, string name, string shift, float pay, string clearence)
        {
            ID = id;
            Name = name;
            Shift = shift;
            Pay = pay;
            Clearence = clearence;
        }

        //to string
        public override string ToString()
        {
            return String.Format("ID={0}, Name={1}, Shift={2}, Pay={3}, Clearence={4}", ID, Name, Shift, Pay, Clearence);
        }
    }
}
