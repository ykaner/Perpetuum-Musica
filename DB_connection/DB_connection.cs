using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DB_connection
{
    class DB_conn
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;


        public DB_conn()
        {
            server = "localhost";
            database = "pm_musics";
            uid = "defuser";
            password = "12qwaszx";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" +
            "Allow User Variables=True";

            connection = new MySqlConnection(connectionString);

        }


        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex )
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                /*switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }*/
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void InsertPlaylist(int parent_id, int index, Duration duration, string name="play list", 
            string composer="unknown commposer", string image_uri=null)
        {
            string query = 
                String.Format(
                    @"insert into playable(name, image, duration, composer) values
                   ('{0}', '{1}', Time('{2:hh:mm:ss}'), '{3}');
                    set @id := last_insert_id();
                    insert into playlist(playable_list_id) values
                   (@id);
                    insert into playlistitem(content_playable, parent_playlistItem, `index`) values
                   (@id, '{4}', '{5}'); ",
                   name, image_uri, duration, composer, parent_id, index);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    int a = 1 + 1;
                }

                this.CloseConnection();
            }
        }



        public List<string>[] Select()
        {
            string query = "SELECT idplaylistitem, `index` FROM playlistItem";

            //Create a list to store the result
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            //list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["idplaylistItem"] + "");
                    list[1].Add(dataReader["index"] + "");
                    //list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }


    }
}
