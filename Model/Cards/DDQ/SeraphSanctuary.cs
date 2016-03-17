//SeraphSanctuary
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class SeraphSanctuary : Land 
    {
        public SeraphSanctuary() : base()
        {
            Name = "Seraph Sanctuary";
            Base.Text = @"When Seraph Sanctuary enters the battlefield, you gain 1 life. Whenever an Angel enters the battlefield under your control, you gain 1 life. {T}: Add {C} to your mana pool.";
            throw new NotImplementedException();
        }
    }
}
