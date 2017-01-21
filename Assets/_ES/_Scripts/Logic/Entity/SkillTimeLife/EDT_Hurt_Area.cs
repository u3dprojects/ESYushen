using UnityEngine;
using System.Collections;
using System.ComponentModel;
using LitJson;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 类名 : 伤害区域 hit Area
/// 作者 : Canyon
/// 日期 : 2017-01-13 10:30
/// 功能 : 
/// </summary>
public class EDT_Hurt_Area : EDT_Base{
	
	public enum HurtAreaType{
		[Description("无")]
		None = 0,

		[Description("圆形")]
		Circle = 1,

		[Description("弧形")]
		Arc = 2,

		[Description("矩形")]
		Rectangle = 3,
	}

	// 类型
	public HurtAreaType m_emType = HurtAreaType.None;

	// 相对偏移(相对于人物的位置偏移的位置) 就是产生点的位置
	public Vector3 m_v3Offset = Vector3.zero;

	// 伤害范围,最远的一边(圆，弧表示半径,矩形就是长边)
	public float m_fRange;

	// 矩形宽
	public float m_fWidth;

	// 逆时针旋转角度偏移量,就是y轴值
	public float m_fRotation;

	// 角度[0~360] 360表示圆,0就不绘制
	public float m_fAngle;

