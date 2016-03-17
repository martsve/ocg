using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class PreMainPhase : GameStep
    {
        public PreMainPhase(Context Context) : base(Context, StepType.PreMain)
        {
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();

            // 505.4. Second, any abilities that trigger at the beginning of the main phase go on the stack. (See rule 603, “Handling Triggered Abilities.”)
            Context.Methods.TriggerEvents(new EventInfoCollection.BeginningOfMainStep(ap));

            // 505.5. Third, the active player gets priority. Players may cast spells and activate abilities. The active player may play a land.
            Context.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}