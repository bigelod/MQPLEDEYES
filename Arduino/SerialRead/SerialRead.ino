#include <LedControl.h>

//Data can be sent as strings like 1,2,3


const int DIN_PIN = 11;
const int CS_PIN = 3;
const int CLK_PIN = 13;

const uint64_t IMAGES[] = {
  0x0000007e7e000000,
  0x0000181818180000,
  0x0000001818181800,
  0x0000000018181818,
  0x0000000000181818,
  0x0018181818000000,
  0x1818181800000000,
  0x1818180000000000,
  0x00000c0c0c0c0000,
  0x0000060606060000,
  0x0000030303030000,
  0x0000303030300000,
  0x0000606060600000,
  0x0000c0c0c0c00000,
  0x0000000c0c0c0c00,
  0x0000000006060606,
  0x0000000000030303,
  0x0000003030303000,
  0x0000000060606060,
  0x0000000000c0c0c0,
  0x000c0c0c0c000000,
  0x0606060600000000,
  0x0303030000000000,
  0x0030303030000000,
  0x6060606000000000,
  0xc0c0c00000000000,
  0x1824424242424200,
  0xe7dbbdbdbdbdbdff,
  0x4222121e22221e00,
  0xbdddede1dddde1ff,
  0x3c42424242423c00,
  0xc3bdbdbdbdbdc3ff,
  0x4262724a46464200,
  0xbd9d8db5b9b9bdff
};
const int IMAGES_LEN = sizeof(IMAGES)/8;

int currImgL = 0;
int currImgR = 0;

LedControl display = LedControl(DIN_PIN, CLK_PIN, CS_PIN,2);


void setup() {
  display.clearDisplay(0);
  display.clearDisplay(1);
  display.shutdown(0, false);
  display.shutdown(1, false);
  display.setIntensity(0, 10);
  display.setIntensity(1, 10);

  Serial.begin(9600); // you can set it to other baud rates also
}

void displayImageR(uint64_t image) {
  for (int i = 0; i < 8; i++) {
    byte row = (image >> i * 8) & 0xFF;
    for (int j = 0; j < 8; j++) {
      display.setLed(0, i, j, bitRead(row, j));
    }
  }
}

void displayImage(uint64_t image) {
  for (int i = 0; i < 8; i++) {
    byte row = (image >> i * 8) & 0xFF;
    for (int j = 0; j < 8; j++) {
      display.setLed(1, i, j, bitRead(row, j));
    }
  }
}

void loop() {
  displayImage(IMAGES[currImgL]);
  displayImageR(IMAGES[currImgR]);
  while (Serial.available()==0){}
  currImgL = Serial.parseInt();
  currImgR = Serial.parseInt();
}