using ExerciseTWJG.Models;
using System;
using System.Collections.Generic;
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

        public ActionResult EjecutarCalculos()
        {

            string archivo = "AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7";

            clsGrafo obGrafo = new clsGrafo();

            #region GENERACION LISTA ADYACENTE
            string[] lstCordenadas = archivo.Trim().ToUpper().Split(',');
            char[] lstVertexName = archivo.ToUpper().Where(x => char.IsLetter(x)).Distinct().OrderBy(x => x).ToArray();
            List<clsVertex> lstVertex = new List<clsVertex>();
            foreach (var item in lstVertexName)
            {
                lstVertex.Add(new clsVertex { Name = item.ToString(), lstEdge = new List<clsEdge>() });
            }

            obGrafo.lstVertex = lstVertex;

            foreach (var item in lstCordenadas)
            {

                if (!char.IsNumber(item[0]) && !char.IsNumber(item[1]) && char.IsNumber(item[2]) && !item[0].Equals(item[1]))
                {
                    obGrafo.addEdge(new clsEdge { Vertex1 = item[0], Vertex2 = item[1], Cost = item[2] });
                }

            }

            #endregion FIN GENERACION LISTA ADYACENTE



            return View(obGrafo);
        }

    }
}
