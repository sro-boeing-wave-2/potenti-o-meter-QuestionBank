using Admin.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Potentiometer.Core.QuestionTypes;

namespace Admin.Services
{
	public class ConceptMapListener
	{
		private readonly IQuestionServices _questionService;

		
		public ConceptMapListener(IQuestionServices questionService)
		{
			_questionService = questionService;	
	}
		public void CreateQuestionConceptMap()
		{
			QuestionConceptMap questionConceptMap = new QuestionConceptMap();
			string consulIP = Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
			var factory = new ConnectionFactory() { HostName = consulIP, UserName = "preety", Password = "preety", Port = 5672 };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: "Concepts",
								durable: false,
								exclusive: false,
								autoDelete: false,
								arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					var message = ea.Body;
					Console.WriteLine(message);
					var body = Encoding.UTF8.GetString(message);
					ConceptMapData conceptmap = JsonConvert.DeserializeObject<ConceptMapData>(body);
					questionConceptMap.Domain = conceptmap.Domain;
					questionConceptMap.Version = conceptmap.Version;
					questionConceptMap.concepttriplet = conceptmap.Triplet;
					questionConceptMap.concepts = conceptmap.Concepts;
					questionConceptMap.contentConceptTriplet = conceptmap.contentConceptTriplet;
					List<QuestionConceptTriplet> t = new List<QuestionConceptTriplet>();

					foreach (string concept in conceptmap.Concepts)
					{
						var questionbyConcept = _questionService.GetAllQuestionsByConceptTag(concept, conceptmap.Domain).Result;


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

					var questionbydomain = _questionService.GetAllQuestionsByDomain(conceptmap.Domain).Result;
					List<String> questionIds = new List<String>();

					foreach (IQuestion quest in questionbydomain)
					{
						questionIds.Add(quest.QuestionId);


					}
					questionConceptMap.questionIds = questionIds.ToArray();
					var resultdata= _questionService.CreateQuestionConceptMap(questionConceptMap);
					if (resultdata.Equals(questionConceptMap)) {
						channel.QueueDeclare(queue: "ConceptMap",
										 durable: false,
										 exclusive: false,
										 autoDelete: false,
										 arguments: null);
						string bodydata = JsonConvert.SerializeObject(questionConceptMap);
						channel.BasicPublish(exchange: "",
											 routingKey: "ConceptMap",
											 mandatory: true,
											 basicProperties: null,
											 body: Encoding.UTF8.GetBytes(bodydata));

					}


				};
				channel.BasicConsume(queue: "Concepts",
									 autoAck: true,
									 consumer: consumer);

				
			}
		}
	}
}
	
