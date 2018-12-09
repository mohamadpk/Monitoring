#pragma once
LRESULT WINAPI MyDefWindowProc(_In_ HWND   hWnd,_In_ UINT   Msg,_In_ WPARAM wParam,_In_ LPARAM lParam);
void HookDefWindowProc()
{
	hUser32dll = NktHookLibHelpers::GetModuleBaseAddress(L"User32.dll");
	//hook GetMessage
	fnOrigDefWindowProc = NktHookLibHelpers::GetProcedureAddress(hUser32dll, "DefWindowProc");

	dwOsErr = cHookMgr.Hook(&(sDefWindowProc_Hook.nHookId), (LPVOID*)&(sDefWindowProc_Hook.fnDefWindowProc),
		fnOrigDefWindowProc, MyDefWindowProc,
		NKTHOOKLIB_DisallowReentrancy
		);
}



LRESULT WINAPI MyDefWindowProc(
	_In_ HWND   hWnd,
	_In_ UINT   Msg,
	_In_ WPARAM wParam,
	_In_ LPARAM lParam
	)
{

	OutputDebugStringA("msg");
	if (Msg == WM_SYSCOMMAND)
	{
		OutputDebugStringA("WM_SYSCOMMAND");
		if (wParam == SC_CLOSE)
		{
			OutputDebugStringA("SC_CLOSE");
			return 1;
		}
	}

	LRESULT ret = DefWindowProc(hWnd, Msg, wParam, lParam);
	return ret;
}
