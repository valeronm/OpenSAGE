﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenSage.Data.Map;

namespace OpenSage.Scripting
{
    public sealed class ScriptList : Asset
    {
        public const string AssetName = "ScriptList";

        public ScriptGroup[] ScriptGroups { get; private set; } = Array.Empty<ScriptGroup>();
        public Script[] Scripts { get; private set; } = Array.Empty<Script>();

        public void Execute(ScriptExecutionContext context)
        {
            foreach (var scriptGroup in ScriptGroups)
            {
                scriptGroup.Execute(context);
            }

            foreach (var script in Scripts)
            {
                script.Execute(context);
            }
        }

        public Script FindScript(string name)
        {
            // TODO: Use a dictionary.

            foreach (var script in Scripts)
            {
                if (script.Name == name)
                {
                    return script;
                }
            }

            foreach (var scriptGroup in ScriptGroups)
            {
                var result = scriptGroup.FindScript(name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        internal static ScriptList Parse(BinaryReader reader, MapParseContext context)
        {
            return ParseAsset(reader, context, version =>
            {
                if (version != 1)
                {
                    throw new InvalidDataException();
                }

                var scriptGroups = new List<ScriptGroup>();
                var scripts = new List<Script>();

                ParseAssets(reader, context, assetName =>
                {
                    switch (assetName)
                    {
                        case ScriptGroup.AssetName:
                            scriptGroups.Add(ScriptGroup.Parse(reader, context));
                            break;

                        case Script.AssetName:
                            scripts.Add(Script.Parse(reader, context));
                            break;

                        default:
                            throw new InvalidDataException($"Unexpected asset: {assetName}");
                    }
                });
                
                return new ScriptList
                {
                    ScriptGroups = scriptGroups.ToArray(),
                    Scripts = scripts.ToArray()
                };
            });
        }

        internal void WriteTo(BinaryWriter writer, AssetNameCollection assetNames)
        {
            WriteAssetTo(writer, () =>
            {
                foreach (var script in Scripts)
                {
                    writer.Write(assetNames.GetOrCreateAssetIndex(Script.AssetName));
                    script.WriteTo(writer, assetNames);
                }

                foreach (var scriptGroup in ScriptGroups)
                {
                    writer.Write(assetNames.GetOrCreateAssetIndex(ScriptGroup.AssetName));
                    scriptGroup.WriteTo(writer, assetNames);
                }
            });
        }

        internal void Load(StatePersister reader)
        {
            reader.ReadVersion(1);

            var numScripts = (ushort) Scripts.Length;
            reader.ReadUInt16(ref numScripts);

            if (numScripts != Scripts.Length)
            {
                throw new InvalidStateException();
            }

            for (var i = 0; i < numScripts; i++)
            {
                Scripts[i].Load(reader);
            }

            var numScriptGroups = (ushort) ScriptGroups.Length;
            reader.ReadUInt16(ref numScriptGroups);

            if (numScriptGroups != ScriptGroups.Length)
            {
                throw new InvalidStateException();
            }

            for (var i = 0; i < numScriptGroups; i++)
            {
                ScriptGroups[i].Load(reader);
            }
        }

        internal ScriptList Copy(string appendix)
        {
            return new ScriptList()
            {
                Scripts = Scripts.Select(s => s.Copy(appendix)).ToArray(),
                ScriptGroups = ScriptGroups.Select(s => s.Copy(appendix)).ToArray()
                // TODO: how to set Version correctly?
            };
        }
    }
}
