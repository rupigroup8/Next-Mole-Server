﻿using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
using Advanced.Algorithms.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using next_mole_server.Models.DAL;


namespace next_mole_server.Models
{
    public class MainAlgorithm
    {
        public string Source { get; set; }
        public string Source2 { get; set; }
        public string Target { get; set; }
        
        static Random random = new Random();

        
        public MainAlgorithm(string source, string target)
        {
            Source = source;
            Target = target;
        }
        public MainAlgorithm()
        {

        }

        /// <summary>
        /// getting rendom paths of only two-direction vertecies for testing
        /// </summary>
        /// <param name="Paths">empty list of paths to fill</param>
        /// <returns></returns>
        ///         ---NOT IN USE---
        public List<int> GetPathsAdvancedIntersect(ref List<List<string>> Paths)
        {
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();

            // get all vertecies and edges from DB
            DBservices db = new DBservices();
            vertecies = db.GetAllVertecies("TheMoleConnection", "MoviesVertecies");
            edges = db.GetEdges("TheMoleConnection", "MoviesEdges");

            //1. create a graph
            var graph = new WeightedDiGraph<string, int>();
            //2. insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //3. insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //remove one-direction edges
            List<List<string>> twoDirectionsEdges = new List<List<string>>();
            foreach (var edge in edges)
            {
                if (!graph.HasEdge(edge[1], edge[0]))
                {
                    graph.RemoveEdge(edge[0], edge[1]);
                }
                if (graph.HasEdge(edge[1], edge[0]))
                {
                    twoDirectionsEdges.Add(edge);
                }
            }
            //4. create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());


            List<int> pathsCount = new List<int>();
            //var result = algorithm.FindShortestPath(graph, Source, Target);
            //pathsCount.Add(result.Path.Count);
            //Paths.Add(result.Path

            //5.run the algoritm for 110 random vertecies.
            for (int i = 0; i < 200; i++)
            {
                int sourceVertex = random.Next(0, vertecies.Count);
                int targetVertex = random.Next(0, vertecies.Count);
                //if source and target pages are the same DON'T run the algorithm
                if (sourceVertex != targetVertex)
                {
                    var result = algorithm.FindShortestPath(graph, vertecies[sourceVertex], vertecies[targetVertex]);
                    pathsCount.Add(result.Path.Count);
                    Paths.Add(result.Path);

                }
            }

            return pathsCount;
        }

        /// <summary>
        ///getting a list of random paths for testing
        /// </summary>
        /// <param name="Paths">empty list of paths to fill</param>
        /// <returns></returns>
        ///         ---NOT IN USE---
        public List<int> GetPathsSimple_OLD(ref List<List<string>> Paths)
        {
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();

            // get all vertecies and edges from DB
            DBservices db = new DBservices();
            vertecies = db.GetAllVertecies("TheMoleConnection", "PoliticiansVertecies");
            edges = db.GetEdges("TheMoleConnection", "PoliticiansEdges");

            //1. create a graph
            var graph = new WeightedDiGraph<string, int>();
            //2. insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //3. insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }

            //4. create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());


            List<int> pathsCount = new List<int>();
            //var result = algorithm.FindShortestPath(graph, Source, Target);
            //pathsCount.Add(result.Path.Count);
            //Paths.Add(result.Path

            //5.run the algoritm for 110 random vertecies.
            for (int i = 0; i < 200; i++)
            {
                int sourceVertex = random.Next(0, vertecies.Count);
                int targetVertex = random.Next(0, vertecies.Count);
                //if source and target pages are the same DON'T run the algorithm
                if (sourceVertex != targetVertex)
                {
                    List<string> pathsTwo = new List<string>();
                    var result = algorithm.FindShortestPath(graph, vertecies[sourceVertex], vertecies[targetVertex]);
                    pathsCount.Add(result.Path.Count);
                    if (result.Path.Count == 1)
                    {
                        pathsTwo.Add(vertecies[sourceVertex]);
                        pathsTwo.Add(vertecies[targetVertex]);
                        Paths.Add(pathsTwo);
                    }
                    else Paths.Add(result.Path);

                }
            }

            return pathsCount;
        }


