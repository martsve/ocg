using System.Collections.Generic;
using System.Linq;


namespace Delver.View
{
    internal static class GameviewPopulator
    {
        public static GameView GetView(Context context, bool Public = false)
        {
            var view = New;
            view.AddCurrentStep(context);
            view.AddTurn(context);
            view.AddStack(context);
            view.AddCombat(context);
            foreach (var player in context.Players)
                view.AddPlayer(player, Public);
            return view;
        }

        public static GameView MakeView(PlayerView playerView)
        {
            var view = New;
            view.Players = new List<PlayerView>();
            view.Players.Add(playerView);
            return view;
        }

        public static GameView MakeView(Card card)
        {
            var view = New;
            view.Players = new List<PlayerView>();
            var pview = new PlayerView(card.Controller.Id);

            if (card.Zone == Zone.Battlefield)
                pview.Battlefield = new List<CardView>() { card.ToView() };
            else if (card.Zone == Zone.Command)
                pview.Command = new List<CardView>() { card.ToView() };
            else if (card.Zone == Zone.Exile)
                pview.Exile = new List<CardView>() { card.ToView() };
            else if (card.Zone == Zone.Graveyard)
                pview.Graveyard = new List<CardView>() { card.ToView() };
            else if (card.Zone == Zone.Hand)
                pview.Hand = new List<CardView>() { card.ToView() };

            view.Players.Add(pview);
            return view;
        }

        public static GameView New => new GameView();

        public static GameView AddCurrentStep(this GameView view, Context context)
        {
            if (context.CurrentStep != null)
                view.CurrentStep = context.CurrentStep.type.ToString();
            return view;
        }

        public static GameView AddPlayer(this GameView view, Player player, bool Public = false)
        {
            if (view.Players == null) view.Players = new List<PlayerView>();
            view.Players.Add(player.ToView(Public));
            return view;
        }
    
        public static GameView AddActivePlayer(this GameView view, Context context)
        {
            view.ActivePlayer = context.CurrentTurn.Player.Id;
            view.TurnNumber = context.TurnNumber;
            return view;
        }

        public static GameView AddTurn(this GameView view, Context context)
        {
            view.TurnOrder = new List<int>();
            view.TurnOrder.AddRange(context.TurnOrder.Select(x => x.Id));

            if (context.CurrentTurn != null)
            {
                view.Steps = new List<string>();
                view.Steps.AddRange(context.CurrentTurn.GetTurnSteps().Select(x => x.type.ToString()));
            }
            return view;
        }

        public static GameView AddStack(this GameView view, Context context)
        {
            view.Stack = context.CurrentStep.stack.Count > 0
                ? CardViewPopulator(context.CurrentStep.stack.Cast<Card>(), true)
                : null;
            return view;
        }

        public static GameView AddCombat(this GameView view, Context context)
        {
            if (context.CurrentStep.IsCombatStep)
            {
                foreach (var attacker in context.Logic.attackers)
                {
                    view.Combat.Add(new CombatView
                    {
                        Attacker = attacker.ToString(),
                        Blockers = context.Logic.blockers.Where(x => x.IsBlocking.Contains(x)).Select(x => x.ToString()).ToList()
                    });
                }
            }
            else
            {
                view.Combat = null;
            }
            return view;
        }

        public static PlayerView ToView(this Player player, bool Public = false)
        {
            var w = new PlayerView(player.Id)
            {
                Name = player.Name,
                Life = player.Life,
                Manapool = player.ManaPool.Count > 0 ? player.ManaPool.GetView() : null,
                HandCount = player.Hand.Count,
                LibraryCount = player.Library.Count,
                Battlefield = CardViewPopulator(player.Battlefield),
                Exile = CardViewPopulator(player.Exile),
                Command = CardViewPopulator(player.Command),
                Graveyard = CardViewPopulator(player.Graveyard),
                Hand = Public ? CardViewPopulator(player.Hand) : null,
            };
            return w;
        }

        public static List<CardView> CardViewPopulator(IEnumerable<Card> cards, bool showController = false)
        {
            if (!cards.Any())
                return null;

            return cards.Select(x => x.ToView(showController)).ToList();
        }

