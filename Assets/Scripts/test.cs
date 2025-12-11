using System.Text;
using UnityEngine;

public class test : MonoBehaviour {
    public static uint Random1(uint seed)
    {
        seed = seed * 1103515245U + 12345U;
        return seed;
    }

    public static uint SdbmHash(string str)
    {
        uint num = 0U;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        foreach (byte b in bytes)
        {
            num = (uint)b + (num << 6) + (num << 16) - num;
        }
        return num;
    }

    public static string GetNintendo3DSInternetKey()
    {
        string str = Application.companyName.Trim();
        string str2 = Application.productName.Trim();
        uint seed = SdbmHash(str + "::" + str2);
        string key = "0x" + Random1(seed).ToString("X4");


        return key;
    }
    void Start () {
        Debug.Log(GetNintendo3DSInternetKey());
	}
}
