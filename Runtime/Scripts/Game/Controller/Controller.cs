using System.Threading.Tasks;

///游戏可以服用
namespace JFramework.Unity
{
    public abstract class Controller
    {
        public abstract Task Do(GameContext context, params object[] parameters);
    }
}
