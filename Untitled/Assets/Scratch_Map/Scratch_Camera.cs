using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scratch_Camera : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    // Use this for initialization
    void Start()
    {
        offset = new Vector3(-8.0f, 8.0f, -8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + offset;
    }
}
