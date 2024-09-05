using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLightController : MonoBehaviour
{
    //controls the lightsources, turns them on, and after a while off, in delay to eachother
    public Light[] lightSources;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Light lightSource in lightSources)
        {
            lightSource.enabled = false;
        }
        StartCoroutine(ActivateLight());

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ActivateLight()
    {
        foreach (Light lightSource in lightSources)
        {
            lightSource.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        foreach (Light lightSource in lightSources)
        {
            lightSource.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ActivateLight());
    }
}
