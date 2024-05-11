using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOcclusionReceive : MonoBehaviour
{
    [SerializeField] private List<Renderer> meshRenderers;
    

    private List<Material> materials = new List<Material>();

    private Coroutine fadeOut;
    private Coroutine fadeIn;

    //private GameObject shadowCasterObject;

    [HideInInspector] public bool isActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Renderer renderer in meshRenderers)
        {
            materials.Add(renderer.material);
        }
    }

    public void StartFadeOut()
    {
        //Debug.Log("ACTIVATING" + name, gameObject);

        try
        {
            StopCoroutine(fadeIn);
        }
        catch
        {

        }

        if(isActivated == false)
        {
            fadeOut = StartCoroutine(FadeOut());

            GameObject shadowCasterObject = Instantiate(gameObject, transform.position, transform.rotation, transform.parent);
            shadowCasterObject.tag = "Cloned";

            foreach(Renderer renderer in shadowCasterObject.GetComponent<CamOcclusionReceive>().meshRenderers)
            {
                renderer.material.shader = Shader.Find("Shader Graphs/TransparentWithShadow");
            }

            foreach(Component component in shadowCasterObject.GetComponents<Component>())
            {
                if(component.GetType() != typeof(Transform) && component.GetType() != typeof(MeshRenderer) && component.GetType() != typeof(MeshFilter))
                {
                    Destroy(component);
                }
            }

            shadowCasterObject.AddComponent<DestroySelf>();
            
        }
    }

    public void StartFadeIn()
    {
        try
        {
            StopCoroutine(fadeOut);
        }
        catch
        {

        }

        fadeIn = StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        isActivated = true;
        float elapsedTime = 0f;
        float t = 0f;
        float time = 0.4f;

        float startingOpacity = materials[0].GetFloat("_OpacityOverride");
        float finalOpacity;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            foreach(Material material in materials)
            {
                finalOpacity = Mathf.Lerp(1, 0.125f, t);
                material.SetFloat("_OpacityOverride", finalOpacity);
            }
            
            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        foreach (Material material in materials)
        {
            material.SetFloat("_OpacityOverride", 0.125f);
        }
    }

    IEnumerator FadeIn()
    {
        isActivated = false;
        float elapsedTime = 0f;
        float t = 0f;
        float time = 0.3f;

        float startingOpacity = materials[0].GetFloat("_OpacityOverride");
        float finalOpacity;

        while (elapsedTime < time)
        {
            t = elapsedTime / time;

            foreach (Material material in materials)
            {
                finalOpacity = Mathf.Lerp(startingOpacity, 1, t);
                material.SetFloat("_OpacityOverride", finalOpacity);
            }

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        foreach (Material material in materials)
        {
            material.SetFloat("_OpacityOverride", 1);
        }
    }
}
