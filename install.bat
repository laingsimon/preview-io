@set netframework32=c:\Windows\Microsoft.NET\Framework64\v4.0.30319\
@set netframework64=c:\Windows\Microsoft.NET\Framework\v4.0.30319\
@set path=%netframework32%;%netframework64%;%path%
@set installDir=%ProgramFiles%\PreviewIo

@mkdir "%installDir%" 2> nul

@xcopy "PreviewIo\bin\Release\*.*" "%installDir%\" /Y
@copy uninstall.bat "%installDir%\" /Y

@regasm.exe "%installDir%\PreviewIo.dll" /codebase /nologo /silent

@if "%errorlevel%"=="0" @echo Installed successfully