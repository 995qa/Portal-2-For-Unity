using UnityEngine;

public class PortalSpawner : MonoBehaviour 
{
	[SerializeField] private Color portalColor;
	[SerializeField] private Transform portalSpawnLocation;
	[SerializeField] private bool onStart;
	private bool blue;
	private int f;

	public void Spawn(Color color = Color.None)
	{
        if (color == Color.None) { color = portalColor; }

        Transform portal;
		if (color == Color.Blue)
		{
			portal = GameObject.FindGameObjectWithTag("Blue Portal").transform;
		}
		else
		{
            portal = GameObject.FindGameObjectWithTag("Orange Portal").transform;
        }
		portal.GetComponent<PortalTrigger>().placed = true;
		portal.rotation = portalSpawnLocation.rotation;
		portal.position = portalSpawnLocation.position;
    }
	void Update()
	{
		f++;
		if (onStart && portalColor == Color.Orange && f == 5)
		{
            Spawn();
        }
        else if (onStart && portalColor != Color.Orange && !blue)
		{
			blue = true;
			Spawn();
		}
	}
	public enum Color
	{
		None,
		Blue,
		Orange
	}
}
