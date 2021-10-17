using System;

namespace InterCity
{
    [Serializable]
    public class InteractionPointData : EntityData
    {
        public InteractionPointData(int entityId, int typeId) : base(entityId, typeId)
        {
        }
    }
}