using System.Diagnostics;
using System.Reflection;

namespace Shared;

public class Logger
{
	public static void Log(object message)
	{
		// Get the time
		DateTime time = DateTime.Now;

		// Get the namespace name of the program thats calling it
		// TODO: Document and maybe chuck in method
		StackTrace stackTrace = new StackTrace();
		MethodBase? callingMethod = stackTrace.GetFrame(1)?.GetMethod();
		Type? declaringType = callingMethod?.DeclaringType;
		string programName = declaringType?.Namespace ?? "idk";

		// Log it all out
		Console.WriteLine($"{time.ToString("hh:mm:ss")} | {programName}\t| {message}");
	}
}