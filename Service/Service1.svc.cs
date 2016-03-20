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

            var deck = new Decklist();
            deck.Add("Forest", 20);
            deck.Add("Bear", 20);
            deck.Add("LightningStrike", 20);

            gameServer.AddPlayer("P0", deck);
            gameServer.AddPlayer("P1", deck, Ai);

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