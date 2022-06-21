

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using WebApplicationcoredurgesh.DataConnection;
using WebApplicationcoredurgesh.Models;

namespace WebApplicationcoredurgesh.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        Uri apiUrl = new Uri("http://localhost:22043/api/Emp/");
        HttpClient Client;
        //public object CookiAuthenticatonDefaults { get; private set; }

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
            Client = new HttpClient();
            Client.BaseAddress = apiUrl;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> mobj = new List<Employee>();
            HttpResponseMessage listmobj = Client.GetAsync(Client.BaseAddress + "Get/Emp").Result;
            if (listmobj.IsSuccessStatusCode)
            {
             

                string data = listmobj.Content.ReadAsStringAsync().Result;
                
                var res = JsonConvert.DeserializeObject<List<Employee>>(data);

                foreach (var item in res)
                {
                    mobj.Add(new Employee
                    {
                        id = item.id,
                        name = item.name,
                        sal = item.sal,
                        gmail = item.gmail,
                        password = item.password
                    });
                }

            }
            return View(mobj);
           // var res = _db.Employees.ToList();
            //return View(res);
        }
        [HttpGet]
        public IActionResult Addemp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Addemp(Employee obj)
        {
            string data = JsonConvert.SerializeObject(obj);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "Post/Emp/add", content).Result;
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            //if (obj.id == 0)
            //{
            //    _db.Employees.Add(obj);

            //    _db.SaveChanges();

            //}
            //else
            //{
            //    //_db.Employee.Update(obj);
            //    //_db.SaveChanges();
            //    _db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //    _db.SaveChanges();
            //}

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var res = Client.DeleteAsync(Client.BaseAddress + "Post/Emp/delete" + '?' + "id" + "=" + id.ToString()).Result;
            //var delete = _db.Employees.Where(f => f.id == id).FirstOrDefault();
            //_db.Employees.Remove(delete);
            //_db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var res = Client.GetAsync(Client.BaseAddress + "Post/Emp/Edit" + '?' + "id" + "=" + id.ToString()).Result;
            string data = res.Content.ReadAsStringAsync().Result;
            var emp = JsonConvert.DeserializeObject<Employee>(data);
            //var Edit = _db.Employees.Where(f => f.id == id).FirstOrDefault();
            //_db.Employees.Remove(Edit);
            

            return View ("Addemp" , emp);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(User_Info uobj)
        {

            var d = _db.User_Infos.Where(m => m.gmail == uobj.gmail).FirstOrDefault();
            if (d == null)
            {
                TempData["gmail"] = "Gmail not found";
            }
            else
            {
                if (d.gmail == uobj.gmail && d.name == uobj.name)
                {
                    var claims = new [] { new Claim(ClaimTypes.Name, d.name), new Claim(ClaimTypes.Email, d.gmail) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["name"] = "name not found";

                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Sign_Up()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Sign_Up(User_Info obj3)
        {

            _db.User_Infos.Add(obj3);
            _db.SaveChanges();
            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.LoginPath);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Mainpage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
