//CobbledWings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class CobbledWings : Equipment 
    {
        public CobbledWings() : base("2", "1")
        {
            Name = "Cobbled Wings";
            Base.Text = @"Equipped creature has flying. Equip {1} ({1}: Attach to target creature you control. Equip only as a sorcery.)";
            NotImplemented();
        }
    }
}
