namespace logrot
{
    static class Help
    {
        internal const string BufferSize = "The size, in bytes, of the buffer to use when writing to an output stream.";
        internal const string Destination = "A path to a log file that may or may not exist.";
        internal const string FileCount = "The maximum number of files into which logs will be rotated.";
        internal const string FileSize = "The maximum size, in bytes, that a log file in rotation can reach.";
        internal const string LogStartPattern = "A regular expression to match the start of a new log entry. This expression will be automatically prefixed with '^'.";

    }
}