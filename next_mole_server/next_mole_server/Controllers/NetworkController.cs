using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using next_mole_server.Models;

namespace next_mole_server.Controllers
{
    public class NetworkController : ApiController
    {

        // GET: api/Network
        public IEnumerable<List<string>> Get100Paths()
        {
            try
            {

                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                List<List<string>> literalPaths = new List<List<string>>();
                List<int> pathsCount = new List<int>();
                //pathsCount = mainAlgorithm.GetPathsSimple(ref literalPaths);
                //   האם הפונקציה גט פאס סימפל רלוונטית??
                //   לפי איך שהיא נראית במיין אלגוריתם היא עובדת עם טבלה ספציפית של פוליטיקאים


                return literalPaths;
            }
            catch (Exception ex)
            {
                throw new Exception("problem with get100paths, the error: " + ex);
            }
            
        }

        [HttpGet]
        [Route("api/networkGetPath")]
        public IEnumerable<List<string>> GetPaths(string source,string target,string categoryName)
        {
            try
            {
                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                List<List<string>> TwoPaths = new List<List<string>>();
                //TwoPaths = mainAlgorithm.GetPath(source, target, categoryName);
                TwoPaths = mainAlgorithm.GetPath(source, target, categoryName);
                return TwoPaths;
            }
            catch (Exception ex)
            {

                throw new Exception("problem with GetTwoPaths, the error: " + ex);
            }
        }
        [HttpGet]
        [Route("api/networkStartAGame")]
        public IEnumerable<List<string>> StartAGame(string categoryName)
        {
            try
            {
                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                List<List<string>> StartPaths = new List<List<string>>();
                StartPaths = mainAlgorithm.StartAGame(categoryName);

                return StartPaths;
            }
            catch (Exception ex)
            {

                throw new Exception("problem with StartAGame, the error: " + ex);
            }
        }

        [HttpGet]
        [Route("api/networkGetRandomVerteciesFromVertex")]
        public List<string> GetRandomVerteciesFromVertex(string source, string categoryName)
        {
            try
            {
                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                List<string> moreVertecies = new List<string>();
                moreVertecies = mainAlgorithm.getThreeMoreRandom(source,categoryName);

                return moreVertecies;
            }
            catch (Exception ex)
            {

                throw new Exception("problem with GetRandomVerteciesFromVertex, the error: " + ex);
            }
        }

        [HttpGet]
        [Route("api/networkGetRandomVerteciesFromVertex3")]
        public List<string> GetRandomVerteciesFromVertex3(string source, string categoryName)
        {
            try
            {
                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                List<string> moreVertecies = new List<string>();
                moreVertecies = mainAlgorithm.getFourMoreRandom(source, categoryName);

                return moreVertecies;
            }
            catch (Exception ex)
            {

                throw new Exception("problem with networkGetRandomVerteciesFromVertex3, the error: " + ex);
            }
        }

        //random vertecies from the same category
        public string[] GetRandomVertecies(string categoryName)
        {
            try
            {
                MainAlgorithm mainAlgorithm = new MainAlgorithm();
                string[] randomVertecies = new string[6];
                randomVertecies = mainAlgorithm.GetRandomVertecies(categoryName);

                return randomVertecies;
            }
            catch (Exception ex)
            {

                throw new Exception("problem with GetRandomVertecies, the error: " + ex);
            }
        }

        [HttpGet]
        [Route("api/getNetwork")]
        public Network GetNetworkForShow(string categoryName)
        {
            try
            {
                Network n = new Network();
                return n.GetNet(categoryName);              
            }
            catch (Exception ex)
            {
                throw new Exception("problem to get network. error: " + ex);
            }
        }

    }
}
