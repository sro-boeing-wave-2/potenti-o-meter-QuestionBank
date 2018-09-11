using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Services;
using Admin.Models;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionServices _questionService;
        public QuestionsController(IQuestionServices questionService)
        {
            _questionService = questionService;
            
            
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
             var questions = await _questionService.GetAllQuestions();
            return Ok(questions);
        }

        [HttpGet("domain")]
        public async Task<IActionResult> GetDomain()
        {
            var domainlist = await _questionService.GetAllDomain();
            return Ok(domainlist);
           
        }

        [HttpGet("domain/{domain}")]
        public async Task<IActionResult> GetQuestionsDomain([FromRoute] string domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //return await _questionService.GetAllQuestionsByDomain(domain);
            var questions = await _questionService.GetAllQuestionsByDomain(domain);
            return Ok(questions);

        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetQuestionId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //return await _questionService.GetAllQuestionsByDomain(domain);
            var questions = await _questionService.GetAllQuestionById(id);
            return Ok(questions);

        }

        [HttpGet("difficultylevel/{difficultylevel}")]
        public async Task<IActionResult> GetQuestionsDomain([FromRoute] int difficultylevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var questions= await _questionService.GetAllQuestionsByDifficultyLevel(difficultylevel);
            return Ok(questions);

        }

        [HttpPost]
        public async Task<IActionResult> PostQuestion([FromBody] dynamic question) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var questionAsJsonString = JsonConvert.SerializeObject(question);
            string type = question.questionType;
            switch (type)
            {
                case "MCQType":     
                        MCQType mcqType = JsonConvert.DeserializeObject<MCQType>(questionAsJsonString);
                       await _questionService.AddQuestion(mcqType);
                    break;

                case "MMCQType":
                    
                    MMCQType mmcqType = JsonConvert.DeserializeObject<MMCQType>(questionAsJsonString);
                   await _questionService.AddQuestion(mmcqType);
                    break;

                case "FillBlanks":

                    FillBlanks fillBlanks = JsonConvert.DeserializeObject<FillBlanks>(questionAsJsonString);
                    await _questionService.AddQuestion(fillBlanks);
                    break;
                    
                default:
                    TrueFalse trueFalse = JsonConvert.DeserializeObject<TrueFalse>(questionAsJsonString);
                    await _questionService.AddQuestion(trueFalse);
                    break;
            }
            return Ok(question);
        }

        [HttpDelete("id/{id}")]
        public async Task<IActionResult> DeleteQuestionId([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _questionService.DeleteQuestionById(id);
            if (result == false)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("{domain}")]
        public async Task<IActionResult> DeleteQuestionDomain([FromRoute] string domain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _questionService.DeleteQuestionByDomain(domain);
            if (result == false)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion([FromRoute] string id, [FromBody] dynamic question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var questionAsJsonString = JsonConvert.SerializeObject(question);
            string type = question.questionType;
            switch (type)
            {
                case "MCQType":
                    MCQType mcqType = JsonConvert.DeserializeObject<MCQType>(questionAsJsonString);
                    mcqType.QuestionId = id;
                    await _questionService.EditQuestion(id, mcqType);
                    break;

                case "MMCQType":

                    MMCQType mmcqType = JsonConvert.DeserializeObject<MMCQType>(questionAsJsonString);
                    mmcqType.QuestionId = id;
                    await _questionService.EditQuestion(id, mmcqType);
                    break;

                case "FillBlanks":

                    FillBlanks fillBlanks = JsonConvert.DeserializeObject<FillBlanks>(questionAsJsonString);
                    fillBlanks.QuestionId = id;
                    await _questionService.EditQuestion(id, fillBlanks);
                    break;

                default:
                    TrueFalse trueFalse = JsonConvert.DeserializeObject<TrueFalse>(questionAsJsonString);
                    trueFalse.QuestionId = id;
                    await _questionService.EditQuestion(id, trueFalse);
                    break;
            }



            return Ok(question);
        }
    }
}