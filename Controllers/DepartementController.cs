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
    public class DepartementController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartementController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            var dbList = dbClient.GetDatabase("testDB").GetCollection<Departement>("Departement").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Departement dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            int lastDep = dbClient.GetDatabase("testDB").GetCollection<Departement>("Departement").AsQueryable().Count();
            dep.dep_id = lastDep + 1;

            dbClient.GetDatabase("testDB").GetCollection<Departement>("Departement").InsertOne(dep);

            return new JsonResult("Added successfully");

        }

        [HttpPut]
        public JsonResult Put(Departement dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            var filter = Builders<Departement>.Filter.Eq("dep_id", dep.dep_id);

            var update = Builders<Departement>.Update.Set("dep_name", dep.dep_name);

            dbClient.GetDatabase("testDB").GetCollection<Departement>("Departement").UpdateOne(filter, update);

            return new JsonResult("Updated successfully");

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppConn"));

            var filter = Builders<Departement>.Filter.Eq("dep_id", id);

            dbClient.GetDatabase("testDB").GetCollection<Departement>("Departement").DeleteOne(filter);

            return new JsonResult("Deleted successfully");

        }
    }
}