using UnityEngine;

namespace Script.Player.PowerUpScript
{
    public class PowerUp : MonoBehaviour
    {
        public virtual float cdTime { get; }
        public virtual float spawnTime { get; set; }
        public virtual int lvl { get; set; }
    }
}
