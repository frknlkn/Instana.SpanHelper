# Instana.SpanHelper

Instana.SpanHelper is a .NET Framework 4.8 NuGet package that extends the Instana.ManagedTracing.Sdk functionality by providing a set of helper methods for creating and managing spans. This package simplifies working with Instana’s tracing capabilities by offering easy-to-use methods for starting new traces, creating intermediate spans, and handling external (exit) spans, along with built-in error and logging support.

## Table of Contents

- Features
- Installation
- Usage
  - Creating Spans
    - Starting a New Trace (Entry Span)
    - Creating an Intermediate Span
    - Creating an Exit Span
  - Error Handling & Logging
- Contributing
- License
- Repository

## Features

- **Entry Span:** Start a new trace (root span) for an operation.
- **Intermediate Span:** Create a span within an existing trace to represent a step in your process.
- **Exit Span:** Represent external calls (e.g., API calls) with a dedicated exit span.
- **Error Handling Helpers:** Extension methods (SetError, SetWarning, SetSuccess) to easily add error or warning information to your spans.
- **Logging Helpers:** The Log method to attach custom key/value pairs to spans.
- **Automatic Exception Wrapping:** Methods like WrapAction and Wrap<T> to automatically capture exceptions and log them on the span.

## Installation

Instana.SpanHelper depends on the Instana.ManagedTracing.Sdk package, which is included as a dependency.

To install via the NuGet Package Manager Console:

    Install-Package Instana.SpanHelper -Version 1.0.0

Or using the .NET CLI:

    dotnet add package Instana.SpanHelper --version 1.0.0

## Usage

Include the namespace in your project:

    using Instana.SpanHelper;

### Creating Spans

#### Starting a New Trace (Entry Span)

Use the StartEntrySpan method to create a new trace (root span) for an operation. This is typically used at the top level of an operation to mark the start of a trace.

    using (var span = SpanHelper.StartEntry(this, "ProcessOrder"))
    {
        // Your operation code here...
        span.SetLog("orderId", "12345");
        // Perform operations...
    }

#### Creating an Intermediate Span

When your operation includes several internal steps, use StartIntermediateSpan to create a span within the existing trace. This helps in tracking the progress of sub-operations.

    using (var span = SpanHelper.StartIntermediate(this, "ValidateOrder"))
    {
        // Code to validate the order
        span.SetLog("validationStatus", "passed");
    }

#### Creating an Exit Span

For operations that involve external calls (such as API requests), create an exit span using StartExitSpan. This differentiates external interactions from internal processing.

    using (var span = SpanHelper.StartExit(this, "SendOrderConfirmation"))
    {
        // Call to external service, e.g., send an email or API request
        span.SetLog("endpoint", "https://api.example.com/confirm");
        // External call code...
    }

### Error Handling & Logging

**SetError:**
Automatically attach error details (message and stack trace) to the span when an exception occurs.

    try
    {
        // Code that may throw an exception
    }
    catch (Exception ex)
    {
        span.SetError(ex, "Error processing order");
        throw; // Optionally rethrow the exception
    }

**WrapAction / Wrap<T>:**
These helper methods wrap your action and automatically log exceptions on the span.

    // For actions that do not return a value:
    span.WrapAction(() =>
    {
        // Code that might throw an exception
    });

    // For functions that return a value:
    var result = span.Wrap(() =>
    {
        // Code that returns a value
        return someResult;
    });

## Contributing

Contributions to Instana.SpanHelper are welcome!  
If you find issues, have suggestions, or want to contribute new features, please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.

## Repository

The source code and further documentation are available on GitHub:  
https://github.com/frknlkn/Instana.SpanHelper

Note:
This package bundles your custom SpanHelper functionality with the underlying Instana.ManagedTracing.Sdk library. When you install Instana.SpanHelper in your project, it automatically brings in the required Instana tracing library as well.

Happy tracing!
