from noaa_sdk import NOAA
import json
from escpos.connections import getUSBPrinter
import time

def tryAssign(success):
    try:
        return success()
    except:
        return "empty."
        
class Alert:
    def __init__(self, featureBlock):
        self.raw = featureBlock
        self.properties =   tryAssign(lambda: self.raw['properties'])

        self.ID =           tryAssign(lambda: self.properties['id'])
        self.areaDesc =     tryAssign(lambda: self.properties['areaDesc'])
        self.sent =         tryAssign(lambda: self.properties['sent'])
        self.effective =    tryAssign(lambda: self.properties['effective'])
        self.onset =        tryAssign(lambda: self.properties['onset'])
        self.expires =      tryAssign(lambda: self.properties['expires'])
        self.ends =         tryAssign(lambda: self.properties['ends'])
        self.status =       tryAssign(lambda: self.properties['status'])
        self.messageType =  tryAssign(lambda: self.properties['messageType'])
        self.category =     tryAssign(lambda: self.properties['category'])
        self.severity =     tryAssign(lambda: self.properties['severity'])
        self.certainty =    tryAssign(lambda: self.properties['certainty'])
        self.urgency =      tryAssign(lambda: self.properties['urgency'])
        self.event =        tryAssign(lambda: self.properties['event'])
        self.sender =       tryAssign(lambda: self.properties['sender'])
        self.senderName =   tryAssign(lambda: self.properties['senderName'])
        self.headline =     tryAssign(lambda: self.properties['headline'])
        self.description =  tryAssign(lambda: self.properties['description'])
        self.instruction =  tryAssign(lambda: self.properties['instruction'])
        self.response =     tryAssign(lambda: self.properties['response'])
        
        self.parameters =       tryAssign(lambda: self.properties['parameters'])
        self.AWIPSidentifier =  str(tryAssign(lambda: self.parameters['AWIPSidentifier']))
        self.WMOidentifier =    str(tryAssign(lambda: self.parameters['WMOidentifier']))
        self.NWSheadline =      str(tryAssign(lambda: self.parameters['NWSheadline']))
        self.VTEC =             str(tryAssign(lambda: self.parameters['VTEC']))
        
    def __eq__(self, other):
        return self.ID == other.ID and self.properties == other.properties

def boldLine(printer, text):
    printer.doubleStrike()
    printer.text(text)
    printer.doubleStrike(False)
    printer.lf()
    
def sizeLine(printer, text, height, width):
    printer.textSize(height, width)
    printer.text(text)
    printer.textSize(1, 1)
    printer.lf()
    
def sizeBoldLine(printer, text, height, width):
    printer.textSize(height, width)
    printer.doubleStrike()
    printer.text(text)
    printer.doubleStrike(False)
    printer.textSize(1, 1)
    printer.lf()

def printAlert(printer, alert):
    printer.lf()
    sizeBoldLine(printer, "## NEW ALERT ##", 2, 3)
    sizeBoldLine(printer, alert.event, 2, 3)
    printer.lf()
    printer.lf()
    printer.text("SENT: " + alert.sent)
    printer.lf()
    printer.text("EFFECTIVE: " + alert.effective)
    printer.lf()
    printer.text("EXPIRES: " + alert.expires)
    printer.lf()
    printer.lf()
    sizeLine(printer, "SEVERITY: " + alert.severity, 2, 3)
    printer.text("STATUS: " + alert.status)
    printer.lf()
    printer.text("AREA: " + alert.areaDesc)
    printer.lf()
    printer.lf()
    printer.text("HEADLINE: " + alert.headline)
    printer.lf()
    printer.lf()
    printer.text("DESCRIPTION: " + alert.description)
    printer.lf()
    printer.lf()
    printer.text("INSTRUCTIONS: " + alert.instruction)
    printer.lf()
    printer.lf()
    printer.text("SENT BY: " + alert.senderName + " VIA " + alert.sender)
    printer.lf()
    printer.text("STATION ID: " + alert.WMOidentifier)
    printer.lf()
    printer.text("AWIPS: " + alert.AWIPSidentifier)
    printer.lf()
    printer.text("VTEC: " + alert.VTEC)
    printer.lf()
    printer.lf()
    printer.lf()
    printer.lf()
    printer.lf()
    printer.cutPaper(cut='full')
    printer.drawerKickPulse()
    
    



has2alerts = 'COC001'
boulderCounty = 'COC013'

vendor = 0x0fe6
product = 0x811e
inputPoint = 0x81
outputPoint = 0x03

def main():
    p = getUSBPrinter(commandSet='Generic')(idVendor=vendor,idProduct=product,inputEndPoint=inputPoint,outputEndPoint=outputPoint)
    n = NOAA()
    
    alerts = []
    incomingAlerts = []
    
    # Main program loop
    while True:
        res = n.active_alerts(zone_id=has2alerts)
        
        # Iterate through NOAA results to see if there are new alerts
        for block in res['features']:
        
            incomingAlert = Alert(block)
            incomingAlerts.append(incomingAlert)
            
            if incomingAlert in alerts:
                # do nothing
                print("Skipping.")
            else:
                # print it
                alerts.append(incomingAlert)
                print("NEW ALERT")
                print(incomingAlert.event)
                printAlert(p, incomingAlert)
                
            
        print('End of alerts')
        
        # remove any previously stored alerts that are no longer coming from NOAA
        for alert in alerts:
            if alert not in incomingAlerts:
                alerts.remove(alert)
        
        # empty the queue of incoming alerts
        for alert in incomingAlerts:
            incomingAlerts.remove(alert)
        
        time.sleep(60 * 1)

if __name__=='__main__':
    main()