using System;
using System.Diagnostics;
using System.IO;

namespace logrot
{
    static class TextWriterExtensions
    {
        static readonly int ProcessId = Process.GetCurrentProcess().Id;

        internal static void Debug(this TextWriter writer, string message)
        {
            writer.Log("DEBUG", message);
        }

        internal static void Error(this TextWriter writer, string message, int? exitcode = null)
        {
            writer.Log("ERROR", message);

            if (exitcode.HasValue)
            {
                Environment.Exit(exitcode.Value);
            }
        }

        internal static void Info(this TextWriter writer, string message)
        {
            writer.Log("INFO", message);
        }

        internal static void Log(this TextWriter writer, string level, string message)
        {
            writer.Write(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"));
            writer.Write(' ');
            writer.Write(level.ToUpper());
            writer.Write(" [");
            writer.Write(ProcessId);
            writer.Write("] ");
            writer.WriteLine(message);
        }

        internal static void Usage(this TextWriter writer)
        {
            writer.WriteLine();
            writer.WriteColoredLine("Description", ConsoleColor.White);
            writer.WriteColoredLine("  Rotate log entries from STDIN among a fixed-length collection of size-capped files.", ConsoleColor.Gray);
            writer.WriteLine();
            writer.WriteColoredLine("Usage", ConsoleColor.White);
            writer.WriteColored("  logrot", ConsoleColor.Yellow);
            writer.WriteColoredLine(" <destination> [<options>]", ConsoleColor.Gray);
            writer.WriteLine();
            writer.WriteColoredLine("Required Arguments", ConsoleColor.White);
            writer.WriteColoredLine("  destination", ConsoleColor.Yellow);
            writer.WriteColoredLine("    A path to a log file that may or may not exist.", ConsoleColor.Gray);
            writer.WriteLine();
            writer.WriteColoredLine("Options", ConsoleColor.White);
            writer.WriteColoredLine("  -b, --buffer-size", ConsoleColor.Yellow);
            writer.WriteColoredLine("    The size, in bytes, of the buffer to use when writing to an output stream.", ConsoleColor.Gray);
            writer.WriteColoredLine("  -c, --count", ConsoleColor.Yellow);
            writer.WriteColoredLine("   The maximum number of files into which logs will be rotated.", ConsoleColor.Gray);
            writer.WriteColoredLine("  -p, --log-start-pattern", ConsoleColor.Yellow);
            writer.WriteColoredLine("    A regular expression to match the start of a new log entry.", ConsoleColor.Gray);
            writer.WriteColoredLine("    This expression will be automatically prefixed with '^'.", ConsoleColor.Gray);
            writer.WriteColoredLine("    Default value is '[0-9]{4}-[0-9]{2}-[0-9]{2}'.", ConsoleColor.Gray);
            writer.WriteColoredLine("  -s, --size", ConsoleColor.Yellow);
            writer.WriteColoredLine("    The maximum size, in bytes, that a log file in rotation can reach.", ConsoleColor.Gray);
        }

        internal static void Warn(this TextWriter writer, string message)
        {
            writer.Log("WARNING", message);
        }

        internal static void WriteColored(this TextWriter writer, string value, ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;

            writer.Write(value);

            Console.ResetColor();
        }

        internal static void WriteColoredLine(this TextWriter writer, string value, ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;

            writer.WriteLine(value);

            Console.ResetColor();
        }
    }
}