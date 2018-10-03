using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class ConceptMapData
    {
		public string ID { get; set; }
		public double Version { get; set; }
		public string Domain { get; set; }
		public ConceptTriplet[] Triplet { get; set; }
		public ContentTriplet[] contentConceptTriplet { get; set; }
		public String[] Concepts { get; set; }
	}
}
