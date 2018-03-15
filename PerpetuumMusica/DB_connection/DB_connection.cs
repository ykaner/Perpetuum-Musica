using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

using PerpetuumMusica.Model;

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

        public void InsertPlaylist(Playlist playlist, int parent_id, int index, Duration duration, string name="play list", 
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
                   playlist.Title, image_uri, playlist.Time, playlist.Composer, parent_id, index);

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


        private List<PlaylistItem> GetTrack(int parent_id)
        {
            string query = @"select * from playlistitem
                join playable on playable.idplayable = playlistitem.content_playable
                join track on track.idtrack = playable.idplayable
                and parent_playlistItem = @parent_id
                order by `index`;";

            List<PlaylistItem> list = new List<PlaylistItem>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@parent_id", parent_id);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    Track t = new Track((int)dataReader["idtrack"], dataReader["name"].ToString(),
                        dataReader["uri"].ToString(), null,
                        TimeSpan.Parse(dataReader["duration"].ToString()), (int)dataReader["times_played"],
                        dataReader["composer"].ToString());
                    list.Add(new PlaylistItem((int)dataReader["idplaylistitem"], (int)dataReader["index"], t));
                }

                dataReader.Close();
                this.CloseConnection();

                //return list
                return list;
            }
            else
            {
                return list;
            }
        }

        private List<PlaylistItem> GetPlaylist(int parent_id)
        {
            string query = @"select * from playlistitem
                join playable on playable.idplayable = playlistitem.content_playable
                join playlist on playlist.idplaylist = playable.idplayable
                and parent_playlistItem = @parent_id
                order by `index`;";

            List<PlaylistItem> list = new List<PlaylistItem>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@parent_id", parent_id);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = (int)dataReader["idplaylist"];
                    string name = dataReader["name"].ToString();
                    TimeSpan duration;
                    string str_duration = dataReader["duration"].ToString();
                    if (str_duration.Length <= 0)
                    {
                        duration = new TimeSpan();
                    }
                    else
                    {
                        duration = TimeSpan.Parse(str_duration);
                    }
                    int times_played = (int)dataReader["times_played"];
                    string composer = dataReader["composer"].ToString();
                    Playlist t = new Playlist(id, name, null,duration, times_played, composer, null);
                    list.Add(new PlaylistItem((int)dataReader["idplaylistitem"], (int)dataReader["index"], t));
                }

                dataReader.Close();
                this.CloseConnection();

                //return list
                return list;
            }
            else
            {
                return list;
            }
        }


        public List<PlaylistItem> RetrievePlaylist(int parent_id)
        {
            List<PlaylistItem> tracks = GetTrack(parent_id);
            List<PlaylistItem> playlists = GetPlaylist(parent_id);

            List<PlaylistItem> ret = new List<PlaylistItem>();

            int i, j;
            for(i = j = 0;i < tracks.Count && j < playlists.Count;)
            {
                ret.Add(tracks[i].Index < playlists[j].Index ? tracks[i++] : playlists[j++]);
            }
            for(; i < tracks.Count; i++)
            {
                ret.Add(tracks[i]);
            }
            for (; j < playlists.Count; j++)
            {
                ret.Add(playlists[j]);
            }
            return ret;
        }


        public List<PlaylistItem> Select(int parentID)
        {
            string query = @"select * from playlistitem
                    join playable on playable.idplayable = playlistitem.content_playable
                    join playlist on playlist.playable_list_id = playable.idplayable
                    and parent_playlistItem = @parent_id
                    order by `index`;";

            //Create a list to store the result
            List<PlaylistItem> list = new List<PlaylistItem>();
            //list[2] = new List<PlaylistItem>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@parent_id", parentID);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    //list.Add(new PlaylistItem(dataReader["index"], new Playable())
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