        /// <summary>
        ///getting a list of random paths for testing
        /// </summary>
        /// <param name="Paths">empty list of paths to fill</param>
        /// <returns></returns>
        /// מחזיר הפניה לרשימה של כל הדרכים
        ///       ---NOT IN USE ??? ---
        public List<int> GetPathsSimple(ref List<List<string>> Paths, string categoryName)
        {
            //רשימת ערכים
            List<string> vertecies = new List<string>();
            //הקשתות - רשימה של רשימה
            List<List<string>> edges = new List<List<string>>();

            // get all vertecies and edges from DB
            DBservices db = new DBservices();
            string Vsrt = categoryName + "Vertecies";
            string Estr = categoryName + "Edges";
            vertecies = db.GetAllVertecies("DBConnectionString", Vsrt);
            edges = db.GetEdges("DBConnectionString", Estr);

            //הכרזה על גרף עם משקולת של 1 על כל קשת כדי להריץ את אלגוריתם הדיקסטרה
            //1. create a graph
            var graph = new WeightedDiGraph<string, int>();
            //הוספה לגרף את כל הצמתים שהבאנו מהדאתא בייס
            //2. insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //הוספת כל הקשתות - מכיל ממי יצאתי, למי אני יוצא, ומשקל על הקשת 1
            //3. insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //יצירת האלגוריתם הדיקסטרה באמצעות סיפריית 
            //using Advanced.Algorithms.DataStructures.Graph.AdjacencyList;
            //using Advanced.Algorithms.Graph;
            //4. create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());

            //רשימה אשר סופרת בכל הדרכים כמה ערכים ישלי
            //מכילה את התוצאות של הדיקסטרה
            List<int> pathsCount = new List<int>();

            //הרצת האלגוריתם על 200 דרכים כטסט לראות מה הדרכים הכי קצרות
            //5.run the algoritm for 110 random vertecies.
            for (int i = 0; i < 200; i++)
            {
                //קבלת ערך רנדומלי מהרשימה שייצאנו מהדאא בייס מאינדקס 0 ועד הסוף
                int sourceVertex = random.Next(0, vertecies.Count);
                int targetVertex = random.Next(0, vertecies.Count);

                //בדיקת מקרה קצה שהערכים שונים
                //אם שונה מפעיל את האלגוריתם דיקסטרה מלמעלה
                //if source and target pages are the same DON'T run the algorithm
                if (sourceVertex != targetVertex)
                {
                    List<string> pathsTwo = new List<string>();
                    var result = algorithm.FindShortestPath(graph, vertecies[sourceVertex], vertecies[targetVertex]);
                    //הוספת תכונה לרשימת הערכים בדרך מסויימת - כמה צעדים זה לקח
                    pathsCount.Add(result.Path.Count);
                    //מקרה קצה - אם אין ערך שמפריד בין ערך ההתחלה והסיום - בלי לעבור בשום ערך אחר בדרך
                    //נוסיף לליסט שיצרנו את ההערך התחלה וערך הסיום עם כמות צעדים של 1
                    if (result.Path.Count == 1)
                    {
                        pathsTwo.Add(vertecies[sourceVertex]);
                        pathsTwo.Add(vertecies[targetVertex]);
                        Paths.Add(pathsTwo);
                    }
                    //אחרת לא מדובר במקרה קצה ונכניס רגיל
                    else Paths.Add(result.Path);

                }
            }

            return pathsCount;
        }



