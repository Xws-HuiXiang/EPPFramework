using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GameObjectPathEditor : MonoBehaviour
{
    [MenuItem("Tools/复制选择的GameObject路径")]
    public static void GameObjectPathMenuClick()
    {
        if(Selection.activeTransform != null)
        {
            Transform parent = Selection.activeTransform.parent;
            string path = Selection.activeTransform.name;
            while (parent != null)
            {
                if(parent.name.EndsWith("panel", System.StringComparison.OrdinalIgnoreCase))
                {
                    //如果名称以panel结尾 说明达到了外层
                    break;
                }

                path = parent.name + "/" + path;

                parent = parent.parent;
            }

            GUIUtility.systemCopyBuffer = path;

            Debug.Log("拷贝路径：" + path);
        }
    }

    [MenuItem("Tools/拷贝panel物体变量定义")]
    public static void PanelGameObjectDefinition()
    {
        GameObjectPathMenuClick();
        string copyContent = GUIUtility.systemCopyBuffer;
        string varName = copyContent.Replace("/", "_");
        varName = varName[0].ToString().ToLower() + varName.Substring(1);
        copyContent = string.Format("this.{0} = panelTransform:Find('{1}');", varName, copyContent);

        GUIUtility.systemCopyBuffer = copyContent;

        Debug.Log("拷贝的内容：" + copyContent);
    }
}
