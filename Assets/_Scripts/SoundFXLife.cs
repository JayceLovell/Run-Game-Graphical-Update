using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXLife : MonoBehaviour
{
    public float SoundLength
    {
        get
        {
            return _soundLength;
        }
        set
        {
            _soundLength = value;
        }
    }

    private float _soundLength;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteMe(SoundLength));
    }
    IEnumerator DeleteMe(float length)
    {
        yield return new WaitForSeconds(length);
        Destroy(this.gameObject);
    }

}