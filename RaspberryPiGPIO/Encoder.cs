using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPiGPIO
{
  public class Encoder
  {
    public string Name { get; private set; }
    public string Id { get; private set; }
    private int Dt { get; set; }
    private int Clk { get; set; }
    public bool hasRotated { get; private set; }
    public int EncoderState { get; private set; }
    private GpioController controller = null;
    public Encoder(int dt, int clk, GpioController inController, string name = "No Name Given")
    {
      Id = Guid.NewGuid().ToString(); 
      Dt = dt;
      Clk = clk;
      controller = inController;
      Name = (name == "") ? "No Name Given" : name;

    }   

    /// <summary>
    /// Set an event listener on clock pin to monitor the Encoder for changes
    /// </summary>
    public void Monitor()
    {
      controller.OpenPin(Clk);
      controller.OpenPin(Dt);
      controller.SetPinMode(Clk, PinMode.InputPullUp);
      controller.SetPinMode(Dt, PinMode.InputPullUp);

      controller.RegisterCallbackForPinValueChangedEvent(Clk, PinEventTypes.Falling, OnPinFall);    
    }

    /// <summary>
    /// Callback method for handling Encoder Rotation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnPinFall(object sender, PinValueChangedEventArgs args)
    {
      if ((int)controller.Read(Clk) == 0 && (int)controller.Read(Dt) == 0)
      {
        Console.WriteLine("Rotation Clockwise");
        hasRotated = true;
        EncoderState = 1;
      }
      else if ((int)controller.Read(Clk) == 0 && (int)controller.Read(Dt) == 1)
      {
        Console.WriteLine("Rotation Counter-Clockwise");
        hasRotated = true;
        EncoderState = 0;
      }
    }

    public int CheckEncoder()
    {
      // 0 counter-clockwise 1 clockwise 2 no change in position
      int stateEncoder = EncoderState;
      if (EncoderState != 2) EncoderState = 2;      
      return stateEncoder;
    }

    /// <summary>
    /// Closes the Encoder Pins and Removes EventListener
    /// </summary>

    public bool StopMonitor()
    {
      try
      {
        controller.ClosePin(Clk);
        controller.ClosePin(Dt);
        controller.UnregisterCallbackForPinValueChangedEvent(Clk, OnUnregister);
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }
    private void OnUnregister(object sender, PinValueChangedEventArgs args)
    {
      Console.WriteLine($"Pin {args.PinNumber} No longer tracked");
    }
  }
}
