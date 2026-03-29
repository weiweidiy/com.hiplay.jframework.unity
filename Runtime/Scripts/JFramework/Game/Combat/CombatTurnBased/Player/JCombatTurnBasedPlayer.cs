using System.Collections.Generic;
using System.Threading.Tasks;

namespace JFramework.Game
{
    public abstract class JCombatTurnBasedPlayer<T> : JCombatBasePlayer<T> where T : IJCombatUnitData
    {
        public JCombatTurnBasedPlayer(JCombatTurnBasedReportData<T> reportData, IJCombatAnimationPlayer animationPlayer, IObjectPool objPool = null) : base(reportData, animationPlayer,objPool)
        {
        }

        protected override async Task PlayEvents(List<JCombatTurnBasedEvent> events)
        {
            var que = new Queue<JCombatTurnBasedEvent>(events);

            int frame = -1;

            while (que.Count > 0)
            {
                if (!IsRunning)
                    break;

                var runner = GetEventRunner();
                runner.AnimationPlayer = animationPlayer;
                var combatEvent = que.Dequeue();
                var runableData = GetRunableData();
                runableData.Data = combatEvent;

                var curFrame = combatEvent.CurFrame;
                if(curFrame > frame)
                {
                    frame = curFrame;
                    await animationPlayer.PlayTurnStart(frame);
                }

                await runner.Start(runableData);

                ReleaseRunner(runner, runableData);
            }
        }
    }
}
