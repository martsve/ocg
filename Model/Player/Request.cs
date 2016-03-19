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
        public static InputRequest Populate(this InputRequest request, Context Context, Player player)
        {
            request.Mainphase = Context.CurrentStep?.type == StepType.PostMain ||
                                Context.CurrentStep?.type == StepType.PreMain;
            request.YourTurn = Context.ActivePlayer == player;
            request.EmptyStack = Context.CurrentStep == null || Context.CurrentStep.stack.Count == 0;
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
        private readonly Context Context;
        public InteractionHandler Handler;

        public List<string> InputHistory = new List<string>();
        private readonly Player player;

        public Request(Context Context, Player player, Func<InputRequest, string> func = null)
        {
            this.Context = Context;
            this.player = player;

            Handler = new InteractionHandler(func);
        }

        public Func<InputRequest, string> handlerOverride { get; set; }

        public string UserInput(InputRequest r)
        {
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

        public T RequestFromObjects<T>(MessageType type, string message, IEnumerable<T> Options)
        {
            var request = new InputRequest(type, message).Populate(Context, player);
            var i = GetUserSelection(type, Options.Select(x => x.ToString()), request);
            if (i < 0)
                return default(T);
            return Options.ToList()[i];
        }

        public List<T> RequestMultiple<T>(Card source, MessageType type, string message, IEnumerable<T> objects, bool orderAll = true)
        {
            var list = objects.ToList();

            var request = new InputRequest(type, message).Populate(Context, player);

            int c = 1;
            var selection = list.ToDictionary(x => c++, y => y.ToString());
            MessageBuilder.Select(type, selection).Text(message).To(player).Send(Context);

            var numbers = new List<int>();

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

        public Interaction RequestYesNo(MessageType type)
        {
            var request = new InputRequest(type, "").Populate(Context, player);

            var selection = new Dictionary<int, string>() { { 1, "Yes" }, { 2, "No" } };

            MessageBuilder.Select(type, selection).To(player).Send(Context);

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


        private int GetUserSelection(MessageType type, IEnumerable<string> objs, InputRequest request)
        {
            int c = 1;
            var selection = objs.ToDictionary(x => c++, y => y.ToString());

            MessageBuilder.Select(type, selection).Text(request.Text).To(player).Send(Context);
            
            var n = UserInput(request);
            var N = -1;

            if (!int.TryParse(n, out N))
               return -1;

            if (N > 0 && N <= objs.Count())
            {
                return N - 1;
            }

            return -1;
        }
    }
}