using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nxtAPIwrapper
{
    public class PeersList
    {
        public List<string> peers { get; set; }
    }

    public class Peer
    {
        public string platform { get; set; }
        public string application { get; set; }
        public double weight { get; set; }
        public string hallmark { get; set; }
        public int state { get; set; }
        public string announcedAddress { get; set; }
        public double downloadedVolume { get; set; }
        public string version { get; set; }
        public double uploadedVolume { get; set; }

        public string formattedAppInfo { get { return application + " (" + version + ") @ " + platform; } }
    }
}
