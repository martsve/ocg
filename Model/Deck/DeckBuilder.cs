using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
                var name = Regex.Replace(cardName, @"[^a-zA-Z]", "");
                var obj = Assembly.GetExecutingAssembly().CreateInstance($"Delver.Cards.{name}") as Card;
                if (obj == null)
                    throw new Exception($"No such card: Delver.Cards.{name}");
                result.Add(obj);
            }
            return result;
        }
    }

    public class Decklist
    {
        public List<string> Cards { get; set; } = new List<string>();

        public Decklist(string text = "")
        {
            var lines = text.Replace("\r", "").Split('\n');
            foreach (var line in lines)
            {
                var str = line.Trim();
                if (str.Length == 0)
                    continue;

                var w = str.Split(' ');
                int N;
                if (int.TryParse(w[0], out N))
                {
                    var name = string.Join(" ", w.Skip(1));
                    Add(name, N);
                }
                else {
                    throw new Exception("Unable to read decklist");
                }
            }
        }

        public void Add(string cardName, int count = 1)
        {
            // verify card name...

            // add to deck
            for (var i = 0; i < count; i++)
                Cards.Add(cardName);
        }
    }
}
