# Monster Smart Lighting UDP Example
I recently picked up a "Monster Smart Lighting Smart RGB+IC Color Flow Light Bar Pair" which uses an ESP chipset running the Ayla Networks IoT stack.

Poking around in wireshark and at the windows app that monster provides yields the following project for direct programmatic control.

The device calls itself a "Xtreme RGBIC 1.2" in the mobile app, and is stamped with the following marks: MNW7-2024-ICM (FCCID: 2AHAS-MLW71003)

The software version of the one I am using is: led_rgbic 2.0.19 2024-03-18 09:34:45 a9f50fcb

In the app you will need the "DSN" to make any requests to this device. It is used like an API key to secure it.

Future testing will be on the bluetooth protocol used for pairing and initial configuration.
