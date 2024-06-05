# Events

## OnStatusChanged

### Statuses
MassaKStatus (enum)
- Ready
  - The device is successfully connected
- Initializing
  - The device is initializing
- Detached
  - The USB connection is lost, and an automatic reconnection attempt is in progress.
- Disabled
  - The device is not in use.
### Usage

```csharp{1}
MassaK.OnStatusChanged += ReceiveStatus;

private void ReceiveStatus(object? sender, MassaKStatus status)
{
    Status = status;
}
```

## OnWeightChanged

### WeightEventArg
WeightEventArg(class)
- Weight
  - <u>int field</u> of weight (after that / 1000 for normal using)
- IsStable
  - <u>bool field</u> indicate that weight is stable
```csharp{1}
MassaK.OnWeightChanged += ReceiveStatus;

private void ReceiveWeight(object? sender, WeightEventArg e)
{
    Weight = e;
}
```





