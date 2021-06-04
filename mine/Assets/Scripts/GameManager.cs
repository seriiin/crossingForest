using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject MainCamera;

    public Player player;
    public int distance;
    public int gems;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text gemsTxt;

    public Transform[] animalZones;
    public GameObject[] animals;
    public List<int> animalList;

    public GameObject overPanel;
    public Text curScoreText;
    public Text bestText;

    public GameObject cat;

    public bool isStart;

    public AudioSource menuSound;
    

    void Awake()
    {
        maxScoreTxt.text = string.Format("{0:n0}",PlayerPrefs.GetInt("MaxScore")) + "m";
        menuSound.Play();
    }

    public void GameStart()
    {
        isStart = true;
        StartCoroutine(startAnimal());
        //Debug.Log("?");
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
        MainCamera.transform.position = new Vector3(0, 3.6f, -7.6f);
    }

    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if (player.distance > maxScore){
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MaxScore", player.distance);
        }
    }

    void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0}",player.distance) + "m";
        gemsTxt.text = string.Format("{0:n0}",player.gems);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator startAnimal()
    {
        for(int i = 0; i<animalZones.Length;i++)
        {
            StartCoroutine(plusAnimals(i));
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator plusAnimals(int k)
    {
        int ran = Random.Range(0, 5);

        for (int i = 0;i<15;i++)
        {
            GameObject animal = Instantiate(animals[ran], animalZones[k].position, animalZones[k].rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
