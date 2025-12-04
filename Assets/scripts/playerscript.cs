using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class playerscript : MonoBehaviour
{
    [Header("UI")]
    public Text timertext;
    public Text hptext;
    public Text speedtext;
    public Image attackfillimage;
    public Canvas canvas;

    [Header("stage")]
    public Camera camerasozai;
    public Camera camerahome;
    public Camera cameraboss;
    public static float syukaicount = 0;//周回数
    public static bool timermanager;
    public static bool bosstimermanager;
    bool bossmovementtimermanager;
    public float timer = 30f;//素材ステージの時間
    public float bosstimer = 20f;//ボス戦の時間
    public float bossmovementtimer = 2f;//ボス戦移動の内部タイマー
    public float damage = 5;//敵の接触ダメージ
    public float bossdamage = 10;//bossの接触ダメージ
    public float bossenemydamage = 3f;//ボス子分の接触ダメージ
    AudioSource a;

    [Header("attack")]
    public GameObject attack;//近接攻撃範囲のオブジェクト
    public GameObject bullet;
    public float bulletspeed = 10f;
    public float attackrrange = 90f;//必殺の扇形の大きさ
    public int bulletamount =5;//必殺の玉の数
    float attackdis;
    public static float attackcurrentcount;
    public float attackmaxcount = 40f;//40回当たったら必殺
    private SpriteRenderer attackRenderer;
    private PolygonCollider2D attackpol;
    public GameObject firesound;
    public GameObject specialfiresound;
    public GameObject attacksound;
    bool attacked;

    [Header("charactor")]
    public string namae;
    public GameObject front;
    public GameObject back;
    public GameObject left;
    public GameObject right;
    public GameObject normal;
    public float playerspeed = 5.0f;
    public float hp = 100;
    public float maxhp = 100;
    public float defence = 1;

    // Start is called before the first frame update
    /*キャラクターに付けること
      ・移動機能
     */
    void Start()
    {
        camerahome.enabled = true;
        camerasozai.enabled = false;
        cameraboss.enabled = false;
        attackRenderer = attack.GetComponent<SpriteRenderer>();
        attackpol = attack.GetComponent<PolygonCollider2D>();
        a = GetComponent<AudioSource>();
        // 初期状態は非表示＆当たり判定オフ
        attackRenderer.enabled = false;
        attackpol.enabled = false;
        a.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        hptext.text = hp.ToString();
        speedtext.text = playerspeed.ToString();
        hp = Mathf.Max(hp, 0);
        playerspeed = Mathf.Max(playerspeed, 0);
        //画像の切り替え
        if (Input.GetKeyDown("up")) 
        {
            namae = "up";
        }

        if (Input.GetKeyDown("down"))
        {
            namae = "down";
        }

        if (Input.GetKeyDown("left"))
        {
            namae = "left";
        }
        if (Input.GetKeyDown("right"))
        {
            namae ="right";
        }
        if (!Input.anyKey)
        {
            namae = "normal";
        }

        front.SetActive(false);
        back.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
        normal.SetActive(false);

        switch (namae) 
        {
            case "up":
                front.SetActive(true);
                break;
            case "down":
                back.SetActive(true);
                break;
            case "left":
                left.SetActive(true);
                break;
            case "right":
                right.SetActive(true);
                break;
            case "normal":
                normal.SetActive(true);
                break;
        }

        if (Input.GetKey("up") == true)
        {
            this.gameObject.transform.position += new Vector3(0, playerspeed * Time.deltaTime, 0);
            attack.transform.position = this.transform.position + new Vector3(0,4.5f,0);
            attack.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey("down") == true)
        {
            this.gameObject.transform.position -= new Vector3(0, playerspeed * Time.deltaTime, 0);
            attack.transform.position = this.transform.position + new Vector3(0, -4.5f, 0);
            attack.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (Input.GetKey("right") == true)
        {
            this.gameObject.transform.position += new Vector3(playerspeed * Time.deltaTime, 0, 0);
            attack.transform.position = this.transform.position + new Vector3(3.7f, 0, 0);
            attack.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        if (Input.GetKey("left") == true)
        {
            this.gameObject.transform.position -= new Vector3(playerspeed * Time.deltaTime, 0, 0);
            attack.transform.position = this.transform.position + new Vector3(-3.7f, 0, 0);
            attack.transform.rotation = Quaternion.Euler(0,0,90);
        }
        //キャラの移動はここまで
        //以下はキャラの攻撃
        if ((Input.GetKeyDown(KeyCode.Space)||Serialport.getkeydown )&& !camerahome.enabled) // 近接　スペース Bボタン
        {
            Attack1();//近接
        }

        if ((Input.GetKeyDown(KeyCode.RightShift)||Serialport.btnW) && !camerahome.enabled)//遠距離　右シフト　Wボタン
        {
            Attack2();
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)||Serialport.btnY)&&!camerahome.enabled)//必殺 左シフト Yボタン
        {
            if (attackcurrentcount >= attackmaxcount)
            {
                StartCoroutine(Attack3());
            }
            else
            {
                Debug.Log("まだ必殺は溜まっていないよ");    
            }
        }

        attackcurrentcount = Mathf.Min(attackcurrentcount,40);//40以上にならないようにする
        attackfillimage.fillAmount = attackcurrentcount / attackmaxcount;//技ゲージ
        
        if (timermanager)
        {
            timertext.text = timer.ToString("f1");//ステータス表示用
            timer -= Time.deltaTime;
            camerahome.enabled = false;
            camerasozai.enabled = true;
        }

        if (timer < 0 )//時間経過によるステージ移動
        {
            sozaiMove();
            sozaistagecount();
        }
        if (hp == 0 && timermanager)//倒されてステージ移動
        {
            sozaiMove();
            Debug.Log("倒されました");
        }

        if (bossmovementtimermanager)
        {
            bossmovementtimer -= Time.deltaTime;
            if (bossmovementtimer < 0)
            {
                Debug.Log("ボス戦！");
                bossmovementtimermanager = false;
                bosstimermanager = true;
                this.gameObject.transform.position = new Vector3(2, 70, 0);
                bossmovementtimer = 2f;
            }
        }

        if (bosstimermanager)
        {
            camerahome.enabled = false;
            cameraboss.enabled = true;
            bosstimer -= Time.deltaTime;
            timertext.text = bosstimer.ToString("f1");//ステータス表示用
        }
        if (bosstimer < 0 )//ホーム画面に戻った後に動かなくなる挙動が見られたが勝手に治ったので注意
        {
            bossMove();
        }
        if (hp ==0 && bosstimermanager)
        {
            bossMove();
            Debug.Log("倒されたよ");
        }

    }
    void sozaiMove()//素材ステージからホームへの移動
    {
        timermanager = false;
        this.gameObject.transform.position = new Vector3(147, 2, 0);
        camerasozai.enabled = false;
        camerahome.enabled = true;
        playerspeed = 10f;
        hp = 100;
        timer = 30f;
        Debug.Log(attackcurrentcount);
        attackcurrentcount = 0;
        a.volume = 0.9f;
    }
    public void bossMove()
    {
        bosstimermanager = false;
        this.gameObject.transform.position = new Vector3(147, 2, 0);
        cameraboss.enabled = false;
        camerahome.enabled = true;
        bosstimer = 10f;
        playerspeed = 10f;
        hp = 100;
        a.volume = 0.9f;
    }

    public void bosswin()
    {
        bosstimermanager = false;
        this.gameObject.transform.position = new Vector3(147, 2, 0);
        cameraboss.enabled = false;
        camerahome.enabled = true;
        bosstimer = 10f;
        playerspeed = 10f;
        hp = 100;
        a.volume = 0f;
        Invoke("endmove",5f);//5秒後にエンドシーン
    }

    public void SwitchbossCamera()
    {
        if (canvas != null && cameraboss != null)
        {
            canvas.worldCamera = cameraboss;
        }
        else
        {
            Debug.Log("Canvas か Camera が未設定です");
        }
    }

    public void SwitchsozaiCamera()
    {
        if (canvas != null && camerasozai != null)
        {
            canvas.worldCamera = camerasozai;
        }
        else
        {
            Debug.Log("Canvas か Camera が未設定です");
        }
    }
    void sozaistagecount() //周回数のカウント
    {
        syukaicount++;
        Debug.Log($"現在の周回数は{syukaicount}回です");
    }

    void endmove()
    {
        SceneManager.LoadScene("EndScene");
    }


    //攻撃
    void Attack1()//近接
    {
        attackpol.enabled = true;
        attackRenderer.enabled = true;
        GameObject attacksounds = Instantiate(attacksound) as GameObject;
        Destroy( attacksounds,0.5f);
        Invoke("endattack", 0.5f);
    }
    void endattack()
    {
        attackpol.enabled = false;
        attackRenderer.enabled = false;
    }

    void Attack2()//遠距離
    {
        GameObject bullets = Instantiate(bullet,attack.transform.position,attack.transform.rotation)as GameObject;
        Rigidbody2D rb = bullets.GetComponent<Rigidbody2D>();
        rb.velocity = attack.transform.up * bulletspeed;//赤扇の位置を取得して正面方向に発射transform.up
        GameObject firesounds = Instantiate(firesound) as GameObject;
        Destroy(firesounds, 1.5f);
    }

    IEnumerator Attack3()//必殺　5方向の角度を取得。その角度をラジアン角にしてsin,cosを用いてベクトルのx,y成分を取得。生成される位置は赤扇。射出方向もプレイヤーの向きにするため向きに応じて取得する角度を変更した
    {
        Debug.Log("必殺打つよ");
        attackcurrentcount = 0;
        for (int j = 0;j<3;j++) 
        {
            float startangle = -attackrrange / 2;
            Debug.Log(bulletamount);
            for (int i = 0; i < bulletamount; i++)
            {
                float angle = startangle + (attackrrange / (bulletamount - 1)) * i;
                Attack_3(angle);
            }
            GameObject specialfiresounds = Instantiate(specialfiresound)as GameObject;
            Destroy(specialfiresounds, 2f);
            yield return new WaitForSeconds(1f);
        }
    }

    void Attack_3(float angle)
    {
            Debug.Log("発射");
            GameObject bullets = Instantiate(bullet, attack.transform.position, attack.transform.rotation) as GameObject;
            Rigidbody2D rb = bullets.GetComponent<Rigidbody2D>();
            float correctedAngle = angle;
            switch (namae)
            {
                case "up":
                    correctedAngle += 90f;
                    break;
                case "down":
                    correctedAngle -= 90f;
                    break;
                case "left":
                    correctedAngle += 180f;
                    break;
                case "right":
                    correctedAngle += 0f;
                    break;
                case "normal":
                    correctedAngle += 0f;
                    break;
            }
            Vector2 direction = new Vector2(Mathf.Cos(Mathf.Deg2Rad * correctedAngle), Mathf.Sin(Mathf.Deg2Rad * correctedAngle));
            rb.velocity = direction * bulletspeed;
        
    }


    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "enemy")//敵とぶつかったとき
        {
            Debug.Log("敵に当たったよ");
            hp -= Mathf.Max(damage - defence, 0);//大きい方の値を返す関数       
        }
        if (col.gameObject.tag == "boss")//ボスとぶつかったとき
        {
            Debug.Log("ボスと当たったよ");
            hp -= Mathf.Max(bossdamage - defence, 0);//大きい方の値を返す関数       
        }
        if (col.gameObject.tag == "bossenemy")//ボスとぶつかったとき
        {
            Debug.Log("ボスの子分と当たったよ");
            hp -= Mathf.Max(bossenemydamage - defence, 0);//大きい方の値を返す関数       
        }
        if (col.gameObject.tag == "trash")
        {
            Debug.Log("壁に当たったよ");
        }

        if (col.gameObject.tag == "en")//素材集めステージに行くとき
        {
            Debug.Log("当たった");
            timermanager = true;
            this.gameObject.transform.position = new Vector3(4.4f, -2.1f, 0);
            a.volume = 0f;
            SwitchsozaiCamera();//UIの移動
        }

        if (col.gameObject.tag == "bossen")//ボス戦に行くとき
        {
            Debug.Log("そろそろ移動します！");
            bossmovementtimermanager = true;
            SwitchbossCamera();//UIの移動
            a.volume = 0f;
        }
        if (col.gameObject.tag == "toge")
        {
            Debug.Log("とげ");
            hp -= 5;
        }
        if (col.gameObject.tag == "tien")
        {
            Debug.Log("遅延");
            playerspeed -= 1f;
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "toge")
        {
            Debug.Log("とげ");
            hp -= 5;
        }
        if (col.gameObject.tag == "tien")
        {
            Debug.Log("遅延");
            playerspeed -= 1f;
        }
        if (col.gameObject.tag == "enemyattack")//赤扇にタグをつけてる
        {
            hp -= Mathf.Max(redscript.enemyattack - defence, 0);//赤扇のスクリプトのダメージ７//
            Debug.Log("近接ダメージ受けた");
        }
        if (col.gameObject.tag == "bossattack")//赤扇にタグをつけてる
        {
            hp -= Mathf.Max(bossscriptt.bossattackdamage - defence, 0);//赤扇のスクリプトのダメージ７//
            Debug.Log("神の裁き");
        }
        if (col.gameObject.tag == "bossenemyattack")//赤扇にタグをつけてる
        {
            hp -= Mathf.Max(5 - defence, 0);//赤扇のスクリプトのダメージ７//
            Debug.Log("子分の攻撃");
        }
    }
}

