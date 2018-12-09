// _s_Module_SelfDefence.cpp : Defines the exported functions for the DLL application.
//
#if defined _M_IX86
#ifdef _DEBUG
#pragma comment (lib, "NktHookLib_Debug.lib")
#endif //_DEBUG
#elif defined _M_X64
#ifdef _DEBUG
#pragma comment (lib, "NktHookLib64_Debug.lib")
#endif //_DEBUG
#endif


#include "stdafx.h"
#include <Windows.h>
#include <process.h>
#include <Winternl.h>
#include <Shlwapi.h>
#include <Psapi.h>
#include"NktHookLib\inc\NktHookLib.h"
#include <Ntstatus.h>

int targets_len = 2;
char *targets[] = { "Monitoring.exe","watermark.exe" };
wchar_t *wtargets[] = {L"Monitoring.exe",L"watermark.exe" };
//char *target = "notepad.exe";
//WCHAR g_TargetProc[] = L"notepad.exe";


typedef BOOL (WINAPI *lpfnTerminateProcess)(_In_ HANDLE hProcess, _In_ UINT   uExitCode);


static struct {
	SIZE_T nHookId;
	lpfnTerminateProcess fnTerminateProcess;
}sTerminateProcess_Hook = { 0, NULL };

typedef NTSTATUS(WINAPI *lpfnNtQuerySystemInformation)(SYSTEM_INFORMATION_CLASS SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength);
static struct {
	SIZE_T nHookId;
	lpfnNtQuerySystemInformation fnNtQuerySystemInformation;
}sNtQuerySystemInformation_Hook = { 0, NULL };

typedef BOOL(WINAPI *lpGetMessage)(_Out_ LPMSG lpMsg, _In_opt_ HWND hWnd, _In_ UINT wMsgFilterMin, _In_ UINT wMsgFilterMax);
static struct {
	SIZE_T nHookId;
	lpGetMessage fnGetMessage;
}sGetMessageA_Hook = { 0, NULL }, sGetMessageW_Hook = { 0, NULL };


typedef LRESULT (WINAPI *lpDefWindowProc)(_In_ HWND   hWnd, _In_ UINT   Msg, _In_ WPARAM wParam, _In_ LPARAM lParam);
static struct {
	SIZE_T nHookId;
	lpDefWindowProc fnDefWindowProc;
}sDefWindowProc_Hook = { 0, NULL };

CNktHookLib cHookMgr;
HINSTANCE hKernel32dll;
HINSTANCE hNtdlldll;
HINSTANCE hUser32dll;
LPVOID fnOrigTerminateProcess;
LPVOID fnOrigNtQuerySystemInformation;
LPVOID fnOrigGetMessageA;
LPVOID fnOrigGetMessageW;
LPVOID fnOrigDefWindowProc;
DWORD dwOsErr;

#include "TerminateProcess.h"
#include "NtQuerySystemInformation.h"
#include "GetMessage.h"
#include "DefWindowProc.h"
#include "CBTProc.h"

struct HookMain
{
	bool IAmOnTargetProcess = false;
	HookMain()
	{
		OutputDebugStringA("Hook Message0");
		char*FileName = new char[MAX_PATH];
		GetModuleFileNameA(NULL, FileName, MAX_PATH);
		for (size_t i = 0; i < targets_len; i++)
		{
			if (StrStrA(FileName, targets[i]) > 0)
			{
				IAmOnTargetProcess = true;
			}
		}

		if (IAmOnTargetProcess)
		{
			OutputDebugStringA("Hook Message");
			//HookGetMessage();
			//HookDefWindowProc();
			currentHook= SetWindowsHookExA(WH_CBT, CBTHook, NULL, GetCurrentThreadId());
			
		}
		else
		{
			HookTerminateProcess();
			HookNtQuerySystemInformation();
		}


	}
	~HookMain()
	{

		if (IAmOnTargetProcess)
		{
			UnhookWindowsHookEx(currentHook);
		}
		else
		{
			dwOsErr = cHookMgr.Unhook(sTerminateProcess_Hook.nHookId);
			//dwOsErr = cHookMgr.Unhook(sNtQuerySystemInformation_Hook.nHookId);
			dwOsErr = cHookMgr.Unhook(sGetMessageA_Hook.nHookId);
			dwOsErr = cHookMgr.Unhook(sGetMessageW_Hook.nHookId);
			dwOsErr = cHookMgr.Unhook(sDefWindowProc_Hook.nHookId);
		}
		
	}
}g_HookMain;