using UnityEngine;
using System.Collections;

public class Play_System : MonoBehaviour {
    public GameObject Touch;
    public Camera camera;
    public GameObject[] Nightmare;
    [System.Serializable]
    public class Oris
    {
        public int _Hp = 5;
        public int _Score = 0;
        public int _limitScore = 1;
        public int _GetScore = 50;
        public int _LoseHp = 1;
        public int _Level = 0;
        public int _MaxLevel = 5;
        public int _MaxTime = 90;
        public float _CreateSpeed = 1;
        public float _DeleteSpeed = 5;

        public bool _CreateMode = false;
        public bool _PauseMode = false;
    }
    public Oris Ori;
    [System.Serializable]
    public class States
    {
        public int _Hp = 5;
        public int _Score = 0;
        public int _limitScore = 1;
        public int _GetScore = 50;
        public int _LoseHp = 1;
        public int _Level = 0;
        public int _MaxLevel = 5;
        public int _MaxTime = 5400;
        public float _CreateSpeed = 1;
        public float _DeleteSpeed = 5;

        public bool _CreateMode = false;
        public bool _PauseMode = false;
    }
    public States State;

    [System.Serializable]
    public class GUIs
    {
        public GameObject Score;
        public GameObject Setting;
        public GameObject Time;
        public GameObject Setting_Setting;
        public GameObject Timer;
        public GameObject GameOver;
        public GameObject Toggle;
        public GameObject Life;
        public GameObject Level;
        public GameObject GameOverScore;
        public GameObject GameOverLife;
        public GameObject GameOverRealScore;

    }
    public GUIs GUI;
    private bool GameOvers=false;
    private bool pauseStop = false;
    private bool LevelDown = false;
    void GameOver()
    {
        DataReset();
        GameOvers = true;
        State._CreateMode = false;
        GameObject[] nm = GameObject.FindGameObjectsWithTag("Nightmare");
        for (int i = 0; i < nm.Length; i++)
        {
            nm[i].GetComponent<Touch_System>().Active = false;
        }
        GUI.GameOverLife.GetComponent<UILabel>().text = "Life : " + State._Hp.ToString("0");
        GUI.GameOverScore.GetComponent<UILabel>().text = "Score : " + State._Score.ToString("0");
        State._Score += 327 * State._Hp;
        GUI.GameOverRealScore.GetComponent<UILabel>().text = "Score : " + State._Score.ToString("0");
        GUI.GameOver.SetActive(true);
    }
    
    void DataReset()
    {
        PlayerPrefs.SetString("Load", "NULL");
        PlayerPrefs.SetInt("hp", 0);
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("Getscore", 0);
        PlayerPrefs.SetInt("losehp", 0);
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.SetInt("maxlevel", 0);
        PlayerPrefs.SetInt("maxtime", 0);
        PlayerPrefs.SetFloat("createspeed", 0);
        PlayerPrefs.SetFloat("deletespeed", 0);
        PlayerPrefs.SetString("NM1", "");
    }

    void SaveScore()
    {
        if (State._Score > 0)
        {
            int index = PlayerPrefs.GetInt("ScoreIndex", 0);
            if (index == 0)
            {
                PlayerPrefs.SetInt("ScoreIndex", 1);
            }
            else
            {
                index++;
                PlayerPrefs.SetInt("ScoreIndex", index);
            }
            string ScoreList = PlayerPrefs.GetString("ScoreList", "");
            if (ScoreList == "")
            {
                ScoreList += State._Score.ToString("0");
            }
            else
            {
                ScoreList += "," + State._Score.ToString("0");
            }
            PlayerPrefs.SetString("ScoreList", ScoreList);
        }
    }

    void OUT()
    {
        SaveScore();
        PlayerPrefs.SetString("Load", "NULL");
        Application.LoadLevel("IntroMode");
    }

    void Retry()
    {
        SaveScore();
        PlayerPrefs.SetString("Load", "NULL");
        Application.LoadLevel("PlayMode");
    }

