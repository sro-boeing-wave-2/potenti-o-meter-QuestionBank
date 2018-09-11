using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Admin.Models
{

    [BsonKnownTypes(typeof(MCQType), typeof(MMCQType), typeof(FillBlanks), typeof(TrueFalse))]
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuestionId { get; set; }

        [BsonElement("domain")]
        [JsonProperty("domain")]
        public string Domain { get; set; }

        [BsonElement("difficultyLevel")]
        [JsonProperty("difficultyLevel")]
        public int DifficultyLevel { get; set; }

        [BsonElement("conceptTags")]
        [JsonProperty("conceptTags")]
        public string[] ConceptTags;

        [BsonElement("questionType")]
        [JsonProperty("questionType")]
        public string questionType { get; set; }

        //Question()
        //{ if(questionType=="MMCQType")
        //    {
        //        MMCQType mmcq = new MMCQType();
        //    }
        //}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class Options
    {

       [BsonElement]
        public string Option { get; set; }
       
    }
    public class correctOption
    {
       
        [BsonElement]
        public string CorrectOption { get; set; }
    }

    [BsonDiscriminator("MCQType")]
    public class MCQType : Question
    {
        [BsonElement("questionText")]
        public string questionText { get; set; }
        [BsonElement("correctOption")]
        public string correctOption { get; set; }
        [BsonRequired]
        public List<Options> OptionList { get; set; }
    }

    [BsonDiscriminator("MMCQType")]
    public class MMCQType : Question
    {
        [BsonElement("questionText")]
        [JsonProperty("questionText")]
        public string questionText { get; set; }
        
        [BsonElement("correctOptionList")]
        [JsonProperty("correctOptionList")]
        public List<correctOption> CorrectOptionList { get; set; }
        
        [BsonRequired]
        [JsonProperty("optionList")]
        public List<Options> OptionList { get; set; }
    }

    [BsonDiscriminator("fillBlanks")]
    public class FillBlanks : Question
    {
       [BsonElement("questionText")]
       public string questionText { get; set; }
       
       [BsonElement("correctResponse")]
       public string correctResponse { get; set; }
       
       [BsonRequired]
       public string Input { get; set; }
    }

    [BsonDiscriminator("trueFalse")]
    public class TrueFalse : Question
    {        
        [BsonElement("questionText")]
        public string questionText { get; set; }
        
        [BsonElement("correctOption")]
        public string correctOption { get; set; }
        
        [BsonRequired]
        public List<Options> OptionList { get; set; }
    }
}
