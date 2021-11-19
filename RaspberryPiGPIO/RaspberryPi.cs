using Newtonsoft.Json;
using RaspberryPiGPIO.Models;
using System;
using System.Collections.Generic;
using System.Device.Gpio;

namespace RaspberryPiGPIO
{
  public class RaspberryPi
  {
    public GpioController Controller { get; set; }
    public List<Encoder> EncoderList { get; private set; }

    private List<int[]> pinsUsed = new List<int[]>();

    public RaspberryPi(bool noRasp = false)
    {
      EncoderList = new List<Encoder>();
      Console.WriteLine("setGPIO:" + noRasp);
      if (!noRasp)
      {      
        Controller = new GpioController();       
      }
    }    
    /// <summary>
    /// Add a new Encoder and begin monitoring
    /// </summary>
    /// <param name="clk"></param>
    /// <param name="dt"></param>
    /// <param name="name"></param>
    public void AddEncoder(int clk, int dt, string name = "")
    {
      pinsUsed.Add(new int[] { clk, dt });
      Encoder encoder = new Encoder(clk, dt, Controller, name);
      encoder.Monitor();
      EncoderList.Add(encoder);
    }
    public List<EncoderData> CheckEncoderState()
    {
      List<EncoderData> result = new List<EncoderData>();
      foreach (Encoder e in EncoderList)
      {
        result.Add(new EncoderData
        {
          Id = e.Id,
          Name = e.Name,
          State = e.CheckEncoder()
        });
      }
      return result;
    }

    /// <summary>
    /// Closes all Event Monitors and Pins
    /// </summary>
    public void ClosePins()
    {
      Console.WriteLine("Stopping Encoder Monitors");
      foreach(Encoder e in EncoderList)
      {
        e.StopMonitor();
      }
    }
  }
}
