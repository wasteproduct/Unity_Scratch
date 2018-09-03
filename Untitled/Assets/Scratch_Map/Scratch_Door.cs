using UnityEngine;

public class Scratch_Door : MonoBehaviour
{
    public int X { get; private set; }
    public int Z { get; private set; }
    public bool Highlighted { get; private set; }

    private Scratch_DoorHighlighter doorHighlighter;

    // Use this for initialization
    void Start()
    {
        doorHighlighter = this.GetComponentInChildren<Scratch_DoorHighlighter>();

        Highlighted = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        GetComponent<Animator>().SetTrigger("DoorATrigger");
    }

    public void HighlightDoor()
    {
        doorHighlighter.HighlightDoor();

        Highlighted = true;
    }

    public void ResetDoorColor()
    {
        doorHighlighter.ResetDoorColor();

        Highlighted = false;
    }

    public void SetTileIndex(int x, int z)
    {
        X = x;
        Z = z;
    }
}
