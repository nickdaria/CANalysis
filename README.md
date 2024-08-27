<img src="Resources/icon.png" width="10%">

# CANalysis
Easy-to-use C# library for parsing, filtering, analysis, and export of CAN data.

> [!NOTE]  
> CANalysis is still undergoing early development. It is subject to change for upcoming features like DBC functionality.

## Parsing
CAN data can be fed into the library programatically, or via parsers for popular formats:
- SavvyCAN GVRET CSV

*more formats coming soon*

## Filtering
Lambda filters can be used to easily slim down to the desired data set for analysis
```C#
// Filtering by ID and data length
var filteredFrames = new CANFrameFilter()
    .ByID(id => id == 0x123 || id == 0x456)
    .ByLength(length => length == 2)
    .Apply(frames);
```

## Analysis
Various built-in analysis tools are provided for common use-case scenarios
<!-- - `IDComparisonBetweenLogs` finds IDs isolated to specific logs, and IDs shared among all logs
- `BitsChangedBetweenLogs` finds masks of bits that change between seperate logs, but ignores bits that are changing during the logs. Useful for narrowing the search for broadcast data that can be manipulated and seperated by logs such as gear. -->
- `ISOTP.ReadTransmissions` identifies and parses all valid ISO-TP communications
    - Single-frame & multi-frame
    - Ordered by first frame, can handle multiple concurrent transmissions
    - *It is suggested to filter input to known UDS/ISO-TP IDs to prevent false positives*

*more broadcast bit-based tools coming soon*

## Export
Parsed or filtered/analyzed output can be used programatically, or exported to a file in any format that CANalysis can parse. If no operations are made, this means CANalysis can effecively be used as a file format converter.