        public static CardView ToView(this Card card, bool showController = false)
        {
            var w = new CardView(card.Id);
            w.Name = card.Name;
            if (card.IsTapped)
                w.IsTapped = card.IsTapped;

            var super = string.Join(" ", card.Current.Supertype).Trim();
            var typ = string.Join(" ", GetCardType(card)).Trim();
            var sub = string.Join(" ", card.Current.Subtype).Trim();

            w.Type = $"{super} {typ} - {sub}".Trim(' ', '-');

            if (card.isCardType(CardType.Creature))
            {
                w.Power = card.Current.Power;
                w.Thoughness = card.Current.Thoughness;
            }

            if (showController)
                w.Player = card.Controller.Name;

            if (card.Counters.Any())
                w.Counters = card.Counters.GroupBy(x => x.ToString()).ToDictionary(x => x.Key, y => y.Count());

            if (card.Current.CardAbilities.Any())
                w.Abilities = card.Current.CardAbilities.Select(x => x.ToString()).ToList();

            if (card.Current.Keywords.Any())
                w.Abilities = card.Current.Keywords.Select(x => x.ToString()).ToList();

            var text = new List<string>();

            if (card.Current.Text != null)
                text.Add(card.Current.Text);

            //if (card.Current.FollowingLayers.Any())
            //    text.AddRange(card.Current.FollowingLayers.Select(x => x.ToString()));

            if (card.Current.Events.Any())
                text.AddRange(card.Current.Events.Select(x => x.ToString()));
            
            if (text.Any())
                w.Text = string.Join("\n", text);

            return w;
        }

        public static List<ManaView> GetView(this ManaCost manapool)
        {
            var list = new List<ManaView>();
            foreach (var m in manapool)
            {
                var w = new ManaView();
                w.Color = m.ToString();
                w.Special = m.Special;
                list.Add(w);
            }
            return list;
        }

        public static List<string> GetCardSuperType(Card c)
        {
            var list = new List<string>();
            if (c.isCardType(CardType.Legendary)) list.Add("Legendary");
            if (c.isCardType(CardType.Basic)) list.Add("Basic");
            return list;
        }

        public static List<string> GetCardType(Card c)
        {
            var list = new List<string>();
            if (c.isCardType(CardType.Artifact)) list.Add("Artifact");
            if (c.isCardType(CardType.Creature)) list.Add("Creature");
            if (c.isCardType(CardType.Enchantment)) list.Add("Enchantment");
            if (c.isCardType(CardType.Instant)) list.Add("Instant");
            if (c.isCardType(CardType.Land)) list.Add("Land");
            if (c.isCardType(CardType.Planeswalker)) list.Add("Planeswalker");
            if (c.isCardType(CardType.Sorcery)) list.Add("Sorcery");
            return list;
        }
    }


    public class GameView
    {
        public List<CombatView> Combat { get; set; }
        public List<PlayerView> Players { get; set; }
        public List<CardView> Stack { get; set; }
        public List<string> Steps { get; set; }
        public string CurrentStep { get; set; }
        public List<int> TurnOrder { get; set; }
        public int? ActivePlayer { get; set; }
        public int? TurnNumber { get; set; }
    }

    public class CombatView
    {
        public List<string> Blockers = new List<string>();
        public string Attacker { get; set; }
    }

    public class PlayerView
    {
        public PlayerView(int Id)
        {
            this.Id = Id;
        }
        public List<CardView> Battlefield { get; set; }
        public List<CardView> Command { get; set; }
        public List<CardView> Exile { get; set; }
        public List<CardView> Graveyard { get; set; }
        public List<CardView> Hand { get; set; }
        public List<ManaView> Manapool { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Life { get; set; }
        public int? Poison { get; set; }
        public int? HandCount { get; set; }
        public int? LibraryCount { get; set; }
    }

    public class ManaView
    {
        public string Color { get; set; }
        public string Special { get; set; }
    }

    public class CardView
    {
        public CardView(int Id)
        {
            this.Id = Id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsTapped { get; set; }
        public int? Damage { get; set; }
        public int? Power { get; set; }
        public int? Thoughness { get; set; }
        public string Type { get; set; }
        public string Manacost { get; set; }
        public string Text { get; set; }
        public string Owner { get; set; }

        public string Player { get; set; }

        public List<string> Abilities { get; set; }
        public List<string> Keywords { get; set; }

        public Dictionary<string, int> Counters { get; set; }
    }

}