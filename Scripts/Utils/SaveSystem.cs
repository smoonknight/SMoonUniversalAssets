using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace SMoonUniversalAsset
{
    public static class SaveSystem
    {
        private static readonly string encryptionKey = "SMoonSmoonClassr"; // 16, 24, atau 32 karakter sesuai AES
        private static readonly string settingFilename = "settings";
        private static string CreateFileName(int number) => $"playerdata_{number}.{extension}";
        private static readonly string checkpointFileName = "checkpoint.checkpointdata";
        private static readonly string extension = "data";

        public static bool TryGetFileNames(out List<string> fileNames)
        {
            var data = Directory.GetFiles(Application.persistentDataPath, $"*.{extension}");
            fileNames = data.Length > 0 ? data.ToList() : new List<string>();
            return fileNames.Count > 0;
        }

        public static string GetEncryptedJsonOfSaveData(PlayerStatus playerStatus)
        {
            SaveDataFormat saveData = new()
            {
                playerStatusJson = JsonUtility.ToJson(playerStatus),
                timestampString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                gameVersion = GameManager.Instance.GameVersion
            };

            string json = JsonUtility.ToJson(saveData);
            return Encrypt(json, encryptionKey);
        }

        public static void CreateNewSaveData(PlayerStatus playerStatus)
        {
            int lenght = Directory.GetFiles(Application.persistentDataPath, $"*.{extension}").Length;

            string fileName;
            while (true)
            {
                fileName = CreateFileName(lenght);
                string path = Path.Combine(Application.persistentDataPath, fileName);
                if (!File.Exists(path))
                {
                    Debug.Log(fileName);
                    break;
                }
                lenght += 1;
            }

            SaveData(playerStatus, fileName);
        }
        public static void SaveCheckpointData(PlayerStatus playerStatus) => SaveData(playerStatus, checkpointFileName, false);
        public static void SaveData(PlayerStatus playerStatus, string fileName, bool createCheckpoint = true)
        {
            string encryptedJson = GetEncryptedJsonOfSaveData(playerStatus);
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, encryptedJson);
            if (createCheckpoint)
            {
                string checkpointPath = Path.Combine(Application.persistentDataPath, checkpointFileName);
                File.Copy(path, checkpointPath, true);
            }
        }

        public static LoadDataStatus LoadCheckpointData(ref PlayerStatus playerStatus) => LoadData(ref playerStatus, checkpointFileName, false);

        public static LoadDataStatus LoadData(ref PlayerStatus playerStatus, string fileName, bool checkVersion)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(path))
            {
                string encryptedJson = File.ReadAllText(path);
                return LoadDataUsingEncryptedJson(ref playerStatus, encryptedJson, checkVersion);
            }
            else
            {
                Debug.LogWarning("Save file not found.");
                return LoadDataStatus.FAILED;
            }
        }

        public static LoadDataStatus LoadDataUsingEncryptedJson(ref PlayerStatus playerStatus, string encryptedJson, bool checkVersion)
        {
            SaveDataFormat saveData = GetSaveDataFormatUsingEncryptedJson<SaveDataFormat>(encryptedJson);

            if (checkVersion && saveData.gameVersion != GameManager.Instance.GameVersion)
            {
                return LoadDataStatus.WARNING;
            }

            JsonUtility.FromJsonOverwrite(saveData.playerStatusJson, playerStatus);

            return LoadDataStatus.OK;
        }

        public static bool TryGetCheckpointData(out SaveDataFormat saveDataFormat) => TryGetSaveDataFormatUsingFileName(checkpointFileName, out saveDataFormat);
        public static bool TryGetSaveDataFormatUsingFileName(string fileName, out SaveDataFormat saveDataFormat)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            bool isFileExist = File.Exists(path);
            if (isFileExist)
            {
                string encryptedJson = File.ReadAllText(path);
                saveDataFormat = GetSaveDataFormatUsingEncryptedJson<SaveDataFormat>(encryptedJson);
            }
            else
            {
                saveDataFormat = null;
            }

            return isFileExist;

        }

        public static void DeleteData(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static T GetSaveDataFormatUsingEncryptedJson<T>(string encryptedJson) where T : SaveDataFormatBase
        {
            string json = Decrypt(encryptedJson, encryptionKey);
            return JsonUtility.FromJson<T>(json);
        }

        public static PlayerSetting GetPlayerSettingUsingEncryptedJson(string encryptedJson)
        {
            string json = Decrypt(encryptedJson, encryptionKey);
            return JsonUtility.FromJson<PlayerSetting>(json);
        }

        public static PlayerSetting LoadPlayerSetting()
        {
            string path = Path.Combine(Application.persistentDataPath, settingFilename);

            PlayerSetting playerSetting;

            bool isFileExist = File.Exists(path);
            if (isFileExist)
            {
                string encryptedJson = File.ReadAllText(path);
                playerSetting = GetPlayerSettingUsingEncryptedJson(encryptedJson);
            }
            else
            {
                playerSetting = SettingManager.Instance.GetDefaultPlayerSetting();
            }

            return playerSetting;
        }

        public static void SavePlayerSetting(PlayerSetting playerSetting)
        {
            string encryptedJson = Encrypt(JsonUtility.ToJson(playerSetting), encryptionKey);
            string path = Path.Combine(Application.persistentDataPath, settingFilename);
            File.WriteAllText(path, encryptedJson);
        }

        private static string Encrypt(string plainText, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Default IV (all zeroes)
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return System.Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private static string Decrypt(string cipherText, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Default IV (all zeroes)
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(System.Convert.FromBase64String(cipherText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }

    public abstract class SaveDataFormatBase
    {
        public string playerStatusJson;
    }

    [System.Serializable]
    public class SaveDataFormat : SaveDataFormatBase
    {
        // public LocationType locationType;
        public string timestampString;
        public string gameVersion;
    }

    [System.Serializable]
    public struct PlayerSetting
    {
        public float soundSFXPercentage;
        public float soundBGMPercentage;
        public float soundVoicePercentage;
        public LanguageType languageType;
        public int qualitySettingsIndex;
        public int targetFrameRate;
        public bool hasFirstLaunchSetting;
    }

    public enum LoadDataStatus
    {
        OK,
        FAILED,
        WARNING
    }
}