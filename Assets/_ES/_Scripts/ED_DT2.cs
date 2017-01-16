using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ED_DT2 : MonoBehaviour {

    public GameObject gobjEffect;
    public bool isAddToSelf = false;

	EN_Effect _m_eEffect;
	EN_Time _m_eTime = new EN_Time();

	// 添加绘制圆区域
	public float shieldArea = 5;

    [InitializeOnLoadMethod]
    static public void Test()
    {
        Debug.Log("= InitializeOnLoadMethod = test = ");
    }

    // OnUpdate Update
    public void OnUpdate()
    {
		if(_m_eEffect != null)
		{
			_m_eTime.DoUpdateTime ();
			_m_eEffect.DoUpdate(_m_eTime.DeltaTime);
			if (_m_eEffect.isEnd) {
				_m_eEffect.DoDestory ();
				_m_eEffect = null;
			}
			UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
		}
    }

    [ContextMenu("DoPlay")]
    public void DoPlay()
    {
        if (gobjEffect == null)
            return;
		Debug.Log("=== DoPlay ===");

		UnityEngine.GameObject gobj = GameObject.Instantiate(this.gobjEffect, Vector3.zero, Quaternion.identity) as GameObject;
		Transform trsfGobj = gobj.transform;
		if (isAddToSelf)
		{
			trsfGobj = transform;
		}

		trsfGobj.localPosition = Vector3.zero;
		trsfGobj.localEulerAngles = Vector3.zero;
		trsfGobj.localScale = Vector3.one;

		_m_eEffect = new EN_Effect(gobj,0);
		_m_eEffect.DoStart();
		_m_eEffect.SetScale(1);
    }
    
#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }
#endif
}
