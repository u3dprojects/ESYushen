using System;
using System.Reflection;
using System.ComponentModel;

/// <summary>
/// Enum extension.
/// 枚举扩展对象获取描述信息
/// </summary>
public static class EnumExtension {
	
	public static string GetRemark(this Enum value)
	{
		FieldInfo fi = value.GetType().GetField(value.ToString());
		if (fi == null)
		{
			return value.ToString();
		}
		object[] attributes = fi.GetCustomAttributes(typeof(RemarkAttribute), false);
		if (attributes.Length > 0)
		{
			return ((RemarkAttribute)attributes[0]).Remark;
		}
		else
		{
			return value.ToString();
		}
	}

	public static string GetDescription(this Enum value)
	{
		FieldInfo fi = value.GetType().GetField(value.ToString());
		DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
		if (attributes.Length > 0)
		{
			return attributes[0].Description;
		}
		else
		{
			return value.ToString();
		}
	}
}
