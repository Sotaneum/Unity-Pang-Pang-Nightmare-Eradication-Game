using UnityEngine;
using System.Collections;

public class Toggle_System : MonoBehaviour
{
    public GameObject Control;
    public GameObject Setting;
    public GameObject Play;
    public GameObject Exit;

    void Start()
    {
        Game_Start();
        Game_Exit();
        Game_Setting();
        SoundSet();
        LoadCheck();
    }

    //게임 시작
    void Game_Start()
    {
        Transform Ctl_Play = Control.transform.FindChild("Play");
        if (Play.activeSelf == false)
        {
            StartCoroutine(WaitMoving(-270f,180f, Ctl_Play,Play, false));
        }
        else
        {
            StartCoroutine(WaitMoving(-270f,0f, Ctl_Play, Play, true));
        }
    }

    //게임 설정 활성화
    void Game_Setting()
    {
        Transform Ctl_Setting = Control.transform.FindChild("Setting");
        if(Setting.activeSelf==false)
        {
            StartCoroutine(WaitMoving(0f,180f,Ctl_Setting,Setting,false));
        }
        else
        {
            StartCoroutine(WaitMoving(0f,0f, Ctl_Setting, Setting, true));
        }
    }
    //객체 움직임 설정
    IEnumerator WaitMoving(float tagetX,float tagetY, Transform tagetPosition,GameObject DisableTaget, bool min)
    {

        if (min == true)
        {
            while (tagetPosition.localPosition.y >= tagetY)
            {
                yield return new WaitForSeconds(0.001f);
                tagetPosition.Translate(Vector3.down * 2f * Time.deltaTime);
            }
            DisableTaget.SetActive(false);
            tagetPosition.localPosition = new Vector3(tagetX, tagetY, tagetPosition.position.z);
        }
        else
        {
            while (tagetPosition.localPosition.y <= tagetY)
            {
                yield return new WaitForSeconds(0.001f);
                tagetPosition.Translate(Vector3.up * 2f * Time.deltaTime);
            }
            DisableTaget.SetActive(true);
            tagetPosition.localPosition = new Vector3(tagetX, tagetY, tagetPosition.position.z);
        }
    }
    void GameExit()
    {
        Application.Quit();
    }
    void Game_Exit()
    {
        Transform Ctl_Exit = Control.transform.FindChild("Exit");
        if (Exit.activeSelf == false)
        {
            StartCoroutine(WaitMoving(270f, 90f, Ctl_Exit, Exit, false));
        }
        else
        {
            StartCoroutine(WaitMoving(270f, 0f, Ctl_Exit, Exit, true));
        }
    }

    void Game_Sound()
    {
        if (PlayerPrefs.GetString("Sound", "ON") == "OFF")
        {
            PlayerPrefs.SetString("Sound", "ON");
        }
        else
        {
            PlayerPrefs.SetString("Sound", "OFF");
        }
        SoundSet();
    }

    void SoundSet()
    {
        Transform Ctl_Mute=Setting.transform.FindChild("Mute");
        Transform Ctl_SoundOn = Setting.transform.FindChild("Sound On");

        if(PlayerPrefs.GetString("Sound","ON")=="OFF")
        {
            PlayerPrefs.SetString("Sound", "OFF");
            Ctl_Mute.gameObject.SetActive(true);
            Ctl_SoundOn.gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString("Sound", "ON");
            Ctl_Mute.gameObject.SetActive(false);
            Ctl_SoundOn.gameObject.SetActive(true);
        }
    }
    private bool pass=false;
    private bool connect = false;
    void Game_Reset()
    {
        if (PlayerPrefs.GetString("Load", "NULL") != "NULL")
        {
            Transform Ctl_Reset = Setting.transform.FindChild("Reset");
            if (connect == false)
                StartCoroutine(WaitProsess(Ctl_Reset));
            Game_Delete_Data();
            LoadCheck();
        }
    }
    IEnumerator WaitProsess(Transform tagetRotation)
    {
        connect = true;
        while(pass==false)
        {   
            yield return new WaitForSeconds(0.01f);
            tagetRotation.Rotate(0, 0, 10);
        }
        connect = false;
    }
    void Game_Delete_Data()
    {
        PlayerPrefs.SetString("Load", "NULL");
        PlayerPrefs.SetInt("hp", 0);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("Getscore", 0);
        PlayerPrefs.SetInt("losehp",0);
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("maxlevel", 0);
        PlayerPrefs.SetInt("maxtime", 0);
        PlayerPrefs.SetFloat("createspeed", 0);
        PlayerPrefs.SetFloat("deletespeed", 0);
        PlayerPrefs.SetString("NM1", "");
        PlayerPrefs.SetInt("ScoreIndex", 0);
        pass = true;
    }
    void LoadCheck()
    {
        if (PlayerPrefs.GetString("Load", "NULL") == "NULL")
        {
            Play.transform.FindChild("Load Game").gameObject.SetActive(false);
        }
    }
    void LoadGame ()
    {
        if (PlayerPrefs.GetString("Load", "NULL") == "NULL")
        {
            Play.transform.FindChild("Load Game").gameObject.SetActive(false);
        }
        else
        {
            Application.LoadLevel("PlayMode");
        }
    }

    void NewGame()
    {
        PlayerPrefs.SetString("Load", "NULL");
        Application.LoadLevel("PlayMode");
    }
}
