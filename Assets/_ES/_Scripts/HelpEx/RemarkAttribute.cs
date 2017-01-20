using System;

/// <summary>
/// 备注特性
/// </summary>
public class RemarkAttribute : Attribute {
	/// <summary>
	/// 备注
	/// </summary>
	public string Remark{ get; set;}

	public RemarkAttribute(string remark)
	{
		this.Remark = remark;
	}
}
