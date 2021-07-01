TipText = {};
local this = TipText;

local tipTextPrefab = nil;

function this.ShowTipText(text)
    if(tipTextPrefab == nil) then
        tipTextPrefab = ResourcesManager.Instance:LoadPrefabFromAssetBundle("Common.ab", "TipTextPrefab")
    end

    local tipTextGO = UnityEngine.GameObject.Instantiate(tipTextPrefab);
    local tipTextRectTrans = tipTextGO:GetComponent("RectTransform");
    local tipTextText = tipTextGO.transform:Find("Text"):GetComponent("Text");
    tipTextGO.transform:SetParent(PanelManager.UpperLayerCanvasTrans);

    tipTextRectTrans.anchoredPosition = Vector2.zero;
    tipTextRectTrans.anchorMin = Vector2.New(0, 1);
    tipTextRectTrans.anchorMax = Vector2.New(1, 1);
    tipTextRectTrans.sizeDelta = Vector2.New(-600, 60);

    tipTextText.text = text;
    tipTextGO:SetActive(true);

    local canvasGroup = tipTextRectTrans:GetComponent("CanvasGroup");
    canvasGroup.alpha = 1;

    --从上到下的动画，中间停留两秒然后移动到下方屏幕外 中键停留位置为0,-400
    local middlePos = Vector3.New(0, -400, 0);
    local anchorPosTween = tipTextRectTrans:DOAnchorPos(middlePos, 0.4);
    anchorPosTween:SetEase(DG.Tweening.Ease.OutExpo);
    anchorPosTween:OnComplete(function()
        local bottomPos = Vector3.New(0, -UnityEngine.Screen.height + 240, 0)
        local anchorPosBottomTween = tipTextRectTrans:DOAnchorPos(bottomPos, 0.15);
        anchorPosBottomTween:SetEase(DG.Tweening.Ease.InBack);
        anchorPosBottomTween:SetDelay(1.5);
        anchorPosBottomTween:OnComplete(function()
            GameManager.Destroy(tipTextRectTrans.gameObject);
        end);
        --透明度动画
        local canvasGroupTween = canvasGroup:DOFade(0, 0.15);
        canvasGroupTween:SetDelay(1.5);
        canvasGroupTween:SetEase(DG.Tweening.Ease.InExpo);
    end);
end