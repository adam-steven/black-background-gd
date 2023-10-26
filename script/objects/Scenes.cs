using System.Collections.Generic;

public partial class Scenes : List<string> {
        public Scenes(IEnumerable<string> collection) : base(collection) {}
        public Scenes(List<string> collection) : base(collection) {}
        public Scenes(Godot.Collections.Array<string> collection) : base(collection) { }
        public Scenes() {}
 }