	// 区域颜色
	public Color m_cAreaColor = new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f),0.15f);

	// 是否绘制伤害区域
	public bool m_isShowArea;

	// 绘制对象
	bool _m_isDrawRuning = false;
	GameObject m_gobjDraw;

	public EDT_Hurt_Area() : base(){
		m_emType = HurtAreaType.None;
	}

	public override void OnReInit (float castTime, JsonData jsonData)
	{
		base.OnReInit (castTime, jsonData);

		int tpId = (int)jsonData ["m_id"];
		this.m_emType = (HurtAreaType)tpId;

		float x = float.Parse(jsonData ["m_offsetX"].ToString());
		float z = float.Parse(jsonData ["m_offsetZ"].ToString());
		this.m_v3Offset = new Vector3 (x, 0, z);

		IDictionary dicJsonData = (IDictionary)jsonData;

		if (dicJsonData.Contains("m_r")) {
			this.m_fRange = float.Parse (jsonData ["m_r"].ToString ());
		}

		if (dicJsonData.Contains("m_len")) {
			this.m_fRange = float.Parse (jsonData ["m_len"].ToString ());
		}

		if (dicJsonData.Contains("m_rad")) {
			this.m_fAngle = float.Parse (jsonData ["m_rad"].ToString ());
		}

		if (dicJsonData.Contains("m_rot")) {
			this.m_fRotation = float.Parse (jsonData ["m_rot"].ToString ());
		}

		if (dicJsonData.Contains("m_width")) {
			this.m_fWidth = float.Parse (jsonData ["m_width"].ToString ());
		}

		this.m_isJsonDataToSelfSuccessed = true;
		this.m_isInitedFab = true;
	}

	public override JsonData ToJsonData (){
		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return null;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return null;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0 || this.m_fAngle > 360) {
				return null;
			}
		}

		JsonData ret = new JsonData ();
		ret ["m_id"] = (int)this.m_emType;
		ret ["m_offsetX"] = Round2D(m_v3Offset.x,2);
		ret ["m_offsetZ"] = Round2D(m_v3Offset.z,2);

		switch (m_emType) {
		case HurtAreaType.Arc:
			ret ["m_r"] = Round2D(m_fRange,2);
			ret ["m_rad"] = Round2D(m_fAngle,2);
			ret ["m_rot"] = Round2D(m_fRotation,2);
			break;
		case HurtAreaType.Circle:
			ret ["m_r"] = Round2D(m_fRange,2);
			break;
		case HurtAreaType.Rectangle:
			ret ["m_len"] = Round2D(m_fRange,2);
			ret ["m_width"] = Round2D(m_fWidth,2);
			ret ["m_rot"] = Round2D(m_fRotation,2);
			break;
		}
		return ret;
	}

	public override void OnClear ()
	{
		base.OnClear ();

		_OnClearDrawGobj ();
	}

	void _OnClearDrawGobj(){
		if (m_gobjDraw != null) {
			m_gobjDraw.SetActive (false);
			GameObject.DestroyImmediate (m_gobjDraw);
			m_gobjDraw = null;
		}
	}

	public override void OnSceneGUI (Transform trsfOrg)
	{
		base.OnSceneGUI (trsfOrg);

		DrawAreaInSceneView (trsfOrg);

		_DrawArea (trsfOrg);
	}

	// 在区域里面绘制
	void DrawAreaInSceneView(Transform trsfOrg){
		if (!m_isShowArea) {
			return;
		}

		#if UNITY_EDITOR
		if(trsfOrg == null){
			return;
		}

		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0 || this.m_fAngle > 360) {
				return;
			}
		}

		Handles.color = this.m_cAreaColor;

		Vector3 posOrg = trsfOrg.position;
		Vector3 dirOrg = trsfOrg.forward;


		Vector3 pos = posOrg + this.m_v3Offset;
		pos.y = posOrg.y;

		Quaternion quaternion = Quaternion.AngleAxis(m_fRotation,Vector3.up);
		Vector3 dir = (quaternion * dirOrg).normalized;

		switch (m_emType) {
		case HurtAreaType.Arc:
			dir = (Quaternion.AngleAxis(-(m_fAngle / 2),Vector3.up) * dir).normalized;
			Handles.DrawSolidArc(pos,Vector3.up,dir,this.m_fAngle,this.m_fRange);
			break;
		case HurtAreaType.Circle:
			 Handles.DrawSolidDisc(pos,Vector3.up,this.m_fRange);
			break;
		case HurtAreaType.Rectangle:
			float hfw = this.m_fWidth / 2;
			float hfl = this.m_fRange / 2;
			float hfr = Mathf.Sqrt(Mathf.Pow(this.m_fWidth,2)+Mathf.Pow(this.m_fRange,2));
			pos = posOrg;

			Vector3 pos01 = new Vector3(pos.x - hfw,0,pos.z - hfl);
			Vector3 pos1 = quaternion * pos01.normalized * hfr + this.m_v3Offset;
			pos1.y = pos.y;

			Vector3 pos02 = new Vector3(pos.x - hfw,0,pos.z + hfl);
			Vector3 pos2 = quaternion * pos02.normalized * hfr + this.m_v3Offset;
			pos2.y = pos.y;

			Vector3 pos03 = new Vector3(pos.x + hfw,0,pos.z + hfl);
			Vector3 pos3 = quaternion * pos03.normalized * hfr + this.m_v3Offset;
			pos3.y = pos.y;

			Vector3 pos04 = new Vector3(pos.x + hfw,0,pos.z - hfl);
			Vector3 pos4 = quaternion * pos04.normalized * hfr + this.m_v3Offset;
			pos4.y = pos.y;

			Vector3[] verts = new Vector3[] { 
				pos1,pos2,pos3,pos4
			};
			Handles.DrawSolidRectangleWithOutline(verts,this.m_cAreaColor, new Color( 0, 0, 0, 1 ));
			break;
		}
		Handles.color = Color.white;
		#endif
	}

	void _DrawArea(Transform trsfOrg){
		if (!_m_isDrawRuning) {
			return;
		}
		_m_isDrawRuning = true;
		if (m_gobjDraw != null) {
			m_isEnd = true;
			return;
		}

		if(trsfOrg == null){
			return;
		}

		if (this.m_fRange <= 0 || m_emType == HurtAreaType.None) {
			return;
		}

		if (m_emType == HurtAreaType.Rectangle) {
			if (this.m_fWidth <= 0) {
				return;
			}
		} else if (m_emType == HurtAreaType.Arc) {
			if (this.m_fAngle <= 0 || this.m_fAngle > 360) {
				return;
			}
		}

		Vector3 posOrg = trsfOrg.position;
		Vector3 dirOrg = trsfOrg.forward;


		Vector3 pos = posOrg + this.m_v3Offset;
		pos.y = posOrg.y;

		Quaternion quaternion = Quaternion.AngleAxis(m_fRotation,Vector3.up);
		Vector3 dir = (quaternion * dirOrg).normalized;

		Mesh mesh = null;
		switch (m_emType) {
		case HurtAreaType.Arc:
			dir = (Quaternion.AngleAxis (-(m_fAngle / 2), Vector3.up) * dir).normalized;
			mesh = MeshCreate.CreateArc (m_fRange, 36, -(m_fAngle / 2), m_fAngle);
			break;
		case HurtAreaType.Circle:
			mesh = MeshCreate.CreateCircle(m_fRange, 36);
			break;
		case HurtAreaType.Rectangle:
			dir = (Quaternion.AngleAxis (-(m_fAngle / 2), Vector3.up) * dir).normalized;
			mesh = MeshCreate.CreateQuad (m_fRange, m_fWidth);
			break;
		}

		m_gobjDraw = new GameObject (this.m_emType.ToString());

		m_gobjDraw.AddComponent<MeshFilter> ().mesh = mesh;

		Material mat = new Material (Shader.Find ("Diffuse"));
		m_gobjDraw.AddComponent<MeshRenderer> ().material = mat;

		m_gobjDraw.transform.position = pos;
		m_gobjDraw.transform.forward = dir;
	}

}

