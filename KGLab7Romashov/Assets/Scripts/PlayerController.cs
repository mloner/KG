using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timeText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private int count = 0;
    private float movementX;
    private float movementY;

    static DateTime time;
    static DateTime lastTime;
    private bool isPaused = false;
    private bool gameIsOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        setCountText();
        winTextObject.SetActive(false);

        time = new DateTime(2020,10,10,0,0,0);
        lastTime = DateTime.Now;
        float size = PlayerPrefs.GetInt("BallSize");
        if (size < 100)
            size = 100;
        size /= 100;

        rb.transform.localScale = new Vector3(size, size, size);
        
    }

    private void Update()
    {
        if (!this.isPaused && !this.gameIsOver) {
            DateTime newDt = DateTime.Now;
            double tdiff = newDt.Subtract(lastTime).TotalSeconds;
            if (tdiff > 1)
            {
                time = time.AddSeconds(tdiff);
                lastTime = newDt;
            }
            timeText.text = time.ToString("mm:ss");
        }
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void setCountText()
    {
        if(count == 0)
            countText.text = "Поднимай желтые кубики\nи не трогай красные таблетки!\nCount: " + count.ToString();
        else if(count > 0)
            countText.text = "Count: " + count.ToString();
        if (count >= 5 && !this.gameIsOver)
        {
            this.gameIsOver = true;
            winTextObject.SetActive(true);
            string str = "[" + DateTime.Now.ToString() + "]|" + time.ToString("mm:ss");
            PlayerPrefs.SetString("ScoreTime", str);
            PlayerPrefs.SetString("ScoreTable", PlayerPrefs.GetString("ScoreTable") + "\n" + str);
            SceneManager.LoadScene("Menu");
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            setCountText();
        }
        else if (other.gameObject.CompareTag("PickUpBad"))
        {
            other.gameObject.SetActive(false);
            count--;
            setCountText();
        }
    }
    
}