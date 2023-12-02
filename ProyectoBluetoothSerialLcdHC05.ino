#include <LiquidCrystal_I2C.h>
#include <SoftwareSerial.h>
#include <Wire.h>

SoftwareSerial BT(10, 11);
LiquidCrystal_I2C lcd(0x27,16,2);

const int pinLED2 =5;
const int pinBoton = 2;
const int pinLED = 12;
unsigned long tiempoUltimoPulsador = 0;
const int tiempoEspera = 2000;  // 2000 milisegundos (2 segundos)
void setup() {
  
  lcd.init();
  lcd.backlight();
  lcd.setCursor(0, 0);
  BT.begin(9600);
  Serial.begin(9600);
  Serial.println("Conexion exitosa");
  lcd.print("Bienvenido");
  delay(2000);
  lcd.clear();
  pinMode(pinLED, OUTPUT);
  pinMode(pinLED2, OUTPUT);
  pinMode(pinBoton, INPUT_PULLUP);
}

void loop() {
  // Verificar si el botón está presionado
  if (digitalRead(pinBoton) == LOW) {
    lcd.clear();
    lcd.print("Pulsador");
    Serial.println("Boton Presionado");
    tiempoUltimoPulsador = millis();

    // Encender el LED cuando se presiona el botón
    digitalWrite(pinLED, HIGH);

    // Enviar mensaje por Bluetooth
    BT.println("Boton");
  } else {
    // Apagar el LED si el botón no está presionado
    digitalWrite(pinLED, LOW);
  }

  // Verificar si hay datos disponibles por Bluetooth
  if (BT.available()) {
  String receivedMessage = "";  // Cadena para almacenar el mensaje recibido

  // Leer caracteres hasta que no haya más datos disponibles o hasta que llegue un carácter de nueva línea ('\n')
  while (BT.available()) {
    // Controlar el LED según el mensaje recibido
    if (receivedMessage == "A") {
      
      digitalWrite(pinLED2, HIGH); 
      lcd.print("LED2 encendida:");
      BT.println("LED2 encendida BT");
    } else if (receivedMessage == "B") {
      digitalWrite(pinLED2, LOW);   // Apagar el LED
      lcd.print("LED2 apagada:");
      BT.println("LED2 apagada BT");  
    }
    char receivedChar = BT.read();
    if (receivedChar == '\n') {
      break;  // Romper el bucle al encontrar el carácter de nueva línea
    }
    receivedMessage += receivedChar;
  }
    lcd.print(receivedMessage);
    delay(2000);  // Esperar 2 segundos antes de borrar el mensaje
    lcd.clear();
  }

  // Verificar si ha pasado el tiempo de espera desde el último pulsador
  if (millis() - tiempoUltimoPulsador >= tiempoEspera) {
    lcd.clear();
  }

if (Serial.available()) {
    String Message = "";  // Cadena para almacenar el mensaje recibido

    // Leer caracteres hasta que no haya más datos disponibles o hasta que llegue un carácter de nueva línea ('\n')
    while (Serial.available()) {
        // Leer un carácter como cadena
        String receivedChar = Serial.readStringUntil('\n');

        // Controlar el LED según el mensaje recibido
        if (receivedChar == "A") {
            digitalWrite(pinLED2, HIGH); 
            lcd.print("LED2 encendida:");
            BT.println("LED2 encendida S");
        } else if (receivedChar == "B") {
            digitalWrite(pinLED2, LOW);   // Apagar el LED
            lcd.print("LED2 apagada:");
            BT.println("LED2 apagada S");
        }

        // Agregar el carácter al mensaje
        Message += receivedChar;
    }

    lcd.print(Message);
    delay(2000);  // Esperar 2 segundos antes de borrar el mensaje
    lcd.clear();
}
}