public static class MeshCreate{
	/// <summary>
	/// Creates the arc mesh
	/// </summary>
	/// <returns>The arc mesh </returns>
	/// <param name="radius">Radius.扇形半径</param>
	/// <param name="segments">Segments.扇形弧线分段数</param>
	/// <param name="startAngleDegree">Start angle degree.扇形开始角度</param>
	/// <param name="angleDegree">Angle degree.扇形角度</param>
	static public  Mesh CreateArc(float radius,int segments,float startAngleDegree, float angleDegree){
		if (segments <= 0) {
			segments = 1;
#if UNITY_EDITOR
			Debug.Log ("segments must be larger than zero.");
#endif
		}

		Mesh mesh = new Mesh ();
		Vector3[] vertices = new Vector3[3 + segments - 1];
		vertices [0] = new Vector3 (0, 0, 0);//第一个点是圆心点

		//uv是网格上的点对应到纹理上的某个位置的像素, 纹理是一张图片, 所以是二维
		//理解以后才发现, 之前显示出错的原因是原来的代码uv很随意的拿了顶点的计算结果
		Vector2[] uvs = new Vector2[vertices.Length];
		uvs [0] = new Vector2 (0.5f, 0.5f);//纹理的圆心在中心

		float angle = Mathf.Deg2Rad * angleDegree;
		float startAngle = Mathf.Deg2Rad * startAngleDegree;
		float currAngle = angle + startAngle; //第一个三角形的起始角度
		float deltaAngle = angle / segments; //根据分段数算出每个三角形在圆心的角的角度
		for (int i = 1; i < vertices.Length; i++) {   
			//圆上一点的公式: x = r*cos(angle), y = r*sin(angle)
			//根据半径和角度算出弧度上的点的位置
			float x = Mathf.Cos (currAngle);
			float y = Mathf.Sin (currAngle);
			//这里为了我的需求改到了把点算到了(x,y,0), 如果需要其他平面, 可以改成(x,0,y)或者(0,x,y)
			vertices [i] = new Vector3 (x * radius, y * radius, 0);
			//纹理的半径就是0.5, 圆心在0.5f, 0.5f的位置
			uvs [i] = new Vector2 (x * 0.5f + 0.5f, y * 0.5f + 0.5f);
			currAngle -= deltaAngle;
		}

		int[] triangles = new int[segments * 3];
		for (int i = 0, vi = 1; i < triangles.Length; i += 3, vi++) {//每个三角形都是由圆心点+两个相邻弧度上的点构成的
			triangles [i] = 0;
			triangles [i + 1] = vi;
			triangles [i + 2] = vi + 1;
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		mesh.RecalculateNormals();  
		mesh.RecalculateBounds();

		return mesh;
	}
	
	static public Mesh CreateCircle(float radius, int segments)
    {
        //vertices:
		int vertices_count = segments + 1;
        Vector3[] vertices = new Vector3[vertices_count];
        vertices[0] = Vector3.zero;
        float angledegree = 360.0f;
        float angleRad = Mathf.Deg2Rad * angledegree;
        float angleCur = angleRad;
		float angledelta = angleRad / segments;
        for(int i=1;i< vertices_count; i++)
        {
            float cosA = Mathf.Cos(angleCur);
            float sinA = Mathf.Sin(angleCur);

			vertices[i] = new Vector3(radius * cosA, 0, radius * sinA);
            angleCur -= angledelta;
        }

        //triangles
        int triangle_count = segments * 3;
        int[] triangles = new int[triangle_count];

		//因为该案例分割了60个三角形，故最后一个索引顺序应该是：0 60 1；所以需要单独处理
        for(int i=0,vi=1;i<= triangle_count-1;i+=3,vi++)     
        {
            triangles[i] = 0;
            triangles[i + 1] = vi;
            triangles[i + 2] = vi + 1;
        }

		//为了完成闭环，将最后一个三角形单独拎出来
        triangles[triangle_count - 3] = 0;
        triangles[triangle_count - 2] = vertices_count - 1;
        triangles[triangle_count - 1] = 1;                  

        //uv:
        Vector2[] uvs = new Vector2[vertices_count];
        for (int i = 0; i < vertices_count; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
        }

        //负载属性与mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

		mesh.RecalculateNormals();  
		mesh.RecalculateBounds();

        return mesh;
    }
	
	static public Mesh CreateQuad(float m_length,float m_width)
    {
        /* 1. 顶点，三角形，法线，uv坐标, 绝对必要的部分只有顶点和三角形。 
                 如果模型中不需要场景中的光照，那么就不需要法线。如果模型不需要贴材质，那么就不需要UV */
        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        vertices [0] = new Vector3 (0, 0, 0);
        uv [0] = new Vector2 (0, 0);
        normals [0] = Vector3.up;

        vertices [1] = new Vector3 (0, 0, m_length);
        uv [1] = new Vector2 (0, 1);
        normals [1] = Vector3.up;


        vertices [2] = new Vector3 (m_width, 0, m_length);
        uv [2] = new Vector2 (1, 1);
        normals [2] = Vector3.up;

        vertices [3] = new Vector3 (m_width, 0, 0);
        uv [3] = new Vector2 (1, 0);
        normals [3] = Vector3.up;

        /*2. 三角形,顶点索引： 
         三角形是由3个整数确定的，各个整数就是角的顶点的index。 各个三角形的顶点的顺序通常由下往上数， 可以是顺时针也可以是逆时针，这通常取决于我们从哪个方向看三角形。 通常，当mesh渲染时，"逆时针" 的面会被挡掉。 我们希望保证顺时针的面与法线的主向一致 */
        int[] indices = new int[6];
        indices [0] = 0;
        indices [1] = 1;
        indices [2] = 2;

        indices [3] = 0;
        indices [4] = 2;
        indices [5] = 3;

        Mesh mesh = new Mesh ();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = indices;
        
		mesh.RecalculateNormals();  
		mesh.RecalculateBounds();

		return mesh;
	}
}