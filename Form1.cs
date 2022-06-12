using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        public int totalCount { get; private set; }
        public ArrayList songList { get; private set; }

        public FilepathForm filepathForm = new FilepathForm();

        public bool buttonFlag { get; set; } = false;
        public int currentSongIndex { get; set; } = 0;
        public string difficulty { get; set; }
        public Player player { get; private set; }
        public Image zoomedImage { get; set; } = null;

        public GameDisplay(string difficulty, Player player)
        {

            InitializeComponent();
            this.difficulty = difficulty;
            this.player = player;
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

            if (this.difficulty == "Normal")
            {
                ZoomButton.Enabled = true;
                ZoomButton.Visible = true;
            }

        }
        public void renderImageSplice()
        {

            Image image = this.currentSong.getImage();
            if (image == null) image = SongHandler.getSafetyImage(SongHandler.current_path);//catch bad image, will still error if NO IMAGES IN SONG FOLDER
            var bitmap = (Bitmap)image;

            int max_x = image.Size.Width;
            int max_y = image.Size.Height;  

            Random r = new Random();
            int random_x = r.Next(1, max_x - 256);
            int random_y = r.Next(1, max_y - 256);
            
            Rectangle clone = new Rectangle(random_x, random_y, 256, 256);

            if (difficulty == "Normal")
            {
                random_x = r.Next(1, max_x - 512);
                random_y = r.Next(1, max_y - 512);
                Rectangle new_clone = new Rectangle(random_x, random_y, 512, 512);
                this.zoomedImage = bitmap.Clone(new_clone, PixelFormat.Format32bppRgb);
            }

            Bitmap cloned_image = bitmap.Clone(clone, PixelFormat.Format32bppRgb);

            pictureBox1.Image = cloned_image;
            pictureBox1.Height = cloned_image.Height;
            pictureBox1.Width = cloned_image.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            textBox2.Text = "Selected image: " + currentSong.imagePath;
        }


        private void renderImageEasy()
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

        private void renderImage()
        {
            if (difficulty == "Hard" || difficulty == "Normal")
            {
                renderImageSplice();
            }
            else if (difficulty == "Easy")
            {
                renderImageEasy();
            }
            else
            {
                MessageBox.Show("Error during image generation - invalid difficulty");
                
            }
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
                HomeScreen hs = new HomeScreen();
                player.TotalGuessed += this.totalCount;
                player.CorrectlyGuessed += this.correctCount;
                MessageBox.Show("You answered " + player.CorrectlyGuessed + " correct out of " + player.TotalGuessed);
                this.Hide();
                hs.ShowDialog();
                this.Close();

            }
            renderImage();

            if (this.difficulty == "Normal")//re-enable button for each new image
            {
                ZoomButton.Enabled = true;
                ZoomButton.Visible = true;
            }

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

                    while (true)//necessary for form to stay open
                    {

                    }
                }
            }


        public bool validateGuess(Song currentSong, string guess)
        {

            this.guessMessage = "Error during guess validation.";

            int guessDistance = GetStringDistance(currentSong.song_name, guess);
            this.totalCount++;
            if (guessDistance >= 0 && guessDistance <= 5)
            {
                if (guessDistance == 0)
                {
                    this.guessMessage = "Perfect! You were spot on with " + currentSong.song_name + ".";
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
            string s_lower = s.ToLower();
            string t_lower = t.ToLower();

            var bounds = new { Height = s_lower.Length + 1, Width = t_lower.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s_lower[height - 1] == t_lower[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s_lower[height - 1] == t_lower[width - 2] && s_lower[height - 2] == t_lower[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
         
            pictureBox1.Image = this.zoomedImage;
            pictureBox1.Height = this.zoomedImage.Height;
            pictureBox1.Width = this.zoomedImage.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

            ZoomButton.Visible = false;

        }
    }
}
