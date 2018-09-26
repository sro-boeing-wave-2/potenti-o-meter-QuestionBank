using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Admin.Data;
using Newtonsoft.Json;
using Potentiometer.Core.QuestionTypes;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Admin.Services
{
    public class QuestionServices: IQuestionServices
    {
        private readonly QuestionContext _context = null;
        public QuestionServices(IOptions<Settings> settings)
        {
            _context = new QuestionContext(settings);
        }
        public async Task<List<IQuestion>> GetAllQuestions()
        {
            return await _context.Questions.Find(x =>true).ToListAsync();

        }

        public async Task<List<String>> GetAllDomain() 
        {
           // List<string> domain = new List<string>();
            
            var context=await _context.Questions.Find(x => true).ToListAsync();
            var domain = context.Select(x => x.Domain).Distinct().ToList();
            return domain;
        }

        public async Task<List<IQuestion>> GetAllQuestionsByDomain(string domain)
        {
			var context = await _context.Questions.Find(x => true).ToListAsync();
			return context.Where(a => a.Domain == domain).ToList();

		}
		public async Task<List<IQuestion>> GetAllQuestionsByConceptTag(string concepttag,string domain)
		{
			var context = await _context.Questions.Find(x => true).ToListAsync();
			return context.Where(a => a.ConceptTags.Contains(concepttag) && a.Domain == domain).ToList();
			//return await _context.Questions.Find(x => x.DifficultyLevel == difficultylevel).ToListAsync();
		}
		public async Task<List<IQuestion>> GetAllQuestionById(string questionid)
        {
            //return await _context.Questions.Find(x => x.QuestionId == questionid).ToListAsync();
            var context = await _context.Questions.Find(x => true).ToListAsync();
            return context.Where(a => a.QuestionId == questionid).ToList();

        }

        public async Task<List<IQuestion>> GetAllQuestionsByDifficultyLevel(int difficultylevel)
        {
            return await _context.Questions.Find(x => x.DifficultyLevel == difficultylevel).ToListAsync();
        }

        public async Task<IQuestion> AddQuestion(IQuestion question)
        {
            await _context.Questions.InsertOneAsync(question);
            return question;
        }

        public async Task<bool> DeleteQuestionByDomain(string domain)
        {
            DeleteResult actionResult = await _context.Questions.DeleteManyAsync(Builders<IQuestion>.Filter.Eq("Domain", domain));
            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteQuestionById(string id)
        {
            ObjectId objid = new ObjectId(id);
            DeleteResult actionResult = await _context.Questions.DeleteOneAsync(Builders<IQuestion>.Filter.Eq("_id", objid));
            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }

        public async Task EditQuestion(string id, IQuestion question)
        {
            ObjectId objid = new ObjectId(id);
            var filter = Builders<IQuestion>.Filter.Eq("_id", objid);
            var result = await _context.Questions.FindOneAndReplaceAsync(filter, question);
        }
		public QuestionConceptMap CreateQuestionConceptMap()
		{
			QuestionConceptMap questionConceptMap = new QuestionConceptMap();
			string consulIP = Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
			var factory = new ConnectionFactory() { HostName = consulIP, UserName = "preety", Password = "preety", Port = 5672 };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				//channel.QueueDeclare(queue: "Concepts",
				//						 durable: false,
				//						 exclusive: false,
				//						 autoDelete: false,
				//						 arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				bool noAck = false;
				BasicGetResult result = channel.BasicGet("Concepts", noAck);
				if (result == null)
				{
					Console.WriteLine("message can't be retrieved from queue");
				}
				else
				{
					IBasicProperties props = result.BasicProperties;
					var body = Encoding.UTF8.GetString(result.Body);
					ConceptMapData conceptmap = JsonConvert.DeserializeObject<ConceptMapData>(body);
					questionConceptMap.Domain = conceptmap.Domain;
					questionConceptMap.Version = conceptmap.Version;
					questionConceptMap.concepttriplet = conceptmap.Triplet;
					questionConceptMap.concepts = conceptmap.Concepts;
					List<QuestionConceptTriplet> t = new List<QuestionConceptTriplet>();
					
					foreach (string concept in conceptmap.Concepts)
					{
						var questionbyConcept = GetAllQuestionsByConceptTag(concept,conceptmap.Domain).Result;
						
						
						foreach (IQuestion question in questionbyConcept)
						{
							
							QuestionConceptTriplet t1 = new QuestionConceptTriplet();

							Concept source = new Concept();
							Question target = new Question();
							Predicate relationship = new Predicate();
							source.Name = concept;
							source.Domain = conceptmap.Domain;
							target.QuestionId = question.QuestionId;
							relationship.Name = question.Taxonomy;
							t1.Source = source;
							t1.Target = target;
							t1.Relationship = relationship;
							t.Add(t1);

							
							
						}
					}
					questionConceptMap.questionconceptTriplet = t.ToArray();
					
					var questionbydomain = GetAllQuestionsByDomain(conceptmap.Domain).Result;
					List<String> questionIds = new List<String>();
					
					foreach (IQuestion quest in questionbydomain)
					{
						questionIds.Add(quest.QuestionId);
						

					}
					questionConceptMap.questionIds = questionIds.ToArray();
					_context.QuestionConceptMap.InsertOneAsync(questionConceptMap);
					channel.BasicConsume(queue: "Concepts", autoAck: true, consumer: consumer);
				}
				
				Console.WriteLine(questionConceptMap);

			}
			return questionConceptMap;
		}

		public async Task<List<QuestionConceptMap>> GetDatabyVersionandDomainAsync(string domain)
		{
			var result= await _context.QuestionConceptMap.Find(x => true ).ToListAsync();
			return result;
		}
	}
}
