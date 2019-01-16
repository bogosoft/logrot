using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace logrot
{
    public class Parameters
    {
        [Option('b', "buffer-size", Default = 1024U, HelpText = Help.BufferSize)]
        public uint BufferSize { get; set; }

        [Value(0, HelpText = Help.Destination)]
        public string Destination { get; set; }

        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Rotate log files every 4096 bytes", new Parameters
                {
                    Destination = "/var/log/my-app/my-app.log",
                    FileSize = 4096
                });
            }
        }

        [Option('c', "count", Default = (byte)10, HelpText = Help.FileCount)]
        public byte FileCount { get; set; }

        [Option('s', "size", Default = 1024000U, HelpText = Help.FileSize)]
        public uint FileSize { get; set; }

        public Regex LogStart => new Regex('^' + LogStartPattern, RegexOptions.Compiled);

        [Option('p', "log-start-pattern", Default = Default.LogStartPattern, HelpText = Help.LogStartPattern)]
        public string LogStartPattern { get; set; }
    }
}