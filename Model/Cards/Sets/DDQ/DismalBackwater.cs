//DismalBackwater
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards
{
    [Serializable]
    class DismalBackwater : Land 
    {
        public DismalBackwater() : base()
        {
            Name = "Dismal Backwater";
            Base.Text = @"Dismal Backwater enters the battlefield tapped. When Dismal Backwater enters the battlefield, you gain 1 life. {T}: Add {U} or {B} to your mana pool.";
            throw new NotImplementedException();
        }
    }
}
