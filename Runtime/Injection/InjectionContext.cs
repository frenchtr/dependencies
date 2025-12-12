using System;
using System.Reflection;

namespace TravisRFrench.Dependencies.Injection
{
	public class InjectionContext : IInjectionContext
	{
		public InjectionMode InjectionMode { get; set; }
		public Type InjectedObjectType { get; set; }
		public object InjectedObjectInstance { get; set; }
		public string MemberName { get; set; }
		public Type MemberType { get; set; }
		public MemberInfo TargetMember { get; set; }
		public FieldInfo TargetField { get; set; }
		public PropertyInfo TargetProperty { get; set; }
		public MethodInfo TargetMethod { get; set; }
		public ParameterInfo TargetParameter { get; set; }
		public ConstructorInfo TargetConstructor { get; set; }
		public Type ParameterType { get; set; }
		public string ParameterName { get; set; }
		public string InjectedName { get; set; }
	}
}
