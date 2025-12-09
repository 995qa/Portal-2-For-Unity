using UnityEngine;

public class SpawnerManager : MonoBehaviour 
{
	[SerializeField] private GameObject stuff;
	[SerializeField] HPD hpd;
	void Awake() 
	{
		Transform things = Instantiate(stuff, transform.position, transform.rotation).transform;
		things = things.FindChild("Canvas");
	}

	private enum HPD
	{
		None,
		Blue,
		Both
	}
}
