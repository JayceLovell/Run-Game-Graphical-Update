using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAssignment : MonoBehaviour
{
    public Shader Diffuse;
    public Shader Emissive;
    public Shader Shinny;
    public Shader ToonWrap;
    public Material cool;
    public Material warm;
    public Material grey;
    public Material material;

    [SerializeField]
    private GameObject[] _batteries;

    // Start is called before the first frame update
    void Start()
    {
        
        _batteries = GameObject.FindGameObjectsWithTag("Battery");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().enabled = !GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _batteries = GameObject.FindGameObjectsWithTag("Battery");
            foreach (GameObject battery in _batteries)
            {
                battery.GetComponent<Renderer>().material.shader = Diffuse;
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            _batteries = GameObject.FindGameObjectsWithTag("Battery");
            foreach (GameObject battery in _batteries)
            {
                battery.GetComponent<Renderer>().material.shader= Emissive;
            }    
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _batteries = GameObject.FindGameObjectsWithTag("Battery");
            foreach (GameObject battery in _batteries)
            {
                battery.GetComponent<Renderer>().material.shader = Shinny;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _batteries = GameObject.FindGameObjectsWithTag("Battery");
            foreach (GameObject battery in _batteries)
            {
                battery.GetComponent<Renderer>().material.shader = ToonWrap;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().m_renderMaterial = cool;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().m_renderMaterial = warm;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().m_renderMaterial = grey;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().enabled = !GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraLutScript>().enabled;
        }
    }
}
