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

        [SerializeField]
        private SpriteToCameraFitter _spriteToCameraFitter;

        [SerializeField]
        private SpriteRenderer[] _fogList;

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
            _spriteToCameraFitter.Fit();

            // Set fog color
            for (int i = 0; i < _fogList.Length; i++)
            {
                _fogList[i].color = biome.FogColor;
            }
        }
    }
}