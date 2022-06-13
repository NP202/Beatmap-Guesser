using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Beatmap_Guesser
{
	[Serializable()]
	public class Player
	{
		public string Name { get; set; }
		public int TotalGuessed { get; set; }
		public int CorrectlyGuessed { get; set; }
		public string password { get; set; }	

		public Player()
		{

		}

		public Player(string name)
		{
			//new player
			this.Name = name;
			this.TotalGuessed = 0;
			this.CorrectlyGuessed = 0;
		}

		public static Player retrievePlayer(string name)
		{

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
	
			System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Player));
			var dir_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\osu! Beatmap Guesser\\userdata";
			string file_path = this.Name + ".xml";
			string full_filepath = Path.Combine(dir_path, file_path);

			if (Directory.Exists(dir_path)){//path already exists, only create file

				if (File.Exists(full_filepath))//file exists
				{
					File.Delete(full_filepath);//delete existing file
				}
				//dir exists, file does not
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