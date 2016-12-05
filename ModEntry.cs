// GrayFoxMods
// December 5th, 2016

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.IO;
using System.Collections.Generic;


namespace WeatherClothing
{
    public class WeatherClothing : Mod
    {
        static public List<NPC> listNPC;

        private static readonly string LOG_FILENAME = @"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Mods\WeatherClothing\" + "WeatherClothing Log.txt";

        public static void LogMessageToFile(string msg)

        {

            msg = string.Format("{0:G}: {1}{2}", DateTime.Now, msg, Environment.NewLine);

            File.AppendAllText(LOG_FILENAME, msg);

        }

        public override void Entry(IModHelper helper)
        {
            this.Monitor.Log("Entered WeatherClothing Entry", LogLevel.Debug);
            //StardewModdingAPI.Events.TimeEvents.SeasonOfYearChanged += UpdateSeasonEvent;
            //StardewModdingAPI.Events.GameEvents.GameLoaded += GameLoadEvent; //PlayerEvents.LoadedGame causes crash
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += UpdateSeasonEvent;
            this.Monitor.Log("Exited WeatherClothing Entry", LogLevel.Debug);
        }

        static void UpdateSeasonEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Entered Update Season (TimeOfDayChanged)");
            if (StardewValley.Game1.currentLocation == null)
                return;
            if (StardewValley.Game1.timeOfDay == 610)
                ChangeNPC();
            Console.WriteLine("Exited Update Season (TimeOfDayChanged)");
        } // End Update Season Event

        static void GameLoadEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Entered GameLoadEvent");
            if (StardewValley.Game1.currentLocation == null)
                return;

            if (StardewValley.Game1.timeOfDay == 610)
                ChangeNPC();
            Console.WriteLine("Exited Game Load");
        }

        public static void ChangeNPC()
        {
            LogMessageToFile("Entered ChangeNPC");
            foreach (NPC npc in Utility.getAllCharacters())
            {
                if (!npc.IsMonster)
                {
                    LogMessageToFile("Adding " + npc.name + " to the list");
                    listNPC.Add(npc);
                    LogMessageToFile("Added " + npc.name + " to the list");
                }
                LogMessageToFile("List Complete");
            } // End loop through getAllCharacters
              //reloadWeatherSprite(listNPC);

            LogMessageToFile("Exited ChangeNPC");
        } // End ChangeNPC()

        public static void reloadWeatherSprite(List<NPC> listNPC)
        {
            LogMessageToFile("Entered reloadWeatherSprite");
            for (int i = 0; i < listNPC.Count; ++i)
            {
                LogMessageToFile("Count in list: " + i.ToString());
                string name = listNPC[i].name;
                string str = name;

                if (Game1.IsWinter)
                {
                    string cFolder = @"C:\Program Files(x86)\Steam\steamapps\common\Stardew Valley\Content\Characters\" + name;
                    if (File.Exists(cFolder))
                    { // better to swap location of if statements
                        str = name + "_Winter";
                    }
                }

                LogMessageToFile("About to reassign sprite");
                listNPC[i].sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + str));

                try
                {
                    //currently shows that there is no portrait in NPC, but there is reference of it in the game code as a {get; set;}?
                    //currentNpc.portrait = Game1.content.Load<Texture2D>("Portraits\\" + str);
                    //this.Monitor.Log("Weather portrait should be Portraits\\" + str, LogLevel.Info);
                }
                catch (Exception ex)
                {
                    //currentNpc.portrait = (Texture2D)null;
                    //this.Monitor.Log("Weather portrait error occured", LogLevel.Error);
                }

                int num = listNPC[i].isInvisible ? 1 : 0;
                if (!Game1.newDay && (int)Game1.gameMode != 6)
                    return;
            }
            LogMessageToFile("Exited reloadWeatherSprite");
            return;
        } // End ReloadWeatherSprite()


    } // End ModEntry Class

} // End namespace

