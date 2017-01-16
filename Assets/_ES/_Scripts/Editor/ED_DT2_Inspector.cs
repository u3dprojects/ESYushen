using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ED_DT2),true)]
public class ED_DT2_Inspector : Editor {

    ED_DT2 entity;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Debug.Log(" = ED_DT2_Inspector = ");
        entity = target as ED_DT2;

        if (GUILayout.Button("PlayEffect"))
        {
            if (entity.gobjEffect == null)
            {
                EditorUtility.DisplayDialog("提示", "请选特效Prefab", "Okey");
                return;
            }
            entity.DoPlay();
        }
    }

	void OnSceneGUI( )
	{
		ED_DT2 t = target as ED_DT2;
		Handles.color = Color.blue;
		Handles.Label( t.transform.position + Vector3.up * 2,
			t.transform.position.ToString( ) + "\nShieldArea: " +
			t.shieldArea.ToString( ) );

		Handles.BeginGUI( );
		GUILayout.BeginArea( new Rect( Screen.width - 100, Screen.height - 80, 90, 50 ) );

		if( GUILayout.Button( "Reset Area" ) )
			t.shieldArea = 5;

		GUILayout.EndArea( );
		Handles.EndGUI( );

		Handles.color = new Color( 1, 1, 1, 0.2f );
		Handles.DrawSolidDisc( t.transform.position, t.transform.up, t.shieldArea );

		Handles.color = Color.white;
		t.shieldArea = Handles.ScaleValueHandle( t.shieldArea,
			t.transform.position + t.transform.forward * t.shieldArea,
			t.transform.rotation, 1, Handles.ConeCap, 1 );
	}
}
