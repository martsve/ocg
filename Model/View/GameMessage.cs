using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delver.Interface;
using Delver.View;
using Newtonsoft.Json;

namespace Delver
{

    [Serializable]
    static class MessageBuilder
    {
        public static void Send(this GameMessage msg, Context context)
        {
            //context.PostData(msg.ToJson());
            context.PostData(msg, msg.Player);
        }

        public static GameMessage To(this GameMessage msg, Player player)
        {
            msg.Player = player;
            return msg;
        }

        public static GameMessage Text(this GameMessage msg, string text)
        {
            msg.Text = text;
            return msg;
        }

        public static GameMessage View(GameView view)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.View,
                View = view,
            };
            return msg;
        }

        public static GameMessage Message(string text)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Message,
                Text = text,
            };
            return msg;
        }

        public static GameMessage New(MessageType type)
        {
            var msg = new GameMessage()
            {
                Type = type,
            };
            return msg;
        }

        public static GameMessage Error(string text = null)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Error,
                Text = text,
            };
            return msg;
        }

        public static GameMessage SetAttacking(Card card)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.SetAttacking,
                Card = card,
            };
            return msg;
        }

        public static GameMessage Priority(Player player)
        {
            dynamic data = new ExpandoObject();
            data.Player = player.ToString();

            var msg = new GameMessage()
            {
                Type = MessageType.Priority,
                Data = data,
            };
            return msg;
        }

        public static GameMessage Move(Card card, Zone from, Zone to)
        {
            dynamic data = new ExpandoObject();
            data.FromZone = from.ToString();
            data.ToZone = to.ToString();
            data.CardId = card.ToString();
            var msg = new GameMessage()
            {
                Type = MessageType.Move,
                Data = data,
            };
            return msg;
        }

        public static GameMessage Draw(Card card)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Draw,
                Card = card,
            };
            return msg;
        }

        public static GameMessage ChangeLife(string Text, Player player, int lifeChange)
        {
            dynamic data = new ExpandoObject();
            data.Change = lifeChange;
            data.PlayerView = new PlayerView()
            {
                Life = player.Life,
                Name = player.Name,
            };
            var msg = new GameMessage()
            {
                Type = MessageType.SetLife,
                Data = data,
            };
            return msg;
        }

        public static GameMessage SetBlocking(Card card, List<Card> cards)
        {
            dynamic data = new ExpandoObject();
            data.Blocked = cards;
            var msg = new GameMessage()
            {
                Type = MessageType.SetBlocking,
                Card = card,
                Data = data,
            };
            return msg;
        }

        public static GameMessage Select(MessageType type, object selection)
        {
            dynamic data = new ExpandoObject();
            data.Selection = selection;
            data.Type = type.ToString();
            var msg = new GameMessage()
            {
                Type = MessageType.TakeAction,
                Data = data,
            };
            return msg;
        }


        public static GameMessage AddPlayer(Player player)
        {
            dynamic data = new ExpandoObject();
            data.Name = player.Name;
            data.Decksize = player.Library.Count;
            var msg = new GameMessage()
            {
                Type = MessageType.AddPlayer,
                Data = data,
            };
            return msg;
        }

        public static GameMessage CurrentStep(GameStep step)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.BeginStep,
                Step = step.type,
            };
            return msg;
        }
        public static GameMessage BeginTurn(Context context)
        {
            dynamic data = new ExpandoObject();
            data.ActivePlayer = context.CurrentTurn.Player.ToString();
            data.TurnNumber = context.TurnNumber;
            data.TurnOrder = context.TurnOrder.Select(x => x.ToString()); 
            var msg = new GameMessage()
            {
                Type = MessageType.BeginTurn,
                Data = data,
            };
            return msg;
        }

        public static GameMessage TurnOrder(List<Player> players)
        {
            dynamic data = new ExpandoObject();
            data.TurnOrder = players.Select(x => x.ToString());
            var msg = new GameMessage()
            {
                Type = MessageType.TurnOrder,
                Data = data,
            };
            return msg;
        }

        public static GameMessage Stack(List<IStackCard> cards)
        {
            dynamic data = new ExpandoObject();
            data.Stack = cards.Select(x => x.ToString());
            var msg = new GameMessage()
            {
                Type = MessageType.Stack,
                Data = data,
            };
            return msg;
        }
    }

    [Serializable]
    class GameMessage
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public Player Player { get; set; }
        public StepType Step { get; set; }
        public Card Card { get; set; }
        public GameView View { get; set; }
        public ExpandoObject Data { get; set; } = new ExpandoObject();

        public string ToJson()
        {
            var obj = new Dictionary<string, object>();
            dynamic data = Data;

            if (Type == MessageType.BeginStep) data.Step = Step.ToString();
            if (Text != null )data.Text = Text;
            if (Card != null) data.Card = Card.ToView();

            if (View != null) data = View;

            obj[Type.ToString()] = data;

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
    }

}
