using System.Text.Json.Serialization;

namespace AMFormsCST.Desktop.Models;

public class GitHubGist
{
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("public")]
    public bool Public { get; set; } = true;

    [JsonPropertyName("files")]
    public Dictionary<string, GistFile> Files { get; set; } = [];
}

public class GistFile
{
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}