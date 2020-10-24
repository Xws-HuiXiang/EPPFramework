﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class FDebuggerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginStaticLibs("FDebugger");
		L.RegFunction("Log", Log);
		L.RegFunction("LogFormat", LogFormat);
		L.RegFunction("LogWarning", LogWarning);
		L.RegFunction("LogWarningFormat", LogWarningFormat);
		L.RegFunction("LogError", LogError);
		L.RegFunction("LogErrorFormat", LogErrorFormat);
		L.RegFunction("Break", Break);
		L.EndStaticLibs();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Log(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				FDebugger.Log(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				FDebugger.Log(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.Log");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.LogType, UnityEngine.LogOption, UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 5, count - 4))
			{
				UnityEngine.LogType arg0 = (UnityEngine.LogType)ToLua.ToObject(L, 1);
				UnityEngine.LogOption arg1 = (UnityEngine.LogOption)ToLua.ToObject(L, 2);
				UnityEngine.Object arg2 = (UnityEngine.Object)ToLua.ToObject(L, 3);
				string arg3 = ToLua.ToString(L, 4);
				object[] arg4 = ToLua.ToParamsObject(L, 5, count - 4);
				FDebugger.LogFormat(arg0, arg1, arg2, arg3, arg4);
				return 0;
			}
			else if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				FDebugger.LogFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				FDebugger.LogFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.LogFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarning(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				FDebugger.LogWarning(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				FDebugger.LogWarning(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.LogWarning");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogWarningFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				FDebugger.LogWarningFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				FDebugger.LogWarningFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.LogWarningFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogError(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 1)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				FDebugger.LogError(arg0);
				return 0;
			}
			else if (count == 2)
			{
				object arg0 = ToLua.ToVarObject(L, 1);
				UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
				FDebugger.LogError(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.LogError");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogErrorFormat(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (TypeChecker.CheckTypes<UnityEngine.Object, string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 3, count - 2))
			{
				UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
				FDebugger.LogErrorFormat(arg0, arg1, arg2);
				return 0;
			}
			else if (TypeChecker.CheckTypes<string>(L, 1) && TypeChecker.CheckParamsType<object>(L, 2, count - 1))
			{
				string arg0 = ToLua.ToString(L, 1);
				object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
				FDebugger.LogErrorFormat(arg0, arg1);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: FDebugger.LogErrorFormat");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Break(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			FDebugger.Break();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

