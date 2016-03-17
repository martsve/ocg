using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class DrawStep : GameStep
    {
        public DrawStep(Context Context) : base(Context, StepType.Draw)
        {
        }

        public override void Enter()
        {
            var ap = Context.Logic.GetActivePlayer();

            // 504.1. First, the active player draws a card. This turn-based action doesn’t use the stack.
            if (Context.TurnNumber > 1)
                Context.Methods.DrawCard(ap);

            // 504.2. Second, any abilities that trigger at the beginning of the draw step and any other abilities that have triggered go on the stack.
            Context.Methods.TriggerEvents(new EventInfoCollection.BeginningOfDrawstep(ap));

            // 504.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            Context.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            Context.Methods.EmptyManaPools();
        }
    }
}