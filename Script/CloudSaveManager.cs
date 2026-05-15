using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CloudSaveManager : MonoBehaviour
{
    // डेटा चोरी रोकने के लिए एक सीक्रेट एन्क्रिप्शन की (Encryption Key)
    private private string encryptionKey = "BGBattleRoyaleKey"; 

    [System.Serializable]
    public class PlayerProfileData
    {
        public string playerID;
        public int level;
        public int exp;
        public int coins;
        public int gems;
    }

    private void Awake()
    {
        if (GameServiceLocator.Instance != null)
        {
            GameServiceLocator.Instance.RegisterService<CloudSaveManager>(this);
        }
    }

    // डेटा को एन्क्रिप्ट करके सुरक्षित सेव करने का मेथड
    public void SavePlayerDataSecurely(PlayerProfileData data)
    {
        string json = JsonUtility.ToJson(data);
        string encryptedJson = EncryptString(json, encryptionKey);

        // लोकल डिवाइस पर सेव करें (पर यह एन्क्रिप्टेड है, इसलिए हैकर इसे एडिट नहीं कर पाएगा)
        PlayerPrefs.SetString("SecureProfile", encryptedJson);
        PlayerPrefs.Save();

        // 🚀 बैकएंड इंटीग्रेशन नोट:
        // यहाँ आप अपने API सर्वर को कॉल कर सकते हैं:
        // StartCoroutine(UploadToDatabaseServer(encryptedJson));
        Debug.Log("[CloudSave] प्लेयर डेटा सुरक्षित रूप से एन्क्रिप्ट और सेव कर दिया गया है।");
    }

    public PlayerProfileData LoadPlayerDataSecurely()
    {
        if (!PlayerPrefs.HasKey("SecureProfile"))
        {
            return new PlayerProfileData { level = 1, exp = 0, coins = 1000, gems = 0 }; // नया प्लेयर डेटा
        }

        string encryptedJson = PlayerPrefs.GetString("SecureProfile");
        string decryptedJson = DecryptString(encryptedJson, encryptionKey);

        return JsonUtility.FromJson<PlayerProfileData>(decryptedJson);
    }

    // ---- AES एन्क्रिप्शन एल्गोरिदम (Data Security) ----
    private string EncryptString(string plainText, string key)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    array = memoryStream.ToArray();
                }
            }
        }
        return Convert.ToBase64String(array);
    }

    private string DecryptString(string cipherText, string key)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
