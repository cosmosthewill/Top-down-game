using System.Collections.Generic;
using UnityEngine;

public class BossIndicator : MonoBehaviour
{
    public static BossIndicator Instance;
    public GameObject[] targets;
    public GameObject indicatorPrefab;
    public Camera mainCamera; // Changed to mainCamera for clarity
    private SpriteRenderer sr;
    private float padding = 0.005f; // Add padding
    private Dictionary<GameObject, GameObject> targetIndicators = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        mainCamera = Camera.main;
        sr = indicatorPrefab.GetComponent<SpriteRenderer>();

        foreach (var target in targets)
        {
            if (target != null)
            {
                var indicator = Instantiate(indicatorPrefab);
                indicator.SetActive(true);
                targetIndicators.Add(target, indicator);
            }
        }
    }

    private void Update()
    {
        // Clean up destroyed targets
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var pair in targetIndicators)
        {
            if (pair.Key == null)
            {
                if (pair.Value != null)
                    Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }
        foreach (var target in toRemove)
        {
            targetIndicators.Remove(target);
        }

        // Update indicators
        foreach (KeyValuePair<GameObject, GameObject> entry in targetIndicators)
        {
            if (entry.Key != null && entry.Value != null)
            {
                UpdateTarget(entry.Key, entry.Value);
            }
        }
    }

    private void UpdateTarget(GameObject target, GameObject indicator)
    {
        // Get the viewport position of the target
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(target.transform.position);

        bool isOffScreen = viewportPoint.x <= 0 || viewportPoint.x >= 1 ||
                          viewportPoint.y <= 0 || viewportPoint.y >= 1 ||
                          viewportPoint.z < 0;

        if (isOffScreen)
        {
            indicator.SetActive(true);

            // Calculate the sprite size in viewport space
            float halfHeight = sr.bounds.extents.y;
            float halfWidth = sr.bounds.extents.x;
            Vector3 spriteSize = mainCamera.WorldToViewportPoint(new Vector3(halfWidth, halfHeight, 0)) -
                               mainCamera.WorldToViewportPoint(Vector3.zero);

            // Clamp the position with padding and sprite size consideration
            float clampedX = Mathf.Clamp(viewportPoint.x, padding + spriteSize.x, 1 - padding - spriteSize.x);
            float clampedY = Mathf.Clamp(viewportPoint.y, padding + spriteSize.y, 1 - padding - spriteSize.y);

            // If target is behind camera, clamp to nearest edge
            if (viewportPoint.z < 0)
            {
                viewportPoint.x = 1 - viewportPoint.x;
                viewportPoint.y = 1 - viewportPoint.y;
            }

            // Create the clamped viewport position
            Vector3 clampedViewportPos = new Vector3(clampedX, clampedY, 0);

            // Convert back to world space
            Vector3 worldPos = mainCamera.ViewportToWorldPoint(clampedViewportPos);
            worldPos.z = 0; // Keep at consistent depth

            indicator.transform.position = worldPos;
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    public void AddTarget(GameObject target)
    {
        if (target != null && !targetIndicators.ContainsKey(target))
        {
            var indicator = Instantiate(indicatorPrefab);
            indicator.SetActive(true);
            targetIndicators.Add(target, indicator);
        }
    }

    public void RemoveTarget(GameObject target)
    {
        if (targetIndicators.TryGetValue(target, out GameObject indicator))
        {
            if (indicator != null)
                Destroy(indicator);
            targetIndicators.Remove(target);
        }
    }
}