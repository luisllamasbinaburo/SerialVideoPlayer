using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public class PlaylistFactoryCSV : IPlaylistFactory
    {

        public string CsvPath {get;set;}

        public Playlist CreatePlaylist()
        {
            var csv = File.ReadAllLines(CsvPath);

            var playlistItem = csv.Select(x => new PlaylistItem()
            {
                Command = x.Split(',')[0],
                URL = x.Split(',')[1],
            }).ToArray();
            return new Playlist(playlistItem);
        }
    }
}
