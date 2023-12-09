using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideScript : MonoBehaviour
{

    public GameObject guidePanel;
    public StartingData startingData;

    public GameObject[] listPanel;
    public int indexList;

    public bool active;
    // Start is called before the first frame update
    void Awake()
    {
        if (!startingData.isStarting)
        {
            ResetStatus.instance.ResetData();
            guidePanel.SetActive(true);
            startingData.isStarting = true;
            GameManager.instance.popupActive = true;
            firstGuide();
        }
        else{
            guidePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    public void firstGuide()
    {
        for(int i = 0; i < listPanel.Length; i++)
        {
            listPanel[i].SetActive(false);
        }

        listPanel[0].SetActive(true);


        UIManager.instance.ShowHideUIPanel(false);
    }
}
