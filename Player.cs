using System;
using System.IO;

namespace Beatmap_Guesser
{
    [Serializable()]
    public class Player
    {
        /** Class which represents the user of the application. 
         * Serialization is used to store and retrieve lifetime user statistics.
         */

        public string Name { get; set; }
        public int TotalGuessed { get; set; }
        public int CorrectlyGuessed { get; set; }
        public string password { get; set; }

        public Player()
        {

        }

        public Player(string name)
        {
            this.Name = name;
            this.TotalGuessed = 0;
            this.CorrectlyGuessed = 0;
        }

        public static Player retrievePlayer(string name)
        {
            /** Retrieves the correct player object based on the provided username.
            * 
            * params: 
            * name - the username of the Player to be retrieved
            */

            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Player));
            var dir_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\osu! Beatmap Guesser\\userdata";
            string file_path = name + ".xml";
            string full_filepath = Path.Combine(dir_path, file_path);

            StreamReader file = new StreamReader(full_filepath);
            Player returning_player = (Player)reader.Deserialize(file);
            file.Close();

            return returning_player;
        }

        public void savePlayer()
        {
            /** Stores the current player object, to later be retrieved for displaying user statistics.
            */

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Player));
            var dir_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\osu! Beatmap Guesser\\userdata";
            string file_path = this.Name + ".xml";
            string full_filepath = Path.Combine(dir_path, file_path);

            if (Directory.Exists(dir_path))//path already exists, only create file
            {

                if (File.Exists(full_filepath))//file exists
                {
                    File.Delete(full_filepath);//delete existing file
                }
            }
            else //neither dir nor file exists
            {
                Directory.CreateDirectory(dir_path);

            }

            FileStream file = File.Create(full_filepath);
            writer.Serialize(file, this);
            file.Close();

        }
    }
}