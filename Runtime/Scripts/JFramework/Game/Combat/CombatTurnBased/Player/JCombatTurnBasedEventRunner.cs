using System;
using System.Linq;
using System.Threading.Tasks;

namespace JFramework.Game
{

    public class JCombatTurnBasedEventRunner : BaseRunable, IDisposable
    {
        public IJCombatAnimationPlayer AnimationPlayer { get; set; }

        public virtual void Dispose()
        {
        }

        public override async Task Start(RunableExtraData extraData, TaskCompletionSource<bool> tcs = null)
        {
            if (IsRunning)
            {
                throw new Exception(this.GetType().ToString() + " is running , can't run again! ");
            }
            this.ExtraData = extraData;
            this.IsRunning = true;

            await DoStart(extraData);         
        }

        protected async Task DoStart(RunableExtraData extraData)
        {
            base.OnStart(extraData);

            var combatEvents = (JCombatTurnBasedEvent)extraData.Data;
           // var casterUid = combatEvents.CasterUid; //实现了icaster接口的对象
            var curFrame = combatEvents.CurFrame;
            //var actionUid = combatEvents.CastActionUid;
            

            //var allTargetUids = combatEvents.ActionEffect
            //    .SelectMany(effect => effect.Value)
            //    .Select(actionEffectInfo => actionEffectInfo.TargetUid)
            //    .ToList();

            //播放技能动画等（监听动画命中事件，然后继续）
            await AnimationPlayer?.PlayAcion(combatEvents);

            Stop();
        }
    }
}
