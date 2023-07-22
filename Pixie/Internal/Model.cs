﻿namespace Pixie.Internal
{
    public class Model
    {
        public uint VaoID { get; }
        public ModelData ModelData { get; }

        public Model(uint vaoID, ModelData modelData)
        {
            VaoID = vaoID;
            ModelData = modelData;
        }
    }
}
