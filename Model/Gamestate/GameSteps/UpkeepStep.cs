using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class UpkeepStep : GameStep
    {
        public UpkeepStep(Context Context) : base(Context, StepType.Upkeep)
        {
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();

            // 504.2. Second, any abilities that trigger at the beginning of the draw step and any other abilities that have triggered go on the stack.
            Context.Methods.TriggerEvents(new EventInfoCollection.BeginningOfUpkeep(ap));

            // 504.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            Context.Logic.SetWaitingPriorityList();
        }

        public override void Interact()
        {
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}