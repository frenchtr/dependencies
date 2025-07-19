using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class InjectionContext : IInjectionContext
	{
		public Type TargetType { get; set; }
		public object TargetInstance { get; set; }
		public string MemberName { get; set; }
		public Type MemberType { get; set; }
		public MemberInfo TargetMember { get; set; }
		public FieldInfo TargetField { get; set; }
		public PropertyInfo TargetProperty { get; set; }
		public MethodInfo TargetMethod { get; set; }
		public ParameterInfo TargetParameter { get; set; }

		public Type ParameterType { get; set; }

		public string ParameterName { get; set; }
	}
}
