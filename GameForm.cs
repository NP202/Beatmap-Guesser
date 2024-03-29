﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Beatmap_Guesser
{
    public partial class GameDisplay : Form
    {
        /** Main form of the application. The GameDisplay class handles all game logic, as well as guess validation and image display. 
         * The class can be initialized with one of three difficulties - Easy, Normal, Hard. These change the proportions of the image that is shown, 
         * from full, to 50% of the original image, with and without an additional "zoom out" option respectively. 
         * The class is also double-buffered to minimize graphical errors that the user may potentially see while attempting to display an image.
         */

        public string currentGuess { get; private set; }
        public string guessMessage { get; private set; }
        public Song currentSong { get; set; }
        public int correctCount { get; private set; }
        public int totalCount { get; private set; }
        public ArrayList songList { get; private set; }

        public FilepathForm filepathForm = new FilepathForm();

        public bool buttonFlag { get; set; } = false;
        public int currentSongIndex { get; set; } = 0;
        public string difficulty { get; set; }
        public static Player player { get; set; }
        public Image zoomedImage { get; set; } = null;

        public static string selectedPath { get; set; }
        public static string[] list_songpaths { get; set; }

        public GameDisplay(string difficulty)
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.difficulty = difficulty;

            Load += Form1_Load1;
            Shown += Form1_Shown1;

        }

        private void Form1_Load1(object sender, EventArgs e)
        {
            /**Auto-generated form load method. All relevant objects are added to the Controls collection, and the next image is rendered.
             */

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
                this.currentSong = newCurrent;//skip current image, render next

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
            /** Randomly partitions the current Song's image so that only a portion of it is displayed to the user on Normal and Hard modes. 
             * The unmodified image is also stored as a class variable to be displayed with the clicking of the "Zoom" button on Normal Mode.
             */

            Image image = this.currentSong.getImage();
            if (image == null) image = SongHandler.getSafetyImage(SongHandler.current_path);//catch bad image

            var bitmap = (Bitmap)image;

            int max_x = image.Size.Width;
            int max_y = image.Size.Height;

            int cut_bound_x = (int)(max_x * .5);
            int cut_bound_y = (int)(max_y * .5);

            Random r = new Random();
            int random_x = r.Next(1, max_x - cut_bound_x);//set cursor (top left of image), guaranteed to not be out of bounds
            int random_y = r.Next(1, max_y - cut_bound_y);

            Rectangle clone = new Rectangle(random_x, random_y, (int)(cut_bound_x * .5), (int)(cut_bound_y * .5));

            if (difficulty == "Normal")
            {
                Rectangle new_clone = new Rectangle(random_x, random_y, cut_bound_x, cut_bound_y);
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
            /** Standard image rendering, showing the entire Song image to the user.
             */

            Image image = null;

            image = this.currentSong.getImage();

            if (image == null) image = SongHandler.getSafetyImage(SongHandler.current_path);//catch bad image

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
                MessageBox.Show("Error during image generation - invalid difficulty.");

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

        public void showResults()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /** Handles guess validation on submission and displays the result to the user, 
             * before refreshing the display to the next image to be shown.
             */

            this.currentGuess = answerBox.Text;
            this.buttonFlag = true;

            validateGuess(this.currentSong, this.currentGuess);
            MessageBox.Show(this.guessMessage);//display round results to user

            currentSongIndex++;
            try
            {

                if (this.difficulty == "Normal")
                {
                    ZoomButton.Visible = true;
                }

                answerBox.Clear();//resets guess text per-image
                this.currentSong = (Song)this.songList[currentSongIndex];
            }
            catch (Exception ex)//game ends
            {
                HomeScreen.first_game_flag = false;
                Console.WriteLine("The game has ended.");
                HomeScreen hs = new HomeScreen();
                player.TotalGuessed += this.totalCount;
                player.CorrectlyGuessed += this.correctCount;
                this.ShowInTaskbar = false;

                this.Close();
                hs.ShowDialog();

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
            /** Starts the game, first prompting the user to select their osu! Songs folder path with a new FilepathForm. 
             * The selected path is statically stored so the user does not have to re-select it every time they play.
             */
             
            if (!HomeScreen.first_game_flag) //secondary game
            {
                this.generateSongs(selectedPath, list_songpaths);

                FormCollection collection = Application.OpenForms;

                IEnumerable<HomeScreen> ie = collection.OfType<HomeScreen>();
                List<HomeScreen> list = ie.ToList();

                foreach (HomeScreen screen in list) screen.Dispose(); //Dispose of all unattended HomeScreen forms, so they do not clutter the application window. 

                this.ShowDialog();

            }
            else //first game
            {

                DialogResult result = this.filepathForm.getDialogResult();

                if (result == DialogResult.Cancel || result == DialogResult.Abort) //disallow the user from not selecting a folder
                {
                    start();
                }

                else if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(this.filepathForm.getFilePath())) //if properly selected, set static variables here
                {

                    selectedPath = this.filepathForm.getFilePath();
                    list_songpaths = Directory.GetDirectories(selectedPath);
                    generateSongs(selectedPath, list_songpaths);

                    FormCollection collection = Application.OpenForms;

                    IEnumerable<HomeScreen> ie = collection.OfType<HomeScreen>();
                    List<HomeScreen> list = ie.ToList();

                    foreach (HomeScreen screen in list) screen.Dispose();

                    this.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Uncaught DialogResult.");
                }

            }

        }

        public void generateSongs(string path, string[] song_paths_list)
        {

            //catch the user failing to put in a value with the default

            if (HomeScreen.first_game_flag) MessageBox.Show("Your selected osu! Songs folder: " + path + "\n", "Message");

            SongHandler sh = new SongHandler();

            this.songList = sh.createSongs(song_paths_list);

            MessageBox.Show("You've created " + songList.Count + " songs automatically!");

        }

        public bool validateGuess(Song currentSong, string guess)
        {
            /**Validates the user's guess by calculating the String Distance from the guess to the correct answer. 
             * Results are then displayed depending on how close the user was to the correct answer.
             */

            this.guessMessage = "Error during guess validation.";
            int correctLength = currentSong.song_name.Length;
            int guessDistance = GetStringDistance(currentSong.song_name, guess);
            int correctBound = (int)(correctLength / 4);


            this.totalCount++;
            if (guessDistance >= 0 && guessDistance <= correctBound)
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

            /** Given two strings, returns the number of "steps" required to transform one string into the other. 
             * This is calculated as a cost which is incremented for each necessary character swap, insertion, deletion, or transposition.
             * 
             * params:
             * s - string whose distance from t is to be calculated 
             * t - second string used in distance calculation
             */

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
            /** Sets the current image to the zoomed Image once the button is clicked on Normal mode.
            */

            pictureBox1.Image = this.zoomedImage;
            pictureBox1.Height = this.zoomedImage.Height;
            pictureBox1.Width = this.zoomedImage.Width;
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;

            ZoomButton.Visible = false;

        }
    }
}
