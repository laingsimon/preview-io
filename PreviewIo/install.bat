@set netframework32=c:\Windows\Microsoft.NET\Framework64\v4.0.30319\
@set netframework64=c:\Windows\Microsoft.NET\Framework\v4.0.30319\
@set path=%netframework32%;%netframework64%;%path%
@set installDir=%ProgramFiles%\PreviewIo
@set source=%~dp0

@mkdir "%installDir%" 2> nul

@copy "%source%\uninstall.bat" "%installDir%\" /Y
@if not exist "%installDir%\uninstall.bat" goto error

@xcopy "%source%*.*" "%installDir%\" /Y

@regasm.exe "%installDir%\PreviewIo.dll" /codebase /nologo /silent

@if "%errorlevel%"=="0" @echo Installed successfully
@goto eof

:error
@echo Error copying files
@goto eof

:eof
@pause