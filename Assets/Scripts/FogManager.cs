using UnityEngine;

public class FogManager : MonoBehaviour 
{
	[SerializeField] private GameObject fog;
	[SerializeField] private float start;
    [SerializeField] private float count;
	[SerializeField] private float end;
	[SerializeField] private Material material;
    private float increment;
    void Start () 
	{
        increment = (end - start) / count;
        for (float i = start; i < end; i += increment)
        {
            material = new Material(material);
            material.color = new Color(material.color.r, material.color.r, material.color.r, ((3.5f / 8) * increment) / 255);
            GameObject go = Instantiate(fog, transform);
            go.transform.localScale = new Vector3(i, i, i);
            go.GetComponent<Renderer>().material = material;
        }
    }
}
