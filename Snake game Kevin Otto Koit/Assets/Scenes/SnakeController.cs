using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    // Settings
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 10;
    public int score;
    public AudioSource sound;
    public GameObject soundeffect;
    public Text scoreText;


    // References
    public GameObject BodyPrefab;

    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        sound=soundeffect.GetComponent<AudioSource>();
        scoreText.GetComponent<Text>();
        scoreText.text = "My score is "+score;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Move forward
        transform.position += transform.forward * MoveSpeed * Time.fixedDeltaTime;

        // Steer
        float steerDirection = Input.GetAxis("Horizontal"); // Returns value -1, 0, or 1
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.fixedDeltaTime);

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.fixedDeltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }
    }

    private void GrowSnake()
    {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "food")
        {
            Destroy(other.gameObject, 0.2f);
            sound.Play();
            score++;
            scoreText.text = "My score is " + score;

        }

        if( other.gameObject.tag == "danger")
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    /* void OnTriggerEnter(Collider other)
     {
         if (other.gameObject.tag == "food")
             other.gameObject.SetActive(false);
        // Debug.Log("You got a collectible!"); //player need to be tagged as Player and collectible as Collectible
         if (other.gameObject.tag == "danger")
         {
             Application.LoadLevel(Application.loadedLevel);
         }
     }*/


}
