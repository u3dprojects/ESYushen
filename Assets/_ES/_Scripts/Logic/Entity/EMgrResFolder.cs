using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 类名 : 编辑器模式下资源文件路径管理
/// 作者 : Canyon
/// 日期 : 2017-04-14 10:47
/// 功能 : 用于资源加载，路径寻得
/// </summary>
public static class EMgrResFolder{
	
	#region 资源 -  特效prefab地址

	static string PahtTxt_Effect = "Assets/EffectFolder.txt";
	static string DefFoder_Effect = "Assets\\PackResources\\Arts\\Effect\\Prefabs\\";
	static string strEffectFolder = "";
	static string[] _pathFoder_Effect = null;
	static string[] PathFolderEffects{
		get{
			if (string.IsNullOrEmpty (strEffectFolder)) {
				bool isExists = File.Exists(PahtTxt_Effect);
				if (isExists) {
					UnityEngine.TextAsset obj = UnityEditor.AssetDatabase.LoadAssetAtPath(PahtTxt_Effect, typeof(UnityEngine.TextAsset)) as UnityEngine.TextAsset;
					strEffectFolder = obj.text;
				} else {
					Debug.LogWarning ("路径path = [" + PahtTxt_Effect + "],不存在！！！");
				}
			}
			if (_pathFoder_Effect == null && !string.IsNullOrEmpty (strEffectFolder)) {
				_pathFoder_Effect = strEffectFolder.Split ("\r\n\t".ToCharArray (),System.StringSplitOptions.RemoveEmptyEntries);
			}
			return _pathFoder_Effect;
		}
	}

	static public string GetPathEffect(string objName){
		string path = "";
		_pathFoder_Effect = PathFolderEffects;
		bool isExists = false;
		if (_pathFoder_Effect != null) {
			foreach (var item in _pathFoder_Effect) {
				if (string.IsNullOrEmpty (item))
					continue;

				isExists = Directory.Exists(item);
				if (isExists) {
					path = item + objName+".prefab";
					isExists = File.Exists(path);
					if (isExists) {
						break;
					}
				}
			}
		}

		if (!isExists)
			path = DefFoder_Effect + objName+".prefab";

		return path;
	}

	#endregion

}
