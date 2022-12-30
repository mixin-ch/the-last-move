using Mixin.Utils;
using UnityEngine.UIElements;

namespace Mixin.TheLastMove
{
    public class UIBuildCollector<T> : Singleton<T> where T : Singleton<T>
    {
        protected VisualElement _root;

        protected override void Awake()
        {
            base.Awake();

            _root = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}
