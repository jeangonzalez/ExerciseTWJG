using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExerciseTWJG;
using ExerciseTWJG.Controllers;
using ExerciseTWJG.Models;

namespace ExerciseTWJG.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Querys()
        {
            string archivo = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";

            clsGrafo obGrafo = new clsGrafo();

            #region GENERACION LISTA ADYACENTE
            string[] lstCordenadas = archivo.Trim().ToUpper().Split(',');
            char[] lstVertexName = archivo.ToUpper().Where(x => char.IsLetter(x)).Distinct().OrderBy(x => x).ToArray();
            List<clsVertex> lstVertex = new List<clsVertex>();
            foreach (var item in lstVertexName)
            {
                obGrafo.addVertex(new clsVertex { Name = item.ToString(), lstEdge = new List<clsEdge>(), Estado = false });
            }

            foreach (var item in lstCordenadas)
            {

                if (!char.IsNumber(item[0]) && !char.IsNumber(item[1]) && char.IsNumber(item[2]) && !item[0].Equals(item[1]))
                {
                    obGrafo.addEdge(new clsEdge { Vertex1 = item[0], Vertex2 = item[1], Cost = float.Parse(item[2].ToString()) });
                }

            }

            #endregion FIN GENERACION LISTA ADYACENTE                

            string item1 = obGrafo.getDistance("ABC");
            string item2 = obGrafo.getDistance("AD");
            string item3 = obGrafo.getDistance("ADC");
            string item4 = obGrafo.getDistance("AEBCD");
            string item5 = obGrafo.getDistance("AED");

            List<clsCamino> Item6 = new List<clsCamino>();
            obGrafo.getRouteByStops(obGrafo.lstVertex, "C", "C", 4, "MAX", Item6, string.Empty);
            string item6 = Item6.Count().ToString();

            List<clsCamino> Item7 = new List<clsCamino>();
            obGrafo.getRouteByStops(obGrafo.lstVertex, "A", "C", 5, "EQUAL", Item7, string.Empty);
            string item7 = Item7.Count().ToString();

            List<clsCamino> Item8 = new List<clsCamino>();
            obGrafo.getRoutes(obGrafo.lstVertex, "A", "C", Item8, string.Empty);
            string item8 = Item8.Min(x => x.costo).ToString();


            List<clsCamino> Item9 = new List<clsCamino>();
            obGrafo.getRoutes(obGrafo.lstVertex, "B", "B", Item9, string.Empty);
            string item9 = Item9.Min(x => x.costo).ToString();


        }


    }
}
