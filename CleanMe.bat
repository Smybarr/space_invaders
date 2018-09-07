REM cleanup root director

    rmdir /S /Q Debug
    rmdir /S /Q Release
    rmdir /S /Q ipch
    rmdir /S /Q .vs

    del /F /S /A:H *.suo
    del /F /S *.sdf
    del /F /S *.suo
    del /F *.db

REM walk through subdirectories delete generated directories

for /d %%D in (*) do (
	echo %%~fD
	rmdir /S /Q %%~D\Debug
	rmdir /S /Q %%~D\Release
	rmdir /S /Q %%~D\bin
	rmdir /S /Q %%~D\obj
)