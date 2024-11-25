using Runtime.Script.Pool;
using UnityEngine;

namespace Runtime.Script.Weapon
{
    public class BaseWeapon : MonoBehaviour
    {
        [SerializeField] private BaseBullet bulletPrefab;
        [SerializeField] private Transform firePosition;
        
        [Header("Base Stats")]
        [SerializeField] private float baseDamage;
        [SerializeField] private float baseFireCd;
        [SerializeField] private float baseBulletSpeed;
        
        public float BaseDamage => baseDamage;
        public float BaseFireCd => baseFireCd;
        public float BaseBulletSpeed => baseBulletSpeed;
        
        [SerializeField] private AudioSource shootingSound;
        
        public void RotateGun()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = mousePos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = rotation;

            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
            {
                transform.localScale = new Vector3(0.5f, -0.5f, 0);
            }
            else
                transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
        
        public virtual void FireBullet(float damage, float bulletSpeed)
        {
            var bulletGO = BulletPool.Instance.GetFromPool(bulletPrefab.GetType());
            if (bulletGO == null)
            {
                bulletGO = Instantiate(bulletPrefab, BulletPool.Instance.transform);
            }
            else
            {
                bulletGO.gameObject.SetActive(true);
            }
           
            bulletGO.transform.position = firePosition.position;
            
            //damage calulator
            var bullet = bulletGO.GetComponent<BaseBullet>();
            bullet?.Init(transform.right * bulletSpeed, damage, true);
        }
    }
}