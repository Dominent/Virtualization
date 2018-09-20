namespace KurtBit.Virtualization.Server
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http.Dispatcher;
    using System.IO.Compression;

    public class CommandAsemblyResolver : IAssembliesResolver
    {
        private List<string> documentationRegistry;

        public CommandAsemblyResolver(List<string> documentationRegistry)
        {
            this.documentationRegistry = documentationRegistry;
        }

        public ICollection<Assembly> GetAssemblies()
        {
            const string COMMAND_PREFIX = "KurtBit.Virtualization.Commands.";
            const string COMMAND_EXTENSION = ".dll";
            const string COMMAND_ARCHIVE_EXTENSION = ".zip";
            const string COMMAND_DOCUMENTATION_EXTENSION = ".xml";
            const string COMMANDS_BASE_DIRECTORY = "Commands";

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToList();

            var commandsDirectory = Path.Combine(Directory.GetCurrentDirectory(), COMMANDS_BASE_DIRECTORY);

            var commands = Directory.EnumerateFiles(commandsDirectory)
                .Where((path) =>
                {
                    var name = Path.GetFileName(path);

                    return name.StartsWith(COMMAND_PREFIX) && name.EndsWith(COMMAND_ARCHIVE_EXTENSION);
                })
                .ToList();

            foreach (var command in commands)
            {
                var assemblyDirectory = command.Replace(COMMAND_ARCHIVE_EXTENSION, String.Empty);
                var assemblyName = Path.GetFileName(command).Replace(COMMAND_ARCHIVE_EXTENSION, COMMAND_EXTENSION);
                var assemblyPath = Path.Combine(assemblyDirectory, assemblyName);

                this.documentationRegistry.Add(
                    Path.ChangeExtension(assemblyPath, COMMAND_DOCUMENTATION_EXTENSION));

                if (!Directory.Exists(assemblyDirectory))
                {
                    ZipFile.ExtractToDirectory(command, Path.GetDirectoryName(command));
                }

                var assembly = Assembly.LoadFrom(assemblyPath);

                assemblies.Add(assembly);
            }

            return assemblies;
        }
    }
}
