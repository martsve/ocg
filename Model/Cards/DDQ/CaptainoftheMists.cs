//CaptainoftheMists
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delver.Interface;
using Delver;
namespace Delver.Cards.TestCards
{
    [Serializable]
    class CaptainoftheMists : Creature 
    {
        public CaptainoftheMists() : base("Creature � Human Wizard 2/3, 2U (3)")
        {
            Name = "Captain of the Mists";
            Current.Text = @"Whenever another Human enters the battlefield under your control, untap Captain of the Mists. {1}{U}, {T}: You may tap or untap target permanent.";
            throw new NotImplementedException();
        }
    }
}
