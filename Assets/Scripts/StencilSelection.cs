using System.Collections.Generic;
using System.Linq;
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
        List<int> picked = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, scobjs.Length);
            var selected = scobjs[index];
            do
            {
                (selected, index) = PickOneRandom(scobjs.ToList());
            } while (picked.Contains(index));

            picked.Add(index);
            Debug.Log(selected.name);
            
            var s = Instantiate(stencilPrefab,
                new Vector3(transform.position.x, transform.position.y + (i * 0.2f) - 0.1f,
                    transform.position.z + 0.5f), transform.rotation);
            var stencil = s.GetComponent<StencilScript>();
            stencil.stencil = selected;
            stencil.UpdateStencil();
            stencilScripts.Add(stencil);
        }
    }

    public static (StencilScobj, int) PickOneRandom(List<StencilScobj> list)
    {
        float totalWeight = 0;
        float cumulativeTotal = 0;

        totalWeight = list.Sum(item => item.randomWeight);
        Debug.Log("totalweight: " + totalWeight);

        float rand = Random.Range(0f, totalWeight);

        var pickedItem = list.Find(item =>
            {
                cumulativeTotal += item.randomWeight;
                return cumulativeTotal >= rand;
            }
        );

        return (pickedItem, list.IndexOf(pickedItem));
    }
}