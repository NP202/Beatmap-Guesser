using System;
using System.Drawing;

namespace Beatmap_Guesser {
	public class Song

	{
		public string imagePath { get; private set; }
		public string song_name { get; private set; }
		public string artist_name { get; set; }

		public Song()
		{
			this.imagePath = "OOPS";
			this.song_name = "";
			this.artist_name = "";
		}
		public Song(string imagePath, string song_name, string artist_name)
		{
			this.imagePath = imagePath;
			this.song_name = song_name;
			this.artist_name = artist_name;
		}

		public Image getImage()
        {
			try
			{
				return Image.FromFile(this.imagePath);
			}
			catch (Exception e)
			{
				this.imagePath = null;
				return null;
			}
        }
	}
}
