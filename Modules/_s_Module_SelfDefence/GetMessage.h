#pragma once
BOOL WINAPI MyGetMessage(_Out_ LPMSG lpMsg,_In_opt_ HWND hWnd,_In_ UINT wMsgFilterMin,_In_ UINT wMsgFilterMax);
void HookGetMessage()
{
	hUser32dll = NktHookLibHelpers::GetModuleBaseAddress(L"User32.dll");
	//hook GetMessage
	fnOrigGetMessageA = NktHookLibHelpers::GetProcedureAddress(hUser32dll, "GetMessageA");
	fnOrigGetMessageW = NktHookLibHelpers::GetProcedureAddress(hUser32dll, "GetMessageW");

	dwOsErr = cHookMgr.Hook(&(sGetMessageA_Hook.nHookId), (LPVOID*)&(sGetMessageA_Hook.fnGetMessage),
		fnOrigGetMessageA, MyGetMessage,
		NKTHOOKLIB_DisallowReentrancy
		);
	dwOsErr = cHookMgr.Hook(&(sGetMessageW_Hook.nHookId), (LPVOID*)&(sGetMessageW_Hook.fnGetMessage),
		fnOrigGetMessageW, MyGetMessage,
		NKTHOOKLIB_DisallowReentrancy
		);
}



BOOL WINAPI MyGetMessage(_Out_ LPMSG lpMsg, _In_opt_ HWND hWnd, _In_ UINT wMsgFilterMin, _In_ UINT wMsgFilterMax)
{
	bool ret = GetMessage(lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax);
	//OutputDebugStringA("msg");
	if (lpMsg->message == WM_SYSCOMMAND)
	{
		OutputDebugStringA("WM_SYSCOMMAND");
		if (lpMsg->wParam == SC_CLOSE)
		{
			//OutputDebugStringA("SC_CLOSE");
			lpMsg->wParam == 0;
		}
	}
	return ret;
}
