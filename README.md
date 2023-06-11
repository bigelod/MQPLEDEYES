# MQPLEDEYES
A fun little project to see what it'd be like if you could "see" where someone is looking with their Meta Quest Pro from the outside

------
NOTES:
------

Hacked together by Daven Bigelow, released Public Domain / "Unlicense"

This project uses a simulated VR app that lets the user toggle between "VR Mode" or "Passthrough" without actually changing anything in-app, it will show a preview of what the LED displays would show if you don't have them hooked up IRL as well as some debug data

To do something like this "for real" would probably require direct access given by Meta to the eye tracking data for a background Android app to pass alongside COM OTG or Bluetooth BLE to an Arduino, which seems unlikely to happen any time soon, so this is just a toy / hobby project!

Technically, a smartphone could just display the "fake eyes" you see in the scene and be strapped to the front of a Quest, but I wanted to make it a hardware journey too (I hadn't used a soldering iron in like a decade), but this would still need to become either some third party SDK for other devs to implement, or unlikely data exposure by Meta to background apps.

Further Ideas: It might still be useful to see if someone is in VR or PASSTHROUGH with non-eye tracked devices like Quest 1,2,etc. (You can't see WHERE they are looking, but you can see a general hint *if* they can see you or not)

------------------
Project files:
------------------

1. Arduino project (with 33 baked-in animation frames)

2. Unity project (modified version of Movement SDK from Meta)

3. C# project (VS2017, for UDP to COM talking to Arduino)


------------------
Hardware used:
------------------

1x Arduino Uno R3
2x Generic 8x8 LED matrix devices
1x Quest Pro
1x Windows PC to communicate with the Arduino

------------------
Further credits:
------------------

Meta Movement SDK: https://github.com/oculus-samples/Unity-Movement

Arduino Code: https://docs.arduino.cc/built-in-examples/communication/ReadASCIIString
and https://www.circuitbasics.com/how-to-setup-an-led-matrix-on-the-arduino/
and https://xantorohara.github.io/led-matrix-editor/

Networking Code: https://stackoverflow.com/questions/53731293/sending-udp-calls-in-unity-on-android

Serial COM Code: https://www.instructables.com/C-Serial-Communication-With-Arduino/
