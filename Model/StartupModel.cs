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
            deck.Add("Island", 12);
            deck.Add("Plains", 12);
            deck.Add("GeistOfSaintTraft", 4);
            deck.Add("EerieInterlude", 2);

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