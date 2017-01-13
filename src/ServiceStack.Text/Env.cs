
using System;

namespace ServiceStack.Text
{
	public static class Env
	{
		static Env()
		{
#if CORE_CLR
            IsUnix = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Linux);
#else
            var platform = (int)Environment.OSVersion.Platform;
			IsUnix = (platform == 4) || (platform == 6) || (platform == 128);
#endif

			IsMono = Type.GetType("Mono.Runtime") != null;

			IsMonoTouch = Type.GetType("MonoTouch.Foundation.NSObject") != null;

			SupportsExpressions = SupportsEmit = !IsMonoTouch;

			ServerUserAgent = "ServiceStack/" +
				ServiceStackVersion + " "
#if !CORE_CLR
                + Environment.OSVersion.Platform
#endif
				+ (IsMono ? "/Mono" : "/.NET")
				+ (IsMonoTouch ? " MonoTouch" : "");
		}

		public static decimal ServiceStackVersion = 3.60m;

		public static bool IsUnix { get; set; }

		public static bool IsMono { get; set; }

		public static bool IsMonoTouch { get; set; }

		public static bool SupportsExpressions { get; set; }

		public static bool SupportsEmit { get; set; }

		public static string ServerUserAgent { get; set; }
	}
}