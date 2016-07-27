namespace AmazonHelper.WebApp.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Combres;
    using dotless.Core;
    using dotless.Core.Exceptions;
    using dotless.Core.Importers;
    using dotless.Core.Input;
    using dotless.Core.Loggers;
    using dotless.Core.Parser;
    using dotless.Core.Parser.Infrastructure;
    using dotless.Core.Plugins;
    using dotless.Core.Stylizers;

    public class DotLessCssFilter : ISingleContentFilter, ICacheVaryStateReceiver
    {
        public IList<CacheVaryState> CacheVaryStates { get; set; }

        public string TransformContent(ResourceSet resourceSet, Resource resource, string content)
        {
            try
            {
                if (resource.Path.EndsWith(".css"))
                {
                    return content;
                }

                var reader = new AdeptFileReader(resource.Path);
                var importer = new Importer(reader);
                var parser = new Parser(new ConsoleStylizer(), importer);
                var engine = new LessEngine(parser, new DotLessLogger(LogLevel.Error), resourceSet.CompressionEnabled, resourceSet.DebugEnabled);

                if (resourceSet.Name.Contains(".theme"))
                {
                    var color = CacheVaryStates.SelectMany(vs => vs.Values).Where(v => v.Key == "userColors").ToList();
                    if (color.Any())
                    {
                        return engine.TransformToCssExtended(color[0].Value + content, resource.Path);
                    }
                }

                return engine.TransformToCssExtended(content, resource.Path);
            }
            catch (Exception e)
            {
                new DotLessLogger(LogLevel.Error).Error(e.ToString());
                return string.Format("/*LESS ERROR: {0}*/", (e.InnerException ?? e).Message);
            }

        }

        public bool CanApplyTo(ResourceType resourceType)
        {
            return resourceType == ResourceType.CSS;
        }

        public class AdeptFileReader : IFileReader
        {
            private readonly string _startingDirectory;

            public AdeptFileReader(string original)
            {
                _startingDirectory = original.Substring(0, original.LastIndexOf('/') + 1);
            }

            public bool DoesFileExist(string fileName)
            {
                return File.Exists(GetFilePath(fileName));
            }

            public string GetFileContents(string fileName)
            {
                return File.ReadAllText(GetFilePath(fileName));
            }

            public string GetFilePath(string fileName)
            {
                if (fileName.StartsWith("~") || fileName.StartsWith("/"))
                    return HttpContext.Current.Server.MapPath(fileName);
                return HttpContext.Current.Server.MapPath(_startingDirectory + "/" + fileName);
            }

            public byte[] GetBinaryFileContents(string fileName)
            {
                return File.ReadAllBytes(GetFilePath(fileName));
            }


            public bool UseCacheDependencies
            {
                get { throw new NotImplementedException(); }
            }
        }

        public class DotLessLogger : Logger
        {
            public DotLessLogger(LogLevel level)
                : base(level)
            {
            }

            protected override void Log(string message)
            {
                System.Diagnostics.Debug.WriteLine("LESS:");
                System.Diagnostics.Debug.WriteLine(message);
            }
        }
    }

    public static class LessEngineExtensions
    {
        public static string TransformToCssExtended(this LessEngine engine, string source, string fileName)
        {
            try
            {
                var tree = engine.Parser.Parse(source, fileName);

                var env = engine.Env ?? new Env { Compress = engine.Compress, Debug = engine.Debug, KeepFirstSpecialComment = engine.KeepFirstSpecialComment, DisableVariableRedefines = engine.DisableVariableRedefines };

                if (engine.Plugins != null)
                {
                    foreach (IPluginConfigurator configurator in engine.Plugins)
                    {
                        env.AddPlugin(configurator.CreatePlugin());
                    }
                }

                var css = tree.ToCSS(env);

                return css;
            }
            catch (ParserException e)
            {
                return string.Format("/*LESS ERROR: {0}*/", (e.InnerException ?? e).Message);
            }
            catch (Exception e)
            {
                return string.Format("/*{0}*/", e.Message);
            }
        }
    }
}
