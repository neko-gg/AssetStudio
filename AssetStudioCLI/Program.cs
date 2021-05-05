using System.Collections.Generic;
using System.Linq;
using AssetStudio;
using AssetStudioGUI;
using CommandLine;
using Object = AssetStudio.Object;

namespace AssetStudioCLI
{
    internal class Options
    {
        [Value(0, MetaName = "input", Required = true, HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Value(1, MetaName = "output", Required = false, Default = "./output", HelpText = "Output directory.")]
        public string OutputDir { get; set; }
    }

    internal static class Program
    {
        private static readonly AssetsManager AssetsManager = new AssetsManager();

        private static void Main(string[] args)
        {
            Parser.Default
                  .ParseArguments<Options>(args)
                  .WithParsed(Extract);
        }

        private static void Extract(Options options)
        {
            AssetsManager.LoadFiles(options.InputFile);
            Studio.DoExportAssets(options.OutputDir, GetExportableAssets(), ExportType.Convert);
        }

        private static List<AssetItem> GetExportableAssets()
        {
            var objectCount = AssetsManager.assetsFileList.Sum(x => x.Objects.Count);
            var objectAssetItemDic = new Dictionary<Object, AssetItem>(objectCount);
            var containers = new List<(PPtr<Object>, string)>();
            string productName = null;
            return Studio.GetExportableAssets(AssetsManager, objectAssetItemDic, containers, ref productName);
        }
    }
}
