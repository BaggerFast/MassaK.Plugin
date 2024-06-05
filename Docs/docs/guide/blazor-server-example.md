﻿# Examples
To see how the library works, you can run the blazor application from the official [repository](https://github.com/VladStandard/TscZebra.Plugin/tree/main/Clients/PrinterUI)

```csharp
@page "/"
@using System.Net
@using TscZebra.Plugin
@using TscZebra.Plugin.Abstractions
@using TscZebra.Plugin.Abstractions.Enums
@using TscZebra.Plugin.Abstractions.Exceptions

@implements IDisposable

<div class="w-full flex gap-2 pt-2 items-center justify-center">
  <Button OnClick="@Connect">
    Connect
  </Button>
  <Button OnClick="@Disconnect">
    Disconnect
  </Button>
  <Button OnClick="@GetStatus">
    GetStatusByHand
  </Button>
  <Button>
    Print
  </Button>
</div>
<div class="w-full pt-4 flex flex-col items-center justify-center">
  <p class="text-2xl">StatusByEvent = @Status</p>
  <p class="text-2xl">StatusByHande = @StatusByHand</p>
</div>


@code {
    IZplPrinter Printer { get; set; } = 
        PrinterFactory.Create(IPAddress.Parse("10.0.22.71"), 9100, PrinterTypes.Tsc);
    PrinterStatuses Status { get; set; }= PrinterStatuses.IsDisconnected;
    PrinterStatuses StatusByHand { get; set; }= PrinterStatuses.IsDisconnected;
    
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender) Printer.PrinterStatusChanged += Receive;
        base.OnAfterRender(firstRender);
    }
    
    private async Task Connect()
    {
        try
        {
        await Printer.ConnectAsync();
        Printer.StartStatusPolling(5);
        } catch (PrinterConnectionException) {
            
        } 
    }
    
    private void Disconnect()
    {
        Printer.Disconnect();
    }
    
    private async Task GetStatus()
    {
        try
        {
            StatusByHand = await Printer.RequestStatusAsync();
        } catch (PrinterConnectionException) {
            
        } 
    }
    
    private void Receive(object? sender, PrinterStatuses statuses)
    {
        Status = statuses;
        InvokeAsync(StateHasChanged);
    }
    
    public void Dispose()
    {
        Printer.Dispose();
        Printer.PrinterStatusChanged -= Receive;
    }
}
```