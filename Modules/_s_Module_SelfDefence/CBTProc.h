#pragma once
static HHOOK currentHook;
LRESULT CALLBACK CBTHook(int nCode, WPARAM wParam, LPARAM lParam)
{
	if (nCode == HCBT_SYSCOMMAND && wParam == SC_CLOSE)
			return TRUE;
	LRESULT rv = CallNextHookEx(currentHook, nCode, wParam, lParam);
	return rv;
}