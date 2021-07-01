﻿#define USING_DOTWEENING

using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEditor;
using UnityEngine.UI;

using BindType = ToLuaMenu.BindType;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Networking;
using UNOServer.Common;

public static class CustomSettings
{
    public static string saveDir = Application.dataPath + "/Source/Generate/";    
    public static string toluaBaseType = Application.dataPath + "/ToLua/BaseType/";
    public static string baseLuaDir = Application.dataPath + "/ToLua/Lua/";
    public static string injectionFilesPath = Application.dataPath + "/ToLua/Injection/";

    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
        typeof(UnityEngine.QualitySettings),
        typeof(UnityEngine.GL),
        typeof(UnityEngine.Graphics),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {        
        _DT(typeof(Action)),                
        _DT(typeof(UnityEngine.Events.UnityAction)),
        _DT(typeof(UnityEngine.Events.UnityAction<bool>)),
        _DT(typeof(UnityEngine.Events.UnityAction<float>)),
        _DT(typeof(UnityEngine.Events.UnityAction<int>)),
        _DT(typeof(UnityEngine.Events.UnityAction<Vector2>)),
        _DT(typeof(UnityEngine.Events.UnityAction<string>)),
        _DT(typeof(UnityEngine.Events.UnityAction<BaseEventData>)),
        _DT(typeof(System.Predicate<int>)),
        _DT(typeof(System.Action<int>)),
        _DT(typeof(System.Comparison<int>)),
        _DT(typeof(System.Func<int, int>)),

        //DOTween
        _DT(typeof(DG.Tweening.TweenCallback)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList =
    {                
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),
        //_GT(typeof(TestProtol)),
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),
        //_GT(typeof(KeyValuePair<int, TestAccount>)),
        //_GT(typeof(Dictionary<int, TestAccount>.KeyCollection)),
        //_GT(typeof(Dictionary<int, TestAccount>.ValueCollection)),
        //_GT(typeof(TestExport)),
        //_GT(typeof(TestExport.Space)),
        //-------------------------------------------------------------------        
                        
        _GT(typeof(LuaInjectionStation)),
        _GT(typeof(InjectType)),
        _GT(typeof(Debugger)).SetNameSpace(null),          

#if USING_DOTWEENING
        _GT(typeof(DG.Tweening.DOTween)),
        _GT(typeof(DG.Tweening.Tween)).SetBaseType(typeof(System.Object)).AddExtendType(typeof(DG.Tweening.TweenExtensions)),
        _GT(typeof(DG.Tweening.Sequence)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.Tweener)).AddExtendType(typeof(DG.Tweening.TweenSettingsExtensions)),
        _GT(typeof(DG.Tweening.LoopType)),
        _GT(typeof(DG.Tweening.PathMode)),
        _GT(typeof(DG.Tweening.PathType)),
        _GT(typeof(DG.Tweening.RotateMode)),
        _GT(typeof(DG.Tweening.Ease)),
        _GT(typeof(Component)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Transform)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Light)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Material)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Rigidbody)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(Camera)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(AudioSource)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(LineRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        //_GT(typeof(TrailRenderer)).AddExtendType(typeof(DG.Tweening.ShortcutExtensions)),
        _GT(typeof(RectTransform)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
        _GT(typeof(DG.Tweening.Core.ABSSequentiable)),
        _GT(typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>)),
        _GT(typeof(DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions>)),
        _GT(typeof(DG.Tweening.Core.TweenerCore<Color, Color, DG.Tweening.Plugins.Options.ColorOptions>)),
        _GT(typeof(DG.Tweening.Core.TweenerCore<float,float,DG.Tweening.Plugins.Options.FloatOptions>)),
#else
                                         
        _GT(typeof(Component)),
        _GT(typeof(Transform)),
        _GT(typeof(Material)),
        _GT(typeof(Light)),
        _GT(typeof(Rigidbody)),
        _GT(typeof(Camera)),
        _GT(typeof(AudioSource)),
        //_GT(typeof(LineRenderer))
        //_GT(typeof(TrailRenderer))
#endif
      
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(Application)),
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(Time)),        
        _GT(typeof(Texture)),
        _GT(typeof(Texture2D)).AddExtendType(typeof(ImageConversion)),
        _GT(typeof(Shader)),        
        _GT(typeof(Renderer)),
        //Unity5版本才生成WWW类
#if UNITY_5
        _GT(typeof(WWW)),
