using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Delver.Interface;

namespace Delver.Service
{
    // http://realfiction.net/2008/01/30/The-no-frills-bare-bones-example-to-Duplex-WCF/
    public class ServerWCallbackImpl : IServerWithCallback
    {
        public void StartNewServer()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IGameCallback>();

            var gameServer = new GameServer(callback);

            string blesserStr = @"
1 Cathedral Sanctifier
1 Champion of the Parish
1 Doomed Traveler
1 Nephalia Smuggler
1 Moorland Inquisitor
1 Thraben Heretic
2 Elder Cathar
1 Village Bell-Ringer
1 Captain of the Mists
1 Tandem Lookout
1 Chapel Geist
1 Emancipation Angel
1 Fiend Hunter
1 Geist of Saint Traft
1 Slayer of the Wicked
1 Tower Geist
1 Mist Raven
1 Spectral Gateguards
1 Gryff Vanguard
1 Dearly Departed
1 Goldnight Redeemer
1 Voice of the Provinces
2 Topplegeist
2 Gather the Townsfolk
1 Increasing Devotion
1 Pore Over the Pages
1 Momentary Blink
2 Rebuke
1 Eerie Interlude
1 Sharpened Pitchfork
1 Butcher's Cleaver
1 Bonds of Faith
1 Seraph Sanctuary
4 Tranquil Cove
12 Plains
7 Island";

            var blessed = new Decklist(blesserStr);

            string cursedStr = @"
1 Diregraf Ghoul
1 Gravecrawler
2 Butcher Ghoul
3 Screeching Skaab
1 Unbreathing Horde
1 Scrapskin Drake
2 Ghoulraiser
2 Stitched Drake
2 Diregraf Captain
1 Abattoir Ghoul
1 Driver of the Dead
1 Falkenrath Noble
1 Makeshift Mauler
1 Havengul Runebinder
1 Relentless Skaabs
1 Harvester of Souls
1 Tooth Collector
1 Mindwrack Demon
1 Appetite for Brains
1 Sever the Bloodline
1 Barter in Blood
1 Dread Return
2 Moan of the Unhallowed
1 Human Frailty
1 Victim of Night
1 Tribute to Hunger
1 Forbidden Alchemy 
1 Compelling Deterrence
1 Cobbled Wings
4 Dismal Backwater
12 Swamp
8 Island";

            var cursed = new Decklist(cursedStr);

            gameServer.AddPlayer("P0", blessed);
            gameServer.AddPlayer("P1", cursed, Ai);

            var id = GameList.Instance.Add(gameServer);

            var task = Task.Run(() =>
            {
                gameServer.Start();
                Console.WriteLine($"Log: Game #{id} ended");
            });

            Console.WriteLine($"Log: Game #{id} started");
        }

        public void Ping() { }

        public void SendCommand(int game, int player, string command)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IGameCallback>();
            //callback.SendDataPacket($"Command received");

            GameList.Instance.SendCommand(game, player, command);
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
                case MessageType.Interact:
                    return "-1";
            }
            return "1";
        }

        private class GameList
        {
            private int _counter;
            private readonly Dictionary<int, GameServer> _games = new Dictionary<int, GameServer>();

            private GameList()
            {
            }

            public static GameList Instance { get; } = new GameList();

            public int Add(GameServer game)
            {
                _counter++;
                _games.Add(_counter, game);
                return _counter;
            }

            public void SendCommand(int Game, int Player, string Command)
            {
                _games[Game].Send(Player, Command);
            }
        }
    }
}