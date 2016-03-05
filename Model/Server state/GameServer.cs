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
            game = new Game(this, 1);
            game.SetCallbackFunction(_callbackInterface);
        }

        public void Revert(Revertable state)
        {
            _revert = state;
            game.SetRunning(false);
        }

        public void AddPlayer(string name, List<string> list, Func<InputRequest, string> func = null)
        {
            var cards = new List<Card>();
            foreach (var card in list)
            {
                Card obj;
                try
                {
                    obj = (Card) Assembly.GetExecutingAssembly().CreateInstance($"Delver.Cards.TestCards.{card}");
                }
                catch
                {
                    throw new Exception($"No such card: {card}");
                }
                cards.Add(obj);
            }
            game.Methods.AddPlayer(name, cards, func);
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