//MoorlandInquisitor
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class MoorlandInquisitor : Creature 
    {
        public MoorlandInquisitor() : base("Creature � Human Soldier 2/2, 1W (2)")
        {
            Name = "Moorland Inquisitor";
            Current.Text = @"{2}{W}: Moorland Inquisitor gains first strike until end of turn.";
            throw new NotImplementedException();
        }
    }
}
