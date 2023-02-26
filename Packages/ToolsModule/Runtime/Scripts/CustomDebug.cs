using UnityEngine;

namespace GRV.ToolsModule
{
	public class CustomDebug
	{
		public static bool IsAvailable { get; set; } = true;


		public static void Log(string message, Color color, Object obj = null)
		{
			message = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + message + "</color>";
			Log(message, obj);
		}


		public static void LogError(object message, Object obj = null)
		{
			Log("" + message, Color.red, obj);
		}


		public static void LogWarning(object message, Object obj = null)
		{
			Log("" + message, Color.yellow, obj);
		}


		public static void Log(object message, Object obj = null)
		{
			#if !BUILD_TYPE_PUBLISHER
			if (IsAvailable)
				Debug.Log(message, obj);
			#endif
		}


		public static void LogError(string message, Object obj = null)
		{
			Log(message, Color.red, obj);
		}


		public static void LogWarning(string message, Object obj = null)
		{
			Log(message, Color.yellow, obj);
		}
	}
}
