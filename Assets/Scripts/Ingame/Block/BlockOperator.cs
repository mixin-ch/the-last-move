using Mixin.TheLastMove.Environment;
using System.Collections.Generic;
using UnityEngine;

namespace Mixin.TheLastMove
{
    public class BlockOperator : MonoBehaviour
    {
        [System.Obsolete]
        [SerializeField]
        private SpriteRenderer _sprite;

        [SerializeField]
        private SpriteRenderer _mainSprite;

        [SerializeField]
        private SpriteRenderer _fundament;

        [SerializeField]
        private SpriteRenderer[] _decorationList;

        public Vector2 Position => transform.position;

        public SpriteRenderer MainSprite { get => _mainSprite; }

        public SpriteRenderer Fundament { get => _fundament; }
        public SpriteRenderer[] DecorationList { get => _decorationList; }

        public void Setup(Vector3 position, float size, BiomeSO biome)
        {
            BlockOperator prefab = biome.Prefab.GetComponent<BlockOperator>();

            transform.position = position;
            transform.localScale = Vector2.one * size;

            _mainSprite.sprite = biome.Sprite;
            _mainSprite.gameObject.transform.localPosition =
                prefab.MainSprite.transform.localPosition;

            _fundament.color =
                 prefab._fundament.color;

            SetDecoration(prefab);
        }

        public void Move(Vector2 offset)
        {
            transform.position += (Vector3)offset;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void SetDecoration(BlockOperator prefab)
        {
            for (int i = 0; i < _decorationList.Length; i++)
            {
                // Set sprite
                _decorationList[i].sprite = prefab.DecorationList[i].sprite;

                // Set position
                _decorationList[i].transform.localPosition = prefab.DecorationList[i].transform.localPosition;

                // Set object active or not
                _decorationList[i].gameObject.SetActive(prefab.DecorationList[i].gameObject.activeSelf);
            }
        }
    }
}
