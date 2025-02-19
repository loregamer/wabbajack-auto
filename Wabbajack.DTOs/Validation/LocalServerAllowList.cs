using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Wabbajack.DTOs.Validation;

public class LocalServerAllowList
{
    private static readonly string LocalWhitelistPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerWhitelist.yml");
    
    public static ServerAllowList LoadAndMerge(ServerAllowList remoteList)
    {
        if (!File.Exists(LocalWhitelistPath))
            return remoteList;

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        var localList = deserializer.Deserialize<ServerAllowList>(File.ReadAllText(LocalWhitelistPath));

        return new ServerAllowList
        {
            AllowedPrefixes = remoteList.AllowedPrefixes.Concat(localList.AllowedPrefixes).Distinct().ToArray(),
            GoogleIDs = remoteList.GoogleIDs.Concat(localList.GoogleIDs).Distinct().ToArray()
        };
    }
} 