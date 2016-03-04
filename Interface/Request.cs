using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delver.Interface
{

    [Serializable]
    public enum RequestType
    {
        DiscardACard,
        TakeAction,
        Activate,
        SelectTarget,
        Cast,
        Mulligan,
        StartPlayer,
        ManaAbility,
        SelectAbility,
        Scry,
        OrderTriggers,
        SelectDefender,
        SelectAttacker,
        SelectBlocker,
        SelectAttackerToBlock,
        Attacking,
        ConfirmAttack,
        ConfirmBlock,
        OrderAttackers,
        OrderBlockers,
        AssignDamage
    }

    [Serializable]
    public class InputRequest
    {
        public string Text { get; set; }
        public RequestType Type { get; set; }
        public bool YourTurn { get; set; }
        public bool Mainphase { get; set; }
        public bool EmptyStack { get; set; }

        public InputRequest(RequestType type, string text)
        {
            this.Text = text;
            this.Type = type;
        }
    }
}
