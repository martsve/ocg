//TranquilCove
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class TranquilCove : Land 
    {
        public TranquilCove() : base()
        {
            Name = "Tranquil Cove";
            Base.Text = @"Tranquil Cove enters the battlefield tapped. When Tranquil Cove enters the battlefield, you gain 1 life. {T}: Add {W} or {U} to your mana pool.";
            NotImplemented();
        }
    }
}

