using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRemover : MonoBehaviour
{
    public void RemoveItem()
    {
        Destroy(transform.parent.gameObject);
    }
}
