
# Dadrass.Dev.Cron

A .NET library that provides a flexible cron job scheduling system, allowing jobs to run on specified patterns. This package includes utility classes to define complex timing patterns and execute tasks at intervals specified by the user.

---

## Features

- **Flexible Cron Scheduling**: Define schedules with detailed timing intervals, including milliseconds, seconds, minutes, hours, and days.
- **Event-Driven**: Utilize an event-based approach for executing tasks at each scheduled tick.
- **Utility Functions**: Functional-style utilities for flexible transformations within the library.

## Installation

To install the package via NuGet, run the following command:

```bash
dotnet add package Dadrass.Dev.Cron
```

## Usage

### 1. Define and Start a Cron Job

Define a `CronJob` with a specific schedule pattern. The format for patterns is `hh:mm:ss m s M h d`, where each element represents a time interval.

Example pattern:
- `"00:00:00 0 5 0 0 0"` runs the job every 5 seconds, starting at midnight.

```csharp
using Dadrass.Dev.Cron;
using Dadrass.Dev.Cron.Events;

var cronJob = new CronJob("00:00:00 0 5 0 0 0"); // Starts at midnight, then every 5 seconds
cronJob.Tick += (sender, e) =>
{
    Console.WriteLine($"Tick {e.Sequence} - Pattern: {e.Pattern}");
};

cronJob.Start();
```

To stop the job at any time, use the `Stop()` method:

```csharp
cronJob.Stop();
```

### 2. Understanding the Pattern Format

The cron pattern allows you to set the following intervals:
- **hh:mm:ss** - The start time for the cron job.
- **m** - Milliseconds interval.
- **s** - Seconds interval.
- **M** - Minutes interval.
- **h** - Hours interval.
- **d** - Days interval.

### 3. Utility Functions with WrapperUtilities

The `WrapperUtilities` class provides a simple functional-style helper to transform objects, making it easier to work with intermediate results inline.

Example usage:
```csharp
using Dadrass.Dev.Cron.Utilities;

int transformedValue = 5._(x => x * 2); // Result: 10
```

---

## API Reference

### CronJob Class

Represents a customizable cron job that executes at intervals defined by a schedule pattern.

- **Pattern Syntax**: `"hh:mm:ss m s M h d"`
    - Example: `"00:00:00 0 5 0 0 0"` starts at midnight and repeats every 5 seconds.
- **Events**:
    - **Tick**: Fires on each scheduled tick, passing `TickEventArgs` which includes the pattern and sequence.

| Method       | Description                   |
|--------------|-------------------------------|
| `Start()`    | Starts the cron job.          |
| `Stop()`     | Stops the cron job.           |

### TimePattern Class

Defines the parsed pattern values for different intervals. Used internally by `CronJob` to determine timing.

| Property     | Description                              |
|--------------|------------------------------------------|
| `BaseWait`   | Initial wait time based on the start time |
| `Millisecond`| Interval in milliseconds                |
| `Second`     | Interval in seconds                     |
| `Minute`     | Interval in minutes                     |
| `Hour`       | Interval in hours                       |
| `Day`        | Interval in days                        |

### TickEventHandler Delegate

Defines the method signature for handling the `Tick` event.

| Parameter | Description |
|-----------|-------------|
| `sender`  | The source of the event, typically the `CronJob` instance. |
| `e`       | `TickEventArgs` containing the pattern and sequence. |

### TickEventArgs Class

Provides data for the `Tick` event, including the pattern and sequence number.

| Property   | Description                         |
|------------|-------------------------------------|
| `Pattern`  | The cron schedule pattern string.   |
| `Sequence` | The current tick count since start. |

### WrapperUtilities Class

Utility class for functional transformations.

| Method          | Description                                      |
|-----------------|--------------------------------------------------|
| `_`             | Transforms an object using a specified function. |

---

## Example Project

Refer to the `Examples` folder for a comprehensive example on using the `Dadrass.Dev.Cron` library with various patterns and advanced event handling.

---

## Contributing

We welcome contributions! Please submit issues and pull requests to improve the library.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
