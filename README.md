# pi

https://docs.microsoft.com/en-us/dotnet/iot/

For the blink project - adapt for other projects:

### On Pi:
mkdir blinky

### From dev machine, in project directory.
dotnet publish

scp -r .\bin\Debug\net5.0\publish\* pi@pi-cdp:/home/pi/blinky

### Run on Pi:
dotnet blinky.dll &

The '&' makes it run in the background


## Blinky Notes:
I wired my LED and other components according to the wiring diagram: https://docs.microsoft.com/en-us/dotnet/iot/tutorials/blink-led

But it didn't work. I had to flip the LED around, and then it worked perfectly.
