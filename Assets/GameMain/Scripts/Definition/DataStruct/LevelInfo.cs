using System.Collections.Generic;

namespace InterCity
{
    public class LevelInfo
    {
        public int Id
        {
            get;
            set;
        }

        public List<LevelElement> ElementList
        {
            get;
            set;
        }
    }

    public class LevelElement
    {
        public int EntityId
        {
            get;
            set;
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }
    }
}