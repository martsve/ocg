//MoorlandInquisitor
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class MoorlandInquisitor : Creature 
    {
        public MoorlandInquisitor() : base("1W", 2, 2)
        {
            Name = "Moorland Inquisitor";
            Base.Subtype.Add("Human");
            Base.Subtype.Add("Soldier");
            Base.Text = @"{2}{W}: Moorland Inquisitor gains first strike until end of turn.";
            NotImplemented();
        }
    }
}
