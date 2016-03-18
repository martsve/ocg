using System;
using System.Collections.Generic;
using Delver.GameStates;
using Delver.Interface;
using Delver.StateMachine;

namespace Delver
{
    [Serializable]
    internal class Context : Revertable
    {
        [NonSerialized] private IGameCallback _callback;

        private StateMachineManager<GameState> _stateMachine;


        public Context(IReverter caller, int seed = -1) : base(caller)
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

        public void PostData(GameMessage message, Player player)
        {
            int p = player == null ? -1 : Players.IndexOf(player);
            _callback.SendDataPacket(p + ": " + message.ToJson());
        }

        /*
        public void PostData(string json)
        {
            _callback.SendDataPacket(json);
        }
        /**/

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