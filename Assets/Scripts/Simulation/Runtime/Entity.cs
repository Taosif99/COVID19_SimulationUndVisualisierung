using System;

namespace Simulation.Runtime
{
    public abstract class Entity //Changed to public 
    {
        protected Entity(Edit.Entity editorEntity)
        {
            EditorEntity = editorEntity;
        }

        public Edit.Entity EditorEntity { get; }

        public T GetEditorEntity<T>()
            where T : Edit.Entity
        {
            return EditorEntity as T ?? throw new InvalidCastException(
                $"Editor Entity is of type {EditorEntity.GetType().FullName}. Can't retrieve as {typeof(T).FullName}."
            );
        }
    }
}
