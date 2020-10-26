﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class EPPTools_SimpleStorage_StorageWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(EPPTools.SimpleStorage.Storage), typeof(System.Object));
		L.RegFunction("Reload", Reload);
		L.RegFunction("get_Item", get_Item);
		L.RegFunction("SetEncoding", SetEncoding);
		L.RegFunction("SetAutoSave", SetAutoSave);
		L.RegFunction("SetValue", SetValue);
		L.RegFunction("GetValue", GetValue);
		L.RegFunction("GetAllKeys", GetAllKeys);
		L.RegFunction("DeleteKey", DeleteKey);
		L.RegFunction("DeleteAllKey", DeleteAllKey);
		L.RegFunction("Save", Save);
		L.RegFunction("New", _CreateEPPTools_SimpleStorage_Storage);
		L.RegVar("this", _this, null);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("InitFinished", get_InitFinished, null);
		L.RegVar("AutoSave", get_AutoSave, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateEPPTools_SimpleStorage_Storage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.MonoBehaviour arg1 = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 2);
				EPPTools.SimpleStorage.Storage obj = new EPPTools.SimpleStorage.Storage(arg0, arg1);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 3)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.MonoBehaviour arg1 = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 2);
				System.Action arg2 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 3);
				EPPTools.SimpleStorage.Storage obj = new EPPTools.SimpleStorage.Storage(arg0, arg1, arg2);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 4)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.MonoBehaviour arg1 = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 2);
				System.Action arg2 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 3);
				System.Text.Encoding arg3 = (System.Text.Encoding)ToLua.CheckObject<System.Text.Encoding>(L, 4);
				EPPTools.SimpleStorage.Storage obj = new EPPTools.SimpleStorage.Storage(arg0, arg1, arg2, arg3);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else if (count == 5)
			{
				string arg0 = ToLua.CheckString(L, 1);
				UnityEngine.MonoBehaviour arg1 = (UnityEngine.MonoBehaviour)ToLua.CheckObject<UnityEngine.MonoBehaviour>(L, 2);
				System.Action arg2 = (System.Action)ToLua.CheckDelegate<System.Action>(L, 3);
				System.Text.Encoding arg3 = (System.Text.Encoding)ToLua.CheckObject<System.Text.Encoding>(L, 4);
				bool arg4 = LuaDLL.luaL_checkboolean(L, 5);
				EPPTools.SimpleStorage.Storage obj = new EPPTools.SimpleStorage.Storage(arg0, arg1, arg2, arg3, arg4);
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: EPPTools.SimpleStorage.Storage.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _get_this(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			object o = obj[arg0];
			ToLua.Push(L, o);
			return 1;

		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _this(IntPtr L)
	{
		try
		{
			LuaDLL.lua_pushvalue(L, 1);
			LuaDLL.tolua_bindthis(L, _get_this, null);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reload(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			obj.Reload();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			object o = obj[arg0];
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetEncoding(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			System.Text.Encoding arg0 = (System.Text.Encoding)ToLua.CheckObject<System.Text.Encoding>(L, 2);
			obj.SetEncoding(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoSave(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.SetAutoSave(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetValue(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			object arg1 = ToLua.ToVarObject(L, 3);
			obj.SetValue(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetValue(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				object o = obj.GetValue(arg0);
				ToLua.Push(L, o);
				return 1;
			}
			else if (count == 3)
			{
				EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				object o = obj.GetValue(arg0, arg1);
				ToLua.Push(L, o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: EPPTools.SimpleStorage.Storage.GetValue");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAllKeys(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			string[] o = obj.GetAllKeys();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteKey(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.DeleteKey(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteAllKey(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			obj.DeleteAllKey();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Save(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)ToLua.CheckObject<EPPTools.SimpleStorage.Storage>(L, 1);
			obj.Save();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_InitFinished(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)o;
			bool ret = obj.InitFinished;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index InitFinished on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AutoSave(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			EPPTools.SimpleStorage.Storage obj = (EPPTools.SimpleStorage.Storage)o;
			bool ret = obj.AutoSave;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index AutoSave on a nil value");
		}
	}
}
