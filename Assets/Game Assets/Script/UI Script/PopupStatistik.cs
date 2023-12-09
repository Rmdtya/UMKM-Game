using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupStatistik : MonoBehaviour
{
    public Transform box;
    public CanvasGroup background;
    public Transform worldCanvas;

    public PanelPelanggan[] panelPelanggan;

    private void OnEnable()
    {
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);

        box.localPosition = new Vector2(0, -Screen.height);
        box.LeanMoveLocalY(-1459.262f, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1795f, 0.5f).setEaseOutExpo().delay = 0.1f;

        InitialObject();
    }

    public void InitialObject()
    {
        int standActive = GameManager.instance.GetStandActive();
        double popularityPoint = UserStatus.instance.GetPopularity();

        int levelStand = GameManager.instance.GetStandScriptActive().GetLevelStand();

        for (int i = 0; i < panelPelanggan.Length; i++)
        {
            panelPelanggan[i].SetPanel(standActive - 1, levelStand, popularityPoint);
        }
    }

    private void OnDisable()
    {
        background.LeanAlpha(0, 0.5f);
        box.LeanMoveLocalY(-Screen.height, 0.5f).setEaseOutExpo().delay = 0.1f;

        worldCanvas.LeanMoveLocalY(1095.285f, 0.5f).setEaseOutExpo();
    }
}
