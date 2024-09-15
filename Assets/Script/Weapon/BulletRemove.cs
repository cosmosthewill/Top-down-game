using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRemove : MonoBehaviour
{
    public float removeTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, removeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
