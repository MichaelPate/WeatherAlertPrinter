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

## Sample report from NOAA

Example result retuned from calling `n.active_alerts(zone_id=has2alerts)`

This has a few alerts in the main features block. Each alert can be identified by id, and is treated like a dictionary in python.

```
{
  '@context': [
    'https://geojson.org/geojson-ld/geojson-context.jsonld',
    {
      '@version': '1.1',
      'wx': 'https://api.weather.gov/ontology#',
      '@vocab': 'https://api.weather.gov/ontology#'
    }
  ],
  'type': 'FeatureCollection',
  'features': [
    {
      'id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.a963e9464d3dde969cbf984b47a3626f0ee62506.001.1',
      'type': 'Feature',
      'geometry': {
        'type': 'Polygon',
        'coordinates': [
          [
            [
              -108.28,
              39.19
            ],
            [
              -108.15,
              39.22
            ],
            [
              -108.16000000000001,
              39.17
            ],
            [
              -108.29,
              39.17
            ],
            [
              -108.28,
              39.19
            ]
          ]
        ]
      },
      'properties': {
        '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.a963e9464d3dde969cbf984b47a3626f0ee62506.001.1',
        '@type': 'wx:Alert',
        'id': 'urn:oid:2.49.0.1.840.0.a963e9464d3dde969cbf984b47a3626f0ee62506.001.1',
        'areaDesc': 'Debeque to Silt Corridor; Grand and Battlement Mesas',
        'geocode': {
          'SAME': [
            '008045',
            '008077',
            '008029'
          ],
          'UGC': [
            'COZ007',
            'COZ009'
          ]
        },
        'affectedZones': [
          'https://api.weather.gov/zones/forecast/COZ007',
          'https://api.weather.gov/zones/forecast/COZ009'
        ],
        'references': [
          {
            '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.2531cfbf97902f4b8542148a521496d13aa0569b.001.1',
            'identifier': 'urn:oid:2.49.0.1.840.0.2531cfbf97902f4b8542148a521496d13aa0569b.001.1',
            'sender': 'w-nws.webmaster@noaa.gov',
            'sent': '2023-06-01T14:51:00-06:00'
          },
          {
            '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.741cbebe3b85f99c43ff7122bf69094e2f5863b6.001.1',
            'identifier': 'urn:oid:2.49.0.1.840.0.741cbebe3b85f99c43ff7122bf69094e2f5863b6.001.1',
            'sender': 'w-nws.webmaster@noaa.gov',
            'sent': '2023-06-02T12:48:00-06:00'
          }
        ],
        'sent': '2023-06-02T12:55:00-06:00',
        'effective': '2023-06-02T12:55:00-06:00',
        'onset': '2023-06-02T12:55:00-06:00',
        'expires': '2023-06-04T13:00:00-06:00',
        'ends': None,
        'status': 'Actual',
        'messageType': 'Update',
        'category': 'Met',
        'severity': 'Severe',
        'certainty': 'Possible',
        'urgency': 'Future',
        'event': 'Flood Watch',
        'sender': 'w-nws.webmaster@noaa.gov',
        'senderName': 'NWS Grand Junction CO',
        'headline': 'Flood Watch issued June 2 at 12:55PM MDT by NWS Grand Junction CO',
        'description': '* WHAT...Minor flooding is possible.\n\n* WHERE...Plateau Creek from Collbran to the confluence with the\nColorado River near Cameo.\n\n* WHEN...Until further notice.\n\n* IMPACTS...At 7.5 feet, some lowland flooding can be expected along\nRoute 65.\n\n\n* ADDITIONAL DETAILS...\n- At 12:00 PM MDT Friday, June 2 the stage was 6.3 feet.\n- Forecast...Water levels are expected to remain slightly below\nbankfull conditions through the weekend. However, water\nlevels remain high around the town of Collbran.\n- Flood stage is 8.0 feet.\n- http://www.weather.gov/safety/flood',
        'instruction': None,
        'response': 'Prepare',
        'parameters': {
          'AWIPSidentifier': [
            'FFAGJT'
          ],
          'WMOidentifier': [
            'WGUS65 KGJT 021855'
          ],
          'NWSheadline': [
            'FLOOD WATCH REMAINS IN EFFECT UNTIL FURTHER NOTICE'
          ],
          'BLOCKCHANNEL': [
            'EAS',
            'NWEM',
            'CMAS'
          ],
          'EAS-ORG': [
            'WXR'
          ],
          'VTEC': [
            '/O.CON.KGJT.FL.A.0004.000000T0000Z-000000T0000Z/'
          ],
          'expiredReferences': [
            'w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.018934ccffa11611033aa0596eee088b69eb27f4.001.1,2023-05-31T12:06:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.baadb1d1fa5b8352f5f5a2ac0c0d96d8c6ca823f.001.1,2023-05-30T13:03:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.c2499f0633060202b440bf190b33e25c1501e05a.001.1,2023-05-30T12:49:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.04c04d404b504a370d72569abd969e66405b5cb3.001.1,2023-05-29T13:28:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.8af211aba696784b3d5294d5c097dd6f89ac6975.001.1,2023-05-28T13:32:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.2fb35470c2476bb9952c22f1c8c3d8241e6387f1.001.1,2023-05-28T01:39:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.c0640a2b35569356b64ad7f8a3b77f600b4c10ef.001.1,2023-05-27T03:20:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.55aa56aa31b6f5dda3ebbb8819c6d5ca64d1fc87.001.1,2023-05-28T08:36:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.4bd047f5e073696272d9505a4294dfad99b03e0f.001.1,2023-05-27T09:52:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.4c32f5100c381baf3719ed3f4c4b98979a985d73.001.1,2023-05-25T12:39:00-06:00'
          ]
        }
      }
    },
    {
      'id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.af9226635703ba1bdca274ab1f06822cabcb7423.001.1',
      'type': 'Feature',
      'geometry': {
        'type': 'Polygon',
        'coordinates': [
          [
            [
              -107.74,
              38.79
            ],
            [
              -107.72,
              38.8
            ],
            [
              -107.7,
              38.82
            ],
            [
              -107.68,
              38.83
            ],
            [
              -107.66000000000001,
              38.85
            ],
            [
              -107.64000000000001,
              38.86
            ],
            [
              -107.62000000000002,
              38.85
            ],
            [
              -107.64000000000001,
              38.84
            ],
            [
              -107.66000000000001,
              38.830000000000005
            ],
            [
              -107.68,
              38.82000000000001
            ],
            [
              -107.71000000000001,
              38.790000000000006
            ],
            [
              -107.74000000000001,
              38.78000000000001
            ],
            [
              -107.74,
              38.79
            ]
          ]
        ]
      },
      'properties': {
        '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.af9226635703ba1bdca274ab1f06822cabcb7423.001.1',
        '@type': 'wx:Alert',
        'id': 'urn:oid:2.49.0.1.840.0.af9226635703ba1bdca274ab1f06822cabcb7423.001.1',
        'areaDesc': 'Delta, CO',
        'geocode': {
          'SAME': [
            '008029'
          ],
          'UGC': [
            'COC029'
          ]
        },
        'affectedZones': [
          'https://api.weather.gov/zones/county/COC029'
        ],
        'references': [
          {
            '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.5b3295a37763325fae70a391cd5265a3e7629f0a.001.1',
            'identifier': 'urn:oid:2.49.0.1.840.0.5b3295a37763325fae70a391cd5265a3e7629f0a.001.1',
            'sender': 'w-nws.webmaster@noaa.gov',
            'sent': '2023-05-28T13:28:00-06:00'
          }
        ],
        'sent': '2023-05-30T14:45:00-06:00',
        'effective': '2023-05-30T14:45:00-06:00',
        'onset': '2023-05-30T14:45:00-06:00',
        'expires': '2023-06-06T18:00:00-06:00',
        'ends': '2023-06-06T18:00:00-06:00',
        'status': 'Actual',
        'messageType': 'Update',
        'category': 'Met',
        'severity': 'Minor',
        'certainty': 'Likely',
        'urgency': 'Expected',
        'event': 'Flood Advisory',
        'sender': 'w-nws.webmaster@noaa.gov',
        'senderName': 'NWS Grand Junction CO',
        'headline': 'Flood Advisory issued May 30 at 2:45PM MDT until June 6 at 6:00PM MDT by NWS Grand Junction CO',
        'description': '* WHAT...Hydrologic flooding caused by snowmelt continues.\n\n* WHERE...A portion of the North Fork Gunnison River through Delta\nCounty.\n\n* WHEN...Until further notice.\n\n* IMPACTS...Minor flooding in low-lying and poor drainage areas.\nRiver or stream levels are elevated, and fast flow will continue\nto carve and erode curved banks along the channel.\n\n* ADDITIONAL DETAILS...\n- At 240 PM MDT, emergency management reported flooding in the\nadvisory area, mainly in the form of significant erosion of\nbanks along the river.\n\n- This includes the following streams and drainages...\nNorth Fork Gunnison River\n\n- Some locations that will experience flooding include...\nremote areas near Hotchkiss and Paonia.\n\n- Http://www.weather.gov/safety/flood',
        'instruction': "Turn around, don't drown when encountering flooded roads. Most flood\ndeaths occur in vehicles.\n\nPlease report observed flooding to local emergency services or law\nenforcement and request they pass this information to the National\nWeather Service when you can do so safely.",
        'response': 'Avoid',
        'parameters': {
          'AWIPSidentifier': [
            'FLSGJT'
          ],
          'WMOidentifier': [
            'WGUS85 KGJT 302045'
          ],
          'NWSheadline': [
            'FLOOD ADVISORY FOR SNOWMELT REMAINS IN EFFECT UNTIL FURTHER NOTICE'
          ],
          'BLOCKCHANNEL': [
            'EAS',
            'NWEM',
            'CMAS'
          ],
          'VTEC': [
            '/O.EXT.KGJT.FA.Y.0015.000000T0000Z-230607T0000Z/'
          ],
          'eventEndingTime': [
            '2023-06-07T00:00:00+00:00'
          ],
          'expiredReferences': [
            'w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.a5bf1ca9759ac23d8d01c22d395620b4c7d8b86c.001.1,2023-05-25T13:37:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.aff76b1e86684fdb1c732c507610e7b52b613259.001.1,2023-05-21T15:38:00-06:00'
          ]
        }
      }
    },
    {
      'id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.298fc3659491683abf94480852999630456f650a.001.1',
      'type': 'Feature',
      'geometry': {
        'type': 'Polygon',
        'coordinates': [
          [
            [
              -107.91,
              38.99
            ],
            [
              -107.88,
              38.99
            ],
            [
              -107.96,
              38.77
            ],
            [
              -107.99,
              38.78
            ],
            [
              -107.91,
              38.99
            ]
          ]
        ]
      },
      'properties': {
        '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.298fc3659491683abf94480852999630456f650a.001.1',
        '@type': 'wx:Alert',
        'id': 'urn:oid:2.49.0.1.840.0.298fc3659491683abf94480852999630456f650a.001.1',
        'areaDesc': 'Delta, CO',
        'geocode': {
          'SAME': [
            '008029'
          ],
          'UGC': [
            'COC029'
          ]
        },
        'affectedZones': [
          'https://api.weather.gov/zones/county/COC029'
        ],
        'references': [
          {
            '@id': 'https://api.weather.gov/alerts/urn:oid:2.49.0.1.840.0.befd9769e866ceef6dd9e340cf60da3cec46af0f.001.1',
            'identifier': 'urn:oid:2.49.0.1.840.0.befd9769e866ceef6dd9e340cf60da3cec46af0f.001.1',
            'sender': 'w-nws.webmaster@noaa.gov',
            'sent': '2023-05-28T13:30:00-06:00'
          }
        ],
        'sent': '2023-05-30T12:58:00-06:00',
        'effective': '2023-05-30T12:58:00-06:00',
        'onset': '2023-05-30T12:58:00-06:00',
        'expires': '2023-06-06T18:00:00-06:00',
        'ends': '2023-06-06T18:00:00-06:00',
        'status': 'Actual',
        'messageType': 'Update',
        'category': 'Met',
        'severity': 'Minor',
        'certainty': 'Likely',
        'urgency': 'Expected',
        'event': 'Flood Advisory',
        'sender': 'w-nws.webmaster@noaa.gov',
        'senderName': 'NWS Grand Junction CO',
        'headline': 'Flood Advisory issued May 30 at 12:58PM MDT until June 6 at 6:00PM MDT by NWS Grand Junction CO',
        'description': '* WHAT...Hydrologic flooding caused by snowmelt continues.\n\n* WHERE...Along Surface Creek to the confluence with the Gunnison\nRiver.\n\n* WHEN...Until further notice.\n\n* IMPACTS...Minor flooding in low-lying and poor drainage areas.\nRiver or stream flows are elevated due to high flows from snowmelt.\n\n* ADDITIONAL DETAILS...\n- At 1256 PM MDT, gage reports indicated high flows due to\naccelerated snowmelt off the Grand Mesa. Minor flooding is\nalready occurring in the advisory area.\n- This includes the following streams and drainages...\nSurface Creek\n- Some locations that will experience flooding include...\nOrchard City, Cedaredge and Eckert.\n- http://www.weather.gov/safety/flood',
        'instruction': "Turn around, don't drown when encountering flooded roads. Most flood\ndeaths occur in vehicles.\n\nBe especially cautious at night when it is harder to recognize the\ndangers of flooding.\n\nStay away or be swept away. River banks and culverts can become\nunstable and unsafe.",
        'response': 'Avoid',
        'parameters': {
          'AWIPSidentifier': [
            'FLSGJT'
          ],
          'WMOidentifier': [
            'WGUS85 KGJT 301858'
          ],
          'NWSheadline': [
            'FLOOD ADVISORY FOR SNOWMELT REMAINS IN EFFECT UNTIL FURTHER NOTICE'
          ],
          'BLOCKCHANNEL': [
            'EAS',
            'NWEM',
            'CMAS'
          ],
          'VTEC': [
            '/O.EXT.KGJT.FA.Y.0014.000000T0000Z-230607T0000Z/'
          ],
          'eventEndingTime': [
            '2023-06-07T00:00:00+00:00'
          ],
          'expiredReferences': [
            'w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.c9ad8448bcf875914ac421035182a469ad802874.001.1,2023-05-26T14:23:00-06:00 w-nws.webmaster@noaa.gov,urn:oid:2.49.0.1.840.0.066cc005243dc11d4429dab3a5a6cb5a4b7a7c42.001.1,2023-05-19T19:49:00-06:00'
          ]
        }
      }
    }
  ],
  'title': 'Current watches, warnings, and advisories for Delta County (COC029) CO',
  'updated': '2023-06-03T05:09:00+00:00'
}
```

