# Adding Test Tracing with OpenTelemetry

This is an example library that shows how to use OpenTelemetry as a mechanism to monitor your unit test performance and also see things like casuality in your tests.

## TopLevelTests

This is an example of just caring about the TopLevel of "These are all the tests, and their timings". This is useful as a starting block

![TopLevelTests Screenshot](/assets/TopLevelTest_screenshot.png?raw=true "Top Level Tests Screenshot")

## TestsWithCausality

This is an example of creating trace per test so that you can monitor the tests better where they have lots of spans.

In this example you'll see that you can also monitor Activities created in your application code. This has no impact on the code when OpenTelemetry is not applied, therefore you're safe to add the activities in your code, purely for use in your tests.