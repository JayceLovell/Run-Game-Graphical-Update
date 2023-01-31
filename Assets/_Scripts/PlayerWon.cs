using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class PlayerWon : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI _Instructions;

    private GameController _gameController;

    public Transform PlayerDirection;


    void Start()
    {
        _gameController=GameObject.Find("GameController").GetComponent<GameController>();
    }
    // Update is called once per frame
    void FixedUpdate () {
        // need a variable to hold the location of our Raycast look
        RaycastHit hit;

        //if raycast hits an object then do somthing....
        if(Physics.Raycast (this.PlayerDirection.position, this.PlayerDirection.forward, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Car"))
            {
                _Instructions.gameObject.SetActive(true);
            }
            else
            {
                _Instructions.gameObject.SetActive(false);
            }
        }
        if (Input.GetButtonDown("Fire1"))
        { 

            // if raycast hits an object then do somthing....
            if(Physics.Raycast (this.PlayerDirection.position,this.PlayerDirection.forward, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Car"))
                {
                    _gameController.GameManager.IsGameWon = true;
                }
            }
        }
	}
}
