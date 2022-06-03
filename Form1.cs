using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;


namespace Beatmap_Guesser
{
    public partial class GameDisplay : Form
    {
        public string currentGuess { get; private set; }
        public string guessMessage { get; private set; }
        public Song currentSong { get;  set; }
        public int correctCount { get; private set; }
        public ArrayList songList { get; private set; }

        public FilepathForm filepathForm = new FilepathForm();

        public bool buttonFlag { get; set; } = false;
        public int currentSongIndex { get; set; } = 0;
        public string difficulty { get; set; }

        public GameDisplay(string difficulty)
        {

            InitializeComponent();
            this.difficulty = difficulty;
            Load += Form1_Load1;
            Shown += Form1_Shown1;
          
        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            
            this.currentSong = (Song)this.songList[0];//may be null


            this.Controls.Add(pictureBox1);
            this.Controls.Add(answerBox);
            this.Controls.Add(textBox2);
                
                if (this.currentSong.imagePath != null)
                {
                    renderImage();
                }
                else
                {
                    Song newCurrent = (Song)this.songList[this.songList.IndexOf(this.currentSong) + 1];
                    this.currentSong = newCurrent;//skip current image, render next THIS DOES NOT CHANGE THE DATA DISPLAYED

                    renderImage();//render new image
                }
  
        }

        private void renderImage()
        {

            Image image = null;

            image = this.currentSong.getImage();
            
            if (image == null) image = SongHandler.getSafetyImage(SongHandler.current_path);//catch bad image, will still error if NO IMAGES IN SONG FOLDER

            pictureBox1.Image = image;
            pictureBox1.Height = image.Height;
            pictureBox1.Width = image.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            textBox2.Text = "Selected image: " + currentSong.imagePath;
        }

        private void Form1_Shown1(object sender, EventArgs e)
        {
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
       
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.currentGuess = answerBox.Text;
            this.buttonFlag = true;

            validateGuess(this.currentSong, this.currentGuess);
            MessageBox.Show(this.guessMessage);//display round results to user

            currentSongIndex++;
            try
            {
                this.currentSong = (Song)this.songList[currentSongIndex];
            }
            catch (Exception ex)
            {
                Console.WriteLine("The game has ended.");
                Application.Exit();
            }
            renderImage();

            this.Show();

        }
        private void OnRefresh(object sender, EventArgs e)
        {
            renderImage();
            this.buttonFlag = false;
        }

        public void start()
        {

                DialogResult result = this.filepathForm.getDialogResult();
                var selectedPath = this.filepathForm.getFilePath();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(selectedPath))
                {

                    string[] list_of_song_paths = Directory.GetDirectories(selectedPath);
                    MessageBox.Show("Your selected osu! Songs folder: " + selectedPath + "\n", "Message");
                    SongHandler sh = new SongHandler();

                    this.songList = sh.createSongs(list_of_song_paths);

                    MessageBox.Show("You've created " + songList.Count + " songs automatically!");

                    this.ShowDialog();

                    while (true)
                    {

                    }
                }
            }


        public bool validateGuess(Song currentSong, string guess)
        {

            this.guessMessage = "Error during guess validation.";

            int guessDistance = GetStringDistance(currentSong.song_name, guess);

            if (guessDistance >= 0 && guessDistance <= 5)
            {
                if (guessDistance == 0)
                {
                    this.guessMessage = "Perfect! You were spot on with " + guess + ".";
                    this.correctCount++;
                }
                else
                {
                    this.guessMessage = "Close enough! The answer is " + currentSong.song_name + ".";
                    this.correctCount++;
                }
                return true;
            }
            else
            {
                this.guessMessage = "Not quite! The correct answer was " + currentSong.song_name + ".";
                return false;
            }

        }

        public static int GetStringDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

    }
}
