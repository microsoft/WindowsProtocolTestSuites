# Contributing to Windows Protocol Test Suites
There are many ways to contribute to Windows Protocol Test Suites.

* Report bugs and help verify fixes when they are checked in.
* Submit updates and improvements to the documentation.
* Contribute bug fixes.
* Add new features. But firstly you should log an issue to notify the team before you spend a lot of time on it.

## CLA
Contributors must sign a [Contribution License Agreement (CLA)](https://cla.microsoft.com/) before any pull requests will be considered. 
This is a one time job. Once you have signed a CLA for any project sponsored by Microsoft, you are good to go for all the repos sponsored by Microsoft.

## Coding Style
The basic rule is following the coding style of the existing code. 

## Test
Every time you make changes to Protocol SDK, you must run **buildall.cmd** to make sure the change you made will not effect other test suites.

Every time you make changes to a Test Suite, you must run all the impacted cases of the test suite and make sure they can all pass and be compitable with Windows.

## Documentation
If a new test case is added to a test suite, the corresponding test design spec must be updated.

If a new configuration is added to a test suite, the corresponding user guide must be updated.

