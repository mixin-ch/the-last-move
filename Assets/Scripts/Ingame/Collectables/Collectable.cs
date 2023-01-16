using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment.Collectable
{

    public class Collectable : MonoBehaviour
    {
        private BoxCollider2D _collider;

        [SerializeField]
        private GameObject _collectableModel;

        public static event Action OnCollected;


        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        public void Activate()
        {
            // Enable the collectable's renderer and collider
            _collectableModel.SetActive(true);
            _collider.enabled = true;
        }

        public void Deactivate()
        {
            // Disable the collectable's renderer and collider
            _collectableModel.SetActive(false);
            _collider.enabled = false;
        }

        public void Collect()
        {
            // Handle the logic for what happens when the player collects the collectable
            // e.g. play a sound, give the player points, etc.
            // or you can call an event that you can listen in the player script.
            Debug.Log("Collectable Collected!");

            Deactivate();

            OnCollected?.Invoke();
        }
    }
}