using System;
using System.Collections.Generic;
using System.Reflection;
using Delver.Interface;

namespace Delver
{
    public class GameServer : IReverter
    {
        private readonly IGameCallback _callbackInterface;
        private Game game;
        private Revertable _revert;

        public GameServer(IGameCallback callbackInterface)
        {
            _callbackInterface = callbackInterface;
            game = new Game(this);
            game.SetCallbackFunction(_callbackInterface);
        }

        public void Revert(Revertable state)
        {
            _revert = state;
            game.SetRunning(false);
        }

        public void AddPlayer(string name, Decklist decklist, Func<InputRequest, string> func = null)
        {
            game.Methods.AddPlayer(name, DeckBuilder.Build(decklist), func);
        }

        public void Send(int Player, string Command)
        {
            game.Players[Player].request.Handler.Send(Command);
        }

        public void Start()
        {
            game.PostData("Starting game..");

            game.Start();
            while (_revert != null)
            {
                game.PostData("Reverting..");
                game = (Game) _revert;
                _revert = null;
                game.SetCallbackFunction(_callbackInterface);
                game.SetCaller(this);
                game.Continue();
            }
        }
    }
}