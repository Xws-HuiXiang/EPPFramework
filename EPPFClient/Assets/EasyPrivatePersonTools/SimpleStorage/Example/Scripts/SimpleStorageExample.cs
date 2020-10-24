using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EPPTools.SimpleStorage;
using EPPTools.Utils;

namespace EPPTools.Example.SimpleStorageExample
{
    public struct StorageFormat
    {
        public Color backgroundColor;
        public Color textColor;

        public override string ToString()
        {
            return backgroundColor.r + " " + backgroundColor.g + " " + backgroundColor.b + " " + backgroundColor.a + "|" + textColor.r + " " + textColor.g + " " + textColor.b + " " + textColor.a;
        }
    }

    public class SimpleStorageExample : MonoBehaviour
    {
        public InputField storagePathText;
        public InputField keyInputField;
        public InputField valueInputFiled;
        public Button addButton;
        public Button deleteButton;
        public Button loadButton;
        public Button saveButton;
        public Transform prefabParent;
        public Font textFont;

        private Outline nowSelectedItem;

        private Storage storageInfo;

        private void Start()
        {
            LoadButtonClick();
        }

        private void LoadFinoshedCallBack()
        {
            string[] keys = storageInfo.GetAllKeys();
            foreach (string key in keys)
            {
                string value = (string)storageInfo[key];
                string[] array = value.Split('|');
                Color backgroundColor = ColorUtils.TryGetColor(array[0]);
                Color textColor = ColorUtils.TryGetColor(array[1]);
                CreateItemPrefab(key, backgroundColor, key, textColor);
            }
        }

        public void AddButtonClick()
        {
            if (storageInfo != null)
            {
                if (keyInputField != null && !string.IsNullOrEmpty(keyInputField.text))
                {
                    //[id]=[backageground color]|[text color]
                    if (CheckValueFormat(out StorageFormat info))
                    {
                        storageInfo.SetValue(keyInputField.text, info);

                        keyInputField.text = "";
                    }
                }
                else
                {
                    Debug.LogError("要存储的键为空");
                }
            }
        }

        public void DeleteButtonClick()
        {
            if(nowSelectedItem != null)
            {
                storageInfo.DeleteKey(nowSelectedItem.transform.Find("Text").GetComponent<Text>().text);
                Destroy(nowSelectedItem.gameObject);
            }
        }

        public void LoadButtonClick()
        {
            if(storageInfo == null)
            {
                string fullPath = Application.dataPath + "/" + storagePathText.text;
                fullPath = fullPath.Replace("\\", "/");
                storageInfo = new Storage(fullPath, this, LoadFinoshedCallBack);
                storageInfo.SetAutoSave(false);
            }
            else
            {
                //有一个标题的物体 
                int childrenCount = prefabParent.childCount - 1;
                for(int i = 1; i <= childrenCount; i++)
                {
                    Destroy(prefabParent.GetChild(i).gameObject);
                }

                storageInfo.Reload();
            }
        }

        public void SaveButtonClick()
        {
            if (storageInfo != null)
            {
                storageInfo.Save();
            }
        }

        public void AutoSaveToggleClick(bool isOn)
        {
            if(storageInfo != null)
            {
                storageInfo.SetAutoSave(isOn);
            }
        }

        /// <summary>
        /// 创建文件内容展示的游戏物体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storageBgColor"></param>
        /// <param name="storageText"></param>
        /// <param name="storageTextColor"></param>
        private void CreateItemPrefab(string id, Color storageBgColor, string storageText, Color storageTextColor)
        {
            GameObject item = new GameObject(id);
            GameObject textItem = new GameObject("Text");
            RectTransform rectTrans = item.AddComponent<RectTransform>();
            RectTransform textRectTrans = textItem.AddComponent<RectTransform>();

            rectTrans.SetParent(prefabParent);
            textRectTrans.SetParent(item.transform);

            rectTrans.localEulerAngles = Vector3.zero;
            rectTrans.localScale = Vector3.one;
            rectTrans.pivot = new Vector2(0, 1);
            Image itemImage = item.AddComponent<Image>();
            itemImage.color = storageBgColor;
            Outline outline = item.AddComponent<Outline>();
            outline.effectDistance = new Vector2(5, -5);
            outline.enabled = false;
            EventTrigger trigger = item.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) =>
            {
                if(nowSelectedItem != null)
                {
                    nowSelectedItem.enabled = false;
                }
                Outline newOutLine = trigger.GetComponent<Outline>();
                newOutLine.enabled = true;
                nowSelectedItem = newOutLine;
            });
            trigger.triggers.Add(entry);

            textRectTrans.pivot = new Vector2(0.5f, 0.5f);
            textRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            textRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            textRectTrans.anchorMin = Vector2.zero;
            textRectTrans.anchorMax = Vector2.one;
            textRectTrans.localScale = Vector3.one;
            Text text = textItem.AddComponent<Text>();
            text.text = storageText;
            text.font = textFont;
            text.fontSize = 24;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = storageTextColor;
        }

        /// <summary>
        /// 检查值格式 backgroundColor|textColor
        /// </summary>
        /// <returns></returns>
        private bool CheckValueFormat(out StorageFormat storageContent)
        {
            storageContent = new StorageFormat();

            if(valueInputFiled != null && !string.IsNullOrEmpty(valueInputFiled.text))
            {
                string[] array = valueInputFiled.text.Split('|');
                if (array.Length == 2)
                {
                    Color backgroundColor = ColorUtils.TryGetColor(array[0]);
                    Color textColor = ColorUtils.TryGetColor(array[1]);

                    storageContent.backgroundColor = backgroundColor;
                    storageContent.textColor = textColor;

                    return true;
                }
            }

            return false;
        }
    }
}
