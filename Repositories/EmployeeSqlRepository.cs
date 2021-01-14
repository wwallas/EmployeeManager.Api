using EmployeeManager.Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Api.Repositories
{
    public class EmployeeSqlRepository : IEmployeeRepository
    {
        private readonly AppDbContext db = null;

        public EmployeeSqlRepository(AppDbContext db)
        {
            this.db = db;
        }

        public List<Employee> SelectAll()
        {
            //List<Employee> data = db.Employees.FromSqlRaw("SELECT EmployeeID,FirstName," +
            //                                               " LastName, Title, BirthDate, HireDate," +
            //                                               " Country, Notes FROM Employees" +
            //                                               " ORDER BY EmployeeID ASC").ToList();
            List<Employee> data = db.Employees.FromSqlRaw("EXEC Employees_SelectAll").ToList();
            return data;
        }

        public Employee SelectByID(int id)
        {
            //Employee emp = db.Employees.FromSqlRaw("SELECT EmployeeID, Firstname," +
            //                                       "LastName, Title, BirthDate, HireDate," +
            //                                       " Country, Notes FROM Employees" +
            //                                       "WHERE EmployeeID={0}", id).SingleOrDefault();
            SqlParameter p = new SqlParameter("EmployeeID", id);
            Employee emp = db.Employees. FromSqlRaw("EXEC Employees_SelectByID @EmployeeID", p).ToList().SingleOrDefault();

            return emp;
        }

        public void Insert(Employee emp) 
        {
            SqlParameter[] p = new SqlParameter[8];
            p[0] = new SqlParameter("@FirstName", emp.FirstName);
            p[1] = new SqlParameter("@LastName", emp.LastName);
            p[2] = new SqlParameter("@Title", emp.Title);
            p[3] = new SqlParameter("@BirthDate", emp.BirthDate);
            p[4] = new SqlParameter("@HireDate", emp.HireDate);
            p[5] = new SqlParameter("@Country", emp.Country);
            p[6] = new SqlParameter("@Notes", emp.Notes ?? SqlString.Null);
            p[7] = new SqlParameter("@EmployeeID", SqlDbType.Int);
            p[7].Direction = ParameterDirection.Output;

            int count = db.Database.ExecuteSqlRaw("EXEC Employees_Insert @FirstName,@LastName,@Title,@BirthDate,@HireDate,@Country,@Notes,@EmployeeID OUT", p);
        }

        public void Update(Employee emp)
        {
            //int count = db.Database.ExecuteSqlRaw("UPDATE Employees " +
            //    "SET FirstName={0}, LastName={1}, Title={2}, BirthDate={3}, " +
            //    "HireDate={4},Country ={ 5}, Notes ={ 6} WHERE EmployeeID = { 7 }",emp.FirstName
            //    ,emp.LastName, emp.Title, emp.BirthDate, emp.HireDate, emp.Country,
            //    emp.Notes, emp.EmployeeID);

            SqlParameter[] p = new SqlParameter[8];
            p[0] = new SqlParameter("@EmployeeID", emp.EmployeeID);
            p[1] = new SqlParameter("@FirstName", emp.FirstName);
            p[2] = new SqlParameter("@LastName", emp.LastName);
            p[3] = new SqlParameter("@Title", emp.Title);
            p[4] = new SqlParameter("@BirthDate", emp.BirthDate);
            p[5] = new SqlParameter("@HireDate", emp.HireDate);
            p[6] = new SqlParameter("@Country", emp.Country);
            p[7] = new SqlParameter("@Notes", emp.Notes ?? SqlString.Null);
            int count = db.Database.ExecuteSqlRaw("EXEC Employees_Update" +
            "@EmployeeID, @FirstName, @LastName, @Title, @BirthDate," +
            "@HireDate, @Country, @Notes", p);
        }

        public void Delete(int id)
        {
            SqlParameter p = new SqlParameter("@EmployeeID", id);
            int count = db.Database.ExecuteSqlRaw("EXEC Employees_Delete @EmployeeID", p);
        }        

 
    }
}
