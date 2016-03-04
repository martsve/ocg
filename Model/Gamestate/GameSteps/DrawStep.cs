using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class DrawStep : GameStep
    {
        public DrawStep(Game game) : base(game, StepType.Draw)
        {
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();

            // 504.1. First, the active player draws a card. This turn-based action doesn’t use the stack.
            if (game.TurnNumber > 1)
                game.Methods.DrawCard(ap);

            // 504.2. Second, any abilities that trigger at the beginning of the draw step and any other abilities that have triggered go on the stack.
            game.Methods.TriggerEvents(new EventInfo.BeginningOfDrawstep(game, ap));

            // 504.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }
    }
}