using System;
using System.Collections.Generic;

public static class Log
{
	private static List<String> Consola = new List<string>();


	public static void AddLog(string log)
    {
		Consola.Add(log);
    }

	public static List<String> GetLogs()
    {
		return Consola;
    }

	public static void Clear()
    {
		Consola.Clear();
    }

	
	

}
