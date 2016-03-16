using System.Collections.Generic;
using System.Linq;


namespace Delver.View
{
    internal static class GameviewPopulator
    {
        public static GameView GetView(Game game, Player focused)
        {
            var view = new GameView();
            foreach (var p in game.Players)
            {
                view.Players.Add(new PlayerView
                {
                    Name = p.Name,
                    Life = p.Life,
                    Manapool = p.ManaPool.Count > 0 ? ManaViewPopulator(p.ManaPool) : null,
                    HandCount = p.Hand.Count,
                    LibraryCount = p.Library.Count,
                    Battlefield = p.Battlefield.Count > 0 ? CardViewPopulator(p.Battlefield) : null,
                    Exile = p.Exile.Count > 0 ? CardViewPopulator(p.Exile) : null,
                    Command = p.Command.Count > 0 ? CardViewPopulator(p.Command) : null,
                    Graveyard = p.Graveyard.Count > 0 ? CardViewPopulator(p.Graveyard) : null,
                    Hand = p == focused && p.Hand.Count > 0 ? CardViewPopulator(p.Hand) : null
                });
            }

            view.Stack = game.CurrentStep.stack.Count > 0
                ? CardViewPopulator(game.CurrentStep.stack.Cast<Card>(), true)
                : null;

            view.Steps.Add(game.CurrentStep.type.ToString());
            view.Steps.AddRange(game.CurrentTurn.steps.Select(x => x.type.ToString()));

            view.Turns.AddRange(game.Logic.GetTurnOrder().Select(x => x.Name.ToString()));

            if (game.CurrentStep.IsCombatStep)
            {
                foreach (var attacker in game.Logic.attackers)
                {
                    view.Combat.Add(new CombatView
                    {
                        Attacker = attacker.ToString(),
                        Blockers =
                            game.Logic.blockers.Where(x => x.IsBlocking.Contains(x)).Select(x => x.ToString()).ToList()
                    });
                }
            }
            else
            {
                view.Combat = null;
            }

            return view;
        }

        public static List<CardView> CardViewPopulator(IEnumerable<Card> cards, bool showController = false)
        {
            var list = new List<CardView>();
            foreach (var c in cards)
            {
                var w = new CardView();
                w.Name = c.Name;
                w.ID = c.Id;
                if (c.IsTapped)
                    w.IsTapped = c.IsTapped;

                var super = string.Join(" ", c.Current.Supertype).Trim();
                var typ = string.Join(" ", GetCardType(c)).Trim();
                var sub = string.Join(" ", c.Current.Subtype).Trim();

                w.Type = $"{super} {typ} - {sub}".Trim(' ', '-');

                if (c.isCardType(CardType.Creature))
                {
                    w.Power = c.Current.Power;
                    w.Thoughness = c.Current.Thoughness;
                }

                if (showController)
                    w.Player = c.Controller.Name;

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

        public static List<ManaView> ManaViewPopulator(ManaCost manapool)
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
    }


    public class GameView
    {
        public List<CombatView> Combat = new List<CombatView>();
        public List<PlayerView> Players = new List<PlayerView>();
        public List<CardView> Stack = new List<CardView>();
        public List<string> Steps = new List<string>();
        public List<string> Turns = new List<string>();
    }

    public class CombatView
    {
        public List<string> Blockers = new List<string>();
        public string Attacker { get; set; }
    }

    public class PlayerView
    {
        public List<CardView> Battlefield = new List<CardView>();
        public List<CardView> Command = new List<CardView>();
        public List<CardView> Exile = new List<CardView>();
        public List<CardView> Graveyard = new List<CardView>();
        public List<CardView> Hand = new List<CardView>();
        public List<ManaView> Manapool = new List<ManaView>();
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
        public int ID { get; set; }
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
    }
}