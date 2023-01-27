using Mixin.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove.Environment
{
    public class BackgroundManager : Singleton<BackgroundManager>
    {
        [SerializeField]
        private SpriteRenderer _background;

        private void Start()
        {
            WorldTransitionManager.OnBiomeChangeTransition += SetBiomeBackground;
        }

        private void OnDisable()
        {
            WorldTransitionManager.OnBiomeChangeTransition -= SetBiomeBackground;
        }

        public void Init()
        {
            SetBiomeBackground(EnvironmentManager.Instance.CurrentBiome);
        }

        private void SetBiomeBackground(BiomeSO biome)
        {
            _background.sprite = biome.Background;
        }
    }
}