# LogRotator

*Documentation, like this utility, is still a work in progress*.

This repository contains the code for a log-rotating, command line utility targetting .NET Core 2.

Specifically, this utility maintains a rotating, fixed-length collection of size-capped log files by reading log entries from STDIN and pushing them onto the collection.

## Features

* Fixed-length collection of log files. Length is customizable.
* Log files are size-capped. The cap is customizable.
* Detects the start of a log using a customizable regular expression. This is necessary to ensure that a log file is not rotated midway through a log entry.

## Invocation

`my_log_generating_applicaiton | logrot [-b <int>, -c <int>, -p <regex>, -s <int>, --help, --version] /path/to/output.log`

## Examples

*Windows*: `ping -t example.com | logrot -c 5 -s 128 -p "*" D:\Logs\ping.log`

### Arguments & Options

#### Destination File Path

The first non-optional argument to the utility is the destination file path. This is a path to a *file*, whether or not it currently exists. If the file does not exist, it is created. If it does exist, it will immediately receive logs and enter the rotation.

#### --buffer-size, -b

*Default: **1024***.

The size of the buffer to use, in bytes, when reading and writing stream data.

#### --count, -c

*Default: **10***

The maximum number of log files to keep in the rotation.

#### --log-start-pattern, -p

*Default: **[0-9]{4}-[0-9]{2}-[0-9]{2}***

A regular expression that matches against the start of a new log entry.

#### --size, -s

*Default: **1024000***

The size, in bytes, at which each log file in the rotation will be capped at.

#### --help

Explains the utility's usage and arguments.

#### --version

Displays the current version of the utility.

