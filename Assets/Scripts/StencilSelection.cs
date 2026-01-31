using System.Collections.Generic;
using UnityEngine;

public class StencilSelection : MonoBehaviour
{
    StencilScobj[] scobjs;
    
    public GameObject stencilPrefab;
    
    List<StencilScript> stencilScripts = new List<StencilScript>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scobjs = Resources.LoadAll<StencilScobj>("Stencils");
        LoadNewSelection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillOtherStencils(StencilScript stencil)
    {
        stencilScripts.Remove(stencil);
        foreach (var s in stencilScripts)
        {
            s.KillStencil();
        }
    }

    public void LoadNewSelection()
    {
        stencilScripts.Clear();
        List<StencilScobj> stencils = new List<StencilScobj>();
        List<int> picked = new List<int>();
        
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, scobjs.Length);
            while(picked.Contains(index))
            {
                index = Random.Range(0, scobjs.Length);
            }
            picked.Add(index);
            var selected = scobjs[index];
            Debug.Log(selected.name);
            stencils.Add(selected);
            var s = Instantiate(stencilPrefab, new Vector3(transform.position.x, transform.position.y + (i * 0.2f)-0.1f, transform.position.z+0.5f), transform.rotation);
            var stencil = s.GetComponent<StencilScript>();
            stencil.stencil = selected;
            stencil.UpdateStencil();
            stencilScripts.Add(stencil);
        }
    }
}
