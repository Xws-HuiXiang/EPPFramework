﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class TextureManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(TextureManager), typeof(Singleton<TextureManager>));
		L.RegFunction("GetTexture2DBytes", GetTexture2DBytes);
		L.RegFunction("GetTexture2D", GetTexture2D);
		L.RegFunction("GetSprite", GetSprite);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexture2DBytes(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			TextureManager obj = (TextureManager)ToLua.CheckObject<TextureManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
			System.Action<byte[]> arg3 = (System.Action<byte[]>)ToLua.CheckDelegate<System.Action<byte[]>>(L, 5);
			obj.GetTexture2DBytes(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTexture2D(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			TextureManager obj = (TextureManager)ToLua.CheckObject<TextureManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
			System.Action<UnityEngine.Texture2D> arg3 = (System.Action<UnityEngine.Texture2D>)ToLua.CheckDelegate<System.Action<UnityEngine.Texture2D>>(L, 5);
			obj.GetTexture2D(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSprite(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			TextureManager obj = (TextureManager)ToLua.CheckObject<TextureManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
			System.Action<UnityEngine.Sprite> arg3 = (System.Action<UnityEngine.Sprite>)ToLua.CheckDelegate<System.Action<UnityEngine.Sprite>>(L, 5);
			obj.GetSprite(arg0, arg1, arg2, arg3);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

