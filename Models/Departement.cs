using MongoDB.Bson;

namespace WebApp_AngularDotNetMongoDB.Models
{
    public class Departement
    {
        public ObjectId Id { get; set; }
        public int dep_id { get; set; }
        public string dep_name { get; set; }
    }
}
