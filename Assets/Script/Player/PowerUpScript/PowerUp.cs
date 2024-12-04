using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public abstract class PowerUp : MonoBehaviour
    {
        public abstract string powerUpName { get; }
        public abstract float cdTime { get; }
        public abstract float spawnTime { get; set; }
        public abstract int lvl { get; set; }
    }
}
