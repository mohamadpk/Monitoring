#pragma once
BOOL WINAPI MyTerminateProcess(_In_ HANDLE hProcess,_In_ UINT   uExitCode);

void HookTerminateProcess()
{
	//cHookMgr.SetEnableDebugOutput(TRUE);
	hKernel32dll = NktHookLibHelpers::GetModuleBaseAddress(L"Kernel32.dll");
	//hook TerminateProcess
	fnOrigTerminateProcess = NktHookLibHelpers::GetProcedureAddress(hKernel32dll, "TerminateProcess");

	dwOsErr = cHookMgr.Hook(&(sTerminateProcess_Hook.nHookId), (LPVOID*)&(sTerminateProcess_Hook.fnTerminateProcess),
		fnOrigTerminateProcess, MyTerminateProcess,
		NKTHOOKLIB_DisallowReentrancy
		);
}



BOOL WINAPI MyTerminateProcess(
	_In_ HANDLE hProcess,
	_In_ UINT   uExitCode
	)
{

	bool ismyProtectedProcess = false;

	HANDLE hProcessDuplicate = 0;
	bool Success = DuplicateHandle(GetCurrentProcess(), hProcess, GetCurrentProcess(), &hProcessDuplicate, PROCESS_QUERY_LIMITED_INFORMATION, true, 0);
	if (Success)
	{
		int error = GetLastError();
		char *fileName = new char[MAX_PATH];
		int len = GetProcessImageFileNameA(hProcessDuplicate, fileName, MAX_PATH);

		if (len>0)
		{
			for (size_t i = 0; i < targets_len; i++)
			{
				if (StrStrA(fileName, targets[i])>0)
				{
					ismyProtectedProcess = true;
				}
			}
			//OutputDebugStringA(fileName);

		}
	}

	bool ret = true;
	if (ismyProtectedProcess == false)
	{
		//dwOsErr = cHookMgr.Unhook(sTerminateProcess_Hook.nHookId);		
		ret = TerminateProcess(hProcess, uExitCode);
		//HookTerminateProcess();
	}

	return ret;
}
