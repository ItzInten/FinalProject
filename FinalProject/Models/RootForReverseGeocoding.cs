using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Models
{
    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public List<double> bbox { get; set; }
    }
    public class Properties
    {
        public string name { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
    }

    public class RootForReverseGeocoding
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
    }
}
