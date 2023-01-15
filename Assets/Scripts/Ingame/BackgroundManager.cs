using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgrounds; // array to hold all background layers
    public float parallaxScale; // the proportion of environment movement to move the backgrounds by
    public float parallaxReductionFactor; // how much to reduce the parallax effect on each layer
    public float smoothing; // how smooth the parallax effect will be

    private Vector3 previousEnvironmentPos; // position of environment in previous frame

    void Start()
    {
        previousEnvironmentPos = transform.position;
        StartCoroutine(MoveBackground());
    }

    IEnumerator MoveBackground()
    {
        while (true)
        {
            // the parallax is the opposite of the environment movement since the previous frame multiplied by the scale
            float parallax = (previousEnvironmentPos.x - transform.position.x) * parallaxScale;

            // for each background
            for (int i = 0; i < backgrounds.Length; i++)
            {
                // set a target x position which is the current position plus the parallax
                float backgroundTargetPosX = backgrounds[i].transform.position.x + parallax * (i * parallaxReductionFactor + 1);

                // create a target position which is the background's current position but with it's target x position
                Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);

                // fade between current position and the target position using lerp
                backgrounds[i].transform.position = Vector3.Lerp(backgrounds[i].transform.position, backgroundTargetPos, smoothing * Time.deltaTime);
            }

            // set previousEnvironmentPos to the environment's position at the end of the frame
            previousEnvironmentPos = transform.position;

            yield return null;
        }
    }
}
