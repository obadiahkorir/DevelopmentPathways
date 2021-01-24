using DevelopmentPathWays.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DevelopmentPathWays.Controllers
{
    public class HomeController : Controller
    {
        //Connection to Databse1
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["Pathways"].ToString();
            con = new SqlConnection(constr);

        }
        //connection to database
        private DB_Entities _db = new DB_Entities();

        //index page
        public ActionResult Index()
        {
            //check session
            if (Session["idUser"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //Register View
        public ActionResult Register()
        {
            return View();
        }
        //Login View
        public ActionResult Login()
        {
            return View();
        }
        //Login View
        public ActionResult Employees()
        {
            return View();
        }
        //Get users Detail 
        public ActionResult UsersList()
        {
            var users = _db.Users;
            return View(users);
        }
        //Get All the Employees
        public ActionResult EmployeesList()
        {
            var allemployees = _db.Employees.ToList();
            return View(allemployees);
        }
        //Get All the Departments
        public ActionResult DepartmentsList()
        {
            var alldepartments = _db.Departments.ToList();
            return View(alldepartments);
        }
        //Add New Employee
        public ActionResult AddNewEmployee()
        {
            return View();
        }
        //Update Departments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDepartmentDetails(int id,DepartmentModel model)
        {
            var department = _db.Departments.Single(c => c.DepartmentId == id);
            department.DepartmentCode = model.DepartmentCode;
            department.DepartmentName = model.DepartmentName;
            _db.SaveChanges();
            TempData["success"] = "The Department Details was successfully Updated";
            return View();
        }

        //Add New Department
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Departments(DepartmentModel _departments)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewDeparments", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DepartmentCode", _departments.DepartmentCode);
            com.Parameters.AddWithValue("@DepartmentName", _departments.DepartmentName);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                TempData["success"] = "The Department Details has been successfully submitted";

            }
            else
            {

                TempData["error"] = "The Department Details was not submitted successfully";
            }

            return View();
        }
        public ActionResult Departments()
        {
            return View();
        }
        //Add New User View
        public ActionResult AddNewUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UsersModel _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.EmailAddress == _user.EmailAddress);
                if (check == null)
                {
                    //Encrypt the Password
                    _user.Password = GetMD5(_user.Password);

                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(_user);

                    //Save all details
                    _db.SaveChanges();
                    TempData["success"] = "The User Account with the given Details has been Created Kindly proceed to Login";
                }
                else
                {
                    TempData["error"] = "The Email Address Provided already exists";
                }
            }
            return View();
        }
        //To Add Employee details    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewEmployee(EmployeeModel _employee)
        {

            connection();
            SqlCommand com = new SqlCommand("AddNewEmployeeDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@FirstName", _employee.FirstName);
            com.Parameters.AddWithValue("@LastName", _employee.LastName);
            com.Parameters.AddWithValue("@MiddleName", _employee.MiddleName);
            com.Parameters.AddWithValue("@Address", _employee.Address);
            com.Parameters.AddWithValue("@Gender", _employee.Gender);
            com.Parameters.AddWithValue("@EmailAddress", _employee.EmailAddress);
            com.Parameters.AddWithValue("@MobileNumber", _employee.MobileNumber);
            com.Parameters.AddWithValue("@Position", _employee.Position);
            com.Parameters.AddWithValue("@JoiningDate", _employee.JoiningDate);
            com.Parameters.AddWithValue("@IDNO", _employee.IDNO);
            com.Parameters.AddWithValue("@EmployeeNo", _employee.EmployeeNo);
            com.Parameters.AddWithValue("@Age", _employee.Age);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                TempData["success"] = "The Employee Details has been successfully submitted";

            }
            else
            {

                TempData["error"] = "The Employee Details was not submitted successfully";
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewUser(UsersModel _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.EmailAddress == _user.EmailAddress);
                if (check == null)
                {
                    //Encrypt the Password
                    _user.Password = GetMD5(_user.Password);

                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(_user);

                    //Save all details
                    _db.SaveChanges();
                    TempData["success"] = "The User Account with the given Details has been Created";
                }
                else
                {
                    TempData["error"] = "The Email Address Provided already exists";
                }
            }
            return View();
        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string emailaddress, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = _db.Users.Where(s => s.EmailAddress.Equals(emailaddress) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().EmailAddress;
                    Session["idUser"] = data.FirstOrDefault().Userid;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "The Email Address or Password provided is incorrect. Kindly try Again with the Correct Credentials";
                    return RedirectToAction("Login", "Home");
                }
            }
            return View();
        }
        public class CountriesRepository
        {
            public IEnumerable<SelectListItem> GetDepartments()
            {
                using (var context = new DB_Entities())
                {
                    List<SelectListItem> departments = context.Departments.AsNoTracking()
                        .OrderBy(n => n.DepartmentCode)
                            .Select(n =>
                            new SelectListItem
                            {
                                Value = n.DepartmentCode.ToString(),
                                Text = n.DepartmentName
                            }).ToList();
                    var departmentstip = new SelectListItem()
                    {
                        Value = null,
                        Text = "--- select Departments ---"
                    };
                    departments.Insert(0, departmentstip);
                    return new SelectList(departments, "Value", "Text");
                }
            }
        }
        public ActionResult UpdateEmployeeDetails(int id)
        {
            var allemployees = _db.Employees.Single(c => c.EmployeeId ==id);
            return View(allemployees);
        }
        
        public ActionResult DeleteDepartments(int? id)
        {
            var alldepartments = _db.Departments.Single(c => c.DepartmentId == id);
            return View(alldepartments);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDepartments(int id)
        {
            DepartmentModel allDepartments = _db.Departments.SingleOrDefault(c => c.DepartmentId == id);
            _db.Departments.Remove(allDepartments);
            _db.SaveChanges();
            TempData["success"] = "The Department Details was successfully Deleted";
            return RedirectToAction("DepartmentsList");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEmployee(string id)
        {
            EmployeeModel employee = _db.Employees.Find(id);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
            TempData["success"] = "The Employee Details was successfully Deleted";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateEmployeeDetails(int id, EmployeeModel model)
        {
            var employee = _db.Employees.Single(c => c.EmployeeId == id);
            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Address = model.Address;
            employee.Position = model.Position;
            employee.EmailAddress = model.EmailAddress;
            employee.JoiningDate = model.JoiningDate;
            employee.EmployeeNo = model.EmployeeNo;
            employee.IDNO = model.IDNO;
            employee.Gender = model.Gender;
            employee.MobileNumber = model.MobileNumber;
            employee.Age = model.Age;
            _db.SaveChanges();
            TempData["success"] = "The Employee Details was successfully Updated";
            return View();
        }
        public ActionResult UpdateDepartmentDetails(int id)
        {
            var alldepatmentdetails = _db.Departments.Single(c => c.DepartmentId == id);
            return View(alldepatmentdetails);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ResetPassword()
        {

            return View();
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.AppendHeader("Cache-Control", "no-store");
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}