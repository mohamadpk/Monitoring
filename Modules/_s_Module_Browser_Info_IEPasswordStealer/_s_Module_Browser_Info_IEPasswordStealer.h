#pragma once  


#define DllExport extern "C" __declspec( dllexport )   

using namespace std;
#include<Windows.h>
#include<string>
#include <comutil.h>

DllExport BSTR __cdecl GetIEPasswordFromDLL();

