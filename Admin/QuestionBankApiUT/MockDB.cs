using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Admin.Models;
using System.Linq;
namespace QuestionBankApiUT
{
    class MockDB
    {
        public async Task<Question> GetTestResultData()
        {
            var question = new MCQType
            {
                QuestionId = "1",
                Domain = "maths",
                DifficultyLevel = 10,
                ConceptTags = new string[] { "addition", "substraction" },
                questionType = "MCQType",
                questionText = "2+2=?",
                correctOption = "4",
                OptionList = new List<Options>
                {
                    new Options{Option="2"},
                    new Options{Option="3"},
                    new Options{Option="4"},
                    new Options{Option="10"}
                }
            };
            return await Task.FromResult(question);
        }

        public async Task<List<Question>> GetTestResultListAsync()
        {
            var questions = new List<Question>
        {
            new MCQType()
            {
                QuestionId = "1",
                Domain = "maths",
                DifficultyLevel = 10,
                ConceptTags = new string[] { "addition", "substraction" },
                questionType = "MCQType",
                questionText = "2+2=?",
                correctOption = "4",
                OptionList = new List<Options>
                {
                    new Options{Option="2"},
                    new Options{Option="3"},
                    new Options{Option="4"},
                    new Options{Option="10"}
                }
            },
            new MMCQType()
            {
                QuestionId = "1",
                Domain = "gk",
                DifficultyLevel = 10,
                ConceptTags = new string[] { "addition", "substraction" },
                questionType = "MCQType",
                questionText = "even number=?",
                CorrectOptionList = new List<correctOption>
                {
                    new correctOption{ CorrectOption="4"},
                    new correctOption{ CorrectOption="2"}
                },
                OptionList = new List<Options>
                {
                    new Options{Option="2"},
                    new Options{Option="3"},
                    new Options{Option="4"},
                    new Options{Option="11"}
                }
            }

        };
            return await Task.FromResult(questions);
        }

        public async Task<bool> DeleteTest()
        {
            return await Task.FromResult(true);
        }
    }
    


}
