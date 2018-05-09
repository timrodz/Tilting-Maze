/// <summary>
/// Use this class to print logs only in debug mode/builds
/// 
/// Date: 09/05/2018
/// </summary>

using UnityEngine;

public class Print
{
	public static void Log (string _message)
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log (_message);
		}
	}

	public static void Log (Object _message)
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log (_message);
		}
	}

	public static void Log (Object _message, Object _context)
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log (_message, _context);
		}
	}

	public static void LogFormat (string format, params object[] args)
	{
		if (Debug.isDebugBuild)
		{
			Debug.LogFormat (format, args);
		}
	}

	public static void LogError (string _message)
	{
		// if (Debug.isDebugBuild)
		// {
		// }
		Debug.LogError (_message);
	}
}