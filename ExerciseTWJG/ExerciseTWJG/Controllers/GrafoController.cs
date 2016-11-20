using ExerciseTWJG.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExerciseTWJG.Controllers
{
    public class GrafoController : Controller
    {
        // GET: Grafo
        public ActionResult Index()
        {

            clsGrafo obGrafo = new clsGrafo();
            return View(obGrafo);
        }

        public ActionResult InformacionConsulta(string file)
        {

            clsGrafo obGrafo = new clsGrafo();

            try
            {

                if (string.IsNullOrEmpty(file))
                {
                    throw new Exception("Problem occurred with file upload");
                }
                string archivo = file;
                var allowedVertex = new[] { "A", "B", "C", "D", "E" };
                List<clsVertex> lstVertex = new List<clsVertex>();


                #region GENERACION LISTA ADYACENTE

                string[] lstCordenadas = archivo.Trim().ToUpper().Split(',');
                char[] lstVertexName = archivo.ToUpper().Where(x => char.IsLetter(x)).Distinct().OrderBy(x => x).ToArray();



                foreach (var item in lstVertexName)
                {

                    if (allowedVertex.Contains(item.ToString()))
                        obGrafo.addVertex(new clsVertex { Name = item.ToString(), lstEdge = new List<clsEdge>(), Estado = false });
                    else

                        throw new Exception(string.Format("Can not be added to a vertex with the name: { 0 }", item.ToString()));

                }

                foreach (var item in lstCordenadas)
                {

                    if (!char.IsNumber(item[0]) && !char.IsNumber(item[1]) && char.IsNumber(item[2]) && !item[0].Equals(item[1]))
                    {
                        obGrafo.addEdge(new clsEdge { Vertex1 = item[0], Vertex2 = item[1], Cost = float.Parse(item[2].ToString()) });
                    }
                    else
                    {
                        throw new Exception(string.Format("Coordinate: {0} is incorrect, check the rules", item.ToString()));
                    }

                }

                #endregion FIN GENERACION LISTA ADYACENTE      
            }
            catch (Exception ex)
            {
                obGrafo.MensajeError = ex.Message;
                return View(obGrafo);
            }




            #region COSNTRUCCION SALIDA        

            List<clsQuery> lstSalida = new List<clsQuery>();

            lstSalida.Add(new clsQuery
            {
                question = "The distance of the route A-B-C",
                answer = obGrafo.getDistance("ABC")
            });

            lstSalida.Add(new clsQuery
            {
                question = "The distance of the route A-D",
                answer = obGrafo.getDistance("AD")
            });

            lstSalida.Add(new clsQuery
            {
                question = "The distance of the route A-D-C	",
                answer = obGrafo.getDistance("ADC")
            });

            lstSalida.Add(new clsQuery
            {
                question = "The distance of the route A-E-B-C-D",
                answer = obGrafo.getDistance("AEBCD")
            });

            lstSalida.Add(new clsQuery
            {
                question = "The distance of the route A-E-D	",
                answer = obGrafo.getDistance("AED")
            });

            List<clsCamino> Item6 = new List<clsCamino>();
            obGrafo.getRouteByStops(obGrafo.lstVertex, "C", "C", 4, "MAX", Item6, string.Empty);
            string item6 = Item6.Count().ToString();

            lstSalida.Add(new clsQuery
            {
                question = "The number of trips starting at C and ending at C with a maximum of 3 stops",
                answer = item6
            });

            List<clsCamino> Item7 = new List<clsCamino>();
            obGrafo.getRouteByStops(obGrafo.lstVertex, "A", "C", 5, "EQUAL", Item7, string.Empty);
            string item7 = Item7.Count().ToString();

            lstSalida.Add(new clsQuery
            {
                question = "The number of trips starting at A and ending at C with exactly 4 stops.",
                answer = item7
            });


            List<clsCamino> Item8 = new List<clsCamino>();
            obGrafo.getRoutes(obGrafo.lstVertex, "A", "C", Item8, string.Empty);
            string item8 = Item8.Min(x => x.costo).ToString();

            lstSalida.Add(new clsQuery
            {
                question = "The length of the shortest route (in terms of distance to travel) from A to C.",
                answer = item8
            });


            List<clsCamino> Item9 = new List<clsCamino>();
            obGrafo.getRoutes(obGrafo.lstVertex, "B", "B", Item9, string.Empty);
            string item9 = Item9.Min(x => x.costo).ToString();

            lstSalida.Add(new clsQuery
            {
                question = "The length of the shortest route (in terms of distance to travel) from B to B.	",
                answer = item9
            });

            obGrafo.lstQuerys = lstSalida;
            #endregion FIN CONSTRUCCION SALIDA

            return View(obGrafo);

            //List<clsCamino> Item8 = new List<clsCamino>();
            //obGrafo.getRouteByCost(obGrafo.lstVertex, "C", "C", 30, "MAX", Item8, string.Empty);
            //string item8 = Item8.Count().ToString();



        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {

            string result = string.Empty;

            if (file != null && file.ContentLength > 0)
            {
                result = new StreamReader(file.InputStream).ReadToEnd();
            }

            return RedirectToAction("InformacionConsulta", new { file = result });
        }

    }
}
