using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExerciseTWJG.Models
{
    public class clsGrafo
    {

        public List<clsVertex> lstVertex { get; set; }

        public void addEdge(clsEdge oEdge)
        {
            this.lstVertex.Where(x => x.Name.Equals(oEdge.Vertex1.ToString())).FirstOrDefault().lstEdge.Add(oEdge);
        }
    }
}