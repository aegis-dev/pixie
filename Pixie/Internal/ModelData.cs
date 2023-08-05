namespace Pixie.Internal
{
    internal struct ModelData
    {
        public readonly VertexData[] VertexData;
        public readonly int[] Indices;

        public ModelData() {
            VertexData = new VertexData[0];
            Indices = new int[0];
        }

        public ModelData(VertexData[] vertexData, int[] indices)
        {
            VertexData = vertexData;
            Indices = indices;
        }
    }
}
