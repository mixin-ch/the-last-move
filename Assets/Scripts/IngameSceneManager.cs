using Mixin.Utils;

namespace Mixin.TheLastMove
{
    public class IngameSceneManager : Singleton<IngameSceneManager>
    {
        private void Start()
        {
            EnvironmentManager.Instance.StartGame();
        }
    }
}
