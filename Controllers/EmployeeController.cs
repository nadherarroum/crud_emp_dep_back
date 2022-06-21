using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebApp_AngularDotNetMongoDB.Models;
using Microsoft.AspNetCore.Cors;

namespace WebApp_AngularDotNetMongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[EnableCors("MyCorsImplementationPolicy")]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            // Connect to mongo
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));
            //Get emp list
            var emps = dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").AsQueryable();
            //return emp list
            return new JsonResult(emps);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            int emps = dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").AsQueryable().Count();

            emp.emp_id = emps + 1;

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").InsertOne(emp);

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            var filter = Builders<Employee>.Filter.Eq("emp_id", emp.emp_id);

            var update = Builders<Employee>.Update.Set("emp_name", emp.emp_name)
                                                .Set("Departement", emp.Departement)
                                                .Set("emp_dateOfJoin", emp.emp_dateOfJoin)
                                                .Set("emp_photo", emp.emp_photo);

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").UpdateOne(filter, update);

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            var filter = Builders<Employee>.Filter.Eq("emp_id", id);

            dbClient.GetDatabase("testDB").GetCollection<Employee>("Employee").DeleteOne(filter);

            return new JsonResult("Deleted Successfully");
        }

        [Route("/savefile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpReq = Request.Form;
                var postedFile = httpReq.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);

            }
            catch (Exception)
            {
                return new JsonResult("anonymous.jpg");
            }
        }

    }


}