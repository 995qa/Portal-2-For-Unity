using UnityEngine;

public class ChamberlockDoor : MonoBehaviour 
{
    [SerializeField] private FloorButtonManager[] buttons;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform[] spinny;
    [SerializeField] private float open;
    [SerializeField] private float speed;
    private bool isOpen;
    private float pos;
    private float delta;
    private Vector3 initL;
    private Vector3 initR;
    void Start()
    {
        initL = left.position;
        initR = left.position;
        delta = open * speed;
    }
    void Update()
    {
        isOpen = true;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].isDown)
            {
                isOpen = false;
            }
        }
        if (isOpen)
        {
            pos += delta * Time.deltaTime;
        }
        else
        {
            pos -= delta * Time.deltaTime;
        }
        if (pos > open)
        {
            pos = open;
        }
        if (pos < 0)
        {
            pos = 0;
        }
        float rot = Mathf.Min(pos, open / 2);
        rot /= (open / 2);
        rot *= 180;
        spinny[0].rotation = Quaternion.Euler(0, 0, -rot);
        spinny[1].rotation = Quaternion.Euler(0, 0, -rot);
        float x = Mathf.Max(pos, open / 2) - (open / 2);
        left.position = initL + new Vector3(x, 0, 0);
        right.position = initR + new Vector3(-x, 0, 0);
    }
}
