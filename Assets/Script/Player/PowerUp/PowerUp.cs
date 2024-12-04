using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public abstract float cdTime { get; }
    public abstract float spawnTime { get; set; }
    public abstract int lvl { get; set; }
}
