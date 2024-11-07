using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UltimateBase : MonoBehaviour
{
    public abstract float UltDuration { get; }
    public abstract void Initialize(Transform playerTransform);
}
