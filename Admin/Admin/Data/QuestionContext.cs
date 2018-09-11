using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Admin.Models;
namespace Admin.Data
{
    public class QuestionContext
    {
        private readonly IMongoDatabase _database;
        public QuestionContext(IOptions<Settings> settings)
        {
            try
            {
                var client = new MongoClient(settings.Value.ConnectionString);
                if (client != null)
                    _database = client.GetDatabase(settings.Value.Database);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }

        }

        public IMongoCollection<Question> Questions => _database.GetCollection<Question>("Question");

    }
    
    
}
