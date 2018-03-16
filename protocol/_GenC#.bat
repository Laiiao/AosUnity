@echo off
set cs_out_path=C#\
set target_path=..\Code\Hotfix\Impl\Runtime\Command\Proto\

for /r %cs_out_path% %%i in (*.cs) do del %%i
for /r %target_path% %%i in (*.cs) do del %%i

for %%i in (*.proto) do (
   echo gen %%~nxi...
   tool\protoc.exe --csharp_out=%cs_out_path%  %%~nxi)

copy %cs_out_path%*.cs %target_path%

echo finish... 
pause