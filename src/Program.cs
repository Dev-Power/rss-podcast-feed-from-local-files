using System.Xml.Serialization;
using RssFeedGenerator;

string serverIPAddress = "192.168.1.20";
int serverPort = 9876;
var contentFullPath = "/Temp/webroot/content";
var feedFullPath = "/Temp/webroot/feed.rss";
var audioRootUrl = $"http://{serverIPAddress}:{serverPort}";

var rss = new Rss
{
    Version = "2.0",
    Channel = new Channel
    {
        Title = "[Local] Test Podcast",
        Description = "Testing generating RSS feed from local MP3 files",
        Category = "test",
        Item = new List<Item>()
    }
};

var allMp3s = new DirectoryInfo(contentFullPath)
    .GetFiles("*.mp3", SearchOption.AllDirectories)
    .OrderBy(x => x.Name);

foreach (var mp3 in allMp3s)
{
    rss.Channel.Item.Add(new Item
    {
        Description = mp3.Name,
        Title = mp3.Name,
        Enclosure = new Enclosure
        {
            Url = $"{audioRootUrl}/{mp3.Directory.Name}/{mp3.Name}",
            Type = "audio/mpeg"
        }
    });
}

var serializer = new XmlSerializer(typeof(Rss));
using (var writer = new StreamWriter(feedFullPath))
{
    serializer.Serialize(writer, rss);
}