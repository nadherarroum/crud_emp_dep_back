using MongoDB.Bson;

namespace WebApp_AngularDotNetMongoDB.Models
{
    public class Employee
    {
        public ObjectId Id { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; }
        public string Departement { get; set; }
        public string emp_dateOfJoin { get; set; }
        public string emp_photo { get; set; }
    }
}
