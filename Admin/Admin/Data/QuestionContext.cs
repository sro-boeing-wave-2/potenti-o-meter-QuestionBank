using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Microsoft.Extensions.Options;
using Admin.Models;
using Potentiometer.Core.QuestionTypes;
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
                BsonSerializer.LookupSerializer(typeof(MCQ));
                BsonSerializer.LookupSerializer(typeof(MMCQ));
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to MongoDb server.", ex);
            }

        }

        public IMongoCollection<IQuestion> Questions => _database.GetCollection<IQuestion>("Question");
		public IMongoCollection<QuestionConceptMap> QuestionConceptMap
		{
			get
			{
				return _database.GetCollection<QuestionConceptMap>("QuestionConceptMap");
			}
		}

	}
    
    
}
