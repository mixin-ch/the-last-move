using UnityEngine;
using System.Collections;

public class ParallaxMover : MonoBehaviour
{
    public float speed = 1f; // the speed at which the environment should move

    void Start()
    {
        StartCoroutine(MoveEnvironment());
    }

    IEnumerator MoveEnvironment()
    {
        while (true)
        {
            // move the environment horizontally based on the speed
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }
}
