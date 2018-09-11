using System;
using Xunit;
using Admin.Services;
using Moq;
using Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Admin.Models;
using System.Linq;
namespace QuestionBankApiUT
{
    public class QuestionBankUnitTest
    {
        private readonly  QuestionsController controller;
        private readonly Mock<IQuestionServices> moqservice;
        public QuestionBankUnitTest()
        {
            moqservice = new Mock<IQuestionServices>();
            controller = new QuestionsController(moqservice.Object);

        }

        [Fact]
        public async void TestGetAllQuestions()
        {
            
            MockDB mockDbHelper = new MockDB();
            moqservice.Setup(x => x.GetAllQuestionsByDomain("maths")).Returns(mockDbHelper.GetTestResultListAsync());
            IActionResult result = await controller.GetQuestionsDomain("maths");
            OkObjectResult objectResult = result as OkObjectResult;
            List<Question> objectResultValue = objectResult.Value as List<Question>;
            Assert.Equal(2, objectResultValue.Count);
        }


        [Fact]
        public async void TestGetAllQuestionsByDomain()
        {
            MockDB mockDbHelper = new MockDB();
            moqservice.Setup(x => x.GetAllQuestionsByDomain("maths")).Returns(mockDbHelper.GetTestResultListAsync());
           
            IActionResult result = await controller.GetQuestionsDomain("maths");
            OkObjectResult objectResult = result as OkObjectResult;
            List<Question> objectResultValue = objectResult.Value as List<Question>;
            Assert.Equal(2, objectResultValue.Count);
        }
        [Fact]
        public async void TestAddQuestions()
        {
            MockDB mockDbHelper = new MockDB();
            Question add = await mockDbHelper.GetTestResultData();
            moqservice.Setup(x => x.AddQuestion(add)).Returns(mockDbHelper.GetTestResultData());

            IActionResult result = await controller.PostQuestion(add);
            OkObjectResult objectResult = result as OkObjectResult;
            MCQType objectResultValue = objectResult.Value as MCQType;
            Assert.Equal("4", objectResultValue.correctOption);
        }
        [Fact]
        public async void TestUpdateQuestions()
        {
            MockDB mockDbHelper = new MockDB();
            Question add = await mockDbHelper.GetTestResultData();
            moqservice.Setup(x => x.EditQuestion(add.QuestionId, add)).Returns(mockDbHelper.GetTestResultData());

            IActionResult result = await controller.PutQuestion(add.QuestionId, add);
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteQuestionsByid()
        {
            MockDB mockDbHelper = new MockDB();
            Question add = await mockDbHelper.GetTestResultData();
           moqservice.Setup(x => x.DeleteQuestionById(add.QuestionId)).Returns(mockDbHelper.DeleteTest());

            IActionResult result = await controller.DeleteQuestionId(add.QuestionId);
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async void TestDeleteQuestionsByDomain()
        {
            MockDB mockDbHelper = new MockDB();
            Question add = await mockDbHelper.GetTestResultData();
            moqservice.Setup(x => x.DeleteQuestionByDomain(add.QuestionId)).Returns(mockDbHelper.DeleteTest());

            IActionResult result = await controller.DeleteQuestionDomain(add.QuestionId);
            OkObjectResult objectResult = result as OkObjectResult;

            Assert.Equal(200, objectResult.StatusCode);
        }
    }
}
