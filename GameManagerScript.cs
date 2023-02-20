using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int photos = 0;
    public int maxPhotos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photos >= maxPhotos) {
            SceneManager.LoadScene("End Game");
        }
    }

    public void addPhoto() {
        photos += 1;
    }
}