    void Start()
    {
        if (GameOvers == false)
        {
            Reset();
            StartCoroutine(RandomCreate());
        }
    }
    private GameObject[] Life;
    void DrawLife()
    {
        if (GameOvers == false)
        {
            Life = new GameObject[Ori._Hp];
            for (int i = 0; i < Ori._Hp; i++)
            {
                var LIFE = Instantiate(GUI.Life, Vector3.zero, new Quaternion(0, 0, 0, 0)) as GameObject;
                LIFE.transform.parent = GUI.Toggle.transform;
                LIFE.transform.localScale = new Vector3(30, 30, 0);
                LIFE.transform.localPosition = new Vector3(-(Screen.width / 2) + 40 * (i + 1), Screen.height / 2 - 30, 0);
                Life[i] = LIFE;
                Life[i].SetActive(true);
            }
            for(int i=State._Hp;i<Ori._Hp;i++)
            {
                Life[i].SetActive(false);
            }
        }
    }

    void GetScore()
    {
        if (GameOvers == false)
        {
            State._Score += State._GetScore;
        }
    }

    void GameOUT()
    {
        DataSave();
        Application.LoadLevel("IntroMode");
    }

    void LoseLife()
    {
        if (GameOvers == false)
        {
            print(State._Hp - 1);
            Life[State._Hp - 1].SetActive(false);
            State._Hp--;
            if (State._Hp <= 0)
            {
                GameOver();
            }
            else
            {
                State._Level = 0;
            }
            GUI.Level.GetComponent<UILabel>().text = "Level : " + State._Level.ToString("0");
            State._limitScore = State._Score + ((2 * State._Level - 1) * 1000);
        }
    }

    void GameStarted()
    {
        if(pauseStop==false)
            StartCoroutine(Wait3());
    }

    IEnumerator Wait3()
    {
        GUI.Setting_Setting.SetActive(false);
        pauseStop = true;
        GUI.Timer.SetActive(true);
        yield return new WaitForSeconds(1f);
        GUI.Timer.GetComponent<UILabel>().text = "3";
        yield return new WaitForSeconds(1f);
        GUI.Timer.GetComponent<UILabel>().text = "2";
        yield return new WaitForSeconds(1f);
        GUI.Timer.GetComponent<UILabel>().text = "1";
        yield return new WaitForSeconds(1f);
        Recovery();
        GUI.Timer.GetComponent<UILabel>().text = "";
        State._CreateMode = true;
        StartCoroutine(timeOut());
        PlayerPrefs.SetString("pause", "false");
        pauseStop = false;
        
    }

