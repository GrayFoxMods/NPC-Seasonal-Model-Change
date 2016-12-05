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
        static public NPC[] listNPC = new NPC[41]; // cunrrent npcs in my game. MUST BE CHANGED BEFORE FINALIZATION

        private static readonly string LOG_FILENAME = @"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Mods\WeatherClothing\" + "WeatherClothing Log.txt";

        public static void LogMessageToFile(string msg)

        {

            msg = string.Format("{0:G}: {1}{2}", DateTime.Now, msg, Environment.NewLine);

            File.AppendAllText(LOG_FILENAME, msg);

        }

        public override void Entry(IModHelper helper)
        {
            //LogMessageToFile("Entered WeatherClothing Entry");
            StardewModdingAPI.Events.TimeEvents.TimeOfDayChanged += UpdateTimeEvent;
            //LogMessageToFile("Exited WeatherClothing Entry");
        }

        static void UpdateTimeEvent(object sender, EventArgs e)
        {
            //LogMessageToFile("Entered Update Time");
            if (StardewValley.Game1.currentLocation == null)
                return;
            if (StardewValley.Game1.timeOfDay == 610)
                ChangeNPC();
            //LogMessageToFile("Exited Update Time");
        } // End Update Season Event

        public static void ChangeNPC()
        {
            int count = 0;
            //LogMessageToFile("Entered ChangeNPC");
            foreach (NPC npc in Utility.getAllCharacters())
            {
                if (!npc.IsMonster)
                {
                    //LogMessageToFile("Adding " + npc.name + " to the list");
                    listNPC[count++] = npc;
                    //LogMessageToFile("Added " + npc.name + " to the list");
                }
            } // End loop through getAllCharacters
            //LogMessageToFile("List Complete");

            reloadWeatherSprite(listNPC);

            LogMessageToFile("Exited ChangeNPC");
        } // End ChangeNPC()

        public static void reloadWeatherSprite(NPC[] listNPC)
        {
            LogMessageToFile("Entered reloadWeatherSprite");
            for (int i = 0; i < listNPC.Length - 1; ++i)
            {
                string name = listNPC[i].name;
                string str = name == "Old Mariner" ? "Mariner" : (name == "Dwarf King" ? "DwarfKing" : (name == "Mister Qi" ? "MrQi" : (name == "???" ? "Monsters\\Shadow Guy" : listNPC[i].name)));
                if (listNPC[i].name.Equals(Utility.getOtherFarmerNames()[0]))
                    str = Game1.player.isMale ? "maleRival" : "femaleRival";
                LogMessageToFile("Count in list: " + i.ToString() + "   --character: " + name);

                if (Game1.IsWinter)
                {
                    str = name + "_Winter";
                }

                LogMessageToFile("About to reassign sprite for character " + name);
                string cFolder = @"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\Content\Characters\\" + str + ".xnb";
                if (File.Exists(cFolder))
                {
                    LogMessageToFile("Winter file exists, switching npc sprite to : " + str);
                    Console.WriteLine("Winter file exists, switching npc sprite to : " + str);
                    listNPC[i].sprite = new AnimatedSprite(Game1.content.Load<Texture2D>("Characters\\" + str));
                    if (!listNPC[i].name.Contains("Dwarf"))
                        listNPC[i].sprite.spriteHeight = 32;
                }

                try
                {
                    listNPC[i].Portrait = Game1.content.Load<Texture2D>("Portraits\\" + str);
                    LogMessageToFile("Weather portrait should be Portraits\\" + str);
                }
                catch (Exception ex)
                {
                    listNPC[i].Portrait = (Texture2D)null;
                    LogMessageToFile("Weather portrait error occured");
                }

                int num = listNPC[i].isInvisible ? 1 : 0;
            }
            LogMessageToFile("Exited reloadWeatherSprite");
            return;
        } // End ReloadWeatherSprite()


    } // End ModEntry Class

} // End namespace

