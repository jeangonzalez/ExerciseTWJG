using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExerciseTWJG.Models
{
    public class clsGrafo : clsBase
    {
        public clsGrafo()
        {
            this.lstVertex = new List<clsVertex>();
        }
        public List<clsVertex> lstVertex { get; set; }

        public List<clsQuery> lstQuerys { get; set; }

        public void addVertex(clsVertex oVertex)
        {
            this.lstVertex.Add(oVertex);
        }

        public void addEdge(clsEdge oEdge)
        {
            this.lstVertex.Where(x => x.Name.Equals(oEdge.Vertex1.ToString())).FirstOrDefault().lstEdge.Add(oEdge);
        }


        /// <summary>
        /// Returns de sumatory of cost vertex
        /// </summary>
        /// <param name="route"></param>
        /// <returns>distance</returns>
        public string getDistance(string route)
        {
            string distance = string.Empty;
            clsEdge edge = new clsEdge();
            float total = 0;
            List<clsEdge> edges = new List<clsEdge>();
            for (int i = 0; i < route.Length - 1; i++)
            {
                edge = this.lstVertex.Where(x => x.Name.Equals(route[i].ToString())).FirstOrDefault().lstEdge.Where(x => x.Vertex2.Equals(route[i + 1])).FirstOrDefault();

                if (edge != null)
                {
                    edges.Add(edge);
                    total += edge.Cost;
                }
                else
                {
                    distance = "NO SUCH ROUTE";
                    break;
                }
                distance = total.ToString();
            }

            return distance;
        }

        /// <summary>
        /// Get the routes with stops conditions
        /// </summary>
        /// <param name="lstvertex"></param>
        /// <param name="origin"></param>
        /// <param name="destiny"></param>
        /// <param name="stopsNumber"></param>
        /// <param name="condition"></param>
        /// <param name="caminos"></param>
        /// <param name="path"></param>
        /// <param name="stops"></param>
        public void getRouteByStops(List<clsVertex> lstvertex, string origin, string destiny, int stopsNumber, string condition, List<clsCamino> caminos, string path, int? stops = 0)
        {
            lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().Estado = true;

            path = string.Concat(path, origin);

            foreach (var item in lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().lstEdge)
            {
                clsVertex v = lstVertex.Where(x => x.Name.Equals(item.Vertex2.ToString())).FirstOrDefault();
                switch (condition)
                {
                    case "MAX":
                        {
                            if (v.Name.Equals(destiny) && stops <= stopsNumber)
                            {
                                caminos.Add(new clsCamino() { path = path, stops = (int)stops });
                                v.Estado = false;

                                return;
                            }
                            break;
                        }
                    case "EQUAL":
                        {
                            if (stops == stopsNumber)
                            {
                                caminos.Add(new clsCamino() { path = path, stops = (int)stops });
                                v.Estado = false;
                                return;
                            }
                            break;
                        }
                }
                if (!v.Estado)
                {
                    stops += 1;
                    getRouteByStops(lstVertex, v.Name, destiny, stopsNumber, condition, caminos, path, stops);
                    v.Estado = false;
                }
            }
        }

        /// <summary>
        /// Get the routes with cost-distance conditions
        /// </summary>
        /// <param name="lstvertex"></param>
        /// <param name="origin"></param>
        /// <param name="destiny"></param>
        /// <param name="stopsNumber"></param>
        /// <param name="condition"></param>
        /// <param name="caminos"></param>
        /// <param name="path"></param>
        /// <param name="stops"></param>

        public void getRouteByCost(List<clsVertex> lstvertex, string origin, string destiny, int stopsNumber, string condition, List<clsCamino> caminos, string path, int? stops = 0)
        {
            lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().Estado = true;

            path = string.Concat(path, origin);

            foreach (var item in lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().lstEdge)
            {
                clsVertex v = lstVertex.Where(x => x.Name.Equals(item.Vertex2.ToString())).FirstOrDefault();

                if (path.Length > 1 && int.Parse(this.getDistance(path)) <= stopsNumber)
                {
                    path = string.Concat(path, v.Name);
                    caminos.Add(new clsCamino() { path = path, stops = (int)stops });
                    lstVertex.Where(x => x.Name.Equals(item.Vertex2.ToString())).FirstOrDefault().Estado = false;

                    return;
                }

                if (!v.Estado)
                {
                    stops += 1;
                    getRouteByCost(lstVertex, v.Name, destiny, stopsNumber, condition, caminos, path, stops);
                    lstVertex.Where(x => x.Name.Equals(item.Vertex2.ToString())).FirstOrDefault().Estado = false;

                }
            }
        }


        /// <summary>
        /// Get the routes between 2 points
        /// </summary>
        /// <param name="lstvertex"></param>
        /// <param name="origin"></param>
        /// <param name="destiny"></param>
        /// <param name="caminos"></param>
        /// <param name="path"></param>

        public void getRoutes(List<clsVertex> lstvertex, string origin, string destiny, List<clsCamino> caminos, string path)
        {
            lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().Estado = true;

            path = string.Concat(path, origin);

            foreach (var item in lstVertex.Where(x => x.Name.Equals(origin)).FirstOrDefault().lstEdge)
            {
                clsVertex v = lstVertex.Where(x => x.Name.Equals(item.Vertex2.ToString())).FirstOrDefault();

                if (v.Name.Equals(destiny) &&path.Length > 1)
                {
                    path = string.Concat(path, v.Name);
                    caminos.Add(new clsCamino() { path = path, costo= int.Parse(this.getDistance(path)) });
                    v.Estado = false;
                    return;
                }

                if (!v.Estado)
                {
                    getRoutes(lstVertex, v.Name, destiny, caminos, path);
                    v.Estado = false;
                }
            }
        }

    }
}