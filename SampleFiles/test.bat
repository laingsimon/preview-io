:: DELT.bat
:: 
:: Deletes Specified Directory and All Files 
::    and Directories Below
:: Prompts "Are You Sure?" Before Deletion Commences.
::
@ECHO OFF

IF "%1" == "" GOTO NO-DIRECTORY

ECHO.
ECHO.

TREE %1

DELTREE %1
DR
GOTO END

:NO-DIRECTORY
ECHO.
ECHO   No Directory Specified
ECHO.

:END
