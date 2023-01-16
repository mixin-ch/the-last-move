using System;
using UnityEngine;

namespace Mixin.TheLastMove.Environment.Collectable
{

    public class Collectable : MonoBehaviour
    {
        public static event Action OnCollected;

        public void Collect()
        {
            // Handle the logic for what happens when the player collects the collectable
            // e.g. play a sound, give the player points, etc.
            // or you can call an event that you can listen in the player script.
            Debug.Log("Collectable Collected!");

            OnCollected?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Collectable Collected! 2");
        }
    }
}