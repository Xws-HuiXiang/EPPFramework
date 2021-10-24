## 所有文件生成的基础——协议json文件格式说明

* 每个json文件对应一类协议
* json中根对象为object类型
* 注意json文件的名称，用于编译proto时对应文件名称

##### MsgProtocolJsonData

| 字段名称          | 类型     | 说明                              | 是否必须 |
| ----------------- | -------- | --------------------------------- | -------- |
| Syntax            | string   | proto语法版本                     | 是       |
| Package           | string   | proto文件的包名                   | 是       |
| Namespace         | string   | 创建服务器Builder相关类的命名空间 | 是       |
| HandleID          | string   | 协议处理类枚举字段名称            | 是       |
| HandleIDValue     | int      | 协议处理类枚举字段值              | 是       |
| FileName          | string   | 创建服务器Builder相关类的文件名   | 是       |
| HandleDescription | string   | 协议处理类注释信息                | 是       |
| Message           | object[] | 协议处理方法对象数组              | 是       |

##### MessageItem

| 字段名称                | 类型     | 说明                              | 是否必须 |
| ----------------------- | -------- | --------------------------------- | -------- |
| HandleMethodID          | string   | 协议处理方法枚举字段名称          | 是       |
| HandleMethodIDValue     | int      | 协议处理方法枚举字段值            | 是       |
| HandleMethodDescription | string   | 协议处理方法注释信息              | 是       |
| UsePublicKey            | bool     | 是否使用公钥加密本协议。默认false | 否       |
| MessageEnum             | object[] | 协议内定义的枚举对象数组          | 否       |
| Parameters              | object[] | 协议字段列表                      | 是       |

##### MessageEnumItem

| 字段名称        | 类型     | 说明             | 是否必须 |
| --------------- | -------- | ---------------- | -------- |
| EnumName        | string   | 枚举类型名称     | 是       |
| EnumDescription | string   | 枚举类型注释信息 | 是       |
| EnumItem        | object[] | 枚举项对象数组   | 是       |

##### EnumItemItem

| 字段名称            | 类型   | 说明           | 是否必须 |
| ------------------- | ------ | -------------- | -------- |
| EnumItemName        | string | 枚举项名称     | 是       |
| EnumItemValue       | int    | 枚举项值       | 是       |
| EnumItemDescription | string | 枚举项注释信息 | 是       |

##### ParametersItem

| 字段名称     | 类型   | 说明                                                 | 是否必须 |
| ------------ | ------ | ---------------------------------------------------- | -------- |
| Name         | string | 协议字段名称                                         | 是       |
| ProtoType    | string | 协议字段数据类型                                     | 是       |
| SerializeTag | int    | proto文件中，字段的序列化值。从1开始                 | 是       |
| IsRepeated   | bool   | 在proto文件中，该字段是否使用repeated修饰。默认false | 否       |
| Description  | string | 协议字段说明                                         | 是       |

##### 一个json示例：MsgLogin.json

```json
{
	"Syntax": "proto3",
	"Package": "QiLieGuaner",
	"Namespace": "UNOServer.Protocol.Builder",
	"HandleID": "Login",
	"HandleIDValue": 1,
	"FileName": "MsgLogin",
	"HandleDescription": "登陆相关逻辑",
	"Message": [
		{
			"HandleMethodID": "RegisterRequest",
			"HandleMethodIDValue": 0,
			"HandleMethodDescription": "注册请求",
			"UsePublicKey": false,
			"Parameters": [
				{
					"Name": "username",
					"ProtoType": "string",
					"SerializeTag": 1,
					"IsRepeated": false,
					"Description": "用户名"
				},
				{
					"Name": "password",
					"ProtoType": "string",
					"SerializeTag": 2,
					"Description": "密码"
				},
				{
					"Name": "email",
					"ProtoType": "string",
					"SerializeTag": 3,
					"Description": "邮箱"
				},
				{
					"Name": "phoneNumber",
					"ProtoType": "string",
					"SerializeTag": 4,
					"IsRepeated": true,
					"Description": "手机号"
				}
			]
		},
		{
			"HandleMethodID": "RegisterReceive",
			"HandleMethodIDValue": 1,
			"HandleMethodDescription": "注册响应",
			"UsePublicKey": false,
			"Parameters": [
				{
					"Name": "state",
					"ProtoType": "uint32",
					"SerializeTag": 1,
					"Description": "注册结果"
				}
			]
		},
		{
			"HandleMethodID": "LoginRequest",
			"HandleMethodIDValue": 2,
			"HandleMethodDescription": "登陆请求",
			"UsePublicKey": false,
			"MessageEnum": [
				{
					"EnumName": "LoginTypeEnum",
					"EnumDescription": "登陆类型枚举",
					"EnumItem": [
						{
							"EnumItemName": "Account",
							"EnumItemValue": 0,
							"EnumItemDescription": "账号密码"
						},
						{
							"EnumItemName": "WeiXin",
							"EnumItemValue": 1,
							"EnumItemDescription": "微信"
						},
						{
							"EnumItemName": "QQ",
							"EnumItemValue": 2,
							"EnumItemDescription": "QQ"
						},
						{
							"EnumItemName": "XinLangWeiBo",
							"EnumItemValue": 3,
							"EnumItemDescription": "新浪微博"
						}
					]
				}
			],
			"Parameters": [
				{
					"Name": "loginType",
					"ProtoType": "LoginTypeEnum",
					"SerializeTag": 1,
					"Description": "登陆类型"
				},
				{
					"Name": "username",
					"ProtoType": "string",
					"SerializeTag": 2,
					"Description": "用户名 或 token"
				},
				{
					"Name": "password",
					"ProtoType": "string",
					"SerializeTag": 3,
					"Description": "密码"
				}
			]
		},
		{
			"HandleMethodID": "LoginReceive",
			"HandleMethodIDValue": 3,
			"HandleMethodDescription": "登陆响应",
			"UsePublicKey": false,
			"Parameters": [
				{
					"Name": "state",
					"ProtoType": "uint32",
					"SerializeTag": 1,
					"Description": "登陆结果"
				}
			]
		}
	]
}
```

