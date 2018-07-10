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
            server = "sql11.freemysqlhosting.net";
            database = "sql11230005";
            uid = "sql11230005 ";
            password = "LHi1bsmgfd";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";" +
            "Allow User Variables=True;" +
            "SslMode=none;";

            connection = new MySqlConnection(connectionString);

        }


        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
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
                    Playlist t = new Playlist(id, name, null, duration, times_played, composer, null);
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
            for (i = j = 0; i < tracks.Count && j < playlists.Count;)
            {
                ret.Add(tracks[i].Index < playlists[j].Index ? tracks[i++] : playlists[j++]);
            }
            for (; i < tracks.Count; i++)
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
            string tblName = ("res_set" +
                    (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).Replace(".", "_");
            string query = @"insert into playlistitem(content_playable, parent_playlistItem, `index`) values
                   ({0}, {1}, {2});
                    
                    create table {3} (select idplaylistItem from playlistitem
                    where parent_playlistItem = {1} and `index` > {2});
                    update playlistitem set `index` = `index` + 1 where idplaylistItem in (select * from {3});
                    drop table if exists {3};";

            MySqlCommand cmd = new MySqlCommand(String.Format(query, item.Content.ID, item.ParentID, item.Index, tblName), connection);

            cmd.ExecuteNonQuery();

            item.ID = (int)cmd.LastInsertedId;
        }


        public int InsertPlaylist(Playlist playlist, PlaylistItem pl_item)
        {
            string query =
                String.Format(
                    @"insert into playable(name, image, duration, composer) values
                   ('{0}', '{1}', Time('{2:hh\:mm\:ss}'), '{3}');",
                   playlist.Title, string.Empty /*image_uri*/, playlist.Time, playlist.Composer);

            string insertPlaylist = "insert into playlist(idplaylist) values({0});";

            int id = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();

                    id = (int)cmd.LastInsertedId;

                    new MySqlCommand(String.Format(insertPlaylist, id), connection).ExecuteNonQuery();

                    playlist.ID = id;

                    InsertPlaylistItem(pl_item);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }

                this.CloseConnection();
            }

            return id;
        }

        public int InsertTrack(Track track, PlaylistItem pl_item)
        {
            string query =
                String.Format(
                    @"insert into playable(name, image, duration, composer) values
                   ('{0}', '{1}', Time('{2:hh\:mm\:ss}'), '{3}');",
                   track.Title, string.Empty /*image_uri*/, track.Time, track.Composer);

            string insertTrack = "insert into track(idtrack, uri) values" +
                "({0}, '{1}');";

            int id = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {

                    cmd.ExecuteNonQuery();

                    id = (int)cmd.LastInsertedId;
                    
                    new MySqlCommand(String.Format(insertTrack, id, track.FileUri), connection).ExecuteNonQuery();

                    track.ID = id;

                    InsertPlaylistItem(pl_item);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                }

                this.CloseConnection();
            }

            return id;
        }

        public void RemoveItem(PlaylistItem item)
        {
            string query =
            String.Format(
                @"delete from track where idtrack = {0};
                delete from playlist where idplaylist = {0};
                delete from playable where idplayable = {0};
                delete from playlistitem where idplaylistItem = {2};
                set @res_set := (
                    select idplaylistitem from playlistitem where `index` > {1}
                );
                update playlistitem set `index` = `index` - 1 where idplaylistitem in (select @res_set);",
                item.Content.ID, item.Index, item.ID);

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }

                this.CloseConnection();
            }
        }


        public void Move(PlaylistItem plItem, int toPosition)
        {
            string q_base = @"set @res_set := (
                        select idplaylistItem from playlistitem
                        where parent_playlistItem = {2} and `index` between {3} and {4});";
            string shiftOthersUp = q_base + @"update playlistitem set `index` = {0} where content_playable = {1}
                update playlistitem set `index` = `index` + 1 where `index` in (select @res_set);";
            string shiftOthersDown = q_base + @"update playlistitem set `index` = {0} where content_playable = {1}
                update playlistitem set `index` = `index` - 1 where `index` in (select @res_set);";


            if (this.OpenConnection() == true)
            {
                try
                {
                    if (plItem.Index > toPosition)
                    {
                        new MySqlCommand(String.Format(
                            shiftOthersUp, toPosition, plItem.Content.ID, plItem.ParentID, toPosition, plItem.Index), connection).ExecuteNonQuery();
                    }
                    else
                    {
                        new MySqlCommand(String.Format(
                            shiftOthersDown, toPosition, plItem.Content.ID, plItem.ParentID, plItem.Index, toPosition), connection).ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                }

                this.CloseConnection();
            }
        }


        public void IncreaseTimesPlayed(Playable item)
        {
            string query = @"update playable
                            set times_played = times_played + 1
                            where `idplayable` = @id";


            if (this.OpenConnection() == true)
            {
                try
                {

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", item.ID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                }

                this.CloseConnection();
            }
        }

    }
}
