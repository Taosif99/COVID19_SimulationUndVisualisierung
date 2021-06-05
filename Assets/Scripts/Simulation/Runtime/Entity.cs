namespace Simulation.Runtime
{
    public abstract class Entity //Changed to public 
    {
        protected Entity(Edit.Entity editorEntity)
        {
            EditorEntity = editorEntity;
        }

        public Edit.Entity EditorEntity { get; }
    }
}
