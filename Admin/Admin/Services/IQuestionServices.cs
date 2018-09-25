using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using Potentiometer.Core.QuestionTypes;
namespace Admin.Services
{
    public interface IQuestionServices
    {
        Task<List<IQuestion>> GetAllQuestions();
        Task<List<IQuestion>> GetAllQuestionById(string questionid);
        Task<List<IQuestion>> GetAllQuestionsByDomain(string domain);
        Task<List<IQuestion>> GetAllQuestionsByDifficultyLevel(int difficultylevel);
        Task<List<String>> GetAllDomain();
        Task<IQuestion> AddQuestion(IQuestion question);
        Task<bool> DeleteQuestionById(string id);
        Task<bool> DeleteQuestionByDomain(string domain);
        Task EditQuestion(string id, IQuestion question);
	    QuestionConceptMap CreateQuestionConceptMap();
		Task<List<IQuestion>> GetAllQuestionsByConceptTag(string concepttag,string domain);
		Task<List<QuestionConceptMap>> GetDatabyVersionandDomainAsync( string domain);
		
	}
}
