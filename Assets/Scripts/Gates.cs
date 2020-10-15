using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gates : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI textMeshPro;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        time = 10;
    }

    // Update is called once per frame
    void Update()
    {        

        if (time < 0)
        {
            textMeshPro.text = "GO";
            animator.enabled = true;
        }
        else
        {
            time -= Time.deltaTime;
            int seconds = (int)time;
            textMeshPro.text = seconds.ToString();

        }
    }
}
