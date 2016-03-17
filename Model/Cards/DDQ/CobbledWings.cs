//CobbledWings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class CobbledWings : Artifact 
    {
        public CobbledWings() : base("Artifact � Equipment, 2 (2)")
        {
            Name = "Cobbled Wings";
            Current.Text = @"Equipped creature has flying. Equip {1} ({1}: Attach to target creature you control. Equip only as a sorcery.)";
            throw new NotImplementedException();
        }
    }
}
