using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 类名 : excel子弹实体对象
/// 作者 : Canyon
/// 日期 : 2017-01-22 09:10
/// 功能 : 
/// </summary>
public class EN_Bullet : EN_BaseXls{
    // public int ID;

	// 子弹的资源名称
    public string ResName;

	// 飞行速度
	public float Speed;

	// 是否贴地
	public int IsGround;

	// 生命时间
	public float LifeTime;

	// 是否跟踪
	public int IsFollowTarget;

	// 爆炸点
	public float BlowUpTime;

	// 爆炸特效
	public string BlowUpEffectName;

	// 穿透个数
	public int ThroughNum;

	// 飞行碰撞筛选标识
	public int FlyColliderFiter;

	// 飞行中伤害区域检查
	public string AreaFlying;

	// 爆炸碰撞筛选标识
	public int BlowUpColliderFiter;

	// 爆炸伤害数量
	public int BlowUpHitNum;

	// 爆炸伤害区域检查
	public string AreaBlowUp;

	// 飞行伤害事件
	public string EvtFlying;

	// 爆炸伤害事件
	public string EvtBlowUp;

	public EN_Bullet():base(){
	}

	protected override object[] Columns
    {
        get { 
            object[] ret = {
                this.ID,
                this.ResName,
				this.Speed,
				this.IsGround,
				this.LifeTime,
				this.IsFollowTarget,
				this.BlowUpTime,
				this.BlowUpEffectName,
				this.ThroughNum,
				this.FlyColliderFiter,
				this.AreaFlying,
				this.BlowUpColliderFiter,
				this.BlowUpHitNum,
				this.AreaBlowUp,
				this.EvtFlying,
				this.EvtBlowUp
            };
            return ret;
        }
    }

	public override void DoInit (int rowIndex, NH_Sheet sheet)
	{
		base.DoInit (rowIndex, sheet);

		int colIndex = 0;
		this.ID = sheet.GetInt(rowIndex, colIndex++);
		this.ResName = sheet.GetString(rowIndex, colIndex++);
		this.Speed = sheet.GetFloat(rowIndex, colIndex++);
		this.IsGround = sheet.GetInt(rowIndex, colIndex++);
		this.LifeTime = sheet.GetFloat(rowIndex, colIndex++);
		this.IsFollowTarget = sheet.GetInt(rowIndex, colIndex++);
		this.BlowUpTime = sheet.GetFloat(rowIndex, colIndex++);
		this.BlowUpEffectName = sheet.GetString(rowIndex, colIndex++);
		this.ThroughNum = sheet.GetInt(rowIndex, colIndex++);
		this.FlyColliderFiter = sheet.GetInt(rowIndex, colIndex++);
		this.AreaFlying = sheet.GetString(rowIndex, colIndex++);
		this.BlowUpColliderFiter = sheet.GetInt(rowIndex, colIndex++);
		this.BlowUpHitNum = sheet.GetInt(rowIndex, colIndex++);
		this.AreaBlowUp = sheet.GetString(rowIndex, colIndex++);
		this.EvtFlying = sheet.GetString(rowIndex, colIndex++);
		this.EvtBlowUp = sheet.GetString(rowIndex, colIndex++);
	}

	public override void DoClone (EN_BaseXls org)
	{
		if (!(org is EN_Bullet)) {
			return;
		}

		EN_Bullet tmp = (EN_Bullet)org;

		base.DoClone (tmp);
		this.ID = tmp.ID;
		this.ResName = tmp.ResName;
		this.Speed = tmp.Speed;
		this.IsGround = tmp.IsGround;
		this.LifeTime = tmp.LifeTime;
		this.IsFollowTarget = tmp.IsFollowTarget;
		this.BlowUpTime = tmp.BlowUpTime;
		this.BlowUpEffectName = tmp.BlowUpEffectName;
		this.ThroughNum = tmp.ThroughNum;
		this.AreaFlying = tmp.AreaFlying;
		this.BlowUpHitNum = tmp.BlowUpHitNum;
		this.AreaBlowUp = tmp.AreaBlowUp;
		this.EvtFlying = tmp.EvtFlying;
		this.EvtBlowUp = tmp.EvtBlowUp;
		this.FlyColliderFiter = tmp.FlyColliderFiter;
		this.BlowUpColliderFiter = tmp.BlowUpColliderFiter;
	}
}