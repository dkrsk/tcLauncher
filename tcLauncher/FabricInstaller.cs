using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

using CmlLib.Core;
using CmlLib.Core.Version;
using CmlLib.Core.Installer.FabricMC;


namespace DnKR
{
    namespace tcLauncher
    {
        public class FabricInstaller
        {
            public FabricInstaller(MinecraftPath path, string java)
            {
                //this.javapath = java;
                //this.path = path;
                //logQueue = new ConcurrentQueue<string>();

            }

            //MinecraftPath path;
            //string javapath;
            //ConcurrentQueue<string> logQueue;

            public static async Task<MVersion> installFabric(MinecraftPath path)
            {


                var fabricLoader = new FabricVersionLoader();
                //FabricLoader fabricVersion = fabricLoader.GetFabricLoaders()[0];
                var fabric = fabricLoader.GetVersionMetadatas();
                var f = fabric.GetVersionMetadata(fabric[0].Name);
                await f.SaveAsync(path);

                //await fabric.SaveAsync(path);
                return fabric[0].GetVersion();
            }

        }
    }
}