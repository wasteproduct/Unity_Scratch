using UnityEngine;

public class Scratch_Door : MonoBehaviour
{
    public int X { get; private set; }
    public int Z { get; private set; }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        GetComponent<Animator>().SetTrigger("DoorATrigger");
    }

    public void SetTileIndex(int x, int z)
    {
        X = x;
        Z = z;
    }
}
