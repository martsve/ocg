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
            context.PostData(msg, msg.ToPlayer);
        }

        public static GameMessage To(this GameMessage msg, Player player)
        {
            msg.ToPlayer = player;
            return msg;
        }

        public static GameMessage Text(this GameMessage msg, string text)
        {
            msg.Text = text;
            return msg;
        }


        public static void SendView(Context context)
        {
            Flush(GameviewPopulator.GetView(context)).Send(context);

            foreach (var player in context.Players) {
                UpdateView(GameviewPopulator.GetHand(player)).To(player).Send(context);
            }
        }

        public static GameMessage Flush(GameView view)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Flush,
                View = view,
            };
            return msg;
        }

        public static GameMessage UpdateView(GameView view)
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
            var msg = new GameMessage()
            {
                Type = MessageType.Priority,
                Player = player,
            };
            return msg;
        }

        public static GameMessage Move(Card card, Zone from, Zone to, bool Public = true)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Move,
                Remove = card.Id,
                View = GameviewPopulator.MakeView(card, Public),
            };
            return msg;
        }

        public static GameMessage ChangeLife(string Text, Player player, int lifeChange)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.SetLife,
                View = GameviewPopulator.MakeView(new PlayerView(player.Id) { Life = player.Life }),
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
            var msg = new GameMessage()
            {
                Type = MessageType.AddPlayer,
                View = GameviewPopulator.New.AddPlayer(player),
            };
            return msg;
        }

        public static GameMessage CurrentStep(Context context)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.BeginStep,
                View = GameviewPopulator.New.AddCurrentStep(context),
            };
            return msg;
        }

        public static GameMessage BeginTurn(Context context)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.BeginTurn,
                View = GameviewPopulator.New.AddActivePlayer(context),
            };
            return msg;
        }

        public static GameMessage TurnOrder(Context context)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.TurnOrder,
                View = GameviewPopulator.New.AddTurn(context).AddCurrentStep(context),
            };
            return msg;
        }

        public static GameMessage Stack(Context context)
        {
            var msg = new GameMessage()
            {
                Type = MessageType.Stack,
                View = GameviewPopulator.New.AddStack(context),
            };
            return msg;
        }
    }

    [Serializable]
    class GameMessage
    {
        public Player ToPlayer;

        public MessageType Type { get; set; }
        public string Text { get; set; }
        public Card Card { get; set; }
        public Player Player { get; set; }
        public GameView View { get; set; }
        public int? Remove { get; set; } = -1;
        public ExpandoObject Data { get; set; } = new ExpandoObject();

        public string ToJson()
        {
            var obj = new Dictionary<string, object>();
            dynamic data = Data;

            if (Text != null) data.Text = Text;
            if (Remove != -1) data.Remove = Remove;
            if (Card != null) data.Card = new CardView(Card.Id);
            if (Player != null) data.Player = new PlayerView(Player.Id);

            if (View != null) data.View = View;

            obj[Type.ToString()] = data;

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(obj, Formatting.None, settings);
        }
    }

}
