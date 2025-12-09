using UnityEngine;

public class SpawnerManager : MonoBehaviour 
{
	[SerializeField] private GameObject stuff;
	void Awake() 
	{
		Instantiate(stuff, transform.position, transform.rotation);		
	}
}
