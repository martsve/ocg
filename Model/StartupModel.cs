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
            deck.Add("Plains", 12);
            //deck.Add("GeistOfSaintTraft", 1);
            //deck.Add("EerieInterlude", 4);
            deck.Add("BondsofFaith", 4);
            //deck.Add("CathedralSanctifier", 4);
            //deck.Add("ChampionoftheParish", 4);
            //deck.Add("DearlyDeparted", 4);
            deck.Add("DoomedTraveler", 4);
            //deck.Add("ElderCathar", 4);
            //deck.Add("EmancipationAngel", 4);
            //deck.Add("FiendHunter", 4);
            deck.Add("Shock", 4);
            //deck.Add("ChapelGeist", 4);

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
                case MessageType.StartPlayer:
                    return "1";
                case MessageType.Mulligan:
                    return "0";
            }

            if (request.Type != MessageType.OrderTriggers)
            {
                if (!request.YourTurn || !request.Mainphase)
                    return "1";
            }

            var str = Console.ReadLine();
            return str;
        }

        private static string Ai(InputRequest request)
        {
            switch (request.Type)
            {
                case MessageType.StartPlayer:
                    return "1";
                case MessageType.Mulligan:
                    return "0";
                case MessageType.DiscardACard:
                    return "1";
            }
            return "";
        }
    }
}