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


        ////////////////// inserting //////////////////////////


        public void InsertPlaylistItem(PlaylistItem item)
        {
            string query = @"insert into playlistitem(content_playable, parent_playlistItem, `index`) values
                   (@id, @parent_id, @index);";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@parent_id", item.Path_id.Last());
            cmd.Parameters.AddWithValue("@index", item.Index);

            cmd.ExecuteNonQuery();

            item.ID = (int)cmd.LastInsertedId;
        }


        public void InsertPlaylist(Playlist playlist, PlaylistItem pl_item)
        {
            string query =
                String.Format(
                    @"insert into playable(name, image, duration, composer) values
                   ('{0}', '{1}', Time('{2:hh:mm:ss}'), '{3}');
                    set @id := last_insert_id();
                    insert into playlist(playable_list_id) values
                   (@id);",
                   playlist.Title, string.Empty /*image_uri*/, playlist.Time, playlist.Composer);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();

                    int id = (int)(new MySqlCommand("select @id;", connection)).ExecuteScalar();
                    playlist.ID = id;

                    InsertPlaylistItem(pl_item);
                }
                catch (Exception e)
                {
                    int a = 1 + 1;
                }

                this.CloseConnection();
            }
        }

        //additional functions:
        public void InsertTrack(Track track, PlaylistItem pl_item)
        {
            string query =
            String.Format(
                @"insert into playable(name, image, duration, composer) values
                       ('{0}', '{1}', Time('{2:hh:mm:ss}'), '{3}');
                        set @id := last_insert_id();
                        insert into playlist(playable_list_id) values
                       (@id);",
                track.Title, string.Empty /*image_uri*/, track.Time, track.Composer);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();

                    int id = (int)(new MySqlCommand("select @id;", connection)).ExecuteScalar();
                    track.ID = id;

                    InsertPlaylistItem(pl_item);
                }
                catch (Exception e)
                {
                    int a = 1 + 1;
                }

                this.CloseConnection();
            }
        }
    }
}
