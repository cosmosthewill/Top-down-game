using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [Tooltip("Number of sprites per distance unit.")]
    [SerializeField] private float rate = 2f;
    [SerializeField] private float lifeTime = 0.15f;

    private SpriteRenderer baseRenderer;
    private bool isActive = false;
    private float interval;
    private Vector3 previousPos;
    private List<GameObject> activeTrailParts = new List<GameObject>();
    private void Start()
    {
        baseRenderer = GetComponent<SpriteRenderer>();
        interval = 1f / rate;
    }

    private void Update()
    {
        if (isActive && Vector3.Distance(previousPos, transform.position) > interval)
        {
            SpawnTrailPart();
            previousPos = transform.position;
        }
    }

    /// Call this function to start/stop the trail.
    public void Activate(bool shouldActivate)
    {
        isActive = shouldActivate;
        if (isActive)
            previousPos = transform.position;
    }

    private void SpawnTrailPart()
    {
        GameObject trailPart = new GameObject(gameObject.name + " trail part");

        // Sprite renderer
        SpriteRenderer trailPartRenderer = trailPart.AddComponent<SpriteRenderer>();
        CopySpriteRenderer(trailPartRenderer, baseRenderer);

        /*color
        Color trailColor = Color.white;
        trailColor.a = 0.1f; // Semi-transparent
        trailPartRenderer.color = trailColor;*/

        // Transform
        trailPart.transform.position = transform.position;
        trailPart.transform.rotation = transform.rotation;
        trailPart.transform.localScale = transform.lossyScale;

        // Add to active list
        activeTrailParts.Add(trailPart);

        // Sprite rotation
        //trailPart.AddComponent<CameraSpriteRotater>();

        // Fade & Destroy
        StartCoroutine(FadeTrailPart(trailPartRenderer));
    }

    private IEnumerator FadeTrailPart(SpriteRenderer trailPartRenderer)
    {
        float fadeSpeed = 1 / lifeTime;
        trailPartRenderer.color = Color.white;
        while (trailPartRenderer.color.a > 0)
        {
            Color color = trailPartRenderer.color;
            color.a -= fadeSpeed * Time.deltaTime;
            trailPartRenderer.color = color;

            yield return new WaitForEndOfFrame();
        }

        Destroy(trailPartRenderer.gameObject);
    }

    private static void CopySpriteRenderer(SpriteRenderer copy, SpriteRenderer original)
    {
        // Can modify to only copy what you need!
        copy.sprite = original.sprite;
        copy.flipX = original.flipX;
        copy.flipY = original.flipY;
        copy.sortingLayerID = original.sortingLayerID;
        copy.sortingLayerName = original.sortingLayerName;
        copy.sortingOrder = original.sortingOrder;
    }
    private void OnDestroy()
    {
        // Clean up any remaining trail parts
        foreach (var trailPart in activeTrailParts)
        {
            if (trailPart != null)
                Destroy(trailPart);
        }
    }
}
