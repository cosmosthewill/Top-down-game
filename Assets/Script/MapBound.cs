using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBound : MonoBehaviour
{
    public static MapBound Instance;
    public SpriteRenderer mapSprite; // Assign the map sprite renderer in the Inspector
    public Vector2 minBoundary;
    public Vector2 maxBoundary;
    private Vector2 offset = new Vector2 (5, 5);
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start()
    {
        mapSprite = GetComponent<SpriteRenderer>();
        Bounds mapBounds = mapSprite.bounds;

        minBoundary = new Vector2(mapBounds.min.x, mapBounds.min.y) - offset;
        maxBoundary = new Vector2(mapBounds.max.x, mapBounds.max.y) + offset;

        //Debug.Log($"Min Boundary: {minBoundary}, Max Boundary: {maxBoundary}");
    }
}
