using System;
using System.Threading;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AMFormsCST.Test.Helpers;

/// <summary>
/// A custom Xunit Fact attribute that runs tests on an STA thread.
/// This is necessary for testing code that instantiates WPF UI controls.
/// </summary>
[XunitTestCaseDiscoverer("AMFormsCST.Test.Helpers.WpfFactDiscoverer", "AMFormsCST.Test")]
public class WpfFactAttribute : FactAttribute
{
    public override string? Skip
    {
        get => base.Skip;
        set
        {
            if (value != null && Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                base.Skip = "This test requires an STA thread.";
            }
            else
            {
                base.Skip = value;
            }
        }
    }
}

public class WpfFactDiscoverer : FactDiscoverer
{
    public WpfFactDiscoverer(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, Xunit.Abstractions.ITestMethod testMethod, IAttributeInfo factAttribute)
    {
        return new WpfTestCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod);
    }
}

public class WpfTestCase : XunitTestCase
{
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public WpfTestCase() { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1013:Public method should be marked as test", Justification = "Used by the Xunit framework.")]
    public WpfTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, TestMethodDisplayOptions defaultMethodDisplayOptions, Xunit.Abstractions.ITestMethod testMethod, object[]? testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
    }

    public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
    {
        var tcs = new TaskCompletionSource<RunSummary>();
        var thread = new Thread(() =>
        {
            try
            {
                var summary = base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource).Result;
                tcs.SetResult(summary);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join(); // Wait for the thread to complete
        return tcs.Task;
    }
}