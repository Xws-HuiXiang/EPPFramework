using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPPTools.Localization
{
    /// <summary>
    /// 加载资源方式
    /// </summary>
    public enum LoadAssetsMethod
    {
        Resources,
        UnityWebRequest,
        StreamingAssets
    }

    public enum Language
    {
        /// <summary>
        /// 简体中文(中国)
        /// </summary>
        zh_cn,
        /// <summary>
        /// 繁体中文(台湾地区)
        /// </summary>
        zh_tw,
        /// <summary>
        /// 繁体中文(香港)
        /// </summary>
        zh_hk,
        /// <summary>
        /// 英语(香港)
        /// </summary>
        en_hk,
        /// <summary>
        /// 英语(美国)
        /// </summary>
        en_us,
        /// <summary>
        /// 英语(英国)
        /// </summary>
        en_gb,
        /// <summary>
        /// 英语(全球)
        /// </summary>
        en_ww,
        /// <summary>
        /// 英语(加拿大)
        /// </summary>
        en_ca,
        /// <summary>
        /// 英语(澳大利亚)
        /// </summary>
        en_au,
        /// <summary>
        /// 英语(爱尔兰)
        /// </summary>
        en_ie,
        /// <summary>
        /// 英语(芬兰)
        /// </summary>
        en_fi,
        /// <summary>
        /// 芬兰语(芬兰)
        /// </summary>
        fi_fi,
        /// <summary>
        /// 英语(丹麦)
        /// </summary>
        en_dk,
        /// <summary>
        /// 丹麦语(丹麦)
        /// </summary>
        da_dk,
        /// <summary>
        /// 英语(以色列)
        /// </summary>
        en_il,
        /// <summary>
        /// 希伯来语(以色列)
        /// </summary>
        he_il,
        /// <summary>
        /// 英语(南非)
        /// </summary>
        en_za,
        /// <summary>
        /// 英语(印度)
        /// </summary>
        en_in,
        /// <summary>
        /// 英语(挪威)
        /// </summary>
        en_no,
        /// <summary>
        /// 英语(新加坡)
        /// </summary>
        en_sg,
        /// <summary>
        /// 英语(新西兰)
        /// </summary>
        en_nz,
        /// <summary>
        /// 英语(印度尼西亚)
        /// </summary>
        en_id,
        /// <summary>
        /// 英语(菲律宾)
        /// </summary>
        en_ph,
        /// <summary>
        /// 英语(泰国)
        /// </summary>
        en_th,
        /// <summary>
        /// 英语(马来西亚)
        /// </summary>
        en_my,
        /// <summary>
        /// 英语(阿拉伯)
        /// </summary>
        en_xa,
        /// <summary>
        /// 韩文(韩国)
        /// </summary>
        ko_kr,
        /// <summary>
        /// 日语(日本)
        /// </summary>
        ja_jp,
        /// <summary>
        /// 荷兰语(荷兰)
        /// </summary>
        nl_nl,
        /// <summary>
        /// 荷兰语(比利时)
        /// </summary>
        nl_be,
        /// <summary>
        /// 葡萄牙语(葡萄牙)
        /// </summary>
        pt_pt,
        /// <summary>
        /// 葡萄牙语(巴西)
        /// </summary>
        pt_br,
        /// <summary>
        /// 法语(法国)
        /// </summary>
        fr_fr,
        /// <summary>
        /// 法语(卢森堡)
        /// </summary>
        fr_lu,
        /// <summary>
        /// 法语(瑞士)
        /// </summary>
        fr_ch,
        /// <summary>
        /// 法语(比利时)
        /// </summary>
        fr_be,
        /// <summary>
        /// 法语(加拿大)
        /// </summary>
        fr_ca,
        /// <summary>
        /// 西班牙语(拉丁美洲)
        /// </summary>
        es_la,
        /// <summary>
        /// 西班牙语(西班牙)
        /// </summary>
        es_es,
        /// <summary>
        /// 西班牙语(阿根廷)
        /// </summary>
        es_ar,
        /// <summary>
        /// 西班牙语(美国)
        /// </summary>
        es_us,
        /// <summary>
        /// 西班牙语(墨西哥)
        /// </summary>
        es_mx,
        /// <summary>
        /// 西班牙语(哥伦比亚)
        /// </summary>
        es_co,
        /// <summary>
        /// 西班牙语(波多黎各)
        /// </summary>
        es_pr,
        /// <summary>
        /// 德语(德国)
        /// </summary>
        de_de,
        /// <summary>
        /// 德语(奥地利)
        /// </summary>
        de_at,
        /// <summary>
        /// 德语(瑞士)
        /// </summary>
        de_ch,
        /// <summary>
        /// 俄语(俄罗斯)
        /// </summary>
        ru_ru,
        /// <summary>
        /// 意大利语(意大利)
        /// </summary>
        it_it,
        /// <summary>
        /// 希腊语(希腊)
        /// </summary>
        el_gr,
        /// <summary>
        /// 挪威语(挪威)
        /// </summary>
        no_no,
        /// <summary>
        /// 匈牙利语(匈牙利)
        /// </summary>
        hu_hu,
        /// <summary>
        /// 土耳其语(土耳其)
        /// </summary>
        tr_tr,
        /// <summary>
        /// 捷克语(捷克共和国)
        /// </summary>
        cs_cz,
        /// <summary>
        /// 斯洛文尼亚语
        /// </summary>
        sl_sl,
        /// <summary>
        /// 波兰语(波兰)
        /// </summary>
        pl_pl,
        /// <summary>
        /// 瑞典语(瑞典)
        /// </summary>
        sv_se,
        /// <summary>
        /// 西班牙语(智利)
        /// </summary>
        es_cl,
    }

    /// <summary>
    /// 值存储类型
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// 存储资源路径
        /// </summary>
        Path,
        /// <summary>
        /// 以序列化的字符串对象进行存储
        /// </summary>
        Serializable
    }
}
