using System.Threading.Tasks;

namespace JFramework.Game
{
    public interface IJCombatPlayer
    {
        Task Play();

        void RePlay();

        void Stop();

        void SetScale(float scale);

        float GetScale();
    }
}
