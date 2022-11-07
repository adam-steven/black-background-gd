using System.Collections.Generic;

public class Scenes : List<string> {
        public Scenes(IEnumerable<string> collection) : base(collection) {}
        public Scenes(List<string> collection) : base(collection) {}
        public Scenes() {}
 }