﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Beatmap_Guesser
{
    public class SongHandler

    {
        private const int MAX_SONGS = 100;
        public SongHandler()
        {

        }

        public ArrayList createSongs(string[] list_of_beatmap_paths)
        {
            ArrayList songs = new ArrayList();

            foreach (string beatmap_path in selectRandom(list_of_beatmap_paths))
            {
                Song newSong = new Song(grabImageFromSongFolder(beatmap_path), getSongTitle(beatmap_path), getSongArtist(beatmap_path));
                bool isDupe = false;

                int count = songs.Count;

                for (int i = 0; i < count && count <= songs.Count; i++)//logic: we are generated up to songs.Count assuming no errors, but we have to increment this variable each time an error is encountered
                {
                    if (((Song)songs[i]).imagePath == newSong.imagePath)//successfully checks for duplicates
                    {
                        isDupe = true; 
                    }

                    if (newSong.imagePath == null)//no valid bg
                    {//given that the song doesn't have a valid background
                        string[] temp = selectRandom(list_of_beatmap_paths);
                        list_of_beatmap_paths = temp;//re-randomize selection so that we don't end up short or out of bounds
                        count++; //so loop doesn't exit prematurely
                        Console.WriteLine("HIT NULL IMAGEPATH");
                        int index = Array.IndexOf(list_of_beatmap_paths, newSong);
                        //beatmap_path = selectRandomSingle(list_of_beatmap_paths)[0];
                        list_of_beatmap_paths[index] = selectRandomSingle(list_of_beatmap_paths)[0];
                    }
                    Console.WriteLine("CURRENT COUNT: " + count);
                }

                if (!isDupe)
                {
                    songs.Add(newSong);
                    Console.WriteLine("NEW Song added! Image path: " + newSong.imagePath);
                }
            }

            return songs;
        }

        public string[] selectRandom(string[] largerArray)
        {
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

            ArrayList temp = new ArrayList(largerArray);//copy over so original filepath array is not overwritten

                int randomIndex = random.Next(0, temp.Count);
                randomArray[0] = (string)temp[randomIndex];//increasing randomArray index, random temp index since it doesn't matter where they are stored in randomArray
                //so long as they are random BEFORE being sent there
                temp.Remove(randomIndex);

            return randomArray;
        }

        public string grabImageFromSongFolder(string path)
        {
            string[] temp = Directory.GetFiles(path, "*.osu");
            ArrayList dotOsuPaths = new ArrayList();
            dotOsuPaths.AddRange(temp);//now an array list

            string imagePath = "DEFAULT DANCE";

            if (dotOsuPaths.Count > 0)
            {

                foreach (string dotOsuPath in dotOsuPaths)
                {

                    //this SHOULD grab the background shit, dont know if takes the quotes though.
                    //string backgroundPath = File.ReadLines(dotOsuPath).SkipWhile(line => !line.Contains(",\"")).TakeWhile(line => !line.Contains("\",")).ToString();

                    Boolean bgFound = false;

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
            string[] dotOsuPaths = Directory.GetFiles(path, "*.osu");//Errors when there are another identical parent folder to go through
            string artist = "";
            IEnumerable<string> temp;

            try
            {
                temp = File.ReadLines(dotOsuPaths[0]);//regular non-dogshit folder
               
            }
            catch (Exception ex)
            {
                var temp_path = Directory.GetDirectories(path);//dogshit folder, requires me to delve one subfolder deeper in order to access .osu's
                temp = Directory.GetFiles(path, "*.osu");
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

        public string getSongTitle(string path)
        {
            string[] dotOsuPaths = Directory.GetFiles(path, "*.osu");//Errors when there are another identical parent folder to go through
            string title = "";
            IEnumerable<string> temp;

            try
            {
                temp = File.ReadLines(dotOsuPaths[0]);//regular non-dogshit folder

            }
            catch (Exception ex)
            {
                var temp_path = Directory.GetDirectories(path);//dogshit folder, requires me to delve one subfolder deeper in order to access .osu's
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