#else
        _GT(typeof(UnityWebRequest)),
#endif
        _GT(typeof(Screen)),        
        _GT(typeof(CameraClearFlags)),
        _GT(typeof(AudioClip)),        
        _GT(typeof(AssetBundle)),
        _GT(typeof(ParticleSystem)),
        _GT(typeof(AsyncOperation)).SetBaseType(typeof(System.Object)),        
        _GT(typeof(LightType)),
        _GT(typeof(SleepTimeout)),
#if UNITY_5_3_OR_NEWER && !UNITY_5_6_OR_NEWER
        _GT(typeof(UnityEngine.Experimental.Director.DirectorPlayer)),
#endif
        _GT(typeof(Animator)),
        _GT(typeof(Input)),
        _GT(typeof(KeyCode)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Space)),      
       
        //MeshRenderer生成的Warp包会报编译错误
        //_GT(typeof(MeshRenderer)),
#if !UNITY_5_4_OR_NEWER
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)), 
#endif

        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),        
        _GT(typeof(CharacterController)),
        _GT(typeof(CapsuleCollider)),
        
        _GT(typeof(Animation)),        
        _GT(typeof(AnimationClip)).SetBaseType(typeof(UnityEngine.Object)),        
        _GT(typeof(AnimationState)),
        _GT(typeof(AnimationBlendMode)),
        _GT(typeof(QueueMode)),  
        _GT(typeof(PlayMode)),
        _GT(typeof(WrapMode)),
        //_GT(typeof(LuaByteBuffer)),

        _GT(typeof(QualitySettings)),
        _GT(typeof(RenderSettings)),                                                   
        _GT(typeof(SkinWeights)),           
        _GT(typeof(RenderTexture)),
        _GT(typeof(Resources)),     
        _GT(typeof(LuaProfiler)),
        _GT(typeof(AudioBehaviour)),
        _GT(typeof(Rect)),

        _GT(typeof(UnityEngine.Random)),

        //UnityEngine.UI
        _GT(typeof(Text)),
        _GT(typeof(Image)),
        _GT(typeof(Button)),
        _GT(typeof(Toggle)),
        _GT(typeof(ToggleGroup)),
        _GT(typeof(Slider)),
        _GT(typeof(Scrollbar)),
        _GT(typeof(Dropdown)),
        _GT(typeof(InputField)),
        _GT(typeof(Canvas)),
        _GT(typeof(ScrollRect)),
        _GT(typeof(EventSystem)),
        _GT(typeof(CanvasScaler)),
        _GT(typeof(GraphicRaycaster)),
        _GT(typeof(HorizontalLayoutGroup)),
        _GT(typeof(VerticalLayoutGroup)),
        _GT(typeof(HorizontalOrVerticalLayoutGroup)),
        _GT(typeof(LayoutGroup)),
        _GT(typeof(GridLayoutGroup)),
        _GT(typeof(Mask)),
        _GT(typeof(ContentSizeFitter)),
        _GT(typeof(LayoutElement)),
        _GT(typeof(RectTransform.Edge)),
        //_GT(typeof(Quaternion)),
        _GT(typeof(Sprite)),
        _GT(typeof(CanvasGroup)).AddExtendType(typeof(DG.Tweening.DOTweenModuleUI)),
        _GT(typeof(CanvasRenderer)),
        _GT(typeof(LayoutRebuilder)),
        _GT(typeof(MaskableGraphic)),
        _GT(typeof(Graphic)),
        _GT(typeof(Selectable)),
        _GT(typeof(UIBehaviour)),
        _GT(typeof(RectTransformUtility)),

        //UnityEnigne.Events
        _GT(typeof(EventTrigger)),
        _GT(typeof(UnityEvent)),
        _GT(typeof(PointerEventData)),
        _GT(typeof(UnityEngine.EventSystems.EventTriggerType)),
        _GT(typeof(BaseEventData)),
        _GT(typeof(EventTrigger.Entry)),
        _GT(typeof(BaseRaycaster)),
        _GT(typeof(UnityEventBase)),
        _GT(typeof(AbstractEventData)),

        _GT(typeof(GUIUtility)),

        //添加自定义的类
        //Managers
        _GT(typeof(PanelManager)),
        _GT(typeof(ResourcesManager)),
        _GT(typeof(DownloadManager)),
        _GT(typeof(GameManager)),
        _GT(typeof(LoadingManager)),
        _GT(typeof(PanelManager.OpenPanelStruct)),
        _GT(typeof(NetworkManager)),
        _GT(typeof(AndroidManager)),
        _GT(typeof(AudioManager)),
        _GT(typeof(TextureManager)),
        _GT(typeof(HttpRequestManager)),
        _GT(typeof(MonoSingleton<ResourcesManager>)),
        _GT(typeof(MonoSingleton<DownloadManager>)),
        _GT(typeof(MonoSingleton<GameManager>)),
        _GT(typeof(MonoSingleton<LoadingManager>)),
        _GT(typeof(MonoSingleton<NetworkManager>)),
        _GT(typeof(MonoSingleton<AudioManager>)),
        _GT(typeof(Singleton<AndroidManager>)),
        _GT(typeof(Singleton<TextureManager>)),
        //Common
        _GT(typeof(ConfigData)),
        _GT(typeof(AppConst)),
        _GT(typeof(Ciphertext.AES)),
        _GT(typeof(Ciphertext.DHExchange)),
        //Utils
        _GT(typeof(FDebugger)),
        _GT(typeof(UIEventBinds)),
        _GT(typeof(Utils)),
        _GT(typeof(PanelNameUtil)),
        _GT(typeof(RegexUtil)),
        //Data
        _GT(typeof(HotfixVersionData)),
        _GT(typeof(PbFileListData)),

        //CommonProtocol
        _GT(typeof(MessageBuffer)),

        //EPPTools相关类
        _GT(typeof(EPPTools.SimpleStorage.Storage)),

        _GT(typeof(System.Tuple<int, int, int>)),

        //LitJson
        _GT(typeof(LitJson.JsonMapper)),
        _GT(typeof(LitJson.JsonData)).AddExtendType(typeof(JsonDataExtension)),

        //UnityAds广告
        _GT(typeof(UnityAds)),
        _GT(typeof(UnityEngine.Advertisements.Advertisement)),
        _GT(typeof(UnityEngine.Advertisements.ShowResult)),
    };

    public static List<Type> dynamicList = new List<Type>()
    {
        typeof(MeshRenderer),
#if !UNITY_5_4_OR_NEWER
        typeof(ParticleEmitter),
        typeof(ParticleRenderer),
        typeof(ParticleAnimator),
#endif

        typeof(BoxCollider),
        typeof(MeshCollider),
        typeof(SphereCollider),
        typeof(CharacterController),
        typeof(CapsuleCollider),

        typeof(Animation),
        typeof(AnimationClip),
        typeof(AnimationState),

        typeof(SkinWeights),
        typeof(RenderTexture),
        typeof(Rigidbody),
    };

    //重载函数，相同参数个数，相同位置out参数匹配出问题时, 需要强制匹配解决
    //使用方法参见例子14
    public static List<Type> outList = new List<Type>()
    {
        
    };
        
    //ngui优化，下面的类没有派生类，可以作为sealed class
    public static List<Type> sealedList = new List<Type>()
    {
        /*typeof(Transform),
        typeof(UIRoot),
        typeof(UICamera),
        typeof(UIViewport),
        typeof(UIPanel),
        typeof(UILabel),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UIFont),
        typeof(UITexture),
        typeof(UISprite),
        typeof(UIGrid),
        typeof(UITable),
        typeof(UIWrapGrid),
        typeof(UIInput),
        typeof(UIScrollView),
        typeof(UIEventListener),
        typeof(UIScrollBar),
        typeof(UICenterOnChild),
        typeof(UIScrollView),        
        typeof(UIButton),
        typeof(UITextList),
        typeof(UIPlayTween),
        typeof(UIDragScrollView),
        typeof(UISpriteAnimation),
        typeof(UIWrapContent),
        typeof(TweenWidth),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenRotation),
        typeof(TweenPosition),
        typeof(TweenScale),
        typeof(TweenHeight),
        typeof(TypewriterEffect),
        typeof(UIToggle),
        typeof(Localization),*/
    };

    public static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    public static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    


    [MenuItem("Lua/Attach Profiler", false, 151)]
    static void AttachProfiler()
    {
        if (!Application.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "请在运行时执行此功能", "确定");
            return;
        }

        LuaClient.Instance.AttachProfiler();
    }

    [MenuItem("Lua/Detach Profiler", false, 152)]
    static void DetachProfiler()
    {
        if (!Application.isPlaying)
        {            
            return;
        }

        LuaClient.Instance.DetachProfiler();
    }
}
