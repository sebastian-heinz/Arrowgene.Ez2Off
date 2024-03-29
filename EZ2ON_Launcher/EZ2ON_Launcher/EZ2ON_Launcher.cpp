// Visual Studio 2019
// EZ2ON R14 Launcher

#include "stdafx.h"
#include <stdio.h>
#include <windows.h>
#include <tchar.h>
#include <conio.h>
#include "SimpleIni.h"
#include <iostream> 

using namespace std;

#pragma warning (disable:4996)

#define BUF_SIZE 1062

CHAR EZTOSHR[] = "EZTOSHR";

char *concat(const char *a, const char *b){
	int lena = strlen(a);
	int lenb = strlen(b);
	char *con = (char *)malloc(lena + lenb + 1);
	// copy & concat (including string termination)
	memcpy(con, a, lena);
	memcpy(con + lena, b, lenb + 1);
	return con;
}

int main(PCHAR pBuf)
{
	_tprintf(TEXT("wait...\n"));
	HANDLE hMapFile;

	STARTUPINFO StartupInfo = { 0 };
	StartupInfo.cb = sizeof(STARTUPINFO);
	PROCESS_INFORMATION ProcessInfo;

	const char * iniFile = "EZ2ON_Launcher.ini";
	CSimpleIniA ini(true, true, true);
	SI_Error rc = ini.LoadFile(iniFile);
	if (rc < 0) {
		ini.SetValue("Account", "Account", "test");
		ini.SetValue("Account", "Password", "test");
		ini.SetValue("Server", "IPAddress", "127.0.0.1");
		ini.SetValue("Server", "Port", "9350");
		rc = ini.SaveFile(iniFile);
	}

	hMapFile = CreateFileMappingA(
		INVALID_HANDLE_VALUE,    // use paging file
		NULL,                    // default security
		PAGE_READWRITE,          // read/write access
		0,                       // maximum object size (high-order DWORD)
		BUF_SIZE,                // maximum object size (low-order DWORD)
		EZTOSHR);                 // name of mapping object

	if (hMapFile == NULL)
	{
		_tprintf(TEXT("error 1\n"),
			GetLastError());
		_getch();
		return 1;
	}
	pBuf = (PCHAR)MapViewOfFile(hMapFile,   // handle to map object
		FILE_MAP_ALL_ACCESS, // read/write permission
		0,
		0,
		BUF_SIZE);

	if (pBuf == NULL)
	{
		_tprintf(TEXT("error 2\n"),
			GetLastError());

		CloseHandle(hMapFile);
		_getch();
		return 1;
	}

	const char * c_Account = ini.GetValue("Account", "Account", "test");
	const char * c_Password = ini.GetValue("Account", "Password", "test"); //md5 : 098f6bcd4621d373cade4e832627b4f6
	const char * c_IPAddress = ini.GetValue("Server", "IPAddress", "127.0.0.1");
	const char * c_Port = ini.GetValue("Server", "Port", "9350");

	const char * combined = concat(concat(c_Account, "|"), c_Password);
	strcpy((char*)pBuf, combined);
	strcpy((char*)pBuf + 512, c_IPAddress);
	strcpy((char*)pBuf + 532, c_Port);
	Sleep(1000);
	CreateProcessW(L".\\ez2on.exe",
		NULL, NULL, NULL, FALSE, NULL, NULL, NULL, &StartupInfo, &ProcessInfo);
	Sleep(2000);
	UnmapViewOfFile((LPCVOID)pBuf);

	CloseHandle(hMapFile);

	return 0;
}