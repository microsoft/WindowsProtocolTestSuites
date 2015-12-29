# Contributing to Windows Protocol Test Suites
There are many ways to contribute to Windows Protocol Test Suites.

* Report [issues](https://github.com/Microsoft/windowsProtocolTestSuites/issues) and help verify fixes when they are checked in.
* [Fork](https://github.com/Microsoft/windowsProtocolTestSuites#fork-destination-box) to your local and send us pull request to:
	* Submit updates and improvements to the documentation.
	* Contribute bug fixes.
	* Add new features. But firstly you should log an issue to notify the team before you spend a lot of time on it.

## CLA
Contributors must sign a [Contribution License Agreement (CLA)](https://cla.microsoft.com/) before any pull requests will be considered. 
This is a one time job. Once you have signed a CLA for any project sponsored by Microsoft, you are good to go for all the repos sponsored by Microsoft.

## Pull Request
Before you send out the pull request, you must:

* Run **buildall.cmd** to make sure the change you made will not affect other test suites if you make changes to Protocol SDK.
* Run all the impacted cases of the test suite and make sure they can all pass and be compatible with Windows if you make changes to a Test Suite.

Your pull request should:

* Include a description of what your change intends to do. Have clear commit messages, e.g. "Add feature", "Fix issue", "Add test cases for protocol"
* Follow the code conventions of the existing code.
* Include the update to the specific document if you add new cases or new configurations.
	* If a new test case is added to a test suite, the corresponding test design specification must be updated.
	* If a new configuration is added to a test suite, the corresponding user guide must be updated.

After we receive the pull request, we will do code review and give you feedback.

