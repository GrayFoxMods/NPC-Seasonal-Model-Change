// GrayFoxMods
// December 2nd, 2016

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.IO;


namespace WeatherClothing
{
    public class WeatherClothing : Mod
    {
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
            //this.Monitor.Log("Entered Game Load", LogLevel.Info);
            if (StardewValley.Game1.currentLocation == null)
                return;

            if (StardewValley.Game1.timeOfDay == 610)
                 ChangeNPC();
            //this.Monitor.Log("Exited Game Load", LogLevel.Info);
        }

        public static void ChangeNPC()
        {
            Console.WriteLine("Entered ChangeNPC");
            foreach (NPC npc in Utility.getAllCharacters())
            {
                GameLocation locationFromName = Game1.getLocationFromName(npc.name);
                {
                    if (!npc.DefaultPosition.Equals(Vector2.Zero))
                        npc.position = npc.DefaultPosition;
                    npc.currentLocation = locationFromName;
                    reloadWeatherSprite(npc);

                } // end location check

            } // End loop through getAllCharacters

            Console.WriteLine("Exited ChangeNPC");
        } // End ChangeNPC()

        public static void reloadWeatherSprite(NPC currentNpc)
        {
            Console.WriteLine("Entered reloadWeatherSprite");
            string name = currentNpc.name;
            string str = name;

            if (Game1.IsWinter)
            {
                string cFolder = @"C:\Program Files(x86)\Steam\steamapps\common\Stardew Valley\Content\Characters";
                if (File.Exists(cFolder)){
                    str = name + "_Winter";
                }
            }

            currentNpc.sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + str));

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
            int num = currentNpc.isInvisible ? 1 : 0;
            if (!Game1.newDay && (int)Game1.gameMode != 6)
                return;

            Console.WriteLine("Exited reloadWeatherSprite");
            return;
        } // End ReloadWeatherSprite()

    } // End ModEntry Class

} // End namespace

