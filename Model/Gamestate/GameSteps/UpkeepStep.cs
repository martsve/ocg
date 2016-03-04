using System;

namespace Delver.GameSteps
{
    [Serializable]
    internal class UpkeepStep : GameStep
    {
        public UpkeepStep(Game game) : base(game, StepType.Upkeep)
        {
        }

        public override void Enter()
        {
            var ap = game.Logic.GetActivePlayer();

            // 504.2. Second, any abilities that trigger at the beginning of the draw step and any other abilities that have triggered go on the stack.
            game.Methods.TriggerEvents(new EventInfo.BeginningOfUpkeep(game, ap));

            // 504.3. Third, the active player gets priority. Players may cast spells and activate abilities.
            game.Logic.SetWaitingPriorityList();
        }

        public override void Interact()
        {
        }

        public override void Exit()
        {
            game.Methods.EmptyManaPools();
        }
    }
}