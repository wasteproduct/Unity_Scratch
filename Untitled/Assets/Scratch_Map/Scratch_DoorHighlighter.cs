using UnityEngine;

public class Scratch_DoorHighlighter : MonoBehaviour
{
    Color original;

    // Use this for initialization
    void Start()
    {
        original = this.GetComponent<Renderer>().sharedMaterial.color;
    }

    private void OnMouseEnter()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = original;
    }
}
