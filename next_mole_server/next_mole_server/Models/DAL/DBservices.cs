using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using AdminPage.Models;
using next_mole_server.Models;

namespace next_mole_server.Models.DAL
{
    public class DBservices 
    {
        public SqlConnection connect(string str)
        {
            // read the connection string from the configuration file with the constring name
            string cStr = WebConfigurationManager.ConnectionStrings[str].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        public SqlCommand command(SqlConnection con, string CommandSTR)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object
            cmd.Connection = con; // assign the connection to the command object
            cmd.CommandText = CommandSTR;
            return cmd;
        }
        public bool checkCategory(string name)
        {                                       //valid that there is no category like that yet                     
            SqlConnection con = null;
            string upperName = name.ToUpper();      // insensitive chacking
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM Categories WHERE upper(CategoryName)='" + upperName + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int DBcreateTable(string name)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String cStr = createTableCommand(name);      // helper method to build the insert string

            cmd = command(con, cStr);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public string createTableCommand(string name)
        {
            string categoryStr = "INSERT INTO categories(categoryName) values ('" + name + "')";   // adding the category
            string prefix = "create table " + name + "Vertecies (id int IDENTITY(1,1) PRIMARY KEY, TitlePage nvarchar(255) NOT NULL, nodeImageURL nvarchar(max), nodeDescription nvarchar(max));";
            string prefix2 = "create table " + name + "AllVertecies (id int IDENTITY(1,1) PRIMARY KEY, TitlePage nvarchar(255) NOT NULL, nodeImageURL nvarchar(max), nodeDescription nvarchar(max));";
            string prefix3 = "create table " + name + "Edges (id int IDENTITY(1,1) PRIMARY KEY, OriginPage nvarchar(255) NOT NULL, LinkedPage nvarchar(255) NOT NULL);";
            return categoryStr + prefix + prefix2 + prefix3;
        }

        public int DBinsertNodes(List<Node> nodes, string name)      // insert to uniqe table, by the category name
        {
            SqlConnection con;
            SqlCommand cmd;
            int numEffected = 0;
            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (Node item in nodes)
            {
                String CommandSTR = NodeInsertCommand(item, name);
                try
                {
                    cmd = command(con, CommandSTR);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    numEffected += cmd.ExecuteNonQuery(); // Execute the command
                }
                catch (Exception ex)
                {
                    return 0;
                    throw (ex);
                }
            }
            if (con != null)
            {
                con.Close();
            }
            return numEffected;
        }
        public int DBinsertAllNodes(List<Node> nodes, string name)      // insert to uniqe table, by the category name
        {
            SqlConnection con;
            SqlCommand cmd;
            int numEffected = 0;
            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string tableName = name + "All";
            foreach (Node item in nodes)
            {
                String CommandSTR = NodeInsertCommand(item, tableName);
                try
                {
                    cmd = command(con, CommandSTR);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    numEffected += cmd.ExecuteNonQuery(); // Execute the command
                }
                catch (Exception ex)
                {
                    return 0;
                    throw (ex);
                }
            }
            if (con != null)
            {
                con.Close();
            }
            return numEffected;
        }

        public string NodeInsertCommand(Node node, string name)
        {
            //string str = "IF NOT EXISTS(SELECT * FROM node WHERE nodeNum = '" + node.NodeNum + "')";
            string str = "IF NOT EXISTS(SELECT * FROM "+name+ "Vertecies WHERE TitlePage = '" + node.NodeNum + "')";
            string prefix = "INSERT INTO "+name+ "Vertecies (TitlePage,nodeImageURL, nodeDescription)";
            string cmdValues = string.Format("VALUES('{0}', '{1}', '{2}')", node.NodeNum,node.NodeImageURL, node.NodeDescription);
            string all = str + prefix + cmdValues;
            return all;
        }
        public int DBinsertLinks(List<Link> links, string name)
        {
            SqlConnection con;
            SqlCommand cmd;
            int numEffected = 0;
            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            foreach (Link item in links)
            {
                String CommandSTR = LinkInsertCommand(item, name);
                try
                {
                    cmd = command(con, CommandSTR);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                try
                {
                    numEffected += cmd.ExecuteNonQuery(); // Execute the command
                }
                catch (Exception ex)
                {
                    return 0;
                    throw (ex);
                }

                //finally
                //{
                //    if (con == null)
                //    {
                //        con.Close();        //final step, Close
                //    }
                //}
            }
            if (con != null)
            {
                con.Close();
            }
            return numEffected;
        }
        public string LinkInsertCommand(Link link, string name)
        {
            //string prefix = "INSERT INTO link(sourceNode , targetNode , connectionType, connectionWeight  )";
            string str = "IF NOT EXISTS(SELECT * FROM " + name + "Edges WHERE OriginPage = '" + link.SourceNode + "' and LinkedPage = '" + link.TargetNode+"')";
            string prefix = "INSERT INTO " + name + "Edges (OriginPage, LinkedPage)";
            string cmdValues = string.Format("VALUES('{0}', '{1}')", link.SourceNode,link.TargetNode);
            //string cmdValues = string.Format("VALUES('{0}', '{1}', '{2}', '{3}')", link.SourceNode, link.TargetNode, link.ConnectionType, link.ConnectionWeight);

            return prefix + cmdValues;
        }
        public List<Link> deleteConnection(string connection)
        {
            List<Link> LinksList = new List<Link>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "DELETE FROM link WHERE id='" + connection + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                return getAllLinks();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Link> getAllLinks()
        {
            List<Link> LinksList = new List<Link>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM link";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Link link = new Link();

                    link.LinkNum = Convert.ToInt32(dr["linkNum"]);
                    link.SourceNode = (string)dr["SourceNode"];
                    link.TargetNode = (string)dr["TargetNode"];
                    link.ConnectionType = (string)dr["ConnectionType"];
                    link.ConnectionWeight = Convert.ToDouble(dr["ConnectionWeight"]);
                    
                    LinksList.Add(link);
                }
                return LinksList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Node> deleteNode(string id)
        {
            List<Node> NodesList = new List<Node>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                string getConnection= "SELECT ConnectionType from node WHERE NodeNum = '"+id+"'";
                deleteConnection(getConnection);
                String selectSTR = "DELETE FROM Node WHERE NodeNum='" + id + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                return getAllNodes();
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public List<Node> getAllNodes()
        {
            List<Node> NodesList = new List<Node>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM node";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Node node = new Node();

                    node.NodeId = Convert.ToInt32(dr["NodeId"]);
                    node.NodeNum = (string)dr["NodeNum"];
                    node.NodeDescription = (string)dr["NodeDescription"];

                    NodesList.Add(node);
                }

                return NodesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }
        public bool checkLogin(string email, string password)
        {
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM userPlayer WHERE userEmail='" + email + "' AND userPassword='" + password + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                if (dr.Read())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public bool checkEmail(string email)
        {                                       //valid that there is no other user with this email 
                                                   //that is allready registered
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM userPlayer WHERE userEmail='" + email + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                if (dr.Read())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public int DBinsertUser(User user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String cStr = UserInsertCommand(user);      // helper method to build the insert string

            cmd = command(con, cStr);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public SqlDataAdapter da;
        public DataTable dt;

        //connection string: TheMoleConnection

        public DBservices()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }
        //---------------------------------------------------------------------------------
        // Read winners from the DB into a list - dataReader withOut Filter
        //---------------------------------------------------------------------------------
        public List<Player> GetWinners(string conString)
        {

            SqlConnection con = null;
            List<Player> lp = new List<Player>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "select top 25 UserNickname,numOfWinnings,UserEmail,profile_pic from Player group by numOfWinnings,UserNickname,UserEmail,profile_pic order by numOfWinnings DESC";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Player p = new Player();
                    p.NickName = (string)dr["UserNickname"];
                    p.Email = (string)dr["UserEmail"];
                    p.NumOfWinnings = (Int32)dr["numOfWinnings"];
                    p.ProfilePic = (string)dr["profile_pic"];
                    lp.Add(p);
                }

                return lp;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<List<string>> GetEdges(string conString, string tableName)
        {
            SqlConnection con = null;
            List<List<string>> edgesList = new List<List<string>>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {   // Read till the end of the data into a row
                    List<string> pages = new List<string>();

                    string page = Convert.ToString(dr["OriginPage"]);
                    string linkedPage = Convert.ToString(dr["LinkedPage"]);
                    pages.Add(page);
                    pages.Add(linkedPage);

                    edgesList.Add(pages);
                }

                return edgesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<string> GetEdgesForCategoryAndSource(string conString, string tableName, string source)
        {
            SqlConnection con = null;
            List<string> edgesList = new List<string>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + " where OriginPage='" + source + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {   // Read till the end of the data into a row
                    //string page = Convert.ToString(dr["OriginPage"]);
                    string linkedPage = Convert.ToString(dr["LinkedPage"]);
                    edgesList.Add(linkedPage);
                }

                return edgesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public List<string> GetAllVertecies(string conString, string tableName)
        {
            SqlConnection con = null;

            List<string> verteciesList = new List<string>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                while (dr.Read())
                {
                    string vertex = dr["TitlePage"].ToString();
                    verteciesList.Add(vertex);
                }

                return verteciesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public int insert(Player player)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String pStr = BuildInsertCommand(player);      // helper method to build the insert string

            cmd = CreateCommand(pStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public int insertToken(string token, string uid)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String pStr = "update Player set NotificationToken ='" + token + "' where uid = '" + uid + "'";    // helper method to build the insert string

            cmd = CreateCommand(pStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public int insertWinOrLose(int win, int cashMole, string uid)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String pStr = "update Player set cashMole=" + cashMole + "+cashMole,numOfWinnings=" + win + "+numOfWinnings where uid = '" + uid + "'";    // helper method to build the insert string

            cmd = CreateCommand(pStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public int insertAvatar(string avatarUrl, string uid)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String pStr = "update Player set profile_pic ='" + avatarUrl + "' where uid = '" + uid + "'";    // helper method to build the insert string

            cmd = CreateCommand(pStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        public int insertLastLogin(string uid)
        {
            SqlConnection con;
            SqlCommand cmd;
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime time = DateTime.Now;
            try
            {
                con = connect("DBConnectionString"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            String pStr = "update Player set LastLogin ='" + time.ToString(format) + "' where uid = '" + uid + "'";    // helper method to build the insert string

            cmd = CreateCommand(pStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                return 0;
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        /// <summary>
        /// insert a new player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private String BuildInsertCommand(Player player)
        {
            String command;
            //player nickName is: firstname + lastname
            StringBuilder sb = new StringBuilder();
            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime time = DateTime.Now;
            // use a string builder to create the dynamic string
            sb.AppendFormat("Values('{0}', '{1}','{2}','{3}','{4}','{5}',{6},{7})", player.Email, player.NickName, time.ToString(format), player.Locale, player.ProfilePic, player.Uid, 25, 0);
            String prefix = "INSERT INTO Player " + "(UserEmail, UserNickname,CreatedAt,Locale,profile_pic,uid,cashMole,numOfWinnings) ";
            command = prefix + sb.ToString();

            return command;
        }

        public bool CheckUser(string adminMail, string adminPassword)
        {
            bool userInDB = false;
            SqlConnection con = null;
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file
                string query = "select * from Admin where AdminEmail ='" + adminMail + "'";
                SqlCommand cmd = new SqlCommand(query, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    if (dr["AdminPassword"].ToString() == adminPassword)
                    {
                        userInDB = true;
                    }
                }

                return userInDB;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }
        }
        public Player GetPlayer(string uid)
        {
            SqlConnection con = null;
            Player p = new Player();
            try
            {
                con = connect("DBConnectionString");

                String selectSTR = "SELECT * FROM Player where uid='" + uid + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    p.ProfilePic = dr["profile_pic"].ToString();
                    p.NickName = dr["UserNickname"].ToString();
                    p.Email = dr["UserEmail"].ToString();
                    p.BirthDate = dr["birthDate"].ToString();
                    p.Gender = dr["gender"].ToString();

                    if (dr["numOfWinnings"].ToString() == "")
                    {
                        p.NumOfWinnings = 0;
                    }
                    else p.NumOfWinnings = int.Parse(dr["numOfWinnings"].ToString());

                    if (dr["cashMole"].ToString() == "")
                    {
                        p.CashMole = 250;
                    }
                    else p.CashMole = int.Parse(dr["CashMole"].ToString());

                }

                return p;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        //---------------------------------------------------------------------------------
        // Read Players from the DB into a list - dataReader withOut Filter
        //---------------------------------------------------------------------------------
        public List<Player> GetPlayers(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Player> lp = new List<Player>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Player p = new Player();
                    p.NickName = dr["UserNickname"].ToString();
                    p.Email = dr["UserEmail"].ToString();
                    p.BirthDate = dr["birthDate"].ToString();
                    p.Gender = dr["gender"].ToString();
                    if (dr["numOfWinnings"].ToString() == "")
                    {
                        p.NumOfWinnings = 0;
                    }
                    else p.NumOfWinnings = int.Parse(dr["numOfWinnings"].ToString());

                    lp.Add(p);
                }

                return lp;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Read Games from the DB into a list - dataReader withOut Filter
        //---------------------------------------------------------------------------------
        public List<Game> GetGames(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Game> lg = new List<Game>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Game g = new Game();
                    g.Id = Convert.ToInt32(dr["GameID"]);
                    g.GameDate = Convert.ToString(dr["GameDate"]);
                    g.GameDuration = Convert.ToString(dr["gameDuration"]);
                    g.GameStartTime = Convert.ToString(dr["GameStartTime"]);
                    g.GameFinishTime = Convert.ToString(dr["GameFinishTime"]);


                    lg.Add(g);
                }

                return lg;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        //---------------------------------------------------------------------------------
        // Read Categories from the DB into a list - dataReader withOut Filter
        //---------------------------------------------------------------------------------
        public List<string> GetCategories(string conString, string tableName)
        {

            SqlConnection con = null;
            List<string> lc = new List<string>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    //Category c = new Category();
                    //c.Id = Convert.ToInt32(dr["categoryID"]);
                    //c.Name = Convert.ToString(dr["categoryName"]);
                    String s = Convert.ToString(dr["categoryName"]);
                    lc.Add(s);
                }

                return lc;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public List<Category> GetAmountForCategories(string conString, List<string> categories)
        {

            SqlConnection con = null;
            List<Category> lc = new List<Category>();

            foreach (string item in categories)
            {
                try
                {
                    con = connect("DBConnectionString");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                String CommandSTR = amountVerticiesCommand(item); //v
                String CommandSTR2 = getAmountCommand(item);  //e
                try
                {
                    SqlCommand cmd = command(con, CommandSTR);

                    // get a reader
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end
                    Category c = new Category();
                    while (dr.Read())
                    {   // Read till the end of the data into a row
                        
                        //c.Id = Convert.ToInt32(dr["categoryID"]);
                        c.Name = item;
                        c.VerteciesInCat = Convert.ToInt32(dr[0]);
                    }
                    if (con != null)
                    {
                        con.Close();
                    }
                    try
                    {
                        con = connect("DBConnectionString");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    SqlCommand cmd2 = command(con, CommandSTR2);

                    // get a reader
                    SqlDataReader dr2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                    while (dr2.Read())
                    {   // Read till the end of the data into a row

                        c.EdgesInCat = Convert.ToInt32(dr2[0]);

                        lc.Add(c);
                    }
                    if (con != null)
                    {
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    // write to log
                    throw (ex);
                }  
            }
                if (con != null)
                {
                    con.Close();
                }
            
            return lc;
        }
        
        public string amountVerticiesCommand(string categoryName)
        {
            string strV = "SELECT count(*) FROM " + categoryName + "Vertecies";  
            return strV;
        }
        public string getAmountCommand(string categoryName)
        {
            string strE = "SELECT count(*) FROM " + categoryName + "Edges";
            return  strE;
        }
        public List<Category> GetCategories2(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Category> lc = new List<Category>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Category c = new Category();
                    c.Id = Convert.ToInt32(dr["categoryID"]);
                    c.Name = Convert.ToString(dr["categoryName"]);

                    lc.Add(c);
                }

                return lc;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        //---------------------------------------------------------------------------------
        // Read Vertecies from the DB into a list - dataReader withOut Filter
        //---------------------------------------------------------------------------------
        public List<VerteciesIsInGame> GetVertecies(string conString, string tableName)
        {

            SqlConnection con = null;
            List<VerteciesIsInGame> lv = new List<VerteciesIsInGame>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName;
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    VerteciesIsInGame v = new VerteciesIsInGame();
                    v.VerteciesId = (Int32)dr["VerteciesId"];
                    v.GameID = (Int32)dr["GameID"];
                    v.VerteciesPosition = (Int32)dr["VerteciesPosition"];

                    lv.Add(v);
                }

                return lv;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Read Vertex information from the DB - dataReader with Filter
        //---------------------------------------------------------------------------------
        public Node GetVertexInfo(string tableName, string nodeName)
        {

            SqlConnection con = null;
            Node n = new Node();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + "  WHERE TitlePage= '" + nodeName + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    n.NodeNum = dr["TitlePage"].ToString();
                    n.NodeImageURL = dr["nodeImageURL"].ToString();
                    n.NodeDescription = dr["nodeDescription"].ToString();

                }

                return n;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        public string getToken(string uid)
        {
            SqlConnection con = null;
            string token = "";
            try
            {
                con = connect("DBConnectionString");
                String selectSTR = "SELECT NotificationToken FROM Player where uid='" + uid + "'";
                SqlCommand cmd = new SqlCommand(selectSTR, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    token = dr["NotificationToken"].ToString();
                }

                return token;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }
        //---------------------------------------------------------------------------------
        // Read Players WHO SIGNED UP TODAY from the DB into a list - dataReader with Filter
        //---------------------------------------------------------------------------------

        public List<Player> TodaysPlayers(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Player> lp = new List<Player>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT DISTINCT UserEmail, UserNickname, LastLogin FROM " + tableName + "  WHERE datediff(hour,lastlogin,getdate())<=24";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Player p = new Player();
                    p.Email = dr["UserEmail"].ToString();
                    p.NickName = dr["UserNickname"].ToString();
                    p.LastLogin = dr["LastLogin"].ToString();

                    lp.Add(p);
                }

                return lp;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        //---------------------------------------------------------------------------------
        // Read Players WHO SIGNED IN This MONTH from the DB into a list - dataReader with Filter
        //---------------------------------------------------------------------------------
        public List<Player> MonthPlayers(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Player> lp = new List<Player>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT DISTINCT * FROM " + tableName + "  WHERE datediff(DAY,CreatedAt,getdate())<=30";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Player p = new Player();
                    p.Email = dr["UserEmail"].ToString();
                    p.NickName = dr["UserNickname"].ToString();
                    p.CreatedAt1 = dr["CreatedAt"].ToString();

                    lp.Add(p);
                }

                return lp;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Read Games WHO Created This MONTH from the DB into a list - dataReader with Filter
        //---------------------------------------------------------------------------------
        public List<Game> MonthGames(string conString, string tableName)
        {

            SqlConnection con = null;
            List<Game> lg = new List<Game>();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT DISTINCT * FROM " + tableName + "  WHERE datediff(DAY,GameDate,getdate())<=30";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Game g = new Game();
                    g.Id = Convert.ToInt32(dr["GameID"]);
                    g.GameDate = dr["GameDate"].ToString();

                    lg.Add(g);
                }

                return lg;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public Admin GetAdmin(string conString, string tableName, string email)
        {

            SqlConnection con = null;
            Admin a = new Admin();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + "  WHERE AdminEmail= '" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    a.NickName = dr["AdminNickname"].ToString();
                    a.URL = dr["Pic"].ToString();
                }

                return a;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Read Players with highest num of wins from the DB into a list - dataReader with Filter
        //---------------------------------------------------------------------------------

        public Player PlayerOfTheGame(string conString, string tableName)
        {

            SqlConnection con = null;
            Player p = new Player();
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM " + tableName + "  WHERE numOfWinnings = (Select Max(numOfWinnings) From Player)";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    p.NickName = dr["UserNickname"].ToString();
                    p.NumOfWinnings = (Int32)dr["numOfWinnings"];
                    p.ProfilePic = dr["profile_pic"].ToString();
                }

                return p;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public string UserInsertCommand(User user)
        {
            string prefix = "INSERT INTO userPlayer(userEmail, userPassword, userName, gender)";
            string cmdValues = string.Format("VALUES('{0}', '{1}', '{2}', '{3}')", user.UserEmail, user.UserPassword, user.UserName, user.Gender);
            return prefix + cmdValues;
        }

        public List<User> getAllusers()
        {
            List<User> ManagersList = new List<User>();
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                String selectSTR = "SELECT * FROM userPlayer";
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    User m = new User();
                    m.UserEmail = (string)dr["userName"];
                    m.UserPassword = (string)dr["email"];

                    ManagersList.Add(m);
                }

                return ManagersList;
            }
            catch (Exception ex)
            {

                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public Network GetNetInfo(string categoryName)
        {

            SqlConnection con = null;
            Network n = new Network();
            List<Node> nodesList = new List<Node>();
            List<Link> linksList = new List<Link>();
            string verTableName = categoryName + "Vertecies";
            string edgeTableName = categoryName + "Edges";
            try
            {
                con = connect("DBConnectionString");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            String selectVer = "SELECT * FROM " + verTableName;
            String selectEdg = "SELECT * FROM " + edgeTableName;

            try
            {
                SqlCommand cmd = command(con, selectVer);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr.Read())
                {   // Read till the end of the data into a row
                    Node node = new Node();
                    node.NodeNum = dr["TitlePage"].ToString();
                    nodesList.Add(node);
                }
                n.Nodes = nodesList;
                if (con != null)
                {
                    con.Close();
                }
                try
                {
                    con = connect("DBConnectionString");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                SqlCommand cmd2 = command(con, selectEdg);

                // get a reader
                SqlDataReader dr2 = cmd2.ExecuteReader(CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                while (dr2.Read())
                {   // Read till the end of the data into a row
                    Link link = new Link();
                    link.SourceNode = dr2["OriginPage"].ToString();
                    link.TargetNode = dr2["LinkedPage"].ToString();
                    linksList.Add(link);
                }
                n.Links = linksList;
                if (con != null)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
        
                if (con != null)
                {
                    con.Close();
                }
            
            return n;    
        }
    }
}