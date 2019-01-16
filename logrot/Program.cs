using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace logrot
{
    class Program
    {
        static readonly TextWriter Logger = Console.Out;

        static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        static IEnumerable<(int number, string filepath)> Enumerate(RotationInfo ri)
        {
            string number;

            foreach (var filepath in Directory.EnumerateFiles(ri.Directory, $"{ri.Name}.*.{ri.Extension}"))
            {
                //
                // On Windows, Directory.EnumerateFiles returns all paths in lower case
                // regardless of their actual case.
                //
                number = filepath.ToLower()
                                 .Replace(ri.Directory.ToLower() + Path.DirectorySeparatorChar, "")
                                 .Replace(ri.Name.ToLower() + '.', "")
                                 .Replace('.' + ri.Extension.ToLower(), "");

                yield return (int.Parse(number), filepath);
            }
        }

        static void Execute(Parameters parameters)
        {
            string line;
            StreamWriter output = null;

            var buffer = new StringBuilder();
            var logstart = parameters.LogStart;
            var rotation = GetRotationInfo(parameters);

            Logger.Info("Log rotation started.");

            Logger.Debug($"Input Encoding: {Console.InputEncoding.EncodingName}");
            Logger.Debug($"Full Destination Path: {rotation.FullPath}");
            Logger.Debug($"Destination Directory: {rotation.Directory}");
            Logger.Debug($"Log File Name: {rotation.Name}");
            Logger.Debug($"Log File Extension: {rotation.Extension}");
            Logger.Debug($"Log entries start with: '{logstart.ToString().Substring(1)}'");
            Logger.Debug($"Maximum File Size (bytes): {parameters.FileSize}");

            try
            {
                output = Open(parameters);

                while ((line = Console.In.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    if (output.BaseStream.Length > parameters.FileSize)
                    {
                        output.Dispose();

                        Rotate(parameters, rotation);

                        output = Open(parameters);
                    }

                    if (logstart.IsMatch(line))
                    {
                        output.Write(buffer);

                        output.Flush();

                        buffer.Clear();
                    }

                    buffer.Append(line + Environment.NewLine);
                }

                output.Write(buffer);
            }
            finally
            {
                output?.Dispose();
            }
        }

        static RotationInfo GetRotationInfo(Parameters parameters)
        {
            var fqpath = Path.GetFullPath(parameters.Destination);

            return new RotationInfo
            {
                Directory = Path.GetDirectoryName(fqpath),
                Extension = Path.GetExtension(fqpath).TrimStart('.'),
                File = Path.GetFileName(fqpath),
                FullPath = fqpath,
                Name = Path.GetFileNameWithoutExtension(fqpath)
            };
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Parameters>(args).WithParsed(Execute);
        }

        static StreamWriter Open(Parameters parameters, Encoding encoding = null)
        {
            return new StreamWriter(
                Path.GetFullPath(parameters.Destination),
                true,
                encoding ?? Console.InputEncoding,
                (int)parameters.BufferSize
                );
        }

        static void Rotate(Parameters parameters, RotationInfo ri)
        {
            string destination;

            foreach (var (number, filepath) in Enumerate(ri).OrderByDescending(x => x.number))
            {
                if (number == parameters.FileCount)
                {
                    Logger.Info($"Deleting: '{filepath}'");

                    File.Delete(filepath);

                    Logger.Info("Deleted");
                }
                else
                {
                    destination = $"{ri.Directory}{Path.DirectorySeparatorChar}{ri.Name}.{number + 1}.{ri.Extension}";

                    Logger.Info($"Moving: '{filepath}' => '{destination}'");

                    File.Move(filepath, destination);

                    Logger.Info("Moved");
                }
            }

            destination = $"{ri.Directory}{Path.DirectorySeparatorChar}{ri.Name}.2.{ri.Extension}";

            Logger.Info($"Moving: '{ri.FullPath}' => '{destination}'");

            File.Move(ri.FullPath, destination);

            Logger.Info("Moved");
        }
    }
}