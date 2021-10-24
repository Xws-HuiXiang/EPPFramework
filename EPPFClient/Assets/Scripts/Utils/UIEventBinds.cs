using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI添加事件绑定
/// </summary>
public class UIEventBinds
{
    /// <summary>
    /// 按钮添加点击事件
    /// </summary>
    /// <param name="buttonTransform">Button组件所在物体的Transform</param>
    /// <param name="action">要绑定的事件</param>
    /// <returns></returns>
    public static bool ButtonAddOnClick(Transform buttonTransform, UnityAction action)
    {
        if(buttonTransform != null)
        {
            if(buttonTransform.TryGetComponent<Button>(out Button button))
            {
                button.onClick.AddListener(action);

                return true;
            }
            FDebugger.LogErrorFormat("物体{0}上不存在Button组件", buttonTransform.name);
        }
        else
        {
            FDebugger.LogErrorFormat("增加按钮点击事件的对象为空");
        }

        return false;
    }

    /// <summary>
    /// 按钮移除点击事件
    /// </summary>
    /// <param name="buttonTransform">Button组件所在物体的Transform</param>
    /// <param name="action">要解绑的事件</param>
    /// <returns></returns>
    public static bool ButtonRemoveOnClick(Transform buttonTransform, UnityAction action)
    {
        if (buttonTransform.TryGetComponent<Button>(out Button button))
        {
            button.onClick.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Button组件", buttonTransform.name);

        return false;
    }

    /// <summary>
    /// 添加toggle的值改变事件
    /// </summary>
    /// <param name="toggleTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ToggleAddValueChange(Transform toggleTransform, UnityAction<bool> action)
    {
        if (toggleTransform.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Toggle组件", toggleTransform.name);

        return false;
    }

    /// <summary>
    /// 移除toggle的值改变事件
    /// </summary>
    /// <param name="toggleTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ToggleRemoveValueChange(Transform toggleTransform, UnityAction<bool> action)
    {
        if (toggleTransform.TryGetComponent<Toggle>(out Toggle toggle))
        {
            toggle.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Toggle组件", toggleTransform.name);

        return false;
    }

    /// <summary>
    /// 给物体添加EventTrigger事件
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="eventType"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static bool EventTriggerAddEvent(Transform trans, int eventType, UnityAction<BaseEventData> callback)
    {
        return EventTriggerAddEvent(trans, (EventTriggerType)eventType, callback);
    }

    /// <summary>
    /// 给物体添加EventTrigger事件
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="eventType"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static bool EventTriggerAddEvent(Transform trans, EventTriggerType eventType, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = null;
        EventTrigger trigger = trans.GetComponent<EventTrigger>();

        if (trigger != null)
        {
            //已有EventTrigger，查找是否已经存在要注册的事件
            foreach (EventTrigger.Entry existingEntry in trigger.triggers)
            {
                if (existingEntry.eventID == eventType)
                {
                    entry = existingEntry;
                    break;
                }
            }
        }
        else
        {
            //添加新的EventTrigger
            trigger = trans.gameObject.AddComponent<EventTrigger>();
        }

        //如果这个事件不存在，就创建新的实例
        if (entry == null)
        {
            entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback = new EventTrigger.TriggerEvent();
        }

        //添加触发回调并注册事件
        entry.callback.AddListener(callback);
        trigger.triggers.Add(entry);

        return true;
    }

    /// <summary>
    /// 添加滑动条的值改变事件
    /// </summary>
    /// <param name="sliderTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool SliderAddValueChange(Transform sliderTransform, UnityAction<float> action)
    {
        if (sliderTransform.TryGetComponent<Slider>(out Slider slider))
        {
            slider.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Slider组件", sliderTransform.name);

        return false;
    }

    /// <summary>
    /// 移除滑动条的值改变事件
    /// </summary>
    /// <param name="sliderTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool SliderRemoveValueChange(Transform sliderTransform, UnityAction<float> action)
    {
        if (sliderTransform.TryGetComponent<Slider>(out Slider slider))
        {
            slider.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Slider组件", sliderTransform.name);

        return false;
    }

    /// <summary>
    /// 增加滚动条的值改变事件
    /// </summary>
    /// <param name="scrollbarTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ScrollbarAddValueChange(Transform scrollbarTransform, UnityAction<float> action)
    {
        if(scrollbarTransform.TryGetComponent<Scrollbar>(out Scrollbar scrollbar))
        {
            scrollbar.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Scrollbar组件", scrollbarTransform.name);

        return false;
    }

    /// <summary>
    /// 移除滚动条的值改变事件
    /// </summary>
    /// <param name="scrollbarTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ScrollbarRemoveValueChange(Transform scrollbarTransform, UnityAction<float> action)
    {
        if(scrollbarTransform.TryGetComponent<Scrollbar>(out Scrollbar scrollbar))
        {
            scrollbar.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Scrollbar组件", scrollbarTransform.name);

        return false;
    }

    /// <summary>
    /// 添加输入框的值改变事件
    /// </summary>
    /// <param name="inputFieldTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool InputFieldAddValueChange(Transform inputFieldTransform, UnityAction<string> action)
    {
        if(inputFieldTransform.TryGetComponent<InputField>(out InputField inputField))
        {
            inputField.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在InputField组件", inputFieldTransform.name);

        return false;
    }

    /// <summary>
    /// 移除输入框的值改变事件
    /// </summary>
    /// <param name="inputFieldTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool InputFieldRemoveValueChange(Transform inputFieldTransform, UnityAction<string> action)
    {
        if (inputFieldTransform.TryGetComponent<InputField>(out InputField inputField))
        {
            inputField.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在InputField组件", inputFieldTransform.name);

        return false;
    }

    /// <summary>
    /// 添加输入框输入结束事件
    /// </summary>
    /// <param name="inputFieldTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool InputFieldAddEndEdit(Transform inputFieldTransform, UnityAction<string> action)
    {
        if (inputFieldTransform.TryGetComponent<InputField>(out InputField inputField))
        {
            inputField.onEndEdit.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在InputField组件", inputFieldTransform.name);

        return false;
    }

    /// <summary>
    /// 移除输入框输入结束事件
    /// </summary>
    /// <param name="inputFieldTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool InputFieldRemoveEndEdit(Transform inputFieldTransform, UnityAction<string> action)
    {
        if (inputFieldTransform.TryGetComponent<InputField>(out InputField inputField))
        {
            inputField.onEndEdit.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在InputField组件", inputFieldTransform.name);

        return false;
    }

    /// <summary>
    /// 添加滚动范围的值改变事件
    /// </summary>
    /// <param name="scrollRectTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ScrollRectAddValueChange(Transform scrollRectTransform, UnityAction<Vector2> action)
    {
        if(scrollRectTransform.TryGetComponent<ScrollRect>(out ScrollRect scrollRect))
        {
            scrollRect.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在ScrollRect组件", scrollRectTransform.name);

        return false;
    }

    /// <summary>
    /// 移除滚动范围的值改变事件
    /// </summary>
    /// <param name="scrollRectTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool ScrollViewRemoveValueChange(Transform scrollRectTransform, UnityAction<Vector2> action)
    {
        if (scrollRectTransform.TryGetComponent<ScrollRect>(out ScrollRect scrollRect))
        {
            scrollRect.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在ScrollRect组件", scrollRectTransform.name);

        return false;
    }

    /// <summary>
    /// 添加下拉列表值改变事件
    /// </summary>
    /// <param name="dropdownTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool DropdownAddValueChange(Transform dropdownTransform, UnityAction<int> action)
    {
        if(dropdownTransform.TryGetComponent<Dropdown>(out Dropdown dropdown))
        {
            dropdown.onValueChanged.AddListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Dropdown组件", dropdownTransform.name);

        return false;
    }

    /// <summary>
    /// 移除下拉列表值改变事件
    /// </summary>
    /// <param name="dropdownTransform"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool DropdownRemoveValueChange(Transform dropdownTransform, UnityAction<int> action)
    {
        if (dropdownTransform.TryGetComponent<Dropdown>(out Dropdown dropdown))
        {
            dropdown.onValueChanged.RemoveListener(action);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Dropdown组件", dropdownTransform.name);

        return false;
    }

    /// <summary>
    /// 给下拉列表的选项中增加一项
    /// </summary>
    /// <param name="dropdownTransform"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public static bool DropdownAddOption(Transform dropdownTransform, Dropdown.OptionData option)
    {
        if (dropdownTransform.TryGetComponent<Dropdown>(out Dropdown dropdown))
        {
            dropdown.options.Add(option);

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Dropdown组件", dropdownTransform.name);

        return false;
    }

    /// <summary>
    /// 设置下拉列表的选项
    /// </summary>
    /// <param name="dropdownTransform"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool DropdownSetOptions(Transform dropdownTransform, List<Dropdown.OptionData> options)
    {
        if (dropdownTransform.TryGetComponent<Dropdown>(out Dropdown dropdown))
        {
            dropdown.options = options;

            return true;
        }
        FDebugger.LogErrorFormat("物体{0}上不存在Dropdown组件", dropdownTransform.name);

        return false;
    }
}
