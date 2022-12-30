using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mixin.Board
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
