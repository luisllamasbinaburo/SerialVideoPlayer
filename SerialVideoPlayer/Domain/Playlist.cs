using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleVideoPlayer.Domain
{
    public class Playlist
    {
        public List<PlaylistItem> Items { get; private set; } = new List<PlaylistItem>();
        private Dictionary<string, PlaylistItem> _itemsDictionary { get; set; } = new Dictionary<string, PlaylistItem>();

        public Playlist()
        {
        }

        public Playlist(PlaylistItem[] items)
        {
            _itemsDictionary = items.ToDictionary(x => x.Command);
            Items.AddRange(items);
        }

        public PlaylistItem GetByCommand(string command)
        {
            return _itemsDictionary.ContainsKey(command) ? _itemsDictionary[command] : null;
        }

        public void AddItem(string command, string url)
        {
            var newItem = new PlaylistItem() { Command = command, URL = url };
            _itemsDictionary.Add(command, newItem);
            Items.Add(newItem);
        }
    }
}
