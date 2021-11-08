using System.Collections.Generic;

namespace NSU.Worm
{
    public interface INameGenerator
    {
        public string NextName();
        
        public List<string> NamePool { get; set; }
    }
}