    void Reset()
    {
        if (PlayerPrefs.GetString("Load", "NULL") != "NULL")
        {
            State._Hp = PlayerPrefs.GetInt("hp", Ori._Hp);
            State._Score = PlayerPrefs.GetInt("score", Ori._Score);
            State._GetScore = PlayerPrefs.GetInt("getscore", Ori._GetScore);
            State._LoseHp = PlayerPrefs.GetInt("losehp", Ori._LoseHp);
            State._Level = PlayerPrefs.GetInt("level", Ori._Level);
            State._MaxLevel = PlayerPrefs.GetInt("maxlevel", Ori._MaxLevel);
            State._MaxTime = PlayerPrefs.GetInt("maxtime", Ori._MaxTime);
            State._CreateSpeed = PlayerPrefs.GetFloat("createspeed", Ori._CreateSpeed);
            State._DeleteSpeed = PlayerPrefs.GetFloat("deletespeed", Ori._DeleteSpeed);
            GUI.Setting_Setting.SetActive(true);
            State._CreateMode = false;
            State._PauseMode = true;
            PlayerPrefs.SetString("pause", "true");
        }
        else
        {
            State._Hp = Ori._Hp;
            State._Score = Ori._Score;
            State._GetScore = Ori._GetScore;
            State._LoseHp = Ori._LoseHp;
            State._Level = Ori._Level;
            State._MaxLevel = Ori._MaxLevel;
            State._MaxTime = Ori._MaxTime;
            State._CreateSpeed = Ori._CreateSpeed;
            State._DeleteSpeed = Ori._DeleteSpeed;
            GUI.Setting_Setting.SetActive(false);
            State._CreateMode = false;
            State._PauseMode = false;
            PlayerPrefs.SetString("pause", "false");
            GameStarted();
        }
        DrawLife();
        GUI.Time.transform.localPosition = new Vector3(0, (Screen.height / 2)-40f, 0);
        GUI.Score.transform.localPosition = new Vector3(0, (Screen.height / 2) - 80f, 0);
        GUI.Setting.transform.localPosition = new Vector3((Screen.width/2)-30, (Screen.height / 2) - 40f, 0);
        GUI.Level.transform.localPosition = new Vector3(-(Screen.width / 2) + 120, (Screen.height / 2) - 80f, 0);
        StartCoroutine(Save());
        StartCoroutine(UpdateLevel());
        LevelUpdete();
        GUI.Level.GetComponent<UILabel>().text = "Level : " + State._Level.ToString("0");
    }
    private bool WalkingRandomCreate=false;
   IEnumerator RandomCreate()
    {
        if (WalkingRandomCreate == false)
        {
            while (GameOvers == false)
            {
                WalkingRandomCreate = true;
                int height = Screen.height;
                int width = Screen.width;
                yield return new WaitForSeconds(0.1f);
                while (State._CreateMode == false) yield return new WaitForSeconds(0.1f);
                yield return new WaitForSeconds(State._CreateSpeed);
                for (int i = 0; i < ((State._Level + 1) / 3) + 1; i++)
                {
                    if (State._PauseMode == false)
                    {
                        GameObject NewNightmare = CreateNightmare(Touch.transform,
                            Vector3.zero,
                                new Quaternion(0, 0, 0, 0),
                                new Vector3(100, 100, 1),
                                Nightmare[Random.Range(0, Nightmare.Length)],
                                State._DeleteSpeed);
                        NewNightmare.transform.localPosition = new Vector3(Random.Range(-(width / 2) + 50, (width / 2) - 50),
                                Random.Range(-(height / 2) + 60, (height / 2) - 150), 0f);
                        NewNightmare.GetComponent<Touch_System>().Active = true;
                    }
                }
            }
        }
        
    }
    void PauseMode()
   {
       if (GameOvers == false && pauseStop==false)
       {
           if (PlayerPrefs.GetString("pause", "true") == "true")
           {
               GameStarted();
           }
           else
           {
               PlayerPrefs.SetString("pause", "true");
               State._CreateMode = false;
           }
       }
   }
   void Update()
   {
       if (GameOvers == false)
       {
           if (PlayerPrefs.GetString("pause", "true") == "true" && pauseStop==false)
           {
               GameObject[] nm = GameObject.FindGameObjectsWithTag("Nightmare");
               for (int i = 0; i < nm.Length; i++)
               {
                   nm[i].GetComponent<Touch_System>().Active = false;
               }
               State._PauseMode = true;
               GUI.Setting_Setting.SetActive(true);

           }
           else if (pauseStop == false)
           {
               GameObject[] nm = GameObject.FindGameObjectsWithTag("Nightmare");
               for (int i = 0; i < nm.Length; i++)
               {
                   nm[i].GetComponent<Touch_System>().Active = true;
               }
               State._PauseMode = false;
               
           }
           GUI.Score.GetComponent<UILabel>().text = "Score : " + State._Score.ToString("0");
       }
   }
   private bool WalkingTimeOut = false;
   IEnumerator timeOut()
   {
       if (WalkingTimeOut == false)
       {
           while (GameOvers == false)
           {
               WalkingTimeOut = true;
               yield return new WaitForSeconds(1f);
               while (pauseStop == true || State._PauseMode == true || State._CreateMode == false) yield return new WaitForSeconds(0.01f);
               State._MaxTime--;
               GUI.Time.GetComponent<UILabel>().text = (State._MaxTime / 60).ToString("0") + "분" + (State._MaxTime % 60).ToString("0") + "초";
               if (State._MaxTime <= 0)
               {
                   GameOver();
               }
           }
       }
   }
    GameObject CreateNightmare(Transform parent, Vector3 Position , Quaternion Rotation, Vector3 Scale, GameObject prefab, float deleteSpeed)
    {
        var Nightmare = Instantiate(prefab, Position, Rotation) as GameObject;
        Nightmare.transform.parent = parent;
        Nightmare.transform.localScale = Scale;
        Nightmare.GetComponent<Touch_System>()._deleteSpeed = deleteSpeed;
        Nightmare.GetComponent<Touch_System>().camera = camera;
        Nightmare.GetComponent<Touch_System>().GM = gameObject;
        return Nightmare;
    }
    //데이터 자동 저장

