using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Beatmap_Guesser
{
    public class SongHandler

    {
        /** Utility class which serves to generate the list of songs used in each round of the game.
         * Handles retrieving the images from the respective song folders, as well as randomly populating the songs ArrayList.
         */

        public static int MAX_SONGS { get; set; } = 10;//default song number per-round, user-defined amount takes precedence
        public static string current_path { get; set; }
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        public SongHandler()
        {

        }

        public ArrayList createSongs(string[] list_of_beatmap_paths)
        {
            /** Loops through the provided array of beatmap folder paths, 
            * generating Song objects for each unique element up until the MAX_SONGS limit.
            * Handles errors encountered when there is a missing/corrupt beatmap image, or a
            * duplicate song is encountered to ensure the MAX_SONGS limit is always hit. 
            *
            * params: 
            * list_of_beatmap_paths - array containing folder paths for every Song object to be generated
            *
            */

            ArrayList songs = new ArrayList();
        Dupe:
            foreach (string beatmap_path in selectRandom(list_of_beatmap_paths))
            {
                //generate a new Song object per folder path in list_of_beatmap_paths
                current_path = beatmap_path;
                Song newSong = new Song(grabImageFromSongFolder(beatmap_path), getSongTitle(beatmap_path), getSongArtist(beatmap_path));
                bool isDupe = false;

                int count = songs.Count;

                if (newSong.imagePath == null)//set dupe flag to avoid error with image path and re-generate
                {
                    isDupe = true;

                }

                for (int i = 0; i < count && count <= songs.Count; i++)//Count assuming no errors, increment this variable each time an error or duplicate song is encountered
                {
                    if (songs.Count == MAX_SONGS) return songs;

                    if (((Song)songs[i]).imagePath == newSong.imagePath)//successfully checks for duplicates
                    {
                        count++;
                        isDupe = true;

                    }

                }

                if (!isDupe)
                {
                    Console.WriteLine("CURRENT COUNT: " + count);
                    songs.Add(newSong);
                    Console.WriteLine("NEW Song added! Image path: " + newSong.imagePath);
                }
                else
                {
                    if (songs.Count == MAX_SONGS) return songs;
                    goto Dupe;//break out of loop if duplicate is encountered AND the number of songs generated != MAX_SONGS
                }
            }

            return songs;
        }

        public string[] selectRandom(string[] largerArray)
        {
            /** Utility function to select MAX_SONGS # of elements from a given array, and return a smaller array populated with
            * the randomly-chosen elements.
            *
            * params:
            * largerArray - the array from which random elements will be selected
            */
            Random random = new Random();
            string[] randomArray = new string[MAX_SONGS];

            ArrayList temp = new ArrayList(largerArray);//copy over so original  filepath array is not overwritten

            for (int i = 0; i < MAX_SONGS; i++)
            {
                int randomIndex = random.Next(0, temp.Count);
                randomArray[i] = (string)temp[randomIndex];//increasing randomArray index, random temp index since it doesn't matter where they are stored in randomArray
                                                           //so long as they are random BEFORE being sent there
                temp.Remove(randomIndex);

            }

            return randomArray;
        }

        public string[] selectRandomSingle(string[] largerArray)
        {
            Random random = new Random();
            string[] randomArray = new string[1];

            ArrayList temp = new ArrayList(largerArray);

            int randomIndex = random.Next(0, temp.Count);
            randomArray[0] = (string)temp[randomIndex];

            temp.Remove(randomIndex);

            return randomArray;
        }

        public string grabImageFromSongFolder(string path)
        {
            /** Reads through the .osu file (reflavored text file) for the given beatmap path, and attempts to grab the image from the file. 
            * This is done by locating the filepath specified in the .osu file, and stitching it together with the current directory path.
            * The same logic for combing through the .osu file to find the specified image filepath is also used to retrieve the song's metadata. 
            * 
            * params:
            * path - folder path for the current Song to be looked through
            */

            string[] temp = Directory.GetFiles(path, "*.osu");
            ArrayList dotOsuPaths = new ArrayList();
            dotOsuPaths.AddRange(temp);

            string imagePath = null;

            if (dotOsuPaths.Count > 0)
            {

                foreach (string dotOsuPath in dotOsuPaths)
                {

                    if (File.ReadAllLines(dotOsuPath).Length <= 1) return null;//checks if there is an empty .osu file

                    bool bgFound = false;

                    string backgroundPath = "";
                    foreach (string line in File.ReadLines(dotOsuPath))
                    {

                        if (!line.Contains("Video") && line.Contains(",\"") && !bgFound)
                        {
                            backgroundPath = Regex.Matches(line, "\"([^\"]*)\"").OfType<Match>().Select(m => m.Groups[0].Value).ToArray()[0];  //this eliminates the "0,0," surrounding the path
                            bgFound = true;
                        }
                        
                    }

                    string newStr = backgroundPath.Replace("\"", "");//make sure that no quotes are trying to be added to path

                    if (newStr == "" || newStr == " " || newStr == "\\" || newStr == null)
                    //checks to see if the last character is a slash or empty, meaning the beatmap does NOT have a valid background
                    {
                        return null;
                    }

                    imagePath = path + "\\" + newStr;

                }
            }

            return imagePath;
        }
        public string getSongArtist(string path)
        {
            /** Reads through the .osu file (reflavored text file) for the given beatmap path, and grabs the song artist from the file.
            * 
            * params:
            * path - folder path for the current Song to be looked through
            */
            string[] dotOsuPaths = Directory.GetFiles(path, "*.osu");
            string artist = "";
            IEnumerable<string> temp;

            try
            {
                temp = File.ReadLines(dotOsuPaths[0]);//regular folder

            }
            catch (Exception ex)
            {

                temp = Directory.GetFiles(path, "*.osu");//layered folder, requires me to delve one subfolder deeper in order to access .osu
            }

            foreach (string line in temp)
            {

                if (line.Contains("Artist:"))
                {
                    artist = line.Substring(7, line.Length - 7);
                    return artist;

                }

            }
            return artist;
        }

        public static Image getSafetyImage(string song_directory)
        {
            /** Backup method for image retrieval. Occasionally, the .osu will store a path to an image that either does not exist, or one that has since been renamed by the beatmap creator.
            * This method grabs the first image of an appropriate filetype from the root of the Song folder.
            * 
            * params:
            * song_directory - folder path for the current Song to be looked through
            */

            foreach (string filename in Directory.GetFiles(song_directory))
            {//for each file in the song folder
                if (ImageExtensions.Contains(Path.GetExtension(filename).ToUpperInvariant()))
                { //checks the path against the list of appropriate file extensions
                    return Image.FromFile(filename);
                }
            }

            return null;//no image file was found
        }

        public string getSongTitle(string path) 
        { 
           /** Reads through the .osu file (reflavored text file) for the given beatmap path, and grabs the song title from the file.
            * 
            * params:
            * path - folder path for the current Song to be looked through
            */

            string[] dotOsuPaths = Directory.GetFiles(path, "*.osu");
            string title = "";
            IEnumerable<string> temp;

            try
            {
                temp = File.ReadLines(dotOsuPaths[0]);//regular folder

            }
            catch (Exception ex)
            {
                var temp_path = Directory.GetDirectories(path);//layered folder, requires me to delve one subfolder deeper in order to access .osu
                temp = Directory.GetFiles(path, "*.osu");
            }

            foreach (string line in temp)
            {

                if (line.Contains("Title:"))
                {
                    title = line.Substring(6, line.Length - 6);
                    return title;

                }

            }
            return title;
        }

    }
}