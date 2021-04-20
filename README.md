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

OR Create a background service:

Create file:
sudo nano /etc/systemd/system/daedalus.service


[Unit]
Description=daedalus iot process

[Service]
WorkingDirectory=/home/pi/daedalus
ExecStart=/opt/dotnet/dotnet /home/pi/daedalus/daedalus.iot.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=daedalus-iot
User=pi

[Install]
WantedBy=multi-user.target