    private bool WalkingSave = false;
    IEnumerator Save()
    {
        if (WalkingSave == false)
        {
            while (true)
            {
                WalkingSave = true;
                yield return new WaitForSeconds(0.1f);
                while (State._PauseMode == true || State._CreateMode == false)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                while (State._PauseMode == false && State._CreateMode == true)
                {
                    yield return new WaitForSeconds(1f);
                    DataSave();
                }
            }
        }
    }

    void DataSave()
    {
        if (GameOvers == false)
        {
            if (pauseStop == false)
            {
                print("저장됨");
                PlayerPrefs.SetString("Load", "yes");
                PlayerPrefs.SetInt("hp", State._Hp);
                PlayerPrefs.SetInt("score", State._Score);
                PlayerPrefs.SetInt("Getscore", State._GetScore);
                PlayerPrefs.SetInt("losehp", State._LoseHp);
                PlayerPrefs.SetInt("level", State._Level);
                PlayerPrefs.SetInt("maxlevel", State._MaxLevel);
                PlayerPrefs.SetInt("maxtime", State._MaxTime);
                PlayerPrefs.SetFloat("createspeed", State._CreateSpeed);
                PlayerPrefs.SetFloat("deletespeed", State._DeleteSpeed);
                GameObject[] NM = GameObject.FindGameObjectsWithTag("Nightmare");
                PlayerPrefs.SetString("NM1", "");
                for (int i = 0; i < NM.Length; i++)
                {
                    PlayerPrefs.SetString("NM1",
                        PlayerPrefs.GetString("NM1", "") + NM[i].transform.localPosition.x.ToString("0") +
                        "," + NM[i].transform.localPosition.y.ToString("0") + "," +
                        NM[i].transform.localPosition.z.ToString("0") + "," +
                        (NM[i].GetComponent<Touch_System>()._deleteSpeed - NM[i].GetComponent<Touch_System>().time).ToString("0") + ",");
                }
            }
        }
    }
    
    void Recovery()
    {
        if (GameObject.FindGameObjectsWithTag("Nightmare").Length == 0)
        {
            string NM1data = PlayerPrefs.GetString("NM1", "");
            if (NM1data != "")
            {
                string[] data = NM1data.Split(',');
                for (int i = 0; i < data.Length - 1; i += 4)
                {
                    GameObject NewNightmare = CreateNightmare(Touch.transform,
                                   Vector3.zero,
                                       new Quaternion(0, 0, 0, 0),
                                       new Vector3(100, 100, 1),
                                       Nightmare[0],
                                       System.Int32.Parse(data[i + 3]));
                    NewNightmare.transform.localPosition = new Vector3(System.Int32.Parse(data[i]), System.Int32.Parse(data[i + 1]), System.Int32.Parse(data[i + 2]));
                    NewNightmare.GetComponent<Touch_System>().Active = true;
                }
            }
        }
    }

    void LevelUpdete()
    {
        if(State._limitScore<=State._Score)
        {
            State._Level++;
            State._limitScore = State._Score + ((2 * State._Level - 1) * 1000);
            if (State._Level >= State._MaxLevel)
                State._Level = State._MaxLevel;
            GUI.Level.GetComponent<UILabel>().text = "Level : " + State._Level.ToString("0");
        }
        if(LevelDown==false)
        {
            State._GetScore = Ori._GetScore + ((2 * State._Level - 1) * 15);
            State._CreateSpeed = Ori._CreateSpeed - ((State._Level+1)*0.125f);
            if(State._CreateSpeed<0.3f)
            {
                State._CreateSpeed = 0.3f;
            }
            State._DeleteSpeed = Ori._DeleteSpeed - ((State._Level + 1) * 0.12f);
            if (State._DeleteSpeed < 1f)
            {
                State._DeleteSpeed = 1f;
            }
        }
    }
    private bool WalkingUpdateLevel=false;
    IEnumerator UpdateLevel()
    {
        if (WalkingUpdateLevel == false)
        {
            while (GameOvers == false)
            {
                WalkingUpdateLevel = true;
                yield return new WaitForSeconds(0.1f);
                while (State._PauseMode == false)
                {
                    yield return new WaitForSeconds(0.1f);
                    LevelUpdete();
                }
            }
        }
    }
}
