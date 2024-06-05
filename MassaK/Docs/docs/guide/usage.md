#  Usage

## Setup MassaK
This code creates MassaK Instance without connecting. 
It is recommended to subscribe to [OnStatusChanged](events.md#statuschanged) and [OnWeightChanged](events.md#onweightchanged) before using method `Connect()`

```csharp{4}
using MassaK.Plugin
using MassaK.Plugin.Imp;

IMassaK MassaK = new MassaUsb("COM6");
MassaK.OnStatusChanged += ReceiveStatus;
MassaK.OnWeightChanged += ReceiveWeight;
```

## Connect
This method is safely for use, no exceptions only event `StatusChanged`. 
```csharp
MassaK.Connect();
```

## Get weight
This method requests weight data from the scales. The argument specifies the interval in milliseconds between requests.
A smaller value means faster polling, with a minimum of 100 milliseconds (use cautiously).
Use [OnWeightChanged](events.md#onweightchanged) event
```csharp
MassaK.StartWeightPolling(100);
```

## Calibrate
Ensure there is no weight placed on the scales before connecting them via USB.
If any issues arise, reconnect the scales via USB.
```csharp
MassaK.Сalibrate();
```

## Disconnect
```csharp
MassaK.Disconnect();
```
After calling Disconnect, you can reconnect the object again that's why you need to call `Dispose()` for full destroy.
Or call only `Dispose()` without `Disconnect()`
```csharp
MassaK.Dispose();
```