using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItems : MonoBehaviour
{
    public enum ItemType { Exp, Health, Speed, Mana }
    public ItemType itemType; // Set in the Inspector
    public int value = 10;    // Amount of experience, health, or speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
