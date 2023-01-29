using UnityEngine;

public class SpriteToCameraFitter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    [SerializeField]
    private bool keepAspectRatio = true;

    public bool KeepAspectRatio { get => keepAspectRatio; set => keepAspectRatio = value; }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        Fit();
    }

    public void Fit()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraAspect = mainCamera.aspect;
        float cameraWidth = cameraHeight * cameraAspect;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteAspect = spriteWidth / spriteHeight;

        float scaleFactor = Mathf.Max(cameraHeight / spriteHeight, cameraWidth / spriteWidth);

        if (keepAspectRatio)
        {
            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        }
        else
        {
            transform.localScale = new Vector3(cameraWidth / spriteWidth, cameraHeight / spriteHeight, 1);
        }
    }
}