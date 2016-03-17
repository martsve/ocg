﻿using System;
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

        public void Send(int Player, string Command)
        {
            _context.Players[Player].request.Handler.Send(Command);
        }

        public void Start()
        {
            _context.PostData("Starting game..");

            _context.Start();
            while (_revert != null)
            {
                _context.PostData("Reverting..");
                _context = (Context) _revert;
                _revert = null;
                _context.SetCallbackFunction(_callbackInterface);
                _context.SetCaller(this);
                _context.Continue();
            }
        }
    }
}