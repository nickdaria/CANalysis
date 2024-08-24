# CANalysis
<img src="Resources/icon.png" width="10%">

Easy-to-use C# library and accompanying CLI utility for parsing, filtering, analysis, and exporting of CAN data

## Models
- `CANFrame`
- `CANLog`
- `UDSTransmission`

## Parsing
- File-based log loading from File stream
- Programatic frame intake from program

## Filtering
Lambda filters easily slim down to the desired data set for analysis
```C#
// Filtering by ID and data length
var filteredFrames = new CANFrameFilter()
    .ByID(id => id == 0x123 || id == 0x456)
    .ByLength(length => length == 2)
    .Apply(frames);
```

## Analysis
Various built-in analysis tools are provided for common use-case scenarios
- `IDComparisonBetweenLogs` finds IDs isolated to specific logs, and IDs shared among all logs
- `BitsChangedBetweenLogs` finds masks of bits that change between seperate logs, but ignores bits that are changing during the logs. Useful for narrowing the search for broadcast data that can be manipulated and seperated by logs such as gear.
- `ISOTPScan` finds frames that might match the ISO-TP standard (single-frame & multi-frame) and tries to interpret the full transmissions.

## Exporting
Export analysis results or filtered log file data to a file or as an object for a program to use.