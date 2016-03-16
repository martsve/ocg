using System;
using System.Collections.Generic;
using System.Dynamic;
using Delver.Interface;

namespace Delver
{
    public class StartupModel
    {
        private static void Main(string[] args)
        {
            var gameServer = new GameServer(new ConsoleCallback());

            var deck = new Decklist();
            //deck.Add("Island", 12);
            deck.Add("Plains", 24);
            //deck.Add("GeistOfSaintTraft", 1);
            deck.Add("EerieInterlude", 4);
            //deck.Add("BondsofFaith", 1);
            //deck.Add("CathedralSanctifier", 1);
            deck.Add("ChampionoftheParish", 4);
            //deck.Add("ChapelGeist", 1);
            //deck.Add("DearlyDeparted", 1);
            //deck.Add("DoomedTraveler", 1);
            //deck.Add("ElderCathar", 1);
            deck.Add("EmancipationAngel", 4);
            deck.Add("FiendHunter", 4);
            //deck.Add("GathertheTownsfolk", 1);

            gameServer.AddPlayer("P0", deck, Me);
            gameServer.AddPlayer("P1", deck, Ai);

            gameServer.Start();

            Console.WriteLine("Exiting!");
            Console.ReadLine();
        }

        private static string Me(InputRequest request)
        {
            switch (request.Type)
            {
                case RequestType.StartPlayer:
                    return "1";
                case RequestType.Mulligan:
                    return "0";
            }

            if (!request.YourTurn || !request.Mainphase)
                return "1";

            var str = Console.ReadLine();
            return str;
        }

        private static string Ai(InputRequest request)
        {
            switch (request.Type)
            {
                case RequestType.StartPlayer:
                    return "1";
                case RequestType.Mulligan:
                    return "0";
                case RequestType.DiscardACard:
                    return "1";
            }
            return "";
        }
    }
}