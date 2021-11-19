using RaspberryPiGPIO.Models;
using System;

namespace RaspberryPiGPIO
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      RaspberryPi raspberryPi = new RaspberryPi();
      raspberryPi.AddEncoder(19, 26, "FreqCounter");
      
      while (true) {
        foreach(EncoderData e in raspberryPi.CheckEncoderState())
        {
          if (e.State != 2)
          {
            Console.WriteLine(e.State);
          }
        }
      }
    }
  }
}
