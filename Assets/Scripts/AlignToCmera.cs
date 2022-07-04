using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToCmera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Camera.main.transform.position - transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 2f * Time.deltaTime);
    }
}
