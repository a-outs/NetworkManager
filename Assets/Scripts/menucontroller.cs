using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menucontroller : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip clickClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton() {
        audioSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("Game");
    }
}
