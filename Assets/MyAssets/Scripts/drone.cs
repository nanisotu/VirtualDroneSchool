using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drone : MonoBehaviour
{
    [Header("設定")]
    public float BaseHoveringPow;//ぽうっ！！　ホバリングに必要な力を最初に計算代入するための変数
    Rigidbody rb;
    public float Hovering;//基本的にbasehoveringを代入、増減で上昇下降
    public List<float> SpeedMode;//一つ以上入れないとエラーがはっせいいい
    public List<GameObject> Camera;
    public float Quality;
    public GameObject HomePoint;//プロトタイプで戻る場所を指定するための変数
    public Animator DroneAnimator;
    
    [Header("状態")]
    public bool TakeOffModeFlag;
    public int SpeedModeFlag = 0;
    public int CameraModeFlag = 0;
    float speed;
    [Header("入力")]
    public float LeftVinput;
    public float LeftHinput;
    public float RightVinput;
    public float RightHinput;
    [Header("機能制限")]
    public List<string> restriction;
    [Header("デバッグ用")]
    public bool backhomepoint = true;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        BaseHoveringPow = Quality * 9.81f;
        //最初のスピードを定義
        SpeedModeFlag = SpeedMode.Count / 2;
        speed = SpeedMode[SpeedModeFlag];
    }
    void FixedUpdate()
    {
        //何も操作が入力されていないときに初期位置(homepointに戻す)
        if (!Input.anyKey && backhomepoint)
        {
            //go back your home.
            transform.position = Vector3.MoveTowards(transform.position, HomePoint.transform.position, 0.1f);
        }
        //離陸モード変更
        if (Input.GetKeyDown(KeyCode.Space) && !restriction.Contains("離陸モード変更"))
        {
            StartCoroutine(DelayAction(0.5f, "離陸"));
        }
        //スピードモード変更
        if (Input.GetKeyDown(KeyCode.M) && !restriction.Contains("スピードモード変更"))
        {
            SpeedModeFlag = (SpeedModeFlag % SpeedMode.Count + 1) % SpeedMode.Count;
            speed = SpeedMode[SpeedModeFlag];
        }
        //カメラ変更
        if (Input.GetKeyDown(KeyCode.E) && !restriction.Contains("カメラ変更"))
        {
            CameraModeFlag = (CameraModeFlag % Camera.Count + 1) % Camera.Count;
        }
        //キーボード操作
        if (Input.GetKey(KeyCode.W) && !restriction.Contains("上昇"))
        {
            LeftVinput = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S) && !restriction.Contains("下降"))
        {
            LeftVinput = -1.0f;
        }
        else
        {
            LeftVinput = 0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            LeftHinput = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            LeftHinput = -1.0f;
        }
        else
        {
            LeftHinput = 0f;
        }
        if (Input.GetKey(KeyCode.I) && !restriction.Contains("前方移動"))
        {
            RightVinput = 1.0f;
        }
        else if (Input.GetKey(KeyCode.K) && !restriction.Contains("後方移動"))
        {
            RightVinput = -1.0f;
        }
        else
        {
            RightVinput = 0f;
        }
        if (Input.GetKey(KeyCode.L) && !restriction.Contains("右移動"))
        {
            RightHinput = 1.0f;
        }
        else if (Input.GetKey(KeyCode.J) && !restriction.Contains("左移動"))
        {
            RightHinput = -1.0f;
        }
        else
        {
            RightHinput = 0f;
        }
        //inputの値を参照して貴様に力を与えよう
        Hovering = BaseHoveringPow + LeftVinput * Quality * speed;
        rb.AddForce(new Vector3(0.0f, Hovering, 0.0f), ForceMode.Force);
        rb.AddForce(transform.forward * Quality * speed * -RightVinput, ForceMode.Force);
        rb.AddForce(transform.right * Quality * speed * -RightHinput, ForceMode.Force);
        if (!restriction.Contains("回転")) rb.AddTorque(new Vector3(0, LeftHinput * speed, 0), ForceMode.Acceleration);
    }
    void Update()
    {

    }
    
    private IEnumerator DelayAction(float n, string x)
    {
        yield return new WaitForSecondsRealtime(n);
        if (x == "離陸")
        {
            TakeOffModeFlag = !TakeOffModeFlag;
        }
    }
}
