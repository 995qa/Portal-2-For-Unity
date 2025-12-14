using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
public class PortalTrigger : MonoBehaviour
{
    [System.Serializable]
    public class Checks
    {
        [SerializeField] public Transform top;
        [SerializeField] public Transform right;
        [SerializeField] public Transform bottom;
        [SerializeField] public Transform left;
    }
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform otherPortal;
    [SerializeField] private BoxCollider portalBoxCollider;
    [SerializeField] private AudioClip[] enter;
    [SerializeField] private AudioClip[] exit;
    [SerializeField] private float vol;
    [SerializeField] private LayerMask parallelBumperLayers;
    [SerializeField] private LayerMask perpendicularBumperLayers;
    public List<GameObject> portalableObjects;
    public bool placed;
    private Vector3 oldPos;
    public List<bool> positive;
    public List<bool> prev;
    public List<GameObject> colls;
    Vector3 vel;
    [Space]
    [Space]
    [Space]
    [Space]
    [SerializeField] private Checks parallelChecks;
    [Space]
    [Space]
    [SerializeField] private Checks perpendicularChecks;
    void Start()
    {
        playerTransform = Camera.main.transform.parent;
        oldPos = playerTransform.position;
        colls = new List<GameObject>();
    }
    void Update()
    {
        Debug.DrawLine(parallelChecks.top.position, parallelChecks.top.position + parallelChecks.top.forward * 0.8f, Color.red);
        Debug.DrawLine(parallelChecks.bottom.position, parallelChecks.bottom.position + parallelChecks.bottom.forward * 0.8f, Color.red);
        Debug.DrawLine(parallelChecks.right.position, parallelChecks.right.position + parallelChecks.right.forward * 0.4f, Color.red);
        Debug.DrawLine(parallelChecks.left.position, parallelChecks.left.position + parallelChecks.left.forward * 0.4f, Color.red);
        Debug.DrawLine(perpendicularChecks.top.position, perpendicularChecks.top.position + perpendicularChecks.top.forward * 0.1f, Color.red);
        Debug.DrawLine(perpendicularChecks.bottom.position, perpendicularChecks.bottom.position + perpendicularChecks.bottom.forward * 0.1f, Color.red);
        Debug.DrawLine(perpendicularChecks.right.position, perpendicularChecks.right.position + perpendicularChecks.right.forward * 0.1f, Color.red);
        Debug.DrawLine(perpendicularChecks.left.position, perpendicularChecks.left.position + perpendicularChecks.left.forward * 0.1f, Color.red);
        Debug.DrawLine(this.transform.position, this.transform.position+this.transform.forward);
        Debug.DrawLine(otherPortal.transform.position, otherPortal.transform.position + vel, Color.red);
    }
    void FixedUpdate()
    {
        portalBoxCollider.size = new Vector3(portalBoxCollider.size.x, portalBoxCollider.size.y, 0.02f + Mathf.Abs((playerTransform.position - oldPos).magnitude)*5);
        oldPos = playerTransform.position;
        if (positive == null)
        {
            positive = new List<bool>();
            for (int i = 0; i < portalableObjects.Count; i++)
            {
                positive.Add(true);
            }
        }
        while (positive.Count != portalableObjects.Count)
        {
            positive.Add(true);
        }
        prev = positive;
        if (positive.Count > 0)
        {
            positive = new List<bool>();
            for (int i = 0; i < portalableObjects.Count; i++)
            {
                float signedDistance = Vector3.Dot(portalableObjects[i].transform.position - transform.position, transform.forward);
                if (signedDistance > 0)
                {
                    positive.Add(true);
                }
                else
                {
                    positive.Add(false);
                }
            }
            if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
            {
                for (int i = 0; i < portalableObjects.Count; i++)
                {
                    if (prev[i] && !positive[i])
                    {
                        if (colls.Contains(portalableObjects[i]))
                        {
                            Teleport(portalableObjects[i]);
                        }
                    }
                }
            }
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
        {
            AudioManager.Instance.Play(transform, enter[Random.Range(0, enter.Length)], AudioManager.Mixer.SFX, vol, true, transform.position, false);
            col.gameObject.layer = LayerMask.NameToLayer("Portal Collider");
            GameObject go = col.gameObject;
            if (go.GetComponent<Parent>()  != null)
            {
                go = go.GetComponent<Parent>().parent;
            }
            colls.Add(go);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
        {
            AudioManager.Instance.Play(transform, exit[Random.Range(0, exit.Length)], AudioManager.Mixer.SFX, vol, true, transform.position, false);
        }
        if (col.gameObject.name == "Player" || col.gameObject.name == "Portal Trigger")
        {
            col.gameObject.layer = LayerMask.NameToLayer("Player");
        }
        else
        {
            col.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        GameObject go = col.gameObject;
        if (go.GetComponent<Parent>() != null)
        {
            go = go.GetComponent<Parent>().parent;
        }
        colls.Remove(go);
    }
    public void Teleport(GameObject go)
    {
        if (otherPortal.gameObject.GetComponent<PortalTrigger>().placed)
        {
            go.transform.position -= transform.position;
            go.transform.position = -go.transform.position;
            go.transform.position += otherPortal.position + (otherPortal.forward * 0.1f * 2.5f);

            Quaternion delta = Quaternion.FromToRotation(-this.transform.forward, otherPortal.transform.forward);

            if (otherPortal.transform.rotation.eulerAngles.x != 0 || this.transform.rotation.eulerAngles.x != 0)
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.eulerAngles.x + delta.eulerAngles.x, go.transform.rotation.eulerAngles.y + delta.eulerAngles.y + 180, go.transform.rotation.eulerAngles.z + delta.eulerAngles.z));
            }
            else
            {
                go.transform.rotation = Quaternion.Euler(new Vector3(go.transform.rotation.eulerAngles.x + delta.eulerAngles.x, go.transform.rotation.eulerAngles.y + delta.eulerAngles.y, go.transform.rotation.eulerAngles.z + delta.eulerAngles.z));
            }
            if (go.GetComponent<Rigidbody>() != null)
            {
                go.GetComponent<Rigidbody>().velocity = delta * go.GetComponent<Rigidbody>().velocity;
                vel = go.GetComponent<Rigidbody>().velocity;
            }
        }
    }
    public bool Place(Vector3 position, Quaternion rotation)
    {
        float d = 0.05f;
        Vector3 prevPos = transform.position;
        Quaternion prevRot = transform.rotation;
        transform.position = position;
        transform.rotation = rotation;
        bool plt = !Physics.Raycast(parallelChecks.top.position, parallelChecks.top.forward, 0.8f, parallelBumperLayers);
        bool plb = !Physics.Raycast(parallelChecks.bottom.position, parallelChecks.bottom.forward, 0.8f, parallelBumperLayers);
        bool pll = !Physics.Raycast(parallelChecks.left.position, parallelChecks.left.forward, 0.4f, parallelBumperLayers);
        bool plr = !Physics.Raycast(parallelChecks.right.position, parallelChecks.right.forward, 0.4f, parallelBumperLayers);
        bool prt = Physics.Raycast(perpendicularChecks.top.position, perpendicularChecks.top.forward, 0.1f, perpendicularBumperLayers);
        bool prb = Physics.Raycast(perpendicularChecks.bottom.position, perpendicularChecks.bottom.forward, 0.1f, perpendicularBumperLayers);
        bool prl = Physics.Raycast(perpendicularChecks.left.position, perpendicularChecks.left.forward, 0.1f, perpendicularBumperLayers);
        bool prr = Physics.Raycast(perpendicularChecks.right.position, perpendicularChecks.right.forward, 0.1f, perpendicularBumperLayers);
        int c = 0;
        while (!plt || !plb || !plr || !prt || !prb || !prl || !prr)
        {
            c++;
            if (c > 100)
            {
                transform.position = prevPos;
                transform.rotation = prevRot;
                return false;
            }
            if (!plt)
            {
                transform.position -= parallelChecks.top.forward * d;
            }
            if (!plb)
            {
                transform.position += parallelChecks.top.forward * d;
            }
            if (!pll)
            {
                transform.position -= parallelChecks.left.forward * d;
            }
            if (!plr)
            {
                transform.position += parallelChecks.left.forward * d;
            }
            plt = !Physics.Raycast(parallelChecks.top.position, parallelChecks.top.forward, 0.8f, parallelBumperLayers);
            plb = !Physics.Raycast(parallelChecks.bottom.position, parallelChecks.bottom.forward, 0.8f, parallelBumperLayers);
            pll = !Physics.Raycast(parallelChecks.left.position, parallelChecks.left.forward, 0.4f, parallelBumperLayers);
            plr = !Physics.Raycast(parallelChecks.right.position, parallelChecks.right.forward, 0.4f, parallelBumperLayers);
            prt = Physics.Raycast(perpendicularChecks.top.position, perpendicularChecks.top.forward, 0.1f, perpendicularBumperLayers);
            prb = Physics.Raycast(perpendicularChecks.bottom.position, perpendicularChecks.bottom.forward, 0.1f, perpendicularBumperLayers);
            prl = Physics.Raycast(perpendicularChecks.left.position, perpendicularChecks.left.forward, 0.1f, perpendicularBumperLayers);
            prr = Physics.Raycast(perpendicularChecks.right.position, perpendicularChecks.right.forward, 0.1f, perpendicularBumperLayers);
            if (!prt)
            {
                transform.position -= parallelChecks.top.forward * d;
            }
            if (!prb)
            {
                transform.position += parallelChecks.top.forward * d;
            }
            if (!prl)
            {
                transform.position -= parallelChecks.left.forward * d;
            }
            if (!prr)
            {
                transform.position += parallelChecks.left.forward * d;
            }
            plt = !Physics.Raycast(parallelChecks.top.position, parallelChecks.top.forward, 0.8f, parallelBumperLayers);
            plb = !Physics.Raycast(parallelChecks.bottom.position, parallelChecks.bottom.forward, 0.8f, parallelBumperLayers);
            pll = !Physics.Raycast(parallelChecks.left.position, parallelChecks.left.forward, 0.4f, parallelBumperLayers);
            plr = !Physics.Raycast(parallelChecks.right.position, parallelChecks.right.forward, 0.4f, parallelBumperLayers);
            prt = Physics.Raycast(perpendicularChecks.top.position, perpendicularChecks.top.forward, 0.1f, perpendicularBumperLayers);
            prb = Physics.Raycast(perpendicularChecks.bottom.position, perpendicularChecks.bottom.forward, 0.1f, perpendicularBumperLayers);
            prl = Physics.Raycast(perpendicularChecks.left.position, perpendicularChecks.left.forward, 0.1f, perpendicularBumperLayers);
            prr = Physics.Raycast(perpendicularChecks.right.position, perpendicularChecks.right.forward, 0.1f, perpendicularBumperLayers);
            if ((!plt && !plb) || (!pll && !plr) || (!prt && !prb) || (!prl && !prr))
            {
                transform.position = prevPos;
                transform.rotation = prevRot;
                return false;
            }
        }
        return true;
    }
}
