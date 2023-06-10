Raspberry Pi connected through USB to a thermal printer. Uses escpos library in python. Find the vendor id, product id, input endpoint, output endpoint using lsusb. 

The Pi gets weather alert data from NOAA's API, free to use. It will gather all alerts from a zone, identified by a zone id, and iterate through each alert to generate an alert object based off an alert class. Of these alerts, any that are not already in a list of stored alerts will be printed out on the termal printer before being added to the list. This keeps the Pi from printing existing alerts after the first time (it scans NOAA every interval, so we only want to print an alert once and then stop printing it)

https://alerts.weather.gov/cap/co.php?x=3 for zone codes
https://www.weather.gov/documentation/services-web-api for info on the API

https://pypi.org/project/python-escpos/ library documentation

After this, it will iterate through its list of seen alerts and check against NOAA, this is garbage collection. This way, an alert that was printed, but not seen from NOAA, can be deleted since it has expired.

## Printer information

Using Rongta model RP326 printer, for 80mm paper connected over USB
Use prolific driver version 3.3.2.102, the new default driver when installing this wont work. Get the older version online. If you see anything mentioning "retired in 2012" or you get a "failed to start code 100" error, then the version is too new.

(from the pi, may be different on another machine):
vendor = 0x0fe6
product = 0x811e
inputPoint = 0x81
outputPoint = 0x03

There was a configuration software and a C language SDK available from the printers website. I have not used the SDK but the configuration tool does seem to work.
Look for "Thermal Printer Tool" and "Thermal Printer Windows SDK," they are OneDrive folders.
https://www.rongtatech.com/category/downloads/4

When using the SDK, make sure you include the POSDLL.dll file

If you try to use the example Visual Studio Solution, you will need Visual Studio 2019. This is because the sample requires .NET Framework 4.0 which is only supported up to VS2019.

## Running at startup
https://raspberrypi-guide.github.io/programming/run-script-on-boot

This will need to be ran at startup:
```
sudo nano /etc/rc.local
```

Edit rc.local to include the command to run this program, such as:
```
python /home/administrator/active_alerts_dump.py
exit 0
```
(leave the exit 0 at the end of the file)

Remember to reference the file as absolute.

## Transferring files from PC to Pi


When ssh'ing into the pi you can transfer files written on the main machine to the pi itself using scp. The ip here is the address on the LAN
```
scp active_alerts_dump.py administrator@192.168.1.45:/homme/administrator
```
or
```
scp FILE USERNAME@IP_ADDRESS:FILEPATH
```

## Program code

The tryAssign function is to allow unused properties to go unfilled when instantiating the class objects. For some stupid reason, results from the NOAA API do not all have the exact same fields so without this function the code will break depending on how exactly the data from NOAA is formatted.

Trying to intepret the data from NOAA is hard, because it is not correctly formatted into JSON, or at least I couldnt figure it out and online JSON viewers dont like it
