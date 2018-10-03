using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
	public class QuestionConceptMap
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string ID { get; set; }
		public double Version { get; set; }
		public string Domain { get; set; }
		public QuestionConceptTriplet[] questionconceptTriplet { get; set; }
		public ConceptTriplet[] concepttriplet { get; set; }
		public ContentTriplet[] contentConceptTriplet { get; set; }
		public string[] questionIds { get; set; }
		public string[] concepts { get; set; }


	}
	public class QuestionConceptTriplet
	{
		public Concept Source { get; set; }
		public Question Target { get; set; }
		public Predicate Relationship { get; set; }


	}
	public class ConceptTriplet
	{
		public Concept Source { get; set; }
		public Concept Target { get; set; }
		public Predicate Relationship { get; set; }


	}
	public class Content
	{

		public string Url { get; set; }
		public string[] Tags { get; set; }

	}
	public class ContentTriplet
	{
		public Content Source { get; set; }
		public Concept Target { get; set; }
		public ContentRelationship Relationship { get; set; }
	}
	public class ContentRelationship
	{
		public string Name { get; set; }
		public string Taxonomy { get; set; }

	}

	public class Predicate
	{
		public string Name { get; set; }
	}

	public class Question
	{
		public string QuestionId { get; set; }
		
	}

	public class Concept
	{
		public string Name { get; set; }
		public string Domain { get; set; }
	}
}
