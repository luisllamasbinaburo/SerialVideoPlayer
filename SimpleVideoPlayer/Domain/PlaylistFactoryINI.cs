using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public class PlaylistFactoryINI : IPlaylistFactory
    {
        public string IniPath { get; set; }

        public Playlist CreatePlaylist()
        {
            var lines = System.IO.File.ReadAllLines(IniPath);

            var playlist = new Playlist();
            for (int i = 0; i < lines.Length; i++)
            {
                playlist.AddItem(((char)(i + 'A')).ToString(), lines[i]);
            }

            return playlist;
        }

    }
}
