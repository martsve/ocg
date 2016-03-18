using System;
using System.Collections.Generic;
using System.Reflection;
using Delver.Interface;

namespace Delver
{
    public class GameServer : IReverter
    {
        private readonly IGameCallback _callbackInterface;
        private Context _context;
        private Revertable _revert;

        public GameServer(IGameCallback callbackInterface)
        {
            _callbackInterface = callbackInterface;
            _context = new Context(this);
            _context.SetCallbackFunction(_callbackInterface);
        }

        public void Revert(Revertable state)
        {
            _revert = state;
            _context.SetRunning(false);
        }

        public void AddPlayer(string name, Decklist decklist, Func<InputRequest, string> func = null)
        {
            _context.Methods.AddPlayer(name, DeckBuilder.Build(decklist), func);
        }

        public void Send(int player, string command)
        {
            _context.Players[player].request.Handler.Send(command);
        }

        public void Start()
        {

            MessageBuilder.Message("Starting game..").Send(_context);

            _context.Start();
            while (_revert != null)
            {
                MessageBuilder.Message("Reverting..").Send(_context);
                _context = (Context) _revert;
                _revert = null;
                _context.SetCallbackFunction(_callbackInterface);
                _context.SetCaller(this);
                _context.Continue();
            }
        }
    }
}