using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public interface IInjectionContext
	{
		Type TargetType { get; }
		object TargetInstance { get; }
		string MemberName { get; }
		Type MemberType { get; }
		MemberInfo TargetMember { get; }
		FieldInfo TargetField { get; }
		PropertyInfo TargetProperty { get; }
		MethodInfo TargetMethod { get; }
		ParameterInfo TargetParameter { get; }
		Type ParameterType { get; }
		string ParameterName { get; }
	}
}
