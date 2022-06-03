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
	public class Player
	{
		public string Name { get; set; }
		public int TotalGuessed { get; set; }
		public int CorrectlyGuessed { get; set;}

		
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

		public Player retrievePlayer()
        {
			//get saved player
			return new Player();
        }










	}
}