        /// <summary>
        /// getting the path from a source vertex to a target vertex
        /// </summary>
        /// <param name="source">inital vertex to start from</param>
        /// <param name="traget"> target vertex</param>
        /// <param name="categoryName">choose from: NBA,Politicans,GeneralKnowledge,Movies,Music,Celeb</param>
        /// <returns></returns>
        ///         ---NOT DYNAMIC---
        public List<List<string>> GetPath_OLD(string source, string target,string categoryName)
        {
            //get all vertecies and edges from db
            DBservices db = new DBservices();

            List<List<string>> TwoPaths = new List<List<string>>();
            string edgeCategoryName = "";
            string verteciesCategoryName = "";
            switch (categoryName.ToUpper())
            {
                case "NBA":
                    edgeCategoryName = "NBAEdges";
                    verteciesCategoryName = "NBAVertecies";
                    break;
                case "GENERAL KNOWLEDGE":
                    edgeCategoryName = "GeneralEdges";
                    verteciesCategoryName = "GeneralVertecies";
                    break;
                case "FILMS":
                    edgeCategoryName = "MoviesEdges";
                    verteciesCategoryName = "MoviesVertecies";
                    break;
                case "MUSIC":
                    edgeCategoryName = "MusicEdges";
                    verteciesCategoryName = "MusicVertecies";
                    break;
                case "CELEBRITY":
                    edgeCategoryName = "CelebEdges";
                    verteciesCategoryName = "CelebVertecies";
                    break;
                case "POLITICS":
                    edgeCategoryName = "PoliticiansEdges";
                    verteciesCategoryName = "PoliticiansVertecies";
                    break;

                default:
                    break;
            }
            List<string> path = new List<string>();


            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();
            if (HttpContext.Current.Application[verteciesCategoryName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesCategoryName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("TheMoleConnection", verteciesCategoryName);
            if (HttpContext.Current.Application[edgeCategoryName] != null)
            {
                edges = HttpContext.Current.Application[edgeCategoryName] as List<List<string>>;
            }
            else edges = db.GetEdges( "TheMoleConnection", edgeCategoryName);
            //create a graph 
            var graph = new WeightedDiGraph<string, int>();
            //insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());

            List<string> pathsTwo = new List<string>();
            try
            {
                var result = algorithm.FindShortestPath(graph, source, target);
                if (result.Path.Count == 1)
                {
                    pathsTwo.Add(source);
                    pathsTwo.Add(target);
                    TwoPaths.Add(pathsTwo);
                }

                else TwoPaths.Add(result.Path);
                TwoPaths.Add(getThreeMoreRandom(source, categoryName));
            }
            catch (Exception ex)
            {

               // TwoPaths.Add(getThreeMoreRandom(source, categoryName));
                return TwoPaths;
            }
            return TwoPaths;

        }

        /// <summary>
        /// getting the path from a source vertex to a target vertex
        /// </summary>
        /// <param name="source">inital vertex to start from</param>
        /// <param name="target"> target vertex</param>
        /// <param name="categoryName">choose from: NBA,Politicans,GeneralKnowledge,Movies,Music,Celeb</param>
        /// <returns></returns>
        //הרצה של הדיקסטרה פעם אחת בלבד
        public List<List<string>> GetPath(string source, string target, string categoryName)
        {
            //get all vertecies and edges from db
            DBservices db = new DBservices();

            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            List<List<string>> TwoPaths = new List<List<string>>();
            string edgeTableName = categoryName + "Edges";
            string verteciesTableName = categoryName + "Vertecies";

            //פה נשמור את הדרך שלנו
            List<string> path = new List<string>();


            //קריאה מדאתא בייס על ידי הקטגוריה שקיבלנו מהמשתמש
            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();
            if (HttpContext.Current.Application[verteciesTableName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesTableName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("DBConnectionString", verteciesTableName);
            if (HttpContext.Current.Application[edgeTableName] != null)
            {
                edges = HttpContext.Current.Application[edgeTableName] as List<List<string>>;
            }
            else edges = db.GetEdges("DBConnectionString", edgeTableName);
            //create a graph 
            var graph = new WeightedDiGraph<string, int>();
            //insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());

            List<string> pathsTwo = new List<string>();
            try
            {
                var result = algorithm.FindShortestPath(graph, source, target);
                if (result.Path.Count == 1)
                {
                    pathsTwo.Add(source);
                    pathsTwo.Add(target);
                    TwoPaths.Add(pathsTwo);
                }

                else TwoPaths.Add(result.Path);
                TwoPaths.Add(getThreeMoreRandom(source, categoryName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // TwoPaths.Add(getThreeMoreRandom(source, categoryName));
                return TwoPaths;
            }
            return TwoPaths;

           
        }



        /// <summary>
        /// get random six vertecies from a given category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        ///         ---NOT DYNAMIC---
        public string[] GetRandomVertecies_OLD(string categoryName)
        {
            string[] sixVertecies = new string[6];
            string verteciesCategoryName = "";
            switch (categoryName.ToUpper())
            {
                case "NBA":
                    verteciesCategoryName = "NBAVertecies";
                    break;
                case "GENERAL KNOWLEDGE":
                    verteciesCategoryName = "GeneralVertecies";
                    break;
                case "FILMS":
                    verteciesCategoryName = "MoviesVertecies";
                    break;
                case "MUSIC":
                    verteciesCategoryName = "MusicVertecies";
                    break;
                case "CELEBRITY":
                    verteciesCategoryName = "CelebVertecies";
                    break;
                case "POLITICS":
                    verteciesCategoryName = "PoliticiansVertecies";
                    break;

                default:
                    break;
            }
            //get all vertecies and edges from db
            DBservices db = new DBservices();
            //get vertecies for the given category
            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            if (HttpContext.Current.Application[verteciesCategoryName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesCategoryName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("TheMoleConnection", verteciesCategoryName);
            
            //get six  random articles from vertecies list in a category
            for (int i = 0; i < 6; i++)
            {
                int randomVertex = random.Next(0, vertecies.Count);
                sixVertecies[i] = vertecies[randomVertex];
            }

            return sixVertecies;
        }


        /// <summary>
        /// get random six vertecies from a given category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public string[] GetRandomVertecies(string categoryName)
        {
            string[] sixVertecies = new string[6];
            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            string verteciesTableName = categoryName+ "Vertecies";
            
            //get all vertecies and edges from db
            DBservices db = new DBservices();
            //get vertecies for the given category
            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            if (HttpContext.Current.Application[verteciesTableName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesTableName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("DBConnectionString", verteciesTableName);

            //get six  random articles from vertecies list in a category
            for (int i = 0; i < 6; i++)
            {
                int randomVertex = random.Next(0, vertecies.Count);
                sixVertecies[i] = vertecies[randomVertex];
            }

            return sixVertecies;
        }


        /// <summary>
        /// //get vertecies from DB for a category
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public List<string> GetVerteciesForCategory(string connectionString,string tableName)
        {
            List<string> vertecies = new List<string>();
            DBservices db = new DBservices();
            vertecies = db.GetAllVertecies("DBConnectionString", tableName);

            return vertecies;
        }

        /// <summary>
        /// get edges from DB for a category
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public List<List<string>> GetEdgesForCategory(string connectionString,string categoryName)
        {

            List<List<string>> edges = new List<List<string>>();
            DBservices db = new DBservices();
            edges = db.GetEdges("DBConnectionString", categoryName);


            return edges;
        }

        public List<List<string>> StartAGame_OLD(string categoryName)
        {
            List<List<string>> StartVerteciesAndPaths = new List<List<string>>();
            string edgeCategoryName = "";
            string verteciesCategoryName = "";
            switch (categoryName.ToUpper())
            {
                case "NBA":
                    edgeCategoryName = "NBAEdges";
                    verteciesCategoryName = "NBAVertecies";
                    break;
                case "GENERAL KNOWLEDGE":
                    edgeCategoryName = "GeneralEdges";
                    verteciesCategoryName = "GeneralVertecies";
                    break;
                case "FILMS":
                    edgeCategoryName = "MoviesEdges";
                    verteciesCategoryName = "MoviesVertecies";
                    break;
                case "MUSIC":
                    edgeCategoryName = "MusicEdges";
                    verteciesCategoryName = "MusicVertecies";
                    break;
                case "CELEBRITY":
                    edgeCategoryName = "CelebEdges";
                    verteciesCategoryName = "CelebVertecies";
                    break;
                case "POLITICS":
                    edgeCategoryName = "PoliticiansEdges";
                    verteciesCategoryName = "PoliticiansVertecies";
                    break;

                default:
                    break;
            }
            DBservices db = new DBservices();
            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();
            if (HttpContext.Current.Application[verteciesCategoryName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesCategoryName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("TheMoleConnection", verteciesCategoryName);
            if (HttpContext.Current.Application[edgeCategoryName] != null)
            {
                edges = HttpContext.Current.Application[edgeCategoryName] as List<List<string>>;
            }
            else edges = db.GetEdges("TheMoleConnection", edgeCategoryName);
            //create a graph 
            var graph = new WeightedDiGraph<string, int>();
            //insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());
            bool isDone = false;
            while (!isDone)
            {
                int sourceVertex = random.Next(0, vertecies.Count);
                int targetVertex = random.Next(0, vertecies.Count);
                if (sourceVertex != targetVertex)
                {
                    
                    List<string> pathsTwo = new List<string>();
                    var result = algorithm.FindShortestPath(graph, vertecies[sourceVertex], vertecies[targetVertex]);
                    var result1 = algorithm.FindShortestPath(graph, vertecies[targetVertex], vertecies[sourceVertex]);

                    if (result.Path.Count == result1.Path.Count)
                    {
                        isDone = true;
                        if (result.Path.Count == 1)
                        {
                            //pathsTwo.Add(vertecies[sourceVertex]);
                            //pathsTwo.Add(vertecies[targetVertex]);
                            //StartVerteciesAndPaths.Add(pathsTwo);
                            //pathsTwo.Clear();
                            //pathsTwo.Add(vertecies[targetVertex]);
                            //pathsTwo.Add(vertecies[sourceVertex]);
                            //StartVerteciesAndPaths.Add(pathsTwo);

                            isDone = false;
                        }
                        else
                        {
                            StartVerteciesAndPaths.Add(result.Path);
                            StartVerteciesAndPaths.Add(result1.Path);
                        }
                    }
                }
            }
            List<string> edgesForFirstVertex = this.getThreeMoreRandom(StartVerteciesAndPaths[0][0], categoryName);
            List<string> edgesForSecondVertex = this.getThreeMoreRandom(StartVerteciesAndPaths[1][0], categoryName);
            StartVerteciesAndPaths.Add(edgesForFirstVertex);
            StartVerteciesAndPaths.Add(edgesForSecondVertex);

            return StartVerteciesAndPaths;
        }
        public List<List<string>> StartAGame(string categoryName)
        {
            List<List<string>> StartVerteciesAndPaths = new List<List<string>>();
            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            string edgeTableName = categoryName + "Edges";
            string verteciesTableName = categoryName + "Vertecies";

            DBservices db = new DBservices();
            //get edges and vertecies for the given category
            List<string> vertecies = new List<string>();
            List<List<string>> edges = new List<List<string>>();
            if (HttpContext.Current.Application[verteciesTableName] != null)
            {
                vertecies = HttpContext.Current.Application[verteciesTableName] as List<string>;
            }
            else vertecies = db.GetAllVertecies("DBConnectionString", verteciesTableName);
            if (HttpContext.Current.Application[edgeTableName] != null)
            {
                edges = HttpContext.Current.Application[edgeTableName] as List<List<string>>;
            }
            else edges = db.GetEdges("DBConnectionString", edgeTableName);
            //create a graph 
            var graph = new WeightedDiGraph<string, int>();
            //insert vertecies to the graph
            foreach (string vertex in vertecies)
            {
                graph.AddVertex(vertex);
            }
            //insert edges to the graph
            foreach (var edge in edges)
            {
                graph.AddEdge(edge[0], edge[1], 1);
            }
            //create dijkstra algorithm
            var algorithm = new DijikstraShortestPath<string, int>(new DijikstraShortestPathOperators());
            bool isDone = false;
            while (!isDone)
            {
                int sourceVertex = random.Next(0, vertecies.Count);
                int targetVertex = random.Next(0, vertecies.Count);
                if (sourceVertex != targetVertex)
                {

                    List<string> pathsTwo = new List<string>();
                    var result = algorithm.FindShortestPath(graph, vertecies[sourceVertex], vertecies[targetVertex]);
                    var result1 = algorithm.FindShortestPath(graph, vertecies[targetVertex], vertecies[sourceVertex]);

                    if (result.Path.Count == result1.Path.Count)
                    {
                        isDone = true;
                        if (result.Path.Count == 1)
                        {
                            //pathsTwo.Add(vertecies[sourceVertex]);
                            //pathsTwo.Add(vertecies[targetVertex]);
                            //StartVerteciesAndPaths.Add(pathsTwo);
                            //pathsTwo.Clear();
                            //pathsTwo.Add(vertecies[targetVertex]);
                            //pathsTwo.Add(vertecies[sourceVertex]);
                            //StartVerteciesAndPaths.Add(pathsTwo);

                            isDone = false;
                        }
                        else
                        {
                            StartVerteciesAndPaths.Add(result.Path);
                            StartVerteciesAndPaths.Add(result1.Path);
                        }
                    }
                }
            }
            List<string> edgesForFirstVertex = this.getThreeMoreRandom(StartVerteciesAndPaths[0][0], categoryName);
            List<string> edgesForSecondVertex = this.getThreeMoreRandom(StartVerteciesAndPaths[1][0], categoryName);
            StartVerteciesAndPaths.Add(edgesForFirstVertex);
            StartVerteciesAndPaths.Add(edgesForSecondVertex);

            return StartVerteciesAndPaths;
        }



        /// <summary>
        /// Get Two (+Source = Three) random vertecies to choose from in the game
        /// </summary>
        /// <param name="source"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public List<string> getThreeMoreRandom_OLD(string source,string categoryName)
        {
            List<string> threeMoreRandom = new List<string>();
            string edgeCategoryName = "";
            switch (categoryName.ToUpper())
            {
                case "NBA":
                    edgeCategoryName = "NBAEdges";
                    break;
                case "GENERAL KNOWLEDGE":
                    edgeCategoryName = "GeneralEdges";
                    break;
                case "FILMS":
                    edgeCategoryName = "MoviesEdges";
                    break;
                case "MUSIC":
                    edgeCategoryName = "MusicEdges";
                    break;
                case "CELEBRITY":
                    edgeCategoryName = "CelebEdges";
                    break;
                case "POLITICS":
                    edgeCategoryName = "PoliticiansEdges";
                    break;

                default:
                    break;
            }
            DBservices db = new DBservices();
            //get edges for the given category and source

            List<string> edges = db.GetEdgesForCategoryAndSource("TheMoleConnection", edgeCategoryName, source);

            bool isListReady = false;
            int counter = 0;
            int index = 2;
            while (!isListReady)
            {
                if (edges.Count ==0)
                {
                    return threeMoreRandom;
                }
                if (edges.Count == 1)
                {
                    threeMoreRandom.Add(edges[0]);
                    return threeMoreRandom;
                }

                int edgeIndex = random.Next(0, edges.Count);
                
                if (!threeMoreRandom.Contains(edges[edgeIndex]) && edges[edgeIndex] != source)
                {
                    threeMoreRandom.Add(edges[edgeIndex]);
                    counter++;
                }
                if (counter == index)
                {
                    isListReady = true;
                }
            }
            return threeMoreRandom;
        }
        public List<string> getThreeMoreRandom(string source, string categoryName)
        {
            List<string> threeMoreRandom = new List<string>();
            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            string edgeTableName = categoryName + "Edges";
           
            DBservices db = new DBservices();
            //get edges for the given category and source

            List<string> edges = db.GetEdgesForCategoryAndSource("DBConnectionString", edgeTableName, source);

            bool isListReady = false;
            int counter = 0;
            int index = 2;
            while (!isListReady)
            {
                if (edges.Count == 0)
                {
                    return threeMoreRandom;
                }
                if (edges.Count == 1)
                {
                    threeMoreRandom.Add(edges[0]);
                    return threeMoreRandom;
                }

                int edgeIndex = random.Next(0, edges.Count);

                if (!threeMoreRandom.Contains(edges[edgeIndex]) && edges[edgeIndex] != source)
                {
                    threeMoreRandom.Add(edges[edgeIndex]);
                    counter++;
                }
                if (counter == index)
                {
                    isListReady = true;
                }
            }
            return threeMoreRandom;
        }


        /// <summary>
        /// Get Three (+Source = Four) random vertecies to choose from in the game
        /// </summary>
        /// <param name="source"></param>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public List<string> getFourMoreRandom_OLD(string source, string categoryName)
        {
            List<string> threeMoreRandom = new List<string>();
            string edgeCategoryName = "";
            switch (categoryName.ToUpper())
            {
                case "NBA":
                    edgeCategoryName = "NBAEdges";
                    break;
                case "GENERAL KNOWLEDGE":
                    edgeCategoryName = "GeneralEdges";
                    break;
                case "FILMS":
                    edgeCategoryName = "MoviesEdges";
                    break;
                case "MUSIC":
                    edgeCategoryName = "MusicEdges";
                    break;
                case "CELEBRITY":
                    edgeCategoryName = "CelebEdges";
                    break;
                case "POLITICS":
                    edgeCategoryName = "PoliticiansEdges";
                    break;

                default:
                    break;
            }
            DBservices db = new DBservices();
            //get edges for the given category and source
            List<string> edges = db.GetEdgesForCategoryAndSource("DBConnectionString", edgeCategoryName, source);
            if (edges.Count == 0 || edges.Count == 1 || edges.Count == 2)
            {
                return threeMoreRandom;
            }
            bool isListReady = false;
            int counter = 0;
            while (!isListReady)
            {

                int edgeIndex = random.Next(0, edges.Count);
                if (!threeMoreRandom.Contains(edges[edgeIndex]) && edges[edgeIndex]!=source)
                {
                    threeMoreRandom.Add(edges[edgeIndex]);
                    counter++;
                }
                if (counter == 3)
                {
                    isListReady = true;
                }
            }
            return threeMoreRandom;
        }
        public List<string> getFourMoreRandom(string source, string categoryName)
        {
            List<string> threeMoreRandom = new List<string>();
            switch (categoryName.ToUpper())
            {
                case "GENERAL KNOWLEDGE":
                    categoryName = "General";
                    break;
                case "FILMS":
                    categoryName = "Movies";
                    break;
                case "CELEBRITY":
                    categoryName = "Celeb";
                    break;
                case "POLITICS":
                    categoryName = "Politicians";
                    break;

                default:
                    break;
            }

            string edgeTableName = categoryName + "Edges";

            DBservices db = new DBservices();
            //get edges for the given category and source
            List<string> edges = db.GetEdgesForCategoryAndSource("DBConnectionString", edgeTableName, source);
            if (edges.Count == 0 || edges.Count == 1 || edges.Count == 2)
            {
                return threeMoreRandom;
            }
            bool isListReady = false;
            int counter = 0;
            while (!isListReady)
            {

                int edgeIndex = random.Next(0, edges.Count);
                if (!threeMoreRandom.Contains(edges[edgeIndex]) && edges[edgeIndex] != source)
                {
                    threeMoreRandom.Add(edges[edgeIndex]);
                    counter++;
                }
                if (counter == 3)
                {
                    isListReady = true;
                }
            }
            return threeMoreRandom;
        }
    }

    //helper for the algorithm
    public class DijikstraShortestPathOperators : IShortestPathOperators<int>
    {
        public int DefaultValue
        {
            get
            {
                return 0;
            }


        }

        public int MaxValue
        {
            get
            {
                return int.MaxValue;
            }
        }

        public int Sum(int a, int b)
        {
            return checked(a + b);
        }
    }

}