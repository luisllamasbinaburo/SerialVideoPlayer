using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public interface IPlaylistFactory
    {
        Playlist CreatePlaylist();
    }
}
