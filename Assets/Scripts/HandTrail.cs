using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrail : MonoBehaviour
{
    [SerializeField]

    public TrailRenderer[] tr;
    public TrailRenderer tr2;
    Material m_Material;
    public GameObject ChnageColorGreen;
    public GameObject test;
    GameObject m_eraser;
    public GameObject eraser
    {
        get => m_eraser;
        set => m_eraser = value;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ColorChanged()
    {
        m_Material = GetComponentInChildren<Renderer>().sharedMaterial;
        Debug.Log(m_Material.color);
        m_Material.color = Color.green;
        Debug.Log(m_Material.color);

        // tr = GetComponentInChildren<TrailRenderer>();
        // tr.material = new Material(Shader.Find("Sprites/Default"));

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        //  float alpha = 1.0f;
        //  Gradient gradient = new Gradient();
        //  gradient.SetKeys(
        //     new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.green, 1.0f) },
        //      new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        //  );
        //  tr.colorGradient = gradient;
    }

    public void clear()
    {
        TrailRenderer[] children = gameObject.GetComponentsInChildren<TrailRenderer>();
        if (children != null && children.Length > 0)
        {
            foreach (TrailRenderer child in children)
            {
                Debug.Log(child.gameObject.name);
                child.Clear();
            }
        }
        else
        {
            Debug.Log("no Children");
        }

    }

    public void pause()
    {
        tr2 = GetComponentInChildren<TrailRenderer>();
        if (tr2.emitting == true)
        {
            tr2.emitting = false;
        }
        else
        {
            tr2.emitting = true;
        }
        Debug.Log(tr2.emitting);
    }

}
