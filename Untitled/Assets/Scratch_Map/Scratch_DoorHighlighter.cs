using UnityEngine;

public class Scratch_DoorHighlighter : MonoBehaviour
{
    private Color original;

    // Use this for initialization
    void Start()
    {
        original = this.GetComponent<Renderer>().sharedMaterial.color;
    }

    public void HighlightDoor()
    {
        this.GetComponent<Renderer>().material.color = Color.green;
    }

    public void ResetDoorColor()
    {
        this.GetComponent<Renderer>().material.color = original;
    }

    //private void OnMouseEnter()
    //{
    //    this.GetComponent<Renderer>().material.color = Color.green;
    //}

    //private void OnMouseExit()
    //{
    //    this.GetComponent<Renderer>().material.color = original;
    //}
}
