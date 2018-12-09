#pragma once  


#define DllExport extern "C" __declspec( dllexport )   

using namespace std;
#include<Windows.h>
#include<string>
#include <comutil.h>

DllExport BSTR __cdecl GetOperaPasswordFromDLL();
#include "sqlite3.h"
#include "Public.h"
#pragma comment(lib, "shlwapi")
#pragma comment(lib, "crypt32")
#define FILENAME_INSTALLER_PREFS_JSON				L"\\Opera\\installer_prefs.json"
#define FILENAME_INSTALLATION_STATUS				L"\\Opera\\installation_status.xml"
#define FILENAME_LOGIN_DATA							L"Login Data"
#define FILENAME_PREFERENCES						L"Preferences"
#define FILENAME_LAUNCHER_EXE						L"\\Opera\\launcher.exe"
#define FILENAME_LOCKFILE                           L"lockfile"
#define EXTENSION_JOURNAL							L"-journal"
#define EXTENSION_TEMP								L".temp"
#define EXTENSION_BACKUP							L".backup"
#define FOLDER_PASSWORDS							L"Portable Passwords"

#define ERROR_UNABLE_TO_FIND_LAUNCHER_EXE			1
#define ERROR_UNABLE_TO_FIND_OPERA_PROFILE			2
#define ERROR_INVALID_OPERA_PROFILE					3
#define ERROR_UNABLE_TO_CREATE_PASSWORDS_FOLDER		4
#define ERROR_UNABLE_TO_ENCRYPT_PASSWORDS			5
#define ERROR_UNABLE_TO_DECRYPT_PASSWORDS			6
#define ERROR_UNABLE_TO_RUN_LAUNCHER_EXE			7

char * LoadFileIntoString(PCWSTR pwszFileName);
bool IsPortable();
PCWSTR GetChannel();
PWSTR PathAddBackslash_s(PWSTR pwszPath, int cchPath);
void PathAppend_s(PWSTR pwszPath, int cchPath, PCWSTR pwszMore);
PCWSTR PathCombine_s(PWSTR pwszPathOut, int cchPathOut, PCWSTR pwszPathIn, PCWSTR pwszMore);
bool GetOperaProfileFolder(PWSTR pwsz, int cch);
BYTE *ClearPassword(BYTE * inp);

HRESULT ProcessPasswords(PCWSTR pwszSourceFolder, PCWSTR pwszTargetFolder, bool fDecrypt);
int Error(int nCode, PCWSTR pwszFormat, ...);
