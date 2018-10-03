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
	public class QuestionServices : IQuestionServices
	{
		private readonly QuestionContext _context = null;
		public QuestionServices(IOptions<Settings> settings)
		{
			_context = new QuestionContext(settings);
		}
		public async Task<List<IQuestion>> GetAllQuestions()
		{
			return await _context.Questions.Find(x => true).ToListAsync();

		}

		public async Task<List<String>> GetAllDomain()
		{
			// List<string> domain = new List<string>();

			var context = await _context.Questions.Find(x => true).ToListAsync();
			var domain = context.Select(x => x.Domain).Distinct().ToList();
			return domain;
		}

		public async Task<List<IQuestion>> GetAllQuestionsByDomain(string domain)
		{
			var context = await _context.Questions.Find(x => true).ToListAsync();
			return context.Where(a => a.Domain == domain).ToList();

		}
		public async Task<List<IQuestion>> GetAllQuestionsByConceptTag(string concepttag, string domain)
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
			var c = await GetDatabyVersionandDomainAsync(question.Domain);
			QuestionConceptMap latestConceptMap = JsonConvert.DeserializeObject<QuestionConceptMap>(c.ToString());
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
		public QuestionConceptMap CreateQuestionConceptMap(QuestionConceptMap questionConceptMap)
		{
			_context.QuestionConceptMap.InsertOneAsync(questionConceptMap);
			return questionConceptMap;
		}

		public async Task<List<QuestionConceptMap>> GetDatabyVersionandDomainAsync(string domain)
		{
			var result1 = await _context.QuestionConceptMap.Find(x => x.Domain == domain).ToListAsync();
			var version = result1.Select(x => x.Version).ToArray();
			double latestVersion = version.Max();
			var result = await _context.QuestionConceptMap.Find(x => x.Domain == domain && x.Version == latestVersion).ToListAsync();
			return result;
		}
        public async Task<List<IQuestion>> GetAllQuestionByIdlist(string[] questionid)
        {
            //return await _context.Questions.Find(x => x.QuestionId == questionid).ToListAsync();
            IQuestion addq;
            var questionlist = new List<IQuestion>();
            //Console.Write(string.IsNullOrEmpty(questionid.ToString()));
            //foreach(string x in questionid)
            //Console.WriteLine(string.IsNullOrEmpty(x));

            var context = await _context.Questions.Find(x => true).ToListAsync();
            foreach (string a in questionid)
            {
                addq = context.SingleOrDefault(b => b.QuestionId == a);
                questionlist.Add(addq);
            }
            return questionlist;

        }
    }
}
