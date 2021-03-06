/* Space Mall device code
 *  Whaaat!? Lab 2022 Spring Project
 */
 
#include <Encoder.h>
//   avoid using pins with LEDs attached


// knob 1
Encoder myEncKnob1(4, 14);

// knob 2
Encoder myEncKnob2(5, 15);

// knob 3
Encoder myEncKnob3(6, 16);

long oldPosition1  = -999;
long oldPosition2  = -999;
long oldPosition3  = -999;

// Crank
int crank = A4;
int crankInput = 0;
int crankThresholds[] = {0, 5, 12, 20, 30, 40, 50, 60, 70, 80, 90, 95, 105};

// Channel Knob
int channelChanger = A3;
int channelInput[] = {0, 0, 0, 0, 0};
int channelCounter = 5;
float channelAverage = 0;
int channelThresholds[] = {0, 5, 13, 20, 30, 40, 50, 58, 70, 80, 87, 95, 105};
char channelCodes[] = {'q','w','e','r','t','y','a','s','d','f','g','h'};
int currChannel = -999;

// Microphone Button
const int micButton = 7;
int micButtonState = 0;
bool micCheck = false;

// Device Button
const int deviceButton = 8;
int deviceButtonState = 0;
bool dbutCheck = false;

void setup() {
  Serial.begin(9600);
  Serial.println("Basic Encoder Test:");

  // Set up inputs
  pinMode(micButton, INPUT_PULLUP);
  pinMode(deviceButton, INPUT_PULLDOWN);
}


void loop() {
  

  // channel

  for(int i=0; i<sizeof(channelCounter); i++){
    channelInput[i] = analogRead(channelChanger);
    channelAverage += channelInput[i];
  }

  channelAverage = channelAverage / channelCounter;

  Serial.println(channelAverage);

  for(int i=0; i<sizeof(channelCodes); i++){
     if((channelAverage >= channelThresholds[i]) && (channelAverage < channelThresholds[i+1])){
        if(currChannel != i){
          Keyboard.write(channelCodes[i]);
          currChannel = i;
        }
     }
  }
  
  
  // knob 1 update
  long newPosition1 = myEncKnob1.read();
  if (newPosition1 != oldPosition1) {
    if(newPosition1 < oldPosition1) {
      Keyboard.write('m');
      //Keyboard.println(KEY_RIGHT);
      //Keyboard.press(KEY_LEFT);
    }
    else if((newPosition1 > oldPosition1)) {
      Keyboard.write('n');
      //Keyboard.println(KEY_LEFT);
    }
    oldPosition1 = newPosition1;
  }

  // knob 2 update
  long newPosition2 = myEncKnob2.read();
  if (newPosition2 != oldPosition2) {
    if(newPosition2 < oldPosition2) {
      //Keyboard.println(KEY_UP);
      Keyboard.write('l');
    }
    else if((newPosition2 > oldPosition2)) {
      //Keyboard.println(KEY_DOWN);
      Keyboard.write('k');
    }
    delay(5);
    oldPosition2 = newPosition2;
  }

  // knob 3 update
  long newPosition3 = myEncKnob3.read();
  if (newPosition3 != oldPosition3) {
    if(newPosition3 < oldPosition3) {
      Keyboard.write('p');
    }
    else if((newPosition3 > oldPosition3)) {
      Keyboard.write('o');
    }
    oldPosition3 = newPosition3;
  }

  // crank Update
  long newCrankInput = analogRead(crank); 

  // is it far enough away from the current input (to avoid fluttering)
  if (newCrankInput > crankInput + 5 || newCrankInput < crankInput-5) {
    if(newCrankInput < crankInput-5) {
      Keyboard.write('p');
    }
    else if((newCrankInput > crankInput+5)) {
      Keyboard.write('o');
    }
    crankInput = newCrankInput;
  }

  // Mic Button
  micButtonState = digitalRead(micButton);
  if (micButtonState == LOW) {
    Keyboard.write('z');
  } 

  // Device Button
  deviceButtonState = digitalRead(deviceButton);
  if (deviceButtonState == HIGH) {
    if(!dbutCheck) {
      Keyboard.write('x');
      dbutCheck = true;
    }
  } else {
    dbutCheck = false;
  }

  delay(5);
  
}
