# pi

https://docs.microsoft.com/en-us/dotnet/iot/

### On Pi:
mkdir daedalus

### From dev machine, in project directory.
dotnet publish

scp -r .\bin\Debug\net5.0\publish\\* pi@picdp:/home/pi/daedalus

### Run on Pi:
dotnet daedalus.iot.dll &

The '&' makes it run in the background

