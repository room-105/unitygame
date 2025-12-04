using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class explainSceneManager : MonoBehaviour
{
    public GameObject bgm;//このオブジェクトのclipを変えたら後ろのbgmもなくなってしまうので音を生成する
    bool scene = false;
    AudioSource au;
    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !scene)
        {
            GameObject bgms = Instantiate(bgm) as GameObject;
            Destroy(bgms, 2.9f);
            Invoke("load", 3f);
            scene = true;
        }
    }
    void load()
    {
        SceneManager.LoadScene("main");
    }
}
