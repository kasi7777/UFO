using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UFO : MonoBehaviour
{
    public Rigidbody leftLegRigidBody;
    public Rigidbody rightLegRigidBody;

    public SceneLoader sceneLoader;

    public GameObject bodyRotate;
    public float speed = 20;

    public Slider rightSlider;
    public Slider leftSlider;

    public Text rightText;
    public Text leftText;

    public GameObject startCanvas;
    public GameObject winCanvas;

    public float rotationMultiPlayer = 0.7f;

    public Slider speedForce;


    void Start()
    {
        rightSlider.maxValue = speed;
        leftSlider.maxValue = speed;

        sceneLoader = FindObjectOfType<SceneLoader>();

        startCanvas.SetActive(false);
        winCanvas.SetActive(false);
    }

    void Update()
    {
        Vector3 minForce = Vector3.up * speed * rotationMultiPlayer;
        Vector3 maxForce = Vector3.up * speed;

        Vector3 leftForce = Vector3.zero;
        Vector3 rightForce = Vector3.zero;

        if (Input.GetKey(KeyCode.Escape) && startCanvas.activeSelf == false)
            startCanvas.SetActive(true);

        if (Input.GetKey(KeyCode.V))
            speed = 10;
        if (Input.GetKey(KeyCode.C))
            speed = 20;

        if (Input.GetKey(KeyCode.A) && startCanvas.activeSelf == false)
        {
            leftLegRigidBody.AddRelativeForce(Vector3.up * speed * rotationMultiPlayer * Time.deltaTime); // (0, 1, 0)
            rightLegRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime);

            leftForce = minForce;
            rightForce = maxForce;
        }

        else if (Input.GetKey(KeyCode.D) && startCanvas.activeSelf == false)
        {
            leftLegRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime); 
            rightLegRigidBody.AddRelativeForce(Vector3.up * speed * rotationMultiPlayer * Time.deltaTime);

            leftForce = maxForce;
            rightForce = minForce;
        }

        else if (Input.GetKey(KeyCode.Space) && startCanvas.activeSelf == false)
        {
            leftLegRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime);
            rightLegRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime);

            leftForce = maxForce;
            rightForce = maxForce;
        }

        leftLegRigidBody.AddRelativeForce(leftForce);
        rightLegRigidBody.AddRelativeForce(rightForce);

        leftSlider.value = leftForce.y;
        rightSlider.value = rightForce.y;

        rightText.text = rightForce.y + " KWt";
        leftText.text = leftForce.y + " KWt";

        bodyRotate.transform.Rotate(0, 50 * Time.deltaTime, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "FinishTeleport")
        {
            winCanvas.SetActive(true);
        }

        if (collision.gameObject.tag == "Enemy" && winCanvas.activeSelf == false)
        {
            sceneLoader.RestartScene();
        }

        if (collision.gameObject.tag == "Friend")
        {
            sceneLoader.NextScene();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            sceneLoader.RestartScene();
        }
    }
}
