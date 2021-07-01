﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class PbFileListDataWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(PbFileListData), typeof(System.Object));
		L.RegFunction("New", _CreatePbFileListData);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("URLList", get_URLList, set_URLList);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreatePbFileListData(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				PbFileListData obj = new PbFileListData();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: PbFileListData.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_URLList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			PbFileListData obj = (PbFileListData)o;
			System.Collections.Generic.List<string> ret = obj.URLList;
			ToLua.PushSealed(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index URLList on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_URLList(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			PbFileListData obj = (PbFileListData)o;
			System.Collections.Generic.List<string> arg0 = (System.Collections.Generic.List<string>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.List<string>));
			obj.URLList = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index URLList on a nil value");
		}
	}
}
