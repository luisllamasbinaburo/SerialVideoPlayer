using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public class PlaylistFactoryJSON : IPlaylistFactory
    {

        public string JsonPath {get;set;}

        public Playlist CreatePlaylist()
        {
            PlaylistItem[] playlistItem;
            using (StreamReader jsonStream = File.OpenText(JsonPath))
            {
                var json = jsonStream.ReadToEnd();
                playlistItem = JsonConvert.DeserializeObject<PlaylistItem[]>(json);
            }
            return new Playlist(playlistItem);
        }
    }
}
