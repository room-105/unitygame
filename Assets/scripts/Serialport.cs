using System.IO.Ports;
using UnityEngine;

public class Serialport : MonoBehaviour
{
    public string portName = "COM5"; // Arduinoのポート名
    public int baudRate = 115200; // ボーレート
    private SerialPort serialPort;
    public static string message;//受け取る文字列
    //文字列を変数に変換
    public static int joyX = 512;
    public static int joyY = 512;
    public static bool joySW = false;
    public static bool btnB = false;
    public static bool btnY = false;
    public static bool btnW = false;
    bool attacknow;//今の攻撃の状態
    bool prebottunB, prebottunY, prebottunW;//前回の状態を保存する
    bool prestate;
    public static bool getkeydown { get; private set; }

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();
    }

    void Update()
    {
        getkeydown = false;
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            message = serialPort.ReadLine();　//文字列で信号を受信　数値化するときは変換が必要
            ParseMessage(message);
            Debug.Log(message);
        }

    }

    void ParseMessage(string msg)
    {
        string[] p = msg.Split(',');
        joyX = int.Parse(p[0]);
        joyY = int.Parse(p[1]);
        joySW = p[2] == "0";//1ならtrueで受け取り、0ならfalseで受け取る
        btnB = p[3] == "0";
        btnY = p[4] == "0";
        btnW = p[5] == "0";
        

        attacknow = btnB || btnW || btnY;//現入力
        prestate = prebottunB || prebottunW || prebottunY;//前回の入力 ||は3つ度のボタンでも押した瞬間を実装するため
        if (attacknow && !prestate) 
        {
            getkeydown = true;
        };
        prebottunB = btnB;
        prebottunW = btnW;
        prebottunY = btnY;
    }

    //serialPort.IsOpenは接続されてるか否か
    //serialPortはポートが存在しているか
    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}