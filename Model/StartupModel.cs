using System;
using System.Collections.Generic;
using Delver.Interface;

namespace Delver
{
    public class StartupModel
    {
        private static void Main(string[] args)
        {
            var gameServer = new GameServer(new ConsoleCallback());

            var deck = new List<string>();
            for (var i = 0; i < 20; i++)
            {
                deck.Add("Island");
                deck.Add("Plains");
                deck.Add("GeistOfSaintTraft");
            }

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