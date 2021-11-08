using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPhysics : MonoBehaviour
{
    #region variables
    [SerializeField] private Transform buttonTop;
    [SerializeField] private Transform buttonLowerLimit;
    [SerializeField] private Transform buttonUpperLimit;

    [SerializeField] private float threshold;
    [SerializeField] private float springForce = 10;
    [SerializeField] private bool isPressed;
    [SerializeField] private AudioSource pressedSound;

    private float upperLowerDiff = .5f;
    private Material redButtonMaterial;
    private Material greenButtonMaterial;
    
    #endregion

    public UnityEvent onPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        redButtonMaterial = buttonTop.GetComponent<MeshRenderer>().materials[0];
        greenButtonMaterial = buttonTop.GetComponent<MeshRenderer>().materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        buttonTop.transform.localPosition = new Vector3(0, buttonTop.transform.localPosition.y,0 );

        if (buttonTop.localPosition.y >= 0)
        {
            buttonTop.transform.position = new Vector3(buttonUpperLimit.position.x, buttonUpperLimit.position.y, buttonUpperLimit.position.z);
        }
        else{
            buttonTop.GetComponent<Rigidbody>().AddForce(buttonTop.transform.up * springForce * Time.fixedDeltaTime);
        }

        if (buttonTop.localPosition.y <= buttonLowerLimit.localPosition.y)
        {
            buttonTop.transform.position = new Vector3(buttonLowerLimit.position.x,buttonLowerLimit.position.y, buttonLowerLimit.position.z);
        }



        if (Vector3.Distance(buttonTop.position, buttonLowerLimit.position) < upperLowerDiff * threshold)
        {
            isPressed = true;
            pressedSound.Play();
            Pressed();
        }
        else
        {
            isPressed = false;
            buttonTop.GetComponent<MeshRenderer>().material = redButtonMaterial;
        }

    }

    void Pressed()
    {
        buttonTop.GetComponent<MeshRenderer>().material = greenButtonMaterial;
        
        NetworkManager.ConnectToServer();
        //this.gameObject.SetActive(false);
    }
}
