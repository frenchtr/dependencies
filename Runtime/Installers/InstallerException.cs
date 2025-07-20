using System;

namespace TravisRFrench.Dependencies.Installers
{
	public abstract class InstallerException : DependencyInjectionException
	{
		public InstallerException(IInstaller installer, string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}
	}
}
