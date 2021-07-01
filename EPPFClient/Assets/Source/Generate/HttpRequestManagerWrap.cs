﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class HttpRequestManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(HttpRequestManager), typeof(Singleton<TextureManager>));
		L.RegFunction("HttpGetRequest", HttpGetRequest);
		L.RegFunction("HttpGetRequestAsync", HttpGetRequestAsync);
		L.RegFunction("HttpPostRequest", HttpPostRequest);
		L.RegFunction("New", _CreateHttpRequestManager);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateHttpRequestManager(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				HttpRequestManager obj = new HttpRequestManager();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: HttpRequestManager.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HttpGetRequest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			string o = HttpRequestManager.HttpGetRequest(arg0);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HttpGetRequestAsync(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				System.Action<string> arg1 = (System.Action<string>)ToLua.CheckDelegate<System.Action<string>>(L, 2);
				HttpRequestManager.HttpGetRequestAsync(arg0, arg1);
				return 0;
			}
			else if (count == 3)
			{
				string arg0 = ToLua.CheckString(L, 1);
				string arg1 = ToLua.CheckString(L, 2);
				System.Action<string> arg2 = (System.Action<string>)ToLua.CheckDelegate<System.Action<string>>(L, 3);
				HttpRequestManager.HttpGetRequestAsync(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: HttpRequestManager.HttpGetRequestAsync");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HttpPostRequest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			string o = HttpRequestManager.HttpPostRequest(arg0, arg1);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
