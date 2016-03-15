using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delver.Interface;

namespace Delver
{
    [Serializable]
    internal static class InputRequestBuilder
    {
        // public GameView gameView { get { return GameviewPopulator.GetView(game, player); } }
        public static InputRequest Populate(this InputRequest request, Game game, Player player)
        {
            request.Mainphase = game.CurrentStep?.type == StepType.PostMain ||
                                game.CurrentStep?.type == StepType.PreMain;
            request.YourTurn = game.ActivePlayer == player;
            request.EmptyStack = game.CurrentStep == null || game.CurrentStep.stack.Count == 0;
            return request;
        }
    }


    [Serializable]
    internal class Interaction
    {
        public Interaction(Player player, InteractionType type)
        {
            this.Type = type;
            this.Player = player;
        }

        public InteractionType Type { get; set; }
        public Player Player { get; set; }
        public Card Card { get; set; }
    }

    [Serializable]
    internal class InteractionHandler
    {
        private readonly Func<InputRequest, string> _callback;

        private readonly List<string> _inputQueue = new List<string>();

        [NonSerialized]
        private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public InteractionHandler(Func<InputRequest, string> callback = null)
        {
            _callback = callback;
        }

        public async Task<string> AwaitInteraction(InputRequest request)
        {
            if (_callback != null)
            {
                _inputQueue.Add(_callback.Invoke(request));
            }

            while (_inputQueue.Count == 0)
            {
                _tcs = new TaskCompletionSource<bool>();
                await _tcs.Task;
            }

            var cmd = _inputQueue.Pop(0);
            return cmd;
        }

        public void Send(string command)
        {
            _inputQueue.Add(command);
            _tcs.TrySetResult(true);
        }
    }


    [Serializable]
    internal class Request
    {
        private readonly Game game;
        public InteractionHandler Handler;

        public List<string> InputHistory = new List<string>();
        private readonly Player player;

        public Request(Game game, Player player, Func<InputRequest, string> func = null)
        {
            this.game = game;
            this.player = player;

            Handler = new InteractionHandler(func);
        }

        public Func<InputRequest, string> handlerOverride { get; set; }

        public string UserInput(InputRequest r)
        {
            game.PostData(r.Text);

            if (handlerOverride != null)
            {
                var so = handlerOverride.Invoke(r);
                if (so != null)
                {
                    InputHistory.Add(so);
                    return so;
                }
                handlerOverride = null;
            }

            var s = Handler.AwaitInteraction(r).Result;
            InputHistory.Add(s);
            return s;
        }


        public T RequestFromObjects<T>(RequestType type, string message, IEnumerable<T> Options)
        {
            var request = new InputRequest(type, message).Populate(game, player);
            var i = GetUserSelection(Options.Select(x => x.ToString()), request);
            if (i < 0)
                return default(T);
            return Options.ToList()[i];
        }


        public List<T> RequestMultiple<T>(Card source, RequestType type, string message, IEnumerable<T> objects,
            bool orderAll = true)
        {
            var list = objects.ToList();
            var request = new InputRequest(type, message).Populate(game, player);
            SendSelection(list, request.Type);

            List<int> numbers = new List<int>();

            var input = UserInput(request);
            if (input.Length > 0)
            {
                var words = input.Replace(',', ' ').Split(' ');
                foreach (var x in words)
                {
                    int num;
                    if (!int.TryParse(x, out num))
                        return RequestMultiple(source, type, message, list, orderAll);
                    numbers.Add(num - 1);
                }
                numbers = numbers.Distinct().ToList();
            }

            var result = new List<T>();
            foreach (var i in numbers)
            {
                if (i < 0 || i >= list.Count())
                    return RequestMultiple(source, type, message, list, orderAll);
                result.Add(list[i]);
            }

            if (orderAll && result.Count < list.Count())
                return RequestMultiple(source, type, message, list, orderAll);

            return result;
        }


        public Interaction RequestYesNo(RequestType type, string message)
        {
            var request = new InputRequest(type, message).Populate(game, player);

            var key = UserInput(request);
            switch (key)
            {
                case "1":
                    return new Interaction(player, InteractionType.Accept);
                case "2":
                    return new Interaction(player, InteractionType.Pass);
                default:
                    return new Interaction(player, InteractionType.Pass);
            }
        }


        private int GetUserSelection(IEnumerable<string> objs, InputRequest request)
        {
            var objList = objs.ToList();
            SendSelection(objList, request.Type);

            var n = UserInput(request);
            var N = -1;

            if (!int.TryParse(n, out N))
               return -1;

            if (N > 0 && N <= objList.Count())
            {
                return N - 1;
            }

            return -1;
        }

        private void SendSelection<T>(IEnumerable<T> objs, RequestType type)
        {
            if (type == RequestType.TakeAction)
            {
                var obj = objs.ToList();
                for (var i = 0; i < obj.Count; i++)
                    game.PostData($"{i + 1}. {obj[i]}");
                game.PostData("");
            }
            else
            {
                var obj = objs.ToList();
                for (var i = 0; i < obj.Count; i++)
                    game.PostData($"{i + 1}. {obj[i]}");
            }
        }
    }
}