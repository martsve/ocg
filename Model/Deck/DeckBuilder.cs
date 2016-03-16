using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Delver
{
    static class DeckBuilder
    {
        public static List<Card> Build(Decklist decklist)
        {
            return Build(decklist.Cards);
        }

        public static List<Card> Build(List<string> cards)
        {
            var result = new List<Card>();
            foreach (var cardName in cards)
            {
                Card obj;
                try
                {
                    obj =
                        (Card) Assembly.GetExecutingAssembly().CreateInstance($"Delver.Cards.TestCards.{cardName}");
                    result.Add(obj);
                }
                catch
                {
                    throw new Exception($"No such card: {cardName}");
                }

            }
            return result;
        }
    }

    public class Decklist
    {
        public List<string> Cards { get; set; } = new List<string>();

        public void Add(string cardName, int count = 1)
        {
            // verify card name...

            // add to deck
            for (var i = 0; i < count; i++)
                Cards.Add(cardName);
        }
    }
}
