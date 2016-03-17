using System;
using System.Collections.Generic;
using Delver.GameStates;
using Delver.Interface;
using Delver.StateMachine;

namespace Delver
{
    [Serializable]
    internal class Game : Revertable
    {
        [NonSerialized] private IGameCallback _callback;

        private StateMachineManager<GameState> _stateMachine;


        public Game(IReverter caller, int seed = -1) : base(caller)
        {
            Logic = new GameLogic(this);
            Methods = new GameMethods(this);
            Rand = new Rand(seed);
            TurnNumber = 0;
        }

        public GameLogic Logic { get; set; }
        public GameMethods Methods { get; set; }
        public Rand Rand { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public Player ActivePlayer { get; set; }

        public GameStep CurrentStep { get; set; }
        public Turn CurrentTurn { get; set; }

        public Stack<Turn> Turns { get; set; } = new Stack<Turn>();

        public List<LayeredEffect> LayeredEffects { get; set; } = new List<LayeredEffect>();

        public List<Player> TurnOrder { get; set; }

        public int TurnNumber { get; set; }

        private int Timestamp { get; set; }
        public int GetTimestamp()
        {
            return Timestamp++;
        }

        private bool Running { get; set; }

        public void SetCallbackFunction(IGameCallback callback)
        {
            this._callback = callback;
        }

        public void PostData(string json, Player player = null)
        {
            var p = player != null ? Players.IndexOf(player) : -1;
            _callback.SendDataPacket($"{p}: {json}");
        }

        public void SetRunning(bool running)
        {
            this.Running = running;
        }

        public bool IsRunning()
        {
            return Running;
        }

        public void Continue()
        {
            _stateMachine.Run(IsRunning);
        }

        public void Start()
        {
            _stateMachine = new StateMachineManager<GameState>(this, GameState.StateMachineCallback, new InitState());
            SetRunning(true);

            //SaveState("gamestart.obj");

            _stateMachine.Run(IsRunning);
        }
    }
}