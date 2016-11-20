using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExerciseTWJG.Models
{
    public class clsVertex
    {

        public string Name { get; set; }
        public bool Estado { get; set;  }
        public List<clsEdge> lstEdge { get; set; }